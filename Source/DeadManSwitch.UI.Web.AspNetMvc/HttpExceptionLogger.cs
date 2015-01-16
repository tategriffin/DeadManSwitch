using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NLog;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    internal class HttpExceptionLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Log(HttpException ex)
        {
            int httpStatusCode = ex.GetHttpCode();

            if (httpStatusCode >= 400 && httpStatusCode < 500)
            {
                this.Log4xxRange(httpStatusCode, ex);
            }
            else if (httpStatusCode >= 500)
            {
                logger.Fatal("HttpStatusCode: {0} Exception: {1}", httpStatusCode, ex.ToString());
            }
            else
            {
                logger.Error("HttpStatusCode: {0} Exception: {1}", httpStatusCode, ex.ToString());
            }
        }

        private void Log4xxRange(int httpStatusCode, HttpException ex)
        {
            switch (httpStatusCode)
            {
                case 404:
                    logger.Warn("HttpStatusCode: {0} Message: {1}", httpStatusCode, ex.Message);
                    break;
                default:
                    logger.Error("HttpStatusCode: {0} Details: {1}", httpStatusCode, ex.ToString());
                    break;
            }
        }

    }
}