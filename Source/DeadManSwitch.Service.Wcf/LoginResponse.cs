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
    public class LoginResponse
    {
        [DataMember]
        public bool IsSuccessful
        {
            get { return (User != null); }
            set { /* dummy set accessor for WCF */ }
        }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public List<string> LoginFailedUserMessageList { get; set; } = new List<string>();
    }
}
