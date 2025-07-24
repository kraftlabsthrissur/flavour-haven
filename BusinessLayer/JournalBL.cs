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
    public class JournalBL : IJournalContract
    {
        JournalDAL journalDAL;
        public JournalBL()
        {
            journalDAL = new JournalDAL();
        }
        public List<JournalBO> GetCreditAccountAutoComplete(string AccountNameHint, string AccountCodeHint )
        {
            return journalDAL.GetCreditAccountAutoComplete(AccountNameHint, AccountCodeHint);
        }
        public List<JournalBO> GetDebitAccountAutoComplete(string AccountNameHint, string AccountCodeHint)
        {
            return journalDAL.GetDebitAccountAutoComplete(AccountNameHint, AccountCodeHint);
        }
        public String Save(JournalBO Master, List<JournalTransBO> Details)
        {
            return journalDAL.Save(Master, Details);
        }
        public bool Update(JournalBO Master, List<JournalTransBO> Details)
        {
            return journalDAL.Update(Master, Details);
        }
        public List<JournalBO> JournalList()
        {
            return journalDAL.JournalList();
        }
        public List<JournalBO> GetJournalDetails(int JournalID)
        {
            return journalDAL.GetJournalDetails(JournalID);
        }
        public List<JournalTransBO> GetJournalTransDetails(int JournalID)
        {
            return journalDAL.GetJournalTransDetails(JournalID);
        }
        public DatatableResultBO GetJournalList(string Type, string VoucherNoHint, string TransDateHint, string DebitAccountNameHint, string TotalDebitAmountHint, string CreditAccountNameHint, string TotalCreditAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return journalDAL.GetJournalList(Type, VoucherNoHint, TransDateHint, DebitAccountNameHint, TotalDebitAmountHint, CreditAccountNameHint, TotalCreditAmountHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
