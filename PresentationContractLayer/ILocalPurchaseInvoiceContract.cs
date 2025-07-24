using BusinessObject;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface ILocalPurchaseInvoiceContract
    {
        int Save(LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO, List<PurchaseOrderTransBO> LocalPurchaseInvoiceItems);

        DatatableResultBO GetLocalPurchases(string Type, string TransNoHint, string TransDateHint, string SupplierHint, string SortField, string SortOrder, int Offset, int Limit);

        LocalPurchaseInvoiceBO GetLocalPurchaseOrder(int ID);

        LocalPurchaseInvoiceBO GetLocalPurchaseID();

        bool GetIsLocalPurchase(int id);

        List<PurchaseOrderTransBO> GetLocalPurchaseOrderItems(int ID);

        string GetPrintTextFile(int ID);
    }
}
