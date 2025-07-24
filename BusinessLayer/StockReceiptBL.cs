//File created by prama on 29-6-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using BusinessLayer;

namespace BusinessLayer
{
    public class StockReceiptBL : IStockReceiptContract
    {
        StockReceiptDAL stockReceiptDAL;

        public StockReceiptBL()
        {
            stockReceiptDAL = new StockReceiptDAL();
        }

        public string SaveStockReceipt(StockReceiptBO stockReceiptBO, List<StockReceiptItemBO> stockReceiptItems)
        {
            return stockReceiptDAL.SaveStockReceipt(stockReceiptBO, stockReceiptItems);
        }

        public List<StockReceiptBO> GetStockReceiptList()
        {
            return stockReceiptDAL.GetStockReceiptList();
        }

        public List<StockReceiptBO> GetStockReceiptDetail(int ReceiptID)
        {
            return stockReceiptDAL.GetStockReceiptDetail(ReceiptID);
        }

        public List<StockReceiptItemBO> GetStockReceiptTrans(int ID)
        {
            return stockReceiptDAL.GetStockReceiptTrans(ID);
        }

        public DatatableResultBO GetStockReceiptList(string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return stockReceiptDAL.GetStockReceiptList(TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
