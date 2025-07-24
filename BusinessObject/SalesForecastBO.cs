using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class SalesForecastBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public string Month { get; set; }
        public bool IsFinalize { get; set; }
    }

    public class SalesForecastItemBO
    {
        public int SalesForecastID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set;}
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public decimal ComputedForecast { get; set; }
        public decimal FinalForecast { get; set; }
        public decimal ActualSales { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public decimal ComputedForecastInKg { get; set; }
    }
}
