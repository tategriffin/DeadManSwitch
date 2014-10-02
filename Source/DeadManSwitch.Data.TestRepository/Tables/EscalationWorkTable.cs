using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class EscalationWorkTable
    {
        private int TableKeyIdentity;

        public EscalationWorkTable()
        {
            TableKeyIdentity = 0;
            this.Rows = BuildPersistentRows();
        }

        public List<EscalationWorkTableRow> Rows { get; private set; }
        public void AddRow(EscalationWorkTableRow row)
        {
            row.Data.Id = TableKeyIdentity++;
            Rows.Add(row);
        }

        private List<EscalationWorkTableRow> BuildPersistentRows()
        {
            List<EscalationWorkTableRow> persistentRows = new List<EscalationWorkTableRow>();

            return persistentRows;
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
