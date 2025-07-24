using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
 public   class RateAdjustmentBO
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string TransNo { get; set; }
           public string Status { get; set; }
        public bool IsDraft { get; set; }

        public List<RateAdjustmentItemBO> Items { get; set; }
    }
    public class RateAdjustmentItemBO
    {
        public int ID { get; set; }
        public int? ItemID { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public decimal? SystemStockQty { get; set; }
        public decimal? SystemAvgCost { get; set; }
        public decimal? ActualAvgCost { get; set; }
        public decimal? DifferenceInAvgCost { get; set; }
        public decimal? SystemStockValue { get; set; }
        public decimal? ActualStockValue { get; set; }
        public decimal? DifferenceInStockValue { get; set; }
        public DateTime EffectDate { get; set; }
        public string Remark { get; set; }
        
    }
}
