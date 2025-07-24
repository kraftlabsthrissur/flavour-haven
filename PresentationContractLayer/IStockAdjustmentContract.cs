using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IStockAdjustmentContract
    {

        List<StockAdjustmentItemBO> GetStockAdjustmentItems(int WarehouseID, int ItemCategoryID, int ItemID,int SalesCategoryID);
        List<StockAdjustmentItemBO> GetStockAdjustmentItemsForAlopathy(DateTime FromDate, DateTime ToDate, int ItemID, int StockAjustmentPremise);
        int Save(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items);
        List<StockAdjustmentBO> GetStockAdjustmentList();
        List<StockAdjustmentItemBO> GetStockAdjustmentTrans(int ID);
        List<StockAdjustmentBO> GetStockAdjustmentDetail(int ID);
        List<StockAdjustmentItemBO> GetBatchesByItemIDForStockAdjustment(int WarehouseID, int ItemID,int BatchTypeID);
        List<StockAdjustmentItemBO> ReadExcel(string Path);
        int SaveV3(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items);

        DatatableResultBO GetStockAdjustmentList(string TransNo,string TransDate,string Store,string ItemName, string SalesCategory,string SortField,string SortOrder,int Offset,int Limit);
        List<StockAdjustmentItemBO> GetScheduledStockItems(DateTime FromDate, DateTime ToDate);
        int Revert(List<StockAdjustmentItemBO> items);

        int SaveStockAdjustmentForAPI(StockAdjustmentBO StockAdjBO);

        List<StockAdjustmentItemBO> GetStockAdjustmentItemsForAlopathyAPI(DateTime FromDate, DateTime ToDate, int ItemID);
        //int GetIsStockCheckingDone(string ItemCode, string Batch);
        StockAdjustmentBO GetIsStockCheckingDone(string ItemCode, string Batch);

    }
}
