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
    
    public partial class SpGetApprovalFlow_Result
    {
        public int ID { get; set; }
        public string QueueName { get; set; }
        public string FlowName { get; set; }
        public string Code { get; set; }
        public Nullable<int> DeptID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<decimal> AmountAbove { get; set; }
        public Nullable<decimal> AmountBelow { get; set; }
        public Nullable<int> KeyValue1 { get; set; }
        public string KeyValue1Name { get; set; }
        public Nullable<int> KeyValue2 { get; set; }
        public string KeyValue2Name { get; set; }
        public Nullable<int> KeyValue3 { get; set; }
        public string KeyValue3Name { get; set; }
        public Nullable<int> KeyValue4 { get; set; }
        public string KeyValue4Name { get; set; }
        public string LocationName { get; set; }
    }
}
