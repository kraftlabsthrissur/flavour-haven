using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IAdvancePayment
    {
        List<AdvancePaymentBO> GetAdvancePaymentList(int ID);

        List<AdvancePaymentBO> GetAdvancePaymentDetails(int AdvancePaymentID);

        List<AdvancePaymentPurchaseOrderBO> GetAdvancePaymentTransDetails(int AdvancePaymentID);

        List<AdvanceRequestTransBO> GetAdvanceRequest(int EmployeeID, int IsOfficial);

        List<AdvanceRequestTransBO> GetAdvanceRequestForEdit(int ID);

        List<AdvancePaymentPurchaseOrderBO> GetPurchaseOrders(int SupplierID);

        List<AdvancePaymentPurchaseOrderBO> GetPurchaseOrdersAdvancePaymentForEdit(int ID);

        List<AdvancePaymentBO> GetUnProcessedAdvancePaymentListSupplierWise(int SupplierID, int EmployeeID);

        List<AdvancePaymentTransBO> GetUnProcessedAdvancePaymentTransList(int PaymentID);

        string GetPrintTextFile(int AdvancePaymentID);

        DatatableResultBO GetAdvancePaymentListForDataTable(string Type, string AdvancePaymentNo, string AdvancePaymentDate, string Category, string Name, string Amount, string SortField, string SortOrder, int Offset, int Limit);
    }

}
