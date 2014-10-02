using System;
using System.Collections.Generic;
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
            try
            {
                cut.CreateAccount(user, pwd);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.IsTrue(ex.Message.Contains("first"));    //expect error related to blank first name
            }
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
            try
            {
                cut.CreateAccount(user, pwd);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.IsTrue(ex.Message.Contains("last"));    //expect error related to blank last name
            }
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
        public void UserProviderFindUserById_ThrowsEx_WhenUserIdIsNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            int userId = 45660965;

            try
            {
                //Act
                cut.FindById(userId);

                //Assert
                Assert.Fail("FindUserById should throw an exception when userId is not found.");

            }
            catch (Exception)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderTryFindUserById_ReturnsNull_WhenUserIdIsNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            int userId = 45660965;

            //Act
            User user;
            var actual = cut.TryFindById(userId, out user);

            //Assert
            Assert.IsFalse(actual);
            Assert.IsNull(user);
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
        public void UserProviderFindUserByUserName_ThrowsEx_WhenUserNameIsNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            string userName = "nvyawenalidtaenavbwe";

            try
            {
                //Act
                cut.FindByUserName(userName);

                //Assert
                Assert.Fail("FindUserByUserName should throw an exception when userName is not found.");

            }
            catch (Exception)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void UserProviderTryFindUserByUserName_ReturnsNull_WhenUserNameIsNotFound()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            UserProvider cut = new UserProvider(container);
            string userName = "nvyawenalidtaenavbwe";

            //Act
            User user;
            var actual = cut.TryFindByUserName(userName, out user);

            //Assert
            Assert.IsFalse(actual);
            Assert.IsNull(user);
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

    }
}
