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
   public class FSOBL: IFSOContract
    {
        FSODAL fSODAL;

        public FSOBL()
        {
            fSODAL = new FSODAL();
        }

        public int Save(FSOBO fsoBO, List<FSOIncentiveItemBO> FSOIncentiveItems)
        {
            string StringItems = XMLHelper.Serialize(FSOIncentiveItems);
            if (fsoBO.ID == 0)
            {
                return fSODAL.Save(fsoBO, StringItems);
            }
            else
            {
                return fSODAL.Update(fsoBO, StringItems);
            }
        }

        public List<FSOBO> GetFSOLst()
        {
            return fSODAL.GetFSOLst();
        }

        public FSOBO GetFSODetails(int ID)
        {
            return fSODAL.GetFSODetails(ID);
        }

        public bool IsFSOExist(int ID)
        {
            return fSODAL.IsFSOExist(ID);
        }

        public DatatableResultBO GetFSOList(string CodeHint, string NameHint, string SalesManagerHint, string RegionalManagerHint, string ZonalManagerHint, string AreaManagerHint, string RouteCodeHint, string RouteNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return fSODAL.GetFSOList(CodeHint, NameHint, SalesManagerHint, RegionalManagerHint, ZonalManagerHint, AreaManagerHint, RouteCodeHint, RouteNameHint, SortField, SortOrder, Offset, Limit);
        }
        public List<FSOIncentiveItemBO> GetCustomersByFilters(int StateID, int DistrictID, int CustomerCategoryID, int FSOID,int SalesIncentiveCategoryID)
        {
            return fSODAL.GetCustomersByFilters(StateID, DistrictID, CustomerCategoryID, FSOID, SalesIncentiveCategoryID);
        }
        public List<FSOIncentiveItemBO> GetCustomersByFSO(int ID)
        {
            return fSODAL.GetCustomersByFSO(ID);
        }
        public bool InactiveConfirm(int ID)
        {
            return fSODAL.InactiveConfirm(ID);
        }
        public DatatableResultBO GetFSOManagersList(string CodeHint, string NameHint, string DesignationHint, string RouteNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return fSODAL.GetFSOManagersList(CodeHint, NameHint, DesignationHint, RouteNameHint, SortField, SortOrder, Offset, Limit);
        }



    }
}
