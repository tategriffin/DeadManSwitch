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
    public class CheckInAmPmController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IDailyScheduleService DailyScheduleService;

        public CheckInAmPmController(IScheduleService scheduleService)
        {
            DailyScheduleService = scheduleService.DailyScheduleService;
        }

        // GET CheckinAmPm/
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await DailyScheduleService.CheckInAmPmOptionsAsync();

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
