using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Action.KillSwitches
{
    public class KillSwitchRuleResults
    {
        public KillSwitchRuleResults(int killSwitchId)
        {
            this.KillSwitchId = killSwitchId;
            this.TriggeredRules = new List<KillSwitchRule>();
            this.ForceNotifyAdmin = false;
        }

        public int KillSwitchId { get; private set; }
        internal bool ForceNotifyAdmin { get; set; }
        public bool NotifyAdmin { get { return (this.ForceNotifyAdmin || this.TriggeredRules.Count > 0); } }
        public bool ActivateKillSwitch { get { return this.TriggeredRulesActivateKillSwitch(); } }

        public List<KillSwitchRule> TriggeredRules { get; private set; }

        internal void AddRule(KillSwitchRule rule)
        {
            this.TriggeredRules.Add(rule);
        }

        private bool TriggeredRulesActivateKillSwitch()
        {
            return this.TriggeredRules.Count(r => r.ActivateKillSwitchIfSatisfied) > 0;
        }
    }
}
