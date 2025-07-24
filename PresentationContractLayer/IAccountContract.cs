using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IAccountContract
    {
        List<AccountBO> GetAccountHeadsForAutoComplete(string Hint);
        List<AccountBO> GetAccountHeadsForSLAAutoComplete(string Hint);
        List<AccountBO> GetAccountHeadNameAutoComplete(string Hint);
        //DatatableResultBO GetDebitAccountHeadList( string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
