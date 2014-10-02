using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;

namespace DeadManSwitch.Providers
{
    /// <summary>
    /// Provides funtionality to allow users to change their escalation
    /// procedures.
    /// </summary>
    /// <remarks>
    /// <see cref="EscalationProvider"/> for executing escalations.
    /// </remarks>
    public class UserEscalationProcedureProvider
    {
        private IUnityContainer Container;
        private IUserEscalationProcedureRepository UserEscalationProceduresRepository;

        public UserEscalationProcedureProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.Container = container;
            this.UserEscalationProceduresRepository = container.Resolve<IUserEscalationProcedureRepository>();
        }

        public EscalationProcedures FindByUserId(int userId)
        {
            if (userId == 0) throw new ArgumentException("userId is not valid.");

            EscalationProcedures procedures = this.UserEscalationProceduresRepository.FindByUserId(userId);
            if (procedures == null)
            {
                procedures = new EscalationProcedures(userId);
            }

            return procedures;
        }

        public void Save(User user, EscalationProcedures procedures)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (procedures == null) throw new ArgumentNullException("procedures");
            if (procedures.UserId == 0) throw new ArgumentException("procedures.UserId is not valid.");
            if (IsUserAuthorized(user, procedures) == false) throw new Exception("You're not authorized to save this schedule.");

            DateTime? nextCheckInDateTime = DetermineNextCheckIn(user);
            this.UserEscalationProceduresRepository.Upsert(procedures, nextCheckInDateTime);
        }

        private DateTime? DetermineNextCheckIn(User user)
        {
            CheckInProvider checkInPvdr = new CheckInProvider(this.Container);
            DateTime? nextCheckInDateTime = checkInPvdr.RecalculateNextCheckInForUser(user);

            return nextCheckInDateTime;
        }

        private bool IsUserAuthorized(User user, EscalationProcedures procedures)
        {
            bool isAuthorized = false;
            if (user.UserId == procedures.UserId)
            {
                isAuthorized = true;
            }

            return isAuthorized;
        }

    }
}
