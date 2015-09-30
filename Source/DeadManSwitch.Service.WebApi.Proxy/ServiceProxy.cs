using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Configuration;
using Newtonsoft.Json;

namespace DeadManSwitch.Service.WebApi.Proxy
{
    public abstract class ServiceProxy
    {
        protected ServiceProxy(IHostSettingsReader config)
        {
            ServiceLocation = new Uri(config.GetSetting<string>(DeadManSwitch.Configuration.ConfigurationKeys.ApiLocation));
        }

        protected Uri ServiceLocation { get; set; }

        protected HttpClient CreateHttpClient()
        {
            var client = new HttpClient {BaseAddress = ServiceLocation};

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpMediaType.ApplicationJson));

            //TODO: temporary "security" hack
            const string HeaderKey = "X-UserName";
            client.DefaultRequestHeaders.Add(HeaderKey, "testuser");

            return client;
        }

        protected HttpContent BuildJsonHttpContent(string json)
        {
            return new StringContent(json, Encoding.Unicode, HttpMediaType.ApplicationJson);
        }

    }
}
