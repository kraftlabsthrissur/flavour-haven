using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class SalesBudgetController : Controller
    {
        private IAttendanceContract atendanceBL;
        private ILocationContract locationBL;
        private IBatchTypeContract batchTypeBL;
        private ISalesBudgetContract salesBudgetBL;

        public SalesBudgetController()
        {
            atendanceBL = new AttendanceBL();
            locationBL = new LocationBL();
            batchTypeBL = new BatchTypeBL();
            salesBudgetBL = new SalesBudgetBL();
        }

        // GET: Masters/SalesBudget
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            SalesBudgetModel Model = new SalesBudgetModel();
            Model.Date = General.FormatDate(DateTime.Now);
            Model.MonthList = new SelectList(atendanceBL.GetMonthList(), "ID", "Name");
            Model.BranchList = new SelectList(locationBL.GetBranchList(), "ID", "Name");
            Model.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            return View(Model);
        }

        public JsonResult ReadExcel(string Path)
        {
            try
            {
                List<SalesBudgetItemBO> SalesBudget = salesBudgetBL.ReadExcel(Path);
                return Json(new { Status = "success", Data = SalesBudget }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(SalesBudgetModel model)
        {
            try
            {
                SalesBudgetBO salesBudgetBO = new SalesBudgetBO()
                {
                    ID = model.ID
                };
                salesBudgetBL.Save(salesBudgetBO);
                return Json(new { Status = "success", Message = "Saved " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalesBudgetList(DatatableModel Datatable)
        {
            try
            {
                string ItemCodeHint = Datatable.Columns[1].Search.Value;
                string ItemNameHint = Datatable.Columns[2].Search.Value;
                string MonthHint = Datatable.Columns[3].Search.Value;
                string SalesCategoryHint = Datatable.Columns[4].Search.Value;
                string BatchTypeHint = Datatable.Columns[5].Search.Value;
                string BranchHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = salesBudgetBL.GetSalesBudgetList(ItemCodeHint, ItemNameHint, MonthHint,SalesCategoryHint, BatchTypeHint, BranchHint, SortField, SortOrder, Offset, Limit);
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