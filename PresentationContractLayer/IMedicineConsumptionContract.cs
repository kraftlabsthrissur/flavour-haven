using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IMedicineConsumptionContract
    {
        List<MedicineConsumptionBO> GetMedicineConsumptionList(DateTime Date, int StoreID, int RoomID=0, string Time=null);
        List<MedicinesConsumptionListBO> MedicinesForConsumption(int PatientMedicinesID, int StoreID);
        int Save(List<MedicineConsumptionBO> Items,List<MedicinesConsumptionListBO> Medicines);
        DatatableResultBO GetMedicineConsumptionListForDataTable(string Type,string DateHint,string TimeHint,string PatientHint,string DoctorHint,string MedicineHint,string RoomHint,string StatusHint,string SortField,string SortOrder,int Offset,int Limit);
    }
}
