using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ISalesRepresentativeContract
    {
        List<SalesRepresentativeBO> GetSalesRepresentatives();
        List<SalesAreaBO> GetAreasByParentArea(int ParentAreaID);
        List<SalesAreaBO> GetAreas(int AreaID);
        int Save(SalesRepresentativeBO salesRepresentativeBO);
        bool RemoveFSO(int ID);
        DatatableResultBO GetSalesRepresentativeList(string CodeHint, string NameHint, string ParentNameHint, string AreaHint,string SalesIncentiveCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
