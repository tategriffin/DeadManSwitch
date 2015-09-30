using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch
{
    public static class StringExtensions
    {
        public static bool EqualsCaseInsensitive(this string value, string other)
        {
            return (string.Compare(value, other, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static void AddNonWhitespaceValue(this List<string> list, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            list.Add(value);
        }

        public static void AddNonWhitespaceValues(this List<string> list, IEnumerable<string> values)
        {
            if(values == null) return;

            foreach (var value in values)
            {
                list.AddNonWhitespaceValue(value);
            }
        }

    }
}
