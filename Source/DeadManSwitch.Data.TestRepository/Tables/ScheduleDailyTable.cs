using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Schedule;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class ScheduleDailyTable
    {
        private int TableRowIdentity;

        public ScheduleDailyTable()
        {
            TableRowIdentity = 1;
            this.Rows = BuildPersistentRows();
        }

        public List<DailySchedule> Rows { get; private set; }

        private List<DailySchedule> BuildPersistentRows()
        {
            List<DailySchedule> persistentRows = new List<DailySchedule>();

            persistentRows.Add(
                new DailySchedule() 
                { 
                    Id=TableRowIdentity++,
                    UserId = 1,
                    Name = "UserId1Schedule1",
                    CheckInTime = new TimeSpan(9, 0, 0),
                    CheckInWindowStartTime = new TimeSpan(0, 0, 0),
                    Monday = true,
                    Tuesday = true,
                    Wednesday = true,
                    Thursday = true,
                    Friday = true,
                });
                    

            return persistentRows;
        }

        private DailySchedule CreateDailySchedule(int userId)
        {
            DailySchedule schedule = new DailySchedule()
            {
                Id = TableRowIdentity++,
                UserId = userId,
            };

            return schedule;
        }

    }
}
