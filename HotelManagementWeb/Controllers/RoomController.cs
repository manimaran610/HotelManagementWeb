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
        readonly string Admin = "Admin";
        private Random randomnumber = new Random();


        [HttpGet]
        public ActionResult AddRoom()
        {
            if (User.IsInRole(Admin))
            {
                Room model = new Room();
                HelperClass.FillDropDownListItems(model);
                ModelState.Clear();
                return View(model);
            }
            return new HttpNotFoundResult();
        }



        [HttpGet]
        public ActionResult EditRoom(Room model)
        {
            if (User.IsInRole(Admin))
            {
                if (model != null)
                {
                    HelperClass.FillDropDownListItems(model);
                    return View(model);
                }
                return View("Error");
            }
            return new HttpNotFoundResult();
        }

        public ActionResult DeleteRoom(int Id)
        {
            if (User.IsInRole("Admin") && IsRoomIdAlreadyExists(Id))
            {
                var ExistingRoom = HotelDatabaseLayer.GetRooms().Find(item => item.RoomId == Id);
                RoomCrudOperations.DeleteRoomFromDatabase(ExistingRoom);
                HelperClass.DeleteExistingImageInProjectFolder(ExistingRoom.RoomImage);
                return RedirectToAction("Index", "Dashboard");
            }
            return new HttpNotFoundResult();
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddRoom(Room model)
        {
            if (User.IsInRole(Admin))
            {
                if (ModelState.IsValid && !IsRoomNumberAlreadyExists(model.RoomNumber))
                {
                    model.RoomImage = HelperClass.AddImageToProjectFolder(model);
                    model.IsActive = true;
                    RoomCrudOperations.AddRoomToDatabase(model);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("RoomNumber", "Room Number already exists");
                    HelperClass.FillDropDownListItems(model);
                    return View(model);
                }

            }
            return new HttpNotFoundResult();

        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateExistingRoom(Room model)
        {

                if (ModelState.IsValid && !IsRoomNumberAlreadyExists(model.RoomNumber,model.RoomId))
                {
                    if (model.UploadedImage != null)
                    {
                        HelperClass.DeleteExistingImageInProjectFolder(model.RoomImage);
                        model.RoomImage = HelperClass.AddImageToProjectFolder(model);                //Generates random room name ,save it and return that image name
                    }
                    RoomCrudOperations.UpdateExistingRoomInDatabase(model);
                    return RedirectToAction("index", "dashboard");
                }
                else
                {
                    ModelState.AddModelError("RoomNumber", "Room Number already exists with different record");
                    HelperClass.FillDropDownListItems(model);
                    return View("EditRoom", model);
                }
            }

        

     

        private bool IsRoomNumberAlreadyExists(int? RoomNumber)
        {
            if (RoomNumber != null)
            {

                return HotelDatabaseLayer.GetRooms().Exists(item => item.RoomNumber == RoomNumber);
            }
            else throw new ArgumentNullException();
        }

        private bool IsRoomNumberAlreadyExists(int? RoomNumber,int Id)
        {
            if (RoomNumber != null)
            {
               
                return HotelDatabaseLayer.GetRooms().Exists(item => item.RoomNumber == RoomNumber && item.RoomId !=Id);
            }
            else throw new ArgumentNullException();
        }

        private bool IsRoomIdAlreadyExists(int? Id)
        {
            if (Id != null)
            {
                return HotelDatabaseLayer.GetRooms().Exists(item => item.RoomId == Id);
            }
            else throw new ArgumentNullException();
        }


    }
}
