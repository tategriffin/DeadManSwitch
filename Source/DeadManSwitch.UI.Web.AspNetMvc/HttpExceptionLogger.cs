using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NLog;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    internal class HttpExceptionLogger
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Log(HttpException ex)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            int httpStatusCode = ex.GetHttpCode();

            if (httpStatusCode >= 400 && httpStatusCode < 500)
            {
                this.Log4xxRange(ip, httpStatusCode, ex);
            }
            else if (httpStatusCode >= 500)
            {
                logger.Fatal("IP Address: {0}; HttpStatusCode: {1} Exception: {2}", ip, httpStatusCode, ex);
            }
            else
            {
                logger.Error("IP Address: {0}; HttpStatusCode: {1} Exception: {2}", ip, httpStatusCode, ex);
            }
        }

        private void Log4xxRange(string ip, int httpStatusCode, HttpException ex)
        {
            switch (httpStatusCode)
            {
                case 404:
                    logger.Warn("IP Address: {0}; HttpStatusCode: {1} Exception: {2}", ip, httpStatusCode, ex);
                    break;
                default:
                    logger.Error("IP Address: {0}; HttpStatusCode: {1} Exception: {2}", ip, httpStatusCode, ex);
                    break;
            }
        }

    }
}