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
    
    public partial class SpGetPreProcessReceiptList_Result
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string IssuedItem { get; set; }
        public string IssuesUnit { get; set; }
        public Nullable<decimal> IssuedQty { get; set; }
        public string ReceiptItem { get; set; }
        public string ReceiptItemUnit { get; set; }
        public Nullable<decimal> ReceiptQuantity { get; set; }
        public Nullable<decimal> QtyLoss { get; set; }
        public string Process { get; set; }
        public string Status { get; set; }
        public Nullable<int> recordsFiltered { get; set; }
        public Nullable<int> totalRecords { get; set; }
    }
}
