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
        private IUnityContainer Container;

        private UserProvider UserProvider;
        private CheckInProvider CheckInProvider;
        private EscalationProvider EscalationProvider;

        public CheckInService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.Container = container;
            this.UserProvider = new UserProvider(this.Container);
            this.CheckInProvider = new CheckInProvider(this.Container);
            this.EscalationProvider = new EscalationProvider(this.Container);
        }

        public CheckInInfo CheckInUser(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");

            var existingUser = UserProvider.FindByUserName(userName);
            return this.CheckInProvider.RecordCheckIn(existingUser).ToServiceEntity();
        }

        public CheckInInfo FindLastUserCheckIn(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");

            DeadManSwitch.User existingUser;

            if (UserProvider.TryFindByUserName(userName, out existingUser))
            {
                return this.CheckInProvider.FindLastCheckIn(existingUser).ToServiceEntity();
            }
            else
            {
                return null;
            }
        }

        public string GetUserFirstName(string userName)
        {
            string firstName = string.Empty;
            DeadManSwitch.User existingUser;

            bool found = this.UserProvider.TryFindByUserName(userName, out existingUser);
            return (found ? existingUser.FirstName : string.Empty);
        }

    }
}
