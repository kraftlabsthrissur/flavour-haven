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
    
    public partial class SpGetUnMappedValuesToSLA_Result
    {
        public int ID { get; set; }
        public string TrnType { get; set; }
        public string KeyValue { get; set; }
        public decimal Amount { get; set; }
        public string Event { get; set; }
        public Nullable<int> TransID { get; set; }
        public Nullable<bool> IsSLAMapped { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string TableName { get; set; }
        public Nullable<int> TablePrimaryID { get; set; }
        public string Remarks { get; set; }
        public string Supplier { get; set; }
        public string DocumentTable { get; set; }
        public string DocumentNo { get; set; }
        public string Item { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }
    }
}
