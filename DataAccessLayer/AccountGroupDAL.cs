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
   public class AccountGroupDAL
    {
        public DatatableResultBO GetAccountGroupParentList(string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetAccountGroupParentList( NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                AccountName = item.AccountName
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }


        public int Save(AccountGroupBO AccountGroup)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                ObjectParameter AccountGroupID = new ObjectParameter("AccountGroupID", typeof(int));
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                var j = dbEntity.SpUpdateSerialNo("AccountGroup", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = dbEntity.SpCreateAccountGroup(
                           SerialNo.Value.ToString(),
                           AccountGroup.AccountGroupName,
                           AccountGroup.AccountHeadCodePrefix,
                           AccountGroup.ParentGroupID,
                           AccountGroup.IsAllowAccountsUnder,
                           GeneralBO.CreatedUserID,
                           GeneralBO.FinYear,
                           GeneralBO.ApplicationID,
                           AccountGroupID
                            );
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        int ID = Convert.ToInt32(AccountGroupID.Value);
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int Update(AccountGroupBO AccountGroup)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        var i = dbEntity.SpUpdateAccountGroup(
                           AccountGroup.ID,
                           AccountGroup.Code,
                           AccountGroup.AccountGroupName,
                           AccountGroup.AccountHeadCodePrefix,
                           AccountGroup.ParentGroupID,
                           AccountGroup.IsAllowAccountsUnder,
                           GeneralBO.CreatedUserID,
                           GeneralBO.ApplicationID
                            );
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public AccountGroupBO GetAccountGroup(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetAccountGroup(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new AccountGroupBO
                    {
                        Code = k.Code,
                        AccountGroupName = k.AccountName,
                        IsAllowAccountsUnder = (bool)k.IsAllowAccountsUnder,
                        AccountHeadCodePrefix = k.AccountHeadCodePrefix,
                        ParentGroupID = k.ParentID,
                        ParentGroup = k.ParentAccountName,
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetAccountGroupListV3( string AccountNameHint, string ParentAccountNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAccountGroupList(AccountNameHint, ParentAccountNameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID=item.ID,
                                AccountID = item.AccountID,
                                AccountName = item.AccountName,
                                ParentAccount = item.ParentAccount
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
