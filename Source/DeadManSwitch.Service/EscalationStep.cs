using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public class EscalationStep
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WaitMinutes { get; set; }
        public ActionType ActionType { get; set; }
        public string Recipient { get; set; }

        public EscalationStep()
        {
            this.Id = 0;
            this.Number = 0;
            this.WaitMinutes = 0;
            this.ActionType = ActionType.None;
            this.Recipient = string.Empty;
        }

    }

}
