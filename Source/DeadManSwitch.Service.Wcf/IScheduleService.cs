using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DeadManSwitch.Service.Wcf
{
    [ServiceContract]
    public interface IScheduleService
    {
        [OperationContract]
        OperationResponse<List<Schedule>> SearchAllSchedulesByUser(string userName);

        [OperationContract]
        OperationResponse<bool> DeleteSchedule(string userName, int scheduleTypeId, int scheduleId);

        #region DailyRecurrenceInterval
        [OperationContract]
        OperationResponse<DailySchedule> FindDailySchedule(string userName, int scheduleId);
        [OperationContract]
        OperationResponse<bool> SaveDailySchedule(string userName, DailySchedule schedule);
        #endregion

    }
}
