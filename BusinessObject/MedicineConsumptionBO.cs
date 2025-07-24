using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class MedicineConsumptionBO
    {
        public int RoomID { get; set; }
        public string Room { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientMedicinesID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string Medicine { get; set; }
        public string ActiualTime { get; set; }
        public int IPID { get; set; }
        public string BeforeOrAfterFood { get; set; }
        public string ModeOfAdminstration { get; set; }
        public string Description { get; set; }
        public int MedicineConsumptionID { get; set; }
    }
    public class MedicinesConsumptionListBO
    {
        public int ItemID { get; set; }
        public string Item { get; set; }
        public decimal Qty { get; set; }
        public decimal Stock { get; set; }
        public int UnitID { get; set; }
        public string Unit { get; set; }
        public int PatientMedicinesID { get; set; }
        public int ProductionGroupID { get; set; }
        public int StoreID { get; set; }
        public int BatchID { get; set; }
        public int MedicineConsumptionID { get; set; }
    }
}
