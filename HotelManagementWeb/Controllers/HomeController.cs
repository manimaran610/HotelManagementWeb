using HotelManagementSystem.Models;
using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
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

        public ActionResult Dashboard()
        {
            using (var database = new HMSContext())
            {

                List<Room> ListOfRooms = database.Rooms.ToList();
                foreach (var data in ListOfRooms)
                {
                    data.Type = database.RoomTypes.ToList().Find(option => option.RoomTypeId == data.RoomTypeId).RoomType;
                    data.Status = database.BookingStatus.ToList().Find(option => option.BookingStatusId == data.BookingStatusId).Status;
                    var model = ListOfRooms.Find(item => item.RoomId == data.RoomId);
                    model = data;
                }


                return View(ListOfRooms);
            }
        }

        public ActionResult AddRoom(Room model)
        {

            using (var database = new HMSContext())
            {
                // Room model = new Room();
                model.RoomTypeList = database.RoomTypes.ToList();
                model.BookStatusList = database.BookingStatus.ToList();

                return View(model);
            }

        }

        [HttpPost]
        public ActionResult AddRoomPost()
        {
            var model = new Room();

            TryUpdateModel<Room>(model);
            if (model.UploadedImage != null)
            {
                Random randomnumber = new Random();
                string ImageName = model.RoomNumber + randomnumber.Next() + Path.GetExtension(model.UploadedImage.FileName);
                model.UploadedImage.SaveAs(Server.MapPath("~/RoomImages/" + ImageName));
                model.RoomImage = ImageName;
                model.IsActive = true;
            }
            Debug.WriteLine(model.RoomImage);
            using (var database = new HMSContext())
            {
                database.Rooms.AddOrUpdate(model);
                database.SaveChanges();
                return View(model);
            }
        }
        
        public ActionResult DeleteRoom(int Id)
        {
            using(var database = new HMSContext())
            {
                var room = database.Rooms.Where(item => item.RoomId == Id).First();
                database.Rooms.Remove(room);
                database.SaveChanges();
                return RedirectToAction("Dashboard");

            }
        }


    }
}
