using System.Web;
using System.Web.Mvc;

namespace DeadManSwitch.Service.WebApi.Host
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Using Application_Error instead
            //filters.Add(new HandleErrorAttribute());
        }
    }
}
