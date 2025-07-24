using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class SchemeAllocationBO
    {
        public string Code { get; set; }
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategoryName { get; set; }
        public int StateID { get; set; }
        public string State { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public int DistrictID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SchemeName { get; set; }
        public List<SchemeItemBO> Items { get; set; }
        public List<SchemeCustomerBO> Customers { get; set; }
        public List<SchemeCustomerCategoryBO> CustomerCategory { get; set; }
        public List<SchemeDistrictBO> Districts { get; set; }
        public List<SchemeStateBO> States { get; set; }

    }
    public class SchemeItemBO
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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsNewItem { get; set; }
        public int IsEnded { get; set; }
   
    }
    public class SchemeCustomerBO
    {
        public int CustomerID { get; set; }
        public string Customer { get; set; }

    }
    public class SchemeCustomerCategoryBO
    {
        public int CustomerCategoryID { get; set; }
        public string CustomerCategory { get; set; }
        public int SchemeCategoryID { get; set; }
    }
    public class SchemeDistrictBO
    {
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string District { get; set; }
        public int SchemeDistrictID { get; set; } // For re-populating in edit mode
    }
    public class SchemeStateBO
    {
        public int StateID { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public int DistrictID { get; set; }
        public string Country { get; set; }
        public int CountryID { get; set; }
        public int SchemeStateID { get; set; } // For re-populating in edit mode
    }
}
