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
    
    public partial class SpGetPackingScheduleItems_Result
    {
        public int PackingScheduleID { get; set; }
        public int ItemID { get; set; }
        public decimal RequiredQty { get; set; }
        public System.DateTime RequiredDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> QtyMet { get; set; }
        public Nullable<int> ProductDefinitionTransID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<decimal> StdQtyForStdBatch { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string ItemCode { get; set; }
        public Nullable<decimal> Variance { get; set; }
        public decimal StdQtyForActualBatch { get; set; }
        public Nullable<decimal> AvailableStock { get; set; }
    }
}
