using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PaymentDaysDAL
    {
        public List<PaymentDaysBO> GetPaymentDaysList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPaymentDays().Select(a => new PaymentDaysBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Days = a.Days
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<PaymentDaysBO> GetPaymentDaysDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPaymentDaysDetails(ID).Select(a => new PaymentDaysBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Days = a.Days
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Save(PaymentDaysBO paymentDaysBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreatePaymentDays(
                        paymentDaysBO.Name,
                        paymentDaysBO.Days,
                        GeneralBO.CreatedUserID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(PaymentDaysBO paymentDaysBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatePaymentDays(
                        paymentDaysBO.ID, 
                        paymentDaysBO.Name,
                        paymentDaysBO.Days,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
