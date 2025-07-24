using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class LabTestBL : ILabTestContract
    {
        LabTestDAL labTestDAL;

        public LabTestBL()
        {
            labTestDAL = new LabTestDAL();
        }

        public DatatableResultBO GetLabTestList(string Type, string TransNo, string Date, string PatientCode, string Patient, string LabTest, string Doctor, string SortField, string SortOrder, int Offset, int Limit)
        {
            return labTestDAL.GetLabTestList(Type,TransNo, Date, PatientCode, Patient, LabTest, Doctor, SortField, SortOrder, Offset, Limit);
        }

        public List<LabTestBO> GetPatientDetails(int ID, int PatientID)
        {
            return labTestDAL.GetPatientDetails(ID, PatientID);
        }

        public List<LabTestItemBO> GetLabTestItems(int ID, int PatientLabTestMasterID,int IPID)
        {
            return labTestDAL.GetLabTestItems(ID, PatientLabTestMasterID,IPID);
        }

        public int Save(LabTestBO labtestBO, List<LabTestItemBO> Items)
        {
            return labTestDAL.Save(labtestBO, Items);
        }
        public List<LabTestBO> GetLabTestDetailsForBilling(int ID, int SalesInvoiceID)
        {
            return labTestDAL.GetLabTestDetailsForBilling(ID, SalesInvoiceID);
        }

        public List<LabTestBO> GetLabTestDetailsForPrint(int ID, int patientLabTestMasterID, int IPID)
        {
            return labTestDAL.GetLabTestDetailsForPrint(ID, patientLabTestMasterID,IPID);
        }
        public LabTestItemBO GetLabTestItemsDetails(int ID, int PatientID)
        {
            return labTestDAL.GetLabTestItemsDetails(ID, PatientID);
        }
        public List<LabTestBO> GetPatientLabTestID(int appoinmentprocessID)
        {
            return labTestDAL.GetPatientLabTestID(appoinmentprocessID);
        }
        public int SaveLabTestItems(List<LabtestsBO> LabTestItems)
        {
            return labTestDAL.SaveLabTestItems(LabTestItems);
        }
        public List<LabTestBO> GetLaboratoryInvoice(int InvoiceID)
        {
            return labTestDAL.GetLaboratoryInvoice(InvoiceID);
        }
        public List<LabTestItemBO> GetLaboratoryInvoiceItems(int InvoiceID)
        {
            return labTestDAL.GetLaboratoryInvoiceItems(InvoiceID);
        }
    }
}
