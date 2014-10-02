using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Action.KillSwitches
{
    public class KillSwitchRule
    {
        public int Id { get; set; }
        public int KillSwitchId { get; set; }
        public bool IsActive { get; set; }
        public int Limit { get; set; }
        public TimeSpan Period { get; set; }
        public bool ActivateKillSwitchIfSatisfied { get; set; }

        public bool IsSatisfiedBy(int numberOfActionsInPeriod)
        {
            return numberOfActionsInPeriod >= this.Limit;
        }
    }
}
