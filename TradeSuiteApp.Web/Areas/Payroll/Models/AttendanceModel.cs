using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Payroll.Models
{
    public class AttendanceModel
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeCategoryID { get; set; }
        public string EmployeeCategoryName { get; set; }
        public int PayrollCategoryID { get; set; }
        public string PayrollCategory { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int MonthID { get; set; }
        public string Month { get; set; }
        public int TotalLeave { get; set; }
        public int BalanceLeave { get; set; }

        public SelectList Departmentlist { get; set; }
        public SelectList PayrollCategoryList { get; set; }
        public SelectList EmployeeCategoryList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList MonthList { get; set; }
        public SelectList EmployeeList { get; set; }
    }
    public class MonthModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}