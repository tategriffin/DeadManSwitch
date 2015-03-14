using System;
using System.Linq;
using DeadManSwitch.Data.TestRepository;
using DeadManSwitch.Providers;
using DeadManSwitch.Schedule;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class UserPreferenceProviderTests
    {
        [TestMethod]
        public void UserPreferenceProviderFind_ReturnsDefaultPreferences_WhenUserPreferencesDoNotExist()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var cut = new UserPreferenceProvider(container);

            var user = userProvider.FindByUserName("UserPreferenceProviderUnitTestUser");
            var expected = UserPreferences.GetDefaultPreferences(user.UserId);

            //Act
            var actual = cut.Find(user);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void UserPreferenceProviderSave_UpdatesDatastore_WhenUserPreferencesAreValid()
        {
            //Arrange
            IUnityContainer container = TestIoCConfig.BuildContainer(new RepositoryContext());
            var userProvider = new UserProvider(container);
            var cut = new UserPreferenceProvider(container);

            var user = userProvider.FindByUserName("UserPreferenceProviderUnitTestUser");
            var expected = UserPreferences.GetDefaultPreferences(user.UserId);
            var prefs = cut.Find(user);

            //Act
            prefs.EarlyCheckInOffset = new TimeSpan(0, 3, 0);
            cut.Save(prefs);

            var actual = cut.Find(user);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(expected.Equals(actual));
        }

    }
}
