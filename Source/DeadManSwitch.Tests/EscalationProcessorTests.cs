using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DeadManSwitch.Action;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using DeadManSwitch.Schedule;
using DeadManSwitch.Tests.Fakes;
using ExternalServiceAdapters;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class EscalationProcessorTests
    {
        private void InitializeUnitTestData(IUnityContainer container, RepositoryContext context, string testUserName)
        {
            container.RegisterType<ICheckInRepository, IgnoreCheckInRepository>();
            container.RegisterType<ISendSMSAdapter, Fakes.SendSMSAdapterFake>();
            container.RegisterType<IApplicationSettingsRepository, Data.TestRepository.ApplicationSettingsRepository>(new InjectionConstructor(context));
            container.RegisterType<ICheckInRepository, Data.TestRepository.CheckInRepository>(new InjectionConstructor(context));
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
                Name = "EscalationProcessorTestSchedule",
                CheckInWindowStartTime = DateTime.Now.AddMinutes(-30).TimeOfDay,
                CheckInTime = DateTime.Now.AddMinutes(-15).TimeOfDay
            };
        }

        /// <summary>
        /// EscalationProcessor.Execute will only allow one instance to execute at a time.
        /// This is the behavior we want for production, but for these tests, the 
        /// escalation repositories are isolated per test, so we need to call execute for each test.
        /// Tests are multi-threaded, so one Execute may not complete before the next test calls Execute.
        /// When that happens, Execute is skipped, and the expected escalation items are not added
        /// to the repository, causing the test to fail.
        /// </summary>
        private void RetryExecute(EscalationProcessor cut, EscalationRepository escalationRepository)
        {
            do
            {
                int milliseconds = new Random().Next(5, 10);
                Thread.Sleep(milliseconds);

                cut.Execute();
            } while (escalationRepository.All().Count == 0);
        }

        [TestMethod]
        public void EscalationProcessorExecute_CallsEscalationProiderRecordActionSuccess_WhenActionSucceeds()
        {
            //Arrange
            const string testUserName = "EscalationProcessorSuccessTestUser";
            var context = new RepositoryContext();
            IUnityContainer container = new UnityContainer();
            var escalationRepository = new EscalationRepository(context);
            container.RegisterInstance<IEscalationRepository>(escalationRepository);
            container.RegisterType<ISendEmailAdapter, Fakes.SendEmailAdapterFake>();

            InitializeUnitTestData(container, context, testUserName);

            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName(testUserName);
            var procedureProvider = new EscalationProvider(container);
            procedureProvider.StartUserEscalationProcedures(user.UserId);

            var cut = new EscalationProcessor(container);
            //Act
            RetryExecute(cut, escalationRepository);

            //Assert
            var workItemRows = escalationRepository.FindByUserId(user.UserId);
            Assert.AreEqual(1, escalationRepository.All().Count);
            Assert.IsNotNull(workItemRows);
            Assert.IsTrue(workItemRows.Count == 1);
            var firstRow = workItemRows.First();
            Assert.AreEqual(0, firstRow.NumberOfFailures, firstRow.ToString());
            Assert.IsTrue(firstRow.Success.HasValue, firstRow.ToString());
            Assert.IsTrue(firstRow.Success.Value, firstRow.ToString());
        }

        [TestMethod]
        public void EscalationProcessorExecute_CallsEscalationProiderRecordActionFailure_WhenActionFails()
        {
            //Arrange
            const string testUserName = "EscalationProcessorFailTestUser";
            var context = new RepositoryContext();
            IUnityContainer container = new UnityContainer();
            var escalationRepository = new EscalationRepository(context);
            container.RegisterInstance<IEscalationRepository>(escalationRepository);
            container.RegisterType<ISendEmailAdapter, SendEmailAlwaysFailsAdapterFake>();

            InitializeUnitTestData(container, context, testUserName);

            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName(testUserName);
            var procedureProvider = new EscalationProvider(container);
            procedureProvider.StartUserEscalationProcedures(user.UserId);

            var cut = new EscalationProcessor(container);
            //Act
            RetryExecute(cut, escalationRepository);

            //Assert
            var workItemRows = escalationRepository.FindByUserId(user.UserId);
            Assert.AreEqual(1, escalationRepository.All().Count);
            Assert.IsNotNull(workItemRows);
            Assert.IsTrue(workItemRows.Count == 1);
            var firstRow = workItemRows.First();
            Assert.AreNotEqual(0, firstRow.NumberOfFailures, firstRow.ToString());
            Assert.IsFalse(firstRow.Success.HasValue, firstRow.ToString());
        }

        [TestMethod]
        public void EscalationProcessorExecute_CallsEscalationProiderRecordActionFailure_WhenActionThrowsException()
        {
            //Arrange
            const string testUserName = "EscalationProcessorExceptionTestUser";
            var context = new RepositoryContext();
            IUnityContainer container = new UnityContainer();
            var escalationRepository = new EscalationRepository(context);
            container.RegisterInstance<IEscalationRepository>(escalationRepository);
            container.RegisterType<ISendEmailAdapter, SendEmailThrowsExAdapterFake>();

            InitializeUnitTestData(container, context, testUserName);

            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName(testUserName);
            var procedureProvider = new EscalationProvider(container);
            procedureProvider.StartUserEscalationProcedures(user.UserId);

            var cut = new EscalationProcessor(container);
            //Act
            RetryExecute(cut, escalationRepository);

            //Assert
            var workItemRows = escalationRepository.FindByUserId(user.UserId);
            Assert.AreEqual(1, escalationRepository.All().Count);
            Assert.IsNotNull(workItemRows);
            Assert.IsTrue(workItemRows.Count == 1);
            var firstRow = workItemRows.First();
            Assert.AreNotEqual(0, firstRow.NumberOfFailures, firstRow.ToString());
            Assert.IsFalse(firstRow.Success.HasValue, firstRow.ToString());
        }
    }
}
