//File created by prama on 14-4-18
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
    public class LocationBL : ILocationContract
    {
        LocationDAL locationDAL;

        public LocationBL()
        {
            locationDAL = new LocationDAL();
        }

        public List<LocationBO> GetLocationList()
        {
            return locationDAL.GetLocationList();
        }

        public List<LocationBO> GetLocationListByUser(int UserID)
        {
            return locationDAL.GetLocationListByUser(UserID);
        }

        public void SetCurrentLocation(int UserID, int LocationID)
        {
            locationDAL.SetCurrentLocation(UserID, LocationID);
        }

        public bool IsBranchLocation(int LocationID)
        {
            return locationDAL.IsBranchLocation(LocationID);
        }

        public List<LocationBO> GetLocationDetails(int LocationID)
        {
            return locationDAL.GetLocationDetails(LocationID);
        }

        public int Save(LocationBO Location, List<LocationAddressBO> Address)
        {
            return locationDAL.Save(Location, Address);
        }

        public int UpdateLocation(LocationBO Location, List<LocationAddressBO> Address)
        {
            return locationDAL.Update(Location, Address);
        }

        public List<LocationBO> GetTransferableLocationList()
        {
            return locationDAL.GetTransferableLocationList();
        }

        public List<LocationBO> GetSupplierLocationBySupplierID(int SupplierID)
        {
            return locationDAL.GetSupplierLocationBySupplierID(SupplierID);
        }

        public List<LocationBO> GetItemLocationByItemID(int ItemID)
        {
            return locationDAL.GetItemLocationByItemID(ItemID);
        }

        public List<LocationBO> GetCustomerLocationMappingByCustomerID(int CustomerID)
        {
            return locationDAL.GetCustomerLocationMappingByCustomerID(CustomerID);
        }

        public List<LocationBO> GetFreeMedicineLocationsByEmployeeID(int EmployeeID)
        {
            return locationDAL.GetFreeMedicineLocationsByEmployeeID(EmployeeID);
        }

        public LocationBO GetHeadLocation(int LocationID)
        {
            return locationDAL.GetHeadLocation(LocationID);
        }

        public List<LocationBO> GetProductionLocationList()
        {
            return locationDAL.GetProductionLocationList();
        }
        public List<LocationBO> GetLocationListByLocationHead()
        {
            return locationDAL.GetLocationListByLocationHead();
        }
        public List<LocationBO> getInterCompanyLocation(int LocationID)
        {
            return locationDAL.getInterCompanyLocation(LocationID);
        }

        public List<LocationBO> GetProductionLocationMapping(int ProductionGroupID)
        {
            return locationDAL.GetProductionLocationMapping(ProductionGroupID);
        }

        public List<LocationBO> GetBranchList()
        {
            return locationDAL.GetBranchList();
        }
        public DatatableResultBO GetCurrencyLocationSearchList(int LocationID, string Name, string Country, string Currency, string SortField, string SortOrder, int Offset, int Limit)
        {
            return locationDAL.GetCurrencyLocationSearchList(LocationID, Name, Country, Currency, SortField, SortOrder, Offset, Limit);
        }
        public List<LocationBO> GetCurrentLocationTaxDetails()
        {
            return locationDAL.GetCurrentLocationTaxDetails(GeneralBO.LocationID);
        }
    }
}
