using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
   public interface IFSOContract
    {
        int Save(FSOBO fsoBO, List<FSOIncentiveItemBO> FSOIncentiveItems);
        List<FSOBO> GetFSOLst();
        FSOBO GetFSODetails(int ID);
        bool IsFSOExist(int ID);
        List<FSOIncentiveItemBO> GetCustomersByFilters(int StateID,int DistrictID,int CustomerCategoryID,int FSOID,int SalesIncentiveCategoryID);
        DatatableResultBO GetFSOList(string CodeHint, string NameHint, string SalesManagerHint, string RegionalManagerHint, string ZonalManagerHint, string AreaManagerHint, string RouteCodeHint, string RouteNameHint, string SortField, string SortOrder, int Offset, int Limit);
        List<FSOIncentiveItemBO> GetCustomersByFSO(int ID);
        bool InactiveConfirm(int ID);
        DatatableResultBO GetFSOManagersList(string CodeHint,string NameHint,string DesignationHint,string RouteNameHint, string SortField, string SortOrder,int Offset,int Limit);
    }
}
