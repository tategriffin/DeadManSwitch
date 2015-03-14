using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Providers
{
    public class UserPreferenceProvider
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private IUserPreferenceRepository UserPreferenceRepository;

        public UserPreferenceProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.UserPreferenceRepository = container.Resolve<IUserPreferenceRepository>();
        }

        public UserPreferences Find(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId == 0) throw new ArgumentException("userId is not valid.");

            UserPreferences preferences = UserPreferenceRepository.FindById(user.UserId);
            if (preferences == null)
            {
                Log.Warn("User '{0}' does not have any preferences.", user.UserName);

                preferences = UserPreferences.GetDefaultPreferences(user.UserId);
            }

            return preferences;
        }

        public void Save(UserPreferences preferences)
        {
            if (preferences == null) throw new ArgumentNullException("preferences");
            if (preferences.UserId == 0) throw new ArgumentException("userId is not valid.");

            UserPreferenceRepository.Save(preferences);
        }

    }
}
