using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    public class UserController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult GetAll()
        {
            try
            {
                List<ApplicationUser> applicationUsers = UserDatabaseLayer.GetAllUsers();
                return View(applicationUsers);

            }
            catch (Exception exception)
            {
                return new HttpNotFoundResult();
            }

        }


        public ActionResult DeleteUser(string Id)
        {
            if (User.IsInRole("Admin") && Id !=null)
            {
                try
                {
                    UserDatabaseLayer.DeleteUser(Id);
                    return RedirectToAction("GetAll");
                }
                catch (Exception exception)
                {
                    return new HttpNotFoundResult();
                }
            }
            return new HttpNotFoundResult();
        }
    }
}