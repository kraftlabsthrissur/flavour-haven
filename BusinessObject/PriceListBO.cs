using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PriceListBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<PriceListItemBO> Items { get; set; }
    }

    public class PriceListItemBO
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal MRP { get; set; }
        public decimal LoosePrice { get; set; }
        public decimal ISKMRP { get; set; }
        public decimal ISKLoosePrice { get; set; }
        public decimal OSKMRP { get; set; }
        public decimal OSKLoosePrice { get; set; }
        public decimal ExportMRP { get; set; }
        public decimal ExportLoosePrice { get; set; }
    }
}
