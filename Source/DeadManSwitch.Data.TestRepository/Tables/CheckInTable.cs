using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class CheckInTable
    {
        public CheckInTable()
        {
            List<CheckInTableRow> persistentRows = BuildPersistentRows();

            Rows = persistentRows;
        }

        public List<CheckInTableRow> Rows { get; private set; }

        private static List<CheckInTableRow> BuildPersistentRows()
        {
            List<CheckInTableRow> persistentRows = new List<CheckInTableRow>();
            //careful what you add here. Every instance will see what you add.

            return persistentRows;
        }

    }

    public class CheckInTableRow
    {
        public int UserId { get; set; }
        public DateTime? LastCheckIn { get; set; }
        public DateTime? NextCheckIn { get; set; }
    }

}
