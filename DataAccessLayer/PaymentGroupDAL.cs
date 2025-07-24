using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PaymentGroupDAL
    {

        public List<PaymentGroupBO> GetPaymentGroupList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPaymentGroup().Select(a => new PaymentGroupBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        PaymentWeek = (int)a.PaymentWeek
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PaymentGroupBO> GetPaymentGroupDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPaymentGroupDetails(ID).Select(a => new PaymentGroupBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        PaymentWeek = (int)a.PaymentWeek
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Save(PaymentGroupBO paymentGroupBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreatePaymentGroup(
                        paymentGroupBO.Name,
                        paymentGroupBO.PaymentWeek,
                        GeneralBO.CreatedUserID,
                        GeneralBO.ApplicationID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(PaymentGroupBO paymentGroupBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatePaymentGroup(
                        paymentGroupBO.ID,
                        paymentGroupBO.Name,
                        paymentGroupBO.PaymentWeek,
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
