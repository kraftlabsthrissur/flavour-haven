using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IPaymentDaysContract
    {
        List<PaymentDaysBO> GetPaymentDaysList();

        List<PaymentDaysBO> GetPaymentDaysDetails(int ID);

        int Save(PaymentDaysBO PaymentDays);
    }
}
