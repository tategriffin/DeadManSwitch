using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadManSwitch.UI
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

        public static Dictionary<string, string> ToStringKeysAndValues(this Dictionary<int, string> dict)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var kvp in dict)
            {
                result.Add(kvp.Key.ToString(), kvp.Value);
            }

            return result;
        }
    }

    public static class StringExtensions
    {
        public static string ToPhoneNumber(this string unformattedText)
        {
            if (string.IsNullOrWhiteSpace(unformattedText)) return string.Empty;

            string phoneNumber = unformattedText;

            Int64 textAsInt;
            if (unformattedText.Length == 10 && Int64.TryParse(unformattedText, out textAsInt))
            {
                System.Text.StringBuilder formatted = new System.Text.StringBuilder(14);
                formatted
                    .Append("(")
                    .Append(unformattedText.Substring(0, 3))
                    .Append(") ")
                    .Append(unformattedText.Substring(3, 3))
                    .Append("-")
                    .Append(unformattedText.Substring(6, 4));

                phoneNumber = formatted.ToString();
            }

            return phoneNumber;
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