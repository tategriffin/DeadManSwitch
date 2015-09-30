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
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ApplicationSettingsProvider AppSettingsPvdr;
        private readonly ReferenceDataProvider ReferenceDataPvdr;
        private readonly UserProvider UserPvdr;
        private readonly UserPreferenceProvider UserPrefPvdr;

        public AccountService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.ReferenceDataPvdr = new ReferenceDataProvider(container);
            this.UserPvdr = new UserProvider(container);
            this.UserPrefPvdr = new UserPreferenceProvider(container);
            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
        }

        public Task<bool> IsRegistrationOpenAsync()
        {
            return Task.FromResult(IsRegistrationOpen());
        }

        public bool IsRegistrationOpen()
        {
            return AppSettingsPvdr.IsRegistrationOpen();
        }

        public Task<bool> UserNameExistsAsync(string userName)
        {
            return Task.FromResult(UserNameExists(userName));
        }

        public bool UserNameExists(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");

            return this.UserPvdr.UserNameExists(userName);
        }

        public Task<List<string>> RegisterUserAsync(User user, string password)
        {
            return Task.FromResult(RegisterUser(user, password));
        }

        public List<string> RegisterUser(User user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "password cannot be null or empty.");

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

        public Task<LoginResponse> LoginAsync(string userName, string password)
        {
            return Task.FromResult(Login(userName, password));
        }

        public LoginResponse Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password), "password cannot be null or empty.");

            var response = new LoginResponse();
            response.User = UserPvdr.AuthenticateUser(userName, password).ToServiceEntity();
            if (!response.IsSuccessful)
            {
                response.LoginFailedUserMessageList.Add("Invalid username or password");
            }

            return response;
        }

        public Task<User> FindUserAsync(string userName)
        {
            return Task.FromResult(FindUser(userName));
        }

        public User FindUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");

            return this.UserPvdr.FindByUserName(userName).ToServiceEntity();
        }

        public Task<UserPreferences> FindUserPreferencesAsync(string userName)
        {
            return Task.FromResult(FindUserPreferences(userName));
        }

        public UserPreferences FindUserPreferences(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");

            DeadManSwitch.User user = this.UserPvdr.FindByUserName(userName);
            return this.UserPrefPvdr.Find(user).ToServiceEntity();
        }

        public Task UpdatePreferencesAsync(string userName, UserPreferences preferences)
        {
            UpdatePreferences(userName, preferences);

            return Task.CompletedTask;
        }

        public void UpdatePreferences(string userName, UserPreferences preferences)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");
            if (preferences == null) throw new ArgumentNullException(nameof(preferences));

            DeadManSwitch.User user = this.UserPvdr.FindByUserName(userName);
            DeadManSwitch.UserPreferences existingPrefs = this.UserPrefPvdr.Find(user);

            existingPrefs.TzInfo = preferences.TzInfo;
            existingPrefs.EarlyCheckInOffset = preferences.EarlyCheckInOffset;

            this.UserPrefPvdr.Save(existingPrefs);
        }

        public Task UpdateProfileAsync(string userName, UserProfile profile)
        {
            UpdateProfile(userName, profile);

            return Task.CompletedTask;
        }

        public void UpdateProfile(string userName, UserProfile profile)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");
            if (profile == null) throw new ArgumentNullException(nameof(profile));

            DeadManSwitch.User user = this.UserPvdr.FindByUserName(userName);
            UserMapper.MapProfileToUser(user, profile);

            this.UserPvdr.SaveProfile(user);
        }

        public Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            return Task.FromResult(ChangePassword(userName, oldPassword, newPassword));
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentNullException(nameof(oldPassword), "oldPassword cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException(nameof(newPassword), "newPassword cannot be null or empty.");

            List<string> failureMessages = new List<string>();
            bool changed = this.UserPvdr.TryChangePassword(userName, oldPassword, newPassword, out failureMessages);
            if(!changed)
            {
                Log.Warn($"Attempt to change password for '{userName}' failed. {string.Join(";", failureMessages)}");
            }

            return changed;
        }

        public Task<Dictionary<string, string>> GetSupportedTimeZonesAsync()
        {
            return Task.FromResult(GetSupportedTimeZones());
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

        public Task<Dictionary<string, string>> GetCheckInWindowOptionsAsync()
        {
            return Task.FromResult(GetCheckInWindowOptions());
        }

        public Dictionary<string, string> GetCheckInWindowOptions()
        {
            return ReferenceDataPvdr.EarlyCheckInOptions()
                .ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
        }

    }
}
