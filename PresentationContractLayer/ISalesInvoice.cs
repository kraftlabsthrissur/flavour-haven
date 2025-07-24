using BusinessObject;
using System;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface ISalesInvoice
    {
        DatatableResultBO GetInvoiceListForSalesReturn(int CustomerID, string TransHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetSalesInvoiceHistory(int ItemID, string SalesOrderNos, string InvoiceDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetCounterSalesHistory(int ItemID, string TransNo, string TransDate, string CustomerName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetPendingPOHistory(int ItemID, string PurchaseOrderNo, string PurchaseOrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetPurchaseHistory(int ItemID, string PurchaseOrderNo, string PurchaseOrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetLegacyPurchaseHistory(int ItemID, string ReferenceOrderNo, string OrderDate, string SupplierName, string ItemName, string PartsNumber, string SortField, string SortOrder, int Offset, int Limit);
        List<SalesInvoiceItemBO> GetInvoiceTransList(int InvoiceID, int PriceListID);

        int Save(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails, List<SalesPackingDetailsBO> PackingDetails);

        int Cancel(int SalesInvoiceID);

        SalesInvoiceBO GetSalesInvoice(int SalesInvoiceID, int LocationID);

        List<SalesItemBO> GetSalesInvoiceItems(int SalesInvoiceID, int LocationID);
        List<SalesItemBO> GetGoodsReceiptSalesInvoiceItems(int[] SalesInvoiceID, int LocationID);
        List<SalesAmountBO> GetSalesInvoiceAmountDetails(int SalesInvoiceID, int LocationID);

        DatatableResultBO GetSalesInvoiceList(string Type, string CodeHint, string DateHint, string SalesTypeHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetCustomerSalesInvoiceList(int CustomerID,string TransNoHint, string TranDateHint, string CustomerNameHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);


        bool IsCancelable(int SalesInvoiceID);

        List<SalesInvoiceBO> GetIntercompanySalesInvoiceList(int SupplierID, int LocationID);

        decimal GetCreditAmountByCustomer(int CustomerID);

        int GetFreightTaxForEcommerceCustomer();

        List<SalesInvoiceBO> GetSalesInvoiceCodeAutoCompleteForReport(string CodeHint, DateTime FromDate, DateTime ToDate);

        string GetPrintTextFile(int SalesInvoiceID);

        List<SalesPackingDetailsBO> GetSalesInvoicePackingDetails(int SalesInvoiceID, int LocationID);

    }
}
