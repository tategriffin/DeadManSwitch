using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using DeadManSwitch.Service;
using DeadManSwitch.UI.Models.Builders;
using Microsoft.Ajax.Utilities;

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
        public ActionResult Index()
        {
            var model = new ScheduleListModelBuilder(ScheduleSvc, CheckInSvc).BuildModel(User.Identity.Name);

            return View(model);
        }

        //
        // GET: /Schedule/Create
        public ActionResult Create()
        {
            var model = ModelBuilder.BuildModelForCreate(User.Identity.Name);

            return View("Modify", model);
        }

        //
        // POST: /Schedule/Create
        [HttpPost]
        public ActionResult Create(DailyScheduleEditModel scheduleModel) //FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DailySchedule schedule = scheduleModel.ToDailySchedule();
                    ScheduleSvc.DailyScheduleService.Save(User.Identity.Name, schedule);

                    return RedirectToAction("Index");
                }
                
                //model state is not valid, so render the page again, keeping user data
                ModelBuilder.PopulateModelNonPersistentInfo(User.Identity.Name, scheduleModel);
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
        public ActionResult Edit(int id)
        {
            DailySchedule schedule = ScheduleSvc.DailyScheduleService.FindByScheduleId(User.Identity.Name, id);
            if (schedule == null)
            {
                Log.Warn("Schedule ID: {0} was not found for user {1}.", id, User.Identity.Name);
                return RedirectToAction("Create");
            }

            DailyScheduleEditModel model = ModelBuilder.BuildModelForEdit(User.Identity.Name, schedule);
            return View("Modify", model);
        }

        //
        // POST: /Schedule/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DailyScheduleEditModel scheduleModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DailySchedule schedule = scheduleModel.ToDailySchedule();
                    schedule.Id = id;
                    ScheduleSvc.DailyScheduleService.Save(User.Identity.Name, schedule);

                    return RedirectToAction("Index");
                }

                //model state is not valid, so render the page again, keeping user data
                ModelBuilder.PopulateModelNonPersistentInfo(User.Identity.Name, scheduleModel);
                return View("Modify", scheduleModel);
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not edit schedule ID: {1}. {2}", User.Identity.Name, id, ex);
                return View("Error");
            } 
        }

        //
        // GET: /Schedule/Delete/5
//        public ActionResult Delete(int id)
//        {
//            return View();
//        }

        //
        // POST: /Schedule/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                ScheduleSvc.DailyScheduleService.Delete(User.Identity.Name, id);

                return Json(new { redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not delete schedule ID: {1}. {2}", User.Identity.Name, id, ex);
                return View("Error");
            }
        }
    }
}
