using HotelManagementWeb.Models;
using System;

using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;

using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomController : Controller
    {
        private Random randomnumber = new Random();

        [HttpGet]
        public ActionResult AddRoom(Room model)
        {
            if (User.IsInRole("Admin"))
            ViewBag.Title = "Add Room";
            ViewData["PostAction"] = "AddRoomPost";
            ViewBag.required = "required";
            using (var database = new HMSContext())
            {

                model.RoomTypeList = database.RoomTypes.ToList();
                model.BookStatusList = database.BookingStatus.ToList();
                ModelState.Clear();
                if (model.ErrorMessage != null) ModelState.AddModelError("RoomNumber", model.ErrorMessage);
                return PartialView("AddOrUpdateRoom", model);
            }
            return new HttpNotFoundResult();
        }

        public ActionResult EditRoom(Room model)
        {
            if (User.IsInRole("Admin"))
                using (var database = new HMSContext())
                {
                   // Room model = database.Rooms.ToList().Find(item => item.RoomId == Id);
                    if (model != null)
                    {
                        model.RoomTypeList = database.RoomTypes.ToList();
                        model.BookStatusList = database.BookingStatus.ToList();
                        ViewBag.Title = "Update Room";
                        ViewData["PostAction"] = "UpdateRoomPost";
                        if (model.ErrorMessage != null) ModelState.AddModelError("RoomNumber", model.ErrorMessage);
                        return PartialView("AddOrUpdateRoom", model);
                    }
                    return View("Error");
                }

            return new HttpNotFoundResult();
        }

        


            [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddRoomPost(Room model)
        {
            if (User.IsInRole("Admin"))
                using (var database = new HMSContext())
                {
                    bool IsValid = database.Rooms.ToList().Exists(item => item.RoomNumber == model.RoomNumber);
                    if (ModelState.IsValid && !IsValid)
                    {
                       // Image saving in the project folder
                        string ImageName = model.RoomNumber + randomnumber.Next() + Path.GetExtension(model.UploadedImage.FileName);
                        model.UploadedImage.SaveAs(Server.MapPath("~/RoomImages/" + ImageName));
                        model.RoomImage = ImageName;

                        ////Image saving as bytes in the database
                        //model.ImageByte = new byte[model.UploadedImage.ContentLength];
                        //model.UploadedImage.InputStream.Read(model.ImageByte, 0, model.UploadedImage.ContentLength);

                        model.IsActive = true;
                        database.Rooms.Add(model);
                        database.SaveChanges();
                        return RedirectToAction("index", "dashboard");
                    }
                    else
                    {
                        model.ErrorMessage = "Room Number already exists";
                        return RedirectToAction("AddRoom", model);
                    }
                }
            return new HttpNotFoundResult();

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateRoomPost(Room model)
        {
           
                using (var database = new HMSContext())
                {
                    bool IsValid = database.Rooms.ToList().Exists(item => item.RoomNumber == model.RoomNumber && item.RoomId != model.RoomId);
                    if (ModelState.IsValid && !IsValid)
                    {
                        if (model.UploadedImage != null)
                        {
                        FileInfo file = new FileInfo(Server.MapPath("~/RoomImages/" + model.RoomImage));
                        file.Delete();
                        string ImageName = model.RoomNumber + randomnumber.Next() + Path.GetExtension(model.UploadedImage.FileName);
                        model.UploadedImage.SaveAs(Server.MapPath("~/RoomImages/" + ImageName));
                        model.RoomImage = ImageName;

                        //model.ImageByte = new byte[model.UploadedImage.ContentLength];
                        //    model.UploadedImage.InputStream.Read(model.ImageByte, 0, model.UploadedImage.ContentLength);

                        }
                        database.Rooms.AddOrUpdate(model);
                        database.SaveChanges();
                        return RedirectToAction("index", "dashboard");
                    }
                    else
                    {
                        model.ErrorMessage = "Room Number already exists with different record";
                        return RedirectToAction("EditRoom",model);
                    }
                }
        
        }


        public ActionResult DeleteRoom(int Id)
        {
            if (User.IsInRole("Admin"))
                using (var database = new HMSContext())
                {
                    var room = database.Rooms.Where(item => item.RoomId == Id).First();
                    database.Rooms.Remove(room);
                    database.SaveChanges();

                    FileInfo file = new FileInfo(Server.MapPath("~/RoomImages/" + room.RoomImage));
                    file.Delete();

                    return RedirectToAction("Index", "Dashboard");

                }
            return new HttpNotFoundResult();
        }
    }
}
