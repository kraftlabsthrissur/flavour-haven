using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.SNOP.Models
{
    public class SalesForecastModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public string Month { get; set; }
        public bool IsFinalize { get; set; }
        public List<SalesForecastItemModel> Items { get; set; }
    }

    public class SalesForecastItemModel
    {
        public int SalesForecastID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string Location { get; set; }
        public decimal ComputedForecast { get; set; }
        public decimal FinalForecast { get; set; }
        public decimal ActualSales { get; set; }
        public decimal Price { get; set; }
        public decimal ComputedForecastInKg { get; set; }
    }
}