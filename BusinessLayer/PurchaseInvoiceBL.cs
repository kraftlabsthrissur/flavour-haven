using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class PurchaseInvoiceBL : IPurchaseInvoice
    {
        PurchaseInvoiceRepository purchaseInvoiceDAL;

        public PurchaseInvoiceBL()
        {
            purchaseInvoiceDAL = new PurchaseInvoiceRepository();
        }

        public bool Approve(int ID, string Status)
        {
            return purchaseInvoiceDAL.Approve(ID, Status);
        }

        public int Cancel(int ID, string Table)
        {
            return purchaseInvoiceDAL.Cancel(ID, Table);
        }

        public List<PurchaseInvoiceBO> GetInvoiceList(int SupplierID)
        {
            return purchaseInvoiceDAL.GetInvoiceList(SupplierID);
        }

        public int GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            return purchaseInvoiceDAL.GetInvoiceNumberCount(Hint, Table, SupplierID);
        }

        public List<PurchaseInvoiceTransItemBO> GetInvoiceTransList(int InvoiceID)
        {
            return purchaseInvoiceDAL.GetInvoiceTransList(InvoiceID);
        }

        public PurchaseInvoiceBO GetPurchaseInvoice(int purchaseInvoiceID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoice(purchaseInvoiceID);
        }
        public PurchaseInvoiceBO GetPurchaseInvoiceDetails(int purchaseInvoiceID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceDetails(purchaseInvoiceID);
        }
        public List<PurchaseInvoiceBO> GetPurchaseInvoiceDetails(int? ID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceDetails(ID);
        }

        public DatatableResultBO GetPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string InvoiceNoHint, string InvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceList(Type, TransNoHint, TransDateHint, InvoiceNoHint, InvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
        }

        public List<PurchaseInvoiceOtherChargeDetailBO> GetPurchaseInvoiceOtherChargeDetails(int purchaseID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceOtherChargeDetails(purchaseID);
        }

        public List<PurchaseInvoiceTaxDetailBO> GetPurchaseInvoiceTaxDetails(int purchaseID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceTaxDetails(purchaseID);
        }

        public List<GRNTransItemBO> GetPurchaseInvoiceTrans(int purchaseID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceTrans(purchaseID);
        }
        public List<GRNTransItemBO> GetPurchaseInvoiceTransDetails(int purchaseID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceTransDetails(purchaseID);
        }
        public List<PurchaseInvoiceBO> GetPurchaseInvoiceWithItemID(int itemID)
        {
            return purchaseInvoiceDAL.GetPurchaseInvoiceWithItemID(itemID);
        }

        public GRNTransItemBO GetStockInvoiceTransForPurchaseReturn(int ItemID, int invoiceID)
        {
            return purchaseInvoiceDAL.GetStockInvoiceTransForPurchaseReturn(ItemID, invoiceID);
        }

        public List<UnSettledPurchaseInoviceBO> GetUnSettledInvoicesBySupplier(int supplierID)
        {
            return purchaseInvoiceDAL.GetUnSettledInvoicesBySupplier(supplierID);
        }

        public int SavePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            return purchaseInvoiceDAL.SavePurchaseInvoice(purchaseInvoiceBO);
        }
        public int GeneratePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            return purchaseInvoiceDAL.GeneratePurchaseInvoice(purchaseInvoiceBO);
        }
        public int ApprovePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO)
        {
            return purchaseInvoiceDAL.ApprovePurchaseInvoice(purchaseInvoiceBO);
        }
        public List<PurchaseInvoiceBO> GetInvoiceTypeList()
        {
            return purchaseInvoiceDAL.GetInvoiceTypeList();
        }
    }
}
