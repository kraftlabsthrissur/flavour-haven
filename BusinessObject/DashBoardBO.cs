using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{

    public class DateWiseSalesBO
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    public class CategoryWiseStockBO
    {
        public string LocationHead { get; set; }
        public string BusinessCategory { get; set; }
        public decimal Stock { get; set; }
    }

    public class ProductionQtyBO
    {
        public string LocationHead { get; set; }
        public decimal TotalProduction { get; set; }
    }

    public class ProductionQtyMonthWiseBO
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }
        public decimal TotalProductionInTKY { get; set; }
        public decimal TotalProductionInPOL { get; set; }
        public decimal TotalProductionInCHU { get; set; }
    }

    public class ProductionProgressBO
    {
        public string ItemName { get; set; }
        public decimal StandardOutput { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string BatchNo { get; set; }
    }

    public class SalesSummaryBO
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

        public string Location { get; set; }
        public string LocationHead { get; set; }
        public string Category { get; set; }

        public string XaxisName { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }
    }
    
}