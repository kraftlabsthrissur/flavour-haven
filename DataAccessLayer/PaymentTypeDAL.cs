using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PaymentTypeDAL
    {
        public List<PaymentTypeBO> GetPaymentTypeList()
        {
            try
            {
                List<PaymentTypeBO> PaymentType = new List<PaymentTypeBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    PaymentType = dbEntity.SpGetPaymentType().Select(a => new PaymentTypeBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }).ToList();

                    return PaymentType;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<PaymentTypeBO> GetPaymentTypeDetails(int PaymentModeID)
        {
            try
            {
                List<PaymentTypeBO> PaymentType = new List<PaymentTypeBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    PaymentType = dbEntity.SpGetPaymentTypeDetails(PaymentModeID).Select(a => new PaymentTypeBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }).ToList();

                    return PaymentType;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int Save(PaymentTypeBO PaymentType)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreatePaymentType(PaymentType.Name, GeneralBO.CreatedUserID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int Update(PaymentTypeBO PaymentType)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatePaymentType(PaymentType.ID, PaymentType.Name, GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
