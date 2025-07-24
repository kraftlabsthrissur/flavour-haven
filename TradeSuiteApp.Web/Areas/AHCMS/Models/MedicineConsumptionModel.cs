using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class MedicineConsumptionModel
    {
        public int RoomID { get; set; }
        public string Room { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public SelectList RoomList { get; set; }
        public SelectList TimeList { get; set; }
        public int StoreID { get; set; }
        public List<MedicinesConsumptionListModel> Medicines { get; set; }
        public List<MedicineConsumptionItemModel> Items {get;set;}
    }
    public class MedicineConsumptionItemModel
    {
        public int RoomID { get; set; }
        public string Room { get; set; }
        public string Date { get; set; }
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
        public SelectList StatusList { get; set; }
        public string BeforeOrAfterFood { get; set; }
        public string ModeOfAdminstration { get; set; }
        public string Description { get; set; }
        public int MedicineConsumptionID { get; set; }
    }

    public class MedicinesConsumptionListModel
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
        public SelectList BatchList { get; set; }
        public int BatchID { get; set; }
        public int MedicineConsumptionID { get; set;  }
    }
}