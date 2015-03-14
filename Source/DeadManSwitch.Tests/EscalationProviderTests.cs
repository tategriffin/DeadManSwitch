using System;
using System.Collections.Generic;
using System.Linq;
using DeadManSwitch.Action;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using DeadManSwitch.Schedule;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class EscalationProviderTests
    {
        private IUnityContainer InitializeUserAndActions()
        {
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var userActionProvider = new UserEscalationProcedureProvider(container);

            userProvider.CreateAccount(
                new User("EscalationProviderTestUser", "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName("EscalationProviderTestUser");
            
            userActionProvider.Save(user, BuildEscalationProcedures(user));

            return container;
        }

        private EscalationProcedures BuildEscalationProcedures(User user)
        {
            int stepNumber = 0;
            var factory = new ActionFactory();
            
            var steps = new List<UserEscalationTask>();
            steps.Add(new UserEscalationTask() { UserId = user.UserId, Action = factory.CreateAction(ActionType.EmailMessage), ExecutionOrder = ++stepNumber, WaitTimeSpan = new TimeSpan(0, 0, 0) });
            steps.Add(new UserEscalationTask() { UserId = user.UserId, Action = factory.CreateAction(ActionType.TextMessage), ExecutionOrder = ++stepNumber, WaitTimeSpan = new TimeSpan(0, 5, 0) });
            
            return new EscalationProcedures(user.UserId, steps);
        }

        [TestMethod]
        public void EscalationProviderStartUserEscalationProcedures_SetsImmediateTriggerTime_WhenNoDelay()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("EscalationProviderTestUser");

            var cut = new EscalationProvider(container);

            //Act
            cut.StartUserEscalationProcedures(user.UserId);

            var repository = container.Resolve<IEscalationRepository>() as EscalationRepository;
            var actual = repository.FindByUserId(user.UserId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
            Assert.IsTrue(actual[0].Data.TriggerTime <= DateTime.UtcNow);
        }

        [TestMethod]
        public void EscalationProviderStartUserEscalationProcedures_SetsDelayedTriggerTime_WhenDelayIsSpecified()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("EscalationProviderTestUser");

            var cut = new EscalationProvider(container);

            //Act
            var delay = new TimeSpan(0, 1, 0);
            cut.StartUserEscalationProcedures(user.UserId, delay);

            var repository = container.Resolve<IEscalationRepository>() as EscalationRepository;
            var actual = repository.FindByUserId(user.UserId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 2);
            Assert.IsTrue(actual[0].Data.TriggerTime <= DateTime.UtcNow.Add(delay));
        }

        [TestMethod] public void EscalationProviderStopUserEscalationProcedures_RemovesEscalationRows_WhenCalled()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("EscalationProviderTestUser");

            var cut = new EscalationProvider(container);
            cut.StartUserEscalationProcedures(user.UserId);

            //Act
            cut.StopUserEscalationProcedures(user.UserId);

            var repository = container.Resolve<IEscalationRepository>() as EscalationRepository;
            var actual = repository.FindByUserId(user.UserId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 0);
        }

    }
}
