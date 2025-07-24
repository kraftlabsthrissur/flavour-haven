using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ICustomerContract
    {
        DatatableResultBO GetCustomerList(string Type, int CustomerCategoryID,int StateID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string CurrencyNameHint, string LandLineHint, string MobileHit, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetCustomerMainList(string Type,int CustomerCategoryID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint,string PropratorNameHint,string OldCodeHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetPartyList(string NameHint, string DoctorNameHint, string SortField, string SortOrder, int Offset, int Limit);

        int CreateCustomer(CustomerBO customerBO, List<AddressBO> AddressCreateList, List<CustomerBO> CustomerLocationList);

        int EditCustomer(CustomerBO customerBO);

        List<CustomerBO> GetCustomerAutoComplete(string Hint, int CustomerCategoryID);

        List<CategoryBO> GetCustomerCategories();

        List<CustomerBO> GetPriceList();

        List<CustomerBO> GetDiscountList();

        List<CustomerBO> GetCashDiscountList();

        CustomerBO GetCustomerDetails(int CustomerID);

        List<CustomerBO> GetCustomersByOldCodes(string[] Codes);

        bool IsInterCompanyCustomer(int CustomerID);

        bool HasOutstandingAmount(int CustomerID);

        decimal GetTurnOverDiscount(int CustomerID);

        decimal CashDiscountPercentage(int CustomerID);

        BatchTypeBO GetBatchTypeID(int CustomerID);

        DatatableResultBO GetCustomerDetails(string CodeHint, string CustomerNameHint, string CategoryHint, string LocationHint, string CustomerSchemeHint, string DiscountPercentageHint, string PriceListHint, string MinCreditLimitHint, string MaxCreditLimitHint, string OutStandingAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetCustomerItemDetails(int CustomerID,string CodeHint,string ItemNameHint,string MRPHint,string DiscountPercentageHint,string QuantityHint,string OfferQuantityHint,string SortField,string SortOrder,int Offset,int Limit);
        string CheckCustomerAlradyExist(int ID, string Name, string GstNo, string PanCardNo, string AdhaarCardNo, string Mobile, string LandLine1, string landline2);

        DatatableResultBO GetServiceCustomerList(string Type, int CustomerCategoryID, int StateID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        int DeleteCustomer(int ID);
    }
}
