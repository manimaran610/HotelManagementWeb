﻿
using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
  
    [Route("{action=index}")]
   
    public class DashboardController : Controller
    {
        // GET: Dashboard
        private Random randomnumber = new Random();
       
        
        public ActionResult Index()
            
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

      

        [HttpGet]
        public ActionResult AddRoom(Room model)
        {
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
        }

        public ActionResult EditRoom(Room model)
        {
            using (var database = new HMSContext())
            {
                model.RoomTypeList = database.RoomTypes.ToList();
                model.BookStatusList = database.BookingStatus.ToList();
                ViewBag.Title = "Update Room";
                ViewData["PostAction"] = "UpdateRoomPost";
                if (model.ErrorMessage != null) ModelState.AddModelError("RoomNumber", model.ErrorMessage);
                return PartialView("AddOrUpdateRoom", model);
            }


        }





        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddRoomPost(Room model)
        {
            using (var database = new HMSContext())
            {
                bool IsValid = database.Rooms.ToList().Exists(item => item.RoomNumber == model.RoomNumber);
                if (ModelState.IsValid && !IsValid)
                {
                    string ImageName = model.RoomNumber + randomnumber.Next() + Path.GetExtension(model.UploadedImage.FileName);
                    model.UploadedImage.SaveAs(Server.MapPath("~/RoomImages/" + ImageName));
                    model.RoomImage = ImageName;
                    model.IsActive = true;
                    database.Rooms.Add(model);
                    database.SaveChanges();
                    return View(model);
                }
                else
                {
                    model.ErrorMessage = "Room Number already exists";
                    return RedirectToAction("AddRoom", model);
                }
            }

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
                    }
                    database.Rooms.AddOrUpdate(model);
                    database.SaveChanges();
                    return View("AddRoomPost", model);
                }
                else
                {
                    model.ErrorMessage = "Room Number already exists with different record";
                    return RedirectToAction("EditRoom", model);
                }
            }

        }


        public ActionResult DeleteRoom(int Id)
        {
            using (var database = new HMSContext())
            {
                var room = database.Rooms.Where(item => item.RoomId == Id).First();
                database.Rooms.Remove(room);
                database.SaveChanges();
                FileInfo file = new FileInfo(Server.MapPath("~/RoomImages/" + room.RoomImage));
                file.Delete();
                return RedirectToAction("Index");

            }
        }
    }
}