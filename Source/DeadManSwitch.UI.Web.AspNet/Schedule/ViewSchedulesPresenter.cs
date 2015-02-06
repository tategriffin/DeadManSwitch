using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Schedule
{
    public class ViewSchedulesPresenter : DMSPagePresenter
    {
        private IScheduleService ScheduleSvc;
        private ICheckInService CheckInSvc;

        public ViewSchedulesPresenter(CurrentUser user)
            : base(user)
        {
            this.ScheduleSvc = GetService<IScheduleService>();
            this.CheckInSvc = GetService<ICheckInService>();

            this.Schedules = BuildUserSchedules();
            this.NextCheckInText = BuildNextCheckInText();
        }

        public List<ScheduleViewModel> Schedules { get; private set; }
        public string NextCheckInText { get; private set; }

        private List<ScheduleViewModel> BuildUserSchedules()
        {
            List<ScheduleViewModel> userSchedules;

            if (this.CurrentUser.IsAuthenticated)
            {
                userSchedules = this.GetUserSchedulesFromSvc();
            }
            else
            {
                userSchedules = new List<ScheduleViewModel>();
            }

            return userSchedules;
        }

        private List<ScheduleViewModel> GetUserSchedulesFromSvc()
        {
            List<ISchedule> allUserSchedules = this.ScheduleSvc.SearchAllSchedulesByUser(this.CurrentUser.UserName);
            
            //TODO: Begin - Move sorting to service
            allUserSchedules.Sort(this.CompareByEnabledAndName);
            this.FlagDisabledSchedules(allUserSchedules);
            //TODO: End - Move sorting to service

            return allUserSchedules.Take(10).ToScheduleViewModel();
        }

        private void FlagDisabledSchedules(IEnumerable<ISchedule> schedules)
        {
            var disabledSchedules = schedules.Where(s => s.Enabled == false).ToList();
            foreach (var item in disabledSchedules)
            {
                item.Name = "Disabled - " + item.Name;
            }
        }

        private int CompareByEnabledAndName(ISchedule current, ISchedule other)
        {
            if (current == null && other == null) return 0;
            if (current == null && other != null) return -1;
            if (current != null && other == null) return 1;

            //neither parm is null
            int enabledFlag = CompareByEnabled(current, other);
            if (enabledFlag != 0)
            {
                return enabledFlag;
            }
            else
            {
                return CompareByName(current, other);
            }

        }

        private int CompareByEnabled(ISchedule current, ISchedule other)
        {
            int result = current.Enabled.CompareTo(other.Enabled);

            //Show enabled first
            return (result == 0 ? result : (-1) * result);
        }

        private int CompareByName(ISchedule current, ISchedule other)
        {
            return current.Name.CompareTo(other.Name);
        }

        private string BuildNextCheckInText()
        {
            string text = string.Empty;
            if (this.CurrentUser.IsAuthenticated)
            {
                if (this.Schedules != null && this.Schedules.Count > 0)
                {
                    CheckInInfo info = this.CheckInSvc.FindLastUserCheckIn(this.CurrentUser.UserName);
                    text = "Your next scheduled check in is " + info.GetNextCheckInText();
                }
            }

            return text;
        }
    }
}
