using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class ServicePurchaseInvoiceBL : IServicePurchaseInvoice
    {
        ServicePurchaseInvoiceRepository servicePurchaseInvoiceDAL;

        public ServicePurchaseInvoiceBL()
        {
            servicePurchaseInvoiceDAL = new ServicePurchaseInvoiceRepository();
        }

        public bool Approve(int ID, string Status)
        {
            return servicePurchaseInvoiceDAL.Approve(ID, Status);
        }

        public int Cancel(int ID, string Table)
        {
            return servicePurchaseInvoiceDAL.Cancel(ID, Table);
        }

        public List<KeyValuePair<int, string>> GetCompanies()
        {
            return servicePurchaseInvoiceDAL.GetCompanies();
        }

        public List<KeyValuePair<int, string>> GetDepartments()
        {
            return servicePurchaseInvoiceDAL.GetDepartments();
        }

        public List<KeyValuePair<int, string>> GetEmployees()
        {
            return servicePurchaseInvoiceDAL.GetEmployees();
        }

        public int GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            return servicePurchaseInvoiceDAL.GetInvoiceNumberCount(Hint, Table, SupplierID);
        }

        public List<KeyValuePair<int, string>> GetLocations()
        {
            return servicePurchaseInvoiceDAL.GetLocations();
        }

        public List<KeyValuePair<int, string>> GetProjects()
        {
            return servicePurchaseInvoiceDAL.GetProjects();
        }

        public ServicePurchaseInvoiceBO GetPurchaseInvoice(int invoiceID)
        {
            return servicePurchaseInvoiceDAL.GetPurchaseInvoice(invoiceID);
        }

        public DatatableResultBO GetPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string InvoiceNoHint, string InvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return servicePurchaseInvoiceDAL.GetPurchaseInvoiceList(Type, TransNoHint, TransDateHint, InvoiceNoHint, InvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
        }

        public List<PurchaseOrderTransBOService> GetPurchaseOrderTransDetails_Service(int ID)
        {
            return servicePurchaseInvoiceDAL.GetPurchaseOrderTransDetails_Service(ID);
        }

        public List<ServicePurchaseInvoiceTaxDetailsBO> GetServicePurchaseInvoiceTaxDetails(int servicePurchaseInvoiceID)
        {
            return servicePurchaseInvoiceDAL.GetServicePurchaseInvoiceTaxDetails(servicePurchaseInvoiceID);
        }

        public List<ServicePurchaseInvoiceTransItemBO> GetServicePurchaseInvoiceTransItems(int servicePurchaseInvoiceID)
        {
            return servicePurchaseInvoiceDAL.GetServicePurchaseInvoiceTransItems(servicePurchaseInvoiceID);
        }

        public decimal GetTDsAmountForUnProcessedSRNItems(string ids)
        {
            return servicePurchaseInvoiceDAL.GetTDsAmountForUnProcessedSRNItems(ids);
        }

        public List<SRNBO> GetUnProcessedSRNBySupplier(int supplierID)
        {
            return servicePurchaseInvoiceDAL.GetUnProcessedSRNBySupplier(supplierID);
        }

        public List<SRNTransItemBO> GetUnProcessedSRNItems(int srnID)
        {
            return servicePurchaseInvoiceDAL.GetUnProcessedSRNItems(srnID);
        }

        public int Save(ServicePurchaseInvoiceBO servicePurchaseInvoiceBO)
        {
            return servicePurchaseInvoiceDAL.Save(servicePurchaseInvoiceBO);
        }
    }
}
