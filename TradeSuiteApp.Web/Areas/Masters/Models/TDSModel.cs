using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class TDSModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemAccountCategory { get; set; }
        public string ITSection { get; set; }
        public decimal TDSRate { get; set; }
        public string CompanyType { get; set; }
        public string ExpenseType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Remarks { get; set; }
    }
}