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
    
    public partial class SpGetUnProcessedPurchaseOrderForService_Result
    {
        public int ID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public System.DateTime PurchaseOrderDate { get; set; }
        public int SupplierID { get; set; }
        public string AdvancePercentage { get; set; }
        public Nullable<decimal> AdvanceAmount { get; set; }
        public int PaymentModeID { get; set; }
        public int ShippingAddressID { get; set; }
        public int BillingAddressID { get; set; }
        public bool InclusiveGST { get; set; }
        public Nullable<int> SelectedQuotationID { get; set; }
        public string OtherQuotationIDS { get; set; }
        public int DeliveryWithin { get; set; }
        public int PaymentWithinID { get; set; }
        public Nullable<int> PaymentWithInDays { get; set; }
        public Nullable<decimal> SGSTAmt { get; set; }
        public Nullable<decimal> CGSTAmt { get; set; }
        public Nullable<decimal> IGSTAmt { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public string SupplierName { get; set; }
        public string RequestedBy { get; set; }
    }
}
