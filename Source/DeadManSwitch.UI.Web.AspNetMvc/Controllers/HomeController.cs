using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<ActionResult> CheckIn()
        {
            try
            {
                var result = await CheckInSvc.CheckInUserAsync(User.Identity.Name);
                string msg = $"Thanks for checking in {result.DisplayName}. Your next scheduled check in is {result.GetNextCheckInText()}";

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