using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNetMvc.Controllers
{
    public class HomeController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly ICheckInService CheckInSvc;

        public HomeController(ICheckInService checkInService)
        {
            CheckInSvc = checkInService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CheckIn()
        {
            try
            {
                var result = CheckInSvc.CheckInUser(User.Identity.Name);
                string msg = string.Format("Thanks for checking in {0}. Your next scheduled check in is {1}", result.DisplayName, result.GetNextCheckInText());

                return Json(new {message = msg});
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not check in. {1}", User.Identity.Name, ex);
                return Json(new { message = "Check in failed." });
            }
        }

    }
}