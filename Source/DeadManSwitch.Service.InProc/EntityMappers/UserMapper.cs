using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.EntityMappers
{
    internal static class UserMapper
    {
        public static DeadManSwitch.Service.User ToServiceEntity(this DeadManSwitch.User domainUser)
        {
            if (domainUser == null) return null;

            var svcUser = new DeadManSwitch.Service.User();

            svcUser.Email = domainUser.Email;
            svcUser.FirstName = domainUser.FirstName;
            svcUser.LastName = domainUser.LastName;
            svcUser.UserId = domainUser.UserId;
            svcUser.UserName = domainUser.UserName;

            return svcUser;
        }

        public static DeadManSwitch.User ToDomainEntity(this DeadManSwitch.Service.User svcUser)
        {
            if (svcUser == null) return null;

            var domainUser = new DeadManSwitch.User();

            domainUser.Email = svcUser.Email;
            domainUser.FirstName = svcUser.FirstName;
            domainUser.LastName = svcUser.LastName;
            domainUser.UserId = svcUser.UserId;
            domainUser.UserName = svcUser.UserName;

            return domainUser;
        }

        public static void MapProfileToUser(DeadManSwitch.User user, DeadManSwitch.Service.UserProfile profile)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (profile == null) throw new ArgumentNullException("profile");

            user.Email = profile.Email;
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
        }
    }
}
