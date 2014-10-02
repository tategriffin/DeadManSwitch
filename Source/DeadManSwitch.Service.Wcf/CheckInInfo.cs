using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    [Serializable]
    [DataContract]
    public class CheckInInfo
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public DateTime? CheckInTimeUtc { get; set; }
        [DataMember]
        public DateTime? NextCheckInTimeUtc { get; set; }
        [DataMember]
        public string UserTimeZoneId { get; set; }
    }
}
