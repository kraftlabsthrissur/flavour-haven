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
    public class ChartOfAccountsDAL
    {
        public List<ChartOfAccountBO> GetChartOfAccountList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetChartOfAccounts().Select(a => new ChartOfAccountBO
                {
                    ID = a.ID,
                    AccountID = a.AccountID,
                    AccountName = a.AccountName,
                    ParentID = (int)a.ParentID,
                    Level = (int)a.Level,
                    IsManual = (bool)a.IsManual,
                    OpeningAmount = (decimal)a.OpeningAmount
                }).ToList();

            }
        }

        public int Save(ChartOfAccountBO chartOfAccountBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateChartOfAccount(
                chartOfAccountBO.AccountID,
                chartOfAccountBO.AccountName,
                chartOfAccountBO.OpeningAmount,
                chartOfAccountBO.IsManual,
                chartOfAccountBO.ParentID,
                chartOfAccountBO.Level,
                GeneralBO.CreatedUserID,
                GeneralBO.ApplicationID,
                ReturnValue
                );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Account Code Already Exist");
                }
            }
            return 1;
        }

        public List<ChartOfAccountBO> GetAccountHeadList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetAccountHeadList().Select(a => new ChartOfAccountBO
                {
                    ID = a.ID,
                    AccountID = a.AccountID,
                    AccountName = a.AccountName,
                    ParentID = (int)a.ParentID,
                    OpeningAmount = (decimal)a.OpeningAmount,
                    Level = (int)a.Level,
                    ParentName = a.ParentName,
                    ParentAccountCode = a.ParentAccountCode
                }).ToList();

            }
        }

        public int Update(ChartOfAccountBO chartOfAccountBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateChartOfAccounts(
                        chartOfAccountBO.ID,
                        chartOfAccountBO.AccountName,
                        chartOfAccountBO.OpeningAmount,
                        chartOfAccountBO.IsManual,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsRemovedItem(int ID)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpIsRemovedChartOfAccounts(
                   ID,
                   ReturnValue);
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Delete failed");
                }
            }
            return true;
        }

    }
}
