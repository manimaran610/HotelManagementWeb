using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementWeb.Controllers
{
    public class UserController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult GetAll()
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
                    return RedirectToAction("GetAll");
                }
            }
            return new HttpNotFoundResult();
        }
    }
}