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
    
    public partial class SpGetPurchaseReturnTrans_Result
    {
        public int ID { get; set; }
        public int PurchaseReturnID { get; set; }
        public int GrnID { get; set; }
        public string ITEMNAME { get; set; }
        public string unit { get; set; }
        public int ItemID { get; set; }
        public decimal AcceptedQty { get; set; }
        public decimal QTY { get; set; }
        public decimal Rate { get; set; }
        public Nullable<decimal> SGSTPercent { get; set; }
        public Nullable<decimal> CGSTPercent { get; set; }
        public Nullable<decimal> IGSTPercent { get; set; }
        public Nullable<decimal> SGSTAmount { get; set; }
        public Nullable<decimal> CGSTAmount { get; set; }
        public Nullable<decimal> IGSTAmount { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int FinYear { get; set; }
        public int ApplicationID { get; set; }
        public int LocationID { get; set; }
        public string PurchaseReturnOrderNo { get; set; }
        public int BatchTypeID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> ReturnOrderID { get; set; }
        public Nullable<int> ReturnOrderTransID { get; set; }
        public Nullable<decimal> Stock { get; set; }
        public Nullable<decimal> GRNQty { get; set; }
    }
}
