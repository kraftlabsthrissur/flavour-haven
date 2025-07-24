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
    public class PaymentDaysBL : IPaymentDaysContract
    {
        PaymentDaysDAL paymentDaysDAL;

        public PaymentDaysBL()
        {
            paymentDaysDAL = new PaymentDaysDAL();
        }

        public List<PaymentDaysBO> GetPaymentDaysList()
        {
            return paymentDaysDAL.GetPaymentDaysList();
        }

        public List<PaymentDaysBO> GetPaymentDaysDetails(int ID)
        {
            return paymentDaysDAL.GetPaymentDaysDetails(ID);
        }

        public int Save(PaymentDaysBO PaymentDaysBO)
        {
            if(PaymentDaysBO.ID == 0)
            {
                return paymentDaysDAL.Save(PaymentDaysBO);
            }
            else
            {
                return paymentDaysDAL.Update(PaymentDaysBO);
            }
        }
    }
}
