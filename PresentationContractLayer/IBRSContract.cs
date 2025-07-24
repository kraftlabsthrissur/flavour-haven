using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IBRSContract
    {
        List<BRSTransBO> getStatusAsPerBooks(DateTime FromTransactionNumber, DateTime ToTransactionNumber);
        string Save(BRSBO Master, List<BRSTransBO> Details, List<BankStatementBO> Statements);
        List<BRSBO> getBRSList();
        List<BRSBO> getBRSDetails(int BRSID);
        List<BRSTransBO> getBRSTransDetails(int BRSID);
        List<BankStatementBO> getBRSBankTransDetails(int BRSID);
        bool UpdateBRS(BRSBO BRS, List<BRSTransBO> ItemList, List<BankStatementBO> StatementList);
        List<BRSTransBO> GetDataForBankReconciliation(int BankID, DateTime FromDate, DateTime ToDate);
        List<BRSBO> GetTotalBalanceAmountDetailsForBankReconciliation(int BankID, DateTime FromDate, DateTime ToDate);
        string SaveBankReconciledDateV3(List<BRSTransBO> ItemList);
        DatatableResultBO GetBRSListV3(string Type, string DocumentType, string DocumentNumber, string TransactionDate, string AccountName, string BankName, string DebitAmount, string CreditAmount, string ReconciledDate, string SortField, string SortOrder, int Offset, int Limit);
    }
}
