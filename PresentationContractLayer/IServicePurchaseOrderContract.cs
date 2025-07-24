using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;


namespace PresentationContractLayer
{
    public interface IServicePurchaseOrderContract
    {
        PurchaseOrderBO GetPurchaseOrder(int ID);

        JSONOutputBO SavePurchaseOrder(PurchaseOrderBO PO, List<PurchaseOrderTransBO> POTrans);

        JSONOutputBO UpdatePurchaseOrder(PurchaseOrderBO PO, List<PurchaseOrderTransBO> POTrans);

        List<PurchaseOrderTransBO> GetUnProcessedPurchaseRequisitionTransForPO(int ID, int SupplierID);

        List<PurchaseOrderTransBO> GetPurchaseOrderTransDetails(int ID);

        bool IsPOSCancellable(int POSID);

        bool CancelServicePurchaseOrder(int ServicePurchaseOrderID);

        void CreateAutomaticSRNAndInvoice(int PurchaseOrderID,string InvoiceNo,DateTime InvoiceDate, decimal Discount, decimal OtherDeductions);

        int SuspendPurchaseOrder(int ID, string Table);

        int GetInvoiceNumberCount(string Hint, string Table, int SupplierID);

        DatatableResultBO GetPurchaseOrderList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string ItemNameHint, string CategoryNameHint,string NetAmtHint, string SortField, string SortOrder, int Offset, int Limit);

    }
}
