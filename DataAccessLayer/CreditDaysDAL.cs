using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CreditDaysDAL
    {
        public List<CreditDaysBO> GetCreditDaysList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCreditDays().Select(a => new CreditDaysBO
                    {
                        ID = a.ID,
                        Name = a.CreditDays,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<CreditDaysBO> GetCreditDaysDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCreditDaysDetails(ID).Select(a => new CreditDaysBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Days = (int)a.Days
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int Save(CreditDaysBO creditDaysBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateCreditDays(
                        creditDaysBO.Name,
                        creditDaysBO.Days, 
                        GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int Update(CreditDaysBO creditDaysBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateCreditDays(
                        creditDaysBO.ID,
                        creditDaysBO.Name,
                        creditDaysBO.Days,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
