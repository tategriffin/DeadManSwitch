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
    public class CheckInWindowsController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public CheckInWindowsController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        // GET CheckInWindows
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await AccountSvc.GetCheckInWindowOptionsAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

    }
}
