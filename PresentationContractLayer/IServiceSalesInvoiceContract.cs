using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
   public  interface IServiceSalesInvoiceContract
    {
        
        int Save(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails);

        SalesInvoiceBO GetServiceSalesInvoice(int SalesInvoiceID, int LocationID);

        List<SalesItemBO> GetServiceSalesInvoiceItems(int SalesInvoiceID, int LocationID);        

        DatatableResultBO GetServiceSalesInvoiceList(string Type,string CodeHint, string DateHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit);

        int Cancel(int SalesInvoiceID);
    }
}
