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
    public class PaymentTypeBL : IPaymentTypeContract
    {
        PaymentTypeDAL paymentTypeDAL;

        public PaymentTypeBL()
        {
            paymentTypeDAL = new PaymentTypeDAL();

        }
        public List<PaymentTypeBO> GetPaymentTypeList()
        {
            return paymentTypeDAL.GetPaymentTypeList();
        }
        public List<PaymentTypeBO> GetPaymentTypeDetails(int PaymentTypeID)
        {
            return paymentTypeDAL.GetPaymentTypeDetails(PaymentTypeID);
        }
        public int Save(PaymentTypeBO PaymentType)
        {
            return paymentTypeDAL.Save(PaymentType);
        }
        public int Update(PaymentTypeBO PaymentType)
        {
            return paymentTypeDAL.Update(PaymentType);
        }
    }
}
