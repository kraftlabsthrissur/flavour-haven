using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class SLABL : ISLAContract
    {
        SLADAL slaDAL;
        public SLABL()
        {
            slaDAL = new SLADAL();
        }

        public bool CreateAccountEntryDetails()
        {
            return slaDAL.CreateAccountEntryDetails();
        }

        public bool GenerateAccountEntryDataUsingSLARules()
        {
            return slaDAL.GenerateAccountEntryDataUsingSLARules();
        }

        public List<SLAPostedBO> GetAccountsPostedValues(DateTime FromDate, DateTime ToDate)
        {
            return slaDAL.GetAccountsPostedValues(FromDate, ToDate);
        }

        public List<SLAToBePostedBO> GetAccountsToBePostedValues(DateTime FromDate, DateTime ToDate)
        {
            return slaDAL.GetAccountsToBePostedValues(FromDate, ToDate);
        }

        public List<SLAError> GetErrorsDuringSLAMapping()
        {
            return slaDAL.GetErrorsDuringSLAMapping();
        }

        public List<SLAValuesBO> GetUnMappedValuesToSLA()
        {
            return slaDAL.GetUnMappedValuesToSLA();
        }
        public List<SLAValuesBO> GetTransTypeAutoComplete(string hint)
        {
            return slaDAL.GetTransTypeAutoComplete(hint);
        }

        public List<SLABO> GetSLAKeyValueByTransactionType(string TransactionType)
        {
            return slaDAL.GetSLAKeyValueByTransactionType(TransactionType);
        }
        public List<SLABO> GetSLAFilterByType(string Type)
        {
            return slaDAL.GetSLAFilterByType(Type);
        }
        public List<SLABO> GetProcessCycleList()
        {
            return slaDAL.GetProcessCycleList();
        }       
        public List<SLABO> GetTransactionType(string ProcessCycle)
        {
            return slaDAL.GetTransactionTypeByProcessCycle(ProcessCycle);
        }
        public int CreateSLA(SLABO slaBO)
        {
            if (slaBO.ID == 0)
            {
                return slaDAL.CreateSLA(slaBO);
            }
            else
            {
                return slaDAL.UpdateSLA(slaBO);
            }
        }
        public SLABO GetSLADetails(int SLAID)
        {
            return slaDAL.GetSLADetails(SLAID);
        }

        public DatatableResultBO GetAllSLAList(string CycleHint, string TransactionTypeHint, string KeyValueHint, string ItemHint, string SupplierHint, string CustomerHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return slaDAL.GetAllSLAList(CycleHint, TransactionTypeHint, KeyValueHint, ItemHint, SupplierHint, CustomerHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetSLAValuesList(string Type, string DateHint, string TransationTypeHint, string KeyValueHint, string AmountHint, string EventHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return slaDAL.GetSLAValuesList(Type,DateHint,TransationTypeHint,KeyValueHint,AmountHint,EventHint,DocumentTableHint,DocumentNumberHint,SortField,SortOrder,Offset,Limit);
        }

        public DatatableResultBO GetSLAToBePostedList(DateTime FromDate, DateTime ToDate, string Type, string DateHint, string DebitAccountHint, string DebitAccountNameHint, string CreditAccountHint, string CreditAccountNameHint, string AmountHint, string ItemNameHint, string SupplierNameHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return slaDAL.GetSLAToBePostedList(FromDate,ToDate, Type, DateHint, DebitAccountHint, DebitAccountNameHint, CreditAccountHint, CreditAccountNameHint, AmountHint, ItemNameHint, SupplierNameHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetSLAPostedList(DateTime FromDate, DateTime ToDate, string Type, string DateHint, string DebitAccountHint, string DebitAccountNameHint, string CreditAccountHint, string CreditAccountNameHint, string AmountHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return slaDAL.GetSLAPostedList(FromDate, ToDate, Type, DateHint, DebitAccountHint, DebitAccountNameHint, CreditAccountHint, CreditAccountNameHint, AmountHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetSLAErrorList(DateTime FromDate, DateTime ToDate, string Type, string DateHint, string TransationTypeHint, string KeyValueHint, string EventHint, string ItemNameHint, string SupplierNameHint, string DescriptionHint, string RemarksHint, string DocumentTableHint, string DocumentNumberHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return slaDAL.GetSLAErrorList(FromDate, ToDate, Type, DateHint, TransationTypeHint, KeyValueHint, EventHint, ItemNameHint, SupplierNameHint, DescriptionHint, RemarksHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
