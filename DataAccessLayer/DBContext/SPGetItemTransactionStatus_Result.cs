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
    
    public partial class SPGetItemTransactionStatus_Result
    {
        public decimal Stock { get; set; }
        public decimal QtyUnderQC { get; set; }
        public decimal LastPR { get; set; }
        public decimal LowestPR { get; set; }
        public decimal PendingOrderQty { get; set; }
        public Nullable<int> ItemID { get; set; }
    }
}
