using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class CustomerBL : ICustomerContract
    {
        CustomerDAL customerDAL;

        public CustomerBL()
        {
            customerDAL = new CustomerDAL();
        }

        public bool HasOutstandingAmount(int CustomerID)
        {
            return customerDAL.HasOutstandingAmount(CustomerID);
        }

        public int CreateCustomer(CustomerBO customerBO, List<AddressBO> AddressCreateList, List<CustomerBO>CustomerLocationList)
        {
            if (customerBO.ID == 0)
            {
                return customerDAL.CreateCustomer(customerBO, AddressCreateList, CustomerLocationList);
            }
            else
            {
                return customerDAL.UpdateCustomer(customerBO, AddressCreateList, CustomerLocationList);
            }
        }

        public int EditCustomer(CustomerBO customerBO)
        {
            return customerDAL.EditCustomer(customerBO);
        }

        public List<CustomerBO> GetCustomerAutoComplete(string Hint, int CustomerCategoryID)
        {
            return customerDAL.GetCustomerAutoComplete(Hint, CustomerCategoryID);
        }

        public DatatableResultBO GetCustomerList(string Type, int CustomerCategoryID,int StateID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string CurrencyNameHint, string LandLineHint, string MobileHit, string SortField, string SortOrder, int Offset, int Limit)
        {
            return customerDAL.GetCustomerList(Type,CustomerCategoryID, StateID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, CurrencyNameHint, LandLineHint, MobileHit, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetCustomerMainList(string Type,int CustomerCategoryID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string PropratorNameHint, string OldCodeHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return customerDAL.GetCustomerMainList(Type,CustomerCategoryID, CodeHint, NameHint, LocationHint, CustomerCategoryHint,PropratorNameHint,OldCodeHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetPartyList(string NameHint, string DoctorNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return customerDAL.GetPartyList(NameHint, DoctorNameHint, SortField, SortOrder, Offset, Limit);
        }

        public List<CategoryBO> GetCustomerCategories()
        {
            return customerDAL.GetCustomerCategories();
        }

        public List<CustomerBO> GetPriceList()
        {
            return customerDAL.GetPriceList();
        }

        public List<CustomerBO> GetDiscountList()
        {
            return customerDAL.GetDiscountList();
        }

        public List<CustomerBO> GetCashDiscountList()
        {
            return customerDAL.GetCashDiscountList();
        }

        public CustomerBO GetCustomerDetails(int CustomerID)
        {
            return customerDAL.GetCustomerDetails(CustomerID);
        }

        public CustomerBO GetCustomer(int CustomerID)
        {
            return customerDAL.GetCustomer(CustomerID);
        }

        public List<CustomerBO> GetCustomersByOldCodes(string[] Codes)
        {
            string CommaSeparatedCodes = string.Join(",", Codes);
            return customerDAL.GetCustomersByOldCodes(CommaSeparatedCodes);
        }

        public bool IsInterCompanyCustomer(int CustomerID)
        {
            return customerDAL.IsInterCompanyCustomer(CustomerID);
        }

        public decimal GetTurnOverDiscount(int CustomerID)
        {
            return customerDAL.GetTurnOverDiscount(CustomerID);
        }

        public BatchTypeBO GetBatchTypeID(int CustomerID)
        {
            return customerDAL.GetBatchTypeID(CustomerID);
        }

        public decimal CashDiscountPercentage(int CustomerID)
        {
            return customerDAL.CashDiscountPercentage(CustomerID);
        }

        public DatatableResultBO GetCustomerDetails(string CodeHint, string CustomerNameHint, string CategoryHint, string LocationHint, string CustomerSchemeHint, string DiscountPercentageHint, string PriceListHint, string MinCreditLimitHint, string MaxCreditLimitHint, string OutStandingAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return customerDAL.GetCustomerDetails(CodeHint, CustomerNameHint, CategoryHint, LocationHint, CustomerSchemeHint, DiscountPercentageHint, PriceListHint, MinCreditLimitHint, MaxCreditLimitHint, OutStandingAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetCustomerItemDetails(int CustomerID,string CodeHint, string ItemNameHint, string MRPHint, string DiscountPercentageHint, string QuantityHint, string OfferQuantityHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return customerDAL.GetCustomerItemDetails(CustomerID,CodeHint, ItemNameHint, MRPHint, DiscountPercentageHint, QuantityHint, OfferQuantityHint, SortField, SortOrder, Offset, Limit);
        }

        public string CheckCustomerAlradyExist(int ID, string Name, string GstNo, string PanCardNo, string AdhaarCardNo, string Mobile, string LandLine1, string landline2)
        {
            return customerDAL.CheckCustomerAlradyExist(ID, Name, GstNo, PanCardNo, AdhaarCardNo, Mobile, LandLine1, landline2);
        }

        public DatatableResultBO GetServiceCustomerList(string type,int CustomerCategoryID, int StateID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return customerDAL.GetServiceCustomerList(type, CustomerCategoryID, StateID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public int DeleteCustomer (int ID)
        {
            return customerDAL.DeleteCustomer(ID);
        }
    }
}
