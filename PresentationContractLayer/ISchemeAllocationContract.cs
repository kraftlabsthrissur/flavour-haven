using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface ISchemeAllocationContract
    {
        int Save(SchemeAllocationBO schemeallocationBO);
                
        DatatableResultBO GetSchemeAllocationList(string CodeHint, string NameHint, string CustomerNameHint, string CustomerCategoryHint, string CustomerStateHint, string CustomerDistrictHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetSchemeAllocationTransList(int ID,string  NameHint,string SalesCategoryHint,string  StartDate,string  EndDate,string  SortField,string  SortOrder,int Offset,int Limit);

        SchemeAllocationBO GetSchemeAllocationDetails(int ID);

        List<SchemeItemBO> GetSchemeItemList(int ID);

        List<SchemeCustomerBO> GetSchemeCustomerList(int ID);

        List<SchemeCustomerCategoryBO> GetSchemeCategoryList(int ID);

        List<SchemeStateBO> GetSchemeStateList(int ID);

        List<SchemeDistrictBO> GetSchemeDistrictList(int ID);

    }
}