using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public class MissedCheckInProcessorTests
    {
        private void InitializeUnitTestData(IUnityContainer container, RepositoryContext context, string testUserName)
        {
            container.RegisterType<ICheckInRepository, Data.TestRepository.PriorDateTimeCheckInRepository>(new InjectionConstructor(context));

            container.RegisterType<ISendEmailAdapter, Fakes.SendEmailAdapterFake>();
            container.RegisterType<ISendSMSAdapter, Fakes.SendSMSAdapterFake>();
            container.RegisterType<IApplicationSettingsRepository, Data.TestRepository.ApplicationSettingsRepository>(new InjectionConstructor(context));
            container.RegisterType<IAccountRepository, Data.TestRepository.AccountRepository>(new InjectionConstructor(context));
            container.RegisterType<IUserEscalationProcedureRepository, Data.TestRepository.UserEscalationProcedureRepository>(new InjectionConstructor(context));
            container.RegisterType<IScheduleRepository, Data.TestRepository.ScheduleRepository>(new InjectionConstructor(context));
            container.RegisterType<IDailyScheduleRepository, Data.TestRepository.DailyScheduleRepository>(new InjectionConstructor(context));
            container.RegisterType<IUserPreferenceRepository, Data.TestRepository.UserPreferenceRepository>(new InjectionConstructor(context));
            container.RegisterType<IReferenceDataRepository, Data.TestRepository.ReferenceDataRepository>(new InjectionConstructor(context));
            container.RegisterType<IKillSwitchRepository, Data.TestRepository.KillSwitchRepository>(new InjectionConstructor(context));

            var userProvider = new UserProvider(container);
            var userActionProvider = new UserEscalationProcedureProvider(container);
            var scheduleProvider = new DailyScheduleProvider(container);

            userProvider.CreateAccount(
                new User(testUserName, testUserName + "@test.com", testUserName, testUserName),
                "1234"
                );

            var user = userProvider.FindByUserName(testUserName);

            userActionProvider.Save(user, BuildEscalationProcedures(user));
            scheduleProvider.SaveDailySchedule(user, BuildDailySchedule(user));
        }

        private EscalationProcedures BuildEscalationProcedures(User user)
        {
            int stepNumber = 0;
            var factory = new ActionFactory();
            var action = factory.CreateAction(ActionType.EmailMessage);
            action.Message = "Test message";
            action.Recipient = user.Email;

            var steps = new List<UserEscalationTask>();
            steps.Add(new UserEscalationTask() { UserId = user.UserId, Action = action, ExecutionOrder = ++stepNumber, WaitTimeSpan = new TimeSpan(0, 0, 0) });

            return new EscalationProcedures(user.UserId, steps);
        }

        private DailySchedule BuildDailySchedule(User user)
        {
            return new DailySchedule(true)
            {
                Name = "MissedCheckInProcessorTestSchedule",
                CheckInWindowStartTime = DateTime.Now.AddMinutes(-30).TimeOfDay,
                CheckInTime = DateTime.Now.AddMinutes(-15).TimeOfDay
            };
        }

        /// <summary>
        /// MissedCheckInProcessor.Execute will only allow one instance to execute at a time.
        /// This is the behavior we want for production, but for these tests, the 
        /// escalation repositories are isolated per test, so we need to call execute for each test.
        /// Tests are multi-threaded, so one Execute may not complete before the next test calls Execute.
        /// When that happens, Execute is skipped, and the expected escalation items are not added
        /// to the repository, causing the test to fail.
        /// </summary>
        private void RetryExecute(MissedCheckInProcessor cut, EscalationRepository escalationRepository)
        {
            int runawayCount = 0;
            do
            {
                int milliseconds = new Random().Next(5, 10);
                Thread.Sleep(milliseconds);

                cut.Execute();
            } while (escalationRepository.All().Count == 0 && ++runawayCount < 500);
        }

        [TestMethod]
        public void MissedCheckInProcessorExecute_StartsEscalationProcedures_WhenUserMissedCheckIn()
        {
            //Arrange
            const string testUserName = "MissedCheckInProcessorSuccessTestUser";
            var context = new RepositoryContext();
            IUnityContainer container = new UnityContainer();
            var escalationRepository = new EscalationRepository(context);
            container.RegisterInstance<IEscalationRepository>(escalationRepository);

            InitializeUnitTestData(container, context, testUserName);

            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName(testUserName);
            var checkInProvider = new CheckInProvider(container);
            checkInProvider.RecordCheckIn(user);

            var cut = new MissedCheckInProcessor(container);
            //Act
            RetryExecute(cut, escalationRepository);

            //Assert
            Assert.AreEqual(1, escalationRepository.All().Count, "EscalationWorkTable contains unexpected number of rows");
            var workItemRows = escalationRepository.FindByUserId(user.UserId);
            Assert.IsNotNull(workItemRows);
            Assert.IsTrue(workItemRows.Count == 1, "No Escalation Work Items found for user");
        }

    }
}
