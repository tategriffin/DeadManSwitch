using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace DeadManSwitch.Service.WebApi.Host
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            logger.Trace("start");
            logger.Info("ApplicationStart");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            IocConfig.Register();

            logger.Trace("end");
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            logger.Info("ApplicationEnd");
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            System.Web.HttpContext context = HttpContext.Current;
            System.Exception ex = Context.Server.GetLastError();

            try
            {
                HttpException httpEx = ex as HttpException;
                if (httpEx != null)
                {
                    new HttpExceptionLogger().Log(httpEx);
                }
                else
                {
                    logger.Fatal(ex.ToString());
                }
            }
            catch (Exception secondaryEx)
            {
                logger.Fatal("Primary Exception: {0}; Secondary Exception: {1}", ex, secondaryEx);
            }
        }
    }
}
