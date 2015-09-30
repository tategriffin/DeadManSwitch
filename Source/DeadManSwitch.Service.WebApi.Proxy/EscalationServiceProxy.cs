using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Configuration;

namespace DeadManSwitch.Service.WebApi.Proxy
{
    public class EscalationServiceProxy : ServiceProxy, DeadManSwitch.Service.IEscalationService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public EscalationServiceProxy(IHostSettingsReader config)
            : base(config) { }

        public async Task<bool> RunAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("Task/Escalate");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<bool>();
            }
        } 
    }
}
