using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class UserPreferenceRepository : RepositoryWithContext, IUserPreferenceRepository
    {
        public UserPreferenceRepository(RepositoryContext context)
            : base(context) { }

        public UserPreferences FindById(int userId)
        {
            UserPreferences prefs =
                Context.UserPreferences
                .Where(p => p.UserId == userId)
                .SingleOrDefault();

            return prefs;
        }

        public void Save(UserPreferences preferences)
        {
            UserPreferences existingPrefs = this.FindById(preferences.UserId);
            if (existingPrefs == null)
            {
                Context.UserPreferences.Add(preferences);
            }
            else
            {
                existingPrefs.TzInfo = preferences.TzInfo;
                existingPrefs.EarlyCheckInOffset = preferences.EarlyCheckInOffset;
            }
        }

    }
}
