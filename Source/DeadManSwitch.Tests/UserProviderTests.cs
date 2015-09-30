using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using DeadManSwitch.Data.TestRepository.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeadManSwitch;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class UserProviderTests
    {
        [TestMethod]
        public void UserProviderCreateAccount_CreatesUser_WhenAllFieldsAreValid()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            string userName = Stopwatch.GetTimestamp().ToString();  //some unique user name

            User user = new User(userName, "x@example.com", "Test", "User");
            string pwd = "1234";

            //Act
            cut.CreateAccount(user, pwd);
            var actual = cut.FindByUserName(userName);

            //Assert
            Assert.IsNotNull(actual, "CreateAccount should create a new user.");
        }

        [TestMethod]
        public void UserProviderCreateAccount_ThrowsEx_WhenUserIsNotZero()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            User user = new User() { UserId = 7007 };
            string pwd = "1234";

            try
            {
                //Act
                cut.CreateAccount(user, pwd);

                //Assert
                Assert.Fail("CreateAccount should throw ArgumentException when user.userId is not 0.");

            }
            catch (ArgumentException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderCreateAccount_ThrowsEx_WhenPasswordIsNull()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            User user = new User();
            string pwd = null;

            try
            {
                //Act
                cut.CreateAccount(user, pwd);

                //Assert
                Assert.Fail("CreateAccount should throw ArgumentNullException when password is null.");

            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderCreateAccount_ThrowsEx_WhenPasswordIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            
            User user = new User();
            string pwd = string.Empty;

            try
            {
                //Act
                cut.CreateAccount(user, pwd);

                //Assert
                Assert.Fail("CreateAccount should throw ArgumentNullException when password is empty.");

            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderCreateAccount_ThrowsEx_WhenPasswordIsWhiteSpace()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            User user = new User();
            string pwd = "   ";

            try
            {
                //Act
                cut.CreateAccount(user, pwd);

                //Assert
                Assert.Fail("CreateAccount should throw ArgumentNullException when password is white space.");

            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderUserNameExists_ReturnsTrueWhenUserNamesAreDifferentCase()
        {
            //Arrange
            var context = new RepositoryContext();
            string userName = "CaseInsensitiveUserName";
            context.UserAccounts.Add(new UserAccountTableRow(new User(userName, "me@example.com", "test", "user"), "1234"));

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            UserProvider cut = new UserProvider(container);

            string upperUserName = userName.ToUpper();

            //Act
            bool actual = cut.UserNameExists(upperUserName);

            //Assert
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void UserProviderCreateAccount_FailsValidation_WhenUserNameAlreadyExists()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            string pwd = "1234";
            User user = new User("UnitTestDuplicateUserNameValidation", "anyone@example.com", "user", "test");
            cut.CreateAccount(user, pwd);

            User duplicateUser = new User("UnitTestDuplicateUserNameValidation", "anyoneelse@example.com", "user2", "test2");
            //Act
            var validationMsgs = cut.CreateAccount(duplicateUser, pwd);

            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("already exists"));
        }

        [TestMethod]
        public void UserProviderCreateAccount_FailsValidation_WhenFirstNameIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            
            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestFirstNameValidation";
            user.Email = "anyone@example.com";
            user.FirstName = string.Empty;
            user.LastName = "test";

            //Act
            var validationMsgs = cut.CreateAccount(user, pwd);

            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("First"));
        }

        [TestMethod]
        public void UserProviderCreateAccount_FailsValidation_WhenLastNameIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            
            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestLastNameValidation";
            user.Email = "anyone@example.com";
            user.FirstName = "test";
            user.LastName = string.Empty;

            //Act
            var validationMsgs = cut.CreateAccount(user, pwd);

            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("Last"));
        }

        [TestMethod]
        public void UserProviderCreateAccount_FailsValidation_WhenEmailIsMissingAtSign()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestEmailValidation";
            user.Email = "anyoneexample.com";
            user.FirstName = "test";
            user.LastName = "user";

            //Act
            var validationMsgs = cut.CreateAccount(user, pwd);

            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("email"));
        }

        [TestMethod]
        public void UserProviderCreateAccount_FailsValidation_WhenEmailIsMissingPeriod()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestEmailValidation";
            user.Email = "anyone@examplecom";
            user.FirstName = "test";
            user.LastName = "user";

            //Act
            var validationMsgs = cut.CreateAccount(user, pwd);

            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("email"));
        }

        [TestMethod]
        public void UserProviderAuthenticateUser_ThrowsEx_WhenUserNameIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            
            string userName = string.Empty;
            string pwd = "1234";

            try
            {
                //Act
                cut.AuthenticateUser(userName, pwd);

                //Assert
                Assert.Fail("AuthenticateUser should throw ArgumentNullException when userName is empty.");

            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderAuthenticateUser_ThrowsEx_WhenPasswordIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            
            string email = "anyone@example.com";
            string pwd = string.Empty;

            try
            {
                //Act
                cut.AuthenticateUser(email, pwd);

                //Assert
                Assert.Fail("AuthenticateUser should throw ArgumentNullException when password is empty.");

            }
            catch (ArgumentNullException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderAuthenticateUser_ReturnsNull_WhenUserNameNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            string nonExistingUserName = "fakeuser";
            string pwd = "1234";

            //Act
            User usr = cut.AuthenticateUser(nonExistingUserName, pwd);

            //Assert
            Assert.IsNull(usr);
        }

        [TestMethod]
        public void UserProviderAuthenticateUser_ReturnsNull_WhenPasswordDoesNotMatch()
        {
            //Arrange
            var context = new RepositoryContext();
            context.UserAccounts.Add(new UserAccountTableRow(new User("AuthenticateUserTest", "me@example.com", "test", "user"), "1234"));

            IUnityContainer container = TestIoCConfig.BuildContainer(context);
            UserProvider cut = new UserProvider(container);

            string userName = "AuthenticateUserTest";
            string badPassword = "4321";

            //Act
            User usr = cut.AuthenticateUser(userName, badPassword);

            //Assert
            Assert.IsNull(usr);
        }

        [TestMethod]
        public void UserProviderFindUserById_ReturnsUser_WhenUserIsFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            string pwd = "1234";
            User user = new User("UnitTest", "anyone@example.com", "test", "user");
            var validationMsgs = cut.CreateAccount(user, pwd);
            user = cut.FindByUserName(user.UserName);

            //Act
            var actual = cut.FindById(user.UserId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(user.UserName, actual.UserName);
        }

        [TestMethod]
        public void UserProviderFindUserById_ThrowsEx_WhenUserIdIsZero()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            int userId = 0;

            try
            {
                //Act
                cut.FindById(userId);

                //Assert
                Assert.Fail("FindUserById should throw an exception when userId == 0.");

            }
            catch (ArgumentException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderFindUserById_ReturnsNull_WhenUserIdIsNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            int userId = 45660965;

            //Act
            var actual = cut.FindById(userId);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void UserProviderFindUserByUserName_ThrowsEx_WhenUserNameIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            string userName = " ";

            try
            {
                //Act
                cut.FindByUserName(userName);

                //Assert
                Assert.Fail("FindUserByUserName should throw an exception when userName is empty.");

            }
            catch (ArgumentException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderFindUserByUserName_ReturnsNull_WhenUserNameIsNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            string userName = "nvyawenalidtaenavbwe";

            //Act
            var actual = cut.FindByUserName(userName);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void UserProviderTryChangePassword_ReturnsFalseAndMessages_WhenUserAuthenticationFails()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            string userName = Stopwatch.GetTimestamp().ToString();  //some unique user name

            User user = new User(userName, "x@example.com", "Test", "User");
            string pwd = "1234";

            cut.CreateAccount(user, pwd);
            var actual = cut.FindByUserName(userName);
            List<string> failureMsgList;
            const string expected = "User authentication failed.";

            //Act
            bool changed = cut.TryChangePassword(actual.UserName, "wrongpassword", "newPassword", out failureMsgList);

            //Assert
            Assert.IsFalse(changed);
            Assert.IsNotNull(failureMsgList);
            Assert.IsTrue(failureMsgList.Count == 1);
            Assert.AreEqual(expected, failureMsgList.First());
        }

        [TestMethod]
        public void UserProviderTryChangePassword_ReturnsTrueAndNoMessages_WhenUserAuthenticationSucceeds()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            string userName = Stopwatch.GetTimestamp().ToString();  //some unique user name

            User user = new User(userName, "x@example.com", "Test", "User");
            string pwd = "1234";

            cut.CreateAccount(user, pwd);
            var actual = cut.FindByUserName(userName);
            List<string> failureMsgList;

            //Act
            bool changed = cut.TryChangePassword(actual.UserName, pwd, "newPassword", out failureMsgList);

            //Assert
            Assert.IsTrue(changed);
            Assert.IsNotNull(failureMsgList);
            Assert.IsTrue(failureMsgList.Count == 0);
        }

        [TestMethod]
        public void UserProviderSaveProfile_FailsValidation_WhenFirstNameIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            string expected = string.Empty;
            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestEmailValidation";
            user.Email = "anyone@example.com";
            user.FirstName = "test";
            user.LastName = "user";

            cut.CreateAccount(user, pwd);
            user = cut.FindByUserName(user.UserName);
            user.FirstName = expected;

            //Act
            var validationMsgs = cut.SaveProfile(user);
            var actual = cut.FindByUserName(user.UserName);

            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("First"));
        }

        [TestMethod]
        public void UserProviderSaveProfile_FailsValidation_WhenLastNameIsEmpty()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            string expected = string.Empty;
            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestEmailValidation";
            user.Email = "anyone@example.com";
            user.FirstName = "test";
            user.LastName = "user";

            cut.CreateAccount(user, pwd);
            user = cut.FindByUserName(user.UserName);
            user.LastName = expected;

            //Act
            var validationMsgs = cut.SaveProfile(user);
            var actual = cut.FindByUserName(user.UserName);
            
            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("Last"));
        }

        [TestMethod]
        public void UserProviderSaveProfile_FailsValidation_WhenEmailIsMissingAtSign()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            const string expected = "updatedEmailexample.com";
            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestEmailValidation";
            user.Email = "anyone@example.com";
            user.FirstName = "test";
            user.LastName = "user";

            cut.CreateAccount(user, pwd);
            user = cut.FindByUserName(user.UserName);
            user.Email = expected;

            //Act
            var validationMsgs = cut.SaveProfile(user);
            var actual = cut.FindByUserName(user.UserName);


            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("email"));
        }

        [TestMethod]
        public void UserProviderSaveProfile_FailsValidation_WhenEmailIsMissingPeriod()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            const string expected = "updatedEmail@examplecom";
            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestEmailValidation";
            user.Email = "anyone@example.com";
            user.FirstName = "test";
            user.LastName = "user";

            cut.CreateAccount(user, pwd);
            user = cut.FindByUserName(user.UserName);
            user.Email = expected;

            //Act
            var validationMsgs = cut.SaveProfile(user);
            var actual = cut.FindByUserName(user.UserName);


            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 1);
            Assert.IsTrue(validationMsgs.First().Contains("email"));
        }

        [TestMethod]
        public void UserProviderSaveProfile_ReturnsEmptyList_WhenProfileIsSuccessfullyUpdated()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);

            const string expected = "updatedEmail@example.com";
            string pwd = "1234";
            User user = new User();
            user.UserName = "UnitTestEmailValidation";
            user.Email = "anyone@example.com";
            user.FirstName = "test";
            user.LastName = "user";

            cut.CreateAccount(user, pwd);
            user = cut.FindByUserName(user.UserName);
            user.Email = expected;

            //Act
            var validationMsgs = cut.SaveProfile(user);
            var actual = cut.FindByUserName(user.UserName);

            //Assert
            Assert.IsNotNull(validationMsgs);
            Assert.IsTrue(validationMsgs.Count == 0);
            Assert.AreEqual(expected, actual.Email);
        }

    }
}
