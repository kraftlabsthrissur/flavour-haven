using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class BankExpensesBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime Date { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string AccountCode { get; set; }
        public int AccountHeadID { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string InstrumentNumber { get; set; }
        public DateTime InstrumentDate { get; set; }
        public string Remarks { get; set; }
        public bool IsDraft { get; set; }
        public decimal TotalAmount { get; set; }
        public int BankID { get; set; }
        public string Bank { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string ModeOfPayment { get; set; }
        public string ItemName { get; set; }
        public List<BankExpensesTransBO> Items { get; set; }
    }
    public class BankExpensesTransBO
    {
        public int ID { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string AccountCode { get; set; }
        public int AccountHeadID { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string ModeOfPayment { get; set; }
        public string Remarks { get; set; }
        public string ReferenceNo { get; set; }
    }


}

