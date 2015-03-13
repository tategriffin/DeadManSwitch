using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Schedule;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class ScheduleDailyTable : Table<DailySchedule>
    {
        private int TableRowIdentity;

        public ScheduleDailyTable()
        {
            AddRange(BuildPersistentRows());
        }

        public override void Add(DailySchedule item)
        {
            item.Id = ++TableRowIdentity;

            base.Add(item);
        }

        public DailySchedule Update(DailySchedule schedule)
        {
            var existing = this.AsQueryable().SingleOrDefault(s => s.Id == schedule.Id);
            if (existing != null)
            {
                this.Remove(existing);
                this.Add(schedule);
            }

            return schedule;
        }

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

    }
}
