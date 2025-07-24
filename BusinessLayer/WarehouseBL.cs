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
    public class WarehouseBL : IWareHouseContract
    {
        WarehouseDAL warehouseDAL;

        public WarehouseBL()
        {
            warehouseDAL = new WarehouseDAL();
        }

        public List<WareHouseBO> GetWareHouseList()
        {
            return warehouseDAL.GetWareHouseList();
        }

        public List<WareHouseBO> GetWareHouses()
        {
            return warehouseDAL.GetWareHouses();
        }

        public List<WareHouseBO> GetWareHousesForStockRequestReceipt(int ID)
        {
            return warehouseDAL.GetWareHousesForStockRequestReceipt(ID);
        }

        public List<WareHouseBO> GetWareHousesForStockRequestIssue(int ID)
        {
            return warehouseDAL.GetWareHousesForStockRequestIssue(ID);
        }

        public List<WareHouseBO> GetWareHousesByLocation(int LocationID)
        {
            return warehouseDAL.GetWareHousesByLocation(LocationID);
        }

        public int CreateWarehouse(WareHouseBO warehouseBO)
        {
            return warehouseDAL.CreateWareHouse(warehouseBO);
        }

        public WareHouseBO GetWareHouseDetails(int WareHouseID)
        {
            return warehouseDAL.GetWareHouseDetails(WareHouseID);
        }

        public int EditCountry(WareHouseBO wareHouseBO)
        {
            return warehouseDAL.UpdateWareHouse(wareHouseBO);
        }

        public int EditWareHouse(WareHouseBO warehouseBO)
        {
            return warehouseDAL.UpdateWareHouse(warehouseBO);
        }

        public DatatableResultBO GetNursingStationAutoComplete(string Hint)
        {
            return warehouseDAL.GetNursingStationAutoComplete(Hint);
        }
    }
}
