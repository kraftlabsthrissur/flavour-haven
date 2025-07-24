using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IServiceItemIssueContract
    {
       decimal GetTradeDiscountPercent();
       bool Save(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems);
       DatatableResultBO GetServiceItemIssueList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit);
       List<StockIssueBO> GetServiceItemIssueDetail(int ID);
       List<StockIssueItemBO> GetServiceItemIssueTrans(int ID);
       bool IsServiceOrStockIssue(int ID,string Type);
       List<StockIssueBO> GetUnProcessedServiceItemIssueList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID);

    }
}
