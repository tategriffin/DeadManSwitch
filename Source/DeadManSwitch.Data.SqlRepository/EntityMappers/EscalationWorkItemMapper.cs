using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class EscalationWorkItemMapper
    {
        public static SqlRepository.EscalationWorkTable ToDataEntity(this DeadManSwitch.Action.EscalationWorkItem domainEntity)
        {
            SqlRepository.EscalationWorkTable workTableItem = new EscalationWorkTable();
            workTableItem.EscalationWorkTableId = domainEntity.Id;
            workTableItem.CreateDate = DateTime.UtcNow;
            workTableItem.ModDate = workTableItem.CreateDate;
            workTableItem.LockExpiration = workTableItem.CreateDate;

            workTableItem.UserId = domainEntity.UserId;
            workTableItem.UserEscalationActionId = domainEntity.UserTaskId;
            workTableItem.TriggerDateTime = domainEntity.TriggerTime;
            workTableItem.NumberOfFailures = 0;
            workTableItem.Success = null;

            return workTableItem;
        }

        public static DeadManSwitch.Action.EscalationWorkItem ToDomainEntity(this SqlRepository.EscalationWorkTable dataEntity)
        {
            Action.ActionFactory factory = new Action.ActionFactory();
            Action.IAction action = factory.CreateAction(dataEntity.UserEscalationAction.EscalationActionTypeId);
            action.Recipient = dataEntity.UserEscalationAction.ActionTarget;
            action.Message = dataEntity.UserEscalationAction.ActionMessage;

            DeadManSwitch.Action.EscalationWorkItem domain = new Action.EscalationWorkItem();
            domain.Id = dataEntity.EscalationWorkTableId;
            domain.Action = action;
            domain.TriggerTime = dataEntity.TriggerDateTime;
            domain.UserId = dataEntity.UserId;
            domain.UserTaskId = dataEntity.UserEscalationActionId;

            return domain;
        }

    }
}
