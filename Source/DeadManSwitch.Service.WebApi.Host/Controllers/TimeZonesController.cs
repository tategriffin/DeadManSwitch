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
    public class TimeZonesController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public TimeZonesController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        // GET TimeZones
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await AccountSvc.GetSupportedTimeZonesAsync();

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
