using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action.KillSwitches;
using DeadManSwitch.Action.Processors;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Action
{
    public class ActionFactory
    {
        public IAction CreateAction(int actionType)
        {
            ActionType actionTypeAsEnum = (ActionType)actionType;

            return CreateAction(actionTypeAsEnum);
        }

        public IAction CreateAction(ActionType actionType)
        {
            IAction concreteAction;
            switch (actionType)
            {
                case ActionType.EmailMessage:
                    concreteAction = new SendEmailAction();
                    break;

                case ActionType.TextMessage:
                    concreteAction = new SendSMSAction();
                    break;

                case ActionType.None:
                default:
                    throw new ArgumentException(string.Format("ActionType '{0}' is not supported.", actionType));
            }

            return concreteAction;
        }

        internal IEscalationWorkItemProcessor CreateWorkItemProcessor(int actionType)
        {
            ActionType actionTypeAsEnum = (ActionType)actionType;

            return CreateWorkItemProcessor(actionTypeAsEnum);
        }

        internal IEscalationWorkItemProcessor CreateWorkItemProcessor(ActionType actionType)
        {
            IEscalationWorkItemProcessor concreteProcessor;
            switch (actionType)
            {
                case ActionType.EmailMessage:
                    concreteProcessor = new SendEmailProcessor();
                    break;

                case ActionType.TextMessage:
                    concreteProcessor = new SendSMSProcessor();
                    break;

                case ActionType.None:
                default:
                    throw new ArgumentException(string.Format("ActionType '{0}' is not supported.", actionType));
            }

            return concreteProcessor;
        }

    }
}
