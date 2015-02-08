using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI.Models
{
    public class UserProfileViewModel : IUserProfile
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //Preferences
        [DisplayName("Time Zone")]
        public string TimeZone { get; set; }
        [DisplayName("Check in window start")]
        public string EarlyCheckinDesc { get; set; }

        public string Message { get; set; }
    }
}
