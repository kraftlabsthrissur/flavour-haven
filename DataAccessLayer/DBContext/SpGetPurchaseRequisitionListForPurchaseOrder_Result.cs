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
    
    public partial class SpGetPurchaseRequisitionListForPurchaseOrder_Result
    {
        public int ID { get; set; }
        public string RequisitionNo { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string RequisitedPhoneNumber1 { get; set; }
        public string RequisitedPhoneNumber2 { get; set; }
        public string RequisitedCustomerAddress { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> recordsFiltered { get; set; }
        public Nullable<int> totalRecords { get; set; }
        public string SupplierName { get; set; }
    }
}
