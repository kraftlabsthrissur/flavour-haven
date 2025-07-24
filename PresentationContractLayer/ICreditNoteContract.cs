using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ICreditNoteContract
    {
        int Save(CreditNoteBO CreditNote);
        CreditNoteBO GetCreditNote(int CreditNoteID);
        DatatableResultBO GetCreditNoteList(string Type, string TransNo, string TransDate, string DebitAccount, string CreditAccount, string Amount, string SortField, string SortOrder, int Offset, int Limit);
    }
}
