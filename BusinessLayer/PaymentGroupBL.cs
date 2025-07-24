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
    public class PaymentGroupBL : IPaymentGroupContract
    {
        PaymentGroupDAL paymentGroupDAL;

        public PaymentGroupBL()
        {
            paymentGroupDAL = new PaymentGroupDAL();
        }

        public List<PaymentGroupBO> GetPaymentGroupList()
        {
            return paymentGroupDAL.GetPaymentGroupList();
        }

        public List<PaymentGroupBO> GetPaymentGroupDetails(int ID)
        {
            return paymentGroupDAL.GetPaymentGroupDetails(ID);
        }

        public int Save(PaymentGroupBO PaymentGroupBO)
        {
            if (PaymentGroupBO.ID == 0)
            {
                return paymentGroupDAL.Save(PaymentGroupBO);
            }
            else
            {
                return paymentGroupDAL.Update(PaymentGroupBO);
            }
        }

    }
}
