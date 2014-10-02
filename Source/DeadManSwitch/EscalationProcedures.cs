using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;

namespace DeadManSwitch
{
    public class EscalationProcedures
    {
        public EscalationProcedures()
        {
            this.SortedEscalationTasks = new SortedList<int, UserEscalationTask>();
        }

        public EscalationProcedures(int userId)
            : this()
        {
            this.UserId = userId;
        }

        public EscalationProcedures(int userId, IEnumerable<UserEscalationTask> actions)
            : this()
        {
            if (actions == null) throw new ArgumentNullException("actions");

            this.UserId = userId;
            try
            {
                foreach (var item in actions)
                {
                    SortedEscalationTasks.Add(item.ExecutionOrder, item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UserEscalationTasks must have unique ExecutionOrder values.", ex);
            }
        }

        private int UserIdValue;
        public int UserId
        {
            get { return UserIdValue; }
            set
            {
                if (0 == value) throw new ArgumentException("UserId cannot be 0.");
                UserIdValue = value;
            }
        }

        private SortedList<int, UserEscalationTask> SortedEscalationTasks { get; set; }
        public IReadOnlyList<UserEscalationTask> EscalationList { get { return SortedEscalationTasks.Values.ToList(); } }

        public IEnumerable<EscalationWorkItem> ToEscalationWorkItems(DateTime baseUtcStartTime)
        {
            if (baseUtcStartTime.Kind != DateTimeKind.Utc) throw new ArgumentException("baseUtcStartTime must be UtcTime");

            List<EscalationWorkItem> userWorkItems = new List<EscalationWorkItem>();

            DateTime nextTriggerTime = baseUtcStartTime;
            foreach (UserEscalationTask task in this.EscalationList)
            {
                nextTriggerTime = nextTriggerTime.Add(task.WaitTimeSpan);

                EscalationWorkItem workItem = new EscalationWorkItem(task);
                workItem.TriggerTime = nextTriggerTime;
                userWorkItems.Add(workItem);
            }

            return userWorkItems;
        }

    }
}
