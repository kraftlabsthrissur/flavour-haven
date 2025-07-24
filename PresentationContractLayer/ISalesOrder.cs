using BusinessObject;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface ISalesOrder
    {
        bool SaveSalesOrder(SalesOrderBO SalesOrder, List<SalesItemBO> Items);

        void ProcessOrder(SalesOrderBO SalesOrder, List<SalesItemBO> Items);

        void Cancel(int SalesOrderID);

        bool IsCancelable(int SalesOrderID);

        SalesOrderBO GetSalesOrder(int ID);

        List<SalesItemBO> GetSalesOrderItems(int SalesOrderID);

        List<SalesItemBO> GetBatchwiseSalesOrderItems(int[] SalesOrderID, int StoreID, int CustomerID, int SchemeID);
        List<SalesItemBO> GetGoodsReceiptSalesOrderItems(int[] SalesOrderID);

        int GetSchemeAllocation(int CustomerID);

        DiscountAndOfferBO GetDiscountAndOfferDetails(int CustomerID, int SchemeID, int ItemID, decimal Qty,int UnitID);

        List<DiscountAndOfferBO> GetOfferDetails(int CustomerID, int SchemeID, int[] ItemID, int[] UnitID);

        DatatableResultBO GetSalesOrderList(int CustomerID, int ItemCategoryID, string SalesType, string CodeHint, string DateHint, string CustomerNameHint, string SalesTypeHint, string DespatchDateHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetCustomerSalesOrderList(int CustomerID, string TransNo, string TransDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        
        List<UploadOrderBO> ReadExcel(string Path);

        void Approve(int SalesOrderID);

        string GetPrintTextFile(int SalesOrderID);
        DatatableResultBO GetSalesOrderHistory(string Type, int ItemID, string SalesOrderNo, string OrderDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit);


    }
}
