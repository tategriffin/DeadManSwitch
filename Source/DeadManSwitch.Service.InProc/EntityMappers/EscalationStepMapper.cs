using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.EntityMappers
{
    internal static class EscalationStepMapper
    {
        public static List<DeadManSwitch.Action.UserEscalationTask> ToUserEscalationTasks(this IEnumerable<EscalationStep> steps, int userId)
        {
            List<DeadManSwitch.Action.UserEscalationTask> taskList = new List<DeadManSwitch.Action.UserEscalationTask>();

            foreach (var item in steps)
            {
                taskList.Add(item.ToUserEscalationTask(userId));
            }

            return taskList;
        }

        public static DeadManSwitch.Action.UserEscalationTask ToUserEscalationTask(this EscalationStep step, int userId)
        {
            DeadManSwitch.Action.UserEscalationTask task = new DeadManSwitch.Action.UserEscalationTask();

            task.Id = step.Id;
            task.Action = MapAction(step.ActionType, step.Recipient);
            task.ExecutionOrder = step.Number;
            task.UserId = userId;
            task.WaitTimeSpan = new TimeSpan(0, step.WaitMinutes, 0);

            return task;
        }

        public static List<DeadManSwitch.Service.EscalationStep> ToEscalationSteps(this IEnumerable<DeadManSwitch.Action.UserEscalationTask> tasks)
        {
            List<DeadManSwitch.Service.EscalationStep> stepList = new List<DeadManSwitch.Service.EscalationStep>();

            foreach (var item in tasks)
            {
                stepList.Add(item.ToEscalationStep());
            }

            return stepList;
        }

        public static DeadManSwitch.Service.EscalationStep ToEscalationStep(this DeadManSwitch.Action.UserEscalationTask task)
        {
            DeadManSwitch.Service.EscalationStep step = new DeadManSwitch.Service.EscalationStep();

            step.Id = task.Id;
            step.ActionType = task.Action.ActionType;
            step.Recipient = task.Action.Recipient;
            task.Action = MapAction(step.ActionType, step.Recipient);
            step.Number = task.ExecutionOrder;
            step.WaitMinutes = (int)task.WaitTimeSpan.TotalMinutes;

            return step;
        }

        private static DeadManSwitch.Action.IAction MapAction(ActionType actionType, string recipient)
        {
            DeadManSwitch.Action.ActionFactory factory = new DeadManSwitch.Action.ActionFactory();

            DeadManSwitch.Action.IAction action = factory.CreateAction(actionType);
            action.Recipient = recipient;

            return action;
        }
    }

}
