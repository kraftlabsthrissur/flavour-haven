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
    
    public partial class SpGetApprovals_Result
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> TransID { get; set; }
        public string TransNo { get; set; }
        public Nullable<int> ApprovalFlowID { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string Status { get; set; }
        public Nullable<int> LastActionUserID { get; set; }
        public int NextActionUserID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ApprovalTypeID { get; set; }
        public string UserName { get; set; }
        public string Requirement { get; set; }
    }
}
