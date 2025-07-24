using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.User.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Controllers
{

    [Authorize]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {
        private ICategoryContract categoryBL;
        private IDashBoardContract dashboardBL;
        private IGeneralContract generalBL;

        public HomeController()
        {
            categoryBL = new CategoryBL();
            dashboardBL = new DashBoardBL();
            generalBL = new GeneralBL();
        }

        public ActionResult Index()
        {

            //var obj = MvcApplication.Sessions;
            DashboardModel model = new DashboardModel();
            model.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(222), "ID", "Name");

            if (generalBL.IsRoleExistForUser("ISKDashBoard") && generalBL.IsRoleExistForUser("OSKDashBoard"))
            {
                model.BatchTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "ALL", Value ="ALL", },
                                                 new SelectListItem { Text = "ISK", Value = "ISK"},
                                                 new SelectListItem { Text = "OSK", Value = "OSK"},
                                                 }, "Value", "Text");
            }
            else if (generalBL.IsRoleExistForUser("ISKDashBoard"))
            {
                model.BatchTypeList = new SelectList(new List<SelectListItem>{

                                                 new SelectListItem { Text = "ISK", Value = "ISK"},

                                                 }, "Value", "Text");
            }
            else if (generalBL.IsRoleExistForUser("OSKDashBoard"))
            {
                model.BatchTypeList = new SelectList(new List<SelectListItem>{

                                                 new SelectListItem { Text = "OSK", Value = "OSK"},
                                                 }, "Value", "Text");
            }
            else 
            {
                model.BatchTypeList = new SelectList(new List<SelectListItem>{

                                                 new SelectListItem { Text = "NONE", Value = "NONE"},
                                                 }, "Value", "Text");
            }


            model.LocationList = dashboardBL.GetSalesByLocation(DateTime.Today, 0).Select(a => new SalesByLocationModel()
            {
                LocationCode = a.LocationCode,
                LocationType = a.LocationType,
                LocationName = a.LocationName,
                Amount = a.Amount,
                PreviousMonthAmount = a.PreviousMonthAmount,
                Budget = a.Budget
            }).ToList();



            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult PrintList()
        {
            ViewBag.Date = General.FormatDate(DateTime.Now);
            return View();
        }

        public JsonResult GetPrintList(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string Form = Datatable.Columns[2].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string DateFromString = Datatable.GetValueFromKey("DateFrom", Datatable.Params);
                DateTime DateFrom = General.ToDateTime(DateFromString);

                DatatableResultBO resultBO = generalBL.GetPrintList(DateFrom,TransNo, Form, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
     
    }
}