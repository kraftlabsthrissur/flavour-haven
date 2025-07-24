using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class CustomerModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int StateID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int PriceListID { get; set; }
        public string CustomerCategory { get; set; }

        //code below by jinto   
        public string Name2 { get; set; }
        public string EmailID { get; set; }
        public string AadhaarNo { get; set; }
        public string PanNo { get; set; }
        public string FaxNo { get; set; }
        public decimal MinCreditLimit { get; set; }
        public decimal MaxCreditLimit { get; set; }
        public string StartDate { get; set; }
        public string ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsInterCompany { get; set; }
        public bool IsMappedtoExpsEntries { get; set; }
        public bool IsBlockedForSalesOrders { get; set; }
        public bool IsBlockedForSalesInvoices { get; set; }
        public bool IsAlsoASupplier { get; set; }
        public bool IsDeactivated { get; set; }
        public int IsBilling { get; set; }
        public int IsShipping { get; set; }
        public int FSOID { get; set; }
        public string FSOName { get; set; }
        public string GstNo { get; set; }
        public string OldCode { get; set; }
        public string OldName { get; set; }
        public string CategoryName { get; set; }
        public int SupplierID { get; set; }
        public int CustomerRouteID { get; set; }
        public int CashDiscountCategoryID { get; set; }
        public int CategoryID { get; set; }
        public int BillingDistrictID { get; set; }
        public int ShippingDistrictID { get; set; }
        public int CustomerCategoryID { get; set; }
        public string CustomerCategoryName { get; set; }
        public string Currency { get; set; }
        public string ContactPersonName { get; set; }
        public int DiscountID { get; set; }
        public int CashDiscountID { get; set; }
        public int CustomerTaxCategoryID { get; set; }
        public int CustomerAccountsCategoryID { get; set; }
        public int CreditDaysID { get; set; }
        public int CreditDays { get; set; }
        public int PaymentTypeID { get; set; }
        public string PriceListName { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal CashDiscountPercentage { get; set; }
        public string CustomerTaxCategory { get; set; }
        public string CustomerAccountsCategoryName { get; set; }
        public string PaymentTypeName { get; set; }

        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int CustomerLocationID { get; set; }
        public decimal CustomerMonthlyTarget { get; set; }
        public string TradeLegalName { get; set; }
        public List<CustomerAddressModel> AddressList { get; set; }
        public SelectList CurrencyLists { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList DisitrictList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList PriceList { get; set; }
        public SelectList DiscountList { get; set; }
        public SelectList CashDiscountList { get; set; }
        public SelectList CustomerTaxCategoryList { get; set; }
        public SelectList CustomerAccountsCategoryList { get; set; }
        public SelectList CreditDaysList { get; set; }
        public SelectList PaymentTypeList { get; set; }
        public SelectList LocationList { get; set; }
        public List<CustomerLocationMapping> CustomerLocationList { get; set; }
        //public List<CurrencyLists> CurrencyItemCategoryList { get; set; }
        public bool IsMappedToServiceSales { get; set; }
        public string SupplierName { get; set; }
        public bool IsBlockedForChequeReceipt { get; set; }
        public bool IsDraft { get; set; }
    }
/*    public class CurrencyLists
    {
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }*/
    public class CustomerAddressModel
    {
        public int AddressID { get; set; }
        public string LandLine1 { get; set; }
        public string LandLine2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PIN { get; set; }
        public string MobileNo { get; set; }
        public string ContactPerson { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public bool IsBilling { get; set; }
        public bool IsShipping { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultShipping { get; set; }
    }

    public class CustomerLocationMapping
    {
        public int LocationID { get; set; }
        public int CustomerLocationID { get; set; }
        public string LocationName { get; set; }
    }


}
