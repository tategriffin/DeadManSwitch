using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;

namespace DeadManSwitch.Service.WebApi
{
    public static class ScheduleMapper
    {
        public static List<DeadManSwitch.Service.WebApi.Schedule> ToWebApiEnitity(this IEnumerable<DeadManSwitch.Service.ISchedule> source)
        {
            var result = new List<DeadManSwitch.Service.WebApi.Schedule>();
            foreach (var item in source)
            {
                result.Add(item.ToWebApiEnitity());
            }

            return result;
        }

        public static DeadManSwitch.Service.WebApi.Schedule ToWebApiEnitity(this DeadManSwitch.Service.ISchedule source)
        {
            DeadManSwitch.Service.WebApi.Schedule dest;
            switch (source.Interval)
            {
                case DeadManSwitch.RecurrenceInterval.Daily:
                    dest = DailyScheduleMapper.ToWebApiEntity(((DeadManSwitch.Service.DailySchedule)source));
                    break;

                default:
                    throw new Exception("Interval id {0} is not supported.");
            }

            return dest.MapValuesFromServiceInterface(source);
        }

        public static async Task<List<DeadManSwitch.Service.ISchedule>> ToScheduleList(this HttpResponseMessage source)
        {
            var serializedContent = await source.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DeadManSwitch.Service.ISchedule>>(serializedContent, new ScheduleListJsonConverter());
        }

        public static DeadManSwitch.Service.Schedule ToSchedule(this DeadManSwitch.Service.WebApi.Schedule source)
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

            return dest.MapValuesFromWebApiEntity(source);
        }

        public static DeadManSwitch.Service.Schedule MapValuesFromWebApiEntity(this DeadManSwitch.Service.Schedule dest, DeadManSwitch.Service.WebApi.Schedule source)
        {
            dest.Id = source.Id;
            dest.UniqueId = source.UniqueId;
            dest.Enabled = source.Enabled;
            dest.Description = source.Description;

            return dest;
        }

        public static DeadManSwitch.Service.WebApi.Schedule MapValuesFromServiceInterface(this DeadManSwitch.Service.WebApi.Schedule dest, DeadManSwitch.Service.ISchedule source)
        {
            dest.Id = source.Id;
            dest.UniqueId = source.UniqueId;
            dest.Enabled = source.Enabled;
            dest.Description = source.Description;

            return dest;
        }

    }
}
