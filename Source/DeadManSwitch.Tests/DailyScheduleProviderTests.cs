using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using DeadManSwitch.Schedule;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeadManSwitch.Tests
{
    /// <summary>
    /// Test DailyScheduleProvider
    /// </summary>
    [TestClass]
    public class DailyScheduleProviderTests
    {
        [TestMethod]
        public void DailyScheduleProviderFindDailySchedule_ReturnsNonNullSchedule_WhenAuthorizedUserExecutesFind()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new ScheduleProvider(container);
            var cut = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User("authUser", "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName("authUser");
            cut.SaveDailySchedule(
                user,
                new DailySchedule(true)
                {
                    Name = "FindDailyScheduleTestSchedule",
                    CheckInWindowStartTime = DateTime.Now.AddMinutes(-15).TimeOfDay,
                    CheckInTime = DateTime.Now.AddMinutes(15).TimeOfDay
                }
                );

            var addedSchedule = scheduleProvider
                .GetAllUserSchedules(user)
                .Single(s => s.Name == "FindDailyScheduleTestSchedule");

            //Act
            var actual = cut.FindDailySchedule(user, addedSchedule.Id);

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void DailyScheduleProviderFindDailySchedule_ReturnsNullSchedule_WhenUnauthorizedUserExecutesFind()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new ScheduleProvider(container);
            var cut = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User("authUser", "x@example.com", "test", "user"),
                "1234"
                );
            userProvider.CreateAccount(
                new User("unauthUser", "x@example.com", "test", "user"),
                "1234"
                );

            var unauthUser = userProvider.FindByUserName("unauthUser");
            var authUser = userProvider.FindByUserName("authUser");
            cut.SaveDailySchedule(
                authUser,
                new DailySchedule(true)
                {
                    Name = "FindDailyScheduleTestSchedule",
                    CheckInWindowStartTime = DateTime.Now.AddMinutes(-15).TimeOfDay,
                    CheckInTime = DateTime.Now.AddMinutes(15).TimeOfDay
                }
                );

            var addedSchedule = scheduleProvider
                .GetAllUserSchedules(authUser)
                .Single(s => s.Name == "FindDailyScheduleTestSchedule");

            //Act
            var actual = cut.FindDailySchedule(unauthUser, addedSchedule.Id);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void DailyScheduleProviderFindDailySchedule_ReturnsNullSchedule_WhenScheduleIsNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var cut = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User("authUser", "x@example.com", "test", "user"),
                "1234"
                );

            var authUser = userProvider.FindByUserName("authUser");

            //Act
            var actual = cut.FindDailySchedule(authUser, Int32.MaxValue);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void DailyScheduleProviderDeleteSchedule_DeletesSchedule_WhenAuthorizedUserExecutesDelete()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new ScheduleProvider(container);
            var cut = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User("authUser", "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName("authUser");
            cut.SaveDailySchedule(
                user,
                new DailySchedule(true)
                {
                    Name = "FindDailyScheduleTestSchedule",
                    CheckInWindowStartTime = DateTime.Now.AddMinutes(-15).TimeOfDay,
                    CheckInTime = DateTime.Now.AddMinutes(15).TimeOfDay
                }
                );

            var addedSchedule = scheduleProvider
                .GetAllUserSchedules(user)
                .Single(s => s.Name == "FindDailyScheduleTestSchedule");

            //Act
            cut.DeleteSchedule(user, addedSchedule.Id);
            var actual = cut.FindDailySchedule(user, addedSchedule.Id);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void DailyScheduleProviderDeleteSchedule_ThrowsEx_WhenUnauthorizedUserExecutesDelete()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new ScheduleProvider(container);
            var cut = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User("authUser", "x@example.com", "test", "user"),
                "1234"
                );
            userProvider.CreateAccount(
                new User("unauthUser", "x@example.com", "test", "user"),
                "1234"
                );

            var unauthUser = userProvider.FindByUserName("unauthUser");
            var authUser = userProvider.FindByUserName("authUser");
            cut.SaveDailySchedule(
                authUser,
                new DailySchedule(true)
                {
                    Name = "FindDailyScheduleTestSchedule",
                    CheckInWindowStartTime = DateTime.Now.AddMinutes(-15).TimeOfDay,
                    CheckInTime = DateTime.Now.AddMinutes(15).TimeOfDay
                }
                );

            var addedSchedule = scheduleProvider
                .GetAllUserSchedules(authUser)
                .Single(s => s.Name == "FindDailyScheduleTestSchedule");

            try
            {
                //Act
                cut.DeleteSchedule(unauthUser, addedSchedule.Id);

                //Assert
                Assert.Fail("Unauthorized user should not be able to delete schedule.");
            }
            catch (Exception)
            {
                //Test passes
            }
        }

        [TestMethod]
        public void DailyScheduleProviderValidate_ReturnsMessage_WhenSchedulesOverlap()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var scheduleProvider = new ScheduleProvider(container);
            var cut = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User("authUser", "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName("authUser");

            var start = DateTime.Now.AddMinutes(15).TimeOfDay;
            var end = DateTime.Now.AddMinutes(45).TimeOfDay;

            cut.SaveDailySchedule(
                user,
                new DailySchedule(true)
                {
                    Name = "FindDailyScheduleTestSchedule",
                    UserId = user.UserId,
                    CheckInWindowStartTime = start,
                    CheckInTime = end
                }
                );

            var overlappingSchedule = new DailySchedule(true)
            {
                Name = "OverlappingSchedule",
                UserId = user.UserId,
                CheckInWindowStartTime = start,
                CheckInTime = end,
            };

            //Act
            var actual = cut.ValidateDailySchedule(overlappingSchedule);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 1);
            Assert.IsTrue(actual.First().Contains("same as existing"));
        }

    }
}
