using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class KillSwitchMapper
    {
        private const string DirectionIncoming = "I";
        private const string DirectionOutgoing = "O";

        public static char ToChar(this ActionDirection direction)
        {
            string value;
            switch (direction)
            {
                case ActionDirection.Incoming:
                    value = DirectionIncoming;
                    break;
                case ActionDirection.Outgoing:
                    value = DirectionOutgoing;
                    break;

                default:
                    throw new Exception(string.Format("No character value defined for direction '{0}'", direction));
            }

            return value.ToCharArray()[0];
        }

        public static DeadManSwitch.Action.KillSwitches.KillSwitch ToDomain(this DeadManSwitch.Data.SqlRepository.KillSwitch data)
        {
            DeadManSwitch.Action.KillSwitches.KillSwitch domain = new Action.KillSwitches.KillSwitch();

            domain.Id = data.KillSwitchID;
            domain.ActionType = (ActionType)data.EscalationActionTypeID;
            domain.IsEngaged = data.Engaged;
            switch (data.Direction.ToUpper())
            {
                case DirectionIncoming:
                    domain.Direction = ActionDirection.Incoming;
                    break;
                case DirectionOutgoing:
                    domain.Direction = ActionDirection.Outgoing;
                    break;
                default:
                    throw new Exception(string.Format("KillSwitch direction '{0}' is not supported.", data.Direction));
            }

            return domain;
        }

        public static DeadManSwitch.Action.KillSwitches.KillSwitchRules ToDomain(this IEnumerable<DeadManSwitch.Data.SqlRepository.KillSwitchRule> dataItems, int killSwitchId)
        {
            DeadManSwitch.Action.KillSwitches.KillSwitchRules domain = new Action.KillSwitches.KillSwitchRules(killSwitchId);
            foreach (var item in dataItems)
            {
                domain.Add(item.ToDomain());
            }

            return domain;
        }

        public static DeadManSwitch.Action.KillSwitches.KillSwitchRule ToDomain(this DeadManSwitch.Data.SqlRepository.KillSwitchRule data)
        {
            DeadManSwitch.Action.KillSwitches.KillSwitchRule domain = new Action.KillSwitches.KillSwitchRule();

            domain.Id = data.KillSwitchRuleID;
            domain.KillSwitchId = data.KillSwitchID;
            domain.IsActive = data.IsActive;
            domain.Limit = data.Limit;
            domain.Period = new TimeSpan(data.TimePeriodTicks);
            domain.ActivateKillSwitchIfSatisfied = data.ActivateKillSwitch;

            return domain;
        }

    }
}
