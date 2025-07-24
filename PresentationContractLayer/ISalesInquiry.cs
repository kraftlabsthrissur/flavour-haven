using BusinessObject;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface ISalesInquiry
    {
        bool SaveSalesInquiry(SalesInquiryBO SalesInquiry, List<SalesItemBO> Items);

        SalesInquiryBO GetSalesInquiry(int ID);
        List<SalesItemBO> GetSalesInquiryItemsPurchaseRequisition(int SalesOrderID);
        List<SalesItemBO> GetSalesInquiryItems(int SalesOrderID);

        List<SalesItemBO> GetBatchwiseSalesOrderItems(int[] SalesOrderID, int StoreID, int CustomerID, int SchemeID);
        List<SalesItemBO> GetGoodsReceiptSalesOrderItems(int[] SalesOrderID);
        List<SalesItemBO> GetInqueryCustomerAutoComplete(string CustomerName);
        int GetSchemeAllocation(int CustomerID);
        int CheckItemCreatedForSalesInquiryItems(int SalesInquiryItemID);


        DiscountAndOfferBO GetDiscountAndOfferDetails(int CustomerID, int SchemeID, int ItemID, decimal Qty, int UnitID);

        List<DiscountAndOfferBO> GetOfferDetails(int CustomerID, int SchemeID, int[] ItemID, int[] UnitID);

        DatatableResultBO GetAllSalesInquiryList(string Type, string SalesInquiryNo, string SalesInquiryDateHint, string RequestedCustomerNameHint, string PhoneNo, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetSalesInquiryList(string Type, string SalesInquiryNo, string SalesInquiryDateHint, string RequestedCustomerNameHint, string PhoneNo, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetCustomerSalesOrderList(string TransNo, string TransDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);


        void Approve(int SalesOrderID);



    }
}
