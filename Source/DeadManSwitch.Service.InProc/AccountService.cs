using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Providers;
using DeadManSwitch.Service.EntityMappers;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Service
{
    public class AccountService : IAccountService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private UserProvider UserPvdr;
        private UserPreferenceProvider UserPrefPvdr;
        private ApplicationSettingsProvider AppSettingsPvdr;

        public AccountService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.UserPvdr = new UserProvider(container);
            this.UserPrefPvdr = new UserPreferenceProvider(container);
            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
        }

        public bool IsRegistrationOpen()
        {
            return AppSettingsPvdr.IsRegistrationOpen();
        }

        public bool UserNameExists(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");

            return this.UserPvdr.UserNameExists(userName);
        }

        public IEnumerable<string> RegisterUser(User user, string password)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (String.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "password cannot be null or empty.");

            List<string> failureMessages = new List<string>();
            if (IsRegistrationOpen() == false)
            {
                failureMessages.Add("Registration is currently closed.");
            }
            else
            {
                failureMessages.AddRange(this.UserPvdr.CreateAccount(user.ToDomainEntity(), password));
            }

            return failureMessages;
        }

        public User Login(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "password cannot be null or empty.");

            return this.UserPvdr.AuthenticateUser(userName, password).ToServiceEntity();
        }

        public User FindUser(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");

            return this.UserPvdr.FindByUserName(userName).ToServiceEntity();
        }

        public UserPreferences FindUserPreferences(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");

            DeadManSwitch.User user = this.UserPvdr.FindByUserName(userName);
            return this.UserPrefPvdr.Find(user).ToServiceEntity();
        }

        public void UpdatePreferences(string userName, UserPreferences preferences)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (preferences == null) throw new ArgumentNullException("preferences");

            DeadManSwitch.User user = this.UserPvdr.FindByUserName(userName);
            DeadManSwitch.UserPreferences existingPrefs = this.UserPrefPvdr.Find(user);

            existingPrefs.TzInfo = preferences.TzInfo;
            existingPrefs.EarlyCheckInOffset = preferences.EarlyCheckInOffset;

            this.UserPrefPvdr.Save(existingPrefs);
        }

        public void UpdateProfile(string userName, UserProfile profile)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (profile == null) throw new ArgumentNullException("profile");

            DeadManSwitch.User user = this.UserPvdr.FindByUserName(userName);
            UserMapper.MapProfileToUser(user, profile);

            this.UserPvdr.SaveProfile(user);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentNullException("oldPassword", "oldPassword cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException("newPassword", "newPassword cannot be null or empty.");

            List<string> failureMessages = new List<string>();
            bool changed = this.UserPvdr.TryChangePassword(userName, oldPassword, newPassword, out failureMessages);
            if(!changed)
            {
                logger.Warn(string.Format("Attempt to change password for '{0}' failed. {1}", userName, string.Join(";", failureMessages)));
            }

            return changed;
        }

        public Dictionary<string, string> GetSupportedTimeZones()
        {
            var timeZones = new Dictionary<string, string>();

            var allTz = TimeZoneInfo.GetSystemTimeZones().OrderBy(tz => tz.BaseUtcOffset);
            foreach (var tz in allTz)
            {
                timeZones.Add(tz.Id, tz.DisplayName);
            }

            return timeZones;
        }
    }
}
