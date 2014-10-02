using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data.SqlRepository.EntityMappers;

namespace DeadManSwitch.Data.SqlRepository
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        public UserPreferences FindById(int userId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                SqlRepository.UserPreference pref = context.UserPreferences.SingleOrDefault(p => p.UserId == userId);

                return (pref == null ? null : pref.ToDomain());
            }
            finally
            {
                context.Dispose();
            }
        }

        public void Save(DeadManSwitch.UserPreferences preferences)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                DateTime utcNow = DateTime.UtcNow;

                SqlRepository.UserPreference existingPrefs =
                    context.UserPreferences
                    .Where(p => p.UserId == preferences.UserId)
                    .SingleOrDefault();

                if (existingPrefs == null)
                {
                    existingPrefs = new UserPreference();
                    existingPrefs.UserPreferenceId = 0;
                    existingPrefs.CreateDate = utcNow;
                    context.UserPreferences.Add(existingPrefs);
                }

                UserPreferenceMapper.MapDomainToData(preferences, existingPrefs);
                existingPrefs.ModDate = utcNow;
                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
