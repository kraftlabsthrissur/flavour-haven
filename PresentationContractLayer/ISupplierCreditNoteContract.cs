using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ISupplierCreditNoteContract
    {
        List<SupplierCreditNoteBO> SupplierCreditNoteList();
        string Save(SupplierCreditNoteBO Master, List<SupplierCreditNoteTransBO> Details);
        List<SupplierCreditNoteBO> GetCreditNoteDetail(int CreditNoteID);
        List<SupplierCreditNoteTransBO> GetCreditNoteDetailTrans(int CreditNoteID);
        string GetPrintTextFile(int ID);
        DatatableResultBO GetSupplierCreditNoteList(string Type, string TransNo, string TransDate, string Supplier, string ReferenceInvoiceNumber, string ReferenceDocumentDate, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit);
    }
}
