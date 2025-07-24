using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PrivilegeCardBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DiscountCategoryID { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int ValidDays { get; set; }
        public decimal Rate { get; set; }
    }
}
