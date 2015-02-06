﻿using System;
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
        private readonly IEscalationService EscalationSvc;
        public HomeController(ICheckInService checkInService, IEscalationService escalationService)
        {
            CheckInSvc = checkInService;
            EscalationSvc = escalationService;
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

        public ActionResult Escalate()
        {
            System.Net.HttpStatusCode result = Run();
            switch (result)
            {
                case HttpStatusCode.InternalServerError:
                    ViewBag.EscalateResultMessage = "The requested task could not be completed.";
                    break;
                default:
                    ViewBag.EscalateResultMessage = "OK";
                    break;
            }

            return View();
        }

        #region MoveToService
        public System.Net.HttpStatusCode Run()
        {
            string userName = (User.Identity.IsAuthenticated ? User.Identity.Name : "anonymous user");
            Log.Info(string.Format("{0} from IP: {1} requested escalate.", userName, Request.UserHostAddress));

            System.Net.HttpStatusCode? statusCode = GetFakeStatusCode();
            if (!statusCode.HasValue)
            {
                statusCode = GetRealStatusCode();
            }

            LogRunResult(statusCode.Value);
            return statusCode.Value;
        }

        private static System.Net.HttpStatusCode? GetFakeStatusCode()
        {
            return EscalationTaskStatusCodeFactory.GetHttpStatusCode();
        }

        private System.Net.HttpStatusCode GetRealStatusCode()
        {
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.InternalServerError;
            try
            {
                bool successful = this.EscalationSvc.Run();
                if (successful)
                {
                    statusCode = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return statusCode;
        }

        private void LogRunResult(HttpStatusCode code)
        {
            if (code.IsInformational() || code.IsSuccess())
            {
                Log.Info(string.Format("Escalate returned: {0}", code));
            }
            else if (code.IsClientError() || code.IsServerError())
            {
                Log.Error(string.Format("Escalate returned error: {0}", code));
            }
            else
            {
                Log.Warn(string.Format("Escalate returned unexpected status: {0}", code));
            }
        }
        #endregion

    }
}