using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CurrencyBO
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
        public int CountryID { get; set; }
        public int DecimalPlaces { get; set; }
        public string MinimumCurrency { get; set; }
        public string MinimumCurrencyCode { get; set; }
        public string CurrencyCode { get; set; }

    }
}
