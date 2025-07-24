using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IStockConsumptionContract
    {
        List<StockConsumptionItemBO> GetStockConsumptionItems(int WarehouseID, int ItemCategoryID, int ItemID, int SalesCategoryID);
        int Save(StockConsumptionBO StockConBO, List<StockConsumptionItemBO> items);
        DatatableResultBO GetStockConsumptionList(string Type, string TransNo, string TransDate, string Store, string ItemName, string SalesCategory, string SortField, string SortOrder, int Offset, int Limit);
        List<StockConsumptionBO> GetStockConsumptionDetail(int ID);
        List<StockConsumptionItemBO> GetStockConsumptionTrans(int ID);
        
    }
}
