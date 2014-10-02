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
    public class UserPreferences
    {
        [DataMember]
        public string TimeZoneId { get; set; }

        [DataMember]
        public TimeSpan EarlyCheckInOffset { get; set; }
    }
}
