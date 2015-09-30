using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public class LoginResponse
    {
        public bool IsSuccessful { get { return (User != null); } }
        public User User { get; set; }
        public List<string> LoginFailedUserMessageList { get; private set; } = new List<string>();
    }
}
