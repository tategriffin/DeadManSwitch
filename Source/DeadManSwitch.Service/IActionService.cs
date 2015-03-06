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

        EscalationStep FindEscalationStepById(string userName, int stepId);
        List<EscalationStep> FindAllEscalationStepsByUserName(string userName);

        void SaveEscalationStep(string userName, EscalationStep step);
        void SaveEscalationSteps(string userName, IEnumerable<EscalationStep> allSteps);

        void DeleteEscalationStep(string userName, int stepId);

        List<EscalationStep> ReorderEscalationSteps(string userName, IEnumerable<int> orderedStepIds);
    }
}
