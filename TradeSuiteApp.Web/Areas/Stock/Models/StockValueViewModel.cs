using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class StockValueViewModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal OpeningValue { get; set; }
        public decimal OpeningRate { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal ClosingRate { get; set; }
        public decimal ClosingValue { get; set; }
        public decimal StockIn { get; set; }
        public decimal NetStockValueIn { get; set; }
        public decimal NetStockRateIn { get; set; }
        public decimal IssueStock { get; set; }
        public decimal IssueValue { get; set; }
        public decimal IssueRate { get; set; }
        public decimal LastUpdatedRate { get; set; }
        public string LastUpdatedDate { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string Status { get; set; }

    }
}