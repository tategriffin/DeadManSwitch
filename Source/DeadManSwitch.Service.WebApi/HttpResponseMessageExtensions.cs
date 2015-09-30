using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeadManSwitch.Service.WebApi
{
    public static class HttpResponseMessageExtensions
    {
        public static bool IsClientError(this HttpResponseMessage responseMessage)
        {
            return (responseMessage.StatusCode >= HttpStatusCode.BadRequest && responseMessage.StatusCode < HttpStatusCode.InternalServerError);
        }

        public static bool IsServerError(this HttpResponseMessage responseMessage)
        {
            return (responseMessage.StatusCode >= HttpStatusCode.InternalServerError);
        }

        public static async Task<T> DeserializeResponseContentAsync<T>(this HttpResponseMessage responseMessage)
        {
            var serializedContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(serializedContent);
        }

    }
}
