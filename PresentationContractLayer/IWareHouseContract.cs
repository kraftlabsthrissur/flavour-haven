using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IWareHouseContract
    {
        List<WareHouseBO> GetWareHouseList();

        List<WareHouseBO> GetWareHouses();

        List<WareHouseBO> GetWareHousesForStockRequestIssue(int ID);

        List<WareHouseBO> GetWareHousesForStockRequestReceipt(int ID);

        List<WareHouseBO> GetWareHousesByLocation(int LocationID);

        int CreateWarehouse(WareHouseBO warehouseBO);

        int EditWareHouse(WareHouseBO warehouseBO);

        WareHouseBO GetWareHouseDetails(int WareHouseID);

        DatatableResultBO GetNursingStationAutoComplete(string Hint);

    }
}
