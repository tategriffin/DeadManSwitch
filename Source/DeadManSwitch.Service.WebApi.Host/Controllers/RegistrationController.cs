using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using NLog;

namespace DeadManSwitch.Service.WebApi.Host.Controllers
{
    public class RegistrationController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public RegistrationController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        // GET Register
        [Route("Register")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                bool response = await AccountSvc.IsRegistrationOpenAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

    }
}
