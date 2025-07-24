using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class FundTransferReceiptModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public decimal Amount { get; set; }
        public int FromLocationID { get; set; }
        public int FromBankID { get; set; }
        public int ToLocationID { get; set; }
        public int ToBankID { get; set; }
        public string FromLocationName { get; set; }
        public string ToLocationName { get; set; }
        public string FromBankName { get; set; }
        public string ToBankName { get; set; }
        public string Payment { get; set; }
        public int ModeOfPayment { get; set; }
        public DateTime InstrumentDate { get; set; }
        public string InstrumentNumber { get; set; }
        public string Remarks { get; set; }
        public SelectList FromLocationList { get; set; }
        public SelectList ToLocationList { get; set; }
        public SelectList ToBankList { get; set; }
        public SelectList FromBankList { get; set; }
        public List<FundTransferItemModel> Items { get; set; }
    }

    public class FundTransferItemModel
    {
        public int IssueTransID { get; set; }
        public int FromLocationID { get; set; }
        public int FromBankID { get; set; }
        public int ToLocationID { get; set; }
        public int ToBankID { get; set; }
        public int ModeOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string FromLocationName { get; set; }
        public string ToLocationName { get; set; }
        public string Payment { get; set; }
        public string FromBankName { get; set; }
        public string ToBankName { get; set; }
    }
}