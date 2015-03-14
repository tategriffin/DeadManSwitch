using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;

namespace DeadManSwitch.Data
{
    public interface IEscalationRepository
    {
        void Add(IEnumerable<EscalationWorkItem> actions);

        void RecordExecutionSuccess(EscalationWorkItem workItem);
        void RecordExecutionFailure(EscalationWorkItem workItem);

        void RemoveByUser(int userId);

        EscalationWorkItem LockNextUnexecuted(TimeSpan lockTimeout, int maxFailures);
    }
}
