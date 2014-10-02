using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data.SqlRepository.EntityMappers;

namespace DeadManSwitch.Data.SqlRepository
{
    public class EscalationRepository : IEscalationRepository
    {
        public void Add(IEnumerable<Action.EscalationWorkItem> workItems)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                int userId = workItems.First().UserId;

                foreach (Action.EscalationWorkItem item in workItems)
                {
                    SqlRepository.EscalationWorkTable workTableItem = item.ToDataEntity();
                    context.EscalationWorkTables.Add(workTableItem);
                }

                this.ResetEscalationAttemptCount(context, userId);
                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        private void ResetEscalationAttemptCount(DeadManSwitchEntities context, int userId)
        {
            SqlRepository.EscalationAttempt attempt =
                context.EscalationAttempts
                .Where(a => a.UserId == userId)
                .SingleOrDefault();
            if(attempt != null)
            {
                context.EscalationAttempts.Remove(attempt);
            }
        }

        public void RecordExecutionSuccess(Action.EscalationWorkItem workItem)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var existingItem = context.EscalationWorkTables
                    .Where(w => w.EscalationWorkTableId == workItem.Id)
                    .SingleOrDefault();

                if (existingItem != null)
                {
                    DateTime now = DateTime.UtcNow;

                    EscalationActionLog logItem = BuildLogItem(now, workItem.Action.ActionType, ActionDirection.Outgoing);

                    existingItem.ModDate = now;
                    existingItem.Success = true;
                    existingItem.LockExpiration = now;
                    
                    context.EscalationActionLogs.Add(logItem);
                    context.SaveChanges();
                }
            }
            finally
            {
                context.Dispose();
            }
        }

        private EscalationActionLog BuildLogItem(DateTime createDate, ActionType actionType, ActionDirection direction)
        {
            EscalationActionLog logItem = new EscalationActionLog();
            logItem.CreateDate = createDate;
            logItem.EscalationActionTypeId = (int)actionType;
            logItem.Direction = direction.ToChar().ToString();

            return logItem;
        }

        public void RecordExecutionFailure(Action.EscalationWorkItem workItem)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var existingItem = context.EscalationWorkTables
                    .Where(w => w.EscalationWorkTableId == workItem.Id)
                    .SingleOrDefault();

                if (existingItem != null)
                {
                    existingItem.ModDate = DateTime.UtcNow;
                    existingItem.NumberOfFailures += 1;
                    context.SaveChanges();
                }
            }
            finally
            {
                context.Dispose();
            }
        }

        public void RemoveByUser(int userId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                this.RemoveByUser(context, userId);

                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        internal void RemoveByUser(DeadManSwitchEntities context, int userId)
        {
            var existingItems = context.EscalationWorkTables
                .Where(w => w.UserId == userId)
                .ToList();

            if (existingItems != null && existingItems.Count > 0)
            {
                foreach (var item in existingItems)
                {
                    context.EscalationWorkTables.Remove(item);
                }
                this.ResetEscalationAttemptCount(context, userId);
            }
        }

        public Action.EscalationWorkItem LockNextUnexecuted(TimeSpan lockTimeout, int maxFailures)
        {
            Action.EscalationWorkItem workItem = null;

            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                DateTime utcNow = DateTime.UtcNow;

                var existingItem = context.EscalationWorkTables
                    .Where(w => w.TriggerDateTime < utcNow)
                    .Where(w => w.Success == null || w.Success == false)
                    .Where(w => w.NumberOfFailures < maxFailures)
                    .Where(w => w.LockExpiration < utcNow)
                    .OrderBy(w => w.TriggerDateTime)
                    .FirstOrDefault();

                if (existingItem != null)
                {
                    existingItem.ModDate = utcNow;
                    existingItem.LockExpiration = utcNow.Add(lockTimeout);
                    context.SaveChanges();

                    workItem = existingItem.ToDomainEntity();
                }

                return workItem;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
