
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
            if (User.IsInRole("Admin")) { 

                    return View(HelperClass.GetRoomsForDashboard());
                }
            return new HttpNotFoundResult();
        }
    }
}