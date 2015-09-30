using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using DeadManSwitch.Service;
using DeadManSwitch.UI.Models.Builders;

namespace DeadManSwitch.UI.Web.AspNetMvc.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly ICheckInService CheckInSvc;
        private readonly IScheduleService ScheduleSvc;

        private readonly DailyScheduleModelBuilder ModelBuilder;

        public ScheduleController(IAccountService accountService, IScheduleService scheduleService, ICheckInService checkInService)
        {
            CheckInSvc = checkInService;
            ScheduleSvc = scheduleService;
            ModelBuilder = new DailyScheduleModelBuilder(accountService, scheduleService);
        }

        //
        // GET: /Schedule/
        public async Task<ActionResult> Index()
        {
            var builder = new ScheduleListModelBuilder(ScheduleSvc, CheckInSvc);
            var model = await builder.BuildModelAsync(User.Identity.Name);

            return View(model);
        }

        //
        // GET: /Schedule/Create
        public async Task<ActionResult> Create()
        {
            var model = await ModelBuilder.BuildModelForCreateAsync(User.Identity.Name);

            return View("Modify", model);
        }

        //
        // POST: /Schedule/Create
        [HttpPost]
        public async Task<ActionResult> Create(DailyScheduleEditModel scheduleModel) //FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DailySchedule schedule = scheduleModel.ToDailySchedule();
                    await ScheduleSvc.DailyScheduleService.SaveAsync(User.Identity.Name, schedule);

                    return RedirectToAction("Index");
                }
                
                //model state is not valid, so render the page again, keeping user data
                await ModelBuilder.PopulateModelNonPersistentInfoAsync(User.Identity.Name, scheduleModel);
                return View("Modify", scheduleModel);
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not create a new schedule. {1}", User.Identity.Name, ex);
                return View("Error");
            }
        }

        //
        // GET: /Schedule/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            DailySchedule schedule = await ScheduleSvc.DailyScheduleService.FindByScheduleIdAsync(User.Identity.Name, id);
            if (schedule == null)
            {
                Log.Warn("Schedule ID: {0} was not found for user {1}.", id, User.Identity.Name);
                return RedirectToAction("Create");
            }

            DailyScheduleEditModel model = await ModelBuilder.BuildModelForEditAsync(User.Identity.Name, schedule);
            return View("Modify", model);
        }

        //
        // POST: /Schedule/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, DailyScheduleEditModel scheduleModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DailySchedule schedule = scheduleModel.ToDailySchedule();
                    schedule.Id = id;
                    await ScheduleSvc.DailyScheduleService.SaveAsync(User.Identity.Name, schedule);

                    return RedirectToAction("Index");
                }

                //model state is not valid, so render the page again, keeping user data
                await ModelBuilder.PopulateModelNonPersistentInfoAsync(User.Identity.Name, scheduleModel);
                return View("Modify", scheduleModel);
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not edit schedule ID: {1}. {2}", User.Identity.Name, id, ex);
                return View("Error");
            } 
        }

        //
        // POST: /Schedule/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                //Delete FormCollection parm if it is not required
                await ScheduleSvc.DailyScheduleService.DeleteAsync(User.Identity.Name, id);

                return Json(new { redirectUrl = Url.Action("Index") });
//                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not delete schedule ID: {1}. {2}", User.Identity.Name, id, ex);
                return View("Error");
            }
        }
    }
}
