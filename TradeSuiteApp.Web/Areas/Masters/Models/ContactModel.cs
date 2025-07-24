using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class ContactModel
    {
        public int ID { get; set; } 
        public string Firstname { get; set; }
        public string Lastname { get; set; }
       
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string AlternateNo { get; set; }
        public string Designation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Company { get; set; }
        public int SchemeID { get; set; }
        public int LocationStateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int PriceListID { get; set; }
        public int StateID { get; set; }
        public int CustomerID { get; set; }

        public bool CheckStock { get; set; }
        public decimal MinCreditLimit { get; set; }
        public decimal CashDiscountPercentage { get; set; }
        public int StoreID { get; set; }

        public string CustomerCategory { get; set; }

        public decimal CreditAmount { get; set; }
        public decimal MaxCreditLimit { get; set; }

        public int FreightTax { get; set; }

        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }

        public decimal CurrencyExchangeRate { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }

        public int TaxTypeID { get; set; }

        public int CountryID { get; set; }

        public bool IsActive { get; set; }







    }
  
    

  


}
