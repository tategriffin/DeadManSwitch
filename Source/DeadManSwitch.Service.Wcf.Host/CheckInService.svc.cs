using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;

namespace DeadManSwitch.Service.Wcf.Host
{
    public class CheckInService : ICheckInService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public OperationResponse<CheckInInfo> CheckIn(string userName)
        {
            OperationResponse<CheckInInfo> response;
            try
            {
                var svc = new Service.CheckInService(CurrentAppState.IoCContainer);
                var result = svc.CheckInUser(userName);

                response = new OperationResponse<CheckInInfo>(result.ToWcfEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<CheckInInfo>("Checkin failed.");
            }

            return response;
        }

        public OperationResponse<CheckInInfo> FindCheckInInfo(string userName)
        {
            OperationResponse<CheckInInfo> response;
            try
            {
                var svc = new Service.CheckInService(CurrentAppState.IoCContainer);
                var result = svc.FindLastUserCheckIn(userName);

                response = new OperationResponse<CheckInInfo>(result.ToWcfEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<CheckInInfo>("An error occurred while attempting to find the last check in.");
            }

            return response;
        }
    }
}
