using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPurchaseOrder
    {
        JSONOutputBO SavePurchaseOrder(PurchaseOrderBO _masterPO, List<PurchaseOrderTransBO> _pOdetails);

        PurchaseOrderBO GetPurchaseOrder(int ID);

        List<PurchaseOrderTransBO> GetPurchaseOrderItems(int ID);

        bool CheckInvoiceNumberValid(int supplierID, string invoiceNo);

        bool IsPOCancellable(int POID);

        bool CancelPurchaseOrder(int PurchaseOrderID);

        List<PurchaseOrderItemBO> GetUnProcessedPurchaseRequisitionTransForPO(int PurchaseRequisitionID, int SupplierID);

        List<IsItemSuppliedBySupplier> IsItemSuppliedBySupplier(string ItemLists, int SupplierID);

        List<RequisitionBO> GetUnProcessedPurchaseRequisitionForPO();

        int SuspendPurchaseOrder(int ID, string Table);
        int SuspendPurchaseOrderItem(int ID);

        decimal GetRateForInterCompany(int ItemID, string BatchType);

        DatatableResultBO GetPurchaseOrderList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string ItemNameHint, string CategoryNameHint, string NetAmtHint, string SortField, string SortOrder, int Offset, int Limit);
        List<PurchaseOrderBO> GetOrderTypeList();
    }
}
