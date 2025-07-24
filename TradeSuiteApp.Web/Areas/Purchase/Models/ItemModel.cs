using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class ItemModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int UnitID { get; set; }
        public int Stock { get; set; }
        public int QtyUnderQC { get; set; }
        public int QtyOrdered { get; set; }
    }
}