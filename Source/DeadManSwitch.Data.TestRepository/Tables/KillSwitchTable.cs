using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;
using DeadManSwitch.Action.KillSwitches;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class KillSwitchTable
    {
        public KillSwitchTable()
        {
            List<KillSwitch> persistentRows = BuildPersistentRows();

            Rows = persistentRows;
        }

        public List<KillSwitch> Rows { get; private set; }

        private List<KillSwitch> BuildPersistentRows()
        {
            List<KillSwitch> persistentRows = new List<KillSwitch>();

            var knownActions = Enum.GetValues(typeof(ActionType)).Cast<ActionType>();
            int identity = 1;
            foreach (var action in knownActions)
            {
                if (action == ActionType.None) continue;

                persistentRows.Add(new KillSwitch() { Id = identity++, ActionType = action, Direction = ActionDirection.Incoming, IsEngaged = false });
                persistentRows.Add(new KillSwitch() { Id = identity++, ActionType = action, Direction = ActionDirection.Outgoing, IsEngaged = false });
            }

            return persistentRows;
        }

    }
}
