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
    
    public partial class SpGetStockListForAPI_Result
    {
        public int ID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public int BatchID { get; set; }
        public string BatchNo { get; set; }
        public int WarehouseID { get; set; }
        public string TransactionType { get; set; }
        public decimal MRP { get; set; }
        public int SalesUnitID { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public decimal GSTPercentage { get; set; }
        public Nullable<decimal> Issue { get; set; }
        public Nullable<decimal> Receipt { get; set; }
        public Nullable<decimal> Value { get; set; }
    }
}
