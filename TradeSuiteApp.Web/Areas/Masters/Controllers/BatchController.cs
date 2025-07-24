using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class BatchController : Controller
    {
        private IBatchContract batchBL;
        private ItemContract itemBL;
        private IUnitContract unitBL;
        private IDiscountCategoryContract discountCategoryBL;
        private IGSTContract GstBL;

        public BatchController()
        {
            batchBL = new BatchBL();
            itemBL = new ItemBL();
            unitBL = new UnitBL();
            discountCategoryBL = new DiscountCategoryBL();
            GstBL = new GSTBL();
        }

        public JsonResult GetBatch(int ItemID, int StoreID = 0)
        {
            List<BatchBO> Batches = batchBL.GetBatchList(ItemID, StoreID).ToList();
            return Json(new { Status = "success", data = Batches }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchBatchTypeWise(int ItemID, int StoreID = 0, int BatchTypeID = 0)
        {
            List<BatchBO> Batches = batchBL.GetBatchBatchTypeWise(ItemID, StoreID, BatchTypeID).ToList();
            return Json(new { Status = "success", data = Batches }, JsonRequestBehavior.AllowGet);
        }
        // GET: Masters/Batch
        public ActionResult Index()
        {
            List<BatchModel> batchList = new List<BatchModel>();
            return View(batchList);
        }

        // GET: Masters/Batch/Details/5
        public ActionResult Details(int id)
        {
            var obj = batchBL.GetBatchDetails(id);
            BatchModel batchModel = new BatchModel();
            batchModel.ID = obj.ID;
            batchModel.BatchNo = obj.BatchNo;
            batchModel.CustomBatchNo = obj.CustomBatchNo;
            batchModel.ItemName = obj.Name;
            batchModel.Itemtype = obj.ItemType;
            batchModel.BatchTypeName = obj.BatchTypeName;
            batchModel.ManufacturingDate = General.FormatDate(obj.ManufacturingDate, "view");
            batchModel.ExpDate = General.FormatDate(obj.ExpiryDate, "view");
            batchModel.ISKPrice = obj.ISKPrice;
            batchModel.OSKPrice = obj.OSKPrice;
            batchModel.ExportPrice = obj.ExportPrice;
            batchModel.PurchaseMRP = obj.PurchaseMRP;
            batchModel.PurchaseLooseRate = obj.PurchaseLooseRate;
            batchModel.RetailMRP = obj.RetailMRP;
            batchModel.RetailLooseRate = obj.RetailLooseRate;
            batchModel.UnitID = obj.UnitID;
            batchModel.PackSize = obj.PackSize;
            batchModel.GstList = GstBL.GetGstList();
            batchModel.UOMList = unitBL.GetUnitsList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM,
                PackSize = a.PackSize
            }).ToList();
            batchModel.Trans = batchBL.GetBatchTrans((int)id,"Batch").Select(a => new PreviousBatchModel()
            {

                ID = a.ID,
                InvoiceNo = a.InvoiceNo,
                InvoiceDate = General.FormatDate(a.InvoiceDate, "view"),
                SupplierName = a.SupplierName,
                Unit = a.Unit,
                Quantity = a.Quantity,
                OfferQty = a.OfferQty,
                PurchasePrice = a.PurchasePrice,
                PurchaseLooseRate = a.PurchaseLooseRate,
                SalesRate = a.SalesRate,
                LooseSalesRate = a.LooseSalesRate,
                LooseQty = a.LooseQty,
                GSTPercentage = a.GSTPercentage,
                GSTAmount = a.GSTAmount,
                ProfitRatio = a.ProfitRatio,
                DiscountID = a.DiscountID,
                RetailMRP = a.RetailMRP,
                CessPercentage = a.CessPercentage,
                InvoiceRate = a.InvoiceRate,
                CGSTAmt=a.CGSTAmt,
                SGSTAmt=a.SGSTAmt,
                Discount=a.Discount,
                DiscountList = discountCategoryBL.GetDiscountList().Select(m => new DiscountCategoryModel()
                {
                    ID = (int)m.ID,
                    DiscountPercentage = (decimal)m.DiscountPercentage,
                    DiscountType = m.DiscountType,
                    Days = (int)m.Days,
                    DiscountCategory = m.DiscountCategory
                }).ToList(),

            }).ToList();
            return View(batchModel);
        }

        // GET: Masters/Batch/Create
        public ActionResult Create()
        {
            BatchModel batchModel = new BatchModel();
            batchModel.ManufacturingDate = General.FormatDate(DateTime.Now);
            return View(batchModel);
        }



        public ActionResult Save(BatchModel batchModel)
        {
            try
            {
                BatchBO batchBO = new BatchBO()
                {
                    ID = (int)batchModel.ID,
                    ExpiryDate = General.ToDateTime(batchModel.ExpDate),
                    CreatedDate = General.ToDateTime(batchModel.ManufacturingDate),
                    ItemID = batchModel.ItemID,
                    BatchNo = batchModel.BatchNo,
                    CustomBatchNo = batchModel.BatchNo,
                    ISKPrice = batchModel.ISKPrice,
                    OSKPrice = batchModel.OSKPrice,
                    ExportPrice = batchModel.ExportPrice
                };
                batchBL.Save(batchBO);
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateBatch(BatchModel batchModel)
        {
            int ID;
            try
            {
                BatchBO batchBO = new BatchBO()
                {
                    ID = (int)batchModel.ID,
                    ExpiryDate = General.ToDateTime(batchModel.ExpDate),
                    ManufacturingDate = General.ToDateTime(batchModel.ManufacturingDate),
                    ItemID = batchModel.ItemID,
                    BatchNo = batchModel.BatchNo,
                    CustomBatchNo = batchModel.CustomBatchNo,
                    ISKPrice = batchModel.ISKPrice,
                    OSKPrice = batchModel.OSKPrice,
                    ExportPrice = batchModel.ExportPrice,
                    RetailMRP = batchModel.RetailMRP,
                    RetailLooseRate = batchModel.RetailLooseRate,
                    BatchRate = batchModel.BatchRate,
                    PurchaseLooseRate = batchModel.PurchaseLooseRate,
                    UnitID = batchModel.UnitID,
                    ProfitPrice = batchModel.ProfitPrice

                };
                ID = batchBL.CreateBatch(batchBO);
                return Json(new { Status = "Success", data = ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Masters/Batch/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                BatchModel batchModel = new BatchModel();
                var obj = batchBL.GetBatchDetails(id);
                batchModel.ID = obj.ID;
                batchModel.BatchNo = obj.BatchNo;
                batchModel.CustomBatchNo = obj.CustomBatchNo;
                batchModel.ItemName = obj.Name;
                batchModel.Itemtype = obj.ItemType;
                batchModel.BatchTypeName = obj.BatchTypeName;
                batchModel.ManufacturingDate = General.FormatDate(obj.ManufacturingDate);
                batchModel.ExpDate = General.FormatDate(obj.ExpiryDate);
                batchModel.ISKPrice = obj.ISKPrice;
                batchModel.OSKPrice = obj.OSKPrice;
                batchModel.ExportPrice = obj.ExportPrice;
                batchModel.PurchaseMRP = obj.PurchaseMRP;
                batchModel.PurchaseLooseRate = obj.PurchaseLooseRate;
                batchModel.RetailMRP = obj.RetailMRP;
                batchModel.RetailLooseRate = obj.RetailLooseRate;
                batchModel.PackSize = obj.PackSize;
                return View(batchModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetAvailableBatchesForSales(int ItemID, string FullOrLoose, int WarehouseID, int ItemCategoryID, int PriceListID)
        {
            try
            {
                List<BatchModel> Batches = batchBL.GetAvailableBatchesForSales(ItemID, FullOrLoose, WarehouseID, ItemCategoryID, PriceListID).Select(
                    a => new BatchModel()
                    {
                        BatchNo = a.BatchNo,
                        CustomBatchNo = a.CustomBatchNo,
                        ExpiryDate = a.ExpiryDate,
                        BatchID = a.ID,
                        Rate = a.Rate,
                        Stock = a.Stock
                    }
                    ).ToList();

                return Json(new { Status = "success", Batches = Batches }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAvailableBatches(int ItemID, decimal OrderQty, int[] SalesOrderTransIDs, int WarehouseID, int CustomerID, int SchemeID, int UnitID, int ProformaInvoiceID = 0)
        {
            try
            {
                List<SalesBatchModel> Batches = batchBL.GetAvailableBatchesForSales(ItemID, OrderQty, SalesOrderTransIDs, WarehouseID, CustomerID, SchemeID, UnitID, ProformaInvoiceID).Select(
                    a => new SalesBatchModel()
                    {
                        BatchNo = a.BatchNo,
                        CustomBatchNo = a.CustomBatchNo,
                        ExpiryDate = a.ExpiryDate,
                        BatchID = a.BatchID,
                        BatchTypeID = a.BatchTypeID,
                        Rate = a.Rate,
                        Stock = a.Stock,
                        Qty = a.Qty,
                        OfferQty = a.OfferQty,
                        InvoiceQty = a.InvoiceQty,
                        InvoiceOfferQty = a.InvoiceOfferQty,
                        SalesOrderNo = a.SalesOrderNo,
                        SalesOrderTransID = a.SalesOrderTransID,
                        SalesOrderID = a.SalesOrderID
                    }
                    ).ToList();

                return Json(new { Status = "success", Batches = Batches }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAvailableBatchesForStockIssue(int ItemID, int WarehouseID, int BatchTypeID, decimal RequiredQty, int[] RequestTransIDs, int UnitID, int StockIssueID = 0)
        {
            try
            {
                List<StockIssueBatchModel> Batches = batchBL.GetAvailableBatchesForStockIssue(ItemID, RequiredQty, WarehouseID, RequestTransIDs, BatchTypeID, UnitID, StockIssueID).Select(
                    a => new StockIssueBatchModel()
                    {
                        BatchNo = a.BatchNo,
                        BatchTypeName = a.BatchTypeName,
                        CustomBatchNo = a.CustomBatchNo,
                        ExpiryDate = a.ExpiryDate,
                        BatchID = a.ID,
                        Rate = a.Rate,
                        Stock = a.Stock,
                        IssueQty = a.IssueQty,
                        StockRequisitionID = (int)a.StockRequisitionID,
                        StockRequisitionTransID = (int)a.StockRequisitionTransID,
                        StockRequisitionNo = a.StockRequisitionNo,

                    }
                    ).ToList();

                return Json(new { Status = "success", Batches = Batches }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetBatchWiseStock(int BatchID, int StoreID)
        {

            var res = new List<object>();
            decimal Stock = batchBL.GetBatchWiseStock(BatchID, StoreID);
            res.Add(new { Stock = Stock.ToString("n2") });
            return Json(new { Status = "success", data = res }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBatchWiseStockForPackingSemiFinishedGood(int BatchID, int StoreID, int ProductionGroupID)
        {

            var res = new List<object>();
            decimal Stock = batchBL.GetBatchWiseStockForPackingSemiFinishedGood(BatchID, StoreID, ProductionGroupID);

            return Json(new { Status = "success", data = Stock }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetBatchesByItemIDForCounterSales(int ItemID = 0, int WarehouseID = 0, int BatchTypeID = 0, int UnitID = 0, decimal Qty = 0)
        {
            try
            {
                List<SalesBatchModel> batchlist = batchBL.GetBatchesByItemIDForCounterSales(ItemID, WarehouseID, BatchTypeID, UnitID, Qty).Select(
                    m => new SalesBatchModel()
                    {

                        ItemID = m.ItemID,
                        BatchID = m.BatchID,
                        BatchTypeID = m.BatchTypeID,
                        BatchNo = m.BatchNo,
                        CGSTPercentage = m.CGSTPercentage,
                        SGSTPercentage = m.SGSTPercentage,
                        IGSTPercentage = m.IGSTPercentage,
                        Stock = (decimal)m.Stock,
                        Unit = m.Unit,
                        UnitID = m.UnitID,
                        Rate = m.Rate,
                        BatchType = m.BatchType,
                        Code = m.Code,
                        ExpiryDate = m.ExpiryDate,
                        FullRate = m.FullRate,
                        LooseRate = m.LooseRate,
                        SalesUnitID = m.SalesUnitID,
                        CessPercentage = m.CessPercentage,
                        IsGSTRegisteredLocation = m.IsGSTRegisteredLocation

                    }).ToList();
                return Json(new { Status = "success", Data = batchlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetBatchList(DatatableModel Datatable)
        {
            try
            {
                string BatchNoHint = Datatable.Columns[1].Search.Value;
                string CustomBatchNoHint = Datatable.Columns[0].Search.Value;
                string ItemNameHint = Datatable.Columns[2].Search.Value;
                string ItemCategoryHint = Datatable.Columns[3].Search.Value;
                string RetailMRPHint = Datatable.Columns[4].Search.Value;
                string BasePriceHint = Datatable.Columns[0].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = batchBL.GetAllBatchList(BatchNoHint, CustomBatchNoHint, ItemNameHint, ItemCategoryHint, RetailMRPHint, BasePriceHint, SortField, SortOrder, Offset, Limit);
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
        public JsonResult GetBatchForProductionIssueMaterialReturn(int productionIssueD, int itemID)
        {
            var batch = batchBL.GetBatchForProductionIssueMaterialReturn(productionIssueD, itemID);
            return Json(new { Status = "success", data = batch }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchesBatchTypeWise(int ItemID, int BatchTypeID)
        {
            var batch = batchBL.GetBatchesBatchTypeWise(ItemID, BatchTypeID);
            return Json(new { Status = "success", data = batch }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPreProcessItemBatchwise(int ItemID, int UnitID, decimal Quantity)
        {
            try
            {
                List<PreProcessBatchBO> Items = batchBL.GetPreProcessItemBatchwise(ItemID, UnitID, Quantity);

                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetBatchesForAutoComplete(int ItemID, string Hint = "")
        {
            //List<BatchModel> Batches = batchBL.GetBatchesForAutoComplete(ItemID, Hint).Select(
            //        a => new BatchModel()
            //        {
            //            BatchNo = a.BatchNo,
            //            BatchID = a.ID,
            //            BatchTypeID = a.BatchTypeID
            //        }
            //        ).ToList();
            DatatableResultBO resultBO = batchBL.GetBatchListForGrn(Hint, ItemID, "Name", "ASC", 0, 20);
            var Batches = new { data = resultBO.data };
            return Json(Batches, JsonRequestBehavior.AllowGet);
            //return Json(Batches, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchListForGrn(DatatableModel Datatable)
        {
            try
            {
                string BatchNoHint = Datatable.Columns[2].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ItemIDHint = Convert.ToInt32(Datatable.GetValueFromKey("ItemID", Datatable.Params));

                DatatableResultBO resultBO = batchBL.GetBatchListForGrn(BatchNoHint, ItemIDHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBatchForGrnAutocomplete(string BatchHint, int ItemHint)
        {
            try
            {
                DatatableResultBO resultBO = batchBL.GetBatchListForGrn(BatchHint, ItemHint, "Name", "ASC", 0, 20);
                var result = new { data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLatestBatchDetails(int ItemID)
        {
            try
            {
                BatchModel Batches = batchBL.GetLatestBatchDetails(ItemID).Select(
                    a => new BatchModel()
                    {
                        RetailMRP = a.RetailMRP,
                        RetailLooseRate = a.RetailLooseRate,
                        PurchaseLooseRate = a.PurchaseLooseRate,
                        BatchRate = a.BatchRate,
                        ProfitPrice = a.ProfitPrice,
                        UnitID = a.UnitID,
                        Unit = a.Unit,
                        PackSize = a.PackSize
                    }
                    ).FirstOrDefault();

                return Json(new { Status = "success", data = Batches }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult GetPreviousBatchDetails(int BatchID, decimal CurrentBatchNetProfit,
            decimal RetailMRP, decimal PurchasePrice, decimal DiscountAmt, decimal OfferQty, decimal ReceivedQty,
            string SupplierName, string ItemName, string Unit, int UnitID)
        {
            PreviousBatchModel Batches = new PreviousBatchModel();
            try
            {
                Batches = batchBL.GetPreviousBatchDetails(BatchID).Select(
                   a => new PreviousBatchModel()
                   {
                       RetailMRP = a.RetailMRP,
                       RetailLooseRate = a.RetailLooseRate,
                       PurchaseLooseRate = a.PurchaseLooseRate,
                       BatchRate = a.BatchRate,
                       ProfitPrice = a.ProfitPrice,
                       TransNo = a.TransNo,
                       InvoiceNo = a.InvoiceNo,
                       PODate = (a.TransNo == "-" ? "-" : General.FormatDate((DateTime)a.PODate)),
                       SupplierName = a.SupplierName,
                       Quantity = a.Quantity,
                       OfferQty = a.OfferQty,
                       ProfitRatio = a.ProfitRatio,
                       ProfitTolerance = a.ProfitTolerance,
                       CurrentBatchNetProfit = CurrentBatchNetProfit,
                       CurrentProfitTolerance = (CurrentBatchNetProfit * a.ProfitTolerance) / 100,
                       Profit = ((CurrentBatchNetProfit + ((CurrentBatchNetProfit * a.ProfitTolerance) / 100)) - a.ProfitRatio),
                       CurrentBatchRetailMRP = RetailMRP,
                       PurchasePrice = PurchasePrice,
                       Discount = a.Discount,
                       CurrentDiscountAmt = DiscountAmt,
                       CurrentOfferQty = OfferQty,
                       CurrentQty = ReceivedQty,
                       CurrentSupplierName = SupplierName,
                       ItemName = ItemName,
                       Unit = a.Unit,
                       UnitID = a.UnitID,
                       CurrentUnitID = UnitID,
                       CurrentUnit = Unit

                   }
                   ).FirstOrDefault();

                return PartialView("~/Areas/Masters/Views/Batch/_batchDetails.cshtml", Batches);
            }
            catch (Exception e)
            {
                return PartialView("~/Areas/Masters/Views/Batch/_batchDetails.cshtml", Batches);

            }
        }

        public PartialViewResult GetBatchInvoiceDetails(BatchModel model)
        {
            List<PreviousBatchModel> Batches = new List<PreviousBatchModel>();

            try
            {
                if (model.Trans != null)
                {
                    PreviousBatchModel Item;
                    foreach (var item in model.Trans)
                    {
                        Item = new PreviousBatchModel()
                        {
                            //ID = item.ID,
                            InvoiceNo = item.InvoiceNo,
                            InvoiceDate = item.InvoiceDate,
                            CurrentQty = item.CurrentQty,
                            Quantity = item.Quantity,
                            PreviousOfferQty = item.PreviousOfferQty,
                            PreviousQuantity=item.PreviousQuantity,
                            OfferQty = item.OfferQty,
                            InvoiceRate = item.InvoiceRate,
                            PreviousInvoiceRate = item.PreviousInvoiceRate,
                            PurchasePrice = item.PurchasePrice,
                            PreviousPurchasePrice = item.PreviousPurchasePrice,
                            PurchaseLooseRate = item.PurchaseLooseRate,
                            PreviousPurchaseLooseRate = item.PreviousPurchaseLooseRate,
                            SalesRate = item.SalesRate,
                            PreviousLooseSalesRate = item.PreviousLooseSalesRate,
                            LooseSalesRate = item.LooseSalesRate,
                            PreviousSalesRate = item.PreviousSalesRate,
                            LooseQty = item.LooseQty,
                            PreviousLooseQty = item.PreviousLooseQty,
                            GSTAmount = item.GSTAmount,
                            PreviousGSTAmount = item.PreviousGSTAmount,
                            Discount = item.Discount,
                            PreviousDiscount = item.PreviousDiscount,
                            PreviousProfitRatio = item.PreviousProfitRatio,
                            ProfitRatio = item.ProfitRatio,
                            SGSTAmt=item.SGSTAmt,
                            CGSTAmt=item.CGSTAmt,
                            PreviousCGSTAmt=item.PreviousCGSTAmt,
                            PreviousSGSTAmt=item.PreviousSGSTAmt,
                            Unit=item.Unit,
                            PreviousUnit=item.PreviousUnit
                        };
                        Batches.Add(Item);
                    }
                }
                return PartialView("~/Areas/Masters/Views/Batch/BatchInvoiceDetails.cshtml", Batches);
            }
            catch (Exception e)
            {

                return PartialView("~/Areas/Masters/Views/Batch/BatchInvoiceDetails.cshtml", Batches);

            }
        }

        public ActionResult EditBatchInvoice(BatchModel batchModel)
        {
            try
            {
                BatchBO batchBO = new BatchBO()
                {
                    ID = (int)batchModel.ID,
                    RetailMRP=batchModel.RetailMRP,
                    RetailLooseRate=batchModel.RetailLooseRate,
                    UnitID=batchModel.UnitID
                };
                List<PreviousBatchBO> ItemsBO = new List<PreviousBatchBO>();
                PreviousBatchBO ItemBO;
                if (batchModel.Trans != null)
                {
                    foreach (var item in batchModel.Trans)
                    {

                        ItemBO = new PreviousBatchBO()
                        {
                            InvoiceID=item.InvoiceID,
                            DiscountPercent=item.DiscountPercent,
                            Quantity=item.Quantity,
                            OfferQty=item.OfferQty,
                            InvoiceRate = item.InvoiceRate,
                            PurchasePrice=item.PurchasePrice,
                            PurchaseLooseRate=item.PurchaseLooseRate,
                            SalesRate=item.SalesRate,
                            LooseSalesRate=item.LooseSalesRate,
                            Discount=item.Discount,
                            ProfitRatio=item.ProfitRatio,
                            SGSTAmt=item.SGSTAmt,
                            CGSTAmt=item.CGSTAmt,
                            DiscountID=item.DiscountID,
                            GrossAmount=item.GrossAmount,
                            NetAmount=item.NetAmount
                        };
                      

                        ItemsBO.Add(ItemBO);
                    }
                }
                batchBL.EditBatchInvoice(batchBO, ItemsBO);
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //for Janaushadhi by neethu (649-696)
        public JsonResult GetCustomBatchForGrnAutocomplete(string BatchHint, int ItemHint)
        {
            try
            {
                DatatableResultBO resultBO = batchBL.GetCustomBatchForGrnAutocomplete(BatchHint, ItemHint, "Name", "ASC", 0, 20);
                var result = new { data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLatestBatchDetailsByCustomBatchNo(int ItemID,string CustomBatchNo)
        {
            try
            {
                BatchModel Batches = batchBL.GetLatestBatchDetailsByCustomBatchNo(ItemID, CustomBatchNo).Select(
                    a => new BatchModel()
                    {
                        ID = a.ID,
                        BatchNo = a.BatchNo,
                        ManufacturingDate = General.FormatDate(a.ManufacturingDate),
                        ExpDate = General.FormatDate(a.ExpiryDate),
                        ISKPrice = a.ISKPrice,
                        OSKPrice = a.OSKPrice,
                        ExportPrice = a.ExportPrice,
                        RetailMRP = a.RetailMRP,
                        RetailLooseRate = a.RetailLooseRate,
                        PurchaseMRP = a.PurchaseMRP,
                        PurchaseLooseRate = a.PurchaseLooseRate,
                        PackSize = a.PackSize,
                        UnitID = a.UnitID,
                        Unit = a.Unit

                    }
                    ).FirstOrDefault();

                return Json(new { Status = "success", data = Batches }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        
        public JsonResult GetBatchDetailsByBatch(int BatchID)
        {
            //generalBL.LogError("CounterSales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", 0, BatchNo, "", "");
            try
            {
                var obj = batchBL.GetBatchDetailsByBatchID(BatchID);
                //generalBL.LogError("CounterSales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", obj.ID, BatchNo, "", "");
                BatchModel batchModel = new BatchModel();
                batchModel.ID = obj.ID;
                batchModel.BatchNo = obj.BatchNo;
                batchModel.CustomBatchNo = obj.CustomBatchNo;
                batchModel.ItemName = obj.Name;
                batchModel.ExpDate = General.FormatDate(obj.ExpiryDate, "view");
                batchModel.Unit = obj.Unit;
                batchModel.UnitID = obj.UnitID;
                batchModel.Stock = obj.Stock;
                batchModel.ItemID = obj.ItemID;
                batchModel.GSTPercentage = obj.GSTPercentage;
                batchModel.CessPercentage = obj.CessPercentage;
                batchModel.RetailLooseRate = obj.RetailLooseRate;
                //batchModel.BusinessCategory = obj.BusinessCategory;
                //batchModel.BusinessCategoryID = obj.BusinessCategoryID;
                return Json(new { Status = "Success", Data = batchModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //generalBL.LogError("CounterSales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", 0, BatchNo, "", "");
                //generalBL.LogError("Sales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", 0, e);
                return Json(new { Status = "failure", Data = e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLatestBatchDetailsV3(int ItemID)
        {
            try
            {
                BatchModel Batches = batchBL.GetLatestBatchDetailsV3(ItemID).Select(
                    a => new BatchModel()
                    {
                        RetailMRP = a.RetailMRP,
                        RetailLooseRate = a.RetailLooseRate,
                        PurchaseLooseRate = a.PurchaseLooseRate,
                        BatchRate = a.BatchRate,
                        ProfitPrice = a.ProfitPrice,
                        UnitID = a.UnitID,
                        Unit = a.Unit,
                        PackSize = a.PackSize,
                        Category=a.Category,
                        PrimaryUnit=a.PrimaryUnit,
                        InventoryUnit=a.InventoryUnit,
                        PrimaryUnitID=a.PrimaryUnitID,
                        InventoryUnitID=a.InventoryUnitID,
                        ConversionFactorPtoI=a.ConversionFactorPtoI,
                        LooseRatePercent=a.LooseRatePercent,
                        GSTPercentage=a.GSTPercentage
                    }
                    ).FirstOrDefault();

                return Json(new { Status = "success", data = Batches }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}
