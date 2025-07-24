using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DiscountCategoryBO
    {
        public int ID { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string DiscountType { get; set; }
        public int Days { get; set; }
        public string DiscountCategory { get; set; }
    }
}
