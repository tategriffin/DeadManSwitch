using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public static class UserProfileMapper
    {
        public static UserProfileModel ToUiModel(this DeadManSwitch.Service.User source)
        {
            UserProfileModel target = new UserProfileModel();

            target.UserName = source.UserName;
            target.Email = source.Email;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;

            return target;
        }

        public static DeadManSwitch.Service.UserProfile ToServiceModel(this UserProfileModel source)
        {
            var target = new DeadManSwitch.Service.UserProfile();

            target.Email = source.Email;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;

            return target;
        }

    }
}
