using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Action;
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

        public UserEscalationTask FindTaskById(int userId, int taskId)
        {
            if (userId == 0) throw new ArgumentException("userId is not valid.");
            if (taskId == 0) throw new ArgumentException("taskId is not valid.");

            var task = UserEscalationProceduresRepository.FindTaskById(userId, taskId);

            return task;
        }

        public EscalationProcedures FindProceduresByUserId(int userId)
        {
            if (userId == 0) throw new ArgumentException("userId is not valid.");

            EscalationProcedures procedures = this.UserEscalationProceduresRepository.FindProceduresByUserId(userId);
            if (procedures == null)
            {
                procedures = new EscalationProcedures(userId);
            }

            return procedures;
        }

        public void Save(User user, UserEscalationTask task)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (task == null) throw new ArgumentNullException("task");
            if (task.UserId == 0) throw new ArgumentException("task.UserId is not valid.");
            if (IsUserAuthorized(user, task) == false) throw new Exception("You're not authorized to modify this step.");

            DateTime? nextCheckInDateTime = DetermineNextCheckIn(user);
            UserEscalationProceduresRepository.UpsertTask(task, nextCheckInDateTime);
        }

        public void Save(User user, EscalationProcedures procedures)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (procedures == null) throw new ArgumentNullException("procedures");
            if (procedures.UserId == 0) throw new ArgumentException("procedures.UserId is not valid.");
            if (IsUserAuthorized(user, procedures) == false) throw new Exception("You're not authorized to modify these steps.");

            DateTime? nextCheckInDateTime = DetermineNextCheckIn(user);
            this.UserEscalationProceduresRepository.UpsertProcedures(procedures, nextCheckInDateTime);
        }

        public void Delete(User user, int taskId)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (taskId == 0) throw new ArgumentException("taskId is not valid.");

            var task = FindTaskById(user.UserId, taskId);
            Delete(user, task);
        }

        public void Delete(User user, UserEscalationTask task)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (task == null) throw new ArgumentNullException("task");
            if (task.UserId == 0) throw new ArgumentException("task.UserId is not valid.");
            if (IsUserAuthorized(user, task) == false) throw new Exception("You're not authorized to delete this step.");

            DateTime? nextCheckInDateTime = DetermineNextCheckIn(user);
            UserEscalationProceduresRepository.DeleteTask(task, nextCheckInDateTime);
        }

        public void ReorderSteps(User user, IEnumerable<int> requestedStepOrder)
        {
            var existingSteps = FindProceduresByUserId(user.UserId).EscalationList;
            var reorderedSteps = new List<UserEscalationTask>();

            int stepNumber = 1;
            foreach (var id in requestedStepOrder)
            {
                var task = existingSteps.SingleOrDefault(s => s.Id == id);
                if (task == null) throw new Exception(string.Format("No existing execution step with id {0} found for user {1}", id, user.UserName));

                task.ExecutionOrder = stepNumber++;
                reorderedSteps.Add(task);
            }

            var procedures = new EscalationProcedures(user.UserId, reorderedSteps);
            Save(user, procedures);
        }

        private DateTime? DetermineNextCheckIn(User user)
        {
            CheckInProvider checkInPvdr = new CheckInProvider(this.Container);
            DateTime? nextCheckInDateTime = checkInPvdr.RecalculateNextCheckInForUser(user);

            return nextCheckInDateTime;
        }

        private bool IsUserAuthorized(User user, UserEscalationTask task)
        {
            return (user.UserId == task.UserId);
        }

        private bool IsUserAuthorized(User user, EscalationProcedures procedures)
        {
            return (user.UserId == procedures.UserId);
        }

    }
}
