using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class ScheduleMapper
    {
        public static List<DeadManSwitch.Service.ISchedule> ToServiceInterface(this IEnumerable<DeadManSwitch.Service.Wcf.Schedule> source)
        {
            return new List<DeadManSwitch.Service.ISchedule>(source.ToServiceEntity());
        }

        public static DeadManSwitch.Service.ISchedule ToServiceInterface(this DeadManSwitch.Service.Wcf.Schedule source)
        {
            return source.ToServiceEntity();
        }

        public static List<DeadManSwitch.Service.Wcf.Schedule> ToWcfEnitity(this IEnumerable<DeadManSwitch.Service.ISchedule> source)
        {
            var result = new List<DeadManSwitch.Service.Wcf.Schedule>();
            foreach (var item in source)
            {
                result.Add(item.ToWcfEnitity());
            }

            return result;
        }

        public static DeadManSwitch.Service.Wcf.Schedule ToWcfEnitity(this DeadManSwitch.Service.ISchedule source)
        {
            DeadManSwitch.Service.Wcf.Schedule dest;
            switch (source.Interval)
            {
                case DeadManSwitch.RecurrenceInterval.Daily:
                    dest = new Service.Wcf.DailySchedule();
                    break;

                default:
                    throw new Exception("Interval id {0} is not supported.");
            }

            return dest.MapValuesFromServiceInterface(source);
        }

        public static List<DeadManSwitch.Service.Schedule> ToServiceEntity(this IEnumerable<DeadManSwitch.Service.Wcf.Schedule> source)
        {
            var result = new List<DeadManSwitch.Service.Schedule>();
            foreach (var item in source)
            {
                result.Add(item.ToServiceEntity());
            }

            return result;
        }

        public static DeadManSwitch.Service.Schedule ToServiceEntity(this DeadManSwitch.Service.Wcf.Schedule source)
        {
            DeadManSwitch.Service.Schedule dest;
            switch (source.Interval)
            {
                case (int)DeadManSwitch.RecurrenceInterval.Daily:
                    dest = new Service.DailySchedule();
                    break;

                default:
                    throw new Exception("Interval id {0} is not supported.");
            }

            return dest.MapValuesFromWcfEntity(source);
        }

        public static DeadManSwitch.Service.Schedule MapValuesFromWcfEntity(this DeadManSwitch.Service.Schedule dest, DeadManSwitch.Service.Wcf.Schedule source)
        {
            dest.Id = source.Id;
            dest.UniqueId = source.UniqueId;
            dest.Enabled = source.Enabled;
            dest.Description = source.Description;
            dest.Name = source.Name;

            return dest;
        }

        public static DeadManSwitch.Service.Wcf.Schedule MapValuesFromServiceInterface(this DeadManSwitch.Service.Wcf.Schedule dest, DeadManSwitch.Service.ISchedule source)
        {
            dest.Id = source.Id;
            dest.UniqueId = source.UniqueId;
            dest.Enabled = source.Enabled;
            dest.Description = source.Description;
            dest.Name = source.Name;

            return dest;
        }

    }
}
