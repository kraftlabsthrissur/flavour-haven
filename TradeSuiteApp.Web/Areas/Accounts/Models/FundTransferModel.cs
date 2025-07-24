using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class FundTransferModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string ModeOfPayment { get; set; }
        public string InstrumentNumber { get; set; }
        public string InstrumentDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public bool IsDraft { get; set; }

        public int ModeOfPaymentID { get; set; }
        public int FromLocationID { get; set; }
        public int ToLocationID { get; set; }
        public int FromBankID { get; set; }
        public int ToBankID { get; set; }
        public string BankName { get; set; }
        public string FromLocation{ get; set; }
        public string ToLocation{ get; set; }

        public SelectList ModeOfPaymentList { get; set; }      
        public SelectList FromLocationList { get; set; }      
        public SelectList ToLocationList { get; set; }
        public SelectList ToBankList { get; set; }
       
        public List<FundTransferTransModel> Items { get; set; }
        public List<TreasuryModel> FromBankList { get; set; }

    }

    public class TressuryModel
    {
        public int ID { get; set; }
        public string BankName { get; set; }
        public decimal CreditBalance { get; set; }

    }
    public class FundTransferTransModel
    {
        public int ID { get; set; }
        public int FundTransferID { get; set; }
        public int FromLocationID { get; set; }
        public int ToLocationID { get; set; }
        public int FromBankID { get; set; }
        public int ToBankID { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string FromBank { get; set; }
        public string ToBank { get; set; }
        public decimal Amount { get; set; }
        public string ModeOfPayment { get; set; }
        public int ModeOfPaymentID { get; set; }
        public string InstrumentNumber { get; set; }
        public String InstrumentDate { get; set; }
        public String Remarks { get; set; }
        public decimal CreditBalance { get; set; }
    }
}