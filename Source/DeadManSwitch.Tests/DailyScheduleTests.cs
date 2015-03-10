using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeadManSwitch;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class DailyScheduleTests
    {
        [TestMethod]
        public void DailyScheduleCtor_AllDaysAreFalse_WhenNoConstructorParmIsSpecified()
        {
            //Arrange
            bool expected = false;

            //Act
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule();

            //Assert
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Sunday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Monday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Tuesday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Wednesday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Thursday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Friday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Saturday]);
        }

        [TestMethod]
        public void DailyScheduleCtor_AllDaysAreTrue_WhenConstructorParmIsTrue()
        {
            //Arrange
            bool expected = true;

            //Act
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);

            //Assert
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Sunday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Monday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Tuesday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Wednesday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Thursday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Friday]);
            Assert.AreEqual(expected, cut.Days[DayOfWeek.Saturday]);
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ThrowsException_WhenTimeZoneIsNull()
        {
            //Arrange
            const string testFailureMsg = "DailySchedule.CalculateNextCheckIn should throw an exception when time zone is null.";
            TimeZoneInfo tzInfo = null;
            int earlyCheckinMinutes = 15;
            DateTime testDayTime = DateTime.Now.AddMinutes(-5).ToMinutePrecision();     //Five mins ago
            DateTime earlyCheckInDayTime = testDayTime.AddMinutes(earlyCheckinMinutes * -1);

            //Create a daily schedule for all days of week
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;
            cut.Enabled = false;

            //Act
            try
            {
                DateTime? actual = cut.CalculateNextCheckIn(tzInfo);
                
                Assert.Fail(testFailureMsg);
            }
            catch (ArgumentNullException ex)
            {
                //Assert
                Assert.IsTrue(ex.Message.Contains("userTimeZone"), testFailureMsg);
            }
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ReturnsNull_WhenScheduleIsDisabled()
        {
            //Arrange
            TimeZoneInfo tzInfo = TimeZoneInfo.Local;
            int earlyCheckinMinutes = 15;
            DateTime testDayTime = DateTime.Now.AddMinutes(-5).ToMinutePrecision();     //Five mins ago
            DateTime earlyCheckInDayTime = testDayTime.AddMinutes(earlyCheckinMinutes * -1);

            //Create a daily schedule for all days of week
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;
            cut.Enabled = false;

            //Act
            DateTime? actual = cut.CalculateNextCheckIn(tzInfo);

            //Assert
            Assert.IsFalse(actual.HasValue, "DailySchedule.CalculateNextCheckIn should return null when the daily schedule is disabled.");
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ReturnsTomorrow_WhenScheduleHasAllDaysAndTimeHasPassed()
        {
            //Arrange
            TimeZoneInfo tzInfo = TimeZoneInfo.Local;
            int earlyCheckinMinutes = 15;
            DateTime testDayTime = DateTime.Now.AddMinutes(-5).ToMinutePrecision();     //Five mins ago
            DateTime earlyCheckInDayTime = testDayTime.AddMinutes(earlyCheckinMinutes * -1);
            DateTime expected = testDayTime.AddDays(1);

            //Create a daily schedule for all days of week
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;

            //Act
            DateTime? actual = cut.CalculateNextCheckIn(tzInfo);

            //Assert
            Assert.IsTrue(actual.HasValue, "DailySchedule.CalculateNextCheckIn should not return null for this test.");
            Assert.AreEqual(expected, actual.Value);
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ReturnsTodayPlus7Days_WhenScheduleHas1DayAndTimeHasPassed()
        {
            //Arrange
            TimeZoneInfo tzInfo = TimeZoneInfo.Local;
            int earlyCheckinMinutes = 15;
            DateTime testDayTime = DateTime.Now.AddMinutes(-5).ToMinutePrecision();
            DateTime earlyCheckInDayTime = testDayTime.AddMinutes(earlyCheckinMinutes * -1);
            DateTime expected = testDayTime.AddDays(7);

            //Create a daily schedule for ONLY the test day and time
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule();
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;

            //Act
            DateTime? actual = cut.CalculateNextCheckIn(tzInfo);

            //Assert
            Assert.IsTrue(actual.HasValue, "DailySchedule.CalculateNextCheckIn should not return null for this test.");
            Assert.AreEqual(expected, actual.Value);
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ReturnsTomorrow_WhenUserLocalCurrentTimeIsWithinCheckInWindow()
        {
            //Arrange
            TimeZoneInfo tzInfo = TimeZoneInfo.Local;
            int earlyCheckinMinutes = 60;
            DateTime testDayTime = DateTime.Now.AddMinutes(30).ToMinutePrecision();     //Half hour from now
            DateTime earlyCheckInDayTime = testDayTime.AddMinutes(earlyCheckinMinutes * -1);
            DateTime expected = testDayTime.AddDays(1);

            //Create a daily schedule for the test day and following day
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule();
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;
            cut.Days[testDayTime.AddDays(1).DayOfWeek] = true;

            //Act
            DateTime? actual = cut.CalculateNextCheckIn(tzInfo);

            //Assert
            Assert.IsTrue(actual.HasValue, "DailySchedule.CalculateNextCheckIn should not return null for this test.");
            Assert.AreEqual(expected, actual.Value);
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ReturnsTomorrow_WhenUserLocalCurrentTimeIsAfterCheckInTime()
        {
            //Arrange
            TimeZoneInfo tzInfo = TimeZoneInfo.Local;
            int earlyCheckinMinutes = 15;
            DateTime testDayTime = DateTime.Now.AddMinutes(-30).ToMinutePrecision();     //Half hour prior to now
            DateTime earlyCheckInDayTime = testDayTime.AddMinutes(earlyCheckinMinutes * -1);
            DateTime expected = testDayTime.AddDays(1);

            //Create a daily schedule for the test day and following day
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule();
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;
            cut.Days[testDayTime.AddDays(1).DayOfWeek] = true;

            //Act
            DateTime? actual = cut.CalculateNextCheckIn(tzInfo);

            //Assert
            Assert.IsTrue(actual.HasValue, "DailySchedule.CalculateNextCheckIn should not return null for this test.");
            Assert.AreEqual(expected, actual.Value);
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ReturnsToday_WhenUserLocalCurrentTimeIsPriorToCheckInWindow()
        {
            //Arrange
            TimeZoneInfo tzInfo = TimeZoneInfo.Local;
            int earlyCheckinMinutes = 15;
            DateTime testDayTime = DateTime.Now.AddMinutes(30).ToMinutePrecision();     //Half hour from now
            DateTime earlyCheckInDayTime = testDayTime.AddMinutes(earlyCheckinMinutes * -1);
            DateTime expected = testDayTime;

            //Create a daily schedule for the test day and following day
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule();
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;
            cut.Days[testDayTime.AddDays(1).DayOfWeek] = true;

            //Act
            DateTime? actual = cut.CalculateNextCheckIn(tzInfo);

            //Assert
            Assert.IsTrue(actual.HasValue, "DailySchedule.CalculateNextCheckIn should not return null for this test.");
            Assert.AreEqual(expected, actual.Value);
        }

        [TestMethod]
        public void DailyScheduleCalculateNextCheckIn_ReturnsDayAfterTomorrow_WhenCheckInTimeIsTomorrowButUserLocalCurrentTimeIsWithinCheckInWindow()
        {
            //Arrange
            TimeZoneInfo tzInfo = TimeZoneInfo.Local;
            DateTime testDayTime = DateTime.Today.AddDays(1).AddHours(2).ToMinutePrecision();     //Tomorrow at 2 am
            DateTime earlyCheckInDayTime = DateTime.Now.AddMinutes(-15).ToMinutePrecision();
            DateTime expected = testDayTime.AddDays(1);

            //Create a daily schedule for the test day and following day
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule();
            cut.CheckInTime = new TimeSpan(testDayTime.Hour, testDayTime.Minute, 0);
            cut.CheckInWindowStartTime = new TimeSpan(earlyCheckInDayTime.Hour, earlyCheckInDayTime.Minute, 0);
            cut.Days[testDayTime.DayOfWeek] = true;
            cut.Days[testDayTime.AddDays(1).DayOfWeek] = true;

            //Act
            DateTime? actual = cut.CalculateNextCheckIn(tzInfo);

            //Assert
            Assert.IsTrue(actual.HasValue, "DailySchedule.CalculateNextCheckIn should not return null for this test.");
            Assert.AreEqual(expected, actual.Value);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsNever_WhenNoDaysAreSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            const string expected = "Never";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when no days are selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsWeekends_WhenSatSunAreSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Sunday = true;
            cut.Saturday = true;
            const string expected = "Weekends";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Saturday and Sunday are selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(actual.StartsWith(expected, StringComparison.InvariantCultureIgnoreCase), testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsWeekdays_WhenMthruFAreSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);
            cut.Sunday = false;
            cut.Saturday = false;
            const string expected = "Weekdays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when all days except Saturday and Sunday are selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(actual.StartsWith(expected, StringComparison.InvariantCultureIgnoreCase), testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsDaily_WhenAllDaysAreSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);
            const string expected = "Daily";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when all days are selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(actual.StartsWith(expected, StringComparison.InvariantCultureIgnoreCase), testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsSundays_WhenOnlySundayIsSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Sunday = true;
            const string expected = "Sundays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Sunday is selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsMondays_WhenOnlyMondayIsSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Monday = true;
            const string expected = "Mondays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Monday is selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsTuesdays_WhenOnlyTuesdayIsSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Tuesday = true;
            const string expected = "Tuesdays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Tuesday is selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsWednesdays_WhenOnlyWednesdayIsSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Wednesday = true;
            const string expected = "Wednesdays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Wednesday is selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsThursdays_WhenOnlyThursdayIsSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Thursday = true;
            const string expected = "Thursdays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Thursday is selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsFridays_WhenOnlyFridayIsSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Friday = true;
            const string expected = "Fridays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Friday is selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsSaturdays_WhenOnlySaturdayIsSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Saturday = true;
            const string expected = "Saturdays";
            string testFailureMsg = string.Format("DailySchedule.Description should return \"{0}\" when only Saturday is selected.", expected);

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0, testFailureMsg);
        }

        [TestMethod]
        public void DailyScheduleDescription_ReturnsUnambiguousAbbreviations_WhenTuesdayThursdaySaturdaySundayAreSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(false);
            cut.Tuesday = true;
            cut.Thursday = true;
            cut.Saturday = true;
            cut.Sunday = true;
            string testFailureMsgFormat = "DailySchedule.Description should return \"{0}\" when only Saturday is selected.";

            //Act
            string actual = cut.Description;

            //Assert
            Assert.IsTrue(actual.Contains("Tu"), string.Format(testFailureMsgFormat, "Tu"));
            Assert.IsTrue(actual.Contains("Th"), string.Format(testFailureMsgFormat, "Th"));
            Assert.IsTrue(actual.Contains("Sa"), string.Format(testFailureMsgFormat, "Sa"));
            Assert.IsTrue(actual.Contains("Su"), string.Format(testFailureMsgFormat, "Su"));
        }

        [TestMethod]
        public void DailyScheduleValidate_ReturnsMessage_WhenUserIdIsZero()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);
            cut.Name = "Test Schedule";

            const string expected = "The schedule UserId must be set";

            //Act
            var actual = cut.Validate();

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.AreEqual(expected, actual.First());
        }

        [TestMethod]
        public void DailyScheduleValidate_ReturnsMessage_WhenScheduleNameIsSpace()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true);
            cut.UserId = 1;
            cut.Name = " ";

            const string expected = "The schedule name cannot be empty";

            //Act
            var actual = cut.Validate();

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.AreEqual(expected, actual.First());
        }

        [TestMethod]
        public void DailyScheduleValidate_ReturnsMessage_WhenNoDaysAreSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule();
            cut.UserId = 1;
            cut.Name = "Test Schedule";

            const string expected = "At least one day must be selected";

            //Act
            var actual = cut.Validate();

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.AreEqual(expected, actual.First());
        }

        [TestMethod]
        public void DailyScheduleEqualDaysAndTime_ReturnsTrue_WhenDayAndTimesAreEqual()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true)
            {
                CheckInWindowStartTime = new TimeSpan(0, 7, 0, 0),
                CheckInTime = new TimeSpan(0, 9, 30, 0)
            };
            DeadManSwitch.Schedule.DailySchedule other = new Schedule.DailySchedule(true)
            {
                CheckInWindowStartTime = new TimeSpan(0, 7, 0, 0),
                CheckInTime = new TimeSpan(0, 9, 30, 0)
            };

            const bool expected = true;

            //Act
            var actual = cut.EqualDaysAndTime(other);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DailyScheduleEqualDaysAndTime_ReturnsFalse_WhenDifferentDaysAreSelected()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule()
            {
                Saturday = true,
                CheckInWindowStartTime = new TimeSpan(0, 7, 0, 0),
                CheckInTime = new TimeSpan(0, 9, 30, 0)
            };
            DeadManSwitch.Schedule.DailySchedule other = new Schedule.DailySchedule()
            {
                Tuesday = true, 
                CheckInWindowStartTime = new TimeSpan(0, 7, 0, 0),
                CheckInTime = new TimeSpan(0, 9, 30, 0)
            };

            const bool expected = false;

            //Act
            var actual = cut.EqualDaysAndTime(other);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DailyScheduleEqualDaysAndTime_ReturnsFalse_WhenCheckInStartTimesAreDifferent()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true)
            {
                CheckInWindowStartTime = new TimeSpan(0, 9, 0, 0),
                CheckInTime = new TimeSpan(0, 9, 30, 0)
            };
            DeadManSwitch.Schedule.DailySchedule other = new Schedule.DailySchedule(true)
            {
                CheckInWindowStartTime = new TimeSpan(0, 7, 0, 0),
                CheckInTime = new TimeSpan(0, 9, 30, 0)
            };

            const bool expected = false;

            //Act
            var actual = cut.EqualDaysAndTime(other);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DailyScheduleEqualDaysAndTime_ReturnsFalse_WhenCheckInTimesAreDifferent()
        {
            //Arrange
            DeadManSwitch.Schedule.DailySchedule cut = new Schedule.DailySchedule(true)
            {
                CheckInWindowStartTime = new TimeSpan(0, 7, 0, 0),
                CheckInTime = new TimeSpan(0, 8, 30, 0)
            };
            DeadManSwitch.Schedule.DailySchedule other = new Schedule.DailySchedule(true)
            {
                CheckInWindowStartTime = new TimeSpan(0, 7, 0, 0),
                CheckInTime = new TimeSpan(0, 9, 30, 0)
            };

            const bool expected = false;

            //Act
            var actual = cut.EqualDaysAndTime(other);

            //Assert
            Assert.AreEqual(expected, actual);
        }

    }
}
