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
    
    public partial class SpRptDayAndMonthWiseProduction_Result
    {
        public string PackingCode { get; set; }
        public string PackingName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string BatchNo { get; set; }
        public string Batchtype { get; set; }
        public Nullable<decimal> QtyInNo { get; set; }
        public decimal QtyInKg { get; set; }
        public Nullable<decimal> BasicPrice { get; set; }
        public decimal Value { get; set; }
        public string SalesCategory { get; set; }
        public string ProductionCategory { get; set; }
        public string UOM { get; set; }
    }
}
