using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IPaymentReturnVoucherContract
    {
        bool Save(PaymentReturnVoucherBO Master, List<PaymentReturnVoucherItemBO> Details);
        List<PaymentReturnVoucherBO> GetpaymentReturnVoucherDetails(int PaymentReturnID);
        List<PaymentReturnVoucherItemBO> GetPaymentReturnTransDetails(int PaymentReturnID);
        DatatableResultBO GetPaymentReturnVoucherList(string Type, string VoucherNoHint, string VoucherDateHint, string SupplierNameHint, string PaymentHint, string ReturnAmountHint, string SortField, string SortOrder, int Offset, int Limit);
        List<DebitNoteBO> GetDebitNoteListForPaymentReturn(int SupplierID);

    }
}
