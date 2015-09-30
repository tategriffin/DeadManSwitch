using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace DeadManSwitch.Service.WebApi.Host.Controllers
{
    public class CheckInController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ICheckInService CheckInService;

        public CheckInController(ICheckInService checkInService)
        {
            CheckInService = checkInService;
        }

        // GET CheckIn
        public async Task<IHttpActionResult> Get()
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");

            try
            {
                var result = await CheckInService.FindLastUserCheckInAsync(userName);

                return Ok(result.ToWebApiEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST CheckIn
        public async Task<IHttpActionResult> Post()
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");

            try
            {
                var result = await CheckInService.CheckInUserAsync(userName);

                return Ok(result.ToWebApiEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

    }
}
