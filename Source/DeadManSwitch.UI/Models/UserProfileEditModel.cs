using DeadManSwitch.UI.Models;

namespace DeadManSwitch.UI
{
    public class UserProfileEditModel : IUserProfile
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
