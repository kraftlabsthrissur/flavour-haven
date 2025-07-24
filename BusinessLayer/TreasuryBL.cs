using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using BusinessObject;

namespace BusinessLayer
{
    public class TreasuryBL : ITreasuryContract
    {
        TreasuryDAL treasuryDAL;
        public TreasuryBL()
        {
            treasuryDAL = new TreasuryDAL();    
        }

        public List<TreasuryBO> GetTreasuryList()
        {
            return treasuryDAL.GetTreasuryList();
        }

        public TreasuryBO GetTreasuryDetails(int treasuryID)
        {
            return treasuryDAL.GetTreasuryDetails(treasuryID);
        }

        public int CreateTreasury(TreasuryBO treasuryBO)
        {
            return treasuryDAL.CreateTreasury(treasuryBO);
        }

        public int EditTreasury(TreasuryBO treasuryBO)
        {
            return treasuryDAL.UpdateTreasury(treasuryBO);
        }

        public List<TypeBO> GetTreasuryType()
        {
            return treasuryDAL.GetTreasuryType();
        }

          public List<TreasuryBO> GetBank(string ModuleName,string Mode)
        {
            return treasuryDAL.GetBank(ModuleName,Mode);
        }
        public List<TreasuryBO> GetBank()
        {
            return treasuryDAL.GetBank();
        }
        public List<TreasuryBO> GetReceiverBankList()
        {
            return treasuryDAL.GetReceiverBankList();
        }
        public List<TreasuryBO> GetBankList()
        {
            return treasuryDAL.GetBankList();
        }
        public List<TreasuryBO> GetAccountCodeAutoComplete(string CodeHint)
        {
            return treasuryDAL.GetAccountCodeAutoComplete(CodeHint);
        }
        public List<TreasuryBO> GetTreasuryDetailsForAutoComplete(string Hint)
        {
            return treasuryDAL.GetTreasuryDetailsForAutoComplete(Hint);
        }

        public List<TreasuryBO> GetBank(int LocationID)
        {
            return treasuryDAL.GetBank(LocationID);
        }
        public List<TreasuryBO> GetBankForCounterSales(string mode)
        {
            return treasuryDAL.GetBankForCounterSales(mode);
        }

        public DatatableResultBO GetTreasuryList(string Type, string AccountCode, string BankName, string AliasName, string CoBranchName, string BankBranchName, string AccountType, string SortField, string SortOrder, int Offset, int Limit)
        {
            return treasuryDAL.GetTreasuryList(Type, AccountCode, BankName, AliasName, CoBranchName, BankBranchName, AccountType, SortField, SortOrder, Offset, Limit);
        }
    }
}
