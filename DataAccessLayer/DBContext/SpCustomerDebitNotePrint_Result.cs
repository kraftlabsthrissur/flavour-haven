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
    
    public partial class SpCustomerDebitNotePrint_Result
    {
        public string DebitNoteNo { get; set; }
        public Nullable<System.DateTime> DebitNoteDate { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> ItemAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string ReferenceNumber { get; set; }
        public Nullable<System.DateTime> ReferenceDate { get; set; }
    }
}
