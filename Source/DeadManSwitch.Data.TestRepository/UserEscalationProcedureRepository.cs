using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Action;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class UserEscalationProcedureRepository : RepositoryWithContext, IUserEscalationProcedureRepository
    {
        public UserEscalationProcedureRepository(RepositoryContext context)
            :base(context) { }

        public UserEscalationTask FindTaskById(int userId, int taskId)
        {
            UserEscalationTask task = null;

            var procedures = FindProceduresByUserId(userId);
            if (procedures != null && procedures.EscalationList != null)
            {
                task = procedures.EscalationList.SingleOrDefault(t => t.Id == taskId);
            }

            return task;
        }

        public EscalationProcedures FindProceduresByUserId(int userId)
        {
            EscalationProcedures procedures = null;

            List<DeadManSwitch.Action.UserEscalationTask> existingRows =
                Context.UserEscalationActions
                .Where(r => r.UserId == userId)
                .ToList();

            if (existingRows.Count > 0)
            {
                procedures = new EscalationProcedures(userId, existingRows);
            }

            return procedures;
        }

        public void UpsertTask(UserEscalationTask userEscalationTask, DateTime? nextCheckInDateTime)
        {
            this.ClearEscalationWorkTableByCheckingInUser(userEscalationTask.UserId, nextCheckInDateTime);

            var existingTask = Context.UserEscalationActions.SingleOrDefault(t => t.Id == userEscalationTask.Id);
            if (existingTask == null)
            {
                Context.UserEscalationActions.Add(userEscalationTask);
            }
            else
            {
                int idx = Context.UserEscalationActions.IndexOf(existingTask);
                Context.UserEscalationActions[idx] = userEscalationTask;
            }

        }

        public void UpsertProcedures(EscalationProcedures userEscalationProcedures, DateTime? nextCheckInDateTime)
        {
            this.ClearEscalationWorkTableByCheckingInUser(userEscalationProcedures.UserId, nextCheckInDateTime);

            IEnumerable<DeadManSwitch.Action.UserEscalationTask> existingRows =
                Context.UserEscalationActions
                .Where(r => r.UserId == userEscalationProcedures.UserId);

            //Delete existing
            var itemsToDelete = existingRows.ToList();
            foreach (var item in itemsToDelete)
            {
                Context.UserEscalationActions.Remove(item);
            }

            //Add
            foreach (var item in userEscalationProcedures.EscalationList)
            {
                Context.UserEscalationActions.Add(item);
            }

        }

        public void DeleteTask(UserEscalationTask userEscalationTask, DateTime? nextCheckInDateTime)
        {
            this.ClearEscalationWorkTableByCheckingInUser(userEscalationTask.UserId, nextCheckInDateTime);

            var existingTask = Context.UserEscalationActions.SingleOrDefault(t => t.Id == userEscalationTask.Id);
            if (existingTask != null)
            {
                Context.UserEscalationActions.Remove(userEscalationTask);
            }
        }

        /// <summary>
        /// Clear any active escalations. It's unlikely, but it's important to clear
        /// the work table to prevent foreign key errors caused by deleting tasks
        /// which exist in the escalation work table.
        /// </summary>
        private void ClearEscalationWorkTableByCheckingInUser(int userId, DateTime? nextCheckIn)
        {
            CheckInRepository checkInRepository = new DeadManSwitch.Data.TestRepository.CheckInRepository(this.Context);

            checkInRepository.RecordCheckIn(userId, DateTime.UtcNow, nextCheckIn);
        }

    }
}
