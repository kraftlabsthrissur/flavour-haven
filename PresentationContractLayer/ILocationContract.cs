using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface ILocationContract
    {
        List<LocationBO> GetLocationList();

        List<LocationBO> GetTransferableLocationList();

        List<LocationBO> GetLocationListByUser(int UserID);

        void SetCurrentLocation(int UserID, int LocationID);

        bool IsBranchLocation(int LocationID);

        List<LocationBO> GetLocationDetails(int LocationID);

        int Save(LocationBO Location,List<LocationAddressBO> Address);

        int UpdateLocation(LocationBO Location, List<LocationAddressBO> Address);

        List<LocationBO> GetSupplierLocationBySupplierID(int SupplierID);

        List<LocationBO> GetItemLocationByItemID(int ItemID);

        List<LocationBO> GetCustomerLocationMappingByCustomerID(int CustomerID);

        List<LocationBO> GetFreeMedicineLocationsByEmployeeID(int EmployeeID);

        LocationBO GetHeadLocation(int LocationID);

        List<LocationBO> GetProductionLocationList();

        List<LocationBO> GetProductionLocationMapping(int ProductionGroupID);

        List<LocationBO> GetBranchList();
        List<LocationBO> GetLocationListByLocationHead();
        List<LocationBO> getInterCompanyLocation(int LocationID);
        DatatableResultBO GetCurrencyLocationSearchList(int LocationID, string Name, string Country, string Currency, string SortField, string SortOrder, int Offset, int Limit);
        List<LocationBO> GetCurrentLocationTaxDetails();
    }
}
