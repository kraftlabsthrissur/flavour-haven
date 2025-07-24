using BusinessObject;
using System;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface IServicePurchaseInvoice
    {

        List<SRNBO> GetUnProcessedSRNBySupplier(int supplierID);

        List<SRNTransItemBO> GetUnProcessedSRNItems(int srnID);

        List<KeyValuePair<int, string>> GetProjects();

        List<KeyValuePair<int, string>> GetCompanies();

        List<KeyValuePair<int, string>> GetDepartments();

        List<KeyValuePair<int, string>> GetLocations();

        List<KeyValuePair<int, string>> GetEmployees();

        int Save(ServicePurchaseInvoiceBO servicePurchaseInvoiceBO);

        ServicePurchaseInvoiceBO GetPurchaseInvoice(int invoiceID);

        List<ServicePurchaseInvoiceTransItemBO> GetServicePurchaseInvoiceTransItems(int servicePurchaseInvoiceID);

        List<PurchaseOrderTransBOService> GetPurchaseOrderTransDetails_Service(int ID);

        List<ServicePurchaseInvoiceTaxDetailsBO> GetServicePurchaseInvoiceTaxDetails(int servicePurchaseInvoiceID);

        bool Approve(int ID, String Status);

        int Cancel(int ID,string Table);

        decimal GetTDsAmountForUnProcessedSRNItems(string ids);

        int GetInvoiceNumberCount(string Hint, string Table, int SupplierID);

        DatatableResultBO GetPurchaseInvoiceList(string Type, string TransNoHint, string TransDateHint, string InvoiceNoHint, string InvoiceDateHint, string SupplierNameHint, string SortField, string SortOrder, int Offset, int Limit);

    }
}
