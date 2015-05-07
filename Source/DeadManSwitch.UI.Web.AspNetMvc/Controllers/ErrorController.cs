using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeadManSwitch.UI.Web.AspNetMvc.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error/NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        // GET: Error/ServerError
        public ActionResult ServerError()
        {
            Response.StatusCode = (int) System.Net.HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}