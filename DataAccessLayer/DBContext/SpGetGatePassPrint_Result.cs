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
    
    public partial class SpGetGatePassPrint_Result
    {
        public string TransNo { get; set; }
        public string SalesInvoiceNo { get; set; }
        public string Salesman { get; set; }
        public string VehicleNo { get; set; }
        public Nullable<System.DateTime> DespatchDateTime { get; set; }
        public string Time { get; set; }
        public string DriverName { get; set; }
        public Nullable<decimal> StartingKilometer { get; set; }
        public string IssuedBy { get; set; }
        public Nullable<int> BagCount { get; set; }
        public Nullable<int> CanCount { get; set; }
        public Nullable<int> BoxCount { get; set; }
        public Nullable<decimal> TotalInvoiceAmount { get; set; }
        public string Helper { get; set; }
    }
}
