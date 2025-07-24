using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Stock.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Stock.Controllers
{
    public class OpeningStockController : Controller
    {
        private IBatchContract batchBL;
        private IBatchTypeContract batchTypeBL;
        private IGeneralContract generalBL;
        private IWareHouseContract warehouseBL;
        private ICategoryContract categoryBL;
        private IOpeningStockContract openingStockBL;
        private IUnitContract unitBL;

        public OpeningStockController()
        {
            batchBL = new BatchBL();
            batchTypeBL = new BatchTypeBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            openingStockBL = new OpeningStockBL();
            unitBL = new UnitBL();
        }

        // GET: Stock/OpenninStock
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult create()
        {
            OpeningStockModel OpenningStock = new OpeningStockModel();
            OpenningStock.Date = General.FormatDate(DateTime.Now);
            OpenningStock.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            OpenningStock.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
            OpenningStock.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            OpenningStock.TransNo = generalBL.GetSerialNo("openingstock", "Code");
            OpenningStock.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultFGStore"));
            OpenningStock.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            return View(OpenningStock);
        }

        // POST: Stock/OpeningStock/Save
        [HttpPost]
        public ActionResult Save(OpeningStockModel OpeningStock)
        {
            var result = new List<object>();
            try
            {
                if (OpeningStock.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    OpeningStockBO Temp = openingStockBL.GetOpeningStock(OpeningStock.ID);
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                OpeningStockBO OpeningStockBO = new OpeningStockBO()
                {
                    ID = OpeningStock.ID,
                    IsDraft = OpeningStock.IsDraft,
                    TransNo = OpeningStock.TransNo,
                    Date = General.ToDateTime(OpeningStock.Date)
                };
                List<OpeningStockItemBO> OpeningStockItems = new List<OpeningStockItemBO>();
                OpeningStockItemBO OpeningStockItem;
                foreach (var item in OpeningStock.Items)
                {
                    OpeningStockItem = new OpeningStockItemBO()
                    {
                        WarehouseID = item.WarehouseID,
                        ItemID = item.ItemID,
                        BatchTypeID = item.BatchTypeID,
                        BatchID = item.BatchID,
                        Batch = item.Batch,
                        UnitID = item.UnitID,
                        Qty = item.Qty,
                        Value = item.Value,
                        ExpDate= General.ToDateTime(item.ExpDate)
                    };

                    OpeningStockItems.Add(OpeningStockItem);
                }
                openingStockBL.Save(OpeningStockBO, OpeningStockItems);

                return
                     Json(new
                     {
                         Status = "success",
                         data = "",
                         message = ""
                     }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Stock", "OpeningStock", "Save", Convert.ToInt16(OpeningStock.ID), e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAsDraft(OpeningStockModel OpeningStock)
        {
            return Save(OpeningStock);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    List<OpeningStockItemBO> OpeningStockItemsBO;
                    OpeningStockModel openingStockModel;
                    OpeningStockItemModel OpeningStockItem;
                    OpeningStockBO OpeningStockBO = openingStockBL.GetOpeningStock((int)id);
                    openingStockModel = new OpeningStockModel()
                    {
                        TransNo = OpeningStockBO.TransNo,
                        Date = General.FormatDate(OpeningStockBO.Date),
                        Status = OpeningStockBO.IsDraft ? "draft" : "",
                        ID = OpeningStockBO.ID
                    };

                    OpeningStockItemsBO = openingStockBL.GetOpeningStockItems((int)id);
                    openingStockModel.Items = new List<OpeningStockItemModel>();
                    foreach (var m in OpeningStockItemsBO)
                    {
                        OpeningStockItem = new OpeningStockItemModel()
                        {
                            Batch = m.Batch,
                            BatchType = m.BatchType,
                            ItemName = m.ItemName,
                            Qty = m.Qty,
                            Unit = m.Unit,
                            Value = m.Value,
                            ExpDate=General.FormatDate(m.ExpDate)
                        };
                        openingStockModel.Items.Add(OpeningStockItem);
                    }


                    return View(openingStockModel);
                }

                catch (Exception e)
                {
                    return View();
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    List<OpeningStockItemBO> OpeningStockItemsBO;
                    OpeningStockModel openingStock;
                    OpeningStockItemModel OpeningStockItem;
                    OpeningStockBO OpeningStockBO = openingStockBL.GetOpeningStock((int)id);
                    if(!OpeningStockBO.IsDraft)
                    {
                        return RedirectToAction("Index");
                    }
                    openingStock = new OpeningStockModel()
                    {
                        TransNo = OpeningStockBO.TransNo,
                        Date = General.FormatDate(OpeningStockBO.Date),
                        Status = OpeningStockBO.IsDraft ? "draft" : "",
                        ID = OpeningStockBO.ID,
                        Store = OpeningStockBO.Store,
                        WarehouseID = OpeningStockBO.WarehouseID,
                        IsDraft=OpeningStockBO.IsDraft
                    };

                    OpeningStockItemsBO = openingStockBL.GetOpeningStockItems((int)id);

                    openingStock.Items = new List<OpeningStockItemModel>();
                    foreach (var m in OpeningStockItemsBO)
                    {
                        OpeningStockItem = new OpeningStockItemModel()
                        {
                            Batch = m.Batch,
                            BatchType = m.BatchType,
                            ItemName = m.ItemName,
                            Qty = m.Qty,
                            BatchTypeID = m.BatchTypeID,
                            ItemID = m.ItemID,
                            BatchID = m.BatchID,
                            Unit = m.Unit,
                            WarehouseID = OpeningStockBO.WarehouseID,
                            UnitID = OpeningStockBO.ID,
                            Value = m.Value,
                            ExpDate = General.FormatDate(m.ExpDate)
                        };
                        openingStock.Items.Add(OpeningStockItem);
                    }
                    openingStock.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                    openingStock.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
                    openingStock.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");
                    return View(openingStock);
                }

                catch (Exception e)
                {
                    return View();
                }
            }
        }

        public JsonResult GetOpeningStockListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string Date = Datatable.Columns[2].Search.Value;
                string Store = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = openingStockBL.GetOpeningStockListForDataTable(Type, TransNo, Date, Store, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMRPForOpeningStock(int ItemID, int BatchTypeID, string Batch)
        {
            List<OpeningStockMRPModel> itemList = new List<OpeningStockMRPModel>();
            itemList = openingStockBL.GetMRPForOpeningStock(ItemID, BatchTypeID, Batch).Select(a => new OpeningStockMRPModel()
            {
                MRP = a.MRP
            }).ToList();
            return Json(new { Status = "success", Data = itemList }, JsonRequestBehavior.AllowGet);
        }

    }
}