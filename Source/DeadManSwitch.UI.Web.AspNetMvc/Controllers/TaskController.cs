using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Configuration;
using DeadManSwitch.Service;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNetMvc.Controllers
{
    public class TaskController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly IUnityContainer Container;
        public TaskController(IUnityContainer container)
        {
            Container = container;
        }

        // GET: Task
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Escalate()
        {
            System.Net.HttpStatusCode result = RunEscalate();
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
        public System.Net.HttpStatusCode RunEscalate()
        {
            string userName = (User.Identity.IsAuthenticated ? User.Identity.Name : "anonymous user");
            Log.Info($"{userName} from IP: {Request.UserHostAddress} requested escalate.");

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
                EscalationDaemon.Start(Container, Container.Resolve<IHostSettingsReader>());

                statusCode = System.Net.HttpStatusCode.OK;
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
                Log.Info($"Escalate returned: {code}");
            }
            else if (code.IsClientError() || code.IsServerError())
            {
                Log.Error($"Escalate returned error: {code}");
            }
            else
            {
                Log.Warn($"Escalate returned unexpected status: {code}");
            }
        }
        #endregion

    }
}