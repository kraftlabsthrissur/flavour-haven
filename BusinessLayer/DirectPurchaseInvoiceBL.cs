using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class DirectPurchaseInvoiceBL:IDirectPurchaseInvoiceContract
    {
        DirectPurchaseInvoiceDAL DirectPurchaseInvoiceDAL;
        public DirectPurchaseInvoiceBL()
        {
            DirectPurchaseInvoiceDAL = new DirectPurchaseInvoiceDAL();
        }

        public int Save(LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO, string XMLItems)
        {
            //string XMLItems = XMLHelper.Serialize(LocalPurchaseInvoiceItems);
            if (LocalPurchaseInvoiceBO.ID == 0)

            {
                return DirectPurchaseInvoiceDAL.Save(LocalPurchaseInvoiceBO, XMLItems);
            }
            else
            {
                return DirectPurchaseInvoiceDAL.Update(LocalPurchaseInvoiceBO, XMLItems);
            }
        }

        public LocalPurchaseInvoiceBO GetDirectPurchaseOrder(int ID)
        {
            return DirectPurchaseInvoiceDAL.GetDirectPurchaseOrder(ID);
        }
        public List<PurchaseOrderTransBO> GetDirectPurchaseOrderItems(int ID)
        {
            return DirectPurchaseInvoiceDAL.GetDirectPurchaseOrderItems(ID);
        }
        public DatatableResultBO GetDirectPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string SupplierHint, String NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return DirectPurchaseInvoiceDAL.GetDirectPurchaseInvoiceList(Type, TransNoHint, TransDateHint, SupplierHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public List<MRPBO> GetMRPForPurchaseInvoice(int ItemID)
        {
            return DirectPurchaseInvoiceDAL.GetMRPForPurchaseInvoice(ItemID);
        }
        public List<MRPBO> GetMRPForPurchaseInvoiceByBatchID(int ItemID,string Batch)
        {
            return DirectPurchaseInvoiceDAL.GetMRPForPurchaseInvoiceByBatchID(ItemID, Batch);
        }
        public bool IsCancelable(int PurchaseOrderID)
        {
            return DirectPurchaseInvoiceDAL.IsCancelable(PurchaseOrderID);
        }
        public int Cancel(int PurchaseOrderID)
        {
            return DirectPurchaseInvoiceDAL.Cancel(PurchaseOrderID);
        }
        public List<ItemCategoryAndUnitBO> GetUnitsAndCategoryByItemID(int ItemID)
        {
            return DirectPurchaseInvoiceDAL.GetUnitsAndCategoryByItemID(ItemID);
        }
    }
}
