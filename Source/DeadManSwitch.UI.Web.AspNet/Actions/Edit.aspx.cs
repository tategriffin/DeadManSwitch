using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Actions
{
    public partial class Edit : DMSPage
    {
        private EditAllUserActionsPresenter Presenter;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Presenter = new EditAllUserActionsPresenter(this.CurrentUser);

            this.ActionRepeater.DataSource = this.Presenter.Model.UserActions;
            this.ActionRepeater.DataBind();
        }

        /// <summary>
        /// Handles the ItemDataBound event of the ActionRepeater control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void ActionRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EditUserActionModel actionModel = (EditUserActionModel)e.Item.DataItem;

                HiddenField stepId = (HiddenField)e.Item.FindControl("StepId");
                Label stepNumber = (Label)e.Item.FindControl("StepNumber");
                DropDownList waitMinutesList = (DropDownList)e.Item.FindControl("StepWaitMinutes");
                DropDownList actionTypeList = (DropDownList)e.Item.FindControl("StepAction");
                TextBox stepRecipient = (TextBox)e.Item.FindControl("StepRecipient");

                PopulateDDL(waitMinutesList, this.Presenter.Model.WaitMinuteOptions);
                PopulateDDL(actionTypeList, this.Presenter.Model.ActionTypeOptions);

                stepId.Value = actionModel.Id.ToString();
                stepNumber.Text = actionModel.Step.ToString();
                waitMinutesList.SelectedValue = actionModel.WaitMinutes.ToString();
                actionTypeList.SelectedValue = ((int)actionModel.ActionType).ToString();
                stepRecipient.Text = actionModel.Recipient;

                if (actionModel.Step == 1)
                {
                    waitMinutesList.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Populates the drop down list.
        /// </summary>
        /// <param name="ddl">The drop down list.</param>
        /// <param name="options">The drop down list options.</param>
        private void PopulateDDL(DropDownList ddl, Dictionary<string, string> options)
        {
            ddl.Items.Clear();
            ddl.Items.AddRange(options.ToListItems());
        }

        /// <summary>
        /// Handles the Click event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            switch (button.CommandName)
            {
                case "Save":
                    SaveChanges();
                    break;
                case "Cancel":
                    RedirectToActionView();
                    break;
            }
        }

        private void SaveChanges()
        {
            List<EditUserActionModel> userActions = GatherUserActionsFromUI();
            try
            {
                Presenter.Save(userActions);
                RedirectToActionView();
            }
            catch (Exception)
            {
                this.ErrorMessage.Text = "The schedule could not be saved.";
            }

        }

        /// <summary>
        /// Gathers the user actions from the UI.
        /// </summary>
        /// <returns></returns>
        private List<EditUserActionModel> GatherUserActionsFromUI()
        {
            List<EditUserActionModel> userActions = new List<EditUserActionModel>();

            int stepNumber = 1;
            for (int i = 0; i < this.ActionRepeater.Items.Count; i++)
            {
                var item = this.ActionRepeater.Items[i];
                EditUserActionModel userAction = GatherUserAction(item);
                if (userAction != null)
                {
                    userAction.Step = stepNumber++;     //Only count non-null steps
                    userActions.Add(userAction);
                }
            }

            return userActions;
        }

        /// <summary>
        /// Gathers a single user action.
        /// </summary>
        /// <param name="item">The UI item.</param>
        /// <returns></returns>
        private EditUserActionModel GatherUserAction(System.Web.UI.WebControls.RepeaterItem item)
        {
            EditUserActionModel userAction = null;

            TextBox stepRecipient = (TextBox)item.FindControl("StepRecipient");
            if (string.IsNullOrWhiteSpace(stepRecipient.Text) == false)
            {
                HiddenField stepId = (HiddenField)item.FindControl("StepId");
                DropDownList waitMinutesList = (DropDownList)item.FindControl("StepWaitMinutes");
                DropDownList actionTypeList = (DropDownList)item.FindControl("StepAction");

                userAction = new EditUserActionModel();
                userAction.Id = this.GetStepIdFromUI(stepId);
                userAction.WaitMinutes = this.GetWaitMinutesFromUI(waitMinutesList);
                userAction.ActionType = this.GetActionTypeFromUI(actionTypeList);
                userAction.Recipient = stepRecipient.Text;
            }

            return userAction;
        }

        /// <summary>
        /// Gets the step id from UI.
        /// </summary>
        /// <param name="stepId">The step id.</param>
        /// <returns></returns>
        private int GetStepIdFromUI(HiddenField stepId)
        {
            int id;
            if (int.TryParse(stepId.Value, out id) == false)
            {
                id = 0;
            }

            return id;
        }

        /// <summary>
        /// Gets the wait minutes from UI.
        /// </summary>
        /// <param name="ddl">The wait minutes drop down list.</param>
        /// <returns></returns>
        private int GetWaitMinutesFromUI(DropDownList ddl)
        {
            string value = (string.IsNullOrWhiteSpace(ddl.SelectedValue) ? string.Empty : ddl.SelectedValue);

            int minutes;
            if (int.TryParse(value, out minutes) == false)
            {
                minutes = 0;
            }

            return minutes;
        }

        /// <summary>
        /// Gets the action type from UI.
        /// </summary>
        /// <param name="ddl">The action type drop down list.</param>
        /// <returns></returns>
        private ActionType GetActionTypeFromUI(DropDownList ddl)
        {
            string value = (string.IsNullOrWhiteSpace(ddl.SelectedValue) ? string.Empty : ddl.SelectedValue);

            int actionTypeId;
            if (int.TryParse(value, out actionTypeId) == false)
            {
                actionTypeId = 0;
            }

            return (ActionType)actionTypeId;
        }

        /// <summary>
        /// Redirects to action view.
        /// </summary>
        private void RedirectToActionView()
        {
            Response.Redirect("~/Actions/View");
        }

    }
}