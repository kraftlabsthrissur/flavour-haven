using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.User.Models
{
    public class DashboardModel
    {
        public List<SalesByLocationModel> LocationList { get; set; }
        public int SalesCategoryID { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList BatchTypeList { get; set; }
        public string BatchType { get; set; }

    }

    public class SalesSummaryModel
    {
        public List<XaxisNameModel> XaxisNames { get; set; }
        public List<SalesSummaryValueModel> SalesSummaryValues { get; set; }

    }

    public class XaxisNameModel
    {
        public string Name { get; set; }
    }

    public class SalesSummaryValueModel
    {
        public decimal BudgetValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal BudgetInKgValue { get; set; }
        public decimal ActualSalesInKgValue { get; set; }
        public decimal BudgetGrossRevenue { get; set; }
        public decimal ActualSalesRevenue { get; set; }
        public decimal PrevoiusYearSalesInKgValue { get; set; }
        public decimal PrevoiusYearGrossRevenue { get; set; }
        public decimal PrevoiusYearSalesValue { get; set; }
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

    public class ProductionSummaryModel
    {
      public List<ProductionQtyModel> TotalProductionQty { get; set; }
      public List<MonthWiseProductionQtyModel> MonthWiseProductionQty { get; set; }
      public List<XaxisNameModel> XaxisNames { get; set; }
      public List<ProductionProgressModel> ProductionInProgress { get; set; }
    }

    public class ProductionQtyModel
    {
        public decimal TotalProduction { get; set; }
    }

    public class MonthWiseProductionQtyModel
    {
        public decimal TotalProductionInTKY { get; set; }
        public decimal TotalProductionInPOL { get; set; }
        public decimal TotalProductionInCHU { get; set; }
    }
    public class ProductionProgressModel
    {
        public string ItemName { get; set; }
        public decimal StandardOutput { get; set; }
        public string ExpectedDate { get; set; }
        public string BatchNo { get; set; }
    }

    public class SalesByLocationModel
    {
        public string LocationCode { get; set; }
        public string LocationType { get; set; }
        public string LocationName { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousMonthAmount { get; set; }
        public decimal Budget { get; set; }
    }
}