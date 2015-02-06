using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.EntityMappers
{
    internal static class DailyScheduleMapper
    {
        public static List<DeadManSwitch.Service.DailySchedule> ToServiceEntityList(this IEnumerable<DeadManSwitch.Schedule.DailySchedule> sourceEntity)
        {
            var targetEntity = new List<DeadManSwitch.Service.DailySchedule>();

            foreach (var schedule in sourceEntity)
            {
                targetEntity.Add(schedule.ToServiceEntity());
            }

            return targetEntity;
        }

        public static DeadManSwitch.Service.DailySchedule ToServiceEntity(this DeadManSwitch.Schedule.DailySchedule sourceEntity)
        {
            if (sourceEntity == null) return null;

            var targetEntity = new DeadManSwitch.Service.DailySchedule();
            targetEntity.MapBaseValues(sourceEntity);

            targetEntity.CheckInTime = sourceEntity.CheckInTime;
            targetEntity.CheckInWindowStartTime = sourceEntity.CheckInWindowStartTime;

            targetEntity.Monday = sourceEntity.Monday;
            targetEntity.Tuesday = sourceEntity.Tuesday;
            targetEntity.Wednesday = sourceEntity.Wednesday;
            targetEntity.Thursday = sourceEntity.Thursday;
            targetEntity.Friday = sourceEntity.Friday;
            targetEntity.Saturday = sourceEntity.Saturday;
            targetEntity.Sunday = sourceEntity.Sunday;

            return targetEntity;
        }

        public static DeadManSwitch.Schedule.DailySchedule ToDomainEntity(this DeadManSwitch.Service.DailySchedule sourceEntity)
        {
            if (sourceEntity == null) return null;

            var targetEntity = new DeadManSwitch.Schedule.DailySchedule();
            targetEntity.MapBaseValues(sourceEntity);

            targetEntity.CheckInTime = sourceEntity.CheckInTime;
            targetEntity.CheckInWindowStartTime = sourceEntity.CheckInWindowStartTime;

            targetEntity.Monday = sourceEntity.Monday;
            targetEntity.Tuesday = sourceEntity.Tuesday;
            targetEntity.Wednesday = sourceEntity.Wednesday;
            targetEntity.Thursday = sourceEntity.Thursday;
            targetEntity.Friday = sourceEntity.Friday;
            targetEntity.Saturday = sourceEntity.Saturday;
            targetEntity.Sunday = sourceEntity.Sunday;

            return targetEntity;
        }

    }
}
