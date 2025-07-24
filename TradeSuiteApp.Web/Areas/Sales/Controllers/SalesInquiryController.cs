using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;
using System.Web.UI.WebControls;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net;
using System.Xml.Linq;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{

    public class SalesInquiryController : Controller
    {
        private ISalesInquiry salesInquiryBL;
        private IGeneralContract generalBL;
        private IUnitContract unitBL;
        private ILocationContract locationBL;
        private ICounterSalesContract counterSalesBL;
        public SalesInquiryController()
        {
            salesInquiryBL = new SalesInquiryBL();
            generalBL = new GeneralBL();
            unitBL = new UnitBL();
            locationBL = new LocationBL();
            counterSalesBL = new CounterSalesBL();
        }

        // GET: Sales/SalesOrder
        public ActionResult Index()
        {

            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled", "approved version2" };
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                ViewBag.normalclass = classdata.normalclass;
            }
            return View();
        }
        private SalesInquiryModel GetSalesInquiry(int SalesInquiryID, string Type)
        {

            SalesInquiryModel SalesInauiry;
            SalesInquiryItemModel SalesInauiryItem;
            List<SalesItemBO> SalesItems;

            SalesInquiryBO SalesOrderBO = salesInquiryBL.GetSalesInquiry(SalesInquiryID);
            SalesInauiry = new SalesInquiryModel()
            {
                ID = SalesOrderBO.ID,
                IsDraft = SalesOrderBO.IsDraft,
                SalesInquiryNo = SalesOrderBO.SalesInquiryNo,
                SalesInquiryDate = SalesOrderBO.SalesInquiryDate.HasValue ? General.FormatDate(SalesOrderBO.SalesInquiryDate.Value, Type) : "",
                RequestedDelivaryDate = SalesOrderBO.RequestedDelivaryDate.HasValue ? General.FormatDate(SalesOrderBO.RequestedDelivaryDate.Value, Type) : "",
                RequestExpiryDate = SalesOrderBO.RequestExpiryDate.HasValue ? General.FormatDate(SalesOrderBO.RequestExpiryDate.Value, Type) : "",
                RequestedCustomerName = SalesOrderBO.RequestedCustomerName,
                RequestedCustomerAddress = SalesOrderBO.RequestedCustomerAddress,
                PhoneNo1 = SalesOrderBO.PhoneNo1,
                PhoneNo2 = SalesOrderBO.PhoneNo2,
                Make = SalesOrderBO.Make,
                Model = SalesOrderBO.Model,
                Year = SalesOrderBO.Year,
                SIOrVINNumber = SalesOrderBO.SIOrVINNumber,
                GrossAmount = (decimal)SalesOrderBO.GrossAmount,
                NetAmount = (decimal)SalesOrderBO.NetAmount,
                Remarks = SalesOrderBO.Remarks
            };
            SalesItems = salesInquiryBL.GetSalesInquiryItems(SalesInquiryID);
            SalesInauiry.Items = new List<SalesInquiryItemModel>();
            foreach (var item in SalesItems)
            {
                SalesInauiryItem = new SalesInquiryItemModel()
                {
                    SalesInquiryItemID = item.SalesInquiryID,
                    ItemID = item.ItemID,
                    ItemCode = item.Code,
                    ItemName = item.Name,
                    PartsNumber = item.PartsNumber,
                    Remarks = item.Remarks,
                    Model = item.Model,
                    Year = item.Year,
                    SIOrVINNumber = item.SIOrVINNumber,
                    DeliveryTerm = item.DeliveryTerm,
                    UnitName = item.UnitName,
                    Qty = (decimal)item.Qty,
                    Rate = (decimal)item.Rate,
                    GrossAmount = (decimal)item.GrossAmount,
                    VATAmount = item.VATAmount,
                    VATPercentage = item.VATPercentage,
                    NetAmount = (decimal)item.NetAmount
                };
                SalesInauiry.Items.Add(SalesInauiryItem);
            }
            return SalesInauiry;
        }
        private List<SecondaryUnitBO> SecondaryUnitList(string Unit, string SecondaryUnits)
        {
            List<SecondaryUnitBO> secondaryUnits = new List<SecondaryUnitBO>();
            SecondaryUnitBO primaryUnitBO = new SecondaryUnitBO();
            primaryUnitBO.Name = Unit;
            primaryUnitBO.PackSize = 1;
            secondaryUnits.Add(primaryUnitBO);
            string[] SecondaryUnitsArray = SecondaryUnits.Split(',');
            for (int i = 0; i < SecondaryUnitsArray.Length; i++)
            {
                var SecondaryUnitItem = SecondaryUnitsArray[i].Split('|'); ;
                if (SecondaryUnitItem.Length > 1)
                {
                    SecondaryUnitBO secondaryUnitBO = new SecondaryUnitBO();
                    var text = SecondaryUnitItem[0];
                    var value = SecondaryUnitItem[1];
                    secondaryUnitBO.Name = text;
                    secondaryUnitBO.PackSize = Convert.ToDecimal(value);
                    secondaryUnits.Add(secondaryUnitBO);
                }
            }
            return secondaryUnits;
        }
        // GET: Sales/SalesOrder/Details/5
        public ActionResult Details(int? id)
        {
            SalesInquiryModel SalesInquiry;
            int SalesInquiryID;

            SalesInquiryID = (int)id;
            SalesInquiry = this.GetSalesInquiry(SalesInquiryID, "view");
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            var CurrencyID = 0;
            if (currency != null)
            {
                CurrencyID = currency.CurrencyID;
                SalesInquiry.DecimalPlaces = currency.DecimalPlaces;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(CurrencyID);
            if (classdata != null)
            {
                SalesInquiry.normalclass = classdata.normalclass;
            }
            return View(SalesInquiry);
        }
        // GET: Sales/SalesOrder/Create
        public ActionResult Create()
        {
            SalesInquiryModel SalesInquiry = new SalesInquiryModel();
            SalesInquiry.SalesInquiryNo = generalBL.GetSerialNo("SalesInquiry", "Code");
            SalesInquiry.SalesInquiryDate = General.FormatDate(DateTime.Now);
            SalesInquiry.Items = new List<SalesInquiryItemModel>();
            SalesInquiry.UnitList = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "", Value = "0" } }, "Value", "Text");
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            var CurrencyID = 0;
            if (currency != null)
            {
                CurrencyID = currency.CurrencyID;
                SalesInquiry.DecimalPlaces = currency.DecimalPlaces;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(CurrencyID);
            if (classdata != null)
            {
                SalesInquiry.normalclass = classdata.normalclass;
            }
            return View(SalesInquiry);
        }

        [HttpPost]
        public ActionResult SaveAsDraft(SalesInquiryModel SalesOrder)
        {
            return Save(SalesOrder);
        }

        // POST: Sales/SalesOrder/Create
        [HttpPost]
        public ActionResult Save(SalesInquiryModel SalesInquiry)
        {
            var result = new List<object>();
            try
            {

                SalesInquiryBO salesInquiryBO = new SalesInquiryBO()
                {
                    ID = SalesInquiry.ID,
                    IsDraft = SalesInquiry.IsDraft,
                    SalesInquiryNo = SalesInquiry.SalesInquiryNo,
                    SalesInquiryDate = General.ToDateTimeNull(SalesInquiry.SalesInquiryDate),
                    RequestedCustomerName = !string.IsNullOrEmpty(SalesInquiry.RequestedCustomerName) ? SalesInquiry.RequestedCustomerName.Trim() : SalesInquiry.RequestedCustomerName,
                    RequestedDelivaryDate = General.ToDateTimeNull(SalesInquiry.RequestedDelivaryDate),
                    RequestExpiryDate = General.ToDateTimeNull(SalesInquiry.RequestExpiryDate),
                    Status = SalesInquiry.Status,
                    NetAmount = SalesInquiry.NetAmount,
                    RequestedCustomerAddress = SalesInquiry.RequestedCustomerAddress,
                    Remarks = SalesInquiry.Remarks,
                    PhoneNo1 = SalesInquiry.PhoneNo1,
                    PhoneNo2 = SalesInquiry.PhoneNo2,
                    Make = SalesInquiry.Make,
                    Model = SalesInquiry.Model,
                    Year = SalesInquiry.Year,
                    SIOrVINNumber = SalesInquiry.SIOrVINNumber,
                };

                List<SalesItemBO> SalesItems = new List<SalesItemBO>();
                SalesItemBO SalesItem;
                foreach (var item in SalesInquiry.Items)
                {
                    SalesItem = new SalesItemBO()
                    {
                        ItemID = item.ItemID,
                        Code = item.ItemCode,
                        ItemName = item.ItemName,
                        UnitName = item.UnitName,
                        PartsNumber = item.PartsNumber,
                        DeliveryTerm = item.DeliveryTerm,
                        Year = item.Year,
                        SIOrVINNumber = item.SIOrVINNumber,
                        Model = item.Model,
                        Rate = item.Rate,
                        Remarks = item.Remarks,
                        Qty = item.Qty,
                        GrossAmount = item.GrossAmount,
                        NetAmount = item.NetAmount,
                    };
                    SalesItems.Add(SalesItem);
                }
                if (salesInquiryBL.SaveSalesInquiry(salesInquiryBO, SalesItems))
                {
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "SalesOrder", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/SalesOrder/Edit/5
        public ActionResult Edit(int? id)
        {
            SalesInquiryModel SalesInquiry;
            int SalesInquiryID;
            SalesInquiryID = (int)id;
            SalesInquiry = GetSalesInquiry(SalesInquiryID, "edit");
            if (!SalesInquiry.IsDraft)
            {
                return RedirectToAction("Index");
            }
            SalesInquiry.UnitList = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "", Value = "0" } }, "Value", "Text");
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            var CurrencyID = 0;
            if (currency != null)
            {
                CurrencyID = currency.CurrencyID;
                SalesInquiry.DecimalPlaces = currency.DecimalPlaces;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(CurrencyID);
            if (classdata != null)
            {
                SalesInquiry.normalclass = classdata.normalclass;
            }
            return View(SalesInquiry);

        }

        // Gets offer and discount details on selecting an item 
        [HttpPost]
        public JsonResult GetDiscountAndOfferDetails(int CustomerID, int SchemeID, int ItemID, decimal Qty, int UnitID)
        {
            DiscountAndOfferBO discountAndOffer = salesInquiryBL.GetDiscountAndOfferDetails(CustomerID, SchemeID, ItemID, Qty, UnitID);
            return Json(new { Status = "success", discountAndOffer = discountAndOffer }, JsonRequestBehavior.AllowGet);
        }

        // Gets offer details for multiple items 
        [HttpPost]
        public JsonResult GetOfferDetails(int CustomerID, int SchemeID, int[] ItemID, int[] UnitID)
        {
            List<DiscountAndOfferBO> discountAndOffer = salesInquiryBL.GetOfferDetails(CustomerID, SchemeID, ItemID, UnitID);
            return Json(new { Status = "success", discountAndOffer = discountAndOffer }, JsonRequestBehavior.AllowGet);
        }

        // Gets offer and discount details on selecting an item 
        [HttpPost]
        public JsonResult GetSchemeAllocation(int CustomerID)
        {
            int SchemeID = salesInquiryBL.GetSchemeAllocation(CustomerID);
            return Json(new { Status = "success", SchemeID = SchemeID }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckItemCreatedForSalesInquiryItems(int SalesInquiryItemID)
        {
            int ItemID = salesInquiryBL.CheckItemCreatedForSalesInquiryItems(SalesInquiryItemID);
            return Json(new { Status = "success", ItemID = ItemID }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSalesInquiryItemsPurchaseRequisition(int SalesInquiryID)
        {
            try
            {
                SalesInquiryModel SalesInauiry = new SalesInquiryModel();
                SalesInquiryBO SalesOrderBO = salesInquiryBL.GetSalesInquiry(SalesInquiryID);
                SalesInauiry = new SalesInquiryModel()
                {
                    //RequestedDelivaryDate = SalesOrderBO.RequestedDelivaryDate.HasValue ? General.FormatDate(SalesOrderBO.RequestedDelivaryDate.Value, Type) : "",
                    RequestedCustomerName = SalesOrderBO.RequestedCustomerName,
                    RequestedCustomerAddress = SalesOrderBO.RequestedCustomerAddress,
                    PhoneNo1 = SalesOrderBO.PhoneNo1,
                    PhoneNo2 = SalesOrderBO.PhoneNo2,
                    Remarks = SalesOrderBO.Remarks
                };
                SalesInquiryItemModel SalesInauiryItem = new SalesInquiryItemModel();
                var SalesItems = salesInquiryBL.GetSalesInquiryItemsPurchaseRequisition(SalesInquiryID);
                SalesInauiry.Items = new List<SalesInquiryItemModel>();
                foreach (var item in SalesItems)
                {
                    SalesInauiryItem = new SalesInquiryItemModel()
                    {
                        SalesInquiryItemID = item.SalesInquiryItemID,
                        ItemID = item.ItemID,
                        ItemCode = item.Code,
                        ItemName = item.Name,
                        PartsNumber = item.PartsNumber,
                        Remarks = item.Remarks,
                        Model = item.Model,
                        DeliveryTerm = item.DeliveryTerm,
                        UnitName = item.UnitName,
                        UnitID = item.UnitID,
                        Qty = (decimal)item.Qty,
                        Rate = (decimal)item.Rate,
                        GrossAmount = (decimal)item.GrossAmount,
                        VATAmount = item.VATAmount,
                        VATPercentage = item.VATPercentage,
                        NetAmount = (decimal)item.NetAmount
                    };
                    SalesInauiry.Items.Add(SalesInauiryItem);
                }
                return Json(new { Status = "success", SalesInauiry = SalesInauiry, Items = SalesInauiry.Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInquiry", "GetSalesInquiryItems", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetSalesInquiryList(DatatableModel Datatable)
        {
            try
            {
                string SalesInquiryNo = Datatable.Columns[1].Search.Value;
                string SalesInquiryDateHint = Datatable.Columns[2].Search.Value;
                string RequestedCustomerNameHint = Datatable.Columns[3].Search.Value;
                string PhoneNo = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = salesInquiryBL.GetAllSalesInquiryList(Type, SalesInquiryNo, SalesInquiryDateHint, RequestedCustomerNameHint, PhoneNo, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesOrder", "GetSalesOrderList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetCustomerSalesOrderList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string PartyNameHint = Datatable.Columns[3].Search.Value;
                string NetAmountHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = salesInquiryBL.GetCustomerSalesOrderList(TransNoHint, TransDateHint, PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesOrder", "GetUnProcessedSalesOrderList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetInqueryCustomerAutoComplete(string CustomerName)
        {
            var data = salesInquiryBL.GetInqueryCustomerAutoComplete(CustomerName).Select(x => new { x.ID, x.Name }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUnProcessedSalesOrderList(DatatableModel Datatable)
        {
            try
            {
                string SalesInquiryNo = Datatable.Columns[1].Search.Value;
                string SalesInquiryDateHint = Datatable.Columns[2].Search.Value;
                string RequestedCustomerNameHint = Datatable.Columns[3].Search.Value;
                string PhoneNo = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                // string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = salesInquiryBL.GetSalesInquiryList("SalesInquiry", SalesInquiryNo, SalesInquiryDateHint, RequestedCustomerNameHint, PhoneNo, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesOrder", "GetUnProcessedSalesOrderList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSalesOrderItems(int[] SalesOrderID, int StoreID, int CustomerID, int SchemeID)
        {
            try
            {
                int[] ItemIDs;
                int[] UnitIDs;
                var Items = salesInquiryBL.GetBatchwiseSalesOrderItems(SalesOrderID, StoreID, CustomerID, SchemeID).MapToModel();

                ItemIDs = Items.Select(a => a.ItemID).Distinct().ToArray();
                UnitIDs = Items.Select(a => a.UnitID).Distinct().ToArray();
                List<DiscountAndOfferBO> discountAndOffer = salesInquiryBL.GetOfferDetails(CustomerID, SchemeID, ItemIDs, UnitIDs);
                return Json(new { Status = "success", Items = Items, discountAndOffer = discountAndOffer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesOrder", "GetSalesOrderItems", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetGoodsReceiptSalesOrderItems(int[] SalesOrderID)
        {
            try
            {
                var BOItems = salesInquiryBL.GetGoodsReceiptSalesOrderItems(SalesOrderID).ToList();
                List<SalesItemModel> Items = new List<SalesItemModel>();
                SalesItemModel Item;
                foreach (var item in BOItems)
                {
                    Item = new SalesItemModel()
                    {
                        SalesOrderID = item.SalesOrderID,
                        SalesOrderItemTransID = item.SalesOrderItemID,
                        TransNo = item.TransNo,
                        ItemID = item.ItemID,
                        ItemName = item.Name,
                        PartsNumber = item.PartsNumber,
                        PrintWithItemName = item.PrintWithItemCode,
                        SecondaryQty = item.SecondaryQty,
                        SecondaryMRP = item.SecondaryMRP,
                        SecondaryUnit = item.SecondaryUnit,
                        Remarks = item.Remarks,
                        Model = item.Model,
                        Code = item.Code,
                        UnitName = item.Unit,
                        BatchName = item.BatchName,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        Rate = item.Rate,
                        BasicPrice = item.BasicPrice,
                        Qty = item.Qty,
                        OfferQty = item.OfferQty,
                        GrossAmount = item.GrossAmount,
                        DiscountPercentage = item.DiscountPercentage,
                        DiscountAmount = item.DiscountAmount,
                        AdditionalDiscount = item.AdditionalDiscount,
                        TaxableAmount = item.TaxableAmount,
                        GSTPercentage = (decimal)item.GSTPercentage,
                        SGSTPercentage = item.SGSTPercentage,
                        CGSTPercentage = item.CGSTPercentage,
                        IGSTPercentage = item.IGSTPercentage,
                        VATPercentage = item.VATPercentage,
                        VATAmount = item.VATAmount,
                        IsGST = item.IsGST,
                        IsVat = item.IsVat,
                        CurrencyID = item.CurrencyID,
                        CurrencyName = item.CurrencyName,
                        IGST = item.IGST,
                        CGST = item.CGST,
                        SGST = item.SGST,
                        //GSTAmount = item.IGST + item.CGST + item.SGST, alreay set in model
                        NetAmount = item.NetAmount,
                        StoreID = item.StoreID,
                        InvoiceQty = item.Qty,
                        InvoiceOfferQty = item.InvoiceOfferQty,
                        InvoiceQtyMet = item.InvoiceQtyMet,
                        InvoiceOfferQtyMet = item.InvoiceOfferQtyMet,
                        Stock = item.Stock,
                        UnitID = item.UnitID,
                        SalesUnitID = item.SalesUnitID,
                        LooseRate = item.LooseRate,
                        MRP = item.MRP,
                        CessAmount = item.CessAmount,
                        CessPercentage = item.CessPercentage,
                        Category = item.Category,
                        CategoryID = item.ItemCategoryID
                    };
                    Items.Add(Item);
                }

                return Json(new { Status = "success", Items = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesOrder", "GetGoodsReceiptSalesOrderItems", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Approve(int SalesOrderID)
        {
            try
            {
                salesInquiryBL.Approve(SalesOrderID);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesOrder", "Approve", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Suspend(int ID, string Table)
        {

            //var output = salesOrderBL.SuspendSalesOrder(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalesOrderPrintPdf(int Id)
        {
            return null;
        }
        public ActionResult ExportToExcel(int SalesInquiryID)
        {

            var SalesInquiry = this.GetSalesInquiry(SalesInquiryID, "view");
            var csv = new StringBuilder();
            csv.AppendLine("Inquiry No,Inquiry Date,Requested Delivery Date,Requested Expiry Date,Requested Customer Name,Address,Phone No,Second PhoneNo,Item Code,Item Name,Parts Number,Unit,Model/Make,Year,SI/VIN Number,Delivery Term,Rate,Qty,Gross Amount,VAT%,VAT Amount,Net Amount");

            foreach (var inquiry in SalesInquiry.Items)
            {
                csv.AppendLine($"{SalesInquiry.SalesInquiryNo},{SalesInquiry.SalesInquiryDate:yyyy-MM-dd},{SalesInquiry.RequestedDelivaryDate:yyyy-MM-dd},{SalesInquiry.RequestExpiryDate:yyyy-MM-dd},{SalesInquiry.RequestedCustomerName},{SalesInquiry.RequestedCustomerAddress},{SalesInquiry.PhoneNo1},{SalesInquiry.PhoneNo2},{inquiry.ItemCode},{inquiry.ItemName},{inquiry.PartsNumber},{inquiry.UnitName},{inquiry.Model},{inquiry.Year},{inquiry.SIOrVINNumber},{inquiry.DeliveryTerm},{inquiry.Rate},{inquiry.Qty},{inquiry.GrossAmount},{inquiry.VATPercentage},{inquiry.VATAmount},{inquiry.NetAmount}");
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "SalesInquiries.csv");


        }

        public ActionResult ExportToPDF(int SalesInquiryID)
        {

            var salesInquiry = this.GetSalesInquiry(SalesInquiryID, "view");
            // Create HTML content for the PDF
            string htmlContent = "<html><head><style>";
            htmlContent += "body { font-family: 'Times New Roman', serif; font-size: 11px; }"; // Set font style and size for the body
            htmlContent += "h2 { font-size: 14px; font-weight: bold; margin-bottom: 10px; }";   // Set font size for headings
            htmlContent += "h3 { font-size: 12px; margin-top: 10px; margin-bottom: 5px; }";     // Set font size for subheadings
            htmlContent += "table { width: 100%; border-collapse: collapse; margin-top: 10px; }"; // Style for all tables
            htmlContent += "td, th { padding: 5px; vertical-align: top; }"; // Style for table cells and headers without border for the first table
            htmlContent += "table.item-details td, table.item-details th { border: 1px solid #808080; }"; // Add border to table cells and headers in the item details table
            htmlContent += "</style></head><body>";

            // Add the report title
            htmlContent += "<h2>Sales Inquiry Report</h2>";

            // Table for main fields, without borders
            htmlContent += "<table cellpadding='5' cellspacing='0' style='font-size: 11px;'>";

            // First row section
            htmlContent += "<tr><td><b>Inquiry No:</b></td><td>" + salesInquiry.SalesInquiryNo + "</td>";
            htmlContent += "<td><b>Date:</b></td><td>" + salesInquiry.SalesInquiryDate + "</td>";
            htmlContent += "<td><b>Delivery Date:</b></td><td>" + salesInquiry.RequestedDelivaryDate + "</td>";
            htmlContent += "<td><b>Expiry Date:</b></td><td>" + salesInquiry.RequestExpiryDate + "</td></tr>";

            // Second row section
            htmlContent += "<tr><td><b>Customer Name:</b></td><td>" + salesInquiry.RequestedCustomerName + "</td>";
            htmlContent += "<td><b>Phone No 1:</b></td><td>" + salesInquiry.PhoneNo1 + "</td>";
            htmlContent += "<td><b>Phone No 2:</b></td><td>" + salesInquiry.PhoneNo2 + "</td>";
            htmlContent += "<td><b>Address:</b></td><td>" + salesInquiry.RequestedCustomerAddress + "</td></tr>";
            htmlContent += "</table>";

            // List fields section for item details
            htmlContent += "<h3>Item Details</h3>";
            htmlContent += "<table class='item-details' cellpadding='5' cellspacing='0' style='border-collapse: collapse; font-size: 10px;'><thead>";
            htmlContent += "<tr><th>Item Code</th><th>Item Name</th><th>Parts Number</th><th>Unit</th><th>Model/Make</th><th>Year</th><th>SI/VIN Number</th><th>Rate</th><th>Qty</th><th>Net Amount</th></tr></thead><tbody>";

            // Add each sales inquiry item to the HTML table
            foreach (var inquiry in salesInquiry.Items)
            {
                htmlContent += $"<tr><td>{inquiry.ItemCode}</td><td>{inquiry.ItemName}</td><td>{inquiry.PartsNumber}</td><td>{inquiry.UnitName}</td><td>{inquiry.Model}</td><td>{inquiry.Year}</td><td>{inquiry.SIOrVINNumber}</td><td>{inquiry.Rate}</td><td>{inquiry.Qty}</td><td>{inquiry.NetAmount}</td></tr>";
            }

            htmlContent += "</tbody></table></body></html>";

            // Convert the HTML content to a PDF
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                using (var stringReader = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                }

                document.Close();

                // Return the generated PDF file
                return File(memoryStream.ToArray(), "application/pdf", "SalesInquiries.pdf");
            }
        }
    }
}