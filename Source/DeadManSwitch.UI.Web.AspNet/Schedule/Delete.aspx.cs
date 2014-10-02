using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeadManSwitch.UI.Web.AspNet.Schedule
{
    public partial class Delete : DMSPage
    {
        internal const string QryParmScheduleId = "id";
        internal const string QryParmScheduleType = "type";

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private DeleteSchedulePresenter Presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new DeleteSchedulePresenter(this.CurrentUser);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                var qry = ParseQryString(Request.QueryString);
                Presenter.DeleteSchedule(qry.Item2, qry.Item1);
                RedirectToScheduleView();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                this.ErrorMessage.Text = "The schedule could not be deleted.";
            }
        }

        private Tuple<string, string> ParseQryString(NameValueCollection qrystring)
        {
            string scheduleId = new string(qrystring[QryParmScheduleId].ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            string typeId = new string(qrystring[QryParmScheduleType].ToCharArray().Where(c => char.IsDigit(c)).ToArray());

            return new Tuple<string, string>(scheduleId, typeId);
        }

        private void RedirectToScheduleView()
        {
            Response.Redirect("~/Schedule/View");
        }
    }
}