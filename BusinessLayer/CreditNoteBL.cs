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
    public class CreditNoteBL : ICreditNoteContract
    {
        CreditNoteDAL creditinoteDAL;
        public CreditNoteBL()
        {
            creditinoteDAL = new CreditNoteDAL();
        }
        public int Save(CreditNoteBO CreditNote)
        {
            if (CreditNote.ID == 0)
            {
                return creditinoteDAL.Save(CreditNote);
            }
            else
            {
                return creditinoteDAL.Update(CreditNote);
            }
        }

        public CreditNoteBO GetCreditNote(int CreditNoteID)
        {
            return creditinoteDAL.GetCreditNote(CreditNoteID);
        }
        public DatatableResultBO GetCreditNoteList(string Type, string TransNo, string TransDate, string DebitAccount, string CreditAccount, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return creditinoteDAL.GetCreditNoteList(Type, TransNo, TransDate, DebitAccount, CreditAccount, Amount, SortField, SortOrder, Offset, Limit);
        }

    }
}
