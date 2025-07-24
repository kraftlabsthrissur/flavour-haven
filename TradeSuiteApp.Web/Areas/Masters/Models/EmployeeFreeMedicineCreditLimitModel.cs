using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class EmployeeFreeMedicineCreditLimitModel
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int LocationID { get; set; }
        public int EmployeeCategoryID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCategory { get; set; }
        public string EmployeeCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Amount { get; set; }
        public decimal BalAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal CreditLimit { get; set; }


        public SelectList LocationList { get; set; }
        public SelectList EmpCategoryList { get; set; }

        public List<EmployeeFreeMedicineCreditLimitModel> Items { get; set; }

    }
}