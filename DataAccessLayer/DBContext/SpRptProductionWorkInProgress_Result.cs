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
    
    public partial class SpRptProductionWorkInProgress_Result
    {
        public string ProductGroup { get; set; }
        public string ProductionCategory { get; set; }
        public string SalesCategory { get; set; }
        public Nullable<decimal> StandardBatchSize { get; set; }
        public Nullable<decimal> ActualBatchSize { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<int> ProcessDays { get; set; }
        public Nullable<System.DateTime> ExpectedEndDate { get; set; }
        public Nullable<decimal> StandardOutput { get; set; }
        public Nullable<decimal> ActualOutput { get; set; }
        public Nullable<decimal> Cogs { get; set; }
        public string Staus { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string Remarks { get; set; }
    }
}
