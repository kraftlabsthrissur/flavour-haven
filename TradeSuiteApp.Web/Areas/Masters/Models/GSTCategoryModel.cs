using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class GSTCategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal SGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal IGSTPercent { get; set; }
    }
}