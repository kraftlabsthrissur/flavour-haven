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
    public class AccountGroupBL : IAccountGroupContract
    {
        AccountGroupDAL accountDAL;
        public AccountGroupBL()
        {
            accountDAL = new AccountGroupDAL();
        }
        public DatatableResultBO GetAccountGroupParentList(string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return accountDAL. GetAccountGroupParentList( NameHint,  SortField,  SortOrder,  Offset, Limit);
        }
        public int Save(AccountGroupBO accountGroup)
        {
            if (accountGroup.ID == 0)
            {
                return accountDAL.Save(accountGroup);
            }
            else
            {
                return accountDAL.Update(accountGroup);
            }

        }

        public AccountGroupBO GetAccountGroup(int ID)
        {
            return accountDAL.GetAccountGroup(ID);
        }

       public DatatableResultBO GetAccountGroupListV3(string AccountNameHint, string ParentAccountNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return accountDAL.GetAccountGroupListV3( AccountNameHint,  ParentAccountNameHint,  SortField,  SortOrder,  Offset,  Limit);
        }
    }
}
