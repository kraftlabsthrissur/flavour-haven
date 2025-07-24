using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SalesBudgetBO
    {
        public int ID { get; set; }
        public List<SalesBudgetItemBO> Items { get; set; }
    }
    public class SalesBudgetItemBO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string SalesCategory { get; set; }
        public string Month { get; set; }
        public string BatchType { get; set; }
        public string Branch { get; set; }
        public decimal BudgetQtyInNos { get; set; }
        public decimal BudgetQtyInKgs { get; set; }
        public decimal BudgetGrossRevenue { get; set; }
        public decimal ForecastsQtyInNos { get; set; }
        public decimal ForecastsQtyInKgs { get; set; }
        public decimal ForecastsGrossRevenue { get; set; }
        public int ActualQtyInNos { get; set; }
        public int ActualQtyInKgs { get; set; }
        public decimal ActualGrossRevenue { get; set; }

    }
}
