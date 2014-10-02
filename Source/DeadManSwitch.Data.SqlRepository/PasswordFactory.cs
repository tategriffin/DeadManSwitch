using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace DeadManSwitch.Data.SqlRepository
{
    internal static class PasswordFactory
    {
        private const string LogMsgFormat = "HashPassword algorithm took {0} milliseconds.";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Adjust work factor so that hash takes as much time as possible, but still less than 1 second
        private const int MooresLawWorkFactor = 13;

        public static string HashPassword(string pwd)
        {
            DateTime startTime = DateTime.Now;
            string hashedPwd = BCrypt.Net.BCrypt.HashPassword(pwd, MooresLawWorkFactor);
            DateTime endTime = DateTime.Now;

            TimeSpan diff = endTime.Subtract(startTime);
            LogTime(diff);

            return hashedPwd;
        }

        public static bool ComparePasswords(string plainTxtPwd, string hashedPwd)
        {
            DateTime startTime = DateTime.Now;
            bool pwdsAreSame = BCrypt.Net.BCrypt.Verify(plainTxtPwd, hashedPwd);
            DateTime endTime = DateTime.Now;

            TimeSpan diff = endTime.Subtract(startTime);
            LogTime(diff);

            return pwdsAreSame;
        }

        private static void LogTime(TimeSpan ts)
        {
            if (ts.TotalMilliseconds < 300 || ts.TotalMilliseconds > 1500)
            {
                //too fast or too slow
                logger.Warn(string.Format(LogMsgFormat, ts.TotalMilliseconds));
            }
            else
            {
                //about right
                logger.Info(string.Format(LogMsgFormat, ts.TotalMilliseconds));
            }
        }
    }
}
