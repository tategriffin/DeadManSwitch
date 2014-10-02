using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DeadManSwitch.Service.Wcf
{
    [ServiceContract]
    public interface IEscalationService
    {
        [OperationContract]
        bool Run();
    }
}
