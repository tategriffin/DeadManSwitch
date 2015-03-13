using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class EscalationWorkTable : Table<EscalationWorkTableRow>
    {
        private int TableKeyIdentity;

        public override void Add(EscalationWorkTableRow item)
        {
            item.Data.Id = ++TableKeyIdentity;
            base.Add(item);
        }
    }

    public class EscalationWorkTableRow
    {
        public Action.EscalationWorkItem Data { get; set; }
        public DateTime LockExpiration { get; set; }
        public bool? Success { get; set; }
        public int NumberOfFailures { get; set; }
    }

}
