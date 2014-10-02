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
        void Upsert(EscalationProcedures userEscalationProcedures, DateTime? nextCheckInDateTime);

        EscalationProcedures FindByUserId(int userId);
    }
}
