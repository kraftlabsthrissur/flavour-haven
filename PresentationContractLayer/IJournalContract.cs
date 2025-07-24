using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IJournalContract 
    {
        List<JournalBO> GetCreditAccountAutoComplete(string AccountNameHint, string AccountCodeHint);
        List<JournalBO> GetDebitAccountAutoComplete(string AccountNameHint, string AccountCodeHint);
        string Save(JournalBO Master, List<JournalTransBO> Details);
        List<JournalBO> JournalList();
        List<JournalBO> GetJournalDetails(int JournalID);
        List<JournalTransBO> GetJournalTransDetails(int JournalID);
        bool Update(JournalBO Master, List<JournalTransBO> Details);
        DatatableResultBO GetJournalList(string Type,string  VoucherNoHint,string TransDateHint,string DebitAccountNameHint,string TotalDebitAmountHint,string CreditAccountNameHint, string TotalCreditAmountHint,string  SortField,string  SortOrder,int Offset,int Limit);
    }
}
