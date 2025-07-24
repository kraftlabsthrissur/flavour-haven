using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
  public class DiscountBO
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
    }
    public class DiscountDetailBO
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
