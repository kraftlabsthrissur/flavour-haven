using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using System.Diagnostics;

namespace BusinessLayer
{
    public class StockAdjustmentBL : IStockAdjustmentContract
    {
        StockAdjustmentDAL stockAdjustmentDAL;

        public StockAdjustmentBL()
        {
            stockAdjustmentDAL = new StockAdjustmentDAL();
        }

        public List<StockAdjustmentItemBO> GetStockAdjustmentItems(int WarehouseID, int ItemCategoryID, int ItemID, int SalesCategoryID)
        {
            return stockAdjustmentDAL.GetStockAdjustmentItems(WarehouseID, ItemCategoryID, ItemID, SalesCategoryID);
        }
        public List<StockAdjustmentItemBO> GetStockAdjustmentItemsForAlopathy(DateTime FromDate, DateTime ToDate, int ItemID,int StockAjustmentPremise)
        {
            return stockAdjustmentDAL.GetStockAdjustmentItemsForAlopathy(FromDate, ToDate, ItemID, StockAjustmentPremise);
        }
        public List<StockAdjustmentItemBO> GetBatchesByItemIDForStockAdjustment(int WarehouseID, int ItemID, int BatchTypeID)
        {
            return stockAdjustmentDAL.GetBatchesByItemIDForStockAdjustment(WarehouseID, ItemID, BatchTypeID);
        }
        public int Save(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items)
        {
            if (StockAdjBO.ID == 0)
            {
                return stockAdjustmentDAL.Save(StockAdjBO, items);
            }
            else
            {
                return stockAdjustmentDAL.Update(StockAdjBO, items);
            }
        }
        public int SaveV3(StockAdjustmentBO StockAdjBO, List<StockAdjustmentItemBO> items)
        {
            if (StockAdjBO.ID == 0)
            {
                return stockAdjustmentDAL.SaveV3(StockAdjBO, items);
            }
            else
            {
                return stockAdjustmentDAL.UpdateV3(StockAdjBO, items);
            }
        }
        public int Revert(List<StockAdjustmentItemBO> items)
        {
            return stockAdjustmentDAL.Revert(items);

        }
        public List<StockAdjustmentBO> GetStockAdjustmentList()
        {
            return stockAdjustmentDAL.GetStockAdjustmentList();
        }
        public List<StockAdjustmentItemBO> GetStockAdjustmentTrans(int ID)
        {
            return stockAdjustmentDAL.GetStockAdjustmentTrans(ID);
        }
        public List<StockAdjustmentBO> GetStockAdjustmentDetail(int ID)
        {
            return stockAdjustmentDAL.GetStockAdjustmentDetail(ID);
        }

        public List<StockAdjustmentItemBO> ReadExcel(string Path)
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(0, "ItemCode");
            dict.Add(1, "ItemName");
            dict.Add(2, "UnitName");
            dict.Add(3, "Batch");
            dict.Add(4, "BatchType");
            dict.Add(5, "WareHouse");
            dict.Add(6, "ExpiryDate");
            dict.Add(7, "CurrentQty");
            dict.Add(8, "PhysicalQty");

            StockAdjustmentItemBO UploadStockList = new StockAdjustmentItemBO();
            GeneralBL generalBL = new GeneralBL();
            List<StockAdjustmentItemBO> StockAdjustmentList;
            List<StockAdjustmentItemBO> StockList;
            try
            {
                StockAdjustmentList = generalBL.ReadExcel(Path, UploadStockList, dict);
                StockList = StockAdjustmentList.Where(o => o.CurrentQty < o.PhysicalQty).ToList();
                string StringItems = XMLHelper.Serialize(StockList);
                StockAdjustmentList = stockAdjustmentDAL.ProcessUploadedStockAdjustment(StringItems);
            }
            catch (Exception e)
            {
                throw e;
            }
            return StockAdjustmentList;
        }

        public DatatableResultBO GetStockAdjustmentList(string TransNo, string TransDate, string Store, string ItemName, string SalesCategory, string SortField, string SortOrder, int Offset, int Limit)
        {
            return stockAdjustmentDAL.GetStockAdjustmentList(TransNo, TransDate, Store, ItemName, SalesCategory, SortField, SortOrder, Offset, Limit);
        }
        public List<StockAdjustmentItemBO> GetScheduledStockItems(DateTime FromDate, DateTime ToDate)
        {
            return stockAdjustmentDAL.GetScheduledStockItems(FromDate, ToDate);
        }
        public int SaveStockAdjustmentForAPI(StockAdjustmentBO StockAdjBO)
        {
            return stockAdjustmentDAL.SaveStockAdjustmentForAPI(StockAdjBO);
        }
        public List<StockAdjustmentItemBO> GetStockAdjustmentItemsForAlopathyAPI(DateTime FromDate, DateTime ToDate, int ItemID)
        {
            return stockAdjustmentDAL.GetStockAdjustmentItemsForAlopathyAPI(FromDate, ToDate, ItemID);
        }
        //public int GetIsStockCheckingDone(string ItemCode,string Batch)
        //{
        //    return stockAdjustmentDAL.GetIsStockCheckingDone(ItemCode, Batch);
        //}
        public StockAdjustmentBO GetIsStockCheckingDone(string ItemCode, string Batch)
        {
            return stockAdjustmentDAL.GetIsStockCheckingDone(ItemCode, Batch);
        }
    }
}
