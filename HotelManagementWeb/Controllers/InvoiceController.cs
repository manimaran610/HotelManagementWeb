using HotelManagementWeb.Models;
using System;

using System.Linq;

using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    public class InvoiceController : Controller
    {
        
        public ActionResult BookingInvoice(int RoomId, int RoomCapacity, DateTime CheckIn, DateTime CheckOut, decimal RoomPrice,string Type)
        {
            //if (RoomId == null) return View("Error");
            var db = new ApplicationDbContext();
            var data = db.Users.Where(item => item.Email == User.Identity.Name).First();

            var model = new Booking();
            model.CustomerName = data.FullName;
            model.CustomerEmail = data.Email;
            model.AssignRoomId = RoomId;
            model.NoOfMembers = RoomCapacity;
            model.BookingFrom = CheckIn;
            model.BookingTo = CheckOut;
            model.NumberOfDays = (CheckOut - CheckIn).Days;
            model.RoomPrice = RoomPrice;
            model.MaxCapacity = RoomCapacity;
            model.MobileNo = Decimal.Round(Decimal.Parse(data.PhoneNumber)).ToString();
            model.ValueAddedTax = Decimal.Round(Decimal.Multiply((Decimal)RoomPrice, (Decimal)0.18));
            model.TotalAmount = model.RoomPrice + model.ValueAddedTax;
            model.TotalAmount = Decimal.Multiply((Decimal)model.TotalAmount, model.NumberOfDays);
            ViewBag.RoomType = Type;

            return View(model);
        }

        [HttpPost]
        public ActionResult BookingInvoice(Booking model = null)
        {
            if (model == null) return View("Error");
            if (ModelState.IsValid)
            {
                if (model.NoOfMembers > model.MaxCapacity)
                {
                    ViewData["NoOfMembers"] = "Maximum capacity is upto " + model.MaxCapacity + "  person";
                    return View(model);
                }
                using (var database = new HMSContext())
                {
                    database.Bookings.Add(model);
                    database.SaveChanges();
                }
                return PartialView("BookedScreen");
            }
            return RedirectToAction("BookingInvoice", model);
        }
       
    }
}