using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI.Models
{
    public class UserProfileViewModel : IUserProfile
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Preferences
        public string TimeZone { get; set; }
        public string EarlyCheckinDesc { get; set; }

        public string Message { get; set; }
    }
}
