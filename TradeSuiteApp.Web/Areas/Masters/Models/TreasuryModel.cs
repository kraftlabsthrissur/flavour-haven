using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class TreasuryModel
    {
        public int? ID { get; set; }
        public string Type { get; set; }
        public string BankCode { get; set; }
        public string AccountCode { get; set; }
        public string BankName { get; set; }
        public string AliasName { get; set; }
        public string CoBranchName { get; set; }
        public string BankBranchName { get; set; }
        public string AccountType1 { get; set; }
        public string AccountType2 { get; set; }
        public string AccountNo { get; set; }
        public string IFSC { get; set; }
        public int LocationMappingID { get; set; }
        public string LocationMapping { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string remarks { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> ApplicationID { get; set; }

        public SelectList TypeList { get; set; }
        public string PaymentType { get; set; }
        public string PaymentID { get; set; }
        public SelectList LocationList { get; set; }
        public string Code { get; set; }
        public string AccountName { get; set; }
        public bool IsPayment { get; set; }
        public bool IsReceipt { get; set; }

        public decimal CreditBalance { get; set; }
        public decimal OpeningAmount { get; set; }
        public string ReceiverBankName { get; set; }
        public string BankInstrumentNumber { get; set; }
        public string checqueDate { get; set; }
        public int ReceiverBankID { get; set; }
    }
}