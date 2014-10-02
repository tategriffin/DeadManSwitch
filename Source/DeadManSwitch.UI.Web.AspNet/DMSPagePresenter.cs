using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNet
{
    public abstract class DMSPagePresenter
    {
        protected DMSPagePresenter(CurrentUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            this.CurrentUser = user;
        }

        protected CurrentUser CurrentUser { get; private set; }

        protected IUnityContainer IocContainer
        {
            get { return CurrentAppState.IoCContainer; }
        }

        protected T GetService<T>()
        {
            return IocContainer.Resolve<T>();
        }

    }
}
