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
    
    public partial class SpRptCostingSummary_Result
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> CostUpdatedDate { get; set; }
        public string Location { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> OpeningStock { get; set; }
        public Nullable<decimal> OpeningRate { get; set; }
        public Nullable<decimal> OpeningStockValue { get; set; }
        public Nullable<decimal> ReceivedStock { get; set; }
        public Nullable<decimal> ReceivedRate { get; set; }
        public Nullable<decimal> ReceivedStockValue { get; set; }
        public Nullable<decimal> IssuedStock { get; set; }
        public Nullable<decimal> IssueRate { get; set; }
        public Nullable<decimal> IssueStockValue { get; set; }
        public Nullable<decimal> ClosingStock { get; set; }
        public Nullable<decimal> ClosingUnitCost { get; set; }
        public Nullable<decimal> ClosingStockValue { get; set; }
        public Nullable<decimal> LastPurchasePrice { get; set; }
        public Nullable<decimal> TransferPrice { get; set; }
    }
}
