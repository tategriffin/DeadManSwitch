using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Models.Builders
{
    public class ScheduleListModelBuilder
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly ICheckInService CheckInSvc;
        private readonly IScheduleService ScheduleSvc;

        public ScheduleListModelBuilder(IScheduleService scheduleService, ICheckInService checkInService)
        {
            CheckInSvc = checkInService;
            ScheduleSvc = scheduleService;
        }

        public ScheduleListModel BuildModel(string userName)
        {
            List<ISchedule> allUserSchedules = this.ScheduleSvc.SearchAllSchedulesByUser(userName);

            ScheduleListModel model = new ScheduleListModel(allUserSchedules.ToScheduleViewModel());
            if (model.Schedules.Any())
            {
                CheckInInfo info = CheckInSvc.FindLastUserCheckIn(userName);
                model.NextCheckInText = "Your next scheduled check in is " + info.GetNextCheckInText();
            }

            return model;
        }

    }
}