using BusinessLayer;
using PresentationContractLayer;
using System;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.ActionFilters
{
    public class ApprovalActionFilter : ActionFilterAttribute
    {
        public void OnException(ExceptionContext filterContext)
        {

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string Area = filterContext.RouteData.DataTokens["area"]?.ToString() ?? "";
            string Controller = filterContext.RouteData.Values["controller"].ToString();
            string Action = filterContext.RouteData.Values["action"].ToString();
            
            IApprovalContract approvalBL = new ApprovalBL();
            bool HasApproval;
            string RequestMethod;
            string ScriptString;
            RequestMethod = filterContext.HttpContext.Request.HttpMethod;

            filterContext.Controller.ViewBag.StartupScript = "";
            try {
                if (RequestMethod == "GET" && filterContext.Result is ViewResult && Area !="")
                {
                    HasApproval = approvalBL.HasApprovalProcess(Area, Controller, Action);
                    if (HasApproval)
                    {
                        string ID = filterContext.RouteData.Values["id"].ToString() ?? "";
                        ScriptString = "Approvals.init('" + Area + "', '" + Controller + "','" + Action + "'," + ID + ");";
                        filterContext.Controller.ViewBag.StartupScript = ScriptString;
                    }
                }
                
            } catch (Exception e) {

            }
            if (RequestMethod == "GET" && filterContext.Result is ViewResult)
            {
                ILocationContract locationBL = new LocationBL();
                filterContext.Controller.ViewBag.VBLocations = new SelectList(locationBL.GetLocationListByUser(Convert.ToInt16(filterContext.HttpContext.Session["UserID"])), "ID", "Name");
                filterContext.Controller.ViewBag.VBCurrentLocationID = filterContext.HttpContext.Session["CurrentLocationID"];
            }
            // --- for ajax pages --- //
            //if (RequestMethod == "GET" && filterContext.Result is ViewResult && filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    var view = filterContext.Result as ViewResult;
            //    view.MasterName = "~/Views/Shared/_LayoutEmpty.cshtml";
            //    base.OnResultExecuting(filterContext);
            //}
            // --- for ajax pages --- //
        }

        private void Log(string Message)
        {
            Console.WriteLine(Message, "Action Filter Log");
        }
    }
}