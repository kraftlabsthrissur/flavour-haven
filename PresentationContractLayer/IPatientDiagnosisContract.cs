using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PresentationContractLayer
{
   public interface IPatientDiagnosisContract
    {
        DatatableResultBO GetManagePatientList(string Type,string CodeHint,string NameHint, string TimeHint, string TokenHint,string DateHint,string SortField,string SortOrder,int Offset,int Limit);
        List<PatientDiagnosisBO> GetPatientDetails(int ID);
        List<DetailedExaminationBO> GetDetailedExaminationList(string Type);
        int Save(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems,List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList);
        List<ExaminationBO> GetExaminationList(int ID,int OPID);
        List<DetailedExaminationBO> GetOptionList(string Type);
        List<DropDownListBO> GetTherapistDetails();
        List<PatientDiagnosisBO> GetDateListByID(int ID,int OPID);
        List<PatientDiagnosisBO> GetTreatmentNumberList();
        List<DropDownListBO> GetInstructionsList();
        List<DropDownListBO> GetMedicineTimeList(string Type);
        PatientDiagnosisBO GetVitalChart(int PatientID,DateTime FromDate,int AppointmentProcessID);
        List<TreatmentBO> GetTreatmentListByID(int PatientID, DateTime FromDate,int AppointmentProcessID);
        List<ReportBO> GetReportListByID(int PatientID, DateTime FromDate, int AppointmentProcessID);
        List<ReportBO> GetReportListByIDV5(int PatientID, DateTime FromDate, int AppointmentProcessID);
        List<TreatmentItemBO> GetTreatmentMedicineListByID(int PatientID, DateTime FromDate, int AppointmentProcessID);
        List<MedicineBO> GetMedicineListByID(int OPID,int PatientID, DateTime FromDate);
        PatientDiagnosisBO GetCaseSheet(int PatientID, DateTime FromDate, int AppointmentProcessID);
        List<ExaminationBO> GetExaminationByDate(int PatientID, DateTime FromDate);
        List<ExaminationBO> GetExamination(int PatientID, DateTime FromDate, int AppointmentProcessID, int ReviewID);
        List<PatientDiagnosisBO> GetPatientDetailsItemsByID(int ID);
        List<MedicineBO> GetMedicinePrescriptionByID(int PatientID, DateTime FromDate);
        DatatableResultBO GetTreatmentDetailsListForPrint(string TransNo, string Date, string Patient, string Doctor, string Time, string TokenNo, string SortField, string SortOrder, int Offset, int Limit);
        List<MedicineBO> GetMedicineListItemsByID(int ID);
        List<TreatmentBO> GetTreatmentListItemsByID(int ID);
        DatatableResultBO GetIPPatientList(string CodeHint, string NameHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit);
        List<MedicineBO> GetMedicinesDetails(int PatientID, DateTime FromDate, int AppointmentProcessID);
        List<MedicineBO> GetAllMedicinesbyProductionGroup(int ProductionGroupID);
        List<ExaminationBO> GetDashaVidhaPareekhsalist(int PatientID,int AppointmentProcessID);
        List<MedicineItemBO> GetMedicinesItemsList(int PatientID, DateTime FromDate,int AppointmentProcessID);
        int GetAppointmentProcessID(int ID,int AppointmentScheduleItemID);
        List<LabTestItemBO> GetLabItems(int PatientID, int OPID, DateTime FromDate);
        List<XraysItemBO> GetXrayItems(int PatientID, int OPID, DateTime FromDate);
        List<LabTestItemBO> GetCategoryWiseLabItems(int[] LabTestCategoryID);
        bool IsInPatient(int PatientID);
        List<MedicineBO> GetMedicines(int PatientID);
        List<MedicineBO> GetPreviousMedicineListByID(int PatientMedicinesID);
        List<MedicineBO> GetPreviousMedicinesList(int PatientMedicinesID);
        List<MedicineItemBO> GetPreviousMedicinesItemsList(int PatientMedicinesID);
        List<ExaminationBO> GetPreviousExamination(int PatientID ,int ReviewID);
        List<HistoryBO> GetHistoryListByID(int OPID,int PatientID);
        List<HistoryBO> GetHistoryByID(int ParentID,int OPID, int PatientID);
        List<MedicineBO> GetMedicinesHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType);
        List<TreatmentBO> GetTreatmentHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType);
        List<VitalChartBO> GetVitalChartHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType);
        List<RoundsBO> GetRoundsHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType);
        DatatableResultBO GetAllLabTestList(string CodeHint, string TypeHint, string ServiceHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);
        PatientDiagnosisBO GetDepartmentItems(int AppointmentScheduleItemID);
        int EditDepartment(PatientDiagnosisBO EditDepartment);
        List<ExaminationBO> GetBaseLineInformationList(int ID, int OPID);
        bool IsPatientHistory(int AppointmentProcessID, int PatientID);
        bool StopMedicine(int PatientMedicinesID);
        List<ExaminationBO> GetBaseLineInformationDetails(int ID, int OPID);
        List<DoctorListBO> GetDoctorList(int PatientID, DateTime FromDate, int AppointmentProcessID);

        //for Ilaj 
        List<ExaminationBO> GetCaseSheetList(int ID, int OPID);
        List<ExaminationBO> GetRogaPareekshaList(int ID, int OPID);
        List<ExaminationBO> GetRogaNirnayamList(int ID, int OPID);
        int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<QuestionnaireBO> QuestionnaireItems);
        int SaveV5(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<ExaminationBO> NewItems, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<QuestionnaireBO> QuestionnaireItems);
        bool CancelPaitentTreatment(int PatientTreatmentID,int TreatmentID,DateTime Date);
    }
}
