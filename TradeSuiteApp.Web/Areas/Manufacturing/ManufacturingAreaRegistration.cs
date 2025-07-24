using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Manufacturing
{
    public class ManufacturingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Manufacturing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Manufacturing_default",
                "Manufacturing/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, lang = "en" }
            );
        }
    }
}