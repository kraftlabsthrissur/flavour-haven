using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ISupplierDebitNoteContract
    {
        List<SupplierDebitNoteBO> SupplierDebitNoteList();
        string Save(SupplierDebitNoteBO Master, List<SupplierDebitNoteTransBO> Details);
        List<SupplierDebitNoteBO> GetDebitNoteDetail(int CreditNoteID);
        List<SupplierDebitNoteTransBO> GetDebitNoteTransDetail(int CreditNoteID);
        string GetPrintTextFile(int ID);
        DatatableResultBO GetSupplierDebitNoteList(string Type, string TransNo, string TransDate, string Supplier, string ReferenceInvoiceNumber, string ReferenceDocumentDate, string TotalAmount, string SortField, string SortOrder,int Offset,int Limit);

    }
}
