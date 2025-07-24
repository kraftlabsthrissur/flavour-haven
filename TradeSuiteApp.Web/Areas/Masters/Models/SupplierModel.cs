using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class SupplierModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Nullable<int> SupplierCategoryID { get; set; }

       

        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }
        public int StateID { get; set; }
        public Nullable<bool> IsGSTRegistered { get; set; }
        public int PaymentDays { get; set; }
        public string ItemCategory { get; set; }
        public string GstNo { get; set; }
        public string UanNo { get; set; }
        public string OldCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AcNo { get; set; }
        public string IfscNo { get; set; }
        public bool IsCustomer { get; set; }
        public int CustomerID { get; set; }
        public bool IsEmployee { get; set; }
        public int EmployeeID { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public string SupplierCategoryName { get; set; }
        public string SupplierAccountCategoory { get; set; }
        public int SupplierAccountsCategoryID { get; set; }
        public string SupplierAccountsCategoryName { get; set; }
        public string BillingDistrictName { get; set; }
        public int BillingDistrictID { get; set; }
        public string ShippingDistrictName { get; set; }
        public int ShippingDistrictID { get; set; }
        public string SupplierTaxCategoryName { get; set; }
        public int SupplierTaxCategoryID { get; set; }
        public string SupplierTaxSubCategoryName { get; set; }
        public int SupplierTaxSubCategoryID { get; set; }
        public string PaymentGroupName { get; set; }
        public int PaymentGroupID { get; set; }
        public string PaymentMethodName { get; set; }
        public int PaymentMethodID { get; set; }
        public string StateName { get; set; }
        public string Currency { get; set; }
        public string BillingDistrict { get; set; }
        public string ShippingDistrict { get; set; }
        public string Email { get; set; }
        public string StartDate { get; set; }
        public string DeactivatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeactivated { get; set; }
        public bool IsBlockForPurcahse { get; set; }
        public bool IsBlockForPayment { get; set; }
        public bool IsBlockForReceipt { get; set; }
        public int IsBilling { get; set; }
        public int IsShipping { get; set; }
        public string AdhaarCardNo { get; set; }
        public string PanCardNo { get; set; }
        public int CreditDaysID { get; set; }
        public bool IsActiveSupplier { get; set; }
       // public int CreditDays { get; set; }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int SupplierItemCategoryID { get; set; }
        public string SupplierItemCategoryName { get; set; }
        public string TradeLegalName { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int SupplierLocationID { get; set; }     
        public SelectList CategoryItemGroup { get; set; }
       
        public SelectList SuppliersAccountCategory { get; set; }
        public SelectList SuppliersTaxCategory { get; set; }
        public SelectList SuppliersTaxSubCategory { get; set; }
        public SelectList PaymentMethod { get; set; }
        public SelectList PaymentGroup { get; set; }
        public SelectList StateList { get; set; }
        public SelectList DisitrictList { get; set; }
        public SelectList ItemCategoryList { get; set; }
        public SelectList LocationList { get; set; }
      
        public SelectList CurrencyList { get; set; }
        public SelectList CountryList { get; set; }
        public List<ItemCategoryList> SupplierItemCategoryList { get; set; }
        public List<AddressModel> AddressList { get; set; }
        public List<LocationList> SupplierLocationList { get; set; }
        public List<RelatedSupplierModel> RelatedSupplierList { get; set; }
        public List<PaymentDaysModel> PaymentDaysList { get; set; }

        public List<CurrencyList> CurrencyItemCategoryList { get; set; }

    }
  
    public class LocationList
    {
        public int LocationID { get; set; }
        public int SupplierLocationID { get; set; }
        public string LocationName { get; set; }
    }

    public class ItemCategoryList
    {
        public int CategoryID { get; set; }
        public int SupplierItemCategoryID { get; set; }
        public string CategoryName { get; set; }
    }

   
    public class AddressModel
    {
        public int AddressID { get; set; }
        public string LandLine1 { get; set; }
        public string LandLine2 { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
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
    public class CurrencyList    {
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }
    public class CategoryItemGroup
    {
        public int SupplierCategoryID { get; set; }
        public string SupplierCategoryName { get; set; }
    }
    public class CreditDaysGroup
    {
        public int CreditDaysID { get; set; }
        public string CreditDaysName { get; set; }
        public int CreditDays { get; set; }
    }
    public class SuppliersTaxCategory
    {
        public int SupplierTaxCategoryID { get; set; }
        public string SupplierTaxCategoryName { get; set; }
    }
    public class SuppliersTaxSubCategory
    {
        public int SupplierTaxSubCategoryID { get; set; }
        public string SupplierTaxSubCategoryName { get; set; }
    }
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
    }
    public class PaymentGroup
    {
        public int PaymentGroupID { get; set; }
        public string PaymentGroupName { get; set; }
    }

    public class RelatedSupplierModel
    {
        public int RelatedSupplierID { get; set; }
        public string RelatedSupplierLocation { get; set; }
        public string RelatedSupplierName { get; set; }
    }


    public class SupplierDescriptionModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}