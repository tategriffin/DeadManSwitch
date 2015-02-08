using System.ComponentModel;
using DeadManSwitch.UI.Models;

namespace DeadManSwitch.UI
{
    public class UserProfileEditModel : IUserProfile
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}
