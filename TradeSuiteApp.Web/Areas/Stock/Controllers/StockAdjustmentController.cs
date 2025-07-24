using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
   
    public class StockAdjustmentController : Controller
    {
        private IStockAdjustmentContract stockAdjustmentBL;
        private ICategoryContract categoryBL;
        private IWareHouseContract warehouseBL;
        private IGeneralContract generalBL;
        private IDamageTypeContract damageTypeBL;
        private IBatchTypeContract batchTypeBL;
        public StockAdjustmentController()
        {
            categoryBL = new CategoryBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            stockAdjustmentBL = new StockAdjustmentBL();
            damageTypeBL = new DamageTypeBL();
            batchTypeBL = new BatchTypeBL();
        }

        // GET: Stock/StockAdjustment
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };

            return View();
        }

        public ActionResult IndexV3()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };

            return View();
        }

        public ActionResult Create()
        {
            StockAdjustmentModel StockAdjustment = new StockAdjustmentModel();
            StockAdjustment.TransNo = generalBL.GetSerialNo("StockAdjustment", "Code");
            StockAdjustment.Date = General.FormatDate(DateTime.Now);
            StockAdjustment.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            StockAdjustment.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(StockAdjustment.ItemCategoryID), "ID", "Name");
            StockAdjustment.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
           
            StockAdjustment.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            StockAdjustment.Items = new List<StockAdjustmentItem>();
            StockAdjustment.BatchList = new List<StockAdjustmentItem>();
            StockAdjustment.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            StockAdjustment.DamageTypeList = new SelectList(damageTypeBL.GetDamageTypeList(), "ID", "Name");
            return View(StockAdjustment);
        }

        public ActionResult CreateV3()
        {
            StockAdjustmentModel StockAdjustment = new StockAdjustmentModel();
            StockAdjustment.TransNo = generalBL.GetSerialNo("StockAdjustment", "Code");
            StockAdjustment.Date = General.FormatDate(DateTime.Now);
            StockAdjustment.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            StockAdjustment.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(StockAdjustment.ItemCategoryID), "ID", "Name");
            StockAdjustment.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");

            StockAdjustment.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            StockAdjustment.Items = new List<StockAdjustmentItem>();
            StockAdjustment.BatchList = new List<StockAdjustmentItem>();
            StockAdjustment.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            StockAdjustment.DamageTypeList = new SelectList(damageTypeBL.GetDamageTypeList(), "ID", "Name");
            return View(StockAdjustment);
        }
        [HttpPost]
        public JsonResult GetStockAdjustmentItems(int WarehouseID, int ItemCategoryID = 0, int ItemID = 0,int SalesCategoryID=0)
        {
            try
            {
                List<StockAdjustmentItemBO> stockadjustmentlist = stockAdjustmentBL.GetStockAdjustmentItems(WarehouseID, ItemCategoryID, ItemID, SalesCategoryID);
                stockadjustmentlist = stockadjustmentlist.Select(a =>
                {
                    a.ExpiryDateString = a.ExpiryDate == null ? "" : General.FormatDate((DateTime)a.ExpiryDate);
                    return a;
                }).ToList();
                return Json(new { Status = "success", Data = stockadjustmentlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetStockAdjustmentItemsForAlopathy(string FromDate,string ToDate, int ItemID=0)
        {
            try
            {
                int StockAjustmentPremise = Convert.ToInt32(generalBL.GetConfig("DefaultStockAdjustmentStore", GeneralBO.CreatedUserID));
                List<StockAdjustmentItemBO> stockadjustmentlist = stockAdjustmentBL.GetStockAdjustmentItemsForAlopathy(General.ToDateTime( FromDate),General.ToDateTime( ToDate),ItemID, StockAjustmentPremise);
                stockadjustmentlist = stockadjustmentlist.Select(a =>
                {
                    a.ExpiryDateString = a.ExpiryDate == null ? "" : General.FormatDate((DateTime)a.ExpiryDate);
                    return a;
                }).ToList();
                return Json(new { Status = "success", Data = stockadjustmentlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="WarehouseID"></param>
        /// <param name="ItemID"></param>
        /// <param name="BatchTypeID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBatchesByItemIDForStockAdjustment(int WarehouseID,int ItemID = 0,int BatchTypeID=0)
        {
            try
            {
                List<StockAdjustmentItemBO> stockadjustmentlist = stockAdjustmentBL.GetBatchesByItemIDForStockAdjustment(WarehouseID, ItemID, BatchTypeID);
                stockadjustmentlist = stockadjustmentlist.Select(a =>
                {
                    a.ExpiryDateString = a.ExpiryDate == null ? "" : General.FormatDate((DateTime)a.ExpiryDate);
                    return a;
                }).ToList();
                return Json(new { Status = "success", Data = stockadjustmentlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

   
        public ActionResult Save(StockAdjustmentModel stockAdjustmentmodel)
        {
            var result = new List<object>();
            try
            {
                if (stockAdjustmentmodel.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    StockAdjustmentBO Temp = stockAdjustmentBL.GetStockAdjustmentDetail(stockAdjustmentmodel.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                StockAdjustmentBO stockAdjustment = new StockAdjustmentBO()
                {
                    IsDraft = stockAdjustmentmodel.IsDraft,
                    Date = General.ToDateTime(stockAdjustmentmodel.Date),
                    TransNo = stockAdjustmentmodel.TransNo,                    
                    ID= stockAdjustmentmodel.ID
                    
                };

                List<StockAdjustmentItemBO> items = new List<StockAdjustmentItemBO>();
                StockAdjustmentItemBO StockAdjBO;
                foreach (var item in stockAdjustmentmodel.Items)
                {
                    StockAdjBO = new StockAdjustmentItemBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        WarehouseID = item.WarehouseID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        CurrentQty = item.CurrentQty,
                        PhysicalQty = item.PhysicalQty,
                        ID=item.ID
                    };
                    items.Add(StockAdjBO);

                           }
                stockAdjustmentBL.Save(stockAdjustment, items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "StockAdjustment", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveV3(StockAdjustmentModel stockAdjustmentmodel)
        {
            var result = new List<object>();
            try
            {
                if (stockAdjustmentmodel.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    StockAdjustmentBO Temp = stockAdjustmentBL.GetStockAdjustmentDetail(stockAdjustmentmodel.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                StockAdjustmentBO stockAdjustment = new StockAdjustmentBO()
                {
                    IsDraft = stockAdjustmentmodel.IsDraft,
                    Date = General.ToDateTime(stockAdjustmentmodel.Date),
                    TransNo = stockAdjustmentmodel.TransNo,
                    ID = stockAdjustmentmodel.ID

                };

                List<StockAdjustmentItemBO> items = new List<StockAdjustmentItemBO>();
                StockAdjustmentItemBO StockAdjBO;
                foreach (var item in stockAdjustmentmodel.Items)
                {
                    StockAdjBO = new StockAdjustmentItemBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        WarehouseID = item.WarehouseID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        CurrentQty = item.CurrentQty,
                        PhysicalQty = item.PhysicalQty,
                        ID = item.ID
                    };
                    items.Add(StockAdjBO);

                }
                stockAdjustmentBL.SaveV3(stockAdjustment, items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "StockAdjustment", "SaveV3", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int id)
        {
            StockAdjustmentModel stockAdjustment = stockAdjustmentBL.GetStockAdjustmentDetail(id).Select(a => new StockAdjustmentModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date, "view"),
                Warehouse = a.Warehouse,
                IsDraft=a.IsDraft
            }).First();
            stockAdjustment.Items = stockAdjustmentBL.GetStockAdjustmentTrans(id).Select(a => new StockAdjustmentItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.CurrentQty,
                PhysicalQty = a.PhysicalQty,
                WarehouseID = a.WarehouseID,
                ExpiryDate=General.FormatDate(a.ExpiryDate,"view"),
                Remark=a.Remark,
                DamageType=a.DamageType,
                Warehouse = a.WareHouse,
                Rate=a.Rate,
                ExcessQty=a.ExcessQty,
                ExcessValue=a.ExcessValue
               
            }).ToList();
            return View(stockAdjustment);
        }
        public ActionResult DetailsV3(int id)
        {
            StockAdjustmentModel stockAdjustment = stockAdjustmentBL.GetStockAdjustmentDetail(id).Select(a => new StockAdjustmentModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date, "view"),
                Warehouse = a.Warehouse,
                IsDraft = a.IsDraft
            }).First();
            stockAdjustment.Items = stockAdjustmentBL.GetStockAdjustmentTrans(id).Select(a => new StockAdjustmentItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.CurrentQty,
                PhysicalQty = a.PhysicalQty,
                WarehouseID = a.WarehouseID,
                ExpiryDate = General.FormatDate(a.ExpiryDate, "view"),
                Remark = a.Remark,
                DamageType = a.DamageType,
                Warehouse = a.WareHouse,
                Rate = a.Rate,
                ExcessQty = a.ExcessQty,
                ExcessValue = a.ExcessValue

            }).ToList();
            return View(stockAdjustment);
        }

        public ActionResult SaveAsDraft(StockAdjustmentModel stockAdjustmentmodel)
        {
            return Save(stockAdjustmentmodel);
        }

        public ActionResult SaveAsDraftV3(StockAdjustmentModel stockAdjustmentmodel)
        {
            return SaveV3(stockAdjustmentmodel);
        }

        public ActionResult Edit(int id)
        {
            StockAdjustmentModel stockAdjustment = stockAdjustmentBL.GetStockAdjustmentDetail(id).Select(a => new StockAdjustmentModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(DateTime.Now),
                Warehouse = a.Warehouse,
                WarehouseID=a.WarehouseID,
                IsDraft=a.IsDraft
            }).First();
            if(!stockAdjustment.IsDraft)
            {
                return RedirectToAction("Index");
            }
            stockAdjustment.Items = stockAdjustmentBL.GetStockAdjustmentTrans(id).Select(a => new StockAdjustmentItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.CurrentQty,
                PhysicalQty = a.PhysicalQty,
                WarehouseID = a.WarehouseID,
                ExpiryDate=General.FormatDate(a.ExpiryDate),
                DamageTypeID=a.DamageTypeID,
                Remark=a.Remark,
                Rate=a.Rate,
                InventoryUnit=a.InventoryUnit,
                InventoryUnitID=a.InventoryUnitID,
                FullRate=a.FullRate,
                LooseRate=a.LooseRate,
                PrimaryUnit=a.PrimaryUnit,
                PrimaryUnitID=a.PrimaryUnitID,
                Warehouse=a.WareHouse,
                ExcessQty=a.ExcessQty,
                ExcessValue=a.ExcessValue


            }).ToList();
            stockAdjustment.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            stockAdjustment.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            stockAdjustment.DamageTypeList = new SelectList(damageTypeBL.GetDamageTypeList(), "ID", "Name");
            stockAdjustment.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(stockAdjustment.ItemCategoryID), "ID", "Name");          
            stockAdjustment.BatchList = new List<StockAdjustmentItem>();
            stockAdjustment.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            stockAdjustment.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            return View(stockAdjustment);
        }
        public ActionResult EditV3(int id)
        {
            StockAdjustmentModel stockAdjustment = stockAdjustmentBL.GetStockAdjustmentDetail(id).Select(a => new StockAdjustmentModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(DateTime.Now),
                Warehouse = a.Warehouse,
                WarehouseID = a.WarehouseID,
                IsDraft = a.IsDraft
            }).First();
            if (!stockAdjustment.IsDraft)
            {
                return RedirectToAction("Index");
            }
            stockAdjustment.Items = stockAdjustmentBL.GetStockAdjustmentTrans(id).Select(a => new StockAdjustmentItem()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
                Batch = a.Batch,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                BatchType = a.BatchType,
                CurrentQty = a.CurrentQty,
                PhysicalQty = a.PhysicalQty,
                WarehouseID = a.WarehouseID,
                ExpiryDate = General.FormatDate(a.ExpiryDate),
                DamageTypeID = a.DamageTypeID,
                Remark = a.Remark,
                Rate = a.Rate,
                InventoryUnit = a.InventoryUnit,
                InventoryUnitID = a.InventoryUnitID,
                FullRate = a.FullRate,
                LooseRate = a.LooseRate,
                PrimaryUnit = a.PrimaryUnit,
                PrimaryUnitID = a.PrimaryUnitID,
                Warehouse = a.WareHouse,
                ExcessQty = a.ExcessQty,
                ExcessValue = a.ExcessValue


            }).ToList();
            stockAdjustment.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            stockAdjustment.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            stockAdjustment.DamageTypeList = new SelectList(damageTypeBL.GetDamageTypeList(), "ID", "Name");
            stockAdjustment.SalesCategoryList = new SelectList(categoryBL.GetSalesCategory(stockAdjustment.ItemCategoryID), "ID", "Name");
            stockAdjustment.BatchList = new List<StockAdjustmentItem>();
            stockAdjustment.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            stockAdjustment.UnitList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            return View(stockAdjustment);
        }

        public ActionResult Cancel(int id)
        {
            return null;
        }

        public JsonResult ReadExcel(string Path)
        {
            try
            {
                StockAdjustmentModel stockAdjustmentModel = new StockAdjustmentModel();
                List<StockAdjustmentItemBO> DiscoundList = stockAdjustmentBL.ReadExcel(Path);
                StockAdjustmentItem StockAdjustmentItem;
                stockAdjustmentModel.Items = new List<StockAdjustmentItem>();
                foreach (var m in DiscoundList)
                {
                        StockAdjustmentItem = new StockAdjustmentItem()
                        {
                            ItemCode = m.ItemCode,
                            ItemName = m.ItemName,
                            UnitName = m.UnitName,
                            Batch = m.Batch,
                            BatchType = m.BatchType,
                            Warehouse = m.WareHouse,
                            ExpiryDate = General.FormatDate(m.ExpiryDate),
                            CurrentQty = m.CurrentQty,
                            PhysicalQty = m.PhysicalQty,
                            BatchID = m.BatchID,
                            BatchTypeID = m.BatchTypeID,
                            ItemID = m.ItemID,
                            WarehouseID = m.WarehouseID,
                            UnitID = m.UnitID
                        };
                        stockAdjustmentModel.Items.Add(StockAdjustmentItem);
                }
                return Json(new { Status = "success", Data = stockAdjustmentModel.Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockAdjustmentList(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value;
               // string Store = Datatable.Columns[3].Search.Value;
                string ItemName = Datatable.Columns[3].Search.Value;
              //  string SalesCategory = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = stockAdjustmentBL.GetStockAdjustmentList(TransNo, TransDate, null,ItemName, null, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "StockAdjustment", "GetStockAdjustmentList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult StockAdjustmentScheduledItemsPrintPdf(DateTime FromDate, DateTime ToDate)
        {
            return null;
        }

        public ActionResult Revert(StockAdjustmentModel stockAdjustmentmodel)
        {
            var result = new List<object>();
            try
            {              
                List<StockAdjustmentItemBO> items = new List<StockAdjustmentItemBO>();
                StockAdjustmentItemBO StockAdjBO;
                foreach (var item in stockAdjustmentmodel.Items)
                {
                    StockAdjBO = new StockAdjustmentItemBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        WarehouseID = item.WarehouseID,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        ID=item.ID
                    };
                    items.Add(StockAdjBO);

                }
                stockAdjustmentBL.Revert( items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "StockAdjustment", "Revert", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}