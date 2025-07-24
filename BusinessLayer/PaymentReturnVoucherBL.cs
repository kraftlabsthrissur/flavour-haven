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
    public class PaymentReturnVoucherBL : IPaymentReturnVoucherContract
    {
        PaymentReturnVoucherDAL paymentReturnVoucherDAL;

        public PaymentReturnVoucherBL()
        {
            paymentReturnVoucherDAL = new PaymentReturnVoucherDAL();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Master"></param>
        /// <param name="Details"></param>
        /// <returns></returns>
        public bool Save (PaymentReturnVoucherBO Master, List<PaymentReturnVoucherItemBO> Details)
        {
            if(Master.ID>0)
            {
                return paymentReturnVoucherDAL.Update(Master, Details);
            }
            else
            {
                return paymentReturnVoucherDAL.Save(Master, Details);
            }
        }

        public List<PaymentReturnVoucherBO> GetpaymentReturnVoucherDetails(int PaymentReturnID)
        {
            return paymentReturnVoucherDAL.GetPaymentReturnVoucherDetails(PaymentReturnID);
        }

        public List<PaymentReturnVoucherItemBO> GetPaymentReturnTransDetails(int PaymentReturnID)
        {
            return paymentReturnVoucherDAL.GetPaymentReturnTransDetails(PaymentReturnID);
        }
        public DatatableResultBO GetPaymentReturnVoucherList(string Type, string VoucherNoHint, string VoucherDateHint, string SupplierNameHint, string PaymentHint, string ReturnAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return paymentReturnVoucherDAL.GetPaymentReturnVoucherList(Type, VoucherNoHint, VoucherDateHint, SupplierNameHint, PaymentHint, ReturnAmountHint, SortField, SortOrder, Offset, Limit);
        }
        public List<DebitNoteBO> GetDebitNoteListForPaymentReturn(int SupplierID)
        {
            return paymentReturnVoucherDAL.GetDebitNoteListForPaymentReturn(SupplierID).ToList();
        }
    }
}
