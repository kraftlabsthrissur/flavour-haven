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
   public class SalesRepresentativeBL: ISalesRepresentativeContract
    {
        SalesRepresentativeDAL salesRepresentativeDAL;

        public SalesRepresentativeBL()
        {
            salesRepresentativeDAL = new SalesRepresentativeDAL();
        }
        public List<SalesRepresentativeBO> GetSalesRepresentatives()
        {
            return salesRepresentativeDAL.GetSalesRepresentatives();
        }
        public List<SalesAreaBO> GetAreasByParentArea(int ParentAreaID)
        {
            return salesRepresentativeDAL.GetAreasByParentArea(ParentAreaID);
        }
        public List<SalesAreaBO> GetAreas(int AreaID)
        {
            return salesRepresentativeDAL.GetAreas(AreaID);
        }
        public int Save(SalesRepresentativeBO salesRepresentativeBO)
        {
            return salesRepresentativeDAL.Save(salesRepresentativeBO);
        }
        public  bool RemoveFSO(int ID)
        {
            return salesRepresentativeDAL.RemoveFSO(ID);
        }
        public DatatableResultBO GetSalesRepresentativeList(string CodeHint, string NameHint, string ParentNameHint, string AreaHint,string SalesIncentiveCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return salesRepresentativeDAL.GetSalesRepresentativeList(CodeHint, NameHint, ParentNameHint, AreaHint, SalesIncentiveCategoryHint,  SortField, SortOrder, Offset, Limit);
        }
    }
}
