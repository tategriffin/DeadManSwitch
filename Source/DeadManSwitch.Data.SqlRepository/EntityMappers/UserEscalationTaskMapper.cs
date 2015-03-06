using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class UserEscalationTaskMapper
    {
        private static Action.ActionFactory DomainActionFactory = new Action.ActionFactory();

        internal static void MapDomainToData(DeadManSwitch.Action.UserEscalationTask domain, SqlRepository.UserEscalationAction data)
        {
            data.UserId = domain.UserId;
            data.ExecutionOrder = domain.ExecutionOrder;
            data.WaitTicksAfterPreviousAction = domain.WaitTimeSpan.Ticks;
            data.EscalationActionTypeId = (int)domain.Action.ActionType;
            data.ActionTarget = domain.Action.Recipient;
            data.ActionMessage = domain.Action.Message;

        }

        internal static List<DeadManSwitch.Action.UserEscalationTask> ToDomain(this IEnumerable<SqlRepository.UserEscalationAction> data)
        {
            List<DeadManSwitch.Action.UserEscalationTask> domainItems = new List<Action.UserEscalationTask>();
            foreach (var item in data)
            {
                domainItems.Add(item.ToDomain());
            }

            return domainItems;
        }

        internal static DeadManSwitch.Action.UserEscalationTask ToDomain(this SqlRepository.UserEscalationAction data)
        {
            if (data == null) return null;
            DeadManSwitch.Action.UserEscalationTask domain = new Action.UserEscalationTask();
            
            domain.Id = data.UserEscalationActionId;
            domain.UserId = data.UserId;
            domain.ExecutionOrder = data.ExecutionOrder;
            domain.WaitTimeSpan = new TimeSpan(data.WaitTicksAfterPreviousAction);
            domain.Action = DomainActionFactory.CreateAction(data.EscalationActionTypeId);
            domain.Action.Recipient = data.ActionTarget;
            domain.Action.Message = data.ActionMessage;

            return domain;
        }

    }
}
