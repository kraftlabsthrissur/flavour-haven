using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IPayment
    {
        int SavePaymentVoucher(PaymentVoucherBO paymentVoucherBO);

        int UpdatePaymentVoucher(PaymentVoucherBO paymentVoucherBO);

        List<KeyValuePair<string, string>> GetNameByCategory(int offset, int limit, string category, string search);

        List<ItemBO> GetItemByPurchaseOrder(int purchaseOrderID, string TransNo, string hint);

        List<PayableDetailsBO> GetPayableDetailsForPaymentVoucher(int supplierID);

        int SaveAdvancePayment(AdvancePaymentBO advancePaymentBO, List<AdvancePaymentPurchaseOrderBO> advancePaymentPurchaseOrderBO);

        List<PayableDetailsBO> GetPayableDetailsForPaymentVoucherV3(int AccountHeadID);

        int SavePaymentVoucherV3(PaymentVoucherBO paymentVoucherBO);

        int UpdatePaymentVoucherV3(PaymentVoucherBO paymentVoucherBO);
    }
}
