using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch;
using DeadManSwitch.Data;
using DeadManSwitch.Providers;
using DeadManSwitch.Service.EntityMappers;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    /// <summary>
    /// Exposes services for check-ins.
    /// </summary>
    /// <remarks>
    /// This class does not contain business logic. It is a glue class that 
    /// interacts with one or more provider classes.
    /// </remarks>
    public class CheckInService : ICheckInService
    {
        private readonly UserProvider UserProvider;
        private readonly CheckInProvider CheckInProvider;

        public CheckInService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.UserProvider = new UserProvider(container);
            this.CheckInProvider = new CheckInProvider(container);
        }

        public Task<CheckInInfo> CheckInUserAsync(string userName)
        {
            return Task.FromResult(CheckInUser(userName));
        }

        public CheckInInfo CheckInUser(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");

            var existingUser = UserProvider.FindByUserName(userName);
            return this.CheckInProvider.RecordCheckIn(existingUser).ToServiceEntity();
        }

        public Task<CheckInInfo> FindLastUserCheckInAsync(string userName)
        {
            return Task.FromResult(FindLastUserCheckIn(userName));
        }

        public CheckInInfo FindLastUserCheckIn(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName), "userName cannot be null or empty.");

            DeadManSwitch.User existingUser = UserProvider.FindByUserName(userName);
            if (existingUser == null) return null;

            return this.CheckInProvider.FindLastCheckIn(existingUser).ToServiceEntity();
        }

    }
}
