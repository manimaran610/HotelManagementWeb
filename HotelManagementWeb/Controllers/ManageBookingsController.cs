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
        public ActionResult Bookings(int? Id, bool FilterManagementBookings = false)
        {
            if (User.IsInRole("Admin"))
            {

                var ListOfBookings = HotelDatabaseLayer.GetBookings();
                decimal? TotalAmountReceived = new decimal();
                foreach (var item in HotelDatabaseLayer.GetBookings())
                {
                    if (Id==1 && item.BookingTo.Date < DateTime.Now.Date)  //Id==1 indicated showcurrentbooking button is clicked ,so list of bookings is filtered accordingly
                    {
                        ListOfBookings.Remove(item);
                    }
                    if (item.CustomerEmail == User.Identity.Name && !FilterManagementBookings)
                    {
                        ListOfBookings.Remove(item);
                    }
                    else { TotalAmountReceived += item.TotalAmount; }

                    item.RoomNumber = HotelDatabaseLayer.GetRooms().Find(room => room.RoomId == item.AssignRoomId).RoomNumber;
                }
                ViewBag.TotalAmountReceived = TotalAmountReceived;
                //For Management or Hotel Admin Bookings page
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
            if (HotelDatabaseLayer.GetBookings().Exists(item => item.BookingId == model.BookingId))
            {

                BookingOperations.DeleteExistingBookingInDatabase(model);
                bool FilterManagementBookings = true;
                return RedirectToAction("Bookings",new { Id = 0, FilterManagementBookings });
            }
            else
            {
                return View("Error");
            }
        }

    }
}