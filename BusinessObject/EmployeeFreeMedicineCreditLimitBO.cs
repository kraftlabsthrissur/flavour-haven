using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class EmployeeFreeMedicineCreditLimitBO
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int LocationID { get; set; }
        public int EmployeeCategoryID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCategory { get; set; }
        public string EmployeeCode{ get; set; }
        public decimal BalAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal CreditLimit { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }

    }
}
