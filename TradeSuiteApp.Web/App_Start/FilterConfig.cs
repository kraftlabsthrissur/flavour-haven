using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.ActionFilters;
using TradeSuiteApp.Web.RolePrivileges;

namespace TradeSuiteApp.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RolePermissionAttribute());
            //filters.Add(new RolePermissionAttribute.HandleAntiforgeryTokenErrorAttribute()
            //{ ExceptionType = typeof(HttpAntiForgeryException) });
            filters.Add(new LocalizationAttribute("en"), 0);

            filters.Add(new LogActionFilter());
            filters.Add(new ApprovalActionFilter());
        }
    }
}
