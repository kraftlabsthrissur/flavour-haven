using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IDirectPurchaseInvoiceContract
    {
        int Save(LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO, string XMLItems);
        List<PurchaseOrderTransBO> GetDirectPurchaseOrderItems(int ID);
        LocalPurchaseInvoiceBO GetDirectPurchaseOrder(int ID);
        DatatableResultBO GetDirectPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string SupplierHint,string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);
        List<MRPBO> GetMRPForPurchaseInvoice(int ItemID);
        List<MRPBO> GetMRPForPurchaseInvoiceByBatchID(int ItemID,string Batch);
        bool IsCancelable(int PurchaseOrderID);
        int Cancel(int PurchaseOrderID);
        List<ItemCategoryAndUnitBO> GetUnitsAndCategoryByItemID(int ItemID);
    }
}
