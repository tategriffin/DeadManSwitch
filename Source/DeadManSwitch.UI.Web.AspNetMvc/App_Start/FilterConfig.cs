using System.Web;
using System.Web.Mvc;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
