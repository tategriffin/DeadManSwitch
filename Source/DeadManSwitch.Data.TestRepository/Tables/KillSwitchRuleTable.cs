using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action.KillSwitches;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class KillSwitchRuleTable
    {
        public KillSwitchRuleTable()
        {
            List<KillSwitchRule> persistentRows = BuildPersistentRows();

            Rows = persistentRows;
        }

        public List<KillSwitchRule> Rows { get; private set; }

        private List<KillSwitchRule> BuildPersistentRows()
        {
            List<KillSwitchRule> persistentRows = new List<KillSwitchRule>();

            return persistentRows;
        }

    }
}
