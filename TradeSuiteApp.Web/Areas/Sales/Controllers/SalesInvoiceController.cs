using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class SalesInvoiceController : Controller
    {
        private IProformaInvoice proformaInvoiceBL;
        private ISalesOrder salesOrderBL;
        private ICustomerContract customerBL;
        private ICategoryContract categroyBL;
        private IAddressContract addressBL;
        private IWareHouseContract warehouseBL;
        private IGeneralContract generalBL;
        private ISalesInvoice salesInvoiceBL;
        private IPaymentModeContract paymentModeBL;
        private ILocationContract locationBL;
        private ICounterSalesContract counterSalesBL;
        private IGSTCategoryContract gstCategoryBL;

        public SalesInvoiceController()
        {
            proformaInvoiceBL = new ProformaInvoiceBL();
            salesOrderBL = new SalesOrderBL();
            customerBL = new CustomerBL();
            categroyBL = new CategoryBL();
            addressBL = new AddressBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            salesInvoiceBL = new SalesInvoiceBL();
            paymentModeBL = new PaymentModeBL();
            locationBL = new LocationBL();
            counterSalesBL = new CounterSalesBL();
            gstCategoryBL = new GSTCategoryBL();
        }
        // GET: Sales/SalesInvoice
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }
        // GET: Sales/SalesInvoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            SalesInvoiceModel salesInvoice = new SalesInvoiceModel();

            int SalesInvoiceID = (int)id;
            salesInvoice = salesInvoiceBL.GetSalesInvoice(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.Items = salesInvoiceBL.GetSalesInvoiceItems(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.AmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.PackingDetails = salesInvoiceBL.GetSalesInvoicePackingDetails(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            //salesInvoice.DespatchDetails = salesInvoiceBL.GetSalesInvoiceDespatchDetails(SalesInvoiceID).MapToSalesModel();
            salesInvoice.IsCancelable = salesInvoiceBL.IsCancelable(SalesInvoiceID);
            salesInvoice.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            ViewBag.PrintOptions = generalBL.GetGroupConfig("PrintSettings", GeneralBO.CreatedUserID);
            try
            {
                ViewBag.PrintPreferenceSaved = generalBL.GetConfig("PrintPreferenceSaved");
            }
            catch (Exception e)
            {
                ViewBag.PrintPreferenceSaved = 0;
            }

            return View(salesInvoice);
        }

        public ActionResult Cancel(int SalesInvoiceID = 0)
        {
            try
            {
                int result = salesInvoiceBL.Cancel(SalesInvoiceID);
                return Json(new { Status = "success", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesInvoice", "Cancel", SalesInvoiceID, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/SalesInvoice/Create
        public ActionResult Create()
        {
            SalesInvoiceModel salesInvoice = new SalesInvoiceModel();
            salesInvoice.InvoiceDate = General.FormatDate(DateTime.Now);
            salesInvoice.InvoiceNo = generalBL.GetSerialNo("SalesInvoice", "Code");
            salesInvoice.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesInvoice.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", salesInvoice.PaymentModeID);
            salesInvoice.Items = new List<SalesInvoiceItemModel>();
            salesInvoice.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            salesInvoice.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(0), "ID", "Name");
            salesInvoice.StoreList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            salesInvoice.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            salesInvoice.CheckStock = true;
            var address = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            salesInvoice.LocationStateID = address != null ? address.StateID : 0;
            salesInvoice.CashDiscountPercentage = 0;
            salesInvoice.CashDiscountEnabled = false;
            salesInvoice.AmountDetails = new List<SalesAmount>();
            salesInvoice.PackingDetails = new List<PackingDetailsModel>();
            salesInvoice.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", 0, ""), "AddressID", "Place");
            salesInvoice.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", 0, ""), "AddressID", "Place");
            salesInvoice.FreightTax = salesInvoiceBL.GetFreightTaxForEcommerceCustomer();

            ViewBag.Statuses = new List<string>() { "Zero-Quantity", "Insufficient-Order-Quantity", "Insufficient-Offer-Quantity" };
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                salesInvoice.CurrencyID = currency.CurrencyID;
                salesInvoice.CurrencyName = currency.CurrencyName;
                salesInvoice.CountryName = currency.CountryName;
                salesInvoice.CountryID = currency.CountryID;
                salesInvoice.IsVat = currency.IsVat;
                salesInvoice.IsGST = currency.IsGST;
                salesInvoice.TaxType = currency.TaxType;
                salesInvoice.TaxTypeID = currency.TaxTypeID;
                salesInvoice.CurrencyCode = currency.CurrencyCode;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                salesInvoice.normalclass = classdata.normalclass;
                salesInvoice.DecimalPlaces = classdata.DecimalPlaces;
            }
            salesInvoice.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            salesInvoice.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            salesInvoice.VATPercentageID = Convert.ToInt16(generalBL.GetConfig("VATPercentageID"));
            return View(salesInvoice);

        }

        // POST: Sales/SalesInvoice/Create
        [HttpPost]
        public ActionResult Save(SalesInvoiceModel model)
        {
            var result = new List<object>();
            try
            {
                SalesInvoiceBO Invoice = model.MapToBO();
                List<SalesItemBO> Items = model.Items.MapToBO();
                List<SalesAmountBO> AmountDetails = model.AmountDetails.MapToBO();
                List<SalesPackingDetailsBO> PackingDetails = model.PackingDetails.MapToBO();
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    SalesInvoiceBO Temp = salesInvoiceBL.GetSalesInvoice(model.ID, GeneralBO.LocationID);
                    if (!Temp.IsDraft || Temp.IsCancelled || Temp.IsProcessed)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var ID = 0;
                if ((ID = salesInvoiceBL.Save(Invoice, Items, AmountDetails, PackingDetails)) != 0)
                {
                    return Json(new { Status = "success", ID = ID, Message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", Message = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "SalesInvoice", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/SalesInvoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            SalesInvoiceModel salesInvoice = new SalesInvoiceModel();

            int SalesInvoiceID = (int)id;
            salesInvoice = salesInvoiceBL.GetSalesInvoice(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            if (!salesInvoice.IsDraft || salesInvoice.IsCanceled || salesInvoice.IsProcessed)
            {
                return RedirectToAction("Index");
            }

            salesInvoice.Items = salesInvoiceBL.GetSalesInvoiceItems(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();

            salesInvoice.AmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.PackingDetails = salesInvoiceBL.GetSalesInvoicePackingDetails(SalesInvoiceID, GeneralBO.LocationID).MapToSalesModel();
            salesInvoice.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesInvoice.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            salesInvoice.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            salesInvoice.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(0), "ID", "Name");
            salesInvoice.StoreList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            salesInvoice.StoreID = salesInvoice.Items[0].StoreID;
            salesInvoice.CheckStock = true;
            salesInvoice.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            salesInvoice.CashDiscountEnabled = !customerBL.HasOutstandingAmount(salesInvoice.CustomerID);
            salesInvoice.TurnoverDiscountAvailable = customerBL.GetTurnOverDiscount(salesInvoice.CustomerID);
            salesInvoice.FreightTax = salesInvoiceBL.GetFreightTaxForEcommerceCustomer();
            salesInvoice.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", salesInvoice.CustomerID, ""), "AddressID", "AddressLine1");
            salesInvoice.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", salesInvoice.CustomerID, ""), "AddressID", "AddressLine1");

            ViewBag.Statuses = new List<string>() { "Zero-Quantity", "Insufficient-Order-Quantity", "Insufficient-Offer-Quantity" };
            if (salesInvoice.Items != null && salesInvoice.Items.Count > 0)
            {
                var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(salesInvoice.Items.First().CurrencyID);
                if (classdata != null)
                {
                    salesInvoice.normalclass = classdata.normalclass;
                    salesInvoice.DecimalPlaces = classdata.DecimalPlaces;
                }
            }
            salesInvoice.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            salesInvoice.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            return View(salesInvoice);
        }

        public JsonResult GetSalesInvoiceItems(int ID)
        {

            List<SalesInvoiceItemModel> Items = salesInvoiceBL.GetSalesInvoiceItems(ID, GeneralBO.LocationID).MapToSalesModel();
            Items = Items.Select(item =>
            {
                if (item.InvoiceQty >= item.Stock)
                {
                    item.InvoiceQtyMet = true;
                    item.InvoiceOfferQtyMet = true;
                }
                else if (item.InvoiceQty - item.InvoiceOfferQty >= item.Stock)
                {
                    item.InvoiceQtyMet = true;
                    item.InvoiceOfferQtyMet = false;
                }
                else if (item.InvoiceQty < item.Stock)
                {
                    item.InvoiceQtyMet = false;
                    item.InvoiceOfferQtyMet = false;
                }
                return item;
            }).ToList();
            return Json(new { Status = "success", Items = Items }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGoodsReceiptSalesInvoiceItems(int[] SalesInvoiceID)
        {
            var BOItems = salesInvoiceBL.GetGoodsReceiptSalesInvoiceItems(SalesInvoiceID, GeneralBO.LocationID).ToList();
            List<SalesItemModel> Items = new List<SalesItemModel>();
            SalesItemModel Item;
            foreach (var item in BOItems)
            {
                Item = new SalesItemModel()
                {
                    SalesInvoiceID = item.SalesInvoiceID,
                    SalesInvoiceTransID = item.SalesInvoiceTransID,
                    TransNo = item.TransNo,
                    ItemID = item.ItemID,
                    ItemName = item.Name,
                    PartsNumber = item.PartsNumber,
                    PrintWithItemName = item.PrintWithItemCode,
                    Remarks = item.Remarks,
                    Model = item.Model,
                    Code = item.Code,
                    SecondaryQty = item.SecondaryQty,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryUnit = item.SecondaryUnit,
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
                    //GSTAmount = item.IGST + item.CGST + item.SGST, already set in model
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

        [HttpPost]
        public JsonResult GetCustomerSalesInvoiceList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string NetAmountHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));
                
                DatatableResultBO resultBO = salesInvoiceBL.GetCustomerSalesInvoiceList(CustomerID,TransNoHint, TransDateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetSalesInvoiceList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSalesInvoiceList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string SalesTypeHint = Datatable.Columns[3].Search.Value;
                string CustomerNameHint = Datatable.Columns[4].Search.Value;
                string LocationHint = Datatable.Columns[5].Search.Value;
                string NetAmountHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = salesInvoiceBL.GetSalesInvoiceList(Type, CodeHint, DateHint, SalesTypeHint, CustomerNameHint, LocationHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetSalesInvoiceList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetInvoiceListForSalesReturn(DatatableModel Datatable)
        {
            try
            {
                string TransHint = Datatable.Columns[2].Search.Value;
                string DateHint = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));
                DatatableResultBO resultBO = salesInvoiceBL.GetInvoiceListForSalesReturn(CustomerID, TransHint, DateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetInvoiceListForSalesReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalesInvoiceNoAutoComplete(int CustomerID, string InvoiceNo)
        {
            try
            {
                DatatableResultBO resultBO = salesInvoiceBL.GetInvoiceListForSalesReturn(CustomerID, InvoiceNo, "", "TransNo", "ASC", 0, 20);
                var result = new { Status = "success", data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesInvoice", "GetSalesInvoiceNoAutoComplete", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalesInvoices(int[] SalesInvoiceIDS, int PriceListID)
        {
            try
            {
                SalesInvoiceBO SalesInvoice = new SalesInvoiceBO();
                SalesInvoice.Items = new List<SalesInvoiceItemBO>();
                if (SalesInvoiceIDS.Length > 0)
                {
                    foreach (var SalesInvoiceID in SalesInvoiceIDS)
                    {
                        var list = salesInvoiceBL.GetInvoiceTransList(SalesInvoiceID, PriceListID);

                        if (list != null)
                        {
                            SalesInvoice.Items.AddRange(list);
                        }
                    }
                }
                return Json(SalesInvoice.Items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesInvoice", "GetSalesInvoices", 0, e);
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDiscountDetails(int CustomerID)
        {
            try
            {
                bool CashDiscountEnabled = !customerBL.HasOutstandingAmount(CustomerID);
                decimal TurnoverDiscountAvailable = customerBL.GetTurnOverDiscount(CustomerID);
                return Json(new { Status = "success", CashDiscountEnabled = CashDiscountEnabled, TurnoverDiscountAvailable = TurnoverDiscountAvailable }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesInvoice", "GetDiscountDetails", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDiscountDetailsCustom(int CustomerID)
        {
            try
            {
                bool CashDiscountEnabled = !customerBL.HasOutstandingAmount(CustomerID);
                decimal TurnoverDiscountAvailable = customerBL.GetTurnOverDiscount(CustomerID);
                decimal CashDiscountPercentage = CashDiscountEnabled ? customerBL.CashDiscountPercentage(CustomerID) : 0;
                return Json(new { Status = "success", CashDiscountPercentage = CashDiscountPercentage, CashDiscountEnabled = CashDiscountEnabled, TurnoverDiscountAvailable = TurnoverDiscountAvailable }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesInvoice", "GetDiscountDetailsCustom", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetIntercompanySalesInvoiceList(int SupplierID, int LocationID)
        {
            List<SalesInvoiceModel> salesinvoice = new List<SalesInvoiceModel>();

            salesinvoice = salesInvoiceBL.GetIntercompanySalesInvoiceList(SupplierID, LocationID).Select(m => new SalesInvoiceModel()
            {
                InvoiceNo = m.InvoiceNo,
                InvoiceDate = General.FormatDate(m.InvoiceDate, "view"),
                NetAmount = m.NetAmount,
                ID = m.ID
            }).ToList();

            return Json(salesinvoice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCreditAmountByCustomer(int CustomerID)
        {

            var creditAmount = salesInvoiceBL.GetCreditAmountByCustomer(CustomerID);
            return Json(new { Status = "success", Data = creditAmount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIntercompanySalesInvoice(int invoiceid, int LocationID)

        {
            try
            {
                SalesInvoiceModel salesInvoice = new SalesInvoiceModel();
                salesInvoice.Items = new List<SalesInvoiceItemModel>();
                salesInvoice = salesInvoiceBL.GetSalesInvoice(invoiceid, LocationID).MapToSalesModel();
                salesInvoice.Items = salesInvoiceBL.GetSalesInvoiceItems(invoiceid, LocationID).MapToSalesModel();
                salesInvoice.AmountDetails = salesInvoiceBL.GetSalesInvoiceAmountDetails(invoiceid, LocationID).MapToSalesModel();
                return Json(salesInvoice, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesInvoice", "GetIntercompanySalesInvoice", 0, e);
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(SalesInvoiceModel model)
        {
            return Save(model);
        }

        public JsonResult GetSalesInvoiceCodeAutoCompleteForReport(string CodeHint, string FromDate, string ToDate)
        {
            List<SalesInvoiceModel> InvoiceCode = new List<SalesInvoiceModel>();
            InvoiceCode = salesInvoiceBL.GetSalesInvoiceCodeAutoCompleteForReport(CodeHint, General.ToDateTime(FromDate), General.ToDateTime(ToDate)).Select(a => new SalesInvoiceModel()
            {
                ID = a.ID,
                InvoiceNo = a.InvoiceNo,
            }).ToList();

            return Json(InvoiceCode, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + salesInvoiceBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalesInvoicePrintPdf(int Id)
        {
            return null;
        }
        public JsonResult GetSalesInvoiceHistory(DatatableModel Datatable)
        {
            try
            {
                string SalesOrderNos = Datatable.Columns[1].Search.Value;
                string InvoiceDate = Datatable.Columns[2].Search.Value != null ? Datatable.Columns[2].Search.Value.Replace("-", " ") : null;
                string CustomerName = Datatable.Columns[3].Search.Value;
                string ItemName = Datatable.Columns[5].Search.Value;
                string PartsNumber = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemID = Convert.ToInt32(Datatable.GetValueFromKey("ItemID", Datatable.Params));
                DatatableResultBO resultBO = salesInvoiceBL.GetSalesInvoiceHistory(ItemID, SalesOrderNos, InvoiceDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetInvoiceListForSalesReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCounterSalesHistory(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value != null ? Datatable.Columns[2].Search.Value.Replace("-", " ") : null;
                string CustomerName = Datatable.Columns[3].Search.Value;
                string ItemName = Datatable.Columns[5].Search.Value;
                string PartsNumber = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemID = Convert.ToInt32(Datatable.GetValueFromKey("ItemID", Datatable.Params));
                DatatableResultBO resultBO = salesInvoiceBL.GetCounterSalesHistory(ItemID, TransNo, TransDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetInvoiceListForSalesReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPurchaseHistory(DatatableModel Datatable)
        {
            try
            {

                string PurchaseOrderNo = Datatable.Columns[1].Search.Value;
                string PurchaseOrderDate = Datatable.Columns[2].Search.Value != null ? Datatable.Columns[2].Search.Value.Replace("-", " ") : null;
                string SupplierName = Datatable.Columns[3].Search.Value;
                string ItemName = Datatable.Columns[5].Search.Value;
                string PartsNumber = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemID = Convert.ToInt32(Datatable.GetValueFromKey("ItemID", Datatable.Params));
                DatatableResultBO resultBO = salesInvoiceBL.GetPurchaseHistory(ItemID, PurchaseOrderNo, PurchaseOrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetInvoiceListForSalesReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPendingPOHistory(DatatableModel Datatable)
        {
            try
            {

                string PurchaseOrderNo = Datatable.Columns[1].Search.Value;
                string PurchaseOrderDate = Datatable.Columns[2].Search.Value != null ? Datatable.Columns[2].Search.Value.Replace("-", " ") : null;
                string SupplierName = Datatable.Columns[3].Search.Value;
                string ItemName = Datatable.Columns[5].Search.Value;
                string PartsNumber = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemID = Convert.ToInt32(Datatable.GetValueFromKey("ItemID", Datatable.Params));
                DatatableResultBO resultBO = salesInvoiceBL.GetPendingPOHistory(ItemID, PurchaseOrderNo, PurchaseOrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetInvoiceListForSalesReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLegacyPurchaseHistory(DatatableModel Datatable)
        {
            try
            {
                string ReferenceNo = Datatable.Columns[1].Search.Value;
                string ItemCode = Datatable.Columns[2].Search.Value;
                string ItemName = Datatable.Columns[3].Search.Value;
                string SupplierName = Datatable.Columns[4].Search.Value;
                string PartsNumber = Datatable.Columns[5].Search.Value;
                string OrderDate = Datatable.Columns[6].Search.Value != null ? Datatable.Columns[6].Search.Value.Replace("-", " ") : null;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ItemID = Convert.ToInt32(Datatable.GetValueFromKey("ItemID", Datatable.Params));

                DatatableResultBO resultBO = salesInvoiceBL.GetLegacyPurchaseHistory(ItemID, ReferenceNo, OrderDate, SupplierName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "SalesInvoice", "GetInvoiceListForSalesReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SalesInvoiceWithCode(int Id)
        {
            return null;
        }
        public JsonResult SalesInvoicePartNo(int Id)
        {
            return null;
        }
        public JsonResult SalesInvoiceExportitemcode(int Id)
        {
            return null;
        }
        public JsonResult SalesInvoiceExportpartno(int Id)
        {
            return null;
        }
    }
}
