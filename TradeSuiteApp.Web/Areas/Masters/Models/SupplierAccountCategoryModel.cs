using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SupplierAccountCategoryModel
    {
        public int ID { get; set; }
        public String Name { get; set; }
        List<SupplierAccountCategoryItemModel> items { get; set; }
    }

    public class SupplierAccountCategoryItemModel
    {
        public int ID { get; set; }
        public String Name { get; set; }
    }
}