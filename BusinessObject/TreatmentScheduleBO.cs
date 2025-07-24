using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class TreatmentScheduleBO
    {
        public int ID { get; set; }
        public int TreatmentRoomID { get; set; }
        public string TreatmentRoom { get; set; }
        public string Duration { get; set; }
        public int TherapistID { get; set; }
        public string TherapistName { get; set; }
        public int DurationID { get; set; }
        public int TreatmentStatusID { get; set; }
        public string TreatmentStatus { get; set; }
        public List<TreatmentScheduleItemBO> Items { get; set; }
    }
    public class TreatmentScheduleItemBO
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public int TreatmentID { get; set; }
        public int PatientTreatmentTransID { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string TreatmentName { get; set; }
        public int NoOfTreatment { get; set; }
        public int TreatmentRoomID { get; set; }
        public string TreatmentRoom { get; set; }
        public string StartTime { get; set; }
        public string Duration { get; set; }
        public int TherapistID { get; set; }
        public string TherapistName { get; set; }
        public string StartDate { get; set; }
        public int DurationID { get; set; }
        public string EndTime { get; set; }
        public int StatusID { get; set; }
        public string Remarks { get; set; }
        public int TreatmentScheduleItemID { get; set; }

        public int PreferedTreatmentRoomID { get; set; }
        public string PreferedTreatmentRoom { get; set; }
        public int PreferedTherapistID { get; set; }
        public string PreferedTherapist { get; set; }
        public string Status { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int ScheduleID { get; set; }
        public int TotalTreatmentNo { get; set; }

        public int AppointmentProcessID { get; set; }
        public DateTime Date { get; set; }
        public int TreatmentProcessID { get; set; }

    }
    public class TreatmentMedicineItemBO
    {
        public int ItemID { get; set; }
        public string Item { get; set; }
        public decimal Qty { get; set; }
        public decimal Stock { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public int TreatmentScheduleID { get;set;}
        public int TreatmentProcessID { get; set; }
        public int ProductionGroupID { get; set; }
        public int StoreID { get; set; }
        public int BatchID { get; set; }
    }


}
