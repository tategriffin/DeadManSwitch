using System;
using System.Collections.Generic;
using DeadManSwitch.Action;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using DeadManSwitch.Schedule;
using ExternalServiceAdapters;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class MissedCheckInProviderTests
    {
        private void InitializeUnitTestData(IUnityContainer container, RepositoryContext context, string testUserName)
        {
            container.RegisterType<ICheckInRepository, Data.TestRepository.PriorDateTimeCheckInRepository>(new InjectionConstructor(context));

            var userProvider = new UserProvider(container);
            var scheduleProvider = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User(testUserName, testUserName + "@test.com", testUserName, testUserName),
                "1234"
                );

            var user = userProvider.FindByUserName(testUserName);

            scheduleProvider.SaveDailySchedule(user, BuildDailySchedule(user));
            var checkInProvider = new CheckInProvider(container);
            checkInProvider.RecordCheckIn(user);    //PriorDateTimeCheckInRepository

            var procedureProvider = new EscalationProvider(container);
            procedureProvider.StartUserEscalationProcedures(user.UserId);
        }

        private DailySchedule BuildDailySchedule(User user)
        {
            return new DailySchedule(true)
            {
                Name = "MissedCheckInProviderTestSchedule",
                CheckInWindowStartTime = DateTime.Now.AddMinutes(-30).TimeOfDay,
                CheckInTime = DateTime.Now.AddMinutes(-15).TimeOfDay
            };
        }

        [TestMethod]
        public void MissedCheckInProviderFindNextUnEscalatedMissedCheckIn_ReturnsNull_WhenNoMissedCheckIns()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());

            var cut = new MissedCheckInProvider(container);
            //Act
            var actual = cut.FindNextUnEscalatedMissedCheckIn();

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void MissedCheckInProviderFindNextUnEscalatedMissedCheckIn_ReturnsNonNull_WhenMissedCheckInExists()
        {
            //Arrange
            var context = new RepositoryContext();
            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            InitializeUnitTestData(container, context, "MissedCheckInProviderTestUser");

            var cut = new MissedCheckInProvider(container);
            //Act
            var actual = cut.FindNextUnEscalatedMissedCheckIn();

            //Assert
            Assert.IsNull(actual);
        }

    }
}
