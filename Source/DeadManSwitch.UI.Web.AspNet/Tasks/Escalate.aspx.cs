using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;

namespace DeadManSwitch.UI.Web.AspNet.Tasks
{
    public partial class Escalate : DMSPage
    {
        private EscalationTaskPresenter Presenter;

        public Escalate()
            : base(allowUnauthenticatedUser: true, requireReauthentication: false) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Presenter = new EscalationTaskPresenter(this.CurrentUser);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            HttpStatusCode runStatusCode = Presenter.Run();
            switch (runStatusCode)
            {
                case HttpStatusCode.InternalServerError:
                    Response.Write("The requested task could not be completed.");
                    break;
                default:
                    Response.Write("OK");
                    break;
            }
        }

    }
}