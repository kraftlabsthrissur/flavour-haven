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
    
    public partial class SpGetJobWorkReceipt_Result
    {
        public string TransNO { get; set; }
        public System.DateTime TransDate { get; set; }
        public string Supplier { get; set; }
        public Nullable<bool> isDraft { get; set; }
        public int ID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> IssueID { get; set; }
        public string IssueNo { get; set; }
    }
}
