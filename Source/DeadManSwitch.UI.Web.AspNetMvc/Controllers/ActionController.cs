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
            var model = new UserActionListModel(ActionSvc.FindUserEscalationSteps(User.Identity.Name).ToUiViewModelList());

            return View(model);
        }

        //
        // POST: /Action/Reorder
        [HttpPost]
        public ActionResult Reorder(IEnumerable<int> stepOrder)
        {
            try
            {
                List<EscalationStep> existingSteps = ActionSvc.FindUserEscalationSteps(User.Identity.Name);
                if (!existingSteps.Any()) throw new Exception(string.Format("No existing execution steps found for user {0}", User.Identity.Name));

                EscalationStep previousFirstStep = existingSteps.Single(s => s.Number == 1);
                List<EscalationStep> reorderedEscalationSteps = ReorderExecutionSteps(existingSteps, stepOrder);

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

        private List<EscalationStep> ReorderExecutionSteps(List<EscalationStep> existingSteps, IEnumerable<int> requestedStepOrder)
        {
            var reorderedSteps = new List<EscalationStep>();

            int stepNumber = 1;
            foreach (var id in requestedStepOrder)
            {
                var step = existingSteps.SingleOrDefault(s => s.Id == id);
                if (step == null) throw new Exception(string.Format("No existing execution step with id {0} found for user {1}", id, User.Identity.Name));

                step.Number = stepNumber++;
                reorderedSteps.Add(step);
            }

            ActionSvc.SaveUserEscalationSteps(User.Identity.Name, reorderedSteps);
            return reorderedSteps;
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
                    DeadManSwitch.Service.EscalationStep step = model.ToServiceEntity();
                    var allSteps = ActionSvc.FindUserEscalationSteps(User.Identity.Name);

                    allSteps.Add(step);
                    ActionSvc.SaveUserEscalationSteps(User.Identity.Name, allSteps);

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
            var step = ActionSvc.FindUserEscalationSteps(User.Identity.Name).SingleOrDefault(s => s.Id == id);
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
                    DeadManSwitch.Service.EscalationStep step = stepModel.ToServiceEntity();
                    var allSteps = ActionSvc.FindUserEscalationSteps(User.Identity.Name);
                    int idx = allSteps.FindIndex(s => s.Id == id);
                    if (idx < 0) throw new Exception(string.Format("Step Id: {0} was not found.", id));

                    allSteps.Insert(idx, step);
                    allSteps.RemoveAt(idx + 1);
                    ActionSvc.SaveUserEscalationSteps(User.Identity.Name, allSteps);

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
                var allSteps = ActionSvc.FindUserEscalationSteps(User.Identity.Name);
                int idx = allSteps.FindIndex(s => s.Id == id);
                if (idx < 0) throw new Exception(string.Format("Step Id: {0} was not found.", id));

                allSteps.RemoveAt(idx);
                ActionSvc.SaveUserEscalationSteps(User.Identity.Name, allSteps);

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
