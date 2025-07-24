using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
    public class GRNTransItemBO
    {
        public string Make { get; set; }
        public int GRNID { get; set; }
        public int GRNTransID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string PartsNumber { get; set; }
        public string Remark { get; set; }
        public string Model { get; set; }
        public int CurrencyID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryRate { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public decimal SecondaryPurchaseOrderQty { get; set; }
        public decimal SecondaryReceivedQty { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public decimal SecondaryReturnQty { get; set; }

        public decimal? AcceptedQty { get; set; }
        public decimal? ApprovedQty { get; set; }
        public decimal? UnMatchedQty { get; set; }
        public decimal PORate { get; set; }
        public decimal ApprovedValue { get; set; }
        public decimal? SGSTPercent { get; set; }
        public decimal? CGSTPercent { get; set; }
        public decimal? IGSTPercent { get; set; }
        public decimal? InvoiceGSTPercent { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal SuppDocAmount { get; set; }
        public decimal SuppShipAmount { get; set; }
        public decimal SuppFreight { get; set; }
        public decimal LocalCustomsDuty { get; set; }
        public decimal LocalFreight { get; set; }
        public decimal LocalMiscCharge { get; set; }
        public decimal LocalOtherCharges { get; set; }
        public decimal PackingForwarding { get; set; }
        public decimal SuppOtherCharge { get; set; }
        public int? GSTPercent { get { return Convert.ToInt16(SGSTPercent + CGSTPercent + IGSTPercent); } set { } }
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
        public string BatchNo { get; set; }
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
        public decimal SecondaryQty { get; set; }//for rdlc
        public decimal InvoiceRate { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal Difference { get; set; }
        public string Remarks { get; set; }
        public decimal Stock { get; set; }
        public int Id { get; set; }
        public int PurchaseInvoiceID { get; set; }
        public bool IsQCRequired { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal QtyTolerance { get; set; }

        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal LastPurchaseRate { get; set; }
        public decimal PendingPOQty { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public decimal PurchaseAmount { get; set; }
        public bool InclusiveGST { get; set; }
        public int WarehouseID { get; set; }
        public string GrnNo { get; set; }
        public string BatchType { get; set; }
        public string InvoiceNo { get; set; }
        public int InvoiceID { get; set; }
        public int InvoiceTransID { get; set; }
        public int MilkPurchaseID { get; set; }
        public decimal AllowedQty { get; set; }
        public decimal PurchaseOrderQuantity { get; set; }
        public string ItemCategory { get; set; }
        public int BatchTypeID { get; set; }
        public string PurchaseReturnOrderNo { get; set; }
        public string PurchaseReturnNo { get; set; }        
        public decimal ConvertedQty { get; set; }
        public decimal ConvertedStock { get; set; }
        public int PrimaryUnitID { get; set; }
        public int PurchaseUnitID { get; set; }
        public string PurchaseUnit { get; set; }
        public string PrimaryUnit { get; set; }
        public string PurchaseNo { get; set; }
        public decimal LooseQty { get; set; }
        public decimal LooseRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal OfferQty { get; set; }
        public decimal OfferReturnQty { get; set; }
        public int DiscountID { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }

        public int BatchID { get; set; }
        public decimal ProfitPrice { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal PurchaseMRP { get; set; }
        public decimal RetailMRP { get; set; }
        public decimal ProfitRatio { get; set; }
        public decimal CessPercent { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public int GSTID { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal CurrentProfitTolerance { get; set; }
        public decimal PrevoiusBatchNetProfitRatio { get; set; }
        public decimal PackSize { get; set; }
        public decimal POLooseQty { get; set; }
        public decimal PrintingQty { get; set; }
        public SelectList GSTPercentageList { get; set; }

        public string Item { get; set; }
        public decimal MRP { get; set; }
        public string SupplierName { get; set; }
        public string GSTNo { get; set; }
        public string SupplierLocation { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public decimal LandingCost { get; set; }
        public string BinCode { get; set; }
    }
}
