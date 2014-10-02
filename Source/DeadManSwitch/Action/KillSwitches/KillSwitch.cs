using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Action.KillSwitches
{
    public class KillSwitch
    {
        public int Id { get; set; }
        public ActionType ActionType { get; set; }
        public ActionDirection Direction { get; set; }
        public string Description { get { return this.BuildDescription(); } }
        public bool IsEngaged { get; set; }

        private string BuildDescription()
        {
            return this.Direction.ToString() + " " + this.ActionType.ToString() + " kill switch";
        }

    }
}
