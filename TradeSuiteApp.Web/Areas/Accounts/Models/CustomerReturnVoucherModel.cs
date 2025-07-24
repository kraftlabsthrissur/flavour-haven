using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class CustomerReturnVoucherModel
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public int CustomerCategoryID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string BankReferenceNumber { get; set; }
        public string Remarks { get; set; }
        public SelectList PaymentTypeList { get; set; }
        public SelectList BankList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public List<CustomerReturnVoucherItemModel> Items { get; set; }
    }
    public class CustomerReturnVoucherItemModel
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }
}