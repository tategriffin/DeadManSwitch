using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Schedule
{
    public class DeleteSchedulePresenter : DMSPagePresenter
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private IScheduleService ScheduleSvc;

        public DeleteSchedulePresenter(CurrentUser user)
            : base(user)
        {
            this.ScheduleSvc = GetService<IScheduleService>();
        }

        public void DeleteSchedule(RecurrenceInterval scheduleType, int scheduleId)
        {
            ScheduleSvc.DeleteSchedule(this.CurrentUser.UserName, (int)scheduleType, scheduleId);
        }

        public void DeleteSchedule(string scheduleType, string scheduleId)
        {
            RecurrenceInterval typeToDelete = ParseScheduleType(scheduleType);
            int idToDelete = ParseScheduleId(scheduleId);

            DeleteSchedule(typeToDelete, idToDelete);
        }

        private RecurrenceInterval ParseScheduleType(string qryValue)
        {
            int typeId = -1;

            if (string.IsNullOrWhiteSpace(qryValue) == false)
            {
                int value;
                if (int.TryParse(qryValue, out value))
                {
                    typeId = value;
                }
            }

            if(typeId < 0) throw new Exception("The specified schedule type is not valid.");
            return (RecurrenceInterval)typeId;
        }

        private int ParseScheduleId(string qryValue)
        {
            int? id = null;

            if (string.IsNullOrWhiteSpace(qryValue) == false)
            {
                int value;
                if (int.TryParse(qryValue, out value))
                {
                    id = value;
                }
            }

            if (id.HasValue == false || id.Value <= 0) throw new Exception("The specified schedule id is not valid.");
            return id.Value;
        }

    }
}
