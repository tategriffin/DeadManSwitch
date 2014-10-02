using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeadManSwitch;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Service;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service.Tests
{
    [TestClass]
    public class CheckInServiceTests
    {
        IUnityContainer Container;

        public CheckInServiceTests()
        {
            this.Container = TestIoCConfig.BuildContainer(new RepositoryContext());
        }

        [TestMethod]
        public void CheckInServiceCheckInUser_ThrowsEx_WhenUserNameIsEmpty()
        {
            //Arrange
            string testUserName = null;
            var cut = new CheckInService(this.Container);

            try
            {
                //Act
                cut.CheckInUser(testUserName);

                //Assert
                Assert.Fail("CheckInUser should throw exception when userName is null or empty.");
            }
            catch (ArgumentException)
            {
                //Expected this exception. Test passes.
            }
        }

        [TestMethod]
        public void CheckInServiceCheckInUser_ThrowsEx_WhenUserNameIsNotFound()
        {
            //Arrange
            string testUserName = "nvyawenalidtaenavbwe";
            var cut = new CheckInService(this.Container);

            try
            {
                //Act
                cut.CheckInUser(testUserName);

                //Assert
                Assert.Fail("CheckInUser should throw exception when userName is not found.");
            }
            catch (Exception)
            {
                //Expected exception. Test passes.
            }

        }

        [TestMethod]
        public void CheckInServiceGetLastCheckIn_ThrowsEx_WhenUserNameIsNullOrEmpty()
        {
            //Arrange
            string testUserName = null;
            var cut = new CheckInService(this.Container);

            try
            {
                //Act
                var result = cut.FindLastUserCheckIn(testUserName);

                //Assert
                Assert.Fail("GetLastUserCheckIn should throw exception when userName is null or empty.");
            }
            catch (ArgumentException)
            {
                //Expected this exception. Test passes.
            }
        }

        [TestMethod]
        public void CheckInServiceGetLastCheckIn_ThrowsEx_WhenUserIdGuidIsNotFound()
        {
            //Arrange
            string testUserName = "nvyawenalidtaenavbwe";
            var cut = new CheckInService(this.Container);

            try
            {
                //Act
                var result = cut.FindLastUserCheckIn(testUserName);

                //Assert
                Assert.Fail("GetLastUserCheckIn should throw exception when userName is not found.");
            }
            catch (Exception)
            {
                //Expected this exception. Test passes.
            }
        }

    }
}
