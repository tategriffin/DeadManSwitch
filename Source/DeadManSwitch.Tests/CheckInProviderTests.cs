using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeadManSwitch;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using DeadManSwitch.Schedule;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class CheckInProviderTests
    {
        [TestMethod]
        public void CheckInProviderFindLastCheckIn_ReturnsNull_WhenCheckInIsNotFound()
        {
            //Arrange
            User fakeUser = new User() { UserId = 6007 };
            DateTime checkInTime = DateTime.UtcNow.AddHours(-2);
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());

            CheckInProvider cut = new CheckInProvider(container);

            //Act
            var actual = cut.FindLastCheckIn(fakeUser);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CheckInProviderRecalculateNextCheckInForUser_ReturnsNonNullDateTime_WhenValidUser()
        {
            //Arrange
            const string userName = "RecalculateNextCheckInTestUser";
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User(userName, "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName(userName);
            scheduleProvider.SaveDailySchedule(
                user,
                new DailySchedule(true)
                {
                    Name = "RecalculateNextCheckInTestSchedule",
                    CheckInWindowStartTime = DateTime.Now.AddMinutes(15).TimeOfDay,
                    CheckInTime = DateTime.Now.AddMinutes(30).TimeOfDay
                }
                );

            CheckInProvider cut = new CheckInProvider(container);

            //Act
            var actual = cut.RecalculateNextCheckInForUser(user);

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void CheckInProviderRecordCheckIn_ReturnsToday_WhenPriorToCheckInWindow()
        {
            //Arrange
            const string userName = "RecordCheckInTestUser";
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User(userName, "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName(userName);
            scheduleProvider.SaveDailySchedule(
                user,
                new DailySchedule(true)
                {
                    Name = "RecordCheckInTestSchedule",
                    CheckInWindowStartTime = DateTime.Now.AddMinutes(15).TimeOfDay,
                    CheckInTime = DateTime.Now.AddMinutes(30).TimeOfDay
                }
                );

            CheckInProvider cut = new CheckInProvider(container);

            //Act
            var expected = DateTime.UtcNow.Date;
            var actual = cut.RecordCheckIn(user);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.NextCheckInTimeUtc);
            Assert.AreEqual(expected, actual.NextCheckInTimeUtc.Value.Date, "Expected next check in date to be today.");
        }

        [TestMethod]
        public void CheckInProviderRecordCheckIn_ReturnsTomorrow_WhenInCheckInWindow()
        {
            //Arrange
            const string userName = "RecordCheckInTestUser";
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User(userName, "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName(userName);
            scheduleProvider.SaveDailySchedule(
                user,
                new DailySchedule(true)
                {
                    Name = "RecordCheckInTestSchedule",
                    CheckInWindowStartTime = DateTime.Now.AddMinutes(-15).TimeOfDay,
                    CheckInTime = DateTime.Now.AddMinutes(15).TimeOfDay
                }
                );

            CheckInProvider cut = new CheckInProvider(container);

            //Act
            var expected = DateTime.UtcNow.Date.AddDays(1);
            var actual = cut.RecordCheckIn(user);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.NextCheckInTimeUtc);
            Assert.AreEqual(expected, actual.NextCheckInTimeUtc.Value.Date, "Expected next check in date to be tomorrow.");
        }

    }
}
