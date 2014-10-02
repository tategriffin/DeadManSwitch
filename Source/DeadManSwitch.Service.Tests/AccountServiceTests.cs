using System;
using DeadManSwitch.Data.TestRepository.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeadManSwitch.Service;
using DeadManSwitch.Data.TestRepository;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace DeadManSwitch.Service.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        [TestMethod]
        public void AccountService_IsRegistrationOpen_ReturnsTrueWhenRegistrationIsOpen()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "1";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            bool actual = cut.IsRegistrationOpen();

            //Assert
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void AccountService_IsRegistrationOpen_ReturnsFalseWhenRegistrationIsClosed()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "0";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            bool actual = cut.IsRegistrationOpen();

            //Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void AccountService_UserNameExists_ReturnsTrueWhenNameExists()
        {
            //Arrange
            string existingUserName = "NameThatAlreadyExists";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(existingUserName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            bool result = cut.UserNameExists(existingUserName);

            //Assert
            Assert.IsTrue(result, "AccountService.UserNameExists did not recognize an existing user name.");
        }

        [TestMethod]
        public void AccountService_UserNameExists_ReturnsFalseWhenNameDoesNotExist()
        {
            //Arrange
            string existingUserName = "NameThatAlreadyExists";
            string newUserName = "NotANameThatAlreadyExists";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(existingUserName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            bool result = cut.UserNameExists(newUserName);

            //Assert
            Assert.IsFalse(result, "AccountService.UserNameExists incorrectly identified a user name as an existing user name.");
        }

        [TestMethod]
        public void AccountService_RegisterUser_ThrowsExceptionWhenUserIsNull()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "1";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            User fakeUser = null;
            string password = "1234";

            //Act
            try
            {
                var result = cut.RegisterUser(fakeUser, password);

                //Assert
                Assert.Fail("AccountService.RegisterUser should throw exception when user parameter is null.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_RegisterUser_ThrowsExceptionWhenPasswordIsNull()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "1";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            User fakeUser = new User();
            string password = null;

            //Act
            try
            {
                var result = cut.RegisterUser(fakeUser, password);

                //Assert
                Assert.Fail("AccountService.RegisterUser should throw exception when password parameter is null.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }


        [TestMethod]
        public void AccountService_RegisterUser_ThrowsExceptionWhenPasswordIsEmpty()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "1";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            User fakeUser = new User();
            string password = string.Empty;

            //Act
            try
            {
                var result = cut.RegisterUser(fakeUser, password);

                //Assert
                Assert.Fail("AccountService.RegisterUser should throw exception when password parameter is empty.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_RegisterUser_ThrowsExceptionWhenPasswordIsWhitespace()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "1";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            User fakeUser = new User();
            string password = " ";

            //Act
            try
            {
                var result = cut.RegisterUser(fakeUser, password);

                //Assert
                Assert.Fail("AccountService.RegisterUser should throw exception when password parameter is whitespace.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_RegisterUser_DoesNotThrowExceptionWhenRegistrationIsClosed()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "0";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            User fakeUser = new User() { UserName = "testRegistrationClosed", Email = "user@example.com", FirstName = "test", LastName = "user" };
            string password = "1234";

            //Act
            try
            {
                var result = cut.RegisterUser(fakeUser, password);

            }
            catch (Exception)
            {
                //Assert
                Assert.Fail("AccountService.RegisterUser not throw exception when registration is closed.");
            }

        }

        [TestMethod]
        public void AccountService_RegisterUser_ReturnsOneMessageWhenRegistrationIsClosed()
        {
            //Arrange
            var context = new RepositoryContext();
            context.ApplicationSettings["AllowNewUserAccounts"] = "0";

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            User fakeUser = new User() { UserName = "testRegistrationClosed", Email = "user@example.com", FirstName = "test", LastName = "user" };
            string password = "1234";

            //Act
            IEnumerable<string> result = cut.RegisterUser(fakeUser, password);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void AccountService_Login_ThrowsExceptionWhenUserNameIsNull()
        {
            //Arrange
            string userName = "anyUser";
            string password = "1234";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(userName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            try
            {
                var result = cut.Login(null, password);

                //Assert
                Assert.Fail("AccountService.Login should throw exception when userName parameter is null.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_Login_ThrowsExceptionWhenUserNameIsEmpty()
        {
            //Arrange
            string userName = "anyUser";
            string password = "1234";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(userName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            try
            {
                var result = cut.Login(string.Empty, password);

                //Assert
                Assert.Fail("AccountService.Login should throw exception when userName parameter is empty.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_Login_ThrowsExceptionWhenUserNameIsWhitespace()
        {
            //Arrange
            string userName = "anyUser";
            string password = "1234";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(userName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            try
            {
                var result = cut.Login(" ", password);

                //Assert
                Assert.Fail("AccountService.Login should throw exception when userName parameter is whitespace.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_Login_ThrowsExceptionWhenPasswordIsNull()
        {
            //Arrange
            string userName = "anyUser";
            string password = "1234";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(userName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            try
            {
                var result = cut.Login(userName, null);

                //Assert
                Assert.Fail("AccountService.Login should throw exception when password parameter is null.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_Login_ThrowsExceptionWhenPasswordIsEmpty()
        {
            //Arrange
            string userName = "anyUser";
            string password = "1234";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(userName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            try
            {
                var result = cut.Login(userName, string.Empty);

                //Assert
                Assert.Fail("AccountService.Login should throw exception when password parameter is empty.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

        [TestMethod]
        public void AccountService_Login_ThrowsExceptionWhenPasswordIsWhitespace()
        {
            //Arrange
            string userName = "anyUser";
            string password = "1234";
            var context = new RepositoryContext();
            var user = new DeadManSwitch.User(userName, "name@example.com", "first", "last");
            var userRecord = new UserAccountTableRow(user, "password");
            context.UserAccounts.Add(userRecord);

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            AccountService cut = new AccountService(container);

            //Act
            try
            {
                var result = cut.Login(userName, " ");

                //Assert
                Assert.Fail("AccountService.Login should throw exception when password parameter is whitespace.");
            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }

        }

    }
}
