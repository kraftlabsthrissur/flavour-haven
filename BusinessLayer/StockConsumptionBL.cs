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
    public  class StockConsumptionBL : IStockConsumptionContract
    {
        StockConsumptionDAL stockConsumtionDAL;

        public StockConsumptionBL()
        {
            stockConsumtionDAL = new StockConsumptionDAL();
        }
        public List<StockConsumptionItemBO> GetStockConsumptionItems(int WarehouseID, int ItemCategoryID, int ItemID, int SalesCategoryID)
        {
            return stockConsumtionDAL.GetStockConsumptionItems(WarehouseID, ItemCategoryID, ItemID, SalesCategoryID);
        }
        public int Save(StockConsumptionBO StockConBO, List<StockConsumptionItemBO> items)
        {
            if (StockConBO.ID == 0)
            {
                return stockConsumtionDAL.Save(StockConBO, items);
            }
            else
            {
                return stockConsumtionDAL.Update(StockConBO, items);
            }
        }
        public DatatableResultBO GetStockConsumptionList(string Type,string TransNo, string TransDate, string Store, string ItemName, string SalesCategory, string SortField, string SortOrder, int Offset, int Limit)
        {
            return stockConsumtionDAL.GetStockConsumptionList(Type,TransNo, TransDate, Store, ItemName, SalesCategory, SortField, SortOrder, Offset, Limit);
        }
        public List<StockConsumptionBO> GetStockConsumptionDetail(int ID)
        {
            return stockConsumtionDAL.GetStockConsumptionDetail(ID);
        }
        public List<StockConsumptionItemBO> GetStockConsumptionTrans(int ID)
        {
            return stockConsumtionDAL.GetStockConsumptionTrans(ID);
        }
    }
}
