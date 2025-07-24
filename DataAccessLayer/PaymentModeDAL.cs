using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PaymentModeDAL
    {
        public List<PaymentModeBO> GetPaymentModeList()
        {
            try
            {
                List<PaymentModeBO> PaymentMode = new List<PaymentModeBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    PaymentMode = dbEntity.PaymentModes.Select(a => new PaymentModeBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }).ToList();

                    return PaymentMode;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<PaymentModeBO> GetPaymentModeDetails(int PaymentModeID)
        {
            try
            {
                List<PaymentModeBO> PaymentMode = new List<PaymentModeBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    PaymentMode = dbEntity.SpGetPaymentModeDetails(PaymentModeID).Select(a => new PaymentModeBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }).ToList();

                    return PaymentMode;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int Save(PaymentModeBO PaymentMode)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreatePaymentMode(PaymentMode.Name, GeneralBO.CreatedUserID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int Update(PaymentModeBO PaymentMode)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatePaymentMode(PaymentMode.ID, PaymentMode.Name, GeneralBO.CreatedUserID,GeneralBO.LocationID,GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
