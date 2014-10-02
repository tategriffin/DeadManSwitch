using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    [ServiceContract]
    public interface ICheckInService
    {
        [OperationContract]
        OperationResponse<CheckInInfo> CheckIn(string userName);

        [OperationContract]
        OperationResponse<CheckInInfo> FindCheckInInfo(string userName);

    }
}
