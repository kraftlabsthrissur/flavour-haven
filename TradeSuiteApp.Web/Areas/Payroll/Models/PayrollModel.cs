using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Payroll.Models
{
    public class PayrollModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int LocationHeadID { get; set; }
        public string LocationHeadName { get; set; }
        public int IncometaxComputeID { get; set; }
        public string IncometaxCompute { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int RunPayrollID { get; set; }
        public string RunPayroll { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<LocationBO> LocationList { get; set; }
        public List<LocationHeadBO> LocationHeadList { get; set; }
        public SelectList IncometaxComputeList { get; set; }
        public SelectList RunPayrollList { get; set; }
    }
}