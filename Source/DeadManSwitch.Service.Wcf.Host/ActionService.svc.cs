using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DeadManSwitch.Service.Wcf.EntityMappers;
using NLog;

namespace DeadManSwitch.Service.Wcf.Host
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ActionService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ActionService.svc or ActionService.svc.cs at the Solution Explorer and start debugging.
    public class ActionService : IActionService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public OperationResponse<Dictionary<int, string>> GetAllEscalationActionTypes()
        {
            OperationResponse<Dictionary<int, string>> response;
            try
            {
                var svc = new Service.ActionService(CurrentAppState.IoCContainer);
                var result = svc.GetAllEscalationActionTypes();

                response = new OperationResponse<Dictionary<int, string>>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<Dictionary<int, string>>("An error occurred while attempting to retrieve the supported action types.");
            }

            return response;
        }

        public OperationResponse<Dictionary<int, string>> GetAllEscalationWaitMinutes()
        {
            OperationResponse<Dictionary<int, string>> response;
            try
            {
                var svc = new Service.ActionService(CurrentAppState.IoCContainer);
                var result = svc.GetAllEscalationWaitMinutes();

                response = new OperationResponse<Dictionary<int, string>>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<Dictionary<int, string>>("An error occurred while attempting to retrieve the wait minutes.");
            }

            return response;
        }

        public OperationResponse<List<EscalationStep>> FindUserEscalationSteps(string userName)
        {
            OperationResponse<List<EscalationStep>> response;
            try
            {
                var svc = new Service.ActionService(CurrentAppState.IoCContainer);
                var result = svc.FindUserEscalationSteps(userName);

                response = new OperationResponse<List<EscalationStep>>(result.ToWcfEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<List<EscalationStep>>("An error occurred while attempting to retrieve the user escalation steps.");
            }

            return response;
        }

        public OperationResponse<bool> SaveUserEscalationSteps(string userName, IEnumerable<EscalationStep> allSteps)
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.ActionService(CurrentAppState.IoCContainer);
                svc.SaveUserEscalationSteps(userName, allSteps.ToServiceEntity());

                response = new OperationResponse<bool>(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to save the user escalation steps.");
            }

            return response;
        }
    }
}
