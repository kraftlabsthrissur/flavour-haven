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
    
    public partial class SpGetCounterSales_Result
    {
        public long ID { get; set; }
        public string TransNo { get; set; }
        public Nullable<System.DateTime> Transdate { get; set; }
        public Nullable<decimal> NetAmountTotal { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public Nullable<decimal> PackingPrice { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public string Warehouse { get; set; }
        public Nullable<bool> IsDraft { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
        public Nullable<decimal> RoundOff { get; set; }
        public int PaymentModeID { get; set; }
        public decimal TotalAmountReceived { get; set; }
        public Nullable<decimal> SGSTAmount { get; set; }
        public Nullable<decimal> CGSTAmount { get; set; }
        public Nullable<decimal> IGSTAmount { get; set; }
        public decimal BalanceToBePaid { get; set; }
        public decimal CessAmount { get; set; }
        public int SalesTypeID { get; set; }
        public string SalesType { get; set; }
        public int EmployeeID { get; set; }
        public string Employee { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public Nullable<decimal> BalAmount { get; set; }
        public string Type { get; set; }
        public string PartyName { get; set; }
    }
}
