using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.AHCMS
{
    public class AHCMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AHCMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AHCMS_default",
                "AHCMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}