using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public static class UserActionMapper
    {
        public static List<DeadManSwitch.Service.EscalationStep> ToServiceEntityList(this IEnumerable<EditUserActionModel> sourceList)
        {
            var targetList = new List<DeadManSwitch.Service.EscalationStep>();

            foreach (var item in sourceList)
            {
                targetList.Add(item.ToServiceEntity());
            }

            return targetList;
        }

        public static DeadManSwitch.Service.EscalationStep ToServiceEntity(this EditUserActionModel source)
        {
            var target = new DeadManSwitch.Service.EscalationStep();

            target.Id = source.Id;
            target.Number = source.Step;
            target.ActionType = source.ActionType;
            target.WaitMinutes = source.WaitMinutes;
            target.Recipient = source.Recipient;

            return target;
        }

        public static List<EditUserActionModel> ToUiEditModelList(this IEnumerable<DeadManSwitch.Service.EscalationStep> sourceList)
        {
            var targetList = new List<EditUserActionModel>();

            foreach (var item in sourceList)
            {
                targetList.Add(item.ToUiEditModel());
            }

            return targetList;
        }

        public static EditUserActionModel ToUiEditModel(this DeadManSwitch.Service.EscalationStep source)
        {
            var target = new EditUserActionModel();

            target.Id = source.Id;
            target.Step = source.Number;
            target.ActionType = source.ActionType;
            target.WaitMinutes = source.WaitMinutes;
            target.Recipient = source.Recipient;

            return target;
        }

        public static List<ViewUserActionModel> ToUiViewModelList(this IEnumerable<DeadManSwitch.Service.EscalationStep> sourceList)
        {
            var targetList = new List<ViewUserActionModel>();

            bool isFirst = true;
            foreach (var item in sourceList)
            {
                targetList.Add(item.ToUiViewModel(isFirst));
                isFirst = false;
            }

            return targetList;
        }

        private static ViewUserActionModel ToUiViewModel(this DeadManSwitch.Service.EscalationStep source, bool isFirstStep)
        {
            var target = new ViewUserActionModel();

            target.Id = source.Id;
            target.StepNumber = source.Number;
            target.StepDescription = BuildActionDescription(source, isFirstStep);

            return target;
        }

        private static string BuildActionDescription(DeadManSwitch.Service.EscalationStep step, bool isFirstStep)
        {
            string desc = BuildStepPrefix(step, isFirstStep) + BuildStepDetails(step);

            return desc;
        }

        private static string BuildStepPrefix(DeadManSwitch.Service.EscalationStep step, bool isFirstStep)
        {
            if (isFirstStep)
            {
                return "If I miss a check in, ";
            }
            else
            {
                return string.Format("then wait {0} minutes, and ", step.WaitMinutes);
            }
        }

        private static string BuildStepDetails(DeadManSwitch.Service.EscalationStep step)
        {
            string desc;
            switch (step.ActionType)
            {
                case ActionType.EmailMessage:
                    desc = "send an email to " + step.Recipient;
                    break;

                case ActionType.TextMessage:
                    desc = "send a text to " + step.Recipient.ToPhoneNumber();
                    break;

                default:
                    desc = "do nothing";
                    break;
            }

            return desc;
        }
    }
}
