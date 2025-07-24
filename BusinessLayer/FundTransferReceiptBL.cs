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
    public class FundTransferReceiptBL : IFundTransferReceiptContract
    {
        FundTransferReceiptDAL fundTransferReceiptDAL;

        public FundTransferReceiptBL()
        {
            fundTransferReceiptDAL = new FundTransferReceiptDAL();
        }

        public DatatableResultBO GetFundTransferIssueList(int IssueLocationID, string IssueTransNoHint, string IssueLocationHint, string IssueBankDetailsHint, string ReceiptLocationHint, string ReceiptBankDetailsHint, string ModeOfPaymentHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return fundTransferReceiptDAL.GetFundTransferIssueList(IssueLocationID, IssueTransNoHint, IssueLocationHint, IssueBankDetailsHint, ReceiptLocationHint, ReceiptBankDetailsHint, ModeOfPaymentHint, AmountHint, SortField, SortOrder, Offset, Limit);
        }

        public List<FundTransferReceiptBO> GetTransferIssuedItems(int IssueID)
        {
            return fundTransferReceiptDAL.GetTransferIssuedItems(IssueID);
        }

        public int Save(FundTransferReceiptBO ReceiptBO, List<FundTransferItemBO> Items)
        {
            return fundTransferReceiptDAL.Save(ReceiptBO, Items);
        }

        public List<FundTransferReceiptBO> GetFundTransferReceiptByID(int ID)
        {
            return fundTransferReceiptDAL.GetFundTransferReceiptByID(ID);
        }

        public List<FundTransferItemBO> GetFundTransferReceiptTransByID(int ID)
        {
            return fundTransferReceiptDAL.GetFundTransferReceiptTransByID(ID);
        }

        public List<FundTransferReceiptBO> GetFundTransferReceiptList()
        {
            return fundTransferReceiptDAL.GetFundTransferReceiptList();
        }
        public DatatableResultBO GetFundTransferReceipt(string FundTransferNo, string FundTransferDate, string FromLocation, string ToLocation, string ModeOfPayment, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return fundTransferReceiptDAL.GetFundTransferReceipt(FundTransferNo, FundTransferDate, FromLocation, ToLocation, ModeOfPayment, TotalAmount, SortField, SortOrder, Offset, Limit);
        }
    }
}
