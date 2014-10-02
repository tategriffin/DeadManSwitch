using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Schedule
{
    public class DailySchedule : CheckInSchedule
    {
        private const int NumOfDaysInWeek = 7;
        private const string TimeOfDayPrefix = " at ";

        public DailySchedule(bool allDaysSet = false)
        {
            this.Days = InitializeDayDictionary(allDaysSet);

            this.CheckInTime = new TimeSpan();
            this.CheckInWindowStartTime = new TimeSpan();
        }

        public Dictionary<DayOfWeek, bool> Days { get; private set; }
        public TimeSpan CheckInTime { get; set; }

        /// <summary>
        /// Gets or sets the check in window start time.
        /// </summary>
        /// <remarks>
        /// This could be greater than the CheckInTime, indicating a time from the prior day
        /// e.g. CheckInTime = 3 hours; CheckInWidnowStartTime = 11 hours, means that the 
        /// person must check in by 3am, but as early as 11pm the prior day.
        /// </remarks>
        public TimeSpan CheckInWindowStartTime { get; set; }

        public bool Sunday { get { return this.Days[DayOfWeek.Sunday]; } set { this.Days[DayOfWeek.Sunday] = value; } }
        public bool Monday { get { return this.Days[DayOfWeek.Monday]; } set { this.Days[DayOfWeek.Monday] = value; } }
        public bool Tuesday { get { return this.Days[DayOfWeek.Tuesday]; } set { this.Days[DayOfWeek.Tuesday] = value; } }
        public bool Wednesday { get { return this.Days[DayOfWeek.Wednesday]; } set { this.Days[DayOfWeek.Wednesday] = value; } }
        public bool Thursday { get { return this.Days[DayOfWeek.Thursday]; } set { this.Days[DayOfWeek.Thursday] = value; } }
        public bool Friday { get { return this.Days[DayOfWeek.Friday]; } set { this.Days[DayOfWeek.Friday] = value; } }
        public bool Saturday { get { return this.Days[DayOfWeek.Saturday]; } set { this.Days[DayOfWeek.Saturday] = value; } }

        public override RecurrenceInterval Recurrence { get { return RecurrenceInterval.Daily; } }

        private Dictionary<DayOfWeek, bool> InitializeDayDictionary(bool withAllDaysSet)
        {
            Dictionary<DayOfWeek, bool> dict = new Dictionary<DayOfWeek, bool>();

            foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
            {
                dict.Add((DayOfWeek)item, withAllDaysSet);
            }

            return dict;
        }

        protected override string BuildScheduleDescription()
        {
            string desc = null;
            string timeOfDaySuffix = this.CheckInTime.ToTimeOfDay(TimeOfDayPrefix);

            int numOfDays = this.Days.Count(kvp => kvp.Value == true);
            switch (numOfDays)
            {
                case 0:
                    desc = "Never";
                    break;
                case 1:
                    desc = BuildSingleDayDescription() + timeOfDaySuffix;
                    break;
                case 2:
                    if (Saturday && Sunday)
                    {
                        desc = "Weekends" + timeOfDaySuffix;
                    }
                    break;
                case 5:
                    if (!Saturday && !Sunday)
                    {
                        desc = "Weekdays" + timeOfDaySuffix;
                    }
                    break;
                case 7:
                    desc = "Daily" + timeOfDaySuffix;
                    break;

            }

            if (string.IsNullOrWhiteSpace(desc))
            {
                desc = BuildCustomDescription() + timeOfDaySuffix;
            }
            return desc;
        }

        private string BuildSingleDayDescription()
        {
            if (Sunday) return "Sundays";
            if (Monday) return "Mondays";
            if (Tuesday) return "Tuesdays";
            if (Wednesday) return "Wednesdays";
            if (Thursday) return "Thursdays";
            if (Friday) return "Fridays";
            if (Saturday) return "Saturdays";

            return "Never";
        }

        private string BuildCustomDescription()
        {
            List<string> days = new List<string>();

            if (Sunday) days.Add("Su");
            if (Monday) days.Add("M");
            if (Tuesday) days.Add("Tu");
            if (Wednesday) days.Add("W");
            if (Thursday) days.Add("Th");
            if (Friday) days.Add("F");
            if (Saturday) days.Add("Sa");

            return string.Join(", ", days);
        }

        /// <summary>
        /// Calculates the next check in.
        /// </summary>
        /// <param name="userTimeZone">The user time zone.</param>
        /// <returns></returns>
        /// <remarks>
        /// MUST return times in the user's local time zone (not the system time zone, nor UTC)
        /// </remarks>
        public override DateTime? CalculateNextCheckIn(TimeZoneInfo userTimeZone)
        {
            if (userTimeZone == null) throw new ArgumentNullException("userTimeZone");

            if (this.Enabled == false) { return null; };

            DateTime? nextExpectedCheckIn = null;
            DateTime userLocalCurrentDateTime = DateTimeCalculator.GetUserLocalDateTime(userTimeZone);

            bool checkNextDay = true;
            DateTime dayToCheck = userLocalCurrentDateTime;
            int numOfDaysChecked = 0;
            do
            {
                nextExpectedCheckIn = GetCheckInForSpecifiedDay(dayToCheck);

                if (nextExpectedCheckIn.HasValue)
                {
                    if (nextExpectedCheckIn.Value > userLocalCurrentDateTime)
                    {
                        bool inWindow = new DailyScheduleCheckInWindow(nextExpectedCheckIn.Value, CheckInTime, CheckInWindowStartTime).IsInWindow(userLocalCurrentDateTime);

                        checkNextDay = inWindow;
                    }
                }
                if (numOfDaysChecked++ > 10) {checkNextDay = false;}

                dayToCheck = dayToCheck.AddDays(1);
            } while (checkNextDay);

            return nextExpectedCheckIn;
        }

        private DateTime? GetCheckInForSpecifiedDay(DateTime userLocalDay)
        {
            DateTime? expectedCheckIn = null;

            if (this.Days[userLocalDay.DayOfWeek])
            {
                expectedCheckIn =
                    new DateTime(
                        userLocalDay.Year, userLocalDay.Month, userLocalDay.Day,
                        this.CheckInTime.Hours, this.CheckInTime.Minutes, 0);
            }

            return expectedCheckIn;
        }

        public List<string> Validate()
        {
            List<string> validationMessages = new List<string>();

            validationMessages.AddIfNonEmpty(ValidateUserId());
            validationMessages.AddIfNonEmpty(ValidateScheduleName());
            validationMessages.AddIfNonEmpty(ValidateScheduleDays());

            return validationMessages;
        }

        private string ValidateUserId()
        {
            string msg = string.Empty;
            if (this.UserId == 0)
            {
                msg = "The schedule UserId must be set";
            }

            return msg;
        }

        private string ValidateScheduleName()
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                msg = "The schedule name cannot be empty";
            }

            return msg;
        }

        private string ValidateScheduleDays()
        {
            string msg = "At least one day must be selected";
            foreach (var kvp in this.Days)
            {
                if (kvp.Value == true)
                {
                    msg = string.Empty;
                    break;
                }
            }
            return msg;
        }

        public bool EqualDaysAndTime(DailySchedule other)
        {
            if (other == null) throw new ArgumentNullException("other");

            bool sameDaysSelected = true;
            foreach (var kvp in this.Days)
            {
                if (this.Days[kvp.Key] != other.Days[kvp.Key])
                {
                    sameDaysSelected = false;
                    break;
                }
            }

            bool sameTimeSelected = false;
            if (sameDaysSelected)
            {
                sameTimeSelected = (this.CheckInTime.EqualHourAndMinutes(other.CheckInTime) && this.CheckInWindowStartTime.EqualHourAndMinutes(other.CheckInWindowStartTime));
            }

            return (sameDaysSelected && sameTimeSelected);
        }

    }
}
