using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PresentationContractLayer
{
    public interface ISalesGoodsReceiptNote
    {
        int Save(SalesGoodsReceiptBO Invoice, List<SalesGoodsReceiptItemBO> Items);

        void Cancel(int GoodReceiptNoteID);
        bool IsCancelable(int GoodReceiptNoteID);

        SalesGoodsReceiptBO GetGoodReceiptNote(int GoodReceiptNoteID);
        SalesGoodsReceiptBO GetGoodReceiptNotes(string GoodReceiptNoteID);
        List<SalesGoodsReceiptItemBO> GetGoodReceiptNotes1(string salesOrderIDs);

        List<SalesGoodsReceiptItemBO> GetGoodReceiptNotesItems(string salesOrderIDs);
        List<SalesGoodsReceiptItemBO> GetGoodReceiptNoteItem(int GoodReceiptNoteID);

        DatatableResultBO GetGoodReceiptNoteList(string CodeHint, string DateHint, string CustomerNameHint, string NetAmountHint, string InvoiceType, string SortField, string SortOrder, int Offset, int Limit);

    }
}
