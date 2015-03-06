using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;

namespace DeadManSwitch.Data
{
    public interface IUserEscalationProcedureRepository
    {
        UserEscalationTask FindTaskById(int userId, int taskId);
        EscalationProcedures FindProceduresByUserId(int userId);

        void UpsertTask(UserEscalationTask userEscalationTask, DateTime? nextCheckInDateTime);
        void UpsertProcedures(EscalationProcedures userEscalationProcedures, DateTime? nextCheckInDateTime);

        void DeleteTask(UserEscalationTask userEscalationTask, DateTime? nextCheckInDateTime);
    }
}
