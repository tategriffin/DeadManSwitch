using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DeadManSwitch.Service.WebApi
{
    internal static class ModelStateMapper
    {
        private const string ModelStateKey = "ModelState";

        public static async Task<List<string>> ToModelStateErrorList(this HttpResponseMessage source)
        {
            var errorList = new List<string>();

            var content = await source.DeserializeResponseContentAsync<JObject>();
            var errors = content[ModelStateKey];

            foreach (var errorProperty in errors.OfType<JProperty>())
            {
                foreach (var error in errorProperty.Values())
                {
                    errorList.Add(error.ToString());
                }
            }

            return errorList;
        }


    }
}
