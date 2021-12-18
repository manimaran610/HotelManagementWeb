

using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Index(CheckInOut model)
        {
            if (ModelState.IsValid)
            {
                if (model.CheckIn.Date < DateTime.Now.Date || model.CheckOut.Date < DateTime.Now.Date ||model.CheckOut.Date <=model.CheckIn.Date)
                {
                    if(model.CheckIn.Date < DateTime.Now.Date)ViewData["CheckInError"] = "Date must be greater than or equal to Today's Date";
                    if(model.CheckOut.Date < DateTime.Now.Date)ViewData["CheckOutError"] = "Date must be greater than Today's Date";
                    if (model.CheckOut.Date <= model.CheckIn.Date) ViewData["CheckOutError"] = "Date must be greater than CheckIn Date";
                    return View(model);
                }

            }return RedirectToAction("Rooms",model);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        public ActionResult Rooms(CheckInOut model)
        {

            using (var database = new HMSContext())
            {
                var BookedRooms = database.Bookings.ToList().FindAll(item => (model.CheckIn.Date >= item.BookingFrom.Date && model.CheckIn.Date <= item.BookingTo.Date) ||
                (model.CheckOut.Date >= item.BookingFrom.Date && model.CheckOut.Date <= item.BookingTo.Date));
                List<Room> rooms = database.Rooms.ToList();
                BookedRooms.ForEach(room => rooms.RemoveAll(item => item.RoomId == room.AssignRoomId));
               

                foreach (var item in rooms)
                {
                   item.Type= database.RoomTypes.ToList().Find(eachItem => eachItem.RoomTypeId == item.RoomTypeId).RoomType;
                    item.CheckIn=model.CheckIn;
                    item.CheckOut = model.CheckOut;
                }

                return View(rooms);
            }
        }


      
        public ActionResult BookingRoom(Room room=null)

        {
            if (room.RoomId == 0) return View("Error");
            var db = new ApplicationDbContext();
           var data = db.Users.Where(item => item.Email == User.Identity.Name).First();

            var model = new Booking();
            model.CustomerName = data.FullName;
            model.CustomerEmail = data.Email;
            model.AssignRoomId = room.RoomId;
            model.NoOfMembers = room.RoomCapacity;
            model.BookingFrom =  room.CheckIn;
            model.BookingTo = room.CheckOut;
            model.NumberOfDays = (room.CheckOut - room.CheckIn).Days;
            model.RoomPrice = room.RoomPrice;
            model.MaxCapacity= room.RoomCapacity;
            model.Phone = Decimal.Round(Decimal.Parse(data.PhoneNumber));
            model.ValueAddedTax= Decimal.Round(Decimal.Multiply((Decimal)room.RoomPrice, (Decimal)0.18));
            model.TotalAmount = model.RoomPrice + model.ValueAddedTax;

            model.TotalAmount= Decimal.Multiply((Decimal)model.TotalAmount, model.NumberOfDays);
           
           return View(model);
        }

        [HttpPost]
        public ActionResult BookingRoom(Booking model)
        {
            if (ModelState.IsValid)
            {
                if (model.NoOfMembers > model.MaxCapacity)
                {
                    ViewData["NoOfMembers"] = "Maximum capacity is upto " + model.MaxCapacity + "  person";
                    return View(model);
                }
                using(var database = new HMSContext())
                {
                    database.Bookings.Add(model);
                    
                    database.SaveChanges();
                }
                return PartialView("BookedScreen");
            }
            return RedirectToAction("BookingRoom",model);
        }
        
        public ActionResult UserBookings()
        {
            using(var database=new HMSContext())
            {
               var BookedList =  database.Bookings.ToList().FindAll(item => item.CustomerEmail == User.Identity.Name);
                foreach(var item in BookedList)
                {
                    item.RoomNumber = database.Rooms.ToList().Find(room => room.RoomId == item.AssignRoomId).RoomNumber;
                }
                if (BookedList.Count == 0) ViewBag.NoBookings = "There are no record of bookings from you";
                return View(BookedList);

            }
        }
      

    }

}
