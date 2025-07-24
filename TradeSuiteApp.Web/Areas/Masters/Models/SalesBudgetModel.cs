using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SalesBudgetModel
    {
        public int ID { get; set; }
        public int LocationID { get; set; }
        public int MonthID { get; set; }
        public int BatchTypeID { get; set; }
        public string Date { get; set; }
        public SelectList MonthList { get; set; }
        public SelectList BranchList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public List<SalesBudgetItemModel> Items { get; set; }
    }

    public class SalesBudgetItemModel
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string SalesCategory { get; set; }
        public string Month { get; set; }
        public string BatchType { get; set; }
        public string Branch { get; set; }
        public int BudgetQtyInNos { get; set; }
        public int BudgetQtyInKgs { get; set; }
        public decimal BudgetGrossRevenue { get; set; }
        public int ForecastsQtyInNos { get; set; }
        public int ForecastsQtyInKgs { get; set; }
        public decimal ForecastsGrossRevenue { get; set; }
        public int ActualQtyInNos { get; set; }
        public int ActualQtyInKgs { get; set; }
        public decimal ActualGrossRevenue { get; set; }

    }
}