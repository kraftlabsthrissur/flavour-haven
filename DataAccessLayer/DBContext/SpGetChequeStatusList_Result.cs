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
    
    public partial class SpGetChequeStatusList_Result
    {
        public string TransNo { get; set; }
        public int ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string InstrumentStatus { get; set; }
        public Nullable<System.DateTime> FromReceiptDate { get; set; }
        public Nullable<System.DateTime> ToReceiptDate { get; set; }
        public bool IsDraft { get; set; }
        public string CustomerName { get; set; }
    }
}
