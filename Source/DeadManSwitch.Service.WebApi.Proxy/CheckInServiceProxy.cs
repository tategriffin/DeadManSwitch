using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Configuration;
using Newtonsoft.Json;

namespace DeadManSwitch.Service.WebApi.Proxy
{
    public class CheckInServiceProxy : ServiceProxy, DeadManSwitch.Service.ICheckInService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public CheckInServiceProxy(IHostSettingsReader config)
            : base(config) { }

        public async Task<Service.CheckInInfo> CheckInUserAsync(string userName)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.PostAsync("CheckIn", BuildJsonHttpContent(String.Empty));
                response.EnsureSuccessStatusCode();

                var dto = await response.DeserializeResponseContentAsync<WebApi.CheckInInfo>();
                return dto.ToServiceEntity();
            }
        }

        public async Task<Service.CheckInInfo> FindLastUserCheckInAsync(string userName)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("CheckIn");
                response.EnsureSuccessStatusCode();

                var dto = await response.DeserializeResponseContentAsync<WebApi.CheckInInfo>();
                return dto.ToServiceEntity();
            }
        }

    }
}
