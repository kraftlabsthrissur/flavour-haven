using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ITreatmentProcessContract
    {
        List<TreatmentScheduleItemBO> GetTreatmentScheduleList(DateTime Date);
        List<TreatmentScheduleBO> GetDropDownDetails();
        int Save(List<TreatmentScheduleItemBO> Items,List<TreatmentMedicineItemBO>Medicines);
        DatatableResultBO GetTreatmentProcessList(string Type,string DateHint, string StartTimeHint, string EndTimeHint, string TreatmentHint, string PatientHint, string DoctorHint,string MedicineHint, string TreatmentRoomHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit);
        List<TreatmentMedicineItemBO> GetTreatmentMedicines(int TreatmentScheduleID, int TreatmentProcessID);
        decimal GetItemStockByBatchID(int ItemID,int BatchID,int TreatmentScheduleID);
    }
}
