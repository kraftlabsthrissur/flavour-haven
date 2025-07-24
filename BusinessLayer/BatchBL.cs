//File created by prama on 18-4-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class BatchBL : IBatchContract
    {

        BatchDAL batchDAL;
        public BatchBL()
        {
            batchDAL = new BatchDAL();
        }

        public List<BatchBO> GetAvailableBatchesForSales(int ItemID, string FullOrLoose, int WarehouseID, int ItemCategoryID, int PriceListID)
        {
            return batchDAL.GetAvailableBatchesForSales(ItemID, FullOrLoose, WarehouseID, ItemCategoryID, PriceListID);
        }

        public List<StockIssueBatchBO> GetAvailableBatchesForStockIssue(int ItemID, decimal RequiredQty, int WarehouseID, int[] RequestTransIDs, int BatchTypeID, int UnitID, int StockIssueID)
        {
            string CommaSeperatedIDs = string.Join(",", RequestTransIDs.Select(x => x.ToString()).ToArray());
            if (CommaSeperatedIDs == "0")
            {
                CommaSeperatedIDs = "";
            }
            return batchDAL.GetAvailableBatchesForStockIssue(ItemID, RequiredQty, WarehouseID, CommaSeperatedIDs, BatchTypeID, UnitID, StockIssueID);
        }

        public List<BatchBO> GetBatchList(int ItemID, int StoreID)
        {
            return batchDAL.GetBatchList(ItemID, StoreID);
        }
        public List<BatchBO> GetBatchBatchTypeWise(int ItemID, int StoreID, int BatchTypeID)
        {
            return batchDAL.GetBatchBatchTypeWise(ItemID, StoreID, BatchTypeID);
        }
        public List<BatchBO> GetBatchesBatchTypeWise(int ItemID, int BatchTypeID)
        {
            return batchDAL.GetBatchesBatchTypeWise(ItemID, BatchTypeID);
        }
        public decimal GetBatchWiseStock(int BatchID, int StoreID)
        {
            return batchDAL.GetBatchWiseStock(BatchID, StoreID);
        }
        public decimal GetBatchWiseStockForPackingSemiFinishedGood(int BatchID, int StoreID, int ProductionGroupID)
        {
            return batchDAL.GetBatchWiseStockForPackingSemiFinishedGood(BatchID, StoreID, ProductionGroupID);
        }
        public List<BatchBO> GetStockableItemsListForBatch()
        {
            return new List<BatchBO>(); // batchDAL.GetStockableItemsListForBatch();
        }

        public List<SalesBatchBO> GetBatchesByItemIDForCounterSales(int ItemID, int WarehouseID, int BatchTypeID, int UnitID, decimal Qty)
        {
            return batchDAL.GetBatchesByItemIDForCounterSales(ItemID, WarehouseID, BatchTypeID, UnitID, Qty);
        }

        public List<SalesBatchBO> GetAvailableBatchesForSales(int ItemID, decimal OrderQty, int[] SalesOrderTransIDs, int WarehouseID, int CustomerID, int SchemeID, int UnitID, int ProformaInvoiceID)
        {
            string CommaSeperatedIDs = string.Join(",", SalesOrderTransIDs.Select(x => x.ToString()).ToArray());
            if (CommaSeperatedIDs == "0")
            {
                CommaSeperatedIDs = "";
            }
            return batchDAL.GetAvailableBatchesForSales(ItemID, OrderQty, CommaSeperatedIDs, WarehouseID, CustomerID, SchemeID, UnitID, ProformaInvoiceID);
        }

        public DatatableResultBO GetAllBatchList(string BatchNoHint, string CustomBatchNoHint, string ItemNameHint, string ItemCategoryHint, string RetailMRPHint, string BasePriceHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return batchDAL.GetAllBatchList(BatchNoHint, CustomBatchNoHint, ItemNameHint, ItemCategoryHint, RetailMRPHint, BasePriceHint, SortField, SortOrder, Offset, Limit);
        }

        public BatchBO GetBatchDetails(int BatchID)
        {
            return batchDAL.GetbatchDetails(BatchID);
        }

        public int Save(BatchBO BatchBO)
        {
            if (BatchBO.ID > 0)
            {
                return batchDAL.Update(BatchBO);
            }
            else
            {
                return batchDAL.Save(BatchBO);
            }

        }
        public int EditBatchInvoice(BatchBO Batch, List<PreviousBatchBO> PreviousBatch)
        {
            return batchDAL.EditBatchInvoice(Batch, PreviousBatch);
        }

        public int CreateBatch(BatchBO BatchBO)
        {

            if (BatchBO.ID > 0)
            {
                return batchDAL.UpdateBatch(BatchBO);
            }
            else
            {
                return batchDAL.CreateBatch(BatchBO);
            }

        }
        public List<BatchBO> GetBatchForProductionIssueMaterialReturn(int productionID, int itemID)
        {
            return batchDAL.GetBatchForProductionIssueMaterialReturn(productionID, itemID);
        }
        public List<PreProcessBatchBO> GetPreProcessItemBatchwise(int ItemID, int UnitID, decimal Quantity)
        {
            return batchDAL.GetPreProcessItemBatchwise(ItemID, UnitID, Quantity);
        }

        public List<BatchBO> GetBatchesForAutoComplete(int ItemID, string Hint)
        {
            return batchDAL.GetBatchesForAutoComplete(ItemID, Hint);
        }

        public DatatableResultBO GetBatchListForGrn(string BatchNoHint, int ItemIDHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return batchDAL.GetBatchListForGrn(BatchNoHint, ItemIDHint, SortField, SortOrder, Offset, Limit);
        }

        public List<BatchBO> GetLatestBatchDetails(int ItemID)
        {
            return batchDAL.GetLatestBatchDetails(ItemID);
        }
        public List<PreviousBatchBO> GetPreviousBatchDetails(int BatchID)
        {
            return batchDAL.GetPreviousBatchDetails(BatchID);
        }
        public BatchBO GetBatchDetailsByBatchNo(string BatchNo)
        {
            return batchDAL.GetBatchDetailsByBatchNo(BatchNo);
        }
        public BatchBO GetBatchDetailsByBatchID(int BatchID)
        {
            return batchDAL.GetBatchDetailsByBatchID(BatchID);
        }
        public BatchBO GetStockIssueItemDetailsByQRCodeBatchNo(string BatchNo, int WarehouseID)
        {
            return batchDAL.GetStockIssueItemDetailsByQRCodeBatchNo(BatchNo, WarehouseID);
        }
        public List<PreviousBatchBO> GetBatchTrans(int ID, string Type)
        {
            return batchDAL.GetBatchTrans(ID,Type);

        }
        public DatatableResultBO GetCustomBatchForGrnAutocomplete(string BatchNoHint, int ItemIDHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return batchDAL.GetCustomBatchForGrnAutocomplete(BatchNoHint, ItemIDHint, SortField, SortOrder, Offset, Limit);
        }
        public List<BatchBO> GetLatestBatchDetailsByCustomBatchNo(int ItemID, string CustomBatchNo)
        {
            return batchDAL.GetLatestBatchDetailsByCustomBatchNo(ItemID, CustomBatchNo);
        }

        public List<BatchBO> GetLatestBatchDetailsV3(int ItemID)
        {
            return batchDAL.GetLatestBatchDetailsV3(ItemID);
        }

        public List<BatchBO> GetBatchListForAPI(int ItemID)
        {
            return batchDAL.GetBatchListForAPI(ItemID);
        }
    }
}
