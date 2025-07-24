using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TradeSuiteApp.Web.Areas.Stock.Models
{
    public class RateAdjustmentModel
    {
        public int ID { get; set; }
        public String Date { get; set; }
        public string TransNo { get; set; }
        public int ItemCategoryID { get; set; }
        public SelectList CategoryList { get; set; }
        public string Status { get; set; }
        public bool IsDraft { get; set; }

        public List<RateAdjustmentItemModel> Items { get; set; }
    }
    public class RateAdjustmentItemModel
    {
        public int ID { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public decimal? SystemStockQty { get; set; }
        public decimal? SystemAvgCost { get; set; }
        public decimal? ActualAvgCost { get; set; }
        public decimal? DifferenceInAvgCost { get; set; }
        public decimal? SystemStockValue { get; set; }
        public decimal? ActualStockValue { get; set; }
        public decimal? DifferenceInStockValue { get; set; }
        public string EffectDate { get; set; }
        public string Remark { get; set; }
    }
}