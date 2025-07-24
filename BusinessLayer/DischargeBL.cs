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
   public class DischargeBL:IDischargeContract
    {
        DischargeDAL dischargeDAL;
        public DischargeBL()
        {
            dischargeDAL = new DischargeDAL();
        }
        public DatatableResultBO GetDischargeAdvicedInpatientList(string Patient, string Room, string AdmissionDate, string Doctor, string Type, string SortField, string SortOrder, int Offset, int Limit)
        {
            return dischargeDAL.GetDischargeAdvicedInpatientList(Patient, Room, AdmissionDate, Doctor,Type, SortField, SortOrder, Offset, Limit);
        }
        public List<DischargeBO> GetMedicineList(int IPID)
        {
            return dischargeDAL.GetMedicineList(IPID);
        }
        public DischargeBO GetDischargeSummaryDetails(int IPID)
        {
            return dischargeDAL.GetDischargeSummaryDetails(IPID);
        }
        public List<DischargePatientBO> GetDischargePatientList(int IPID)
        {
            return dischargeDAL.GetDischargePatientList(IPID);
        }
        public List<DischargeMedicineBO> GetInternalMedicineList(int IPID)
        {
            return dischargeDAL.GetInternalMedicineList(IPID);
        }
        public List<DischargeMedicineBO> GetDischargeMedicineList(int IPID)
        {
            return dischargeDAL.GetDischargeMedicineList(IPID);
        }
        public List<DischargeMedicineBO> GetTreatmentList(int IPID)
        {
            return dischargeDAL.GetTreatmentList(IPID);
        }
        public bool IsBillPaid(int IPID)
        {
            return dischargeDAL. IsBillPaid(IPID);
        }
        public int Discharge(int IPID)
        {
            return dischargeDAL.Discharge(IPID);
        }
        public List<DischargeBO> GetDischargeAdviceList(int IPID)
        {
            return dischargeDAL.GetDischargeAdviceList(IPID);
        }
    }
}
