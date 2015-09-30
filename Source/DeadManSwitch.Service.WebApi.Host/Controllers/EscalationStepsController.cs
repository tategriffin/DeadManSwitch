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
    public class EscalationStepsController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IActionService ActionService;

        public EscalationStepsController(IActionService actionService)
        {
            ActionService = actionService;
        }

        // GET EscalationSteps
        public async Task<IHttpActionResult> Get()
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");

            try
            {
                var result = await ActionService.FindAllEscalationStepsByUserNameAsync(userName);

                return Ok(result.ToWebApiEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // GET EscalationSteps/stepId
//        [Route("EscalationSteps/{id}", Name="GetEscalationStepById")]
        public async Task<IHttpActionResult> Get(int id)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (id == 0) return BadRequest(nameof(id) + " is required and cannot be zero.");

            try
            {
                var result = await ActionService.FindEscalationStepByIdAsync(userName, id);

                return Ok(result.ToWebApiEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST EscalationSteps
        public async Task<IHttpActionResult> Post(EscalationStep step)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await ActionService.SaveEscalationStepAsync(userName, step.ToServiceEntity());

//                return CreatedAtRoute("GetEscalationstepById", new {id = 1}, null);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // PUT EscalationSteps/stepId
        public async Task<IHttpActionResult> Put(int id,  EscalationStep step)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (id == 0) return BadRequest(nameof(id) + " is required and cannot be zero.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await ActionService.SaveEscalationStepAsync(userName, step.ToServiceEntity());

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // PUT EscalationSteps
        public async Task<IHttpActionResult> Put(IEnumerable<EscalationStep> steps)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");

            try
            {
                await ActionService.SaveEscalationStepsAsync(userName, steps.ToServiceEntity());

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // DELETE EscalationSteps/stepId
        public async Task<IHttpActionResult> Delete(int id)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (id == 0) return BadRequest(nameof(id) + " is required and cannot be zero.");

            try
            {
                await ActionService.DeleteEscalationStepAsync(userName, id);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // PATCH EscalationSteps
        public async Task<IHttpActionResult> Patch(IEnumerable<int> orderedStepIds)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");

            try
            {
                var result = await ActionService.ReorderEscalationStepsAsync(userName, orderedStepIds);

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
