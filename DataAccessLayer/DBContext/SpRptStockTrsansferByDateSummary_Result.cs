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
    
    public partial class SpRptStockTrsansferByDateSummary_Result
    {
        public string StockRequestNo { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public string IssueNo { get; set; }
        public Nullable<System.DateTime> StockIssueDate { get; set; }
        public string FromLocation { get; set; }
        public string FromPremises { get; set; }
        public string ToLocation { get; set; }
        public string ToPremises { get; set; }
        public string ItemCategory { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public Nullable<decimal> RequestedQty { get; set; }
        public Nullable<decimal> IssueQty { get; set; }
        public Nullable<decimal> ReceiptedQty { get; set; }
        public Nullable<decimal> UndeliveredQty { get; set; }
        public string Remarks { get; set; }
    }
}
