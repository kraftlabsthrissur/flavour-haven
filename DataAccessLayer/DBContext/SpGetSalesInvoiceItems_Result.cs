//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer.DBContext
{
    using System;
    
    public partial class SpGetSalesInvoiceItems_Result
    {
        public int ID { get; set; }
        public Nullable<int> SalesInvoiceID { get; set; }
        public Nullable<int> SalesOrderTransID { get; set; }
        public Nullable<int> ProformaInvoiceTransID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<decimal> InvoiceQty { get; set; }
        public Nullable<decimal> InvoiceOfferQty { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> BasicPrice { get; set; }
        public Nullable<decimal> OfferQty { get; set; }
        public string SecondaryUnit { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal SecondaryOfferQty { get; set; }
        public decimal SecondaryQty { get; set; }
        public decimal SecondaryUnitSize { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<int> BatchID { get; set; }
        public Nullable<int> BatchTypeID { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> AdditionalDiscount { get; set; }
        public Nullable<decimal> TurnoverDiscount { get; set; }
        public Nullable<decimal> TaxableAmount { get; set; }
        public Nullable<decimal> SGSTPercentage { get; set; }
        public Nullable<decimal> CGSTPercentage { get; set; }
        public Nullable<decimal> IGSTPercentage { get; set; }
        public Nullable<int> IsGST { get; set; }
        public Nullable<int> IsVat { get; set; }
        public Nullable<decimal> VATPercentage { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public Nullable<decimal> CGSTAmt { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public Nullable<int> WareHouseID { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string DeliveryTerm { get; set; }
        public string Model { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string UnitName { get; set; }
        public string Code { get; set; }
        public string BatchName { get; set; }
        public Nullable<decimal> Stock { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> CashDiscount { get; set; }
        public decimal LooseRate { get; set; }
        public Nullable<int> SalesUnitID { get; set; }
        public int POTransID { get; set; }
        public int PurchaseOrderID { get; set; }
        public decimal PORate { get; set; }
        public Nullable<bool> PrintWithItemName { get; set; }
        public bool PrintWithItemCode { get; set; }
        public decimal POQuantity { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal CessAmount { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<decimal> PackSize { get; set; }
        public string PrimaryUnit { get; set; }
        public string PurchaseOrderNo { get; set; }
        public Nullable<System.DateTime> PurchaseOrderDate { get; set; }
        public string Remarks { get; set; }
        public string Make { get; set; }
        public string dndate { get; set; }
        public string BatchType { get; set; }
        public int CategoryID { get; set; }
        public string packnumber { get; set; }
        public Nullable<int> DecimalPlaces { get; set; }
    }
}
