using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IDirectSalesInvoiceContract
    {
        int Save(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails, List<SalesPackingDetailsBO> PackingDetails);
        DatatableResultBO GetDirectSalesInvoiceList(string Type, string CodeHint, string DateHint, string SalesTypeHint, string CustomerNameHint, string LocationHint, string DoctorHint,string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);
        SalesInvoiceBO GetDirectSalesInvoice(int SalesInvoiceID, int LocationID);
        List<SalesItemBO> GetDirectSalesInvoiceItems(int SalesInvoiceID, int LocationID);
        List<SalesAmountBO> GetDirectSalesInvoiceAmountDetails(int SalesInvoiceID, int LocationID);
        List<SalesPackingDetailsBO> GetDirectSalesInvoicePackingDetails(int SalesInvoiceID, int LocationID);
        List<SalesInvoiceBO> GetSalesType();
        List<SalesInvoiceBO> GetPatientTypeList();

        List<SalesItemBO> GetMedicineIssueItemsForDirectSalesInvoice(string MedicineIssueType, int MedicineIssuedToID);

        int SaveV3(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails, List<SalesPackingDetailsBO> PackingDetails);
    }
}
