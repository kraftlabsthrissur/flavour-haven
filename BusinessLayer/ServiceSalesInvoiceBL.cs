using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;


namespace BusinessLayer
{
    public class ServiceSalesInvoiceBL : IServiceSalesInvoiceContract
    {
        ServiceSalesInvoiceDAL serviceSalesInvoiceDAL;

        public ServiceSalesInvoiceBL()
        {
            serviceSalesInvoiceDAL = new ServiceSalesInvoiceDAL();
        }
       
        public int Save(SalesInvoiceBO Invoice, List<SalesItemBO> Items, List<SalesAmountBO> AmountDetails)
        {

            if (Invoice.ID == 0)
            {
                return serviceSalesInvoiceDAL.Save(Invoice, Items, AmountDetails);
            }
            else
            {
                return serviceSalesInvoiceDAL.Update(Invoice, Items, AmountDetails);
            }

        }

        public DatatableResultBO GetServiceSalesInvoiceList(string Type,string CodeHint, string DateHint, string CustomerNameHint, string LocationHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return serviceSalesInvoiceDAL.GetServiceSalesInvoiceList(Type,CodeHint, DateHint, CustomerNameHint, LocationHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }

        public SalesInvoiceBO GetServiceSalesInvoice(int SalesInvoiceID, int LocationID)
        {
            return serviceSalesInvoiceDAL.GetServiceSalesInvoice(SalesInvoiceID, LocationID);
        }

        public List<SalesItemBO> GetServiceSalesInvoiceItems(int SalesInvoiceID, int LocationID)
        {
            return serviceSalesInvoiceDAL.GetServiceSalesInvoiceItems(SalesInvoiceID, LocationID);
        }

        public int Cancel(int SalesInvoiceID)
        {
            return serviceSalesInvoiceDAL.Cancel(SalesInvoiceID);
        }



    }
    
}
