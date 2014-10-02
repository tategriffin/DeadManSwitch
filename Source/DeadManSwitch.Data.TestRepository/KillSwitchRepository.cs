using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository
{
    public class KillSwitchRepository : RepositoryWithContext, IKillSwitchRepository
    {
        public KillSwitchRepository(RepositoryContext context)
            :base(context) { }

        public Action.KillSwitches.KillSwitch Find(int killSwitchId)
        {
            return Context.KillSwitches
                .SingleOrDefault(r => r.Id == killSwitchId);
        }

        public Action.KillSwitches.KillSwitch Find(ActionType actionType, ActionDirection direction)
        {
            return Context.KillSwitches
                .SingleOrDefault(r => r.ActionType == actionType && r.Direction == direction);
        }

        public Action.KillSwitches.KillSwitchRules FindRules(int killSwitchId)
        {
            var existingRows = Context.KillSwitchRules
                .Where(r => r.KillSwitchId == killSwitchId)
                .ToList();

            var result = new Action.KillSwitches.KillSwitchRules(killSwitchId);
            result.AddRange(existingRows);

            return result;
        }

        public Action.KillSwitches.KillSwitchRules FindRules(ActionType actionType, ActionDirection direction)
        {
            var rule = this.Find(actionType, direction);
            if (rule == null)
            {
                return new Action.KillSwitches.KillSwitchRules(0);
            }
            else
            {
                return this.FindRules(rule.Id);
            }
        }

        public Dictionary<TimeSpan, int> CountActions(ActionType actionType, ActionDirection direction, IEnumerable<TimeSpan> timeSpans)
        {
            Dictionary<TimeSpan, int> results = new Dictionary<TimeSpan, int>();

            foreach (var item in timeSpans)
            {
                DateTime compareDate = DateTime.UtcNow.Add(item.Negate());
                int count = Context.EscalationActionLogs
                    .Count(r => r.ActionType == actionType
                        && r.Direction == direction
                        && r.CreateDate >= compareDate);

                results.Add(item, count);
            }

            return results;
        }

        public void ActivateKillSwitch(int killSwitchId)
        {
            var ks = Context.KillSwitches.SingleOrDefault(r => r.Id == killSwitchId);
            if (ks != null)
            {
                ks.IsEngaged = true;
            }
        }
    }
}
