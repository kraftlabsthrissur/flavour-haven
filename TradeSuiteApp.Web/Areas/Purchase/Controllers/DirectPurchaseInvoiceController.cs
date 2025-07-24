using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class DirectPurchaseInvoiceController : Controller

    {
        private IGeneralContract generalBL;
        private ICategoryContract categoryBL;
        private IBatchTypeContract BatchBL;
        private IDirectPurchaseInvoiceContract DirectPurchaseInvoiceBL;
        private IWareHouseContract warehouseBL;
        private ILocationContract locationBL;

        public DirectPurchaseInvoiceController(IDropdownContract IDropdown)
        {
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            BatchBL = new BatchTypeBL();
            DirectPurchaseInvoiceBL = new DirectPurchaseInvoiceBL();
            warehouseBL = new WarehouseBL();
            locationBL = new LocationBL();
        }
        // GET: Purchase/DirectPurchaseInvoice
        public ActionResult Index()
        {
            return View();
        }
        //public JsonResult GetBatchwiseItemForDirectPurchaseInvoice(int ItemID = 0, int WarehouseID = 0, int BatchTypeID = 0, decimal Qty = 0, int UnitID = 0, string Unit = "", string CustomerType = "", int TaxTypeID = 0)

        //{
        //    try
        //    {
        //        List<LocalPurchaseInvoiceModel> itemlist = DirectPurchaseInvoiceBL.GetBatchwiseItemForDirectPurchaseInvoice(ItemID, WarehouseID, BatchTypeID, UnitID, Qty, CustomerType, TaxTypeID).Select(
        //            m => new LocalPurchaseInvoiceItemsModel()
        //            {

        //                ItemID = m.ItemID,
        //                BatchID = m.BatchID,
        //                BatchTypeID = m.BatchTypeID,
        //                BatchNo = m.BatchNo,
        //                CGSTPercent = m.CGSTPercentage,
        //                SGSTPercent = m.SGSTPercentage,
        //                IGSTPercent = m.IGSTPercentage,
        //                VATPercentage = m.VATPercentage,
        //                Stock = m.Stock,
        //                Unit = Unit,
        //                UnitID = UnitID,
        //                FullPrice = m.FullPrice,
        //                LoosePrice = m.LoosePrice,
        //                BatchType = m.BatchType,
        //                Code = m.Code,
        //                ExpDate = General.FormatDateNull(m.ExpiryDate),
        //                Quantity = m.Quantity,
        //                WareHouseID = WarehouseID,
        //                Name = m.Name,
        //                SalesUnitID = m.SalesUnitID,
        //                CessPercentage = m.CessPercentage,
        //                IsGSTRegisteredLocation = m.IsGSTRegisteredLocation,
        //                IsGST = m.IsGST,
        //                IsVAT = m.IsVAT,
        //                TaxType = m.TaxType,
        //                CurrencyID = (int)m.CurrencyID,
        //                CurrencyName = m.CurrencyName,
        //            }).ToList();


        //        return Json(new { Status = "success", Data = itemlist }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        generalBL.LogError("Sales", "CounterSales", "GetBatchwiseItemForCounterSales", 0, e);
        //        return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult Create()
        {
            LocalPurchaseInvoiceModel LocalPurchaseModel = new LocalPurchaseInvoiceModel();
            LocalPurchaseModel.PurchaseOrderNo = generalBL.GetSerialNo("LocalPurchaseInvoice", "Code");
            LocalPurchaseModel.PurchaseOrderDate = General.FormatDate(DateTime.Now);
            LocalPurchaseModel.DDLItemCategory = Convert.ToInt32(generalBL.GetConfig("DefaultCategoryForLocalPurchase"));
            LocalPurchaseModel.PurchaseCategoryList = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            LocalPurchaseModel.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            LocalPurchaseModel.BatchTypeList = new SelectList(
                                            BatchBL.GetBatchTypeList(), "ID", "Name");
            LocalPurchaseModel.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}
                                    }, "Value", "Text");
            LocalPurchaseModel.StoreList = new SelectList(warehouseBL.GetWareHousesByLocation(GeneralBO.LocationID), "ID", "Name");
            LocalPurchaseModel.IsGSTRegisteredLocation = generalBL.IsGSTRegisteredLocation(GeneralBO.LocationID);
            LocalPurchaseModel.InvoiceDate = General.FormatDate(DateTime.Now);
            LocalPurchaseModel.LocationID = GeneralBO.LocationID;
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                //LocalPurchaseModel.Description = currency.Description;
                LocalPurchaseModel.CurrencyID = currency.CurrencyID;
                LocalPurchaseModel.CurrencyName = currency.CurrencyName;
                LocalPurchaseModel.CountryName = currency.CountryName;
                LocalPurchaseModel.CurrencyCode = currency.CurrencyCode;
                LocalPurchaseModel.DecimalPlaces = currency.DecimalPlaces;
                LocalPurchaseModel.CountryID = currency.CountryID;
                LocalPurchaseModel.IsVat = currency.IsVat;
                LocalPurchaseModel.IsGST = currency.IsGST;
                LocalPurchaseModel.TaxType = currency.TaxType;
                LocalPurchaseModel.TaxTypeID = currency.TaxTypeID;

            }
            return View(LocalPurchaseModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            List<PurchaseOrderTransBO> PurchaseOrderTransBO;
            LocalPurchaseInvoiceModel LocalPurchaseInvoiceModel;
            LocalPurchaseInvoiceItemsModel localPurchaseInvoiceItems;
            LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO = DirectPurchaseInvoiceBL.GetDirectPurchaseOrder((int)id);
            if (!LocalPurchaseInvoiceBO.IsDraft || LocalPurchaseInvoiceBO.IsCanceled)
            {
                return RedirectToAction("Index"); ;
            }
            LocalPurchaseInvoiceModel = new LocalPurchaseInvoiceModel()
            {
                ID = LocalPurchaseInvoiceBO.ID,
                CurrencyCode = LocalPurchaseInvoiceBO.CurrencyCode,
                PurchaseOrderNo = LocalPurchaseInvoiceBO.PurchaseOrderNo,
                PurchaseOrderDate = General.FormatDate(LocalPurchaseInvoiceBO.PurchaseOrderDate),
                SupplierReference = LocalPurchaseInvoiceBO.SupplierReference,
                NetAmount = LocalPurchaseInvoiceBO.NetAmount,
                GSTAmount = LocalPurchaseInvoiceBO.CGSTAmount + LocalPurchaseInvoiceBO.SGSTAmount + LocalPurchaseInvoiceBO.IGSTAmount,
                TaxableAmount = LocalPurchaseInvoiceBO.TaxableAmount,
                CGSTAmount = LocalPurchaseInvoiceBO.CGSTAmount,
                IGSTAmount = LocalPurchaseInvoiceBO.IGSTAmount,
                SGSTAmount = LocalPurchaseInvoiceBO.SGSTAmount,
                SupplierID = LocalPurchaseInvoiceBO.SupplierID,
                SupplierStateID = LocalPurchaseInvoiceBO.SupplierStateID,
                VATAmount = LocalPurchaseInvoiceBO.VATAmount,
                IsDraft = LocalPurchaseInvoiceBO.IsDraft,
                IsGST = LocalPurchaseInvoiceBO.IsGST,
                IsVat = LocalPurchaseInvoiceBO.IsVAT,
                Discount = LocalPurchaseInvoiceBO.Discount,
                OtherDeductions = LocalPurchaseInvoiceBO.OtherDeductions,
                StoreID = LocalPurchaseInvoiceBO.StoreID,
                Store = LocalPurchaseInvoiceBO.Store,
                IsGSTRegistered = LocalPurchaseInvoiceBO.IsGSTRegistered,
                IsCanceled = LocalPurchaseInvoiceBO.IsCanceled,
                InvoiceDate = General.FormatDate(LocalPurchaseInvoiceBO.InvoiceDate),
                InvoiceNo = LocalPurchaseInvoiceBO.InvoiceNo,
            };

            PurchaseOrderTransBO = DirectPurchaseInvoiceBL.GetDirectPurchaseOrderItems((int)id);
            LocalPurchaseInvoiceModel.Items = new List<LocalPurchaseInvoiceItemsModel>();
            foreach (var m in PurchaseOrderTransBO)
            {
                localPurchaseInvoiceItems = new LocalPurchaseInvoiceItemsModel()
                {
                    ItemID = m.ItemID,
                    ItemCode = m.ItemCode,
                    ItemName = m.ItemName,
                    UnitID = m.UnitID,
                    Qty = (decimal)m.QtyOrdered,
                    Rate = (decimal)m.Rate,
                    //Remarks = m.Remarks,
                    TaxableAmount = (decimal)m.Amount,
                    CGSTAmount = (decimal)m.CGSTAmt,
                    SGSTAmount = (decimal)m.SGSTAmt,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    IGSTAmount = (decimal)m.IGSTAmt,
                    VATAmount = (decimal)m.VATAmount,
                    VATPercentage = (decimal)m.VATPercentage,
                    RetailRate = m.RetailRate.HasValue ? m.RetailRate.Value : 0,
                    RetailMRP = m.RetailMRP.HasValue ? m.RetailMRP.Value : 0,
                    PartsNumber = m.PartsNumber,
                    Remark = m.Remark,
                    Model = m.Model,
                    TotalAmount = (decimal)m.NetAmount,
                    IsGST = (int)m.IsGST,
                    IsVAT = (int)m.IsVat,
                    Unit = m.Unit,
                    Value = (decimal)m.Amount,
                    BatchNo = m.BatchNo,
                    //ExpDate = General.FormatDate(m.ExpDate),
                    GSTAmount = (decimal)m.SGSTAmt + (decimal)m.CGSTAmt + (decimal)m.IGSTAmt,
                    MRP = (decimal)m.MRP,
                    Discount = m.Discount,
                    DiscountPercent = m.DiscountPercent
                };
                LocalPurchaseInvoiceModel.Items.Add(localPurchaseInvoiceItems);
            }
            LocalPurchaseInvoiceModel.IsCancelable = DirectPurchaseInvoiceBL.IsCancelable((int)id);
            LocalPurchaseInvoiceModel.DDLItemCategory = Convert.ToInt32(generalBL.GetConfig("DefaultCategoryForLocalPurchase"));
            LocalPurchaseInvoiceModel.PurchaseCategoryList = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            LocalPurchaseInvoiceModel.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            LocalPurchaseInvoiceModel.BatchTypeList = new SelectList(BatchBL.GetBatchTypeList(), "ID", "Name");
            LocalPurchaseInvoiceModel.StoreList = new SelectList(warehouseBL.GetWareHousesByLocation(GeneralBO.LocationID), "ID", "Name");
            LocalPurchaseInvoiceModel.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}
                                    }, "Value", "Text");
            return View(LocalPurchaseInvoiceModel);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            List<PurchaseOrderTransBO> PurchaseOrderTransBO;
            LocalPurchaseInvoiceModel LocalPurchaseInvoiceModel;
            LocalPurchaseInvoiceItemsModel localPurchaseInvoiceItems;
            LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO = DirectPurchaseInvoiceBL.GetDirectPurchaseOrder((int)id);
            LocalPurchaseInvoiceModel = new LocalPurchaseInvoiceModel()
            {
                ID = LocalPurchaseInvoiceBO.ID,
                CurrencyCode = LocalPurchaseInvoiceBO.CurrencyCode,
                PurchaseOrderNo = LocalPurchaseInvoiceBO.PurchaseOrderNo,
                PurchaseOrderDate = General.FormatDate(LocalPurchaseInvoiceBO.PurchaseOrderDate),
                SupplierReference = LocalPurchaseInvoiceBO.SupplierReference,
                NetAmount = LocalPurchaseInvoiceBO.NetAmount,
                GSTAmount = LocalPurchaseInvoiceBO.GSTAmount,
                TaxableAmount = LocalPurchaseInvoiceBO.TaxableAmount,
                CGSTAmount = LocalPurchaseInvoiceBO.CGSTAmount,
                IGSTAmount = LocalPurchaseInvoiceBO.IGSTAmount,
                SGSTAmount = LocalPurchaseInvoiceBO.SGSTAmount,
                VATAmount = LocalPurchaseInvoiceBO.VATAmount,
                SupplierID = LocalPurchaseInvoiceBO.SupplierID,
                SupplierStateID = LocalPurchaseInvoiceBO.SupplierStateID,
                IsDraft = LocalPurchaseInvoiceBO.IsDraft,
                Discount = LocalPurchaseInvoiceBO.Discount,
                OtherDeductions = LocalPurchaseInvoiceBO.OtherDeductions,
                StoreID = LocalPurchaseInvoiceBO.StoreID,
                Store = LocalPurchaseInvoiceBO.Store,
                IsCanceled = LocalPurchaseInvoiceBO.IsCanceled,
                IsGST = LocalPurchaseInvoiceBO.IsGST,
                IsVat = LocalPurchaseInvoiceBO.IsVAT,
                InvoiceDate = General.FormatDate(LocalPurchaseInvoiceBO.InvoiceDate),
                InvoiceNo = LocalPurchaseInvoiceBO.InvoiceNo,
            };

            PurchaseOrderTransBO = DirectPurchaseInvoiceBL.GetDirectPurchaseOrderItems((int)id);
            LocalPurchaseInvoiceModel.Items = new List<LocalPurchaseInvoiceItemsModel>();
            foreach (var m in PurchaseOrderTransBO)
            {
                localPurchaseInvoiceItems = new LocalPurchaseInvoiceItemsModel()
                {
                    ItemID = m.ItemID,
                    ItemCode = m.ItemCode,
                    ItemName = m.ItemName,
                    UnitID = m.UnitID,
                    Qty = (decimal)m.QtyOrdered,
                    Rate = (decimal)m.Rate,
                    //Remarks = m.Remarks,
                    TaxableAmount = (decimal)m.Amount,
                    CGSTAmount = (decimal)m.CGSTAmt,
                    SGSTAmount = (decimal)m.SGSTAmt,
                    VATAmount = (decimal)m.VATAmount,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    IGSTPercent = (decimal)m.IGSTPercent,
                    IGSTAmount = (decimal)m.IGSTAmt,
                    GSTPercentage = (decimal)m.GSTPercentage,
                    VATPercentage = (decimal)m.VATPercentage,
                    PartsNumber = m.PartsNumber,
                    Remark = m.Remark,
                    Model = m.Model,
                    TotalAmount = (decimal)m.NetAmount,
                    Unit = m.Unit,
                    CurrencyName = m.CurrencyName,
                    Value = (decimal)m.Amount,
                    BatchNo = m.BatchNo,
                    //ExpDate = General.FormatDate(m.ExpDate),
                    GSTAmount = (decimal)m.SGSTAmt + (decimal)m.CGSTAmt + (decimal)m.IGSTAmt,
                    MRP = (decimal)m.MRP,
                    Discount = m.Discount,
                    DiscountPercent = m.DiscountPercent
                };
                LocalPurchaseInvoiceModel.Items.Add(localPurchaseInvoiceItems);
            }
            LocalPurchaseInvoiceModel.IsCancelable = DirectPurchaseInvoiceBL.IsCancelable((int)id);
            LocalPurchaseInvoiceModel.DDLItemCategory = Convert.ToInt32(generalBL.GetConfig("DefaultCategoryForLocalPurchase"));
            LocalPurchaseInvoiceModel.PurchaseCategoryList = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            LocalPurchaseInvoiceModel.ItemCategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            LocalPurchaseInvoiceModel.BatchTypeList = new SelectList(BatchBL.GetBatchTypeList(), "ID", "Name");
            LocalPurchaseInvoiceModel.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}
                                    }, "Value", "Text");
            LocalPurchaseInvoiceModel.StoreList = new SelectList(warehouseBL.GetWareHousesByLocation(GeneralBO.LocationID), "ID", "Name");
            LocalPurchaseInvoiceModel.LocationID = GeneralBO.LocationID;
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                //LocalPurchaseModel.Description = currency.Description;
                LocalPurchaseInvoiceModel.CurrencyID = currency.CurrencyID;
                LocalPurchaseInvoiceModel.CurrencyName = currency.CurrencyName;
                LocalPurchaseInvoiceModel.CountryName = currency.CountryName;
                LocalPurchaseInvoiceModel.CountryID = currency.CountryID;
                LocalPurchaseInvoiceModel.IsVat = currency.IsVat;
                LocalPurchaseInvoiceModel.IsGST = currency.IsGST;
                LocalPurchaseInvoiceModel.TaxType = currency.TaxType;
                LocalPurchaseInvoiceModel.TaxTypeID = currency.TaxTypeID;

            }
            return View(LocalPurchaseInvoiceModel);

        }

        [HttpPost]
        public ActionResult Save(LocalPurchaseInvoiceModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    //LocalPurchaseInvoiceBO Temp = LocalPurchaseInvoiceBL.GetLocalPurchaseOrder(model.ID);
                    //if (!Temp.IsDraft)
                    //{
                    //    result.Add(new { ErrorMessage = "Not Editable" });
                    //    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    //}
                }
                LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO = new LocalPurchaseInvoiceBO()
                {
                    ID = model.ID,
                    IsDraft = model.IsDraft,
                    PurchaseOrderNo = model.PurchaseOrderNo,
                    PurchaseOrderDate = General.ToDateTime(model.PurchaseOrderDate),
                    SupplierReference = model.SupplierReference,
                    GSTAmount = model.GSTAmount,
                    NetAmount = model.NetAmount,
                    SupplierID = model.SupplierID,
                    GrossAmnt = model.GrossAmnt,
                    TaxableAmount = model.TaxableAmount,
                    IGSTAmount = model.IGSTAmount,
                    SGSTAmount = model.SGSTAmount,
                    CGSTAmount = model.CGSTAmount,
                    StoreID = model.StoreID,
                    Discount = model.Discount,
                    OtherDeductions = model.OtherDeductions,
                    InvoiceNo = model.InvoiceNo,
                    IsVAT = model.IsVat,
                    IsGST = model.IsGST,
                    VATAmount = model.VATAmount,
                    CurrencyID = model.CurrencyID,
                    InvoiceDate = General.ToDateTime(model.InvoiceDate)
                };
                List<PurchaseOrderTransBO> LocalPurchaseInvoiceItems = new List<PurchaseOrderTransBO>();
                PurchaseOrderTransBO LocalPurchaseInvoiceItem;

                string XMLItems = "<ArrayOfPurchaseOrderTransBO>";
                foreach (var item in model.Items)
                {
                    XMLItems += "<PurchaseOrderTransBO>";
                    XMLItems += "<PRTransID>0</PRTransID>";
                    XMLItems += "<ItemID>" + item.ItemID + "</ItemID>";
                    XMLItems += "<Quantity>" + item.Qty + "</Quantity>";
                    XMLItems += "<Rate>" + item.Rate + "</Rate>";
                    XMLItems += "<Amount>" + item.TaxableAmount + "</Amount>";
                    XMLItems += "<SGSTPercent>" + item.SGSTPercent + "</SGSTPercent>";
                    XMLItems += "<CGSTPercent>" + item.CGSTPercent + "</CGSTPercent>";
                    XMLItems += "<IGSTPercent>" + item.IGSTPercent + "</IGSTPercent>";
                    XMLItems += "<VATPercentage>" + item.VATPercentage + "</VATPercentage>";
                    XMLItems += "<SGSTAmt>" + item.SGSTAmount + "</SGSTAmt>";
                    XMLItems += "<CGSTAmt>" + item.CGSTAmount + "</CGSTAmt>";
                    XMLItems += "<IGSTAmt>" + item.IGSTAmount + "</IGSTAmt>";
                    XMLItems += "<VATAmount>" + item.VATAmount + "</VATAmount>";
                    XMLItems += "<NetAmount>" + item.TotalAmount + "</NetAmount>";
                    XMLItems += "<LastPurchaseRate>0</LastPurchaseRate>";
                    XMLItems += "<LowestPR>0</LowestPR>";
                    XMLItems += "<QtyInQC>0</QtyInQC>";
                    XMLItems += "<QtyAvailable>0</QtyAvailable>";
                    XMLItems += "<QtyOrdered>" + item.Qty + "</QtyOrdered>";
                    XMLItems += "<Purchased>0</Purchased>";
                    XMLItems += "<QtyMet>" + item.Qty + "</QtyMet>";
                    XMLItems += "<Remarks>" + item.Remarks + "</Remarks>";
                    XMLItems += "<BatchTypeID>" + item.BatchTypeID + "</BatchTypeID>";
                    XMLItems += "<BatchNo>" + item.BatchNo + "</BatchNo>";
                    //XMLItems += "<ExpDate>" + item.ExpDate + "</ExpDate>";
                    XMLItems += "<UnitID>" + item.UnitID + "</UnitID>";
                    XMLItems += "<MRP>" + item.MRP + "</MRP>";
                    XMLItems += "<RetailMRP>" + item.RetailMRP + "</RetailMRP>";
                    XMLItems += "<RetailRate>" + item.RetailRate + "</RetailRate>";
                    XMLItems += "<PurchaseRequisitionID>0</PurchaseRequisitionID>";
                    XMLItems += "<PurchaseRequisitionTrasID>0</PurchaseRequisitionTrasID>";
                    XMLItems += "<Discount>" + item.Discount + "</Discount>";
                    XMLItems += "<DiscountPercent>" + item.DiscountPercent + "</DiscountPercent>";
                    XMLItems += "<IsGST>" + model.IsGST + "</IsGST>";
                    XMLItems += "<IsVat>" + model.IsVat + "</IsVat>";
                    XMLItems += "<CurrencyID>" + model.CurrencyID + "</CurrencyID>";
                    XMLItems += "<ItemCode>" + item.ItemCode + "</ItemCode>";
                    XMLItems += "<ItemName>" + item.ItemName + "</ItemName>";
                    XMLItems += "<PartsNumber>" + item.PartsNumber + "</PartsNumber>";
                    XMLItems += "<Remark>" + item.Remark + "</Remark>";
                    XMLItems += "<Model>" + item.Model + "</Model>";
                    XMLItems += "<ExchangeRate>" + item.ExchangeRate + "</ExchangeRate>";
                    XMLItems += "<SecondaryUnit>" + item.SecondaryUnit + "</SecondaryUnit>";
                    XMLItems += "<SecondaryUnitSize>" + item.SecondaryUnitSize + "</SecondaryUnitSize>";
                    XMLItems += "<SecondaryQty>" + item.SecondaryQty + "</SecondaryQty>";
                    XMLItems += "<SecondaryRate>" + item.SecondaryRate + "</SecondaryRate>";
                    XMLItems += "</PurchaseOrderTransBO>";
                }
                XMLItems += "</ArrayOfPurchaseOrderTransBO>";
                var statusID = DirectPurchaseInvoiceBL.Save(LocalPurchaseInvoiceBO, XMLItems);
                if (statusID == 1)
                    return Json(new { Status = "success", data = "", message = "" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Status = "failure", data = "", message = "Error Occured" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "LocalPurchaseInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAsDraft(LocalPurchaseInvoiceModel model)
        {
            return Save(model);
        }

        [HttpPost]
        public JsonResult GetDirectPurchaseInvoiceList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SupplierHint = Datatable.Columns[3].Search.Value;
                string NetAmountHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                DatatableResultBO resultBO = DirectPurchaseInvoiceBL.GetDirectPurchaseInvoiceList(Type, TransNoHint, TransDateHint, SupplierHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMRPForPurchaseInvoice(int ItemID)
        {
            List<MRPModel> itemList = new List<MRPModel>();
            itemList = DirectPurchaseInvoiceBL.GetMRPForPurchaseInvoice(ItemID).Select(a => new MRPModel()
            {
                MRP = a.MRP,
                Rate = a.Rate
            }).ToList();
            return Json(new { Status = "success", Data = itemList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMRPForPurchaseInvoiceByBatchID(int ItemID, string Batch)
        {
            List<MRPModel> itemList = new List<MRPModel>();
            itemList = DirectPurchaseInvoiceBL.GetMRPForPurchaseInvoiceByBatchID(ItemID, Batch).Select(a => new MRPModel()
            {
                MRP = a.MRP,
                ExpDate = General.FormatDate(a.ExpDate)
            }).ToList();
            return Json(new { Status = "success", Data = itemList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cancel(int PurchaseOrderID)
        {
            try
            {
                if (DirectPurchaseInvoiceBL.IsCancelable(PurchaseOrderID))
                {
                    DirectPurchaseInvoiceBL.Cancel(PurchaseOrderID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                generalBL.LogError("Purchase", "DirectPurchaseInvoice", "Cancel", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUnitsAndCategoryByItemID(int ItemID)
        {
            List<ItemCategoryAndUnitModel> itemList = new List<ItemCategoryAndUnitModel>();
            itemList = DirectPurchaseInvoiceBL.GetUnitsAndCategoryByItemID(ItemID).Select(a => new ItemCategoryAndUnitModel()
            {
                Category = a.Category,
                ConversionFactorPtoI = a.ConversionFactorPtoI,
                PrimaryUnit = a.PrimaryUnit,
                PrimaryUnitID = a.PrimaryUnitID,
                PurchaseUnit = a.PurchaseUnit,
                PurchaseUnitID = a.PurchaseUnitID
            }).ToList();
            return Json(new { Status = "success", Data = itemList }, JsonRequestBehavior.AllowGet);
        }


    }
}