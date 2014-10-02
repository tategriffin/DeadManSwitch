using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Action
{
    public class UserEscalationTask
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExecutionOrder { get; set; }
        public TimeSpan WaitTimeSpan { get; set; }
        public IAction Action { get; set; }

        public static int CompareExecutionOrder(UserEscalationTask first, UserEscalationTask second)
        {
            return first.ExecutionOrder.CompareTo(second);
        }
    }
}
