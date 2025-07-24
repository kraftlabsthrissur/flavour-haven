using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IServiceItemReceiptContract
    {
        int Save(StockReceiptBO stockReceiptBO, List<StockReceiptItemBO> stockReceiptItems);

        List<StockReceiptItemBO> GetServiceItemReceiptTrans(int ID);

        List<StockReceiptBO> GetServiceItemReceiptDetail(int ReceiptID);
        bool IsServiceOrStockReceipt(int ID, string Type);
        DatatableResultBO GetServiceItemReceiptList(string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
