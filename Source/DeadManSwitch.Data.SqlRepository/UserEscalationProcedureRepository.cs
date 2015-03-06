using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Action;
using DeadManSwitch.Data.SqlRepository.EntityMappers;

namespace DeadManSwitch.Data.SqlRepository
{
    public class UserEscalationProcedureRepository : IUserEscalationProcedureRepository
    {
        public UserEscalationTask FindTaskById(int userId, int taskId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var userTask = context.UserEscalationActions
                    .SingleOrDefault(a => a.UserId == userId && a.UserEscalationActionId == taskId)
                    .ToDomain();

                return userTask;
            }
            finally
            {
                context.Dispose();
            }
        }

        public EscalationProcedures FindProceduresByUserId(int userId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                List<Action.UserEscalationTask> userActions = 
                    FindUserEscalationActionsByUserId(userId, context)
                    .ToDomain();

                EscalationProcedures userEscalationProcedures = new EscalationProcedures(userId, userActions);

                return userEscalationProcedures;
            }
            finally
            {
                context.Dispose();
            }
        }

        private List<SqlRepository.UserEscalationAction> FindUserEscalationActionsByUserId(int userId, DeadManSwitchEntities context)
        {
            return context.UserEscalationActions
                .Where(u => u.UserId == userId)
                .OrderBy(u => u.ExecutionOrder)
                .ToList();
        }

        public void UpsertTask(UserEscalationTask userEscalationTask, DateTime? nextCheckInDateTime)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                DateTime utcNow = DateTime.UtcNow;  //Use one time for all updates

                //Checkin user to clear work table
                this.ClearEscalationWorkTableByCheckingInUser(context, userEscalationTask.UserId, nextCheckInDateTime);

                UserEscalationAction existingItem = null;
                if (userEscalationTask.Id != 0)
                {
                    existingItem = context.UserEscalationActions
                        .SingleOrDefault(u => u.UserEscalationActionId == userEscalationTask.Id);

                }

                if (existingItem == null)
                {
                    Insert(userEscalationTask, context, utcNow);
                }
                else
                {
                    Update(userEscalationTask, existingItem, utcNow);
                }

                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        public void UpsertProcedures(EscalationProcedures userEscalationProcedures, DateTime? nextCheckInDateTime)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                DateTime utcNow = DateTime.UtcNow;  //Use one time for all updates

                //Checkin user to clear work table
                this.ClearEscalationWorkTableByCheckingInUser(context, userEscalationProcedures.UserId, nextCheckInDateTime);

                //Retrieve all existing items for user
                List<SqlRepository.UserEscalationAction> itemsToDelete = FindUserEscalationActionsByUserId(userEscalationProcedures.UserId, context);

                foreach (var item in userEscalationProcedures.EscalationList)
                {
                    SqlRepository.UserEscalationAction existingItem = null;
                    if (item.Id != 0)
                    {
                        existingItem = itemsToDelete
                            .Where(u => u.UserEscalationActionId == item.Id)
                            .SingleOrDefault();

                    }

                    if (existingItem == null)
                    {
                        Insert(item, context, utcNow);
                    }
                    else
                    {
                        itemsToDelete.Remove(existingItem);     //Make sure we don't delete this one
                        Update(item, existingItem, utcNow);
                    }
                }

                //Delete any items that are not in userEscalationProcedures.EscalationList
                Delete(itemsToDelete, context);

                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        public void DeleteTask(UserEscalationTask userEscalationTask, DateTime? nextCheckInDateTime)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                //Checkin user to clear work table
                this.ClearEscalationWorkTableByCheckingInUser(context, userEscalationTask.UserId, nextCheckInDateTime);

                UserEscalationAction existingItem;
                if (userEscalationTask.Id != 0)
                {
                    existingItem = context.UserEscalationActions
                        .SingleOrDefault(u => u.UserEscalationActionId == userEscalationTask.Id);

                    if (existingItem != null)
                    {
                        context.UserEscalationActions.Remove(existingItem);
                    }
                }

                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        /// <summary>
        /// Clear any active escalations. It's unlikely, but it's important to clear
        /// the work table to prevent foreign key errors caused by deleting tasks
        /// which exist in the escalation work table.
        /// </summary>
        private void ClearEscalationWorkTableByCheckingInUser(DeadManSwitchEntities context, int userId, DateTime? nextCheckIn)
        {
            CheckInRepository checkInRepository = new DeadManSwitch.Data.SqlRepository.CheckInRepository();

            checkInRepository.RecordCheckIn(context, userId, DateTime.UtcNow, nextCheckIn);
        }

        private void Insert(DeadManSwitch.Action.UserEscalationTask task, DeadManSwitchEntities context, DateTime utcNow)
        {
            SqlRepository.UserEscalationAction newItem = new UserEscalationAction();
            newItem.UserEscalationActionId = 0;

            UserEscalationTaskMapper.MapDomainToData(task, newItem);
            newItem.CreateDate = utcNow;
            newItem.ModDate = utcNow;

            context.UserEscalationActions.Add(newItem);
        }

        private void Update(DeadManSwitch.Action.UserEscalationTask task, SqlRepository.UserEscalationAction existingItem, DateTime utcNow)
        {
            UserEscalationTaskMapper.MapDomainToData(task, existingItem);
            existingItem.ModDate = utcNow;
        }

        private void Delete(IEnumerable<SqlRepository.UserEscalationAction> itemsToDelete, DeadManSwitchEntities context)
        {
            foreach (var item in itemsToDelete)
            {
                context.UserEscalationActions.Remove(item);
            }
        }

    }
}
