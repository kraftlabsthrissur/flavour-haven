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
    public class GoodsReceiptNoteBL : IGoodsReceiptNoteContract
    {
        GoodsReceiptNoteDAL goodsReceiptNoteDAL;

        public GoodsReceiptNoteBL()
        {
            goodsReceiptNoteDAL = new GoodsReceiptNoteDAL();
        }

        public List<GRNBO> GetGoodsReceiptNoteDetails(int ID)
        {
            return goodsReceiptNoteDAL.GetGoodsReceiptNoteDetails(ID);
        }

        public List<ItemBO> GetGoodsReceiptNoteTransDetails(int ID)
        {
            throw new NotImplementedException();
        }

        public List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrn(int SupplierID)
        {
            return goodsReceiptNoteDAL.GetUnProcessedPurchaseOrderForGrn(SupplierID).ToList();
        }

        public List<PurchaseOrderTransBO> GetUnProcessedPurchaseOrderTransItemForGrn(int ID)
        {
            return goodsReceiptNoteDAL.GetUnProcessedPurchaseOrderTransItemForGrn(ID).ToList();
        }
       
        public bool SaveGRN(GRNBO grnBO, List<GRNTransItemBO> grnItems)
        {
            if (grnBO.ID == 0)
            {
                return goodsReceiptNoteDAL.SaveGRN(grnBO, grnItems);
            }
            else
            {
                return goodsReceiptNoteDAL.UpdateGRN(grnBO, grnItems);
            }
        }
        public int CreateGRN(GRNBO grnBO, List<GRNTransItemBO> grnItems)
        {
            if (grnBO.ID == 0)
            {
                return goodsReceiptNoteDAL.CreateGRN(grnBO, grnItems);
            }
            else
            {
                return goodsReceiptNoteDAL.UpdateGoodsReceiptNote(grnBO, grnItems);
            }

        }
        public List<GRNTransItemBO> GetGoodsRecieptNoteItems(int ID)
        {
            return goodsReceiptNoteDAL.GetGoodsRecieptNoteItems(ID);

        }

        public List<GRNBO> GetUnProcessedGRNBySupplier(int supplierID)
        {
            return goodsReceiptNoteDAL.GetUnProcessedGRNBySupplier(supplierID);
        }
        public List<GRNBO> GetUnProcessedGRN(string Hint)
        {
            return goodsReceiptNoteDAL.GetUnProcessedGRN(Hint);
        }

        public List<GRNTransItemBO> GetUnProcessedGRNItems(int grnID)
        {
            return goodsReceiptNoteDAL.GetUnProcessedGRNItems(grnID);
        }

        public List<GRNTransItemBO> GetGRNItemsForPurchaseInvoice(int grnID)
        {
            return goodsReceiptNoteDAL.GetGRNItemsForPurchaseInvoice(grnID);
        }

        public List<GRNTransItemBO> GetUnProcessedMilkItems(int grnID)
        {
            return goodsReceiptNoteDAL.GetUnProcessedMilkItems(grnID);
        }

        public List<GRNBO> GetGRNForPurchaseReturnBySupplier(int supplierID)
        {
            return goodsReceiptNoteDAL.GetGRNForPurchaseReturnBySupplier(supplierID);
        }

        public List<GRNBO> GetUnProcessedMilkPurchase(int SupplierID)
        {
            return goodsReceiptNoteDAL.GetUnProcessedMilkPurchase(SupplierID);
        }

        public List<GRNBO> GetGRNNoWithItemID(int ItemID)
        {
            return goodsReceiptNoteDAL.GetGRNNoWithItemID(ItemID);
        }

        public List<GRNBO> GetGrnInvoiceAutoComplete(string term)
        {
            return goodsReceiptNoteDAL.GetGrnInvoiceAutoComplete(term);
        }

        public int Cancel(int ID)
        {
            return goodsReceiptNoteDAL.Cancel(ID);
        }

        public int GetInvoiceNumberCount(string Hint,string Table,int SupplierID)
        {
            return goodsReceiptNoteDAL.GetInvoiceNumberCount( Hint, Table,SupplierID);
        }

        public DatatableResultBO GetGRNList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string DeliveryChallanNoHint, string DeliveryChallanDateHint, string WarehouseNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return goodsReceiptNoteDAL.GetGRNList(Type, TransNoHint, TransDateHint, SupplierNameHint, DeliveryChallanNoHint, DeliveryChallanDateHint, WarehouseNameHint, SortField, SortOrder, Offset, Limit);
        }
        public List<GRNBO> GetGRNDetail(int ID)
        {
            return goodsReceiptNoteDAL.GetGRNDetail(ID);
        }

        public List<GRNBO> GetGRNDetailV3(int[] ID)
        {
            return goodsReceiptNoteDAL.GetGRNDetailV3(ID);
        }

        public List<GRNTransItemBO> GetGRNItems(int ID)
        {
            return goodsReceiptNoteDAL.GetGRNItems(ID);
        }

        public List<GRNTransItemBO> GetItemForQRCodeGenerator(int GRNID)
        {
            return goodsReceiptNoteDAL.GetItemForQRCodeGenerator(GRNID);
        }

        public int SaveQRCode(List<GRNBO> QRCodeList)
        {
            return goodsReceiptNoteDAL.SaveQRCode(QRCodeList);
        }
        public int CreateGRNV4(GRNBO grnBO, List<GRNTransItemBO> grnItems)
        {
            if (grnBO.ID == 0)
            {
                return goodsReceiptNoteDAL.CreateGRNV4(grnBO, grnItems);
            }
            else
            {
                return goodsReceiptNoteDAL.UpdateGoodsReceiptNote(grnBO, grnItems);
            }

        }

        public List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrnV4(int SupplierID, int BusinessCategoryID)
        {
            return goodsReceiptNoteDAL.GetUnProcessedPurchaseOrderForGrnV4(SupplierID, BusinessCategoryID);
        }

        public List<GRNBO> GetUnProcessedGRNV4(int BusinessCategoryID, string Hint)
        {
            return goodsReceiptNoteDAL.GetUnProcessedGRNV4(BusinessCategoryID, Hint);
        }
        public List<GRNBO> GetUnProcessedGRNV3(int SupplierID, string Hint)
        {
            return goodsReceiptNoteDAL.GetUnProcessedGRNV3(SupplierID, Hint);
        }
        public bool IsBarCodeGenerator()
        {
            return goodsReceiptNoteDAL.IsBarCodeGenerator();
        }
        public bool IsDirectPurchaseInvoice()
        {
            return goodsReceiptNoteDAL.IsDirectPurchaseInvoice();
        }
        public List<GRNTransItemBO> GetGRNPrintPDF(int ID)
        {
            return goodsReceiptNoteDAL.GetGRNPrintPDF(ID);
        }

        public List<GRNTransItemBO> GetBatchListForQRCodePrint(int ID)
        {
            return goodsReceiptNoteDAL.GetBatchListForQRCodePrint(ID);
        }
    }
}
