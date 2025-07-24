using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DiscountAndOfferBO
    {
        public decimal DiscountPercentage { get; set; }
        public List<OfferBO> OfferDetails { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
    }

    public class OfferBO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public decimal OfferQty { get; set; }
        public int UnitID { get; set; }
    }
}
