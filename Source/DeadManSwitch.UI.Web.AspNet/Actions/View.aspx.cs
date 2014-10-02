using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Actions
{
    public partial class View : DMSPage
    {
        private ViewUserActionsPresenter Presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new ViewUserActionsPresenter(this.CurrentUser);

            ShowUserActions();
        }

        private void ShowUserActions()
        {
            if (!Page.IsPostBack)
            {
                ActionRepeater.DataSource = this.Presenter.Model;
                ActionRepeater.DataBind();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            NoActionsLabel.Visible = (this.Presenter.Model.Count == 0);
        }

        protected void ActionRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ViewUserActionModel action = (ViewUserActionModel)e.Item.DataItem;

                //HyperLink deleteScheduleLink = (HyperLink)e.Item.FindControl("DeleteHyperLink");
                //HyperLink editScheduleLink = (HyperLink)e.Item.FindControl("EditHyperLink");
                Label stepNumber = (Label)e.Item.FindControl("StepNumber");
                Literal descLiteral = (Literal)e.Item.FindControl("StepDescription");

                //deleteScheduleLink.NavigateUrl = BuildDeleteLink(action.Id);
                //editScheduleLink.NavigateUrl = BuildEditLink(action.Id);
                stepNumber.Text = action.StepNumber.ToString();
                descLiteral.Text = action.StepDescription;
            }
        }

    }
}