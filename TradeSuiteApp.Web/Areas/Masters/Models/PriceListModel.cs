using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class PriceListModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<PriceListItemModel> Items { get; set; }
        public int ItemID { get; set; }
        public string Unit { get; set; }
      
        public string ItemName { get; set; }
    }

    public class PriceListItemModel
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public decimal ISKMRP { get; set; }
        public decimal ISKLoosePrice { get; set; }
        public decimal OSKMRP { get; set; }
        public decimal OSKLoosePrice { get; set; }
        public decimal ExportMRP { get; set; }
        public decimal ExportLoosePrice { get; set; }
        public decimal MRP { get; set; }
        public decimal LoosePrice { get; set; }

    }
}