

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
            return RedirectToAction("AvailableRooms", model);
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
        public ActionResult AvailableRooms(CheckInOut model)
        {
            if (model.CheckIn.Date < DateTime.Now.Date) return View("Error");
            
                return View(HelperClass.AvailableRoomList(model));
            
        }


        [HttpGet]
        public ActionResult UserBookings()
        {
                var UserBookedRooms= HelperClass.IndividualUserBookings(User.Identity.Name);
                if (UserBookedRooms.Count == 0) ViewBag.NoBookings = "There are no record of bookings from you";

            return View(UserBookedRooms);
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


    }

}
