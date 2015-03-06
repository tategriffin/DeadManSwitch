using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Service;
using DeadManSwitch.UI.Models;
using DeadManSwitch.UI.Models.Builders;

namespace DeadManSwitch.UI.Web.AspNetMvc.Controllers
{
    [Authorize]
    public class ActionController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly IActionService ActionSvc;

        private readonly UserActionModelBuilder ModelBuilder;

        public ActionController(IActionService actionService)
        {
            ActionSvc = actionService;

            ModelBuilder = new UserActionModelBuilder(actionService);
        }

        //
        // GET: /Action/
        public ActionResult Index()
        {
            var model = new UserActionListModel(ActionSvc.FindAllEscalationStepsByUserName(User.Identity.Name).ToUiViewModelList());

            return View(model);
        }

        //
        // POST: /Action/Reorder
        [HttpPost]
        public ActionResult Reorder(IEnumerable<int> stepOrder)
        {
            try
            {
                //TODO: This is inefficient. Implement client logic to change text on first item if necessary
                List<EscalationStep> existingSteps = ActionSvc.FindAllEscalationStepsByUserName(User.Identity.Name);
                if (!existingSteps.Any()) throw new Exception(string.Format("No existing execution steps found for user {0}", User.Identity.Name));

                EscalationStep previousFirstStep = existingSteps.Single(s => s.Number == 1);
                List<EscalationStep> reorderedEscalationSteps = ActionSvc.ReorderEscalationSteps(User.Identity.Name, stepOrder);

                if (previousFirstStep.Id == reorderedEscalationSteps.Single(s => s.Number == 1).Id)
                {
                    //No need to refresh since the first execution step did not change
                    return Json(new { redirectUrl = string.Empty });
                }

                //POSTed via ajax, so need to redirect rather than return a view.
                return Json(new { redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                Log.Error("Reorder failed.", ex);
                //POSTed via ajax, so need to redirect rather than return a view.
                return Json(new { redirectUrl = Url.Action("Index") });
            }
        }

        //
        // GET: /Action/Create
        public ActionResult Create()
        {
            var model = ModelBuilder.BuildCreateModel(User.Identity.Name);

            return View("Modify", model);
        }

        //
        // POST: /Action/Create
        [HttpPost]
        public ActionResult Create(UserActionEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ActionSvc.SaveEscalationStep(User.Identity.Name, model.ToServiceEntity());

                    return RedirectToAction("Index");
                }

                //model state is not valid, so render the page again, keeping user data
                ModelBuilder.PopulateModelNonPersistentInfo(model);
                return View("Modify", model);
            }
            catch(Exception ex)
            {
                Log.Error("User {0} could not add a new step. {1}", User.Identity.Name, ex);
                return View("Error");
            }
        }

        //
        // GET: /Action/Edit/5
        public ActionResult Edit(int id)
        {
            var step = ActionSvc.FindAllEscalationStepsByUserName(User.Identity.Name).SingleOrDefault(s => s.Id == id);
            if (step == null)
            {
                Log.Warn("Step ID: {0} was not found for user {1}.", id, User.Identity.Name);
                return RedirectToAction("Create");
            }

            UserActionEditModel model = ModelBuilder.BuildEditModel(step);
            return View("Modify", model);
        }

        //
        // POST: /Action/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserActionEditModel stepModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ActionSvc.SaveEscalationStep(User.Identity.Name, stepModel.ToServiceEntity());

                    return RedirectToAction("Index");
                }

                //model state is not valid, so render the page again, keeping user data
                ModelBuilder.PopulateModelNonPersistentInfo(stepModel);
                return View("Modify", stepModel);
            }
            catch(Exception ex)
            {
                Log.Error("User {0} could not edit step Id: {1}. {2}", User.Identity.Name, id, ex);
                return View("Error");
            }
        }

        //
        // POST: /Action/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                ActionSvc.DeleteEscalationStep(User.Identity.Name, id);

                return Json(new { redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                Log.Error("User {0} could not delete step Id: {1}. {2}", User.Identity.Name, id, ex);
                return View("Error");
            }
        }
    }
}
