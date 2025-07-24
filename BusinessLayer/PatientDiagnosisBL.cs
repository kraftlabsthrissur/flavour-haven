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
    public class PatientDiagnosisBL : IPatientDiagnosisContract
    {
        PatientDiagnosisDAL managePatientDAL;

        public PatientDiagnosisBL()
        {
            managePatientDAL = new PatientDiagnosisDAL();
        }

        public DatatableResultBO GetManagePatientList(string Type, string CodeHint, string NameHint, string TimeHint, string TokenHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return managePatientDAL.GetManagePatientList(Type, CodeHint, NameHint, TimeHint, TokenHint, DateHint, SortField, SortOrder, Offset, Limit);
        }

        public List<PatientDiagnosisBO> GetPatientDetails(int ID)
        {
            return managePatientDAL.GetPatientDetails(ID);
        }

        public List<DetailedExaminationBO> GetDetailedExaminationList(string Type)
        {
            return managePatientDAL.GetDetailedExaminationList(Type);
        }

        public int Save(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList)
        {
            return managePatientDAL.Save(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList);
        }

        public List<ExaminationBO> GetExaminationList(int ID, int OPID)
        {
            return managePatientDAL.GetExaminationList(ID, OPID);
        }

        public List<DetailedExaminationBO> GetOptionList(string Type)
        {
            return managePatientDAL.GetOptionList(Type);
        }

        public List<DropDownListBO> GetTherapistDetails()
        {
            return managePatientDAL.GetTherapistDetails();
        }

        public List<PatientDiagnosisBO> GetDateListByID(int ID,int OPID)
        {
            return managePatientDAL.GetDateListByID(ID,OPID);
        }

        public List<PatientDiagnosisBO> GetTreatmentNumberList()
        {
            return managePatientDAL.GetTreatmentNumberList();
        }

        public List<DropDownListBO> GetInstructionsList()
        {
            return managePatientDAL.GetInstructionsList();
        }

        public List<DropDownListBO> GetMedicineTimeList(string Type)
        {
            return managePatientDAL.GetMedicineTimeList(Type);
        }

        public PatientDiagnosisBO GetVitalChart(int PatientID, DateTime FromDate,int AppointmentProcessID)
        {
            return managePatientDAL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
        }

        public List<TreatmentBO> GetTreatmentListByID(int PatientID, DateTime FromDate,int AppointmentProcessID)
        {
            return managePatientDAL.GetTreatmentListByID(PatientID, FromDate, AppointmentProcessID);
        }

        public List<ReportBO> GetReportListByID(int PatientID, DateTime FromDate,int AppointmentProcessID)
        {
            return managePatientDAL.GetReportListByID(PatientID, FromDate, AppointmentProcessID);
        }
        public List<ReportBO> GetReportListByIDV5(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            return managePatientDAL.GetReportListByIDV5(PatientID, FromDate, AppointmentProcessID);
        }
        public List<TreatmentItemBO> GetTreatmentMedicineListByID(int PatientID, DateTime FromDate,int AppointmentProcessID)
        {
            return managePatientDAL.GetTreatmentMedicineListByID(PatientID, FromDate, AppointmentProcessID);
        }

        public List<DoctorListBO> GetDoctorList(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            return managePatientDAL.GetDoctorList(PatientID, FromDate, AppointmentProcessID);
        }

        public List<MedicineBO> GetMedicineListByID(int OPID,int PatientID, DateTime FromDate)
        {
            return managePatientDAL.GetMedicineListByID(OPID,PatientID, FromDate);
        }

        public PatientDiagnosisBO GetCaseSheet(int PatientID, DateTime FromDate,int AppointmentProcessID)
        {
            return managePatientDAL.GetCaseSheet(PatientID, FromDate, AppointmentProcessID);
        }

        public List<ExaminationBO> GetExaminationByDate(int PatientID, DateTime FromDate)
        {
            return managePatientDAL.GetExaminationByDate(PatientID, FromDate);
        }

        public List<ExaminationBO> GetExamination(int PatientID, DateTime FromDate, int AppointmentProcessID, int ReviewID)
        {
            return managePatientDAL.GetExamination(PatientID, FromDate, AppointmentProcessID, ReviewID);
        }

        public List<MedicineBO> GetMedicinePrescriptionByID(int PatientID, DateTime FromDate)
        {
            return managePatientDAL.GetMedicinePrescriptionByID(PatientID, FromDate);
        }
        public DatatableResultBO GetTreatmentDetailsListForPrint(string TransNo, string Date, string Patient, string Doctor, string Time, string TokenNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            return managePatientDAL.GetTreatmentDetailsListForPrint(TransNo, Date, Patient, Doctor, Time, TokenNo, SortField, SortOrder, Offset, Limit);
        }
        public List<MedicineBO> GetMedicineListItemsByID(int ID)
        {
            return managePatientDAL.GetMedicineListItemsByID(ID);
        }
        public List<TreatmentBO> GetTreatmentListItemsByID(int ID)
        {
            return managePatientDAL.GetTreatmentListItemsByID(ID);
        }
        public List<PatientDiagnosisBO> GetPatientDetailsItemsByID(int ID)
        {
            return managePatientDAL.GetPatientDetailsItemsByID(ID);
        }
        public DatatableResultBO GetIPPatientList(string CodeHint, string NameHint, string DateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return managePatientDAL.GetIPPatientList(CodeHint, NameHint, DateHint, SortField, SortOrder, Offset, Limit);
        }

        public List<MedicineBO> GetMedicinesDetails(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            return managePatientDAL.GetMedicinesDetails(PatientID, FromDate, AppointmentProcessID);
        }
        public List<MedicineBO> GetAllMedicinesbyProductionGroup(int ProductionGroupID)
        {
            return managePatientDAL.GetAllMedicinesbyProductionGroup(ProductionGroupID);
        }
        public List<ExaminationBO> GetDashaVidhaPareekhsalist(int PatientID, int AppointmentProcessID)
        {
            return managePatientDAL.GetDashaVidhaPareekhsalist(PatientID, AppointmentProcessID);
        }
        public List<MedicineItemBO> GetMedicinesItemsList(int PatientID, DateTime FromDate, int AppointmentProcessID)
        {
            return managePatientDAL.GetMedicinesItemsList(PatientID, FromDate, AppointmentProcessID);
        }

        public int GetAppointmentProcessID(int ID, int AppointmentScheduleItemID)
        {
            return managePatientDAL.GetAppointmentProcessID(ID, AppointmentScheduleItemID);
        }

        public List<LabTestItemBO> GetLabItems(int PatientID, int OPID, DateTime FromDate)
        {
            return managePatientDAL.GetLabItems(PatientID, OPID, FromDate);
        }
        public List<XraysItemBO> GetXrayItems(int PatientID, int OPID, DateTime FromDate)
        {
            return managePatientDAL.GetXrayItems(PatientID, OPID, FromDate);
        }
        public List<LabTestItemBO> GetCategoryWiseLabItems(int[] LabTestCategoryID)
        {
            string CommaSeperatedIDs = string.Join(",", LabTestCategoryID.Select(x => x.ToString()).ToArray());
            if (CommaSeperatedIDs == "0")
            {
                CommaSeperatedIDs = "";
            }
            return managePatientDAL.GetCategoryWiseLabItems(CommaSeperatedIDs);
        }

        public bool IsInPatient(int PatientID)
        {
            return managePatientDAL.IsInPatient(PatientID);
        }

        public List<MedicineBO> GetMedicines(int PatientID)
        {
            return managePatientDAL.GetMedicines(PatientID);
        }

        public List<MedicineBO> GetPreviousMedicineListByID(int PatientMedicinesID)
        {
            return managePatientDAL.GetPreviousMedicineListByID(PatientMedicinesID);
        }

        public List<MedicineBO> GetPreviousMedicinesList(int PatientMedicinesID)
        {
            return managePatientDAL.GetPreviousMedicinesList(PatientMedicinesID);
        }

        public List<MedicineItemBO> GetPreviousMedicinesItemsList(int PatientMedicinesID)
        {
            return managePatientDAL.GetPreviousMedicinesItemsList(PatientMedicinesID);
        }

        public List<ExaminationBO> GetPreviousExamination(int PatientID, int ReviewID)
        {
            return managePatientDAL.GetPreviousExamination(PatientID, ReviewID);
        }

        public List<HistoryBO> GetHistoryListByID(int OPID, int PatientID)
        {
            return managePatientDAL.GetHistoryListByID(OPID, PatientID);
        }

        public List<HistoryBO> GetHistoryByID(int ParentID,int OPID, int PatientID)
        {
            return managePatientDAL.GetHistoryByID(ParentID,OPID, PatientID);
        }

        public List<MedicineBO> GetMedicinesHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            return managePatientDAL.GetMedicinesHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
        }

        public List<TreatmentBO> GetTreatmentHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            return managePatientDAL.GetTreatmentHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
        }

        public List<VitalChartBO> GetVitalChartHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            return managePatientDAL.GetVitalChartHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
        }

        public List<RoundsBO> GetRoundsHistory(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType)
        {
            return managePatientDAL.GetRoundsHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
        }

        public DatatableResultBO GetAllLabTestList(string CodeHint, string TypeHint, string ServiceHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return managePatientDAL.GetAllLabTestList(CodeHint, TypeHint, ServiceHint, NameHint, SortField, SortOrder, Offset, Limit);
        }

        public PatientDiagnosisBO GetDepartmentItems(int AppointmentScheduleItemID)
        {
            return managePatientDAL.GetDepartmentItems(AppointmentScheduleItemID);
        }

        public int EditDepartment(PatientDiagnosisBO EditDepartment)
        {
            return managePatientDAL.EditDepartment(EditDepartment);
        }
        public List<ExaminationBO> GetBaseLineInformationList(int ID, int OPID)
        {
            return managePatientDAL.GetBaseLineInformationList(ID, OPID);
        }

        public bool IsPatientHistory(int AppointmentProcessID, int PatientID)
        {
            return managePatientDAL.IsPatientHistory(AppointmentProcessID, PatientID);
        }

        public bool StopMedicine(int PatientMedicinesID)
        {
            return managePatientDAL.StopMedicine(PatientMedicinesID);
        }

        public List<ExaminationBO> GetBaseLineInformationDetails(int ID, int OPID)
        {
            return managePatientDAL.GetBaseLineInformationDetails(ID, OPID);
        }

        public List<ExaminationBO> GetCaseSheetList(int ID, int OPID)
        {
            return managePatientDAL.GetCaseSheetList(ID, OPID);
        }

        public List<ExaminationBO> GetRogaPareekshaList(int ID, int OPID)
        {
            return managePatientDAL.GetRogaPareekshaList(ID, OPID);
        }

        public List<ExaminationBO> GetRogaNirnayamList(int ID, int OPID)
        {
            return managePatientDAL.GetRogaNirnayamList(ID, OPID);
        }

        public int SaveV2(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<QuestionnaireBO> QuestionnaireItems)
        {
            return managePatientDAL.SaveV2(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem, QuestionnaireItems);
        }
        public int SaveV5(PatientDiagnosisBO diagnosis, List<VitalChartBO> VitalChartItems, List<ExaminationBO> Items, List<ExaminationBO> NewItems, List<BaseLineBO> BaseLineItems, List<ReportBO> ReportItems, List<LabtestsBO> LabTestItems, List<TreatmentBO> Treatments, List<TreatmentItemBO> TreatmentMedicines, List<MedicineBO> MedicinesList, List<MedicineItemBO> MedicinesItemsList, List<XrayBO> XrayItem, List<DoctorListBO> DoctorList, List<BaseLineBO> RogaPareekshaItems, List<BaseLineBO> CaseSheetItems, List<ExaminationBO> RogaNirnayamItem, List<QuestionnaireBO> QuestionnaireItems)
        {
            return managePatientDAL.SaveV5(diagnosis, VitalChartItems, Items, NewItems, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem, QuestionnaireItems);
        }
        public bool CancelPaitentTreatment(int PatientTreatmentID, int TreatmentID, DateTime Date)
        {
            return managePatientDAL.CancelPaitentTreatment(PatientTreatmentID, TreatmentID, Date);
        }
    }
}
