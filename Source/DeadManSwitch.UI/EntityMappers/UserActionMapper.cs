using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public static class UserActionMapper
    {
        public static List<DeadManSwitch.Service.EscalationStep> ToServiceEntityList(this IEnumerable<UserActionEditModel> sourceList)
        {
            var targetList = new List<DeadManSwitch.Service.EscalationStep>();

            foreach (var item in sourceList)
            {
                targetList.Add(item.ToServiceEntity());
            }

            return targetList;
        }

        public static DeadManSwitch.Service.EscalationStep ToServiceEntity(this UserActionEditModel source)
        {
            var target = new DeadManSwitch.Service.EscalationStep();

            target.Id = source.Id;
            target.Number = source.Step;
            target.ActionType = source.ActionType;
            target.WaitMinutes = source.WaitMinutes;
            target.Recipient = source.Recipient;

            return target;
        }

        public static List<UserActionEditModel> ToUiEditModelList(this IEnumerable<DeadManSwitch.Service.EscalationStep> sourceList)
        {
            var targetList = new List<UserActionEditModel>();

            foreach (var item in sourceList)
            {
                targetList.Add(item.ToUiEditModel());
            }

            return targetList;
        }

        public static UserActionEditModel ToUiEditModel(this DeadManSwitch.Service.EscalationStep source)
        {
            var target = new UserActionEditModel();

            target.Id = source.Id;
            target.Step = source.Number;
            target.ActionType = source.ActionType;
            target.WaitMinutes = source.WaitMinutes;
            target.Recipient = source.Recipient;

            return target;
        }

        public static List<UserActionViewModel> ToUiViewModelList(this IEnumerable<DeadManSwitch.Service.EscalationStep> sourceList)
        {
            var targetList = new List<UserActionViewModel>();

            bool isFirst = true;
            foreach (var item in sourceList)
            {
                targetList.Add(item.ToUiViewModel(isFirst));
                isFirst = false;
            }

            return targetList;
        }

        private static UserActionViewModel ToUiViewModel(this DeadManSwitch.Service.EscalationStep source, bool isFirstStep)
        {
            var target = new UserActionViewModel();

            target.Id = source.Id;
            target.StepNumber = source.Number;
            target.ActionType = source.ActionType;
            target.WaitMinutes = source.WaitMinutes;
            target.Recipient = source.Recipient;

            return target;
        }

    }
}
