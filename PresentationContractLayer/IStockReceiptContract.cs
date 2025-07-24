using BusinessObject;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface IStockReceiptContract
    {
        string SaveStockReceipt(StockReceiptBO stockReceiptBO, List<StockReceiptItemBO> stockReceiptItems);      

        List<StockReceiptBO> GetStockReceiptList();

        List<StockReceiptItemBO> GetStockReceiptTrans(int ID);

        List<StockReceiptBO> GetStockReceiptDetail(int ReceiptID);

        DatatableResultBO GetStockReceiptList(string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
