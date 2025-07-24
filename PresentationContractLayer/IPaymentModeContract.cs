using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IPaymentModeContract
    {
        List<PaymentModeBO> GetPaymentModeList();
        List<PaymentModeBO> GetPaymentModeDetails(int PaymentModeID);
        int Save(PaymentModeBO PaymentMode);
        int Update(PaymentModeBO PaymentMode);

    }
}
