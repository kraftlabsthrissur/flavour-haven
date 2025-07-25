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
    
    public partial class SpGetGoodsReceiptItemDetail_Result
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string SalesOrderNo { get; set; }
        public string SalesInvoiceNo { get; set; }
        public string CounterSalesNo { get; set; }
        public string ItemName { get; set; }
        public string PartsNumber { get; set; }
        public string Model { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<int> IsVat { get; set; }
        public Nullable<int> IsGST { get; set; }
        public Nullable<bool> PrintWithItemName { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> BasicPrice { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> OfferQty { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> TaxableAmount { get; set; }
        public Nullable<decimal> SGSTAmount { get; set; }
        public Nullable<decimal> CGSTAmount { get; set; }
        public Nullable<decimal> IGSTAmount { get; set; }
        public decimal CessAmount { get; set; }
        public Nullable<decimal> VATPercentage { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public decimal SecondaryMRP { get; set; }
        public decimal SecondaryQty { get; set; }
        public string SecondaryUnit { get; set; }
        public Nullable<System.DateTime> SONO { get; set; }
    }
}
