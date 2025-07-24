using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPaymentVoucher
    {
        List<PaymentVoucherBO> GetPaymentVoucher(int ID);

        List<PaymentVoucherItemBO> GetPaymentVoucherTrans(int ID);
        List<PaymentVoucherItemBO> GetPaymentVoucherTransForEdit(int ID);
        List<PaymentVoucherBO> GetPaymentVoucherDetail(int ID);

        List<PayableDetailsBO> GetDocumentAutoComplete(string term);
        DatatableResultBO GetPaymentVoucherList(string Type, string VoucherNumber, string VoucherDate, string SupplierName, string Amount, string SortField, string SortOrder, int Offset, int Limit);

        string GetPrintTextFile(int ID);
        List<PaymentVoucherBO> GetPaymentVoucherDetailV3(int ID);
        List<PaymentVoucherItemBO> GetPaymentVoucherTransForEditV3(int ID);

        List<PaymentVoucherItemBO> GetPaymentVoucherTransV3(int ID);
        DatatableResultBO GetPaymentVoucherListV3(string Type, string VoucherNumber, string VoucherDate, string AccountHead, string Amount,string ReconciledDate, string SortField, string SortOrder, int Offset, int Limit);
        int  SaveReconciledDate(int ID, DateTime ReconciledDate, string BankReferanceNumber, string Remarks);
    }
}
