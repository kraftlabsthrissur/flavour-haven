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
    public class DebitNoteBL : IDebitNoteContract
    {
        DebitNoteDAL debitnoteDAL;
        public DebitNoteBL()
        {
            debitnoteDAL = new DebitNoteDAL();
        }
        public int Save(DebitNoteBO CreditNote)
        {
            if (CreditNote.ID == 0)
            {
                return debitnoteDAL.Save(CreditNote);
            }
            else
            {
                return debitnoteDAL.Update(CreditNote);
            }
        }

        public DebitNoteBO GetDebitNote(int DebitNoteID)
        {
            return debitnoteDAL.GetDebitNote(DebitNoteID);
        }
        public DatatableResultBO GetDebitNoteList(string Type, string TransNo, string TransDate, string DebitAccount, string CreditAccount, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return debitnoteDAL.GetDebitNoteList(Type, TransNo, TransDate, DebitAccount, CreditAccount, Amount, SortField, SortOrder, Offset, Limit);
        }

    }
}
