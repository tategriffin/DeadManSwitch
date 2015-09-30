using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace DeadManSwitch.Service.WebApi.Host.Controllers
{
    public class PasswordController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public PasswordController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        // POST: Password
        public async Task<IHttpActionResult> Post(ChangePasswordModel credentials)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await AccountSvc.ChangePasswordAsync(credentials.UserName, credentials.OldPassword, credentials.NewPassword);
                if (result == false) return StatusCode((HttpStatusCode)HttpStatusCodeCustom.UnprocessableRequest);  //TODO: Is this correct?

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

    }
}
