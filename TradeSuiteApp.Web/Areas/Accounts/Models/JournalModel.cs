using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class JournalModel
    {
        public int ID { get; set; }       
        public string Date { get; set; }

        public string Currency { get; set; }
        public string VoucherNo { get; set; }
        
        public int CreditAccountHeadID { get; set; }
        public int DebitAccountHeadID { get; set; }
        public string CreditAccountCode { get; set; }
        public string DebitAccountCode { get; set; }
        public string CreditAccountName { get; set; }
        public string DebitAccountName { get; set; }       
        public decimal CreditAmount { get; set; }      
        public decimal DebitAmount { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public string Status { get; set; }
        public decimal TotalDebitAmount { get; set; }
        public int CurrencyID { get; set; }
        public int DebitCurrencyID { get; set; }
        public int CreditCurrencyID { get; set; }
        public bool IsDraft { get; set; }   

        public int DepartmentID { get; set; }
        public int LocationID { get; set; }
        public int InterCompanyID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }

        public SelectList DepartmentList { get; set; }       
        public SelectList LocationList { get; set; }
        public SelectList InterCompanyList { get; set; }   
        public SelectList EmployeeList { get; set; }          
        public SelectList ProjectList { get; set; }
        public SelectList CurrencyLists { get; set; }

        public List<JournalItemModel> Items { get; set; }
        public string FirstOpenDate { get; set; }
        public decimal DebitExchangeRate { get; set; }
        public decimal CreditExchangeRate { get; set; }
        public string normalclass { get; set; }
        public string largeclass { get; set; }
        public int DecimalPlaces { get; set; }
    }
    public class JournalItemModel
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public int CreditAccountHeadID { get; set; }
        public int DebitAccountHeadID { get; set; }
        public string CreditAccountCode { get; set; }
        public string DebitAccountCode { get; set; }
        public string CreditAccountName { get; set; }
        public string DebitAccountName { get; set; }
        public decimal CreditAmount { get; set; }      
        public decimal DebitAmount { get; set; }     
        public string Remarks { get; set; }

        public int DepartmentID { get; set; }
        public int JournalLocationID { get; set; }
        public int InterCompanyID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }

        public string Location { get; set; }
        public string InterCompany { get; set; }
        public string Department { get; set; }
        public string Employee { get; set; }
        public string Project { get; set; }

        public int DebitCurrencyID { get; set; }
        public int CreditCurrencyID { get; set; }
        public int LocalCurrencyID { get; set; }
        public string DebitCurrency{ get; set; }
        public string CreditCurrency { get; set; }
        public string LocalCurrency { get; set; }
        public decimal DebitExchangeRate { get; set; }
        public decimal CreditExchangeRate { get; set; }
        public decimal LocalCreditAmount { get; set; }
        public decimal LocalDebitAmount { get; set; }
    }
}