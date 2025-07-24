using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class StockValueController : Controller
    {
        IStockValueContract stockValueBL;

        public StockValueController()
        {
            stockValueBL = new StockValueBL();
        }

        // GET: Stock/StockValue
        public ActionResult Index()
        {

            string ProcessedDate = General.FormatDate(DateTime.Now, "view");

            ViewBag.ProcessedDate = ProcessedDate;
            return View();
        }

        public JsonResult GetStockValueList(DatatableModel Datatable)
        {
            try
            {
                string NameHint = Datatable.Columns[1].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = stockValueBL.GetStockValueList(NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Execute()
        {
            List<StockValueViewModel> stockValue = new List<StockValueViewModel>();

            stockValue = stockValueBL.Execute().Select(m => new StockValueViewModel()
            {

            }).ToList();
            Index();
            return Json(stockValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Process(int id)
        {
            return null;
        }
    }
}
