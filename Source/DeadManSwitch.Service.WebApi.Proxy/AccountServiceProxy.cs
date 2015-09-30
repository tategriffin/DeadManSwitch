using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Configuration;
using Newtonsoft.Json;

using static DeadManSwitch.Service.WebApi.LoginModelMapper;
using static DeadManSwitch.Service.WebApi.RegistrationModelMapper;

namespace DeadManSwitch.Service.WebApi.Proxy
{
    public class AccountServiceProxy : ServiceProxy, DeadManSwitch.Service.IAccountService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public AccountServiceProxy(IHostSettingsReader config)
            : base(config) { }

        public async Task<bool> IsRegistrationOpenAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("/Register");

                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<bool>();
            }
        }

        public async Task<bool> UserNameExistsAsync(string userName)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"/Users/{userName}");

                if (response.StatusCode == HttpStatusCode.NotFound) return false;
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<bool>();
            }
        }

        public async Task<List<string>> RegisterUserAsync(User user, string password)
        {
            UserRegistrationModel userRegistration = user.ToRegistrationModel(password);
            using (var client = CreateHttpClient())
            {
                string userJson = JsonConvert.SerializeObject(userRegistration);
                var response = await client.PostAsync("Users", BuildJsonHttpContent(userJson));

                if (response.IsClientError()) return await response.ToRegistrationFailure("Registration failed.");
                response.EnsureSuccessStatusCode();

                return await response.ToRegistrationSuccess();
            }
        }

        public async Task<LoginResponse> LoginAsync(string userName, string password)
        {
            UserLoginModel model = new UserLoginModel() {UserName = userName, Password = password};

            using (var client = CreateHttpClient())
            {
                string modelJson = JsonConvert.SerializeObject(model);
                var response = await client.PostAsync("Login", BuildJsonHttpContent(modelJson));

                if (response.IsClientError()) return await response.ToLoginResponseFailure("Invalid username or password.");
                response.EnsureSuccessStatusCode();

                return await response.ToLoginResponseSuccess();
            }
        }

        public async Task<User> FindUserAsync(string userName)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"Users/{userName}");
                if (response.StatusCode == HttpStatusCode.NotFound) return null;
                response.EnsureSuccessStatusCode();

                return await response.ToServiceEntityUser();
            }
        }

        public async Task<UserPreferences> FindUserPreferencesAsync(string userName)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"UserPreferences/{userName}");
                if (response.StatusCode == HttpStatusCode.NotFound) return null;
                response.EnsureSuccessStatusCode();

                var prefs = await response.DeserializeResponseContentAsync<WebApi.UserPreferencesModel>();
                return prefs.ToServiceEntity();
            }
        }

        public async Task UpdatePreferencesAsync(string userName, UserPreferences preferences)
        {
            var webapiModel = preferences.ToWebApiEntity();

            using (var client = CreateHttpClient())
            {
                var jsonUserPrefs = JsonConvert.SerializeObject(webapiModel);
                var response = await client.PostAsync($"UserPreferences/{userName}", BuildJsonHttpContent(jsonUserPrefs));
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task UpdateProfileAsync(string userName, UserProfile profile)
        {
            var webapiModel = profile.ToWebApiEntity();

            using (var client = CreateHttpClient())
            {
                var jsonUserProfile = JsonConvert.SerializeObject(webapiModel);
                var response = await client.PutAsync($"Users/{userName}", BuildJsonHttpContent(jsonUserProfile));
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            using (var client = CreateHttpClient())
            {
                var model = new ChangePasswordModel() { UserName = userName, NewPassword = newPassword, OldPassword = oldPassword};
                var json = JsonConvert.SerializeObject(model);
                var response = await client.PostAsync($"Password", BuildJsonHttpContent(json));

                return (response.IsSuccessStatusCode);
            }
        }

        public async Task<Dictionary<string, string>> GetSupportedTimeZonesAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"TimeZones");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Dictionary<string, string>>();
            }
        }

        public async Task<Dictionary<string, string>> GetCheckInWindowOptionsAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"CheckInWindows");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Dictionary<string, string>>();
            }
        }

    }
}
