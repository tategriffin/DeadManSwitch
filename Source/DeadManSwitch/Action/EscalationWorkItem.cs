using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Action
{
    public class EscalationWorkItem
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public EscalationWorkItem() { }
        public EscalationWorkItem(UserEscalationTask userTask)
        {
            if (userTask == null) throw new ArgumentNullException("userTask");

            this.Id = 0;
            this.UserId = userTask.UserId;
            this.UserTaskId = userTask.Id;
            this.Action = userTask.Action;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserTaskId { get; set; }
        public DateTime TriggerTime { get; set; }
        public IAction Action { get; set; }

        public override string ToString()
        {
            string result;
            try
            {
                result = this.BuildCustomToStringValue();
            }
            catch (Exception ex)
            {
                logger.Error("ToString failed with exception: {0}", ex);
                result = base.ToString();
            }

            return result;
        }

        private string BuildCustomToStringValue()
        {
            StringBuilder instanceValues = new StringBuilder();

            instanceValues.Append(this.GetType()).Append(".");
            instanceValues.Append("Id: ").Append(this.Id.ToString()).Append(" ");
            instanceValues.Append("UserId: ").Append(this.UserId.ToString()).Append(" ");
            instanceValues.Append("TriggerTime: ").Append(this.TriggerTime.ToString("yyyy-MM-dd HH:mm:ss")).Append(" ");

            if (this.Action == null)    //Not expected, but it could happen, so avoid null exception
            {
                instanceValues.Append("Action: null");
            }
            else
            {
                instanceValues.Append("Action: ").Append(this.Action.ActionType.ToString()).Append(" ");
                instanceValues.Append("Recipient: ").Append(this.Action.Recipient).Append(" ");
            }

            return instanceValues.ToString();
        }

    }
}
