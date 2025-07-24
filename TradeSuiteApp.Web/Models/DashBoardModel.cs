using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Models
{
    public class DashBoardModel
    {
        public List<XaxisNameModel> XaxisNames { get; set; }
        public List<ValueModel> Values { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public int BatchTypeID { get; set; }
        public int SalesCategoryID { get; set; }
        public List<SalesByLocationModel> LocationList { get; set; }
    }
    public class XaxisNameModel
    {
        public string Name { get; set; }
    }
    public class ValueModel
    {
        public decimal BudgetValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal BudgetInKgValue { get; set; }
        public decimal ActualSalesInKgValue { get; set; }
        public decimal BudgetGrossRevenue { get; set; }
        public decimal ActualSalesRevenue { get; set; }
        public decimal PrevoiusYearSalesInKgValue { get; set; }
        public decimal PrevoiusYearGrossRevenue { get; set; }
        public decimal PrevoiusYearSalesRevenue { get; set; }

        public decimal TotalProductionInTKY { get; set; }
        public decimal TotalProductionInPOL { get; set; }
        public decimal TotalProductionInCHU { get; set; }

        public string LocationHead { get; set; }
        public string Month { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public decimal TotalProduction { get; set; }
    }

    public class SalesByLocationModel
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal Amount { get; set; }
    }

    public class DateWiseSalesModel
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string DateString
        {
            get
            {
                return Date.ToString("yyyy-MM-dd");
            }

            set
            {
                DateString = value;
            }
        }
    }

    public class CategoryWiseStockModel
    {
        public string LocationHead { get; set; }
        public string BusinessCategory { get; set; }
        public decimal Stock { get; set; }
    }

    public class LocationHeadWiseSalesModel
    {
        public List<XaxisNameModel> XaxisNames { get; set; }
        public List<ValueModel> Values { get; set; }

    }
    public class ProductionQtyMonthWiseModel
    {
        public List<XaxisNameModel> XaxisNames { get; set; }
        public List<ValueModel> Values { get; set; }
    }

}