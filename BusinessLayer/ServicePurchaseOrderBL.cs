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
    public class ServicePurchaseOrderBL : IServicePurchaseOrderContract
    {
        ServicePurchaseOrderDAL servicePurchaseOrderDAL;

        public ServicePurchaseOrderBL()
        {
            servicePurchaseOrderDAL = new ServicePurchaseOrderDAL();
        }

        public PurchaseOrderBO GetPurchaseOrder(int ID)
        {
            return servicePurchaseOrderDAL.GetPurchaseOrder(ID);
        }

        public List<PurchaseOrderTransBO> GetPurchaseOrderTransDetails(int ID)
        {
            return servicePurchaseOrderDAL.GetPurchaseOrderTransDetails(ID);
        }

        public List<PurchaseOrderTransBO> GetUnProcessedPurchaseRequisitionTransForPO(int ID, int SupplierID)
        {
            return servicePurchaseOrderDAL.GetUnProcessedPurchaseRequisitionTransForPO(ID, SupplierID);
        }

        public JSONOutputBO SavePurchaseOrder(PurchaseOrderBO PO, List<PurchaseOrderTransBO> POTrans)
        {
            return servicePurchaseOrderDAL.SavePurchaseOrder(PO, POTrans);
        }

        public JSONOutputBO UpdatePurchaseOrder(PurchaseOrderBO PO, List<PurchaseOrderTransBO> POTrans)
        {
            return servicePurchaseOrderDAL.UpdatePurchaseOrder(PO, POTrans);
        }

        public bool IsPOSCancellable(int POSID)
        {
            return servicePurchaseOrderDAL.IsPOSCancellable(POSID);
        }

        public bool CancelServicePurchaseOrder(int ServicePurchaseOrderID)
        {
            return servicePurchaseOrderDAL.CancelServicePurchaseOrder(ServicePurchaseOrderID);
        }

        public void CreateAutomaticSRNAndInvoice(int PurchaseOrderID, string InvoiceNo, DateTime InvoiceDate, decimal Discount, decimal OtherDeductions)
        {
            servicePurchaseOrderDAL.CreateAutomaticSRNAndInvoice(PurchaseOrderID, InvoiceNo, InvoiceDate, Discount, OtherDeductions);
        }

        public int SuspendPurchaseOrder(int ID, string Table)
        {
            return servicePurchaseOrderDAL.SuspendPurchaseOrder(ID, Table);
        }

        public int GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            return servicePurchaseOrderDAL.GetInvoiceNumberCount(Hint, Table, SupplierID);
        }

        public DatatableResultBO GetPurchaseOrderList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string ItemNameHint, string CategoryNameHint,string NetAmtHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return servicePurchaseOrderDAL.GetPurchaseOrderList(Type, TransNoHint, TransDateHint, SupplierNameHint, ItemNameHint, CategoryNameHint, NetAmtHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
