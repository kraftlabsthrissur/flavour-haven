using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObject;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DiscountModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategoryName { get; set; }
        public int CustomerStateID { get; set; }
        public string CustomerStateName { get; set; }
        public int BusinessCategoryID { get; set; }
        public string BusinessCategoryName { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public string SalesIncentiveCategoryName { get; set; }
        public int SalesCategoryID { get; set; }
        public string SalesCategoryName { get; set; }
        public int DiscountCategoryID { get; set; }
        public string DiscountCategoryName { get; set; }       
        public decimal DiscountPercentage { get; set; }

        public SelectList SalesCategoryList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public SelectList BusinessCategoryList { get; set; }
        public SelectList SalesIncentiveCategoryList { get; set; }
        public SelectList CustomerStateList { get; set; }
        public List<CategoryBO> DiscountPercentageList { get; set; }
        public List<DiscountDetailModel> DiscountDetails { get; set; }
    }

    public class DiscountDetailModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int ItemID { get; set; }
        public int CustomerID { get; set; }
        public int CustomerCategoryID { get; set; }
        public int CustomerStateID { get; set; }
        public int BusinessCategoryID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public int SalesCategoryID { get; set; }
        public int DiscountCategoryID { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}