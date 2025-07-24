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
    public class AccountBL : IAccountContract
    {
        AccountDAL accountDAL;
        public AccountBL()
        {
            accountDAL = new AccountDAL();
        }
        public List<AccountBO> GetAccountHeadsForAutoComplete(string Hint)
        {
            return accountDAL.GetAccountHeadsForAutoComplete(Hint);
        }
        public List<AccountBO> GetAccountHeadsForSLAAutoComplete(string Hint)
        {
            return accountDAL.GetAccountHeadsForSLAAutoComplete(Hint);
        }
        public List<AccountBO> GetAccountHeadNameAutoComplete(string Hint)
        {
            return accountDAL.GetAccountHeadNameAutoComplete(Hint);
        }
        //public DatatableResultBO GetDebitAccountHeadList()
        //{
        //    return accountDAL.GetDebitAccountHeadList();
        //}
    }
}
