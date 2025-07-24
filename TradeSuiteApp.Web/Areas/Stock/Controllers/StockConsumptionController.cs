using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class StockConsumptionController : Controller
    {
        private IStockConsumptionContract stockconsumptionBL;
        private IGeneralContract generalBL;
        private IWareHouseContract warehouseBL;
        private ICategoryContract categoryBL;
        private IDamageTypeContract damageTypeBL;

        public StockConsumptionController()
        {
            categoryBL = new CategoryBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            damageTypeBL = new DamageTypeBL();
            stockconsumptionBL = new StockConsumptionBL();
        }
        // GET: Stock/StockConsumption
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }
        public ActionResult Create()
        {
            StockConsumptionModel StockConsumption = new StockConsumptionModel();
            StockConsumption.TransNo= generalBL.GetSerialNo("StockConsumption","Code");
            StockConsumption.Date= General.FormatDate(DateTime.Now);
            StockConsumption.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            StockConsumption.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            StockConsumption.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(StockConsumption.ItemCategoryID), "ID", "Name");
            StockConsumption.Items = new List<StockConsumptionItem>();
            StockConsumption.BatchList = new List<StockConsumptionItem>();
          
            return View(StockConsumption);
        }

        [HttpPost]
        public JsonResult GetStockConsumptionItems(int WarehouseID, int ItemCategoryID = 0, int ItemID = 0, int SalesCategoryID = 0)
        {
            try
            {
                List<StockConsumptionItemBO> stockconsumptionlist = stockconsumptionBL.GetStockConsumptionItems(WarehouseID, ItemCategoryID, ItemID, SalesCategoryID);
                stockconsumptionlist = stockconsumptionlist.Select(a =>
                {
                    a.ExpiryDateString = a.ExpiryDate == null ? "" : General.FormatDate((DateTime)a.ExpiryDate);
                    return a;
                }).ToList();
                return Json(new { Status = "success", Data = stockconsumptionlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Save(StockConsumptionModel stockConsumptionmodel)
        {
            var result = new List<object>();
            try
            {
                if (stockConsumptionmodel.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    StockConsumptionBO Temp = stockconsumptionBL.GetStockConsumptionDetail(stockConsumptionmodel.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                StockConsumptionBO stockconsumption = new StockConsumptionBO()
                {
                    IsDraft = stockConsumptionmodel.IsDraft,
                    Date = General.ToDateTime(stockConsumptionmodel.Date),
                    TransNo = stockConsumptionmodel.TransNo,
                    WarehouseID = stockConsumptionmodel.WarehouseID,
                    ID = stockConsumptionmodel.ID,
                };

                List<StockConsumptionItemBO> items = new List<StockConsumptionItemBO>();
                StockConsumptionItemBO StockConBO;
                foreach (var item in stockConsumptionmodel.Items)
                {
                    StockConBO = new StockConsumptionItemBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        WarehouseID = item.WarehouseID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        AvailableQty = item.CurrentQty,
                        PhysicalQty = item.PhysicalQty,
                        ExpiryDate = General.ToDateTime(item.ExpiryDate),
                        DamageTypeID = item.DamageTypeID,
                        Remark = item.Remark,
                        Rate = item.Rate,
                        ExcessQty = item.ExcessQty,
                        ExcessValue = item.ExcessValue
                    };
                    items.Add(StockConBO);

                }
                stockconsumptionBL.Save(stockconsumption, items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "StockConsumption", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(StockConsumptionModel stockConsumptionmodel)
        {
            return Save(stockConsumptionmodel);
        }

        public JsonResult GetStockConsumptionList(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value;
                string Store = Datatable.Columns[3].Search.Value;
                string ItemName = Datatable.Columns[4].Search.Value;
                string SalesCategory = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = stockconsumptionBL.GetStockConsumptionList(Type,TransNo, TransDate, Store, ItemName, SalesCategory, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "StockConsumption", "GetStockConsumptionList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int id)
        {
            StockConsumptionModel stockConsumption = stockconsumptionBL.GetStockConsumptionDetail(id).Select(a => new StockConsumptionModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date, "view"),
                Warehouse = a.Warehouse,
                IsDraft = a.IsDraft
            }).First();
            stockConsumption.Items = stockconsumptionBL.GetStockConsumptionTrans(id).Select(a => new StockConsumptionItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.AvailableQty,
                PhysicalQty = a.PhysicalQty,
                WarehouseID = a.WarehouseID,
                ExpiryDate = General.FormatDate(a.ExpiryDate, "view"),
                Warehouse = a.WareHouse,
                Rate = a.Rate
            }).ToList();
            return View(stockConsumption);
        }

        public ActionResult Edit(int id)
        {
            StockConsumptionModel stockConsumption = stockconsumptionBL.GetStockConsumptionDetail(id).Select(a => new StockConsumptionModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(DateTime.Now),
                Warehouse = a.Warehouse,
                WarehouseID = a.WarehouseID,
                IsDraft = a.IsDraft
            }).First();
            if (!stockConsumption.IsDraft)
            {
                return RedirectToAction("Index");
            }
            stockConsumption.Items = stockconsumptionBL.GetStockConsumptionTrans(id).Select(a => new StockConsumptionItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.AvailableQty,
                PhysicalQty = a.PhysicalQty,
                WarehouseID = a.WarehouseID,
                ExpiryDate = General.FormatDate(a.ExpiryDate),
                Rate = a.Rate,
                InventoryUnit = a.InventoryUnit,
                InventoryUnitID = a.InventoryUnitID,
                PrimaryUnit = a.PrimaryUnit,
                PrimaryUnitID = a.PrimaryUnitID,
                Warehouse = a.WareHouse
            }).ToList();
            stockConsumption.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            stockConsumption.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            stockConsumption.DamageTypeList = new SelectList(damageTypeBL.GetDamageTypeList(), "ID", "Name");
            stockConsumption.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(stockConsumption.ItemCategoryID), "ID", "Name");
            stockConsumption.BatchList = new List<StockConsumptionItem>();
            return View(stockConsumption);
        }

    }
}