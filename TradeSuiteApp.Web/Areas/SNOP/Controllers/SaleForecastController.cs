using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.SNOP.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.SNOP.Controllers
{
    public class SaleForecastController : Controller
    {
        private IGeneralContract generalBL;
        private ISalesForecastContract salesForecastBL;
        private ILocationContract locationBL;

        public SaleForecastController()
        {
            generalBL = new GeneralBL();
            salesForecastBL = new SalesForecastBL();
            locationBL = new LocationBL();
        }
        // GET: SNOP/SaleForecast
        public ActionResult Index()
        {
            SalesForecastModel salesForecastModel = new SalesForecastModel();
            int Month = Convert.ToInt16(DateTime.Now.AddMonths(2).ToString("MM"));
            salesForecastModel.ID = salesForecastBL.IsSalesForecastExist(Month);
            return View(salesForecastModel);
        }

        // GET: SNOP/SaleForecast/Details/5
        public ActionResult Details(int id)
        {
            SalesForecastModel salesForecastModel;
            SalesForecastBO salesforecastbo = salesForecastBL.GetSalesForecast(id);
            salesForecastModel = new SalesForecastModel()
            {
                TransNo = salesforecastbo.TransNo,
                TransDate = General.FormatDate(salesforecastbo.TransDate),
                Month = General.FormatDate(salesforecastbo.TransDate),
                ID = salesforecastbo.ID,
                IsFinalize = salesforecastbo.IsFinalize
            };
            return View(salesForecastModel);
        }

        // GET: SNOP/SaleForecast/Create
        public ActionResult Create()
        {
            SalesForecastModel salesForecastModel = new SalesForecastModel();
            salesForecastModel.TransNo = generalBL.GetSerialNo("SalesForecast", "Code");
            salesForecastModel.TransDate = General.FormatDate(DateTime.Now);
            salesForecastModel.Month = DateTime.Now.AddMonths(2).ToString("MMM");
            return View(salesForecastModel);
        }

        // POST: SNOP/SaleForecast/Save
        [HttpPost]
        public ActionResult Save(int ID)
        {
            try
            {
                salesForecastBL.Save(ID);
                return Json(new { Status = "success", data = "", message = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "", data = "", message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        // GET: SNOP/SaleForecast/Edit/5
        public ActionResult Edit(int id)
        {
            SalesForecastModel salesForecastModel;
            SalesForecastBO salesforecastbo = salesForecastBL.GetSalesForecast(id);
            salesForecastModel = new SalesForecastModel()
            {
                TransNo = salesforecastbo.TransNo,
                TransDate = General.FormatDate(salesforecastbo.TransDate),
                Month = General.FormatDate(salesforecastbo.TransDate),
                ID = salesforecastbo.ID,
                IsFinalize = salesforecastbo.IsFinalize
            };
            return View(salesForecastModel);
        }

        [HttpPost]
        public JsonResult GetSalesForeCastItems(DatatableModel Datatable)
        {
            try
            {
                string LocationHint = Datatable.Columns[1].Search.Value;
                string ItemNameHint = Datatable.Columns[2].Search.Value;
                string CodeHint = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ID = Convert.ToInt32(Datatable.GetValueFromKey("ID", Datatable.Params));
                DatatableResultBO resultBO = salesForecastBL.GetSalesForecastItem(ID, ItemNameHint, CodeHint, "", LocationHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSalesForeCasts(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string MonthHint = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = salesForecastBL.GetSalesForecasts(TransNoHint, MonthHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ReadExcel(string Path, int ID)
        {
            try
            {
                List<SalesForecastItemBO> SalesForecastItems = salesForecastBL.ReadExcel(Path);
                salesForecastBL.UploadSalesForecastItems(ID, SalesForecastItems);
                return Json(new { Status = "success", Data = SalesForecastItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Process(SalesForecastModel Items)
        {
            try
            {
                SalesForecastBO SalesforecastItems = new SalesForecastBO()
                {
                    ID = Items.ID,
                    TransDate = General.ToDateTime(Items.TransDate),
                    TransNo = Items.TransNo
                };
                Items.ID = salesForecastBL.Process(SalesforecastItems);
                return Json(new { Status = "success", Data = Items.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
