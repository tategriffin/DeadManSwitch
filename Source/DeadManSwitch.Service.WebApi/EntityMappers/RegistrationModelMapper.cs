using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.WebApi
{
    public static class RegistrationModelMapper
    {
        public static async Task<List<string>> ToRegistrationSuccess(this HttpResponseMessage source)
        {
            return await Task.FromResult(new List<string>());
        }

        public static async Task<List<string>> ToRegistrationFailure(this HttpResponseMessage source, string failureMessage)
        {
            var target = new List<string>();

            if (source.StatusCode == HttpStatusCode.BadRequest)
            {
                List<string> errorList = await source.ToModelStateErrorList();
                target.AddNonWhitespaceValues(errorList);
            }
            else
            {
                target.AddNonWhitespaceValue(failureMessage);
            }

            return target;
        }

    }

}
