
using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{

    [Route("{action=index}")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
                using (var database = new HMSContext())
                {
                    List<Room> ListOfRooms = database.Rooms.ToList();
                    List<Booking> ListofBookings = database.Bookings.ToList();
                    var RoomTypes = database.RoomTypes.ToList();
                    var BookingStatus = database.BookingStatus.ToList();
                    foreach (var data in ListOfRooms)
                    {
                        data.Type = RoomTypes.Find(option => option.RoomTypeId == data.RoomTypeId).RoomType;
                        data.Status = BookingStatus.Find(option => option.BookingStatusId == data.BookingStatusId).Status;
                        var model = ListOfRooms.Find(item => item.RoomId == data.RoomId);
                        model = data;
                    }
                    return View(ListOfRooms);
                }
            return new HttpNotFoundResult();
        }


        public ActionResult Users()
        {
            if (User.IsInRole("Admin"))
                using (var database = new ApplicationDbContext())
                {
                    return View(database.Users.ToList());
                }
            return new HttpNotFoundResult();
        }


        public ActionResult DeleteUser(string Id)
        {
            if (User.IsInRole("Admin"))
            {
                if (Id == null) return new HttpNotFoundResult();
                using (var database = new ApplicationDbContext())
                {
                    var User = database.Users.Where(user => user.Id == Id).First();
                    database.Users.Remove(User);
                    database.SaveChanges();
                    return RedirectToAction("Users");
                }
            }
            return new HttpNotFoundResult();
        }



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