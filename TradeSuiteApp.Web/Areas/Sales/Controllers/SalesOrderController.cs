using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using DataAccessLayer;
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

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{

    public class SalesOrderController : Controller
    {
        private ISalesOrder salesOrderBL;
        private ICustomerContract customerBL;
        private ICategoryContract categroyBL;
        private IAddressContract addressBL;
        private IGeneralContract generalBL;
        private IUnitContract unitBL;
        private ILocationContract locationBL;
        private ICounterSalesContract counterSalesBL;
        private IGSTCategoryContract gstCategoryBL;
        public SalesOrderController()
        {
            salesOrderBL = new SalesOrderBL();
            customerBL = new CustomerBL();
            categroyBL = new CategoryBL();
            addressBL = new AddressBL();
            generalBL = new GeneralBL();
            unitBL = new UnitBL();
            locationBL = new LocationBL();
            counterSalesBL = new CounterSalesBL();
            gstCategoryBL = new GSTCategoryBL();
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
        public JsonResult ReadExcel(string Path)
        {
            try
            {
                List<UploadOrderBO> OrderLines = salesOrderBL.ReadExcel(Path);
                return Json(new { Status = "success", Data = OrderLines }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesOrder", "ReadExcel", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProcessUploadedOrders(List<SalesOrderModel> SalesOrders)
        {
            try
            {
                SalesOrderBO SalesOrder;
                List<SalesItemBO> Items;
                foreach (SalesOrderModel Order in SalesOrders)
                {
                    SalesOrder = new SalesOrderBO()
                    {
                        CustomerID = Order.CustomerID,
                        Source = "Upload",
                    };
                    Items = Order.Items.Select(a => new SalesItemBO()
                    {
                        ItemID = a.ItemID,
                        Name = a.ItemName,
                        Qty = a.Qty,
                        SGSTPercentage = a.SGSTPercentage,
                        CGSTPercentage = a.CGSTPercentage,
                        IGSTPercentage = a.IGSTPercentage,
                        ItemCategoryID = a.ItemCategoryID,
                        UnitID = a.UnitID,
                        CessPercentage = a.CessPercentage
                    }).ToList();

                    salesOrderBL.ProcessOrder(SalesOrder, Items);
                }

                return Json(new { Status = "success", Message = "Saved " + SalesOrders.Count() + " order(s) as draft" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesOrder", "ProcessUploadedOrders", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private SalesOrderModel GetSalesOrder(int SalesOrderID, string Type)
        {

            SalesOrderModel SalesOrder;
            SalesItemModel SalesItem;
            List<SalesItemBO> SalesItems;

            SalesOrderBO SalesOrderBO = salesOrderBL.GetSalesOrder(SalesOrderID);
            CustomerBO CustomerBO = customerBL.GetCustomerDetails(SalesOrderBO.CustomerID);
            SalesOrder = new SalesOrderModel()
            {
                ID = SalesOrderBO.ID,
                SONo = SalesOrderBO.SONo,
                SODate = General.FormatDate(SalesOrderBO.SODate, Type),
                CustomerEnquiryNumber = SalesOrderBO.CustomerEnquiryNumber,
                QuotationExpiry = SalesOrderBO.QuotationExpiry.HasValue ? General.FormatDate(SalesOrderBO.QuotationExpiry.Value) : "",
                EnquiryDate = SalesOrderBO.EnquiryDate.HasValue ? General.FormatDate(SalesOrderBO.EnquiryDate.Value) : "",
                Remarks = SalesOrderBO.Remarks,
                PaymentTerms = SalesOrderBO.PaymentTerms,
                CustomerID = SalesOrderBO.CustomerID,
                PriceListID = SalesOrderBO.PriceListID,
                StateID = SalesOrderBO.StateID,
                IsGSTRegistered = SalesOrderBO.IsGSTRegistered,
                CustomerName = SalesOrderBO.CustomerName.Trim(),
                DespatchDate = General.FormatDate(SalesOrderBO.DespatchDate, Type),
                CustomerCategoryID = SalesOrderBO.CustomerCategoryID,
                CustomerCategory = SalesOrderBO.CustomerCategory,
                FreightAmount = SalesOrderBO.FreightAmount,
                GrossAmount = SalesOrderBO.GrossAmount,
                DiscountPercentage = SalesOrderBO.DiscountPercentage,
                DiscountAmount = SalesOrderBO.DiscountAmount,
                TaxableAmount = SalesOrderBO.TaxableAmount,
                SGSTAmount = SalesOrderBO.SGSTAmount,
                CGSTAmount = SalesOrderBO.CGSTAmount,
                IGSTAmount = SalesOrderBO.IGSTAmount,
                CessAmount = SalesOrderBO.CessAmount,
                VATAmount = SalesOrderBO.VATAmount,
                CurrencyExchangeRate = SalesOrderBO.CurrencyExchangeRate,
                CurrencyCode = SalesOrderBO.CurrencyCode,
                IsGST = SalesOrderBO.IsGST,
                IsVat = SalesOrderBO.IsVat,
                CurrencyID = SalesOrderBO.CurrencyID,
                CurrencyName = SalesOrderBO.Currencyname,
                RoundOff = SalesOrderBO.RoundOff,
                PrintWithItemCode = SalesOrderBO.PrintWithItemCode,
                NetAmount = SalesOrderBO.NetAmount,
                Status = SalesOrderBO.IsCancelled ? "cancelled" : SalesOrderBO.IsProcessed ? "processed" : SalesOrderBO.IsDraft ? "draft" : "",
                Source = SalesOrderBO.Source,
                PurchaseOrderID = SalesOrderBO.PurchaseOrderID,
                FsoID = SalesOrderBO.FsoID,
                SchemeID = SalesOrderBO.SchemeAllocationID,
                BillingAddressID = SalesOrderBO.BillingAddressID,
                ShippingAddressID = SalesOrderBO.ShippingAddressID,
                BillingAddress = SalesOrderBO.BillingAddress,
                ShippingAddress = SalesOrderBO.ShippingAddress,
                IsCancelled = SalesOrderBO.IsCancelled,
                IsApproved = SalesOrderBO.IsApproved,
                VATPercentageID = SalesOrderBO.VATPercentageID,
                VATPercentage = SalesOrderBO.VATPercentage
            };
            SalesOrder.CustomerDetails = new CustomersModel()
            {
                Name = CustomerBO.Name,
                CustomerCategoryName = CustomerBO.CustomerCategory,
                CustomerAccountsCategoryName = CustomerBO.CustomerAccountsCategoryName,
                GstNo = CustomerBO.GstNo,
                Color = CustomerBO.Color,
                OutstandingAmount = CustomerBO.OutstandingAmount,
                CustomerCode = CustomerBO.Code
            };
            SalesOrder.IsCancelable = salesOrderBL.IsCancelable(SalesOrderBO.ID);
            SalesItems = salesOrderBL.GetSalesOrderItems(SalesOrderID);
            SalesOrder.Items = new List<SalesItemModel>();
            foreach (var item in SalesItems)
            {
                SalesItem = new SalesItemModel()
                {
                    SalesOrderItemID = item.SalesOrderItemID,
                    ItemID = item.ItemID,
                    UnitID = item.UnitID,
                    UnitName = item.Unit,
                    BatchTypeID = item.BatchTypeID,
                    ItemCode = item.Code,
                    ItemName = item.Name,
                    ItemCategoryID = item.ItemCategoryID,
                    FullOrLoose = item.FullOrLoose,
                    Qty = item.Qty,
                    OfferQty = item.OfferQty,
                    MRP = item.MRP,
                    BasicPrice = item.BasicPrice,
                    GrossAmount = item.GrossAmount,
                    PartsNumber = item.PartsNumber,
                    Model = item.Model,
                    DeliveryTerm = item.DeliveryTerm,
                    CurrencyName = item.CurrencyName,
                    CurrencyID = item.CurrencyID,
                    ExchangeRate = item.ExchangeRate,
                    DiscountPercentage = item.DiscountPercentage,
                    DiscountAmount = item.DiscountAmount,
                    AdditionalDiscount = item.AdditionalDiscount,
                    TaxableAmount = item.TaxableAmount,
                    SGST = item.SGST,
                    CGST = item.CGST,
                    IGST = item.IGST,
                    IsGST = item.IsGST,
                    IsVat = item.IsVat,
                    SGSTPercentage = item.SGSTPercentage,
                    CGSTPercentage = item.CGSTPercentage,
                    IGSTPercentage = item.IGSTPercentage,
                    GSTPercentage = item.IGSTPercentage,
                    CessPercentage = item.CessPercentage,
                    CessAmount = item.CessAmount,
                    VATAmount = item.VATAmount,
                    SecondaryUnitList = SecondaryUnitList(item.Unit, item.SecondaryUnits),
                    VATPercentage = item.VATPercentage,
                    SecondaryMRP = item.SecondaryMRP,
                    SecondaryOfferQty = item.SecondaryOfferQty,
                    SecondaryQty = item.SecondaryQty,
                    SecondaryUnit = item.SecondaryUnit,
                    SecondaryUnitSize = item.SecondaryUnitSize,
                    NetAmount = item.NetAmount,
                    Category = item.Category,
                    BatchType = item.BatchTypeName,
                    MinSalesQty = item.MinSalesQty,
                    MaxSalesQty = item.MaxSalesQty
                };
                SalesOrder.Items.Add(SalesItem);
            }
            return SalesOrder;
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
            SalesOrderBO salesorderBO = new SalesOrderBO();
            SalesOrderModel SalesOrder;
            int SalesOrderID;

            SalesOrderID = (int)id;
            SalesOrder = this.GetSalesOrder(SalesOrderID, "view");
            SalesOrder.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(SalesOrder.CurrencyID);
            if (classdata != null)
            {
                SalesOrder.normalclass = classdata.normalclass;
            }
            return View(SalesOrder);
        }
        public JsonResult GetSalesOrderHistory(DatatableModel Datatable)
        {
            try
            {
                string SalesOrderNo = Datatable.Columns[1].Search.Value;
                string OrderDate = Datatable.Columns[2].Search.Value != null ? Datatable.Columns[2].Search.Value.Replace("-", " ") : null;
                string CustomerName = Datatable.Columns[3].Search.Value;
                string ItemName = Datatable.Columns[5].Search.Value;
                string PartsNumber = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemID = Convert.ToInt32(Datatable.GetValueFromKey("ItemID", Datatable.Params));
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                DatatableResultBO resultBO = salesOrderBL.GetSalesOrderHistory(Type, ItemID, SalesOrderNo, OrderDate, CustomerName, ItemName, PartsNumber, SortField, SortOrder, Offset, Limit);
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
        public ActionResult Cancel(int SalesOrderID)
        {
            try
            {
                if (salesOrderBL.IsCancelable(SalesOrderID))
                {
                    salesOrderBL.Cancel(SalesOrderID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "fail" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesOrder", "Cancel", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/SalesOrder/Create
        public ActionResult Create()
        {
            List<CategoryBO> ItemCategoryList = categroyBL.GetItemCategoryForSales();
            int DefaultCategoryID = ItemCategoryList.FirstOrDefault().ID;

            SalesOrderModel salesOrder = new SalesOrderModel();

            salesOrder.SONo = generalBL.GetSerialNo("SalesOrder", "Code");
            salesOrder.SODate = General.FormatDate(DateTime.Now);
            salesOrder.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesOrder.ItemCategoryList = new SelectList(ItemCategoryList, "ID", "Name");
            salesOrder.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(DefaultCategoryID), "ID", "Name");
            salesOrder.Items = new List<SalesItemModel>();
            salesOrder.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault() != null ? addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID : 0;
            salesOrder.DespatchDate = General.FormatDate(DateTime.Now);
            salesOrder.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", 0, ""), "AddressID", "Location");
            salesOrder.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", 0, ""), "AddressID", "Location");
            salesOrder.UnitList = new SelectList(
                                         new List<SelectListItem>
                                         {
                                                new SelectListItem { Text = "", Value = "0"}

                                         }, "Value", "Text");
            salesOrder.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            salesOrder.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                salesOrder.CurrencyID = currency.CurrencyID;
                salesOrder.CurrencyName = currency.CurrencyName;
                salesOrder.CountryName = currency.CountryName;
                salesOrder.CountryID = currency.CountryID;
                salesOrder.CurrencyCode = currency.CurrencyCode;
                salesOrder.DecimalPlaces = currency.DecimalPlaces;
                salesOrder.IsVat = currency.IsVat;
                salesOrder.IsGST = currency.IsGST;
                salesOrder.TaxType = currency.TaxType;
                salesOrder.TaxTypeID = currency.TaxTypeID;

            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                salesOrder.normalclass = classdata.normalclass;
            }
            salesOrder.PrintWithItemCode = true;
            salesOrder.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            salesOrder.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            salesOrder.VATPercentageID = Convert.ToInt16(generalBL.GetConfig("VATPercentageID"));
            return View(salesOrder);
        }

        [HttpPost]
        public ActionResult SaveAsDraft(SalesOrderModel SalesOrder)
        {
            return Save(SalesOrder);
        }

        // POST: Sales/SalesOrder/Create
        [HttpPost]
        public ActionResult Save(SalesOrderModel SalesOrder)
        {
            var result = new List<object>();

            if (SalesOrder.ID != 0)
            {
                //Edit
                //Check whether editable or not
                SalesOrderBO Temp = salesOrderBL.GetSalesOrder(SalesOrder.ID);
                if (!Temp.IsDraft)
                {
                    result.Add(new { ErrorMessage = "Not Editable" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {

                SalesOrderBO SalesOrderBO = new SalesOrderBO()
                {
                    ID = SalesOrder.ID,
                    SONo = SalesOrder.SONo,
                    SODate = General.ToDateTime(SalesOrder.SODate),
                    CustomerCategoryID = SalesOrder.CustomerCategoryID,
                    ItemCategoryID = SalesOrder.ItemCategoryID,
                    CustomerID = SalesOrder.CustomerID,
                    SchemeAllocationID = SalesOrder.SchemeAllocationID,
                    DespatchDate = General.ToDateTime(SalesOrder.DespatchDate),
                    PrintWithItemCode = SalesOrder.PrintWithItemCode,
                    GrossAmount = SalesOrder.GrossAmount,
                    DiscountPercentage = SalesOrder.DiscountPercentage,
                    DiscountAmount = SalesOrder.DiscountAmount,
                    FreightAmount = SalesOrder.FreightAmount,
                    TaxableAmount = SalesOrder.TaxableAmount,
                    CurrencyID = SalesOrder.CurrencyID,
                    SGSTAmount = SalesOrder.SGSTAmount,
                    CGSTAmount = SalesOrder.CGSTAmount,
                    IGSTAmount = SalesOrder.IGSTAmount,
                    CessAmount = SalesOrder.CessAmount,
                    VATAmount = SalesOrder.VATAmount,
                    RoundOff = SalesOrder.RoundOff,
                    NetAmount = SalesOrder.NetAmount,
                    IsDraft = SalesOrder.IsDraft,
                    BillingAddressID = SalesOrder.BillingAddressID,
                    ShippingAddressID = SalesOrder.ShippingAddressID,
                    IsApproved = SalesOrder.IsApproved,
                    IsGST = SalesOrder.IsGST,
                    IsVat = SalesOrder.IsVat,
                    CurrencyExchangeRate = SalesOrder.CurrencyExchangeRate,
                    CustomerEnquiryNumber = SalesOrder.CustomerEnquiryNumber,
                    EnquiryDate = General.ToDateTimeNull(SalesOrder.EnquiryDate),
                    QuotationExpiry = General.ToDateTimeNull(SalesOrder.QuotationExpiry),
                    Remarks = SalesOrder.Remarks,
                    PaymentTerms = SalesOrder.PaymentTerms,
                    VATPercentageID = SalesOrder.VATPercentageID,
                    VATPercentage = SalesOrder.VATPercentage,
                };
                List<SalesItemBO> SalesItems = new List<SalesItemBO>();
                SalesItemBO SalesItem;
                foreach (var item in SalesOrder.Items)
                {
                    SalesItem = new SalesItemBO()
                    {
                        SalesOrderItemID = item.SalesOrderItemID,
                        ID = item.ItemID,
                        ItemName = System.Security.SecurityElement.Escape(item.ItemName),
                        PartsNumber = item.PartsNumber,
                        MRP = item.MRP,
                        BasicPrice = item.BasicPrice,
                        Qty = item.Qty,
                        OfferQty = item.OfferQty,
                        GrossAmount = item.GrossAmount,
                        DiscountPercentage = item.DiscountPercentage,
                        DiscountAmount = item.DiscountAmount,
                        AdditionalDiscount = item.AdditionalDiscount,
                        TaxableAmount = item.TaxableAmount,
                        GSTPercentage = item.GSTPercentage,
                        SGSTPercentage = item.SGSTPercentage,
                        CGSTPercentage = item.CGSTPercentage,
                        IGSTPercentage = item.IGSTPercentage,
                        VATAmount = item.VATAmount,
                        VATPercentage = item.VATPercentage,
                        SecondaryUnit = item.SecondaryUnit,
                        SecondaryMRP = item.SecondaryMRP,
                        SecondaryOfferQty = item.SecondaryOfferQty,
                        SecondaryQty = item.SecondaryQty,
                        SecondaryUnitSize = item.SecondaryUnitSize,
                        ExchangeRate = item.ExchangeRate,
                        CurrencyID = SalesOrder.CurrencyID,
                        DeliveryTerm = item.DeliveryTerm,
                        Model = item.Model,
                        IsGST = item.IsGST,
                        IsVat = item.IsVat,
                        CGST = item.CGST,
                        SGST = item.SGST,
                        IGST = item.IGST,
                        CessPercentage = item.CessPercentage,
                        CessAmount = item.CessAmount,
                        NetAmount = item.NetAmount,
                        UnitID = item.UnitID,
                        BatchTypeID = item.BatchTypeID
                    };
                    SalesItems.Add(SalesItem);
                }
                if (salesOrderBL.SaveSalesOrder(SalesOrderBO, SalesItems))
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
            SalesOrderModel SalesOrder;
            int SalesOrderID;

            SalesOrderID = (int)id;
            SalesOrder = GetSalesOrder(SalesOrderID, "edit");

            if (SalesOrder.Status != "draft")
            {
                return RedirectToAction("Index");
            }
            SalesOrder.ItemCategoryID = SalesOrder.Items.FirstOrDefault().ItemCategoryID;
            SalesOrder.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name", SalesOrder.CustomerCategoryID);
            SalesOrder.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            SalesOrder.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(SalesOrder.ItemCategoryID), "ID", "Name");
            var address = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            SalesOrder.LocationStateID = address != null ? address.StateID : 0;
            SalesOrder.BatchTypeID = customerBL.GetBatchTypeID(SalesOrder.CustomerID).ID;
            SalesOrder.BatchType = customerBL.GetBatchTypeID(SalesOrder.CustomerID).Name;
            SalesOrder.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", SalesOrder.CustomerID, ""), "AddressID", "Location");
            SalesOrder.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", SalesOrder.CustomerID, ""), "AddressID", "Location");
            SalesOrder.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}

                                }, "Value", "Text");
            SalesOrder.IsClone = false;
            SalesOrder.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            SalesOrder.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(SalesOrder.CurrencyID);
            if (classdata != null)
            {
                SalesOrder.normalclass = classdata.normalclass;
                SalesOrder.DecimalPlaces = classdata.DecimalPlaces;
            }
            SalesOrder.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            SalesOrder.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            return View(SalesOrder);

        }

        // Gets offer and discount details on selecting an item 
        [HttpPost]
        public JsonResult GetDiscountAndOfferDetails(int CustomerID, int SchemeID, int ItemID, decimal Qty, int UnitID)
        {
            DiscountAndOfferBO discountAndOffer = salesOrderBL.GetDiscountAndOfferDetails(CustomerID, SchemeID, ItemID, Qty, UnitID);
            return Json(new { Status = "success", discountAndOffer = discountAndOffer }, JsonRequestBehavior.AllowGet);
        }

        // Gets offer details for multiple items 
        [HttpPost]
        public JsonResult GetOfferDetails(int CustomerID, int SchemeID, int[] ItemID, int[] UnitID)
        {
            List<DiscountAndOfferBO> discountAndOffer = salesOrderBL.GetOfferDetails(CustomerID, SchemeID, ItemID, UnitID);
            return Json(new { Status = "success", discountAndOffer = discountAndOffer }, JsonRequestBehavior.AllowGet);
        }

        // Gets offer and discount details on selecting an item 
        [HttpPost]
        public JsonResult GetSchemeAllocation(int CustomerID)
        {
            int SchemeID = salesOrderBL.GetSchemeAllocation(CustomerID);
            return Json(new { Status = "success", SchemeID = SchemeID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalesOrderList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string SalesTypeHint = Datatable.Columns[3].Search.Value;
                string CustomerNameHint = Datatable.Columns[4].Search.Value;
                string DespatchDateHint = Datatable.Columns[5].Search.Value;
                string NetAmountHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = salesOrderBL.GetSalesOrderList(0, 0, Type, CodeHint, DateHint, CustomerNameHint, SalesTypeHint, DespatchDateHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
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
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));
                DatatableResultBO resultBO = salesOrderBL.GetCustomerSalesOrderList(CustomerID,TransNoHint, TransDateHint, PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
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
        public JsonResult GetUnProcessedSalesOrderList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string SalesTypeHint = Datatable.Columns[3].Search.Value;
                string CustomerNameHint = Datatable.Columns[4].Search.Value;
                string NetAmountHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));

                if (CustomerID == 0)
                {
                    CustomerID = -1; // Preventing from populating Proforma invoices when CustomerID is 0 
                }

                DatatableResultBO resultBO = salesOrderBL.GetSalesOrderList(CustomerID, ItemCategoryID, "UnProcessed", CodeHint, DateHint, CustomerNameHint, SalesTypeHint, "", NetAmountHint, SortField, SortOrder, Offset, Limit);
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
                var Items = salesOrderBL.GetBatchwiseSalesOrderItems(SalesOrderID, StoreID, CustomerID, SchemeID).MapToModel();

                ItemIDs = Items.Select(a => a.ItemID).Distinct().ToArray();
                UnitIDs = Items.Select(a => a.UnitID).Distinct().ToArray();
                List<DiscountAndOfferBO> discountAndOffer = salesOrderBL.GetOfferDetails(CustomerID, SchemeID, ItemIDs, UnitIDs);
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
                var BOItems = salesOrderBL.GetGoodsReceiptSalesOrderItems(SalesOrderID).ToList();
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
                salesOrderBL.Approve(SalesOrderID);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "SalesOrder", "Approve", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        // For role privileges
        public ActionResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + salesOrderBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clone(int id)
        {

            SalesOrderModel salesorder = new SalesOrderModel();
            salesorder = GetSalesOrder(id, "clone");

            salesorder.SONo = generalBL.GetSerialNo("SalesOrder", "Code");
            salesorder.SODate = General.FormatDate(DateTime.Now);
            salesorder.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesorder.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            salesorder.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(salesorder.ItemCategoryID), "ID", "Name");
            salesorder.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            salesorder.BatchTypeID = customerBL.GetBatchTypeID(salesorder.CustomerID).ID;

            salesorder.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", salesorder.CustomerID, ""), "AddressID", "Place");
            salesorder.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", salesorder.CustomerID, ""), "AddressID", "Place");
            salesorder.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}

                                    }, "Value", "Text");
            salesorder.IsClone = true;
            salesorder.ID = 0;
            salesorder.IsClone = true;
            salesorder.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            salesorder.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
            salesorder.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            salesorder.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            return View(salesorder);
        }

        //for roles privilege -- TODO  
        public ActionResult Suspend(int ID, string Table)
        {

            //var output = salesOrderBL.SuspendSalesOrder(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalesOrderPrintPdf(int Id)
        {
            return null;
        }

        public JsonResult SalesOrderWithItemCode(int Id)
        {
            return null;
        }
        public JsonResult WithPartNo(int Id)
        {
            return null;
        }
        public JsonResult SalesOrderExportWithItemCode(int Id)
        {
            return null;
        }
        public JsonResult SalesOrderExportPartNo(int Id)
        {
            return null;
        }
    }
}