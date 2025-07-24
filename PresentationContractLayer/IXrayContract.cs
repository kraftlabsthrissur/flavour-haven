using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IXrayContract
    {
        //DatatableResultBO GetXrayList(string Date, string PatientCode, string Patient, string Xray, string Doctor, string SortField, string SortOrder, int Offset, int Limit);
        //List<XraysBO> GetPatientDetails(int ID);
        //List<XraysItemBO> GetXrayItems(int ID);
        //int Save(List<XraysItemBO> Items);
        DatatableResultBO GetXrayList(string Date, string PatientCode, string Patient, string LabTest, string Doctor, string SortField, string SortOrder, int Offset, int Limit);
        List<LabTestBO> GetPatientDetails(int ID, int PatientID);
        List<LabTestItemBO> GetXrayItems(int ID, int PatientLabTestMasterID, int IPID);
        int Save(LabTestBO labtestBO, List<LabTestItemBO> Items);
        List<LabTestBO> GetLabTestDetailsForBilling(int ID, int SalesInvoiceID);
        List<LabTestBO> GetXrayDetailsForPrint(int ID, int patientLabTestMasterID, int IPID);
        LabTestItemBO GetLabTestItemsDetails(int ID, int PatientID);
        int SaveLabTestItems(List<LabtestsBO> LabTestItems);
        List<LabTestBO> GetPatientLabTestID(int appoinmentprocessID);
    }
}
