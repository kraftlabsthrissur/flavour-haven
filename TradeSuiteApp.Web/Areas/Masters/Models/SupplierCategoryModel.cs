using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SupplierCategoryModel
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Remarks { get; set; }
        public List<SupplierCategoryItem> Items { get; set; }

    }
    public class SupplierCategoryItem
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Remarks { get; set; }

    }

}