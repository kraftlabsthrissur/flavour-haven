using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ITreasuryContract
    {
        List<TreasuryBO> GetTreasuryList();

        TreasuryBO GetTreasuryDetails(int TreasuryID);

        int CreateTreasury(TreasuryBO treasuryBO);

        int EditTreasury(TreasuryBO treasuryBO);

        List<TypeBO> GetTreasuryType();

        List<TreasuryBO> GetBank(string ModuleName,string Mode);

        List<TreasuryBO> GetBank();
        List<TreasuryBO> GetReceiverBankList();
        List<TreasuryBO> GetBankList();

        List<TreasuryBO> GetAccountCodeAutoComplete(string CodeHint);

        List<TreasuryBO> GetTreasuryDetailsForAutoComplete(string Hint);

        List<TreasuryBO> GetBank(int LocationID);
        List<TreasuryBO> GetBankForCounterSales(string mode);

        DatatableResultBO GetTreasuryList(string Type,string AccountCode,string BankName,string AliasName,string CoBranchName,string BankBranchName,string AccountType,string SortField,string SortOrder,int Offset,int Limit);
    }
}
