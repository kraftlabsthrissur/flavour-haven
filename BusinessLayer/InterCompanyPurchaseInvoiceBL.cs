using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class InterCompanyPurchaseInvoiceBL : IInterCompanyPurchaseInvoiceContract
    {
        InterCompanyPurchaseInvoiceDAL InvoiceDAL;
        public InterCompanyPurchaseInvoiceBL()
        {
            InvoiceDAL = new InterCompanyPurchaseInvoiceDAL();
        }

        public string SaveInvoice(InterCompanyPurchaseInvoiceBO invoiceBO, List<InterCompanyPurchaseInvoiceItemBO> TransItems, List<SalesAmountBO> AmountDetails)
        {
            return InvoiceDAL.SaveInvoice(invoiceBO, TransItems, AmountDetails);
        }

        public List<InterCompanyPurchaseInvoiceBO> GetPurchaseInvoiceList()
        {
            return InvoiceDAL.GetPurchaseInvoiceList();
        }

        public InterCompanyPurchaseInvoiceBO GetPurchaseInvoiceDetails(int ID)
        {
            return InvoiceDAL.GetPurchaseInvoiceDetails(ID);
        }

        public List<InterCompanyPurchaseInvoiceItemBO> GetPurchaseInvoiceTrans(int ID)
        {
            return InvoiceDAL.GetPurchaseInvoiceTrans(ID);
        }

        public List<SalesAmountBO> GetPurchaseInvoiceTaxDetails(int ID)
        {
            return InvoiceDAL.GetPurchaseInvoiceTaxDetails(ID);
        }

        public DatatableResultBO GetInterCompanyList(string TransNoHint, string TransDateHint, string SalesInvoiceNoHint, string SalesInvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return InvoiceDAL.GetInterCompanyList(TransNoHint, TransDateHint, SalesInvoiceNoHint, SalesInvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
