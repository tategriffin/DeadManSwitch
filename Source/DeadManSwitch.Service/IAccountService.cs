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
        bool IsRegistrationOpen();

        bool UserNameExists(string userName);

        IEnumerable<string> RegisterUser(User user, string password);

        User Login(string userName, string password);

        User FindUser(string userName);

        UserPreferences FindUserPreferences(string userName);

        void UpdatePreferences(string userName, UserPreferences preferences);

        void UpdateProfile(string userName, UserProfile profile);

        bool ChangePassword(string userName, string oldPassword, string newPassword);

        Dictionary<string, string> GetSupportedTimeZones();
    }
}
