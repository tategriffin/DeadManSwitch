using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.UI.Models;

namespace DeadManSwitch.UI
{
    public static class UserProfileMapper
    {
        public static UserProfileEditModel ToUiEditModel(this DeadManSwitch.Service.User source)
        {
            var target = new UserProfileEditModel();

            MapServiceUserToUserProfile(source, target);

            return target;
        }

        public static UserProfileViewModel ToUiViewModel(this DeadManSwitch.Service.User source)
        {
            var target = new UserProfileViewModel();

            MapServiceUserToUserProfile(source, target);

            return target;
        }

        public static DeadManSwitch.Service.UserProfile ToServiceModel(this UserProfileEditModel source)
        {
            var target = new DeadManSwitch.Service.UserProfile();

            target.Email = source.Email;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;

            return target;
        }


        private static void MapServiceUserToUserProfile(DeadManSwitch.Service.User source, IUserProfile target)
        {
            target.UserName = source.UserName;
            target.Email = source.Email;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
        }
    }
}
