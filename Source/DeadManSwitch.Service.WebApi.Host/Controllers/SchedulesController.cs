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
    public class SchedulesController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IScheduleService ScheduleService;
        private readonly IDailyScheduleService DailyScheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            ScheduleService = scheduleService;
            DailyScheduleService = scheduleService.DailyScheduleService;
        }

        // GET Schedules
        public async Task<IHttpActionResult> Get()
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");

            try
            {
                var result = await ScheduleService.SearchAllSchedulesByUserAsync(userName);

                return Ok(result.ToWebApiEnitity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // GET Schedules/5
        public async Task<IHttpActionResult> Get(int id)
        {
            int scheduleId = id;
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (scheduleId == 0) return BadRequest(nameof(scheduleId) + " is required and cannot be zero.");

            try
            {
                var result = await DailyScheduleService.FindByScheduleIdAsync(userName, scheduleId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST Schedules
        public async Task<IHttpActionResult> Post(DailySchedule schedule)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                schedule.Id = 0;
                await DailyScheduleService.SaveAsync(userName, schedule.ToServiceEntity());

                Log.Warn("Post should return Created");
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // PUT Schedules/5
        public async Task<IHttpActionResult> Put(int id, DailySchedule schedule)
        {
            int scheduleId = id;
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (scheduleId == 0) return BadRequest(nameof(scheduleId) + " is required and cannot be zero.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                schedule.Id = scheduleId;
                await DailyScheduleService.SaveAsync(userName, schedule.ToServiceEntity());

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // DELETE Schedules/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            Log.Warn("ScheduleTypeId is hard-coded to Daily");
            int scheduleTypeId = (int) RecurrenceInterval.Daily;
            int scheduleId = id;
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (scheduleId == 0) return BadRequest(nameof(scheduleId) + " is required and cannot be zero.");

            try
            {
                await ScheduleService.DeleteScheduleAsync(userName, scheduleTypeId, scheduleId);

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
