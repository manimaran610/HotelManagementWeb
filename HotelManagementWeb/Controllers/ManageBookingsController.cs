using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageBookingsController : Controller
    {
        public async Task<ActionResult> Bookings(int? Id, bool FilterManagementBookings = false)
        {
            if (User.IsInRole("Admin"))
                using (var database = new HMSContext())
                {
                    var ListOfBookings = await database.Bookings.ToListAsync();
                    decimal? TotalAmountReceived = new decimal();
                    foreach (var item in database.Bookings.ToList())
                    {
                        if (Id == 1 && item.BookingTo.Date < DateTime.Now.Date)
                        {
                            ListOfBookings.Remove(item);
                        }
                        if (item.CustomerEmail == User.Identity.Name && !FilterManagementBookings)
                        {
                            ListOfBookings.Remove(item);
                        }
                        else { TotalAmountReceived += item.TotalAmount; }

                        item.RoomNumber = database.Rooms.ToList().Find(room => room.RoomId == item.AssignRoomId).RoomNumber;
                    }
                    ViewBag.TotalAmountReceived = TotalAmountReceived;
                    //For Management Bookings page
                    if (FilterManagementBookings)
                    {
                        var ManagementBookings = ListOfBookings.FindAll(item => item.CustomerEmail == User.Identity.Name);
                        Decimal? totalamount = 0;
                        ManagementBookings.ForEach(item => totalamount += item.TotalAmount);
                        ViewBag.TotalAmountReceived = totalamount;
                        ViewBag.FilterManagementBookings = true;

                        return View(ManagementBookings);
                    }
                    return View(ListOfBookings);
                }
            return new HttpNotFoundResult();
        }


        public ActionResult CancelBookedRoom(Booking model)
        {
            using (var database = new HMSContext())
            {
                if (database.Bookings.ToList().Exists(item => item.BookingId == model.BookingId))
                {
                    var ExistingOrder = database.Bookings.Where(item => item.BookingId == model.BookingId).First();
                    database.Bookings.Remove(ExistingOrder);
                    database.SaveChanges();
                    return RedirectToAction("Bookings");
                }
                else
                {
                    return View("Error");
                }
            }
        }
    }
}