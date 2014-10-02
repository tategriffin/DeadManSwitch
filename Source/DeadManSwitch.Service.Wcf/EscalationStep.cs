using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    [DataContract]
    public class EscalationStep
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int WaitMinutes { get; set; }
        [DataMember]
        public int ActionType { get; set; }
        [DataMember]
        public string Recipient { get; set; }

    }
}
