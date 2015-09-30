using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.WebApi
{
    public static class LoginModelMapper
    {
        public static async Task<DeadManSwitch.Service.LoginResponse> ToLoginResponseSuccess(this HttpResponseMessage source)
        {
            return await source.DeserializeResponseContentAsync<DeadManSwitch.Service.LoginResponse>();
        }

        public static async Task<DeadManSwitch.Service.LoginResponse> ToLoginResponseFailure(this HttpResponseMessage source, string failureMessage)
        {
            var target = new DeadManSwitch.Service.LoginResponse();

            if (source.StatusCode == HttpStatusCode.BadRequest)
            {
                List<string> errorList = await source.ToModelStateErrorList();
                target.LoginFailedUserMessageList.AddNonWhitespaceValues(errorList);
            }
            else
            {
                target.LoginFailedUserMessageList.AddNonWhitespaceValue(failureMessage);
            }

            return target;
        }

    }
}
