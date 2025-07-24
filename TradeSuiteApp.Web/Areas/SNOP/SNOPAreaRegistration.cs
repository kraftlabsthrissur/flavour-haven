using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.SNOP
{
    public class SNOPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SNOP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SNOP_default",
                "SNOP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}