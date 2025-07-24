using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class BankExpensesModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string Date { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionDate { get; set; }
        public int AccountHeadID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsDraft { get; set; }
        public string Remarks { get; set; }

        public int BankID { get; set; }
        public string Bank { get; set; }
        public SelectList BankList { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public SelectList ItemList { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string ModeOfPayment { get; set; }
        public SelectList ModeOfPaymentList { get; set; }
        public string Status { get; set; }
        public string ReferenceNo { get; set; }
        public List<BankExpensesItemModel> Items { get; set; }


    }
    public class BankExpensesItemModel
    {
        public int ID { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public int AccountHeadID { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string ModeOfPayment { get; set; }
        public string ReferenceNo { get; set; }

    }
}