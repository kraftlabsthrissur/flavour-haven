using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IFundTransferReceiptContract
    {

        List<FundTransferReceiptBO> GetTransferIssuedItems(int IssueID);

        int Save(FundTransferReceiptBO ReceiptBO, List<FundTransferItemBO> Items);

        List<FundTransferReceiptBO> GetFundTransferReceiptList();

        List<FundTransferReceiptBO> GetFundTransferReceiptByID(int ID);

        List<FundTransferItemBO> GetFundTransferReceiptTransByID(int ID);

        DatatableResultBO GetFundTransferIssueList(int IssueLocationID, string IssueTransNoHint, string IssueLocationHint, string IssueBankDetailsHint, string ReceiptLocationHint, string ReceiptBankDetailsHint, string ModeOfPaymentHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetFundTransferReceipt(string FundTransferNo, string FundTransferDate, string FromLocation, string ToLocation, string ModeOfPayment, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit);
    }
}
