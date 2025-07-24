//File Created by prama on 19-4-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IStockIssueContract
    {
        bool SaveStockIssue(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems, List<StockIssuePackingDetailsBO> PackingDetails);

        List<StockIssueBO> GetStockIssueList();

        List<StockIssueBO> GetStockIssueDetail(int ID);

        int Cancel(int ID,string Table);

        List<StockIssueItemBO> GetIssueTrans(int ID);

        List<StockIssuePackingDetailsBO> GetPackingDetails(int ID, string Type);

        List<StockIssueBO> GetUnProcessedSIList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID);

        List<StockIssueItemBO> GetUnProcessedSITransList(int ID);

        List<StockIssueItemBO> GetBatchwiseItem(int ItemID, int BatchTypeID, decimal Qty, int WarehouseID,int UnitID);

        DatatableResultBO GetStockIssueList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit);

        List<StockIssueItemBO> ReadExcel(string Path);

        List<StockIssueItemBO> GetItemsToGrid(List<StockIssueItemBO> stockIssueItems,int IssuePremiseID);

        List<StockIssueBO> GetIssueNoAutoCompleteForReport(string CodeHint,DateTime FromDate,DateTime ToDate);

        string GetPrintTextFile(int StockIssueID);
    }
}
