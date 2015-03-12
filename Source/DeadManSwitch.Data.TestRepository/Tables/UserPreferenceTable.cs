using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class UserPreferenceTable
    {
        public UserPreferenceTable()
        {
            List<UserPreferences> persistentRows = BuildPersistentRows();

            Rows = persistentRows;
        }

        public List<UserPreferences> Rows { get; private set; }

        private List<UserPreferences> BuildPersistentRows()
        {
            List<UserPreferences> persistentRows = new List<UserPreferences>();

            persistentRows.Add(new UserPreferences() { UserId = 1, TzInfo = TimeZoneInfo.Local });

            return persistentRows;
        }

    }
}
