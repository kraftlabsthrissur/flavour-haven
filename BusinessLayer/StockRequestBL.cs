//file Created by prama on 9-4-2018
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
    public class StockRequestBL : IStockRequestContract
    {
        StockRequestDAL stockRequestDAL;

        public StockRequestBL()
        {
            stockRequestDAL = new StockRequestDAL();
        }

        public bool SaveStockRequest(StockRequestBO stockRequestBO, List<StockRequestItemBO> stockRequestItems)
        {
            if (stockRequestBO.ID > 0)
            {
                return stockRequestDAL.UpdateStockRequest(stockRequestBO, stockRequestItems);
            }
            else
            {
                return stockRequestDAL.SaveStockRequest(stockRequestBO, stockRequestItems);
            }
        }

        public List<StockRequestBO> GetStockRequestList()
        {
            return stockRequestDAL.GetStockRequestList();
        }

        public List<StockRequestBO> GetStockRequestDetail(int ID)
        {
            return stockRequestDAL.GetStockRequestDetail(ID);
        }

        public List<StockRequestItemBO> GetStockRequestTrans(int ID)
        {
            return stockRequestDAL.GetStockRequestTrans(ID);
        }

        public List<StockRequestBO> GetUnProcessedSRList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID)
        {
            return stockRequestDAL.GetUnProcessedSRList(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID);
        }

        public List<StockRequestItemBO> GetUnProcessedSRTransList(int[] StockRequisitionIDs)
        {
            string CommaSeperatedIDs = string.Join(",", StockRequisitionIDs.Select(x => x.ToString()).ToArray());
            return stockRequestDAL.GetUnProcessedSRTransList(CommaSeperatedIDs);
        }

        public int Suspend(int ID, String Table)
        {
            return stockRequestDAL.Suspend(ID, Table);
        }

        public int Cancel(int ID, string Table)
        {
            return stockRequestDAL.Cancel(ID, Table);
        }

        public DatatableResultBO GetStockRequisitionList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return stockRequestDAL.GetStockRequisitionList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
        }

        public List<StockRequestItemBO> ReadExcel(string Path)
        {
            ItemContract ItemBL = new ItemBL();
            ItemBO Item;
            IDictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(1, "Code");        
            dict.Add(3, "Name");
            dict.Add(4, "BatchType");
            dict.Add(5, "AverageSales");
            dict.Add(6, "Stock");
            dict.Add(7, "SuggestedQty");
            dict.Add(8, "RequiredQty");
            StockRequestItemBO UploadItemList = new StockRequestItemBO();
            GeneralBL generalBL = new GeneralBL();
            List<StockRequestItemBO> ItemList;
            try
            {
                ItemList = generalBL.ReadExcel(Path, UploadItemList, dict);
                ItemList = ItemList.Where(a => a.Code != "" && a.RequiredQty > 0).ToList();
                var ItemCodes = ItemList.Select(a => a.Code).Distinct();
                List<ItemBO> Items = ItemBL.GetItemsByItemCodes(ItemCodes.ToArray());

                ItemList.Select(a =>
                    {
                        Item = Items.Where(b => b.Code == a.Code).FirstOrDefault();
                        a.BatchTypeID = a.BatchType == "OSK" ? 2 : 1;
                        a.ItemID = Item.ID;
                        a.Unit = Item.Unit;
                        a.UnitID = Item.UnitID;
                        a.SalesCategory = Item.SalesCategoryName;
                        return a;
                    }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return ItemList.ToList();
        }

    }
}
