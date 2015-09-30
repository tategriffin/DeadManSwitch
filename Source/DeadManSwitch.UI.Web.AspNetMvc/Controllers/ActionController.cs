using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            var steps = await ActionSvc.FindAllEscalationStepsByUserNameAsync(User.Identity.Name);
            var model = new UserActionListModel(steps.ToUiViewModelList());

            return View(model);
        }

        //
        // POST: /Action/Reorder
        [HttpPost]
        public async Task<ActionResult> Reorder(IEnumerable<int> stepOrder)
        {
            try
            {
                //TODO: This is inefficient. Implement client logic to change text on first item if necessary
                List<EscalationStep> existingSteps = await ActionSvc.FindAllEscalationStepsByUserNameAsync(User.Identity.Name);
                if (!existingSteps.Any()) throw new Exception($"No existing execution steps found for user {User.Identity.Name}");

                EscalationStep previousFirstStep = existingSteps.Single(s => s.Number == 1);
                List<EscalationStep> reorderedEscalationSteps = await ActionSvc.ReorderEscalationStepsAsync(User.Identity.Name, stepOrder);

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
        public async Task<ActionResult> Create()
        {
            var model = await ModelBuilder.BuildCreateModelAsync(User.Identity.Name);

            return View("Modify", model);
        }

        //
        // POST: /Action/Create
        [HttpPost]
        public async Task<ActionResult> Create(UserActionEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await ActionSvc.SaveEscalationStepAsync(User.Identity.Name, model.ToServiceEntity());

                    return RedirectToAction("Index");
                }

                //model state is not valid, so render the page again, keeping user data
                await ModelBuilder.PopulateModelNonPersistentInfoAsync(model);
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
        public async Task<ActionResult> Edit(int id)
        {
            var step = await ActionSvc.FindEscalationStepByIdAsync(User.Identity.Name, id);
            if (step == null)
            {
                Log.Warn("Step ID: {0} was not found for user {1}.", id, User.Identity.Name);
                return RedirectToAction("Create");
            }

            UserActionEditModel model = await ModelBuilder.BuildEditModelAsync(step);
            return View("Modify", model);
        }

        //
        // POST: /Action/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, UserActionEditModel stepModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await ActionSvc.SaveEscalationStepAsync(User.Identity.Name, stepModel.ToServiceEntity());

                    return RedirectToAction("Index");
                }

                //model state is not valid, so render the page again, keeping user data
                await ModelBuilder.PopulateModelNonPersistentInfoAsync(stepModel);
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
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await ActionSvc.DeleteEscalationStepAsync(User.Identity.Name, id);

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
