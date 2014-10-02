using System;
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

    }
}
