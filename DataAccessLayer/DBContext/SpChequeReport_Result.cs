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
    
    public partial class SpChequeReport_Result
    {
        public string Location { get; set; }
        public string ReceiptNo { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string BAnkName { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string ChequeStatus { get; set; }
        public string Remarks { get; set; }
    }
}
