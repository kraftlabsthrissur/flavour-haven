using BusinessObject;
using System;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface IStockRequestContract
    {

        bool SaveStockRequest(StockRequestBO stockRequestBO, List<StockRequestItemBO> stockRequestItems);

        int Cancel(int ID,string Table);

        List<StockRequestBO> GetStockRequestList();

        List<StockRequestBO> GetStockRequestDetail(int ID);

        List<StockRequestItemBO> GetStockRequestTrans(int ID);

        List<StockRequestBO> GetUnProcessedSRList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID);

        List<StockRequestItemBO> GetUnProcessedSRTransList(int[] StockRequisitionIDs);

        int Suspend(int ID, String Table);

        DatatableResultBO GetStockRequisitionList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit);

        List<StockRequestItemBO> ReadExcel(string Path);
    }
}
