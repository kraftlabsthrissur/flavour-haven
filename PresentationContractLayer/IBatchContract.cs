//file created by prama on 18-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{

    public interface IBatchContract
    {
        List<BatchBO> GetBatchList(int ItemID, int StoreID);
        

        DatatableResultBO GetAllBatchList(string BatchNoHint, string CustomBatchNoHint, string ItemNameHint, string ItemCategoryHint, string RetailMRPHint, string BasePriceHint, string SortField, string SortOrder, int Offset, int Limit);

        decimal GetBatchWiseStock(int BatchID, int StoreID);
        decimal GetBatchWiseStockForPackingSemiFinishedGood(int BatchID, int StoreID, int ProductionGroupID);
        List<BatchBO> GetStockableItemsListForBatch();

        List<SalesBatchBO> GetAvailableBatchesForSales(int ItemID, decimal OrderQty, int[] SalesOrderTransIDs, int WarehouseID, int CustomerID, int SchemeID,int UnitID, int ProformaInvoiceID);

        List<BatchBO> GetAvailableBatchesForSales(int ItemID, string FullOrLoose, int WarehouseID, int ItemCategoryID, int PriceListID);

        List<StockIssueBatchBO> GetAvailableBatchesForStockIssue(int ItemID, decimal RequiredQty, int WarehouseID, int[] RequestTransIDs, int BatchTypeID,int UnitID, int StockIssueID);

        List<SalesBatchBO> GetBatchesByItemIDForCounterSales(int ItemID, int WarehouseID, int BatchTypeID, int UnitID, decimal Qty);

        //int EditBatch(BatchBO batchBO);

        BatchBO GetBatchDetails(int BatchID);

        int Save(BatchBO Batch);
        int EditBatchInvoice(BatchBO Batch, List<PreviousBatchBO> PrevoiosBatch);
        List<BatchBO> GetBatchForProductionIssueMaterialReturn(int productionID, int itemID);
        List<PreProcessBatchBO> GetPreProcessItemBatchwise(int ItemID,int UnitID,decimal Quantity);
        List<BatchBO> GetBatchBatchTypeWise(int ItemID, int StoreID, int BatchTypeID);
        List<BatchBO> GetBatchesBatchTypeWise(int ItemID, int BatchTypeID);
        List<BatchBO> GetBatchesForAutoComplete(int ItemID, string Hint);

        DatatableResultBO GetBatchListForGrn(string BatchNoHint, int ItemIDHint, string SortField, string SortOrder, int Offset, int Limit);
        int CreateBatch(BatchBO Batch);
        List<BatchBO> GetLatestBatchDetails(int ItemID);
        List<PreviousBatchBO> GetPreviousBatchDetails(int BatchID);
        BatchBO GetBatchDetailsByBatchNo(string BatchNo);
        BatchBO GetBatchDetailsByBatchID(int BatchID);
        List<PreviousBatchBO> GetBatchTrans (int ID, string Type);
        DatatableResultBO GetCustomBatchForGrnAutocomplete(string BatchNoHint, int ItemIDHint, string SortField, string SortOrder, int Offset, int Limit);
        List<BatchBO> GetLatestBatchDetailsByCustomBatchNo(int ItemID, string CustomBatchNo);
        BatchBO GetStockIssueItemDetailsByQRCodeBatchNo(string BatchNo,int WarehouseID);
        List<BatchBO> GetLatestBatchDetailsV3(int ItemID);

        List<BatchBO> GetBatchListForAPI(int ItemID);
    }
}
