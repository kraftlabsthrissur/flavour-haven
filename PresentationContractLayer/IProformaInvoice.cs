using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IProformaInvoice
    {
        int Save(ProformaInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails, List<SalesPackingDetailsBO> PackingDetails);

        void Cancel(int ProformaInvoiceID);

        ProformaInvoiceBO GetProformaInvoice(int ProformaInvoiceID);

        List<SalesItemBO> GetProformaInvoiceItems(int ProformaInvoiceID);

        List<SalesItemBO> GetProformaInvoiceItems(int[] ProformaInvoiceID);

        List<SalesAmountBO> GetProformaInvoiceAmountDetails(int ProformaInvoiceID);

        DatatableResultBO GetProformaInvoiceList(string CodeHint, string DateHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string InvoiceType, int ItemCategoryID, int CustomerID, string SortField, string SortOrder, int Offset, int Limit);

        List<SalesBatchBO> GetItemBatchwise(int ItemID, decimal Qty, decimal OfferQty, int StoreID, int CustomerID,int UnitID);

        bool IsCancelable(int ProformaInvoiceID);

        CustomerCreditSummaryBO GetCustomerCreditSummary(int ProformaInvoiceID);

        string GetPrintTextFile(int ProformaInvoiceID);

        List<SalesPackingDetailsBO> GetProformaInvoicePackingDetails(int ProformaInvoiceID);
    }
}
