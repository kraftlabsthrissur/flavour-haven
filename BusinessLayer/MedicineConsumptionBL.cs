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
    public class MedicineConsumptionBL : IMedicineConsumptionContract
    {
        MedicineConsumptionDAL medicineConsumptionDAL;
        public MedicineConsumptionBL()
        {
            medicineConsumptionDAL = new MedicineConsumptionDAL();
        }
        public List<MedicineConsumptionBO> GetMedicineConsumptionList(DateTime Date, int StoreID, int RoomID = 0, string Time = null)
        {
            return medicineConsumptionDAL.GetMedicineConsumptionList(Date, StoreID, RoomID, Time);
        }
        public List<MedicinesConsumptionListBO> MedicinesForConsumption(int PatientMedicinesID, int StoreID)
        {
            return medicineConsumptionDAL.MedicinesForConsumption(PatientMedicinesID, StoreID);
        }
        public int Save(List<MedicineConsumptionBO> Items, List<MedicinesConsumptionListBO> Medicines)
        {
            return medicineConsumptionDAL.Save(Items, Medicines);
        }
        public DatatableResultBO GetMedicineConsumptionListForDataTable(string Type, string DateHint, string TimeHint, string PatientHint, string DoctorHint, string MedicineHint, string RoomHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return medicineConsumptionDAL.GetMedicineConsumptionListForDataTable(Type, DateHint, TimeHint, PatientHint, DoctorHint, MedicineHint,RoomHint, StatusHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
