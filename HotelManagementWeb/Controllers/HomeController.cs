

using HotelManagementWeb.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(CheckInOut model)
        {
            if (ModelState.IsValid)
            {
                if (model.CheckIn.Date < DateTime.Now.Date || model.CheckOut.Date < DateTime.Now.Date || model.CheckOut.Date <= model.CheckIn.Date)
                {
                    if (model.CheckIn.Date < DateTime.Now.Date) ViewData["CheckInError"] = "Date must be greater than or equal to Today's Date";
                    if (model.CheckOut.Date < DateTime.Now.Date) ViewData["CheckOutError"] = "Date must be greater than Today's Date";
                    if (model.CheckOut.Date <= model.CheckIn.Date) ViewData["CheckOutError"] = "Date must be greater than CheckIn Date";
                    return View(model);
                }
            }
            return RedirectToAction("Rooms", model);
        }


        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Rooms(CheckInOut model)
        {
            if (model.CheckIn.Date < DateTime.Now.Date) return View("Error");
            using (var database = new HMSContext())
            {
                var BookedRooms = database.Bookings.ToList().FindAll(item => (model.CheckIn.Date >= item.BookingFrom.Date && model.CheckIn.Date <= item.BookingTo.Date) ||
                (model.CheckOut.Date >= item.BookingFrom.Date && model.CheckOut.Date <= item.BookingTo.Date) ||
                model.CheckIn.Date <= item.BookingFrom.Date && model.CheckOut.Date >= item.BookingTo.Date);
                List<Room> rooms = database.Rooms.ToList();
                BookedRooms.ForEach(room => rooms.RemoveAll(item => item.RoomId == room.AssignRoomId));
                foreach (var item in rooms)
                {
                    item.Type = database.RoomTypes.ToList().Find(eachItem => eachItem.RoomTypeId == item.RoomTypeId).RoomType;
                    item.CheckIn = model.CheckIn;
                    item.CheckOut = model.CheckOut;
                }
                return View(rooms);
            }
        }


        [HttpGet]
        public ActionResult UserBookings()
        {
            using (var database = new HMSContext())
            {
                var BookedList = database.Bookings.ToList().FindAll(item => item.CustomerEmail == User.Identity.Name);
                var ListOfRooms = database.Rooms.ToList();
                foreach (var item in BookedList)
                {
                    item.RoomNumber = ListOfRooms.Find(room => room.RoomId == item.AssignRoomId).RoomNumber;
                    item.QRCode = GenerateQRCode($"Name : {item.CustomerName},\nCheckIn : {item.BookingFrom.ToShortDateString()}," +
                        $"\nCheckOut : {item.BookingTo.ToShortDateString()},\nRoomNo : {item.RoomNumber}," +
                        $"\nAmount Paid:{item.TotalAmount}");
                }
                if (BookedList.Count == 0) ViewBag.NoBookings = "There are no record of bookings from you";
                return View(BookedList);


            }
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            ApplicationUser user = new ApplicationUser();
            using (var database = new ApplicationDbContext())
            {
                user = database.Users.ToList().Find(item => item.Email == User.Identity.Name);
            }
            return View(user);
        }


        //QR code Generator
        public string GenerateQRCode(string QRString)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRString, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap bitMap = qrCode.GetGraphic(20);
                bitMap.Save(ms, ImageFormat.Png);
                return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
        }

    }

}
