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
  public  class AccountHeadBL:IAccountHeadContract
    {
        AccountHeadDAL accountHeadDAL;
        public AccountHeadBL()
        {
            accountHeadDAL = new AccountHeadDAL();
        }
        public int SaveAccountHead(AccountGroupBO AccountHead)
        {
            return accountHeadDAL.SaveAccountHead(AccountHead);
        }
        public List<AccountGroupBO> GetAccountHeadList()
        {
            return accountHeadDAL.GetAccountHeadList();
        }
        public List<AccountGroupBO> GetAccountHeadDetails(int ID)
        {
            return accountHeadDAL.GetAccountHeadDetails(ID);
        }
        public int UpdateAccountHead(AccountGroupBO AccountHead)
        {
            return accountHeadDAL.UpdateAccountHead(AccountHead);
        }
        public DatatableResultBO GetAccountHeadListV3(string AccountIDHint, string AccountNameHint, string AccountGroupHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return accountHeadDAL.GetAccountHeadListV3(AccountIDHint, AccountNameHint, AccountGroupHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
