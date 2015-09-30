using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.WebApi
{
    public class EscalationStep
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WaitMinutes { get; set; }
        public int ActionType { get; set; }
        [Required]
        public string Recipient { get; set; }

    }
}
