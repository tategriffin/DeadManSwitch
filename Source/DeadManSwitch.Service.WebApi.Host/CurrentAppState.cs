using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Configuration;

namespace DeadManSwitch.Service.WebApi.Host
{
    public static class CurrentAppState
    {
        private static IHostSettingsReader AppSettings
        {
            get { return (IHostSettingsReader) DependencyResolver.Current.GetService(typeof (IHostSettingsReader)); }
        }

        //TODO: Implement security
        internal static bool IsUserAuthenticated(HttpRequestMessage requestMessage, out string userName)
        {
            const string HeaderKey = "X-UserName";
            bool isAuthenticated = false;
            userName = String.Empty;

            IEnumerable<string> values;
            if (requestMessage.Headers.TryGetValues(HeaderKey, out values))
            {
                userName = values.First();
                isAuthenticated = true;
            }

            return isAuthenticated;
        }

    }
}