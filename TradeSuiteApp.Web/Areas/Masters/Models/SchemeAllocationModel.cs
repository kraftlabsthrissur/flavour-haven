using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SchemeAllocationModel
    {
        public string Code { get; set; }
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string CustomerName { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Scheme { get; set; }
        public int BusinessCategoryID { get; set; }
        public int SalesCategoryID { get; set; }
        public int CategoryID { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList DistrictList { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList BusinessCategoryList { get; set; }
        public SelectList CustomerCategoryList { get; set; }
        public List<SchemeItemModel> Items { get; set; }
        public List<SchemeCustomer> Customers { get; set; }
        public List<SchemeCustomerCategory> CustomerCategory { get; set; }
        public List<SchemeDistrict> Districts { get; set; }
        public List<SchemeState> States { get; set; }

    }
    public class SchemeItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string Item { get; set; }
        public int OfferItemID { get; set; }
        public String OfferItem { get; set; }
        public string BusinessCategory { get; set; }
        public int? BusinessCategoryID { get; set; }
        public int SalesCategoryID { get; set; }
        public string SalesCategory { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal OfferQty { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsNewItem { get; set; }
        public int IsEnded { get; set; }
    }
    public class SchemeCustomer
    {
        public int CustomerID { get; set; }
        public string Customer { get; set; }

    }
    public class SchemeCustomerCategory
    {
        public int CustomerCategoryID { get; set; }
        public int SchemeCategoryID { get; set; }
        public string CustomerCategory { get; set; }
    }
    public class SchemeDistrict
    {
        public int DistrictID { get; set; }
        public int SchemeDistrictID { get; set; }
        public string District { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
    }
    public class SchemeState
    {
        public int StateID { get; set; }
        public int SchemeStateID { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public int DistrictID { get; set; }
        public string Country { get; set; }
        public int CountryID { get; set; }
    }
}