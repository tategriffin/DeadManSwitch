using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action.KillSwitches;

namespace DeadManSwitch.Data
{
    public interface IKillSwitchRepository
    {

        KillSwitch Find(int killSwitchId);
        KillSwitch Find(ActionType actionType, ActionDirection direction);

        KillSwitchRules FindRules(int killSwitchId);
        KillSwitchRules FindRules(ActionType actionType, ActionDirection direction);

        Dictionary<TimeSpan, int> CountActions(ActionType actionType, ActionDirection direction, IEnumerable<TimeSpan> timeSpans);

        void ActivateKillSwitch(int killSwitchId);
    }
}
