using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
     public interface IPaymentTypeContract
    {
        List<PaymentTypeBO> GetPaymentTypeList();
        List<PaymentTypeBO> GetPaymentTypeDetails(int PaymentType);
        int Save(PaymentTypeBO PaymentType);
        int Update(PaymentTypeBO PaymentType);
    }
}
