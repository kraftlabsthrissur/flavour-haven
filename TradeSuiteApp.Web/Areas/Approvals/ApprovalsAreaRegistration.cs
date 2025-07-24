using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Approvals
{
    public class ApprovalsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Approvals";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Approvals_default",
                "Approvals/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, lang = "en" }
            );
        }
    }
}