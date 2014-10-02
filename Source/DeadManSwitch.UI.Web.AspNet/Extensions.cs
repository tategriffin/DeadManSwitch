using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadManSwitch.UI.Web.AspNet
{
    public static class DictionaryExtensions
    {
        public static System.Web.UI.WebControls.ListItem[] ToListItems(this Dictionary<string, string> dict)
        {
            List<System.Web.UI.WebControls.ListItem> listItems = new List<System.Web.UI.WebControls.ListItem>();

            foreach (var kvp in dict)
            {
                listItems.Add(new System.Web.UI.WebControls.ListItem(kvp.Value, kvp.Key));
            }

            return listItems.ToArray();
        }
    }

    public static class HttpStatusCodeExtensions
    {
        private const int Informational = 100;
        private const int Success = 200;
        private const int Redirection = 300;
        private const int ClientError = 400;
        private const int ServerError = 500;
        private const int NotDefined = 600;

        private static bool IsInRange(int value, int minValueInclusive, int maxValueNonInclusive)
        {
            return (minValueInclusive <= value && value < maxValueNonInclusive);
        }

        public static bool IsInformational(this System.Net.HttpStatusCode statusCode)
        {
            return IsInRange((int)statusCode, Informational, Success);
        }

        public static bool IsSuccess(this System.Net.HttpStatusCode statusCode)
        {
            return IsInRange((int)statusCode, Success, Redirection);
        }

        public static bool IsRedirection(this System.Net.HttpStatusCode statusCode)
        {
            return IsInRange((int)statusCode, Redirection, ClientError);
        }

        public static bool IsClientError(this System.Net.HttpStatusCode statusCode)
        {
            return IsInRange((int)statusCode, ClientError, ServerError);
        }

        public static bool IsServerError(this System.Net.HttpStatusCode statusCode)
        {
            return IsInRange((int)statusCode, ServerError, NotDefined);
        }

    }
}