using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ILaboratoryTestResultContract
    {
        DatatableResultBO GetInvoicedLabTestList(string Type, string InvoiceNo, string InvoiceDate, string Patient, string Doctor, string NetAmt, string SortField, string SortOrder, int Offset, int Limit);
        List<LaboratoryTestResultBO> GetInvoicedLabTestItems(int SalesInvoiceID);
        int Save(List<LaboratoryTestResultBO> Items);
    }
}
