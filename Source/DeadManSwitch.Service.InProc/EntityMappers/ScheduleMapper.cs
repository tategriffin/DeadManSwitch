using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.EntityMappers
{
    internal static class ScheduleMapper
    {
        public static List<DeadManSwitch.Service.ISchedule> ToServiceInterfaceList(this IEnumerable<DeadManSwitch.ISchedule> sourceEntity)
        {
            var targetEntity = new List<DeadManSwitch.Service.ISchedule>();

            foreach (var schedule in sourceEntity)
            {
                targetEntity.Add(schedule.ToServiceInterface());
            }

            return targetEntity;
        }

        public static DeadManSwitch.Service.ISchedule ToServiceInterface(this DeadManSwitch.ISchedule sourceEntity)
        {
            DeadManSwitch.Service.ISchedule targetEntity;

            switch (sourceEntity.Recurrence)
            {
                case RecurrenceInterval.Daily:
                    var entity = (DeadManSwitch.Schedule.DailySchedule)sourceEntity;
                    targetEntity = entity.ToServiceEntity();
                    break;
                    
                default:
                    throw new Exception("Unrecognized RecurrenceInterval.");
            }

            if (targetEntity.Interval == 0) throw new Exception("Unrecognized RecurrenceInterval.");
            
            return targetEntity;
        }

        internal static void MapBaseValues(this DeadManSwitch.Service.Schedule target, DeadManSwitch.ISchedule source)
        {
            target.Id = source.Id;
            //target.UniqueId = source.UserId;

            target.Description = source.Description;
            target.Enabled = source.Enabled;
            target.Name = source.Name;
        }

        internal static void MapBaseValues(this DeadManSwitch.ISchedule target, DeadManSwitch.Service.Schedule source)
        {
            target.Id = source.Id;
            //target.UniqueId = source.UserId;

            target.Enabled = source.Enabled;
            target.Name = source.Name;
        }


    }
}
