using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class CheckInTable : Table<CheckInTableRow>
    {
    }

    public class CheckInTableRow
    {
        public int UserId { get; set; }
        public DateTime? LastCheckIn { get; set; }
        public DateTime? NextCheckIn { get; set; }
    }

}
