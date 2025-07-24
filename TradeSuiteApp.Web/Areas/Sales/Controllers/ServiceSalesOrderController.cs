using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class ServiceSalesOrderController : Controller
    {
        private ISalesOrder salesOrderBL;
        private IServiceSalesOrderContract serviceSalesOrderBL;
        private ICustomerContract customerBL;
        private ICategoryContract categroyBL;
        private IAddressContract addressBL;
        private IGeneralContract generalBL;
        private IUnitContract unitBL;
        private IPaymentModeContract paymentModeBL;
        private IDropdownContract _dropdown;
        public ServiceSalesOrderController(IDropdownContract tempDropdown)
        {
            serviceSalesOrderBL = new ServiceSalesOrderBL();
            salesOrderBL = new SalesOrderBL();
            this._dropdown = tempDropdown;
            customerBL = new CustomerBL();
            categroyBL = new CategoryBL();
            addressBL = new AddressBL();
            generalBL = new GeneralBL();
            paymentModeBL = new PaymentModeBL();
            unitBL = new UnitBL();
        }
        // GET: Sales/ServiceSalesOrder
        public ActionResult Index()
        {
            SalesOrderModel salesOrder = new SalesOrderModel();
            return View(salesOrder);
        }

        //public ActionResult Create()
        //{
        //    SalesOrderModel salesOrder = new SalesOrderModel();
        //    salesOrder.SONo = generalBL.GetSerialNo("ServiceSalesOrder", "Code");
        //    salesOrder.SODate = General.FormatDate(DateTime.Now);
        //    salesOrder.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
        //    salesOrder.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
        //    salesOrder.Items = new List<SalesItemModel>();
        //    salesOrder.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
        //    salesOrder.DespatchDate = General.FormatDate(DateTime.Now);
        //    salesOrder.AmountDetails = new List<SalesAmount>();
        //    salesOrder.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", salesOrder.PaymentModeID);
        //    salesOrder.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", 0, ""), "AddressID", "AddressLine1");
        //    salesOrder.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", 0, ""), "AddressID", "AddressLine1");
        //    return View(salesOrder);

        //}
        public ActionResult Create(int? IPID = 0)
        {
            SalesOrderModel salesOrder = new SalesOrderModel();
            SalesItemModel SalesItem;
            List<SalesItemBO> SalesItems;

            salesOrder.SONo = generalBL.GetSerialNo("ServiceSalesInvoice", "Code");
            salesOrder.SODate = General.FormatDate(DateTime.Now);
            salesOrder.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesOrder.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
            salesOrder.Items = new List<SalesItemModel>();
            salesOrder.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            salesOrder.DespatchDate = General.FormatDate(DateTime.Now);
            salesOrder.AmountDetails = new List<SalesAmount>();
            salesOrder.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", salesOrder.PaymentModeID);
            salesOrder.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", 0, ""), "AddressID", "AddressLine1");
            salesOrder.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", 0, ""), "AddressID", "AddressLine1");
            if ((int)IPID != 0)
            {
                salesOrder.CustomerID = serviceSalesOrderBL.GetCustomerID((int)IPID);
                CustomerBO CustomerBO = customerBL.GetCustomerDetails(salesOrder.CustomerID);
                salesOrder.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", salesOrder.CustomerID, ""), "AddressID", "AddressLine1");
                salesOrder.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", salesOrder.CustomerID, ""), "AddressID", "AddressLine1");
                salesOrder.StateID = addressBL.GetBillingAddress("Customer", salesOrder.CustomerID, "").FirstOrDefault().StateID;
                salesOrder.CustomerName = CustomerBO.Name;
                salesOrder.CustomerCategory = CustomerBO.CategoryName;
                salesOrder.CustomerCategoryID = CustomerBO.CategoryID;
                salesOrder.CustomerDetails = new CustomersModel()
                {
                    Name = CustomerBO.Name,
                    CustomerCategoryName = CustomerBO.CustomerCategory,
                    CustomerAccountsCategoryName = CustomerBO.CustomerAccountsCategoryName,
                    GstNo = CustomerBO.GstNo,
                    Color = CustomerBO.Color,
                    OutstandingAmount = CustomerBO.OutstandingAmount,
                };
                SalesItems = serviceSalesOrderBL.GetBillableDetails((int)IPID, salesOrder.CustomerID);
                salesOrder.Items = new List<SalesItemModel>();
                foreach (var item in SalesItems)
                {
                    SalesItem = new SalesItemModel()
                    {
                        SalesOrderItemID = item.SalesOrderItemID,
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        UnitName = item.Unit,
                        ItemCode = item.Code,
                        ItemName = item.Name,
                        ItemCategoryID = item.ItemCategoryID,
                        FullOrLoose = item.FullOrLoose,
                        Qty = item.Qty,
                        OfferQty = item.OfferQty,
                        MRP = item.MRP,
                        BasicPrice = item.BasicPrice,
                        GrossAmount = item.GrossAmount,
                        DiscountPercentage = item.DiscountPercentage,
                        DiscountAmount = item.DiscountAmount,
                        AdditionalDiscount = item.AdditionalDiscount,
                        TaxableAmount = item.TaxableAmount,
                        SGST = item.SGST,
                        CGST = item.CGST,
                        IGST = item.IGST,
                        SGSTPercentage = item.SGSTPercentage,
                        CGSTPercentage = item.CGSTPercentage,
                        IGSTPercentage = item.IGSTPercentage,
                        CessPercentage = item.CessPercentage,
                        CessAmount = item.CessAmount,
                        GSTPercentage = item.GSTPercentage,
                        NetAmount = item.NetAmount,
                        DoctorID = item.DoctorID,
                        DoctorName = item.DoctorName,
                        Remarks = item.Remarks,
                        BillableID = item.BillableID,
                        //GSTAmount = item.GSTAmount
                    };
                    salesOrder.Items.Add(SalesItem);
                }
            }
            
            return View(salesOrder);
        }
        public JsonResult GetDiscountPercentage(int CustomerID, int ItemID)
        {
            DiscountAndOfferBO discountAndOffer = new DiscountAndOfferBO();
            discountAndOffer.DiscountPercentage = serviceSalesOrderBL.GetDiscountPercentage(CustomerID, ItemID);
            return Json(new { Status = "success", discountAndOffer = discountAndOffer.DiscountPercentage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(SalesOrderModel SalesOrder)
        {
            var result = new List<object>();
            try
            {
                if (SalesOrder.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    SalesOrderBO Temp = serviceSalesOrderBL.GetServiceSalesOrder(SalesOrder.ID);
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                SalesOrderBO SalesOrderBO = new SalesOrderBO()
                {
                    ID = SalesOrder.ID,
                    SONo = SalesOrder.SONo,
                    SODate = General.ToDateTime(SalesOrder.SODate),
                    CustomerCategoryID = SalesOrder.CustomerCategoryID,
                    ItemCategoryID = SalesOrder.ItemCategoryID,
                    CustomerID = SalesOrder.CustomerID,
                    GrossAmount = SalesOrder.GrossAmount,
                    DiscountAmount = SalesOrder.DiscountAmount,
                    TaxableAmount = SalesOrder.TaxableAmount,
                    SGSTAmount = SalesOrder.SGSTAmount,
                    CGSTAmount = SalesOrder.CGSTAmount,
                    IGSTAmount = SalesOrder.IGSTAmount,
                    CessAmount = SalesOrder.CessAmount,
                    RoundOff = SalesOrder.RoundOff,
                    NetAmount = SalesOrder.NetAmount,
                    IsDraft = SalesOrder.IsDraft,
                    BillingAddressID = SalesOrder.BillingAddressID,
                    ShippingAddressID = SalesOrder.ShippingAddressID,
                    DirectInvoice = SalesOrder.DirectInvoice,
                    PaymentModeID = SalesOrder.PaymentModeID,
                    

                };

                List<SalesItemBO> SalesItems = new List<SalesItemBO>();
                SalesItemBO SalesItem;
                foreach (var item in SalesOrder.Items)
                {
                    SalesItem = new SalesItemBO()
                    {
                        SalesOrderItemID = item.SalesOrderItemID,
                        ItemID = item.ItemID,
                        MRP = item.MRP,
                        BasicPrice = item.BasicPrice,
                        Qty = item.Qty,
                        OfferQty = item.OfferQty,
                        GrossAmount = item.GrossAmount,
                        DiscountPercentage = item.DiscountPercentage,
                        DiscountAmount = item.DiscountAmount,
                        TaxableAmount = item.TaxableAmount,
                        GSTPercentage = item.GSTPercentage,
                        SGSTPercentage = item.SGSTPercentage,
                        IGSTPercentage = item.IGSTPercentage,
                        CGSTPercentage = item.IGSTPercentage,
                        CGST = item.CGST,
                        SGST = item.SGST,
                        IGST = item.IGST,
                        CessPercentage = item.CessPercentage,
                        CessAmount = item.CessAmount,
                        NetAmount = item.NetAmount,
                        UnitID = item.UnitID,
                        DoctorID = item.DoctorID,
                        Remarks = item.Remarks,
                        BillableID=item.BillableID
                    };
                    SalesItems.Add(SalesItem);
                }

                List<SalesAmountBO> SalesAmountDetails = new List<SalesAmountBO>();
                SalesAmountBO SalesAmount;
                foreach (var item in SalesOrder.AmountDetails)
                {
                    SalesAmount = new SalesAmountBO()
                    {
                        Particulars = item.Particulars,
                        Amount = item.Amount,
                        Percentage = item.Percentage,
                        TaxableAmount = item.TaxableAmount
                    };
                    SalesAmountDetails.Add(SalesAmount);
                }
                if (serviceSalesOrderBL.SaveServiceSalesOrder(SalesOrderBO, SalesItems, SalesAmountDetails))
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
                generalBL.LogError("Sales", "ServiceSalesOrder", "Save", SalesOrder.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetServiceSalesOrderList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string NetAmountHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                DatatableResultBO resultBO = serviceSalesOrderBL.GetServiceSalesOrderList(Type, CodeHint, DateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "ServiceSalesOrder", "GetServiceSalesOrderList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetServiceSalesUnprocessOrderList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string NetAmountHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));
                DatatableResultBO resultBO = serviceSalesOrderBL.GetServiceSalesUnprocessOrderList(CustomerID, "", CodeHint, DateHint, CustomerNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "ServiceSalesOrder", "GetServiceSalesUnprocessOrderList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetServiceSalesOrderItems(int[] SalesOrderID)
        {
            try
            {
                List<SalesItemBO> Items = serviceSalesOrderBL.GetServiceSalesOrderItemsBySalesOrderIDs(SalesOrderID);
                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "ServiceSalesOrder", "GetServiceSalesOrderItems", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int? id)
        {
            SalesOrderBO salesorderBO = new SalesOrderBO();
            SalesOrderModel SalesOrder;
            int SalesOrderID;
            if (id == null)
            {
                return View("Page Not Found");
            }

            SalesOrderID = (int)id;
            SalesOrder = this.GetServiceSalesOrder(SalesOrderID);
            return View(SalesOrder);

        }

        public ActionResult Edit(int? id)
        {
            SalesOrderBO salesorderBO = new SalesOrderBO();
            SalesOrderModel SalesOrder = new SalesOrderModel();
            int SalesOrderID;
            if (id == null)
            {
                return View("Page Not Found");
            }

            SalesOrderID = (int)id;
            SalesOrder = this.GetServiceSalesOrder(SalesOrderID);
            if (!SalesOrder.IsDraft || SalesOrder.IsCancelled)
            {
                return RedirectToAction("Index");
            }
            SalesOrder.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", SalesOrder.PaymentModeID);

            return View(SalesOrder);

        }

        private SalesOrderModel GetServiceSalesOrder(int SalesOrderID)
        {

            SalesOrderModel SalesOrder;
            SalesItemModel SalesItem;
            List<SalesItemBO> SalesItems;

            SalesOrderBO SalesOrderBO = serviceSalesOrderBL.GetServiceSalesOrder(SalesOrderID);
            CustomerBO CustomerBO = customerBL.GetCustomerDetails(SalesOrderBO.CustomerID);
            SalesOrder = new SalesOrderModel()
            {
                ID = SalesOrderBO.ID,
                SONo = SalesOrderBO.SONo,
                SODate = General.FormatDate(SalesOrderBO.SODate),
                CustomerID = SalesOrderBO.CustomerID,
                PriceListID = SalesOrderBO.PriceListID,
                StateID = SalesOrderBO.StateID,
                IsGSTRegistered = SalesOrderBO.IsGSTRegistered,
                CustomerName = SalesOrderBO.CustomerName.Trim(),
                DespatchDate = General.FormatDate(SalesOrderBO.DespatchDate),
                CustomerCategoryID = SalesOrderBO.CustomerCategoryID,
                CustomerCategory = SalesOrderBO.CustomerCategory,
                GrossAmount = SalesOrderBO.GrossAmount,
                DiscountAmount = SalesOrderBO.DiscountAmount,
                TaxableAmount = SalesOrderBO.TaxableAmount,
                SGSTAmount = SalesOrderBO.SGSTAmount,
                CGSTAmount = SalesOrderBO.CGSTAmount,
                IGSTAmount = SalesOrderBO.IGSTAmount,
                CessAmount = SalesOrderBO.CessAmount,
                RoundOff = SalesOrderBO.RoundOff,
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
                DirectInvoice = SalesOrderBO.DirectInvoice,
                PaymentModeID = SalesOrderBO.PaymentModeID,
                IsDraft = SalesOrderBO.IsDraft
            };
            SalesOrder.CustomerDetails = new CustomersModel()
            {
                Name = CustomerBO.Name,
                CustomerCategoryName = CustomerBO.CustomerCategory,
                CustomerAccountsCategoryName = CustomerBO.CustomerAccountsCategoryName,
                GstNo = CustomerBO.GstNo,
                Color = CustomerBO.Color,
                OutstandingAmount = CustomerBO.OutstandingAmount,
            };
            SalesOrder.IsCancelable = serviceSalesOrderBL.IsCancelable(SalesOrderBO.ID);
            SalesItems = serviceSalesOrderBL.GetServiceSalesOrderItems(SalesOrderID);
            SalesOrder.Items = new List<SalesItemModel>();

            foreach (var item in SalesItems)
            {
                SalesItem = new SalesItemModel()
                {
                    SalesOrderItemID = item.SalesOrderItemID,
                    ItemID = item.ItemID,
                    UnitID = item.UnitID,
                    UnitName = item.Unit,
                    ItemCode = item.Code,
                    ItemName = item.Name,
                    ItemCategoryID = item.ItemCategoryID,
                    FullOrLoose = item.FullOrLoose,
                    Qty = item.Qty,
                    OfferQty = item.OfferQty,
                    MRP = item.MRP,
                    BasicPrice = item.BasicPrice,
                    GrossAmount = item.GrossAmount,
                    DiscountPercentage = item.DiscountPercentage,
                    DiscountAmount = item.DiscountAmount,
                    AdditionalDiscount = item.AdditionalDiscount,
                    TaxableAmount = item.TaxableAmount,
                    SGST = item.SGST,
                    CGST = item.CGST,
                    IGST = item.IGST,
                    SGSTPercentage = item.SGSTPercentage,
                    CGSTPercentage = item.CGSTPercentage,
                    IGSTPercentage = item.IGSTPercentage,
                    CessPercentage = item.CessPercentage,
                    CessAmount = item.CessAmount,
                    GSTPercentage = item.GSTPercentage,
                    NetAmount = item.NetAmount,
                    DoctorID = item.DoctorID,
                    DoctorName = item.DoctorName,
                    Remarks = item.Remarks

                };
                SalesOrder.Items.Add(SalesItem);

            }

            SalesOrder.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            SalesOrder.ItemCategoryList = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
            SalesOrder.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            SalesOrder.DespatchDate = General.FormatDate(DateTime.Now);
            SalesOrder.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", SalesOrderBO.CustomerID, ""), "AddressID", "AddressLine1");
            SalesOrder.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", SalesOrderBO.CustomerID, ""), "AddressID", "AddressLine1");
            return SalesOrder;
        }

        public ActionResult Cancel(int SalesOrderID)
        {
            try
            {
                if (serviceSalesOrderBL.IsCancelable(SalesOrderID))
                {
                    serviceSalesOrderBL.Cancel(SalesOrderID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "fail" }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "ServiceSalesOrder", "Cancel", SalesOrderID, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(SalesOrderModel SalesOrder)
        {
            return Save(SalesOrder);
        }

        [HttpPost]
        public JsonResult GetBillingDetails(int CustomerID)
        {
            try
            {
                List<SalesItemBO> SalesItems = serviceSalesOrderBL.GetBillableDetails(0, CustomerID);
                return Json(new { Status = "success", Data = SalesItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}