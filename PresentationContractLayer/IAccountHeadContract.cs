using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
  public  interface IAccountHeadContract
    {
        int SaveAccountHead(AccountGroupBO AccountHead);
        List<AccountGroupBO> GetAccountHeadList();
        List<AccountGroupBO> GetAccountHeadDetails(int ID);
        int UpdateAccountHead(AccountGroupBO AccountHead);
        DatatableResultBO GetAccountHeadListV3(string AccountIDHint, string AccountNameHint,string AccountGroupHint, string SortField, string SortOrder, int Offset, int Limit);}
}
