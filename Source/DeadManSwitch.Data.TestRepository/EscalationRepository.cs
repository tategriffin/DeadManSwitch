using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class EscalationRepository : RepositoryWithContext, IEscalationRepository
    {
        public EscalationRepository(RepositoryContext context)
            :base(context) { }

        public Guid ContextId { get { return Context.Id; } }

        public void Add(IEnumerable<Action.EscalationWorkItem> actions)
        {
            DateTime utcNow = DateTime.UtcNow;

            foreach (var item in actions)
            {
                Tables.EscalationWorkTableRow row = new Tables.EscalationWorkTableRow();

                row.Data = item;
                row.NumberOfFailures = 0;
                row.Success = null;
                row.LockExpiration = utcNow;

                Context.EscalationWorkItems.Add(row);
            }
        }

        public void RecordExecutionSuccess(Action.EscalationWorkItem workItem)
        {
            Tables.EscalationWorkTableRow existingItem =
                Context.EscalationWorkItems
                .Single(w => w.Data.Id == workItem.Id);

            if (existingItem != null)
            {
                existingItem.Success = true;
            }
        }

        public void RecordExecutionFailure(Action.EscalationWorkItem workItem)
        {
            Tables.EscalationWorkTableRow existingItem =
                Context.EscalationWorkItems
                .Single(w => w.Data.Id == workItem.Id);

            if (existingItem != null)
            {
                existingItem.NumberOfFailures += 1;
            }
        }

        public void RemoveByUser(int userId)
        {
            List<Tables.EscalationWorkTableRow> existingRows =
                Context.EscalationWorkItems
                .Where(w => w.Data.UserId == userId)
                .ToList();

            if (existingRows != null)
            {
                foreach (var item in existingRows)
                {
                    Context.EscalationWorkItems.Remove(item);
                }
            }
        }

        public Action.EscalationWorkItem LockNextUnexecuted(TimeSpan lockTimeout, int maxFailures)
        {
            Action.EscalationWorkItem lockedItem = null;
            DateTime utcNow = DateTime.UtcNow;

            var existingItem = Context.EscalationWorkItems
                .Where(w => w.Data.TriggerTime < utcNow)
                .Where(w => w.Success == null || w.Success == false)
                .Where(w => w.NumberOfFailures < maxFailures)
                .Where(w => w.LockExpiration < utcNow)
                .OrderBy(w => w.Data.TriggerTime)
                .FirstOrDefault();

            if (existingItem != null)
            {
                existingItem.LockExpiration = utcNow.Add(lockTimeout);

                lockedItem = existingItem.Data;
            }

            return lockedItem;
        }

        public List<Tables.EscalationWorkTableRow> All()
        {
            return
                Context.EscalationWorkItems
                .ToList();
        }

        public List<Tables.EscalationWorkTableRow> FindByUserId(int userId)
        {
            return
                Context.EscalationWorkItems
                .Where(w => w.Data.UserId == userId)
                .ToList();
        }

    }
}
