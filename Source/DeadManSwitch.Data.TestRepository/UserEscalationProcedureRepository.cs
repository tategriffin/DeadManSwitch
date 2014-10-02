using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class UserEscalationProcedureRepository : RepositoryWithContext, IUserEscalationProcedureRepository
    {
        public UserEscalationProcedureRepository(RepositoryContext context)
            :base(context) { }

        public void Upsert(EscalationProcedures userEscalationProcedures, DateTime? nextCheckInDateTime)
        {
            //Checkin user to clear work table
            this.RecordUserCheckIn(userEscalationProcedures.UserId, nextCheckInDateTime);

            IEnumerable<DeadManSwitch.Action.UserEscalationTask> existingRows =
                Context.UserEscalationActions
                .Where(r => r.UserId == userEscalationProcedures.UserId);

            //Delete existing
            foreach (var item in existingRows)
            {
                Context.UserEscalationActions.Remove(item);
            }

            //Add
            Context.UserEscalationActions.AddRange(userEscalationProcedures.EscalationList);
        }

        public EscalationProcedures FindByUserId(int userId)
        {
            EscalationProcedures procedures = null;

            List<DeadManSwitch.Action.UserEscalationTask> existingRows =
                Context.UserEscalationActions
                .Where(r => r.UserId == userId)
                .ToList();

            if (existingRows != null && existingRows.Count > 0)
            {
                procedures = new EscalationProcedures(userId, existingRows);
            }

            return procedures;
        }

        private void RecordUserCheckIn(int userId, DateTime? nextCheckIn)
        {
            CheckInRepository checkInRepository = new DeadManSwitch.Data.TestRepository.CheckInRepository(this.Context);

            checkInRepository.RecordCheckIn(userId, DateTime.UtcNow, nextCheckIn);
        }

    }
}
