using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface IActionService
    {
        Task<Dictionary<int, string>> GetAllEscalationActionTypesAsync();

        Task<Dictionary<int, string>> GetAllEscalationWaitMinutesAsync();

        Task<EscalationStep> FindEscalationStepByIdAsync(string userName, int stepId);
        Task<List<EscalationStep>> FindAllEscalationStepsByUserNameAsync(string userName);
        Task SaveEscalationStepAsync(string userName, EscalationStep step);
        Task SaveEscalationStepsAsync(string userName, IEnumerable<EscalationStep> allSteps);

        Task DeleteEscalationStepAsync(string userName, int stepId);

        Task<List<EscalationStep>> ReorderEscalationStepsAsync(string userName, IEnumerable<int> orderedStepIds);
    }
}
