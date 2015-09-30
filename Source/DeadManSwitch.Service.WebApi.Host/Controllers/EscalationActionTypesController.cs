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
    public class EscalationActionTypesController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IActionService ActionService;

        public EscalationActionTypesController(IActionService actionService)
        {
            ActionService = actionService;
        }

        // GET EscalationActionTypes
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await ActionService.GetAllEscalationActionTypesAsync();

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
