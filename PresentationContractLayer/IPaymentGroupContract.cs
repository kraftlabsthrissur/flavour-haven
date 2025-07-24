using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPaymentGroupContract
    {
        List<PaymentGroupBO> GetPaymentGroupList();
        List<PaymentGroupBO> GetPaymentGroupDetails(int ID);
        int Save(PaymentGroupBO PaymentGroup);
    }
}
