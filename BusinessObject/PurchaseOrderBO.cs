using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Web.Mvc;

namespace BusinessObject
{
    public class PurchaseOrderBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MinimumCurrency { get; set; }
        public string AmountInWords { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string SuppQuotNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public int SupplierID { get; set; }
        public string ItemName { get; set; }
        public string ItemCatagory { get; set; }
        public Nullable<int> AdvancePercentage { get; set; }
        public Nullable<decimal> AdvanceAmount { get; set; }
        public decimal? POAmount { get; set; }
        public Nullable<int> PaymentModeID { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<int> ShippingAddressID { get; set; }
        public Nullable<int> ShippingStateID { get; set; }
        public Nullable<int> BillingAddressID { get; set; }
        public bool InclusiveGST { get; set; }
        public bool GstExtra { get; set; }
        public Nullable<int> SelectedQuotationID { get; set; }
        public string OtherQuotationIDS { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public Nullable<int> DeliveryWithinID { get; set; }
        public string DeliveryWithin { get; set; }
        //public int? DeliveryWithin { get; set; }
        public int PaymentWithinID { get; set; }
        public int PaymentWithin { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public Nullable<decimal> CGSTAmt { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public decimal VATPercentage { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal SuppOtherCharge { get; set; }
        public string MobileNo { get; set; }
        public string CountryName { get; set; }
        public Nullable<decimal> FreightAmt { get; set; }
        public Nullable<decimal> OtherCharges { get; set; }
        public Nullable<decimal> PackingShippingCharge { get; set; }
        public decimal NetAmt { get; set; }
        public bool OrderMet { get; set; }
        public bool IsDraft { get; set; }
        public string SuppDocCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string BAddressLine1 { get; set; }
        public string BAddressLine2 { get; set; }
        public string BAddressLine3 { get; set; }
        public string SuppShipCode { get; set; }
        public string SuppOtherRemark { get; set; }
        public string OrderType { get; set; }
       // public string SuppQuotNo { get; set; }
        public string Shipment { get; set; }
        public string Remarks { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal DecimalPlaces { get; set; }
        public bool Cancelled { get; set; }
        public int IsVat { get; set; }
        public int IsGST { get; set; }
        public string TaxType { get; set; }
        public int TaxTypeID { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public int CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int ApplicationID { get; set; }
        public int UnitID { get; set; }
        public int DaysToSupply { get; set; }
        public string ShipplingLocation { get; set; }
        public string SupplierName { get; set; }
        public string BillingLocation { get; set; }
        public string RequestedBy { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSuspended { get; set; }
        public decimal? OtherDeductions { get; set; }
        public decimal? Discount { get; set; }
        public bool IsBranchLocation { get; set; }
        public bool DirectInvoice { get; set; }
        public bool IsClone { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDateStr { get; set; }
        public int GSTID { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int IsInterCompany { get; set; }
        public List<PurchaseOrderTransBO> items { get; set; }

        public SelectList PaymentDaysList { get; set; }
        public SelectList DDLBillTo { get; set; }
        public SelectList DDLShipTo { get; set; }
        public SelectList DDLPaymentMode { get; set; }
        public SelectList DDLItemCategory { get; set; }
        public SelectList DDLPurchaseCategory { get; set; }
        public SelectList UnitList { get; set; }
        public SelectList GSTList { get; set; }
        public List<RequisitionBO> UnProcessedPrList { get; set; }

        public List<FileBO> SelectedQuotation { get; set; }
        public List<FileBO> OtherQuotations { get; set; }

        //Additional
        public int StateId { get; set; }
        public bool IsGSTRegistred { get; set; }
        public string SupplierLocation { get; set; }
        public string SupplierReferenceNo { get; set; }
        public string TermsOfPrice { get; set; }

        public SelectList DDLDepartment { get; set; }
        public SelectList DDLLocation { get; set; }
        public SelectList DDLEmployee { get; set; }
        public SelectList DDLInterCompany { get; set; }
        public SelectList DDLProject { get; set; }
        public bool IsCancellable { get; set; }
        public List<AddressBO> ShippingAddressList { get; set; }
        public List<AddressBO> BillingAddressList { get; set; }

        public AddressBO HomeAddress { get; set; }
        public AddressBO SupplierAddress { get; set; }

        public string PurchaseRequisitionIDS { get; set; }
        public int ItemCategoryID { get; set; }
        public SelectList BatchTypeList { get; set; }
        public int BatchTypeID { get; set; }
        public SelectList TravelFromList { get; set; }
        public SelectList TravelToList { get; set; }
        public SelectList TransportModeList { get; set; }
        public SelectList LocationList { get; set; }
        public int SalesOrderLocationID { get; set; }
        public int CashPaymentLimit { get; set; }
        public int InterCompanyLocationID { get; set; }
        public string Email { get; set; }

        public SelectList BusinessCategoryList { get; set; }
        public int BusinessCategoryID { get; set; }

        public decimal GrossAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string normalclass { get; set; }
        public SelectList OrderTypeList { get; set; }
        public int IsVATExtra { get; set; }
    }

    public class PurchaseOrderTransBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string HSNCode { get; set; }
        public int PurchaseOrderID { get; set; }
        public int? PurchaseReqID { get; set; }
        //public int? PurServiceTransID { get; set; }
        public int? PRTransID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> OfferQty { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> RetailRate { get; set; }
        public Nullable<decimal> RetailMRP { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> SGSTPercent { get; set; }
        public Nullable<decimal> CGSTPercent { get; set; }
        public Nullable<decimal> IGSTPercent { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> VATPercentage { get; set; }
        public Nullable<decimal> CGSTAmt { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public decimal DiscountPercent { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> TaxableAmount { get; set; }
        public Nullable<decimal> LastPurchaseRate { get; set; }
        public Nullable<decimal> LowestPR { get; set; }
        public Nullable<bool> Purchased { get; set; }
        public Nullable<decimal> QtyMet { get; set; }
        public Nullable<decimal> QtyInQC { get; set; }
        public Nullable<decimal> QtyAvailable { get; set; }
        public Nullable<decimal> QtyOrdered { get; set; }
        public string PartsNumber { get; set; }
        public string Remarks { get; set; }
        public string Remark { get; set; }
        public string Model { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public bool IsQCRequired { get; set; }

        public string ServiceLocation { get; set; }
        public string Employee { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string Project { get; set; }

        public decimal? GSTPercentage { get; set; }

        // ADDITIONAL 
        public string PurchaseOrderNo { get; set; }
        public string PurchaseRequisitionNo { get; set; }
        public decimal PendingPOQty { get; set; }
        public decimal PendingPOSecondaryQty { get; set; }
        public int ItemCategoryID { get; set; }

        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public int FGCategoryID { get; set; }

        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string TransportMode { get; set; }
        public string TravelingRemarks { get; set; }
        public int? TravelToID { get; set; }
        public int? TravelFromID { get; set; }
        public int? TransportModeID { get; set; }
        public Nullable<System.DateTime> TravelDate { get; set; }
        public int TravelCategoryID { get; set; }
        public string TravelDateString { get; set; }
        public bool IsSuspended { get; set; }

        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public decimal PackSize { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public string SecondaryUnit { get; set; }
        public Nullable<decimal> SecondaryUnitSize { get; set; }
        public Nullable<decimal> SecondaryRate { get; set; }
        public Nullable<decimal> SecondaryQty { get; set; }
        public List<SecondaryUnitBO> SecondaryUnitList { get; set; }
        public string normalclass { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal SuppOtherCharge { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public int PurchaseRequisitionTrasID { get; set; }
        public int ServiceLocationID { get; set; }
        public int DepartmentID { get; set; }
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public int ProjectID { get; set; }
        public string BinCode { get; set; }
        public int BinID { get; set; }
        public string Make { get; set; }
    }
    public class PurchaseOrderSupplierBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<int> SupplierCategoryID { get; set; }
        public string Location { get; set; }
    }
    public class PurchaseOrderItemBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal? LastPR { get; set; }
        public decimal? LowestPR { get; set; }
        public decimal? PendingOrderQty { get; set; }
        public decimal? Qty { get; set; }
        public decimal? QtyUnderQC { get; set; }
        public decimal? QtyAvailable { get; set; }
        public decimal? RequestedQty { get; set; }
        public decimal? OrderedQty { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public int GSTCategoryID { get; set; }
        public string Remarks { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public int PRTransID { get; set; }
        //code below by prama on 1-6-18
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public int FGCategoryID { get; set; }
        public int PurchaseUnitID { get; set; }
        public string PurchaseUnit { get; set; }
    }

    public class IsItemSuppliedBySupplier
    {
        public int ItemID { get; set; }
        public string Status { get; set; }
    }

    //public class PurchaseOrderTransDetailsBO
    //{
    //    public int PurchaseOrderID { get; set; }
    //    public int ItemID { get; set; }
    //    public string Name { get; set; }
    //    public decimal Quantity { get; set; }
    //    public Nullable<decimal> Rate { get; set; }
    //    public Nullable<decimal> Amount { get; set; }
    //    public Nullable<decimal> SGSTPercent { get; set; }
    //    public Nullable<decimal> CGSTPercent { get; set; }
    //    public Nullable<decimal> IGSTPercent { get; set; }
    //    public Nullable<decimal> SGSTAmt { get; set; }
    //    public Nullable<decimal> CGSTAmt { get; set; }
    //    public Nullable<decimal> IGSTAmt { get; set; }
    //    public Nullable<bool> Purchased { get; set; }
    //    public Nullable<decimal> QtyMet { get; set; }
    //    public string Remarks { get; set; }
    //    public int FinYear { get; set; }
    //    public int LocationID { get; set; }
    //    public int ApplicationID { get; set; }
    //}
}
