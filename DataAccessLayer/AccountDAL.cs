using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AccountDAL
    {
        public List<AccountBO> GetAccountHeadsForAutoComplete(string hint)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    return dbEntity.SpGetDebitAccountHead(hint).Select(a => new AccountBO
                    {
                        ID = a.ID,
                        AccountId = a.AccountId,
                        AccountName= a.AccountName,
                        GroupClassification = a.GroupClassification,
                        OpeningAmt = (decimal)a.OpeningAmt
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AccountBO> GetAccountHeadsForSLAAutoComplete(string AccountCodeHint)
        {
            List<AccountBO> AccountHead = new List<AccountBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    AccountHead = dbEntity.SpGetAccountHeadForSLA(AccountCodeHint, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AccountBO
                    {
                        ID = a.ID,
                        AccountId = a.AccountID,
                        AccountName = a.AccountName,
                    }).ToList();
                    return AccountHead;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AccountBO> GetAccountHeadNameAutoComplete(string AccountNameHint)
        {
            List<AccountBO> AccountHead = new List<AccountBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    AccountHead = dbEntity.SpGetAccountHeadName(AccountNameHint, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new AccountBO
                    {
                        ID = a.ID,
                        AccountId = a.AccountID,
                        AccountName = a.AccountName,
                    }).ToList();
                    return AccountHead;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
