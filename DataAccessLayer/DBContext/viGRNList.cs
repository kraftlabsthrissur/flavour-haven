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
    using System.Collections.Generic;
    
    public partial class viGRNList
    {
        public string PONo { get; set; }
        public System.DateTime PODate { get; set; }
        public string GRNNo { get; set; }
        public System.DateTime GRNDate { get; set; }
        public string DocNo { get; set; }
        public string MalayalamName { get; set; }
        public string SanskritName { get; set; }
        public string BotanicalName { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal AcceptedQty { get; set; }
        public Nullable<decimal> QtyToBeReceived { get; set; }
        public int ID { get; set; }
    }
}
