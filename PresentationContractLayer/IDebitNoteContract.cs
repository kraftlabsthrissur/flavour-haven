using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IDebitNoteContract
    {
        int Save(DebitNoteBO DebitNote);
        DebitNoteBO GetDebitNote(int DebitNoteID);
        DatatableResultBO GetDebitNoteList(string Type, string TransNo, string TransDate, string DebitAccount, string CreditAccount, string Amount, string SortField, string SortOrder, int Offset, int Limit);
   }
}
