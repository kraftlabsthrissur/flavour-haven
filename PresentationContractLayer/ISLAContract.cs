using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ISLAContract
    {
        List<SLAValuesBO> GetUnMappedValuesToSLA();

        List<SLAPostedBO> GetAccountsPostedValues(DateTime FromDate, DateTime ToDate);

        List<SLAToBePostedBO> GetAccountsToBePostedValues(DateTime FromDate, DateTime ToDate);

        List<SLAError> GetErrorsDuringSLAMapping();

        bool CreateAccountEntryDetails();

        bool GenerateAccountEntryDataUsingSLARules();

        List<SLAValuesBO> GetTransTypeAutoComplete(string hint);

        List<SLABO> GetSLAKeyValueByTransactionType(string TransactionType);

        List<SLABO> GetSLAFilterByType(string Type);

        List<SLABO> GetProcessCycleList();

        List<SLABO> GetTransactionType(string ProcessCycle);

        int CreateSLA(SLABO slaBO);

        SLABO GetSLADetails(int SLAID);

        DatatableResultBO GetAllSLAList(string CycleHint, string TransactionTypeHint, string KeyValueHint, string ItemHint,string SupplierHint, string CustomerHint, string SortField, string SortOrder, int Offset, int Limit);
        
        DatatableResultBO GetSLAValuesList(string Type,string DateHint,string TransationTypeHint,string KeyValueHint,string AmountHint,string EventHint,string DocumentTableHint,string DocumentNumberHint,string SortField,string SortOrder,int Offset,int Limit);

        DatatableResultBO GetSLAToBePostedList(DateTime FromDate,DateTime ToDate, string Type,string DateHint,string DebitAccountHint,string DebitAccountNameHint,string CreditAccountHint,string CreditAccountNameHint,string AmountHint,string ItemNameHint,string DocumentTableHint,string SupplierNameHint,string DocumentNumberHint,string SortField,string SortOrder, int Offset,int Limit);

        DatatableResultBO GetSLAPostedList(DateTime FromDate, DateTime ToDate, string Type, string DateHint, string DebitAccountHint, string DebitAccountNameHint, string CreditAccountHint, string CreditAccountNameHint, string AmountHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetSLAErrorList(DateTime FromDate, DateTime ToDate,string Type,string DateHint, string TransationTypeHint,string KeyValueHint,string EventHint,string ItemNameHint,string SupplierNameHint,string DescriptionHint,string RemarksHint,string DocumentTableHint,string DocumentNumberHint,string SortField,string SortOrder, int Offset,int Limit);

    }
}
