using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Configuration;
using Newtonsoft.Json;

using static DeadManSwitch.Service.WebApi.ScheduleMapper;

namespace DeadManSwitch.Service.WebApi.Proxy
{
    public class ScheduleServiceProxy : ServiceProxy, DeadManSwitch.Service.IScheduleService, DeadManSwitch.Service.IDailyScheduleService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public ScheduleServiceProxy(IHostSettingsReader config)
            : base(config) { }


        public async Task<List<ISchedule>> SearchAllSchedulesByUserAsync(string userName)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("Schedules");
                response.EnsureSuccessStatusCode();

                //TODO: Manually create concrete instances and add them to a list
                //return await response.DeserializeResponseContentAsync<List<ISchedule>>();
                return await response.ToScheduleList();
            }
        }

        public async Task DeleteScheduleAsync(string userName, int scheduleTypeId, int scheduleId)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.DeleteAsync($"Schedules/{scheduleId}");
                response.EnsureSuccessStatusCode();
            }
        }

        public IDailyScheduleService DailyScheduleService { get { return this; } }

        public async Task<Service.DailySchedule> FindByScheduleIdAsync(string userName, int scheduleId)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"Schedules/{scheduleId}");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Service.DailySchedule>();
            }
        }

        public async Task SaveAsync(string userName, Service.DailySchedule schedule)
        {
            if (schedule.Id == 0)
            {
                await AddDailyScheduleAsync(userName, schedule);
            }
            else
            {
                await UpdateDailyScheduleAsync(userName, schedule);
            }
        }

        private async Task AddDailyScheduleAsync(string userName, Service.DailySchedule schedule)
        {
            using (var client = CreateHttpClient())
            {
                var scheduleJson = JsonConvert.SerializeObject(schedule);
                var response = await client.PostAsync($"Schedules", BuildJsonHttpContent(scheduleJson));
                response.EnsureSuccessStatusCode();
            }
        }

        private async Task UpdateDailyScheduleAsync(string userName, Service.DailySchedule schedule)
        {
            using (var client = CreateHttpClient())
            {
                var scheduleJson = JsonConvert.SerializeObject(schedule);
                var response = await client.PutAsync($"Schedules/{schedule.Id}", BuildJsonHttpContent(scheduleJson));
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteAsync(string userName, int scheduleId)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.DeleteAsync($"Schedules/{scheduleId}");
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<Dictionary<int, string>> CheckInHourOptionsAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("CheckInHours");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Dictionary<int, string>>();
            }
        }

        public async Task<Dictionary<int, string>> CheckInMinuteOptionsAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("CheckInMinutes");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Dictionary<int, string>>();
            }
        }

        public async Task<Dictionary<string, string>> CheckInAmPmOptionsAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("CheckInAmPm");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Dictionary<string, string>>();
            }
        }
    }
}
