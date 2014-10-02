using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace DeadManSwitch.Action.KillSwitches
{
    public class KillSwitchRules : List<KillSwitchRule>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public KillSwitchRules(int killSwitchId)
        {
            this.KillSwitchId = killSwitchId;
        }

        public int KillSwitchId { get; private set; }

        internal List<TimeSpan> GetAllPeriods()
        {
            List<TimeSpan> allPeriods = new List<TimeSpan>();
            foreach (var item in this)
            {
                allPeriods.Add(item.Period);
            }

            return allPeriods;
        }

        public KillSwitchRuleResults Evaluate(Dictionary<TimeSpan, int> periodCounts)
        {
            if (periodCounts == null) throw new ArgumentNullException("periodCounts");

            KillSwitchRuleResults results = new KillSwitchRuleResults(this.KillSwitchId);
            foreach (var item in periodCounts)
            {
                KillSwitchRule rule = this.FindByPeriod(item.Key);
                if (rule == null)
                {
                    results.ForceNotifyAdmin = true;
                    logger.Error("Period with ticks '{0}' was not found for kill switch id {1}.", item.Key.Ticks, this.KillSwitchId);
                }
                else
                {
                    bool isSatisfied = rule.IsSatisfiedBy(item.Value);
                    if (isSatisfied)
                    {
                        results.AddRule(rule);
                    }
                }
            }

            return results;
        }

        private KillSwitchRule FindByPeriod(TimeSpan period)
        {
            KillSwitchRule rule = null;
            foreach (var item in this)
            {
                if (item.Period.Ticks == period.Ticks)
                {
                    rule = item;
                    break;
                }
            }

            return rule;
        }

    }
}
