using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ILabTestContract
    {
        DatatableResultBO GetLabTestList(string Type, string TransNo, string Date, string PatientCode, string Patient, string LabTest, string Doctor, string SortField, string SortOrder, int Offset, int Limit);
        List<LabTestBO> GetPatientDetails(int ID, int PatientID);
        List<LabTestItemBO> GetLabTestItems(int ID, int PatientLabTestMasterID,int IPID);
        int Save(LabTestBO labtestBO, List<LabTestItemBO> Items);
        List<LabTestBO> GetLabTestDetailsForBilling(int ID, int SalesInvoiceID);
        List<LabTestBO> GetLabTestDetailsForPrint(int ID, int patientLabTestMasterID, int IPID);
        LabTestItemBO GetLabTestItemsDetails(int ID, int PatientID);
        int SaveLabTestItems(List<LabtestsBO> LabTestItems);
        List<LabTestBO> GetPatientLabTestID(int appoinmentprocessID);
        List<LabTestBO> GetLaboratoryInvoice(int InvoiceID);
        List<LabTestItemBO> GetLaboratoryInvoiceItems(int InvoiceID);
    }
}
