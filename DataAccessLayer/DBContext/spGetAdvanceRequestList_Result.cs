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
    
    public partial class spGetAdvanceRequestList_Result
    {
        public int ID { get; set; }
        public string RequestNo { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Status { get; set; }
        public Nullable<int> recordsFiltered { get; set; }
        public Nullable<int> totalRecords { get; set; }
    }
}
