using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ICustomerDebitNoteContract
    {
        List<CustomerDebitNoteBO> CustomerDebitNoteList();
        string Save(CustomerDebitNoteBO Master, List <CustomerDebitNoteTransBO> Details);
        List<CustomerDebitNoteBO> GetDebitNoteDetail(int DebitNoteID);
        List<CustomerDebitNoteTransBO> GetDebitNoteDetailTrans(int DebitNoteID);
        string GetPrintTextFile(int ID);
        DatatableResultBO GetCustomerDebitNoteList(string Type, string TransNoNoHint, string TransDateHint, string CustomerHint, string InvoiceNoHint, string DocumentDateHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
    