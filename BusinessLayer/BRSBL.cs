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
    public class BRSBL : IBRSContract
    {
        BRSDAL brsDAL;
        public BRSBL()
        {
            brsDAL = new BRSDAL();

        }
        public List<BRSTransBO> getStatusAsPerBooks(DateTime FromTransactionDate , DateTime ToTransactionDate )
        {
            return brsDAL.getStatusAsPerBooks(FromTransactionDate, ToTransactionDate);
        }
        public String Save(BRSBO Master, List<BRSTransBO> Details, List<BankStatementBO> Statements)
        {
            return brsDAL.Save(Master, Details, Statements);
        }
        public List<BRSBO> getBRSList()
        {
            return brsDAL.getBRSList();
        }
        public List<BRSBO> getBRSDetails(int BRSID)
        {
            return brsDAL.getBRSDetails(BRSID);
        }
        public List<BRSTransBO> getBRSTransDetails(int BRSID)
        {
            return brsDAL.getBRSTransDetails(BRSID);
        }
     public List<BankStatementBO> getBRSBankTransDetails(int BRSID)
        {
            return brsDAL.getBRSBankTransDetails(BRSID);
        }
        public bool UpdateBRS(BRSBO BRS,List<BRSTransBO> ItemList,List<BankStatementBO> StatementList)
        {
            return brsDAL.UpdateBRS(BRS, ItemList, StatementList);
        }
        public List<BRSTransBO> GetDataForBankReconciliation(int BankID, DateTime FromDate, DateTime ToDate)
        {
            return brsDAL.GetDataForBankReconciliation(BankID, FromDate, ToDate);
        }
        public string SaveBankReconciledDateV3(List<BRSTransBO> ItemList)
        {
            return brsDAL.SaveBankReconciledDateV3(ItemList);
        }
        public DatatableResultBO GetBRSListV3(string Type, string DocumentType, string DocumentNumber, string TransactionDate, string AccountName, string BankName, string DebitAmount, string CreditAmount, string ReconciledDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return brsDAL.GetBRSListV3(Type, DocumentType, DocumentNumber, TransactionDate, AccountName, BankName, DebitAmount, CreditAmount, ReconciledDate, SortField, SortOrder, Offset, Limit);
        }
        public List<BRSBO> GetTotalBalanceAmountDetailsForBankReconciliation(int BankID, DateTime FromDate, DateTime ToDate)
        {
            return brsDAL.GetTotalBalanceAmountDetailsForBankReconciliation(BankID, FromDate, ToDate);
        }
    }

}
