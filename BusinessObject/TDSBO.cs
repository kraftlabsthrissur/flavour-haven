using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class TDSBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemAccountCategory { get; set; }
        public string ITSection { get; set; }
        public decimal TDSRate { get; set; }
        public string Rate { get; set; }
        public string CompanyType { get; set; }
        public string ExpenseType { get; set; }    
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
    }
}
