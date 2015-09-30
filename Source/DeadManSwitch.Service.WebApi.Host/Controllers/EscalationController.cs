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
    public class EscalationController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IEscalationService EscalationSvc;

        public EscalationController(IEscalationService escalationService)
        {
            EscalationSvc = escalationService;
        }

        [Route("Tasks/Escalate")]
        public async Task<IHttpActionResult> Index()
        {
            try
            {
                await EscalationSvc.RunAsync();

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
