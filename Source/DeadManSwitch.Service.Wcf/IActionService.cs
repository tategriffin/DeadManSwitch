using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    [ServiceContract]
    public interface IActionService
    {
        [OperationContract]
        OperationResponse<Dictionary<int, string>> GetAllEscalationActionTypes();

        [OperationContract]
        OperationResponse<Dictionary<int, string>> GetAllEscalationWaitMinutes();

        [OperationContract]
        OperationResponse<List<EscalationStep>> FindUserEscalationSteps(string userName);

        [OperationContract]
        OperationResponse<bool> SaveUserEscalationSteps(string userName, IEnumerable<EscalationStep> allSteps);
    }
}
