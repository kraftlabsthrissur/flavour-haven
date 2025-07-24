using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DiscountCategoryModel
    {
        public int ID { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string DiscountType { get; set; }
        public int Days { get; set; }
        public SelectList DiscountTypeList { get; set; }
        public string DiscountCategory { get; set; }
    }
}