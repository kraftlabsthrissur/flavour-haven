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
    
    public partial class SpRptProductionInputByBatchSummary_Result
    {
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> BatchDate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> StdInputQty { get; set; }
        public Nullable<decimal> ActualQty { get; set; }
        public Nullable<decimal> TotalStdRate { get; set; }
        public Nullable<decimal> ActualRateMaterials { get; set; }
        public Nullable<decimal> ActualRateTotal { get; set; }
        public Nullable<decimal> StdValueMaterials { get; set; }
        public Nullable<decimal> StdValueTotal { get; set; }
        public Nullable<decimal> ActualValueTotal { get; set; }
        public Nullable<decimal> TotalVariance { get; set; }
        public string BatchStatus { get; set; }
        public string ProductionGroupName { get; set; }
    }
}
