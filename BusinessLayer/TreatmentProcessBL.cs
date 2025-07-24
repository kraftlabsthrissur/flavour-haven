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
    public class TreatmentProcessBL : ITreatmentProcessContract
    {
        private TreatmentProcessDAL treatmentProcessDAL;
        public TreatmentProcessBL()
        {
            treatmentProcessDAL = new TreatmentProcessDAL();
        }
        public List<TreatmentScheduleItemBO> GetTreatmentScheduleList(DateTime fromDate)
        {
            return treatmentProcessDAL.GetTreatmentScheduleList(fromDate);
        }
        public List<TreatmentScheduleBO> GetDropDownDetails()
        {
            return treatmentProcessDAL.GetDropDownDetails();
        }
        public int Save(List<TreatmentScheduleItemBO> Items, List<TreatmentMedicineItemBO> Medicines)
        {
            string[] ScheduleIDList = Items.Select(I => Convert.ToString(I.TreatmentScheduleItemID)).ToArray();
            string CommaSeparatedScheduleID = string.Join(",", ScheduleIDList.Select(x => x.ToString()).ToArray());

            return treatmentProcessDAL.Save(Items, Medicines, CommaSeparatedScheduleID);
        }

        public DatatableResultBO GetTreatmentProcessList(string Type,string DateHint, string StartTimeHint, string EndTimeHint, string TreatmentHint, string PatientHint, string DoctorHint,string MedicineHint, string TreatmentRoomHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return treatmentProcessDAL.GetTreatmentProcessList(Type,DateHint, StartTimeHint, EndTimeHint, TreatmentHint, PatientHint, DoctorHint, MedicineHint, TreatmentRoomHint, StatusHint, SortField, SortOrder, Offset, Limit);
        }
        public  List<TreatmentMedicineItemBO> GetTreatmentMedicines(int TreatmentScheduleID, int TreatmentProcessID)
        {
            return treatmentProcessDAL.GetTreatmentMedicines(TreatmentScheduleID, TreatmentProcessID);
        }
        public decimal GetItemStockByBatchID(int ItemID, int BatchID,int TreatmentScheduleID)
        {
            return GetItemStockByBatchID(ItemID, BatchID, TreatmentScheduleID);
        }
    }
}
