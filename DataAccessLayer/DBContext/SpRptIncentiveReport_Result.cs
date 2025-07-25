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
    
    public partial class SpRptIncentiveReport_Result
    {
        public string PartyName { get; set; }
        public int PartyID { get; set; }
        public Nullable<decimal> TotalClassicalTarget { get; set; }
        public Nullable<decimal> TotalPatentTarget { get; set; }
        public Nullable<decimal> TotalTarget { get; set; }
        public Nullable<decimal> TotalAchievedClassicalTarget { get; set; }
        public Nullable<decimal> TotalAchievedPatentTarget { get; set; }
        public Nullable<decimal> TotalAchievedTarget { get; set; }
        public Nullable<decimal> TotalAchievedClassicalPercent { get; set; }
        public Nullable<decimal> TotalAchievedPatentPercent { get; set; }
        public Nullable<decimal> TotalAchievedPercent { get; set; }
        public Nullable<decimal> ClassicalIncentiveAmount { get; set; }
        public Nullable<decimal> PatentIncentiveAmount { get; set; }
        public Nullable<decimal> TotalIncentiveAmount { get; set; }
        public Nullable<decimal> IncentiveAbove105percent { get; set; }
        public Nullable<decimal> TotalEligableAmount { get; set; }
    }
}
