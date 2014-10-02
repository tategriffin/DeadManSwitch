using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface IActionService
    {
        Dictionary<int, string> GetAllEscalationActionTypes();

        Dictionary<int, string> GetAllEscalationWaitMinutes();

        List<EscalationStep> FindUserEscalationSteps(string userName);

        void SaveUserEscalationSteps(string userName, IEnumerable<EscalationStep> allSteps);
    }
}
