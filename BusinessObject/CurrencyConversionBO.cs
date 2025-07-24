using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CurrencyConversionBO
    {

        public int? Id { get; set; }
        public int BaseCurrencyID { get; set; }
        public string BaseCurrencyCode { get; set; }
        public int ConversionCurrencyID { get; set; }
        public string ConversionCurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal InverseExchangeRate { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public bool IsActive { get; set; }

    }
}
