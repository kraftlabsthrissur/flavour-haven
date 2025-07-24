using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using PresentationContractLayer;
using BusinessObject;
namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class StockAdjustmentScheduleController : Controller
    {
        private IStockAdjustmentScheduleContract stockAdjustmentBL;
        private IGeneralContract generalBL;
        private IWareHouseContract warehouseBL;
        // GET: Masters/StockAdjustmentSchedule
        public StockAdjustmentScheduleController()
        {
            stockAdjustmentBL = new StockAdjustmentScheduleBL();
            generalBL = new GeneralBL();
            warehouseBL = new WarehouseBL();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            StockAdjustmentScheduleModel stockAdjustment = new StockAdjustmentScheduleModel();
            stockAdjustment.ItemCount = (int)stockAdjustmentBL.GetItemCount();
            stockAdjustment.Warehouse = new SelectList(warehouseBL.GetWareHouseList(), "ID", "Name");
            stockAdjustment.ExcludedDateList = new List<ExcludedDateModel>();
            return View(stockAdjustment);
        }
        public ActionResult Save(StockAdjustmentScheduleModel stockAdjustmentmodel)
        {
            var result = new List<object>();
            try
            {
                string date = DateTime.Now.ToString("dd-MM-yyyy");
                StockAdjustmentScheduleBO stockAdjustment = new StockAdjustmentScheduleBO()
                {

                    ID = stockAdjustmentmodel.ID,
                    ItemCount = stockAdjustmentmodel.ItemCount,
                    FrequencyOfItem = stockAdjustmentmodel.FrequencyOfItem,
                    TimeLimit = stockAdjustmentmodel.TimeLimit,
                    EveningStartTime = General.ToDateTime(string.Concat(date, ' ', stockAdjustmentmodel.EveningStartTimeStr), "datetime"),
                    EveningEndTime = General.ToDateTime(string.Concat(date, ' ', stockAdjustmentmodel.EveningEndTimeStr), "datetime"),
                    MorningStartTime = General.ToDateTime(string.Concat(date, ' ', stockAdjustmentmodel.MorningStartTimeStr), "datetime"),
                    MorningEndTime = General.ToDateTime(string.Concat(date, ' ', stockAdjustmentmodel.MorningEndTimeStr), "datetime"),
                };

                List<ExcludedDateBO> Dates = new List<ExcludedDateBO>();
                ExcludedDateBO StockAdjBO;
                if (stockAdjustmentmodel.ExcludedDateList != null)
                {
                    foreach (var dates in stockAdjustmentmodel.ExcludedDateList)
                    {
                        StockAdjBO = new ExcludedDateBO()
                        {
                            ID = dates.ID,
                            Date = General.ToDateTime(dates.Date)
                        };
                        Dates.Add(StockAdjBO);

                    }
                }
                int id = stockAdjustmentBL.save(stockAdjustment, Dates);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "StockAdjustmentSchedule", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult StockAdjustmentScheduleList(DatatableModel Datatable)
        {
            try
            {
                //string TransNo = Datatable.Columns[1].Search.Value;
                //string TransDate = Datatable.Columns[2].Search.Value;
                //string Store = Datatable.Columns[3].Search.Value;
                //string ItemName = Datatable.Columns[4].Search.Value;
                //string SalesCategory = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = stockAdjustmentBL.GetStockAdjustmentList( SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "StockAdjustmentSchedule", "StockAdjustmentScheduleList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Details(int id)
        {
            StockAdjustmentScheduleModel stockAdjustment = stockAdjustmentBL.GetStockAdjustmentScheduleDetail(id).Select(a => new StockAdjustmentScheduleModel()
            {
                ID = a.ID,
              FrequencyOfItem=a.FrequencyOfItem,
              ItemCount=a.ItemCount,
              TimeLimit=a.TimeLimit,
              MorningEndTime=a.MorningEndTime,
              MorningStartTime=a.MorningStartTime,
              EveningEndTime= a.EveningEndTime,
              EveningStartTime= a.EveningStartTime
            }).First();
            stockAdjustment.ExcludedDateList = stockAdjustmentBL.GetExcludedDate(id).Select(a => new ExcludedDateModel()
            {
               ID=a.ID,
               Date=General.FormatDate(a.Date,"view")

            }).ToList();
            return View(stockAdjustment);
        }
        public ActionResult Edit(int id)
        {
            StockAdjustmentScheduleModel stockAdjustment = stockAdjustmentBL.GetStockAdjustmentScheduleDetail(id).Select(a => new StockAdjustmentScheduleModel()
            {
                ID = a.ID,
                FrequencyOfItem = a.FrequencyOfItem,
                ItemCount = a.ItemCount,
                TimeLimit = a.TimeLimit,
                MorningEndTime = a.MorningEndTime,
                MorningStartTime = a.MorningStartTime,
                EveningEndTime = a.EveningEndTime,
                EveningStartTime = a.EveningStartTime
            }).First();
            stockAdjustment.ExcludedDateList = stockAdjustmentBL.GetExcludedDate(id).Select(a => new ExcludedDateModel()
            {
                ID = a.ID,
                Date = General.FormatDate(a.Date)

            }).ToList();
            return View(stockAdjustment);
        }

    }
}