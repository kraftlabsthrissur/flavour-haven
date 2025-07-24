using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class TurnOverDiscountsBO
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public decimal TurnOverDiscount { get; set; }
        public List<DiscountItemBO> Items { get; set; }
    }

    public class DiscountItemBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal TurnOverDiscount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Location { get; set; }
        public string Month { get; set; }
    }
}
