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
    
    public partial class SpGetPaymentReturnVoucherDetails_Result
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
        public string SupplierName { get; set; }
        public int SupplierID { get; set; }
        public string PaymentTypeName { get; set; }
        public int PaymentTypeID { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public string BankReferenceNumber { get; set; }
        public Nullable<bool> IsDraft { get; set; }
        public string SupplierBankName { get; set; }
        public string SupplierBankACNo { get; set; }
        public string SupplierIFSCNo { get; set; }
    }
}
