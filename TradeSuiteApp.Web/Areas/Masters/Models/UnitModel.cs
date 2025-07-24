using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class UnitModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int QOM { get; set; }
        public string UOM { get; set; }
        public int CF { get; set; }
        public decimal PackSize { get; set; }
        public Nullable<int> CreatedUserID { get; set; }        
    }
}