using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
//test 
namespace PresentationContractLayer
{
    public interface IGoodsReceiptNoteContract
    {
        List<GRNBO> GetGoodsReceiptNoteDetails(int ID);

        List<ItemBO> GetGoodsReceiptNoteTransDetails(int ID);

        List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrn(int SupplierID);

        List<PurchaseOrderTransBO> GetUnProcessedPurchaseOrderTransItemForGrn(int ID);

        bool SaveGRN(GRNBO grnBO, List<GRNTransItemBO> grnItems);
        int CreateGRN(GRNBO grnBO, List<GRNTransItemBO> grnItems);

        List<GRNTransItemBO> GetGoodsRecieptNoteItems(int ID);

        List<GRNBO> GetUnProcessedGRNBySupplier(int supplierID);
        List<GRNBO> GetUnProcessedGRN(string Hint);

        List<GRNTransItemBO> GetUnProcessedGRNItems(int grnID);

        List<GRNTransItemBO> GetGRNItemsForPurchaseInvoice(int grnID); 

         List<GRNTransItemBO> GetUnProcessedMilkItems(int grnID);

        List<GRNBO> GetGRNForPurchaseReturnBySupplier(int supplierID);

        List<GRNBO> GetUnProcessedMilkPurchase(int SupplierID);

        List<GRNBO> GetGRNNoWithItemID(int itemID);

        List<GRNBO> GetGrnInvoiceAutoComplete(string Term);

        int Cancel(int ID);

        int GetInvoiceNumberCount(string Hint, string Table, int SupplierID);

        DatatableResultBO GetGRNList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string DeliveryChallanNoHint, string DeliveryChallanDateHint, string WarehouseNameHint, string SortField, string SortOrder, int Offset, int Limit);

        List<GRNBO> GetGRNDetail(int ID);

        List<GRNBO> GetGRNDetailV3(int[] ID);

        List<GRNTransItemBO> GetGRNItems(int ID);

        List<GRNTransItemBO> GetItemForQRCodeGenerator(int GRNID);

        int SaveQRCode(List<GRNBO> QRCodeList);
        int CreateGRNV4(GRNBO grnBO, List<GRNTransItemBO> grnItems);
        List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrnV4(int SupplierID, int BusinessCategoryID);

        List<GRNBO> GetUnProcessedGRNV4(int BusinessCategoryID ,string Hint);
        bool IsBarCodeGenerator();
        bool IsDirectPurchaseInvoice();
        List<GRNBO> GetUnProcessedGRNV3(int SupplierID, string Hint);

        List<GRNTransItemBO> GetGRNPrintPDF(int ID);

        List<GRNTransItemBO> GetBatchListForQRCodePrint(int ItemID);

    }
}
