using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IPurchaseInvoice
    {
        int SavePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO);

        List<UnSettledPurchaseInoviceBO> GetUnSettledInvoicesBySupplier(int supplierID);

        PurchaseInvoiceBO GetPurchaseInvoice(int purchaseInvoiceID);
        PurchaseInvoiceBO GetPurchaseInvoiceDetails(int purchaseInvoiceID);
        List<GRNTransItemBO> GetPurchaseInvoiceTrans(int purchaseID);
        List<GRNTransItemBO> GetPurchaseInvoiceTransDetails(int purchaseID);
        List<PurchaseInvoiceOtherChargeDetailBO> GetPurchaseInvoiceOtherChargeDetails(int purchaseID);

        List<PurchaseInvoiceTaxDetailBO> GetPurchaseInvoiceTaxDetails(int purchaseID);

        List<PurchaseInvoiceBO> GetPurchaseInvoiceDetails(int? ID);

        List<PurchaseInvoiceBO> GetPurchaseInvoiceWithItemID(int itemID);

        GRNTransItemBO GetStockInvoiceTransForPurchaseReturn(int ItemID, int invoiceID);

        List<PurchaseInvoiceBO> GetInvoiceList(int SupplierID);

        List<PurchaseInvoiceTransItemBO> GetInvoiceTransList(int InvoiceID);

        bool Approve(int ID, String Status);

        int Cancel(int ID, string Table);

        int GetInvoiceNumberCount(string Hint, string Table, int SupplierID);

        DatatableResultBO GetPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string InvoiceNoHint, string InvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit);
        int GeneratePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO);
        int ApprovePurchaseInvoice(PurchaseInvoiceBO purchaseInvoiceBO);
        List<PurchaseInvoiceBO> GetInvoiceTypeList();
    }
}
