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
   public class XrayBL: IXrayContract
    {
        XrayDAL xrayDAL;

        public XrayBL()
        {
            xrayDAL = new XrayDAL();
        }

        public DatatableResultBO GetXrayList(string Date, string PatientCode, string Patient, string LabTest, string Doctor, string SortField, string SortOrder, int Offset, int Limit)
        {
            return xrayDAL.GetXrayList(Date, PatientCode, Patient, LabTest, Doctor, SortField, SortOrder, Offset, Limit);
        }

        public List<LabTestBO> GetPatientDetails(int ID, int PatientID)
        {
            return xrayDAL.GetPatientDetails(ID, PatientID);
        }

        public List<LabTestItemBO> GetXrayItems(int ID, int PatientLabTestMasterID, int IPID)
        {
            return xrayDAL.GetXrayItems(ID, PatientLabTestMasterID, IPID);
        }

        public int Save(LabTestBO labtestBO, List<LabTestItemBO> Items)
        {
            return xrayDAL.Save(labtestBO, Items);
        }
        public List<LabTestBO> GetLabTestDetailsForBilling(int ID, int SalesInvoiceID)
        {
            return xrayDAL.GetLabTestDetailsForBilling(ID, SalesInvoiceID);
        }

        public List<LabTestBO> GetXrayDetailsForPrint(int ID, int patientLabTestMasterID, int IPID)
        {
            return xrayDAL.GetXrayDetailsForPrint(ID, patientLabTestMasterID, IPID);
        }
        public LabTestItemBO GetLabTestItemsDetails(int ID, int PatientID)
        {
            return xrayDAL.GetLabTestItemsDetails(ID, PatientID);
        }
        public List<LabTestBO> GetPatientLabTestID(int appoinmentprocessID)
        {
            return xrayDAL.GetPatientLabTestID(appoinmentprocessID);
        }
        public int SaveLabTestItems(List<LabtestsBO> LabTestItems)
        {
            return xrayDAL.SaveLabTestItems(LabTestItems);
        }
        //XrayDAL xrayDAL;

        //public XrayBL()
        //{
        //    xrayDAL = new XrayDAL();
        //}

        //public DatatableResultBO GetXrayList(string Date, string PatientCode, string Patient, string Xray, string Doctor, string SortField, string SortOrder, int Offset, int Limit)
        //{
        //    return xrayDAL.GetXrayList(Date, PatientCode, Patient, Xray, Doctor, SortField, SortOrder, Offset, Limit);
        //}

        //public List<XraysBO> GetPatientDetails(int ID)
        //{
        //    return xrayDAL.GetPatientDetails(ID);
        //}

        //public List<XraysItemBO> GetXrayItems(int ID)
        //{
        //    return xrayDAL.GetXrayItems(ID);
        //}

        //public int Save(List<XraysItemBO> Items)
        //{
        //    return xrayDAL.Save(Items);
        //}

    }
}
