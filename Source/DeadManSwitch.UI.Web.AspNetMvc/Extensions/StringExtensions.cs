using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    public static class StringExtensions
    {
        public static string Protect(this string text, params string[] purposes)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            byte[] utfBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = MachineKey.Protect(utfBytes, purposes);
            return HttpServerUtility.UrlTokenEncode(encryptedBytes);
        }

        public static string Unprotect(this string text, params string[] purposes)
        {
            string unprotectedText = null;

            if (!string.IsNullOrEmpty(text))
            {
                byte[] encryptedBytes = HttpServerUtility.UrlTokenDecode(text);
                if (encryptedBytes != null)
                {
                    byte[] unencryptedBytes = MachineKey.Unprotect(encryptedBytes, purposes);
                    if (unencryptedBytes != null)
                    {
                        unprotectedText = Encoding.UTF8.GetString(unencryptedBytes);
                    }
                }
            }

            return unprotectedText;
        }
    }
}