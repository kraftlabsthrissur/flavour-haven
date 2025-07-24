using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class RateAdjustmentController : Controller
    {

        private ICategoryContract categoryBL;
        private IRateAdjustmentContract rateAdjustMentBL;
        private IGeneralContract generalBL;

        public RateAdjustmentController()
        {
            categoryBL = new CategoryBL();

            generalBL = new GeneralBL();
            rateAdjustMentBL = new RateAdjustmentBL();
        }

        // GET: Stock/RateAdjustment
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Stock/RateAdjustment/Details/5
        public ActionResult Details(int id)
        {
            RateAdjustmentModel ratedjustment = rateAdjustMentBL.GetRateAdjustmentDetail(id).Select(a => new RateAdjustmentModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date, "view"),
                //      Warehouse = a.Warehouse,
                IsDraft = a.IsDraft
            }).First();
            ratedjustment.Items = rateAdjustMentBL.GetRateAdjustmentTrans(id).Select(item => new RateAdjustmentItemModel()
            {
                ItemID = item.ItemID,
                SystemAvgCost = item.SystemAvgCost,
                SystemStockQty = item.SystemStockQty,
                SystemStockValue = item.SystemStockValue,
                ActualAvgCost = item.ActualAvgCost,
                ActualStockValue = item.ActualStockValue,
                DifferenceInAvgCost = item.DifferenceInAvgCost,
                DifferenceInStockValue = item.DifferenceInStockValue,
                ItemName = item.ItemName,
                Category = item.Category,
                EffectDate = General.FormatDate(item.EffectDate, "view"),
                Remark = item.Remark

            }).ToList();
            ratedjustment.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");

            return View(ratedjustment);
        }

        // GET: Stock/RateAdjustment/Create
        public ActionResult Create()
        {
            RateAdjustmentModel rateAdjustment = new RateAdjustmentModel();
            rateAdjustment.TransNo = generalBL.GetSerialNo("RateAdjustment", "Code");
            rateAdjustment.Date = General.FormatDate(DateTime.Now);
            rateAdjustment.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");

            rateAdjustment.Items = new List<RateAdjustmentItemModel>();
            return View(rateAdjustment);

        }
        [HttpPost]
        public JsonResult GetRateAdjustmentItems(int ItemCategoryID = 0, int ItemID = 0)
        {
            try
            {
                List<RateAdjustmentItemBO> rateadjustmentlist = rateAdjustMentBL.GetRateAdjustmentItems(ItemCategoryID, ItemID);

                return Json(new { Status = "success", Data = rateadjustmentlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Save(RateAdjustmentModel rateAdjustmentmodel)
        {
            var result = new List<object>();
            try
            {
                if (rateAdjustmentmodel.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    RateAdjustmentBO Temp = rateAdjustMentBL.GetRateAdjustmentDetail(rateAdjustmentmodel.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                RateAdjustmentBO ratedjustment = new RateAdjustmentBO()
                {
                    IsDraft = rateAdjustmentmodel.IsDraft,
                    Date = General.ToDateTime(rateAdjustmentmodel.Date),
                    TransNo = rateAdjustmentmodel.TransNo,
                    ID = rateAdjustmentmodel.ID

                };

                List<RateAdjustmentItemBO> items = new List<RateAdjustmentItemBO>();
                RateAdjustmentItemBO RateAdjBO;
                foreach (var item in rateAdjustmentmodel.Items)
                {
                    RateAdjBO = new RateAdjustmentItemBO()
                    {
                        ItemID = item.ItemID,
                        SystemAvgCost = item.SystemAvgCost,
                        SystemStockQty = item.SystemStockQty,
                        SystemStockValue = item.SystemStockValue,
                        ActualAvgCost = item.ActualAvgCost,
                        ActualStockValue = item.ActualStockValue,
                        DifferenceInAvgCost = item.DifferenceInAvgCost,
                        DifferenceInStockValue = item.DifferenceInStockValue,
                        EffectDate = DateTime.Now,
                        Remark = item.Remark
                    };
                    items.Add(RateAdjBO);

                }
                rateAdjustMentBL.Save(ratedjustment, items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "RateAdjustment", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(RateAdjustmentModel rateAdjustmentmodel)
        {
            return Save(rateAdjustmentmodel);
        }

        // POST: Stock/RateAdjustment/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/RateAdjustment/Edit/5
        public ActionResult Edit(int id)
        {
            RateAdjustmentModel ratedjustment = rateAdjustMentBL.GetRateAdjustmentDetail(id).Select(a => new RateAdjustmentModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date),
                IsDraft = a.IsDraft
            }).First();
            if (!ratedjustment.IsDraft)
            {
                return RedirectToAction("Index");
            }
            ratedjustment.Items = rateAdjustMentBL.GetRateAdjustmentTrans(id).Select(item => new RateAdjustmentItemModel()
            {
                ItemID = item.ItemID,
                SystemAvgCost = item.SystemAvgCost,
                SystemStockQty = item.SystemStockQty,
                SystemStockValue = item.SystemStockValue,
                ActualAvgCost = item.ActualAvgCost,
                ActualStockValue = item.ActualStockValue,
                DifferenceInAvgCost = item.DifferenceInAvgCost,
                DifferenceInStockValue = item.DifferenceInStockValue,
                ItemName = item.ItemName,
                Category = item.Category,
                EffectDate = General.FormatDate(item.EffectDate),
                Remark = item.Remark


            }).ToList();
            ratedjustment.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");

            return View(ratedjustment);
        }

        public JsonResult GetRateAdjustmentListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = rateAdjustMentBL.GetRateAdjustmentListForDataTable(Type, TransNo, TransDate, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Cancel(int id)
        {
            return null;
        }
    }
}
