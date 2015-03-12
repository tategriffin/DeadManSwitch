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
            TableRowIdentity = 0;
            this.Rows = BuildPersistentRows();
        }

        public DailySchedule Add(DailySchedule schedule)
        {
            schedule.Id = ++TableRowIdentity;

            Rows.Add(schedule);

            return schedule;
        }

        public DailySchedule Update(DailySchedule schedule)
        {
            var existing = Rows.SingleOrDefault(s => s.Id == schedule.Id);
            if (existing != null)
            {
                Rows.Remove(existing);
                Rows.Add(schedule);
            }

            return schedule;
        }

        public List<DailySchedule> Rows { get; private set; }

        private List<DailySchedule> BuildPersistentRows()
        {
            List<DailySchedule> persistentRows = new List<DailySchedule>();

            DateTime checkInTime = DateTime.Now.AddMinutes(60);
            DateTime checkInStartTime = checkInTime.AddMinutes(-30);
            persistentRows.Add(
                new DailySchedule(true) 
                { 
                    Id=++TableRowIdentity,
                    UserId = 1,
                    Name = "UserId1Schedule1",
                    CheckInTime = new TimeSpan(checkInTime.Hour, checkInTime.Minute, 0),
                    CheckInWindowStartTime = new TimeSpan(checkInStartTime.Hour, checkInStartTime.Minute, 0),
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
