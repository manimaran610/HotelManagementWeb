

using HotelManagementWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        public ActionResult Rooms()
        {
            using (var database = new HMSContext())
            {
                return View(database.Rooms.ToList());
            }
        }
    }
}
