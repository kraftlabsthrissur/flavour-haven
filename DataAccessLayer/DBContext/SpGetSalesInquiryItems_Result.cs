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
    
    public partial class SpGetSalesInquiryItems_Result
    {
        public Nullable<int> SalesInquiryID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string SIOrVINNumber { get; set; }
        public string PartsNumber { get; set; }
        public string DeliveryTerm { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<decimal> VATPercentage { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
    }
}
