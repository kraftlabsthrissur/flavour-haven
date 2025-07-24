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
    public class PaymentModeBL : IPaymentModeContract
    {
        PaymentModeDAL paymentModeDAL;
        public PaymentModeBL()
        {
            paymentModeDAL = new PaymentModeDAL();

        }
        public List<PaymentModeBO> GetPaymentModeList()
        {
            return paymentModeDAL.GetPaymentModeList();
        }
        public List<PaymentModeBO> GetPaymentModeDetails(int PaymentModeID)
        {
            return paymentModeDAL.GetPaymentModeDetails(PaymentModeID);
        }
        public int Save(PaymentModeBO PaymentMode)
        {
            return paymentModeDAL.Save(PaymentMode);
        }
        public int Update(PaymentModeBO PaymentMode)
        {
            return paymentModeDAL.Update(PaymentMode);
        }
    }
}
