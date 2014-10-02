using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch
{
    public static class StringExtensions
    {
        public static string Inject(this string formattedString, params object[] args)
        {
            return string.Format(formattedString, args);
        }

        public static void AddIfNonEmpty(this List<string> list, string item)
        {
            if (string.IsNullOrWhiteSpace(item) == false)
            {
                list.Add(item);
            }
        }

        public static void AddRangeIfNonEmpty(this List<string> list, IEnumerable<string> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            foreach (string item in items)
            {
                list.AddIfNonEmpty(item);
            }
        }

    }

    public static class TimeSpanExtensions
    {
        public static string ToTimeOfDay(this DateTime dt)
        {
            DateTime rootDt = dt.Date;
            TimeSpan ts = dt.Subtract(rootDt);

            return ts.ToTimeOfDay();
        }

        public static string ToTimeOfDay(this TimeSpan ts)
        {
            return ts.ToTimeOfDay(string.Empty);
        }

        public static string ToTimeOfDay(this TimeSpan ts, string prefix)
        {
            if (ts.TotalHours > 24) return string.Empty;
            if (ts.TotalMinutes < 1) return string.Empty;

            string desc;
            int hours = ts.Hours;
            int minutes = ts.Minutes;

            if (minutes == 0 && (hours == 0 || hours == 12))
            {
                desc = (hours == 0 ? "midnight" : "noon");
            }
            else
            {
                string ampm = " " + (hours < 12 ? "am" : "pm");
                int clockHours = (hours > 12 ? hours - 12 : hours);

                desc = clockHours.ToString("0") + ":" + minutes.ToString("00") + ampm;
            }

            return prefix + desc;
        }

        public static bool EqualHourAndMinutes(this TimeSpan ts, TimeSpan other)
        {
            return (ts.Hours == other.Hours && ts.Minutes == other.Minutes);
        }
    }

    public static class DayOfWeekExtensionMethods
    {
        public static DayOfWeek NextDayOfWeek(this DayOfWeek currentDayOfWeek)
        {
            DayOfWeek nextDayOfWeek;

            //Assumes Sunday = 0
            int dowAsInt = (int)currentDayOfWeek;
            switch (dowAsInt)
            {
                case 6:     //Saturday
                    nextDayOfWeek = DayOfWeek.Sunday;
                    break;
                default:
                    nextDayOfWeek = (DayOfWeek)(dowAsInt + 1);
                    break;
            }

            return nextDayOfWeek;
        }

    }

    public static class DateTimeExtensions
    {
        public static bool IsSameDay(this DateTime dt, DateTime otherDate)
        {
            bool sameDay = false;
            if (dt.Kind == otherDate.Kind)
            {
                sameDay = (dt.Year == otherDate.Year
                    && dt.Month == otherDate.Month
                    && dt.Day == otherDate.Day);
            }

            return sameDay;
        }

        /// <summary>
        /// Determines whether this date is the day after the the specified otherDate.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="otherDate">The other date.</param>
        /// <returns>
        ///   <c>true</c> if this date is the day after the the specified otherDate; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDayAfter(this DateTime dt, DateTime otherDate)
        {
            DateTime dayAfterOtherDate = otherDate.AddDays(1);
            return dt.IsSameDay(dayAfterOtherDate);
        }

        /// <summary>
        /// Determines whether this date is the day before the the specified otherDate.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="otherDate">The other date.</param>
        /// <returns>
        ///   <c>true</c> if this date is the day before the the specified otherDate; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDayBefore(this DateTime dt, DateTime otherDate)
        {
            DateTime dayBeforeOtherDate = dt.AddDays(-1);
            return dt.IsSameDay(dayBeforeOtherDate);
        }

        public static DateTime ToMinutePrecision(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Kind);
        }

    }

}
