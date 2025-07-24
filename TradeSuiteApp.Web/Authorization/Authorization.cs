using BusinessObject;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using PresentationContractLayer;

namespace TradeSuiteApp.Web.Authorization
{
    public static class ActionAuthorization
    {

        public static MvcHtmlString IsAuthorized(string Action, string Button)
        {
            IGeneralContract generalBL = new GeneralBL();
            System.Web.Routing.RouteData RouteData = HttpContext.Current.Request.RequestContext.RouteData;
            string Area = RouteData.DataTokens["area"]?.ToString();
            string Controller = RouteData.Values["controller"]?.ToString();

            if (generalBL.IsAuthorised(Area, Controller, Action, "", "Action"))
            {
                return new MvcHtmlString(Button);
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString IsAuthorized(string Action, MvcHtmlString Button)
        {
            IGeneralContract generalBL = new GeneralBL();
            System.Web.Routing.RouteData RouteData = HttpContext.Current.Request.RequestContext.RouteData;
            string Area = RouteData.DataTokens["area"]?.ToString();
            string Controller = RouteData.Values["controller"]?.ToString();

            if (generalBL.IsAuthorised(Area, Controller, Action, "", "Action"))
            {
                return Button;
            }
            return MvcHtmlString.Empty;
        }
    }

    public static class TabAuthorization
    {
        public static MvcHtmlString IsAuthorized(string Name, string Tab)
        {
            IGeneralContract generalBL = new GeneralBL();
            System.Web.Routing.RouteData RouteData = HttpContext.Current.Request.RequestContext.RouteData;
            string Area = RouteData.DataTokens["area"]?.ToString();
            string Controller = RouteData.Values["controller"]?.ToString();

            if (generalBL.IsAuthorised(Area, Controller, "", Name, "Tab"))
            {
                return new MvcHtmlString(Tab);
            }
            return MvcHtmlString.Empty;
        }

        public static bool IsAuthorized(string Name)
        {
            IGeneralContract generalBL = new GeneralBL();
            System.Web.Routing.RouteData RouteData = HttpContext.Current.Request.RequestContext.RouteData;
            string Area = RouteData.DataTokens["area"]?.ToString();
            string Controller = RouteData.Values["controller"]?.ToString();

            if (generalBL.IsAuthorised(Area, Controller, "", Name, "Tab"))
            {
                return true;
            }
            return false;
        }

        public static bool IsAuthorized(string Area, string Controller, string Name)
        {
            IGeneralContract generalBL = new GeneralBL();

            if (generalBL.IsAuthorised(Area, Controller, "", Name, "Tab"))
            {
                return true;
            }
            return false;
        }
    }
}