using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.EntityMappers
{
    internal static class CheckInMapper
    {
        public static DeadManSwitch.Service.CheckInInfo ToServiceEntity(this DeadManSwitch.CheckInInfo domainEntity)
        {
            var serviceEntity = new DeadManSwitch.Service.CheckInInfo();

            serviceEntity.CheckInTimeUtc = domainEntity.CheckInTimeUtc;
            serviceEntity.NextCheckInTimeUtc = domainEntity.NextCheckInTimeUtc;
            serviceEntity.UserName = domainEntity.User.UserName;
            serviceEntity.UserTimeZone = domainEntity.UserTimeZone;

            serviceEntity.DisplayName = (string.IsNullOrWhiteSpace(domainEntity.User.FirstName)
                ? domainEntity.User.UserName
                : domainEntity.User.FirstName);

            return serviceEntity;
        }

    }
}
