using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Configuration;
using Newtonsoft.Json;

namespace DeadManSwitch.Service.WebApi.Proxy
{
    public class ActionServiceProxy : ServiceProxy, DeadManSwitch.Service.IActionService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public ActionServiceProxy(IHostSettingsReader config)
            : base(config) { }

        public async Task<Dictionary<int, string>> GetAllEscalationActionTypesAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("/EscalationActionTypes");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Dictionary<int, string>>();
            }
        }

        public async Task<Dictionary<int, string>> GetAllEscalationWaitMinutesAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync("/EscalationWaitMinutes");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Dictionary<int, string>>();
            }
        }

        public async Task<Service.EscalationStep> FindEscalationStepByIdAsync(string userName, int stepId)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"EscalationSteps/{stepId}");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<Service.EscalationStep>();
            }
        }

        public async Task<List<Service.EscalationStep>> FindAllEscalationStepsByUserNameAsync(string userName)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync($"EscalationSteps");
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<List<Service.EscalationStep>>();
            }
        }

        public async Task SaveEscalationStepAsync(string userName, Service.EscalationStep step)
        {
            using (var client = CreateHttpClient())
            {
                var jsonEscalationStep = JsonConvert.SerializeObject(step);
                HttpResponseMessage response;
                if (step.Id == 0)
                {
                    response = await client.PostAsync($"EscalationSteps/{step.Id}", BuildJsonHttpContent(jsonEscalationStep));
                }
                else
                {
                    response = await client.PutAsync($"EscalationSteps/{step.Id}", BuildJsonHttpContent(jsonEscalationStep));
                }

                response.EnsureSuccessStatusCode();
            }
        }

        public async Task SaveEscalationStepsAsync(string userName, IEnumerable<Service.EscalationStep> allSteps)
        {
            using (var client = CreateHttpClient())
            {
                var jsonEscalationSteps = JsonConvert.SerializeObject(allSteps);
                var response = await client.PutAsync($"EscalationSteps", BuildJsonHttpContent(jsonEscalationSteps));
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteEscalationStepAsync(string userName, int stepId)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.DeleteAsync($"EscalationSteps/{stepId}");
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<List<Service.EscalationStep>> ReorderEscalationStepsAsync(string userName, IEnumerable<int> orderedStepIds)
        {
            using (var client = CreateHttpClient())
            {
                var jsonEscalationSteps = JsonConvert.SerializeObject(orderedStepIds);
                var response = await client.PatchAsync($"EscalationSteps", BuildJsonHttpContent(jsonEscalationSteps));
                response.EnsureSuccessStatusCode();

                return await response.DeserializeResponseContentAsync<List<Service.EscalationStep>>();
            }
        }

    }
}
