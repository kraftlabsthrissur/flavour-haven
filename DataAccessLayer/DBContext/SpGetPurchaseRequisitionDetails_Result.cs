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
    
    public partial class SpGetPurchaseRequisitionDetails_Result
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string RequisitionNo { get; set; }
        public bool QuotationProcessed { get; set; }
        public bool FullyOrdered { get; set; }
        public Nullable<int> FromDeptID { get; set; }
        public string FromDepartment { get; set; }
        public Nullable<int> ToDeptID { get; set; }
        public string ToDepartment { get; set; }
        public Nullable<bool> IsDraft { get; set; }
        public string PurchaseRequisitedCustomer { get; set; }
        public string RequisitedCustomerAddress { get; set; }
        public string RequisitedPhoneNumber1 { get; set; }
        public string RequisitedPhoneNumber2 { get; set; }
        public string Remarks { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public bool Cancelled { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<int> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int FinYear { get; set; }
        public int LocationID { get; set; }
        public int ApplicationID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
    }
}
