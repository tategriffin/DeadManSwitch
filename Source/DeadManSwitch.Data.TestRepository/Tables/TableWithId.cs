using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal abstract class TableWithId<TRow> : Table<TRow>
    {
        private readonly object Padlock = new object();

        private int TableRowIdentity = 0;

        protected int GetNextIdentity()
        {
            lock (Padlock)
            {
                TableRowIdentity += 1;
                return TableRowIdentity;
            }
        }
    }
}
