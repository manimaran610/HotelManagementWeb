
using HotelManagementWeb.Models;

using System.Collections.Generic;

using System.Linq;

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
    }
}