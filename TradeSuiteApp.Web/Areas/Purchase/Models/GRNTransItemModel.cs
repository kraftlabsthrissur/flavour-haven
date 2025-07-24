//File created by prama on 27-4-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class GRNTransItemModel
    {
        public int GRNID { get; set; }
        public int GRNTransID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string Remark { get; set; }
        public string Model { get; set; }
        public int CurrencyID { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal? AcceptedQty { get; set; }
        public decimal ApprovedQty { get; set; }
        public decimal? UnMatchedQty { get; set; }
        public decimal PORate { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal? SGSTPercent { get; set; }
        public decimal? CGSTPercent { get; set; }
        public int InvoiceID { get; set; }
        public int InvoiceTransID { get; set; }
        public decimal? IGSTPercent { get; set; }
        public decimal? SGSTAmt { get; set; }
        public decimal? CGSTAmt { get; set; }
        public decimal? IGSTAmt { get; set; }
        public decimal? FreightAmt { get; set; }
        public decimal? OtherCharges { get; set; }
        public decimal? PackingShippingCharge { get; set; }
        public int PurchaseOrderID { get; set; }
        public int? POTransID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string Batch { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public decimal PurchaseOrderQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal QualityCheckQty { get; set; }
        public decimal RejectedQty { get; set; }
        public int ItemOrderPreference { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal SecondaryInvoiceQty { get; set; }
        public decimal InvoiceRate { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal OfferQty { get; set; }
        public decimal OfferReturnQty { get; set; }
        public decimal Difference { get; set; }
        public string Remarks { get; set; }
        public int Id { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public bool IsQCRequired { get; set; }
        public decimal ReturnQty { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal LastPurchaseRate { get; set; }
        public decimal PendingPOQty { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public decimal PurchaseAmount { get; set; }
        public bool InclusiveGST { get; set; }
        public int WarehouseID { get; set; }
        public string GrnNo { get; set; }
        public string InvoiceNo { get; set; }
        public int BatchTypeID { get; set; }
        public string PurchaseReturnOrderNo { get; set; }
        public string PurchaseReturnNo { get; set; }
        public decimal Stock { get; set; }
        public decimal ConvertedQty { get; set; }
        public decimal ConvertedStock { get; set; }
        public int PrimaryUnitID { get; set; }
        public int PurchaseUnitID { get; set; }
        public SelectList UOMList { get; set; }
        public string PurchaseNo { get; set; }
        public decimal GSTPercent { get; set; }
        public decimal GSTAmount { get; set; }
        public SelectList GSTPercentageList { get; set; }
        public int GSTID { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Discount { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryRate { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryReturnQty { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
    }
}