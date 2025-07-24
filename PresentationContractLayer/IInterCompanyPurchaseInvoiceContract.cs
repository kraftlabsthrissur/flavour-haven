using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace PresentationContractLayer
{
    public interface IInterCompanyPurchaseInvoiceContract
    {
        string SaveInvoice(InterCompanyPurchaseInvoiceBO invoiceBO, List<InterCompanyPurchaseInvoiceItemBO> TransItems,List<SalesAmountBO> AmountDetails);

        List<InterCompanyPurchaseInvoiceBO> GetPurchaseInvoiceList();

        InterCompanyPurchaseInvoiceBO GetPurchaseInvoiceDetails(int ID);

        List<InterCompanyPurchaseInvoiceItemBO> GetPurchaseInvoiceTrans(int ID);

        List<SalesAmountBO> GetPurchaseInvoiceTaxDetails(int ID);

        DatatableResultBO GetInterCompanyList(string TransNoHint,string TransDateHint,string SalesInvoiceNoHint,string SalesInvoiceDateHint,string SupplierNameHint,string SortField,string SortOrder,int Offset,int Limit);

    }
}
