using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TradeSuiteApp.Web.RolePrivileges
{
    public class RolePermissionAttribute : AuthorizeAttribute
    {
        private IGeneralContract generalBL;
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            generalBL = new GeneralBL();

            

            //HttpContext.Current.Session["UserID"] = 10;
            //HttpContext.Current.Session["Email"] = "ajith@gmail.com";
            //HttpContext.Current.Session["UserName"] = "ajith";
            //HttpContext.Current.Session["EmployeeName"] = "ajith";
            //HttpContext.Current.Session["CurrentLocationID"] = 1;
            //HttpContext.Current.Session["Designation"] = "developer";
            //HttpContext.Current.Session["FinYearTitle"] = "Fin Year 2018-2019";
            //HttpContext.Current.Session["FinYear"] = 2018;
            //HttpContext.Current.Session["ApplicationID"] = 1;

            // return;

            string Area = filterContext.RouteData.DataTokens["area"]?.ToString();
            string Controller = filterContext.RouteData.Values["controller"].ToString();
            string Action = filterContext.RouteData.Values["action"].ToString();



            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
              || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller  
                return;
            }

           
            // Check for authorization  
            if (HttpContext.Current.Session["EmployeeName"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult()
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            Status = "failure",
                            StatusCode = 302,
                            Text = "Session Timeout"
                        }
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary {
                                                { "action", "Login" },
                                                { "controller", "Account" },
                                                { "area","" },
                                                { "returnUrl" , filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped) }
                                                   });
                }

            }
            else if (Area != null && !generalBL.IsAuthorised(Area, Controller, Action, "", "Action"))
            {
                filterContext.Result = new RedirectToRouteResult(
                                                new RouteValueDictionary {
                                                { "action", "UnAuthorized" },
                                                { "controller", "Account" },
                                                { "area","" }
                                                });
            }

            base.OnAuthorization(filterContext);


        }

    }
}