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
    public class EscalationWaitMinutesController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IActionService ActionService;

        public EscalationWaitMinutesController(IActionService actionService)
        {
            ActionService = actionService;
        }

        // GET EscalationWaitMinutes
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await ActionService.GetAllEscalationWaitMinutesAsync();

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
