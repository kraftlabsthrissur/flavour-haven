using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IReceiptVoucher
    {
        bool Save(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, List<ReceiptSettlementBO> Settlements);

        List<ReceiptVoucherBO> GetReceiptList();

        ReceiptVoucherBO GetReceiptDetails(int ReceiptID);

        List<SalesInvoiceBO> GetInvoiceForReceiptVoucher(int CustomerID);

        List<ReceiptItemBO> GetReceiptTrans(int ReceiptID);

        List<ReceiptItemBO> GetReceiptTransForEdit(int ReceiptID);

        DatatableResultBO GetReceiptVoucherList(string Type,string ReceiptNoHint,string InvoiceDateHint,string CustomerHint,string ReceiptAmountHint,string SortField,string SortOrder,int Offset,int Limit);

        List<ReceiptItemBO> GetAdvanceReceiptTrans(int CustomerID);

        string GetPrintTextFile(int ReceiptID);

        //created for version3
        bool SaveV3(ReceiptVoucherBO receiptVoucherBO, List<ReceiptItemBO> receiptItemBO, List<ReceiptSettlementBO> Settlements);

        DatatableResultBO GetReceiptVoucherListV3(string Type, string ReceiptNoHint, string InvoiceDateHint, string AccountHeadHint, string ReceiptAmountHint,string ReconciledDateHint, string SortField, string SortOrder, int Offset, int Limit);

        ReceiptVoucherBO GetReceiptDetailsV3(int ReceiptID);

        List<ReceiptItemBO> GetReceiptTransV3(int ReceiptID);

        List<ReceiptItemBO> GetReceiptTransForEditV3(int ReceiptID);

        int SaveReconciledDate(int ID, DateTime ReconciledDate, string BankReferanceNumber, string Remarks);
    }
}
