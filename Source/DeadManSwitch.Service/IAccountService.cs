using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface IAccountService
    {
        Task<bool> IsRegistrationOpenAsync();

        Task<bool> UserNameExistsAsync(string userName);

        Task<List<string>> RegisterUserAsync(User user, string password);

        Task<LoginResponse> LoginAsync(string userName, string password);

        Task<User> FindUserAsync(string userName);

        Task<UserPreferences> FindUserPreferencesAsync(string userName);

        Task UpdatePreferencesAsync(string userName, UserPreferences preferences);

        Task UpdateProfileAsync(string userName, UserProfile profile);
        
        Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword);

        Task<Dictionary<string, string>> GetSupportedTimeZonesAsync();

        Task<Dictionary<string, string>> GetCheckInWindowOptionsAsync();

    }
}
