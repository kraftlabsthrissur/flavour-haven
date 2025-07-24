using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class PatientDiagnosisModel
    {
        public string PatientName { get; set; }
        public string PatientCode { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public int DistrictID { get; set; }
        public int OccupationID { get; set; }
        public string Code { get; set; }
        public string PinCode { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CountryCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string MobileNumber { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string GSTNo { get; set; }
        public string Category { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public string Registered { get; set; }
        public bool IsGSTRegistered { get; set; }
        public string GuardianName { get; set; }
        public string Occupation { get; set; }
        public string MartialStatus { get; set; }
        public int BloodGroupID { get; set; }
        public string BloodGroup { get; set; }
        public int Age { get; set; }
        public string ReferalContactNo { get; set; }
        public int PatientReferedByID { get; set; }
        public string PatientReferedBy { get; set; }
        public string PurposeOfVisit { get; set; }
        public string PassportNo { get; set; }
        public string DateOfIssuePassport { get; set; }
        public string DateOfIssueVisa { get; set; }
        public string DateOfExpiry { get; set; }
        public string PlaceOfIssue { get; set; }
        public string VisaNo { get; set; }
        public string DateOfExpiryVisa { get; set; }
        public string DestinationTo { get; set; }
        public string ArrivedFrom { get; set; }
        public string DateOfArrival { get; set; }
        public string ProceedingTo { get; set; }
        public int DurationOfStay { get; set; }
        public int PhotoID { get; set; }
        public int PassportID { get; set; }
        public int VisaID { get; set; }
        public string EmployedIn { get; set; }
        public string LandLine { get; set; }
        public int DiscountTypeID { get; set; }
        public decimal MaxDisccountAmount { get; set; }
        public string DiscountStartDate { get; set; }
        public string DiscountEndDate { get; set; }
        public string ReferalName { get; set; }
        public string OtherQuotationIDS { get; set; }
        public string EmergencyContactNo { get; set; }


        public int PatientID { get; set; }
        public string Date { get; set; }
        public string ReportName { get; set; }
        public string ReportDate { get; set; }
        public string Description { get; set; }
        public bool IsBeforeAdmission { get; set; }        
        public List<ReportModel> ReportItems { get; set; }
        public List<ExaminationModel> ExaminationItems { get; set; }
        public List<ExaminationModel> ExaminationNewItems { get; set; }
        public List<KeyValuePair<string, SelectList>> GeneralOptions { get; set; }
        public SelectList ModeOfAdministrationList { get; set; }
        public SelectList TherapistList { get; set; }
        public SelectList TreatmentRoomList { get; set; }
        public string TreatmentName { get; set; }
        public string Doctor { get; set; }
        public int UserID { get; set; }
        public int TherapistID { get; set; }
        public int TreatmentRoomID { get; set; }
        public string Instructions { get; set; }
        public string TreatmentStartDate { get; set; }
        public string TentativeEndDate { get; set; }
        public string Medicine { get; set; }
        public int MedicineID { get; set; }
        public int MedicineUnitID { get; set; }
        public string MedicineUnit { get; set; }
        public int TreatmentMedicineUnitID { get; set; }
        public string TreatmentMedicineUnit { get; set; }
        public SelectList TreatmentList { get; set; }
        public SelectList InstructionsList { get; set; }
        public decimal StandardMedicineQty { get; set; }

        public string BP { get; set; }
        public string Pulse { get; set; }
        public string Temperature { get; set; }
        public string HR { get; set; }
        public string RR { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Others { get; set; }
        public List<TreatmentModel> TreatmentItems { get; set; }
        public List<TreatmentMedicineModel> TreatmentMedicines { get; set; }
        public List<MedicineModel> Medicines { get; set; }
        public List<HistoryModel> History { get; set; }
        public List<MedicineModel> MedicinePrescription { get; set; }
        public List<MedicineItemModel> MedicineItems { get; set; }
        public IEnumerable<SelectListItem> DateList { get; set; }
        public List<VitalChartModel> VitalChartItems { get; set; }
        public List<LabtestsModel> LabTestItems { get; set; }
        public List<PhysiotherapyModel> PhysiotherapyItems { get; set; }
        public List<RoundsModel> RoundsItems { get; set; }
        public List<XrayModel> XrayItems { get; set; }
        public List<DoctorModel> DoctorList { get; set; }
        public SelectList TreatmentNoList { get; set; }
        public int NoofTreatment { get; set; }
        public SelectList MorningList { get; set; }
        public SelectList NoonList { get; set; }
        public SelectList EveningList { get; set; }
        public SelectList NightList { get; set; }
        public int MorningID { get; set; }
        public int NoonID { get; set; }
        public int EveningID { get; set; }
        public int NightID { get; set; }
        public SelectList UnitList { get; set; }
        public int UnitID { get; set; }
        public int NoofDays { get; set; }
        public decimal Qty { get; set; }
        public decimal ExternalQty { get; set; }
        public string StartDateMed { get; set; }
        public string EndDate { get; set; }
        public string Prescription { get; set; }
        public string TimeDescription { get; set; }
        public string NextVisitDate { get; set; }
        public string Remark { get; set; }
        public string ClinicalNote { get; set; }
        public string RoundTime { get; set; }
        public string AppointmentType { get; set; }
        public string VisitType { get; set; }
        public int AppointmentScheduleItemID { get; set; }
        public int EmployeeCategoryID { get; set; }
        public string TransNo { get; set; }
        public string Time { get; set; }
        public int TokenNo { get; set; }
        public int AppointmentProcessID { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }

        public bool IsReferedIP { get; set; }
        public bool IsCompleted { get; set; }
        public bool IswalkIn { get; set; }
        public int TreatmentID { get; set; }
        public int PatientTreatmentID { get; set; }
        
        public string ReferedDate { get; set; }
        public SelectList RoomTypeList { get; set; }
        public int RoomTypeID { get; set; }
        public decimal Rate { get; set; }
        public string AdmissionDate { get; set; }
        public int DoctorID { get; set; }


        public int IPID { get; set; }
        public string VitalChartDate { get; set; }
        public string RoundsDate { get; set; }
        public string LabTest { get; set; }
        public string LabTestCategoryIDs { get; set; }
        public int LabTestID { get; set; }
        public string Physiotherapy { get; set; }
        public int PhysiotherapyID { get; set; }
        public string PhysioFromDate { get; set; }
        public string PhysioToDate { get; set; }

        public string XrayName { get; set; }
        public DateTime XrayID { get; set; }
        public string XrayDate { get; set; }
        public int ModeOfAdministrationID { get; set; }
        public string StartDate { get; set; }
        public string TestDate { get; set; }
        public List<LabTestItemModel> LabAndXrayItems { get; set; }
        public List<XraysItemModel> XrayItemList { get; set; }

        public string CourseInTheHospital { get; set; }
        public string ConditionAtDischarge { get; set; }
        public string DietAdvice { get; set; }
        public List<MedicineModel> InternalMedicine { get; set; }
        public List<MedicineItemModel> InternalMedicineItems { get; set; }
        public bool IsDischargeAdvice { get; set; }
        public List<DischargeSummary> DischargeSummary { get; set; }
        public bool IsDischarged { get; set; }
        public int DischargeSummaryID { get; set; }
        public string RoomName { get; set; }
        public bool IsInPatient { get; set; }
        public string IPDoctor { get; set; }
        public int IPDoctorID { get; set; }
        public string Place { get; set; }
        public int Month { get; set; }
        public int ParentID { get; set; }
        public int ReviewID { get; set; }

        public SelectList DepartmentList { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentID { get; set; }
        public bool IsMultipleTimes { get; set; }
        public List<BaseLineModel> BaseLineItems { get; set; }
        public List<BaseLineModel> OtherConditionsItems { get; set; }

        public string RespiratoryRate { get; set; }
        public decimal BMI { get; set; }
        public string Unit { get; set; }

        public string DoctorName { get; set; }
        public int DoctorNameID { get; set; }
        public List<ExaminationModel> CaseSheetItems { get; set; }
        public List<ExaminationModel> RogaPareekshaItems { get; set; }
        public List<ExaminationModel> RogaNirnayamItems { get; set; }
        public List<BaseLineModel> AssociatedConditionsItems { get; set; }
        public List<QuestionnaireModel> QuestionnaireItems { get; set; }
    
        public string Question { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public string Type { get; set; }
        public SelectList QuestionnaireList { get; set; }
        public bool IsExists { get; set; }

        public int MorningTimeID { get; set; }
        public int NoonTimeID { get; set; }
        public int EveningTimeID { get; set; }
        public int NightTimeID { get; set; }
        public string MedicineStock { get; set; }
        public decimal TotalStock { get; set; }
    }

    public class IPModel
    {
        public int RoomTypeID { get; set; }
        public int RoomID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string RoomName { get; set; }
        public decimal Rate { get; set; }
        public string Descriptions { get; set; }
        public string ReferedDate { get; set; }
        public SelectList RoomTypeList { get; set; }
        public string AdmissionDate { get; set; }
        public int AppointmentProcessID { get; set; }

    }

    public class ExaminationModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string Area { get; set; }
        public string GroupName { get; set; }
        public int GeneralOptionID { get; set; }
        public string Diagnosis { get; set; }
        public int DiagnosisID { get; set; }
        public bool IsParent { get; set; }
        public bool IsChecked { get; set; }

    }
    public class BaseLineModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
        public int GeneralOptionID { get; set; }
        public string Diagnosis { get; set; }
        public int DiagnosisID { get; set; }
        public bool IsParent { get; set; }
        public bool IsChecked { get; set; }

    }
    public class VitalChartModel
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string AppointmentType { get; set; }
        public string Date { get; set; }
        public string BP { get; set; }
        public string Pulse { get; set; }
        public string Temperature { get; set; }
        public string HR { get; set; }
        public string RR { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Others { get; set; }
        public string Time { get; set; }
        public string Unit { get; set; }
        public decimal BMI { get; set; }
        public string RespiratoryRate { get; set; }
    }

    public class ReportModel
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsBeforeAdmission { get; set; }
    }

    public class TreatmentModel
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string Name { get; set; }
        public int TreatmentID { get; set; }
        public int TherapistID { get; set; }
        public int TreatmentRoomID { get; set; }
        public string Instructions { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TreatmentRoomName { get; set; }
        public string TherapistName { get; set; }
        public int TreatmentNo { get; set; }
        public string Status { get; set; }
        public int FinishedTreatment { get; set; }
        public int NoOfTreatment { get; set; }
        public int PatientTreatmentID { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDischarged { get; set; }
        public string Medicine { get; set; }
        public int NoofDays { get; set; }
        public string DoctorName { get; set; }

        public string MorningTime { get; set; }
        public string NoonTime { get; set; }
        public string EveningTime { get; set; }
        public string NightTime { get; set; }

        public bool IsMorning { get; set; }
        public bool IsNoon { get; set; }
        public bool Isevening { get; set; }
        public bool IsNight { get; set; }

        public int MorningID { get; set; }
        public int NoonID { get; set; }
        public int EveningID { get; set; }
        public int NightID { get; set; }

    }

    public class TreatmentMedicineModel
    {
        public int TreatmentID { get; set; }
        public int MedicineID { get; set; }
        public string Medicine { get; set; }
        public decimal StandardMedicineQty { get; set; }
        public int MedicineUnitID { get; set; }
        public int TreatmentMedicineUnitID { get; set; }        
    }

    public class MedicineModel
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public int MedicinesID { get; set; }
        public int UnitID { get; set; }
        public decimal Quantity { get; set; }
        public int GroupID { get; set; }
        public string Medicine { get; set; }
        public string Unit { get; set; }
        public string Prescription { get; set; }
        public int PrescriptionID { get; set; }
        public int TransID { get; set; }
        public string Instructions { get; set; }
        public string PrescriptionNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int PatientMedicinesID { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDischarged { get; set; }
        public int DischargeSummaryID { get; set; }
        public int NoofDays { get; set; }
        public string Qty { get; set; }
        public string Description { get; set; }
        public string MorningTime { get; set; }
        public string NoonTime { get; set; }
        public string EveningTime { get; set; }
        public bool IsMorning { get; set; }
        public bool IsEvening { get; set; }
        public bool IsNoon { get; set; }
        public bool IsNight { get; set; }
        public bool IsEmptyStomach { get; set; }
        public bool IsBeforeFood { get; set; }
        public bool IsAfterFood { get; set; }
        public bool IsMultipleTimes { get; set; }
        public string Status { get; set; }
        public string DoctorName { get; set; }
        public string MedicineInstruction { get; set; }
        public string QuantityInstruction { get; set; }
    }

    public class MedicineItemModel
    {
        public string MorningTime { get; set; }
        public string NoonTime { get; set; }
        public string EveningTime { get; set; }
        public int GroupID { get; set; }
        public string NightTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int InstructionsID { get; set; }
        public bool IsMorning { get; set; }
        public bool Isevening { get; set; }
        public bool IsNoon { get; set; }
        public bool IsNight { get; set; }
        public int NoofDays { get; set; }
        public bool IsEmptyStomach { get; set; }
        public bool IsBeforeFood { get; set; }
        public bool IsAfterFood { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
        public int MorningTimeID { get; set; }
        public int EveningTimeID { get; set; }
        public int NoonTimeID { get; set; }
        public int NightTimeID { get; set; }
        public int ModeOfAdministrationID { get; set; }
        public int PatientMedicineID { get; set; }
        public int DischargeSummaryID { get; set; }
        public bool IsMiddleOfFood { get; set; }
        public bool IsWithFood { get; set; }
        public bool IsMultipleTimes { get; set; }
        public string MedicineInstruction { get; set; }
        public string QuantityInstruction { get; set; }
    }

    public class LabtestsModel
    {
        public string TestDate { get; set; }
        public int LabTestID { get; set; }
        public string LabTest { get; set; }
        public string Status { get; set; }
        public string ObservedValue { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public int ID { get; set; }
        public List<FileBO> SelectedQuotation { get; set; }
    }

    public class PhysiotherapyModel
    {
        public int PhysiotherapyID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class XrayModel
    {
        public int ID { get; set; }
        public string XrayDate { get; set; }
        public int XrayID { get; set; }
        public string XrayName { get; set; }
        public string Status { get; set; }
        public string path { get; set; }
        public string State { get; set; }
    }

    public class DoctorModel
    {
        public int DoctorNameID { get; set; }
        public string DoctorName { get; set; }
    }

    public class RoundsModel
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string AppointmentType { get; set; }
        public string Remarks { get; set; }
        public string ClinicalNote { get; set; }
        public string RoundsDate { get; set; }
        public string RoundsTime { get; set; }
        public int UserID { get; set; }
        public string Doctor { get; set; }
    }

    public class DischargeSummary
    {
        public string CourseInTheHospital { get; set; }
        public string ConditionAtDischarge { get; set; }
        public string DietAdvice { get; set; }
    }

    public class HistoryModel
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int IPID { get; set; }
        public int PatientID { get; set; }
        public string Patient { get; set; }
        public string TransNo { get; set; }
        public int VitalChartID { get; set; }
        public string AppointmentType { get; set; }
        public string Disease { get; set; }
        public string CaseSheet { get; set; }
        public string Remarks { get; set; }
        public string Doctor { get; set; }
        public string Medicines { get; set; }
        public DateTime ReportedDate { get; set; }
        public string DischargedDate { get; set; }
        public string SuggestedReviewDate { get; set; }
        public string BP { get; set; }
        public string Pulse { get; set; }
        public string Temperature { get; set; }
        public string HR { get; set; }
        public string RR { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Others { get; set; }
        public List<MedicineModel> MediceinsHistory { get; set; }
        public List<TreatmentModel> TreatmentHistory { get; set; }
        public List<VitalChartModel> VitalChartHistory { get; set; }
        public List<RoundsModel> RoundsHistory { get; set; }
        public bool IsCompleted { get; set; }
        public string PresentingComplaints { get; set; }
        public string Associatedcomplaints { get; set; }
        public string ContemporaryDiagnosis { get; set; }
        public string AyurvedicDiagnosis { get; set; }
    }

    public class QuestionnaireModel
    {

        public int QuestionID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int PatientID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int DoctorID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
        public int GeneralOptionID { get; set; }
        public string Diagnosis { get; set; }
        public int DiagnosisID { get; set; }
        public bool IsParent { get; set; }
        public bool IsChecked { get; set; }
        public bool IsExists { get; set; }
    }

    //public class LabTestItemModel
    //{
    //    public string TestDate { get; set; }
    //    public int LabTestID { get; set; }
    //    public string LabTest { get; set; }
    //    public string Status { get; set; }
    //    public string ObservedValue { get; set; }
    //    public string BiologicalReference { get; set; }
    //    public string Unit { get; set; }
    //    public int ID { get; set; }
    //    public List<FileBO> SelectedQuotation { get; set; }
    //}
}