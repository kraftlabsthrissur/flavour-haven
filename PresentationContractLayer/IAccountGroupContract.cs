using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IAccountGroupContract
    {
        DatatableResultBO GetAccountGroupParentList(string NameHint,  string SortField, string SortOrder, int Offset, int Limit);
        int Save(AccountGroupBO accountGroup);
        AccountGroupBO GetAccountGroup(int ID);
        DatatableResultBO GetAccountGroupListV3(string AccountNameHint, string ParentAccountNameHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
