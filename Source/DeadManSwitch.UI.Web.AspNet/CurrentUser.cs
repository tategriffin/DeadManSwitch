using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI.Web.AspNet
{
    public class CurrentUser
    {
        public CurrentUser(string userName, bool isAuthenticated, string ipAddress)
        {
            this.UserName = (String.IsNullOrWhiteSpace(userName) ? string.Empty : userName);
            this.IsAuthenticated = isAuthenticated;
            this.IpAddress = ipAddress;
        }

        public string UserName { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public string IpAddress { get; private set; }


        public override string ToString()
        {
            return FormatUserAsString(this);
        }

        public static CurrentUser Anonymous(string ipAddress)
        {
            return new CurrentUser(userName: string.Empty, isAuthenticated: false, ipAddress: ipAddress);
        }

        private static string FormatUserAsString(CurrentUser user)
        {
            string auth = (user.IsAuthenticated ? "authenticated" : "unauthenticated");
            string ip = (string.IsNullOrWhiteSpace(user.IpAddress) ? string.Empty : " from IP: " + user.IpAddress);

            return string.Format("{0} user '{1}'{2}", auth, user.UserName, ip);
        }
    }
}
