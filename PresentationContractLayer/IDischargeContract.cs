using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IDischargeContract
    {
        DatatableResultBO GetDischargeAdvicedInpatientList(string Patient, string Room, string AdmissionDate, string Doctor,string Type, string SortField, string SortOrder, int Offset, int Limit);
        List<DischargeBO> GetMedicineList(int IPID);
        DischargeBO GetDischargeSummaryDetails(int ID);
        List<DischargePatientBO> GetDischargePatientList(int IPID);
        List<DischargeMedicineBO> GetInternalMedicineList(int IPID);
        List<DischargeMedicineBO> GetDischargeMedicineList(int IPID);
        List<DischargeMedicineBO> GetTreatmentList(int IPID);
        List<DischargeBO> GetDischargeAdviceList(int IPID);
        bool IsBillPaid(int IPID);
        int Discharge(int IPID);
    }
}
