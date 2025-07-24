using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace PresentationContractLayer
{
    public interface ICustomerCreditNoteContract
    {
        List<CustomerCreditNoteBO> CreditNoteList();
        string Save(CustomerCreditNoteBO Master, List<CustomerCreditNoteTransBO> Deatils);
        List<CustomerCreditNoteBO> GetCreditNoteDetails(int CreditNoteID);
        List<CustomerCreditNoteTransBO> GetCreditNoteTransDetails(int CreditNoteID);
        string GetPrintTextFile(int ID);
        DatatableResultBO GetCustomerCreditNoteList(string Type,string TransNoNoHint,string TransDateHint,string CustomerHint,string InvoiceNoHint,string DocumentDateHint,string AmountHint,string SortField,string SortOrder,int Offset,int Limit);
    }
}
