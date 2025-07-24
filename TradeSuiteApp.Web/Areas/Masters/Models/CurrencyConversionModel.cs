using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class CurrencyConversionModel
    {

        public int? Id { get; set; }       
        public string BaseCurrencyCode { get; set; }
        public int BaseCurrencyID { get; set; }
        public string ConversionCurrencyCode { get; set; }
        public int ConversionCurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal InverseExchangeRate { get; set; }
        public string Description { get; set; }
        public string FromDate { get; set; }
        public bool IsActive { get; set; }
    }
}