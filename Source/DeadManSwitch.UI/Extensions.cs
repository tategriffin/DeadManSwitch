using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadManSwitch.UI
{
    public static class DictionaryExtensions
    {
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

}