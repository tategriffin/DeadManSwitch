using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using NLog;

namespace DeadManSwitch.Service.WebApi.Host.Controllers
{
    public class LoginController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public LoginController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        // POST: Login
        public async Task<IHttpActionResult> Post(UserLoginModel credentials)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await AccountSvc.LoginAsync(credentials.UserName, credentials.Password);
                if (result.IsSuccessful) return Ok(result);

                return Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

    }
}
