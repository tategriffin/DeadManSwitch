using System;
using System.Collections.Generic;
using System.Linq;
using DeadManSwitch.Action;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class UserEscalationProcedureProviderTests
    {
        private IUnityContainer InitializeUserAndActions()
        {
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var userActionProvider = new UserEscalationProcedureProvider(container);

            userProvider.CreateAccount(
                new User("UserEscalationProcedureProviderTestUser", "x@example.com", "test", "user"),
                "1234"
                );

            var user = userProvider.FindByUserName("UserEscalationProcedureProviderTestUser");

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
        public void UserEscalationProcedureProviderFindByTaskId_ReturnsNonNull_WhenTaskIdExists()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("UserEscalationProcedureProviderTestUser");

            var cut = new UserEscalationProcedureProvider(container);
            var procedures = cut.FindProceduresByUserId(user.UserId);

            //Act
            var actual = cut.FindTaskById(user.UserId, procedures.EscalationList.First().Id);

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void UserEscalationProcedureProviderFindByTaskId_ReturnsNull_WhenTaskIdDoesNotExist()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("UserEscalationProcedureProviderTestUser");

            var cut = new UserEscalationProcedureProvider(container);

            //Act
            var actual = cut.FindTaskById(user.UserId, Int32.MaxValue);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod] 
        public void UserEscalationProcedureProviderSave_UpdatesTask_WhenCalled()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("UserEscalationProcedureProviderTestUser");

            var cut = new UserEscalationProcedureProvider(container);
            var procedures = cut.FindProceduresByUserId(user.UserId);
            var task = procedures.EscalationList.First();
            var originalValue = task.WaitTimeSpan;

            //Act
            task.WaitTimeSpan = task.WaitTimeSpan.Add(new TimeSpan(0, 0, 5));
            cut.Save(user, task);
            var actual = cut.FindTaskById(user.UserId, task.Id);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreNotEqual(originalValue, actual.WaitTimeSpan);
        }

        [TestMethod]
        public void UserEscalationProcedureProviderDelete_RemovesTask_WhenCalledWithId()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("UserEscalationProcedureProviderTestUser");

            var cut = new UserEscalationProcedureProvider(container);
            var procedures = cut.FindProceduresByUserId(user.UserId);
            var task = procedures.EscalationList.Last();

            //Act
            cut.Delete(user, task.Id);
            var actual = cut.FindTaskById(user.UserId, task.Id);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void UserEscalationProcedureProviderDelete_RemovesTask_WhenCalledWithTask()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("UserEscalationProcedureProviderTestUser");

            var cut = new UserEscalationProcedureProvider(container);
            var procedures = cut.FindProceduresByUserId(user.UserId);
            var task = procedures.EscalationList.Last();

            //Act
            cut.Delete(user, task);
            var actual = cut.FindTaskById(user.UserId, task.Id);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void UserEscalationProcedureProviderReorderSteps_Succeeds_WhenCalled()
        {
            //Arrange
            IUnityContainer container = InitializeUserAndActions();
            var userProvider = new UserProvider(container);
            var user = userProvider.FindByUserName("UserEscalationProcedureProviderTestUser");

            var cut = new UserEscalationProcedureProvider(container);
            var procedures = cut.FindProceduresByUserId(user.UserId);
            var order = procedures.EscalationList.Select(task => task.Id).ToList();

            //Act
            order.Reverse();
            cut.ReorderSteps(user, order);
            var actual = cut.FindProceduresByUserId(user.UserId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreNotEqual(procedures.EscalationList.First().Id, actual.EscalationList.First().Id);
            Assert.AreNotEqual(procedures.EscalationList.Last().Id, actual.EscalationList.Last().Id);
        }

    }
}
