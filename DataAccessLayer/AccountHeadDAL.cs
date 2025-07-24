using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class AccountHeadDAL
    {
        public int SaveAccountHead(AccountGroupBO AccountHead)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                ObjectParameter ID = new ObjectParameter("ID", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    //var j = dbEntity.SpUpdateSerialNo(AccountHead.AccountGroupName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                     dbEntity.SpCreateAccountHead(AccountHead.AccountID, AccountHead.AccountName, AccountHead.AccountGroupID, AccountHead.OpeningAmount,AccountHead.OpeningAmountType,
                        GeneralBO.CreatedUserID, GeneralBO.ApplicationID,ID);
                    return Convert.ToInt32(ID.Value);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public List<AccountGroupBO> GetAccountHeadList()
        {
            try
            {
                List<AccountGroupBO> AccountHead = new List<AccountGroupBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    AccountHead = dbEntity.SpGetAccountHeads().Select(a => new AccountGroupBO
                    {
                        ID=a.ID,
                        AccountID=a.AccountID,
                        AccountName=a.AccountName,
                        AccountGroupName=a.GroupName
                    }).ToList();

                    return AccountHead;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AccountGroupBO> GetAccountHeadDetails(int ID)
        {
            try
            {
                List<AccountGroupBO> AccountHead = new List<AccountGroupBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    AccountHead = dbEntity.SpGetAccountHeadDetails(ID).Select(a => new AccountGroupBO
                    {
                     ID=a.ID,
                     AccountID=a.AccountID,
                     AccountName=a.AccountName,
                     AccountGroupName=a.AccountGroup,
                     AccountGroupID=(int)a.ParentID,
                     OpeningAmount=(decimal)a.OpeningAmount,
                     OpeningAmountType=a.DoubleEntryType
                    }).ToList();
                    return AccountHead;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int UpdateAccountHead(AccountGroupBO AccountHead)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpUpdateAccountHead(AccountHead.ID, AccountHead.AccountName,AccountHead.AccountID,
                       AccountHead.AccountGroupID,AccountHead.OpeningAmount,AccountHead.OpeningAmountType, GeneralBO.CreatedUserID,GeneralBO.ApplicationID);
                    return AccountHead.ID;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetAccountHeadListV3(string AccountIDHint, string AccountNameHint, string AccountGroupHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAccountHeadListV3(AccountIDHint, AccountNameHint, AccountGroupHint,SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                AccountID = item.AccountID,
                                AccountName = item.AccountName,
                                GroupName=item.AccountGroup,
                                AccountGroupID = item.AccountGroupID,
                                CurrencyConversionRate = item.CurrencyConversionRate,
                                CurrencyCode = item.CurrencyCode,
                                CurrencyName = item.CurrencyName,
                                CurrencyID = item.CurrencyID,
                                CurrencyPrefix = item.CurrencyPrefix,
                                DecimalPlaces = item.DecimalPlaces,

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

    }
}
