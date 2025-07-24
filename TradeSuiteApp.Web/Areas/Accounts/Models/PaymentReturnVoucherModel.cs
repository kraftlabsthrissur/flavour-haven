using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class PaymentReturnVoucherModel
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public int SupplierCategoryID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int DebitNoteID { get; set; }
        public string DebitNote { get; set; }
        public string DebitAccountCode { get; set; }
        public string TransNo { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string BankReferenceNumber { get; set; }
        public string AccountHead { get; set; }
        public int AccountHeadID { get; set; }
        public string Remarks { get; set; }
        public SelectList PaymentTypeList { get; set; }
        public SelectList BankList { get; set; }
        public List<PaymentReturnVoucherItemModel> Items { get; set; }
        public List<DebitNoteModel> DebitNoteList { get; set; }
    }
    public class PaymentReturnVoucherItemModel
    {
        public int ID { get; set; }
        public string SupplierName { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}