using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DeadManSwitch;
using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class CheckInProviderTests
    {
        [TestMethod]
        public void CheckInProviderFindLastCheckIn_ReturnsNull_WhenCheckInIsNotFound()
        {
            //Arrange
            User fakeUser = new User() { UserId = 6007 };
            DateTime checkInTime = DateTime.UtcNow.AddHours(-2);
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());

            CheckInProvider cut = new CheckInProvider(container);

            //Act
            var actual = cut.FindLastCheckIn(fakeUser);

            //Assert
            Assert.IsNull(actual);
        }


    }
}
