using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

using NLog;

namespace DeadManSwitch.UI.Web.AspNet
{
    public class Global : HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            logger.Trace("start");
            logger.Info("ApplicationStart");
            
            //MS configs
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

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
