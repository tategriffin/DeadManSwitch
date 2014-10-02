using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;

namespace DeadManSwitch.Service.Wcf.Host
{
    public class ScheduleService : IScheduleService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public OperationResponse<List<Schedule>> SearchAllSchedulesByUser(string userName)
        {
            OperationResponse<List<Schedule>> response;
            try
            {
                var svc = new Service.ScheduleService(CurrentAppState.IoCContainer);
                var result = svc.SearchAllSchedulesByUser(userName);

                response = new OperationResponse<List<Schedule>>(result.ToWcfEnitity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<List<Schedule>>("An error occurred while attempting to find the user schedules.");
            }

            return response;
        }

        public OperationResponse<bool> DeleteSchedule(string userName, int scheduleTypeId, int scheduleId)
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.ScheduleService(CurrentAppState.IoCContainer);
                svc.DeleteSchedule(userName, scheduleTypeId, scheduleId);

                response = new OperationResponse<bool>(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to delete the schedule.");
            }

            return response;
        }

        #region DailyRecurrenceInterval
        public OperationResponse<DailySchedule> FindDailySchedule(string userName, int scheduleId)
        {
            OperationResponse<DailySchedule> response;
            try
            {
                var svc = new Service.DailyScheduleService(CurrentAppState.IoCContainer);
                var result = svc.FindByScheduleId(userName, scheduleId);

                response = new OperationResponse<DailySchedule>(result.ToWcfEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<DailySchedule>("An error occurred while attempting to find the schedule.");
            }

            return response;
        }

        public OperationResponse<bool> SaveDailySchedule(string userName, DailySchedule schedule)
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.DailyScheduleService(CurrentAppState.IoCContainer);
                svc.Save(userName, schedule.ToServiceEntity());

                response = new OperationResponse<bool>(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to delete the schedule.");
            }

            return response;
        }
        #endregion

    }
}
