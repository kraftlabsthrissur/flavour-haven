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
   public class ServiceItemReceiptBL : IServiceItemReceiptContract
    {
        ServiceItemReceiptDAL serviceItemReceiptDAL;

        public ServiceItemReceiptBL()
        {
            serviceItemReceiptDAL = new ServiceItemReceiptDAL();
        }

        public int Save(StockReceiptBO stockReceiptBO, List<StockReceiptItemBO> stockReceiptItems)
        {
            return serviceItemReceiptDAL.Save(stockReceiptBO, stockReceiptItems);
        }
        public List<StockReceiptBO> GetServiceItemReceiptDetail(int ReceiptID)
        {
            return serviceItemReceiptDAL.GetServiceItemReceiptDetail(ReceiptID);
        }

        public List<StockReceiptItemBO> GetServiceItemReceiptTrans(int ID)
        {
            return serviceItemReceiptDAL.GetServiceItemReceiptTrans(ID);
        }
        public bool IsServiceOrStockReceipt(int ID, string Type)
        {
            return serviceItemReceiptDAL.IsServiceOrStockReceipt(ID, Type);
        }
        public DatatableResultBO GetServiceItemReceiptList(string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return serviceItemReceiptDAL.GetServiceItemReceiptList(TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
