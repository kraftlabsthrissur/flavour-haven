using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PatientDiagnosisBO
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public string HIN { get; set; }
        public int IPID { get; set; }
        public string PatientName { get; set; }
        public string PatientCode { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public DateTime Date { get; set; }
        public List<ExaminationBO> ExaminationItems { get; set; }
        public string BP { get; set; }
        public string Pulse { get; set; }
        public string Temperature { get; set; }
        public string HR { get; set; }
        public string RR { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Others { get; set; }
        public int Text { get; set; }
        public string Remark { get; set; }
        public DateTime NextVisitDate { get; set; }
        public string AppointmentType { get; set; }
        public string VisitType { get; set; }
        public int AppointmentScheduleItemID { get; set; }
        public string TransNo { get; set; }
        public string Doctor { get; set; }
        public string Time { get; set; }
        public int TokenNumber { get; set; }
        public int AppointmentProcessID { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public bool IsReferedIP { get; set; }
        public bool IsCompleted { get; set; }
        public bool IswalkIn { get; set; }
        public bool IsDischargeAdvice { get; set; }
        public DateTime ReferedDate { get; set; }
        public int DoctorID { get; set; }
        public string CourseInTheHospital { get; set; }
        public string ConditionAtDischarge { get; set; }
        public string DietAdvice { get; set; }
        public string RoomName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string IPDoctor { get; set; }
        public int IPDoctorID { get; set; }
        public string Place { get; set; }
        public int Month { get; set; }
        public int ParentID { get; set; }
        public int ReviewID { get; set; }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Unit { get; set; }
        public decimal BMI { get; set; }
        public string RespiratoryRate { get; set; }

        public string Question { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public string Type { get; set; }
    }

    public class VitalChartBO
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string AppointmentType { get; set; }
        public DateTime Date { get; set; }
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

    public class DetailedExaminationBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
       
    }

    public class ExaminationBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
        public string Diagnosis { get; set; }
        public int GeneralOptionID { get; set; }
        public bool IsParent { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ReportBO
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool IsBeforeAdmission { get; set; }
    }

    public class DropDownListBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

 

    public class TreatmentBO
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string Name { get; set; }
        public int TreatmentID { get; set; }
        public int TherapistID { get; set; }
        public int TreatmentRoomID { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TreatmentRoomName { get; set; }
        public string TherapistName { get; set; }
        public int TreatmentNo { get; set; }
        public string Status { get; set; }
        public int FinishedTreatment { get; set; }
        public int NoOfTreatment { get; set; }
        public int PatientTreatmentID { get; set; }
        public bool IsDischarged { get; set; }
        public string Medicine { get; set; }
        public int NoofDays { get; set; }
        public string DoctorName { get; set; }
        public string MorningTimeID { get; set; }
        public string NoonTimeID { get; set; }
        public string EveningTimeID { get; set; }
        public string NightTimeID { get; set; }

        public bool IsMorning { get; set; }
        public bool IsNoon { get; set; }
        public bool Isevening { get; set; }
        public bool IsNight { get; set; }
        public string MorningTime { get; set; }
        public string NoonTime { get; set; }
        public string EveningTime { get; set; }
        public string NightTime { get; set; }
    }

    public class TreatmentItemBO
    {
        public int TreatmentID { get; set; }
        public int MedicineID { get; set; }
        public string Medicine { get; set; }
        public string MedicineUnit { get; set; }
        public int MedicineUnitID { get; set; }
        public int TreatmentMedicineUnitID { get; set; }
        public string TreatmentMedicineUnit { get; set; }
        public decimal StandardMedicineQty { get; set; }
    }

    public class DoctorListBO
    {
        public int DoctorNameID { get; set; }
        public string DoctorName { get; set; }
    }

    public class MedicineBO
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public int MedicineID { get; set; }
        public int UnitID { get; set; }
        public decimal Quantity { get; set; }
        public string Qty { get; set; }
        public int GroupID { get; set; }
        public string Medicine { get; set; }
        public string Unit { get; set; }
        public string Prescription { get; set; }
        public int PrescriptionID { get; set; }
        public string PrescriptionNo { get; set; }
        public string Instructions { get; set; }
        public string Description { get; set; }
        public string MorningTime { get; set; }
        public string NoonTime { get; set; }
        public string EveningTime { get; set; }
        public string NightTime { get; set; }
        public string MedicineTime { get; set; }
        public int TransID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PatientMedicinesID { get; set; }
        public bool IsDischarged { get; set; }
        public int DischargeSummaryID { get; set; }
        public int NoofDays { get; set; }
        public bool IsCompleted { get; set; }
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
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ProductionGroup { get; set; }
        public decimal Stock { get; set; }
        public string MedicineInstruction { get; set; }
        public string QuantityInstruction { get; set; }
    }

    public class MedicineItemBO
    {
        public string MorningTime { get; set; }
        public string NoonTime { get; set; }
        public string EveningTime { get; set; }
        public int GroupID { get; set; }
        public string NightTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InstructionsID { get; set; }
        public string MedicineTime { get; set; }
        public bool IsMorning { get; set; }
        public bool Isevening { get; set; }
        public bool IsNoon { get; set; }
        public bool IsNight { get; set; }
        public int NoofDays { get; set; }
        public bool IsEmptyStomach { get; set; }
        public bool IsBeforeFood { get; set; }
        public bool IsAfterFood { get; set; }
        public bool IsMiddleOfFood { get; set; }
        public bool IsWithFood { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
        public int MorningTimeID { get; set; }
        public int EveningTimeID { get; set; }
        public int NoonTimeID { get; set; }
        public int NightTimeID { get; set; }
        public int ModeOfAdministrationID { get; set; }
        public int PatientMedicineID { get; set; }
        public int DischargeSummaryID { get; set; }
        public bool IsMultipleTimes { get; set; }
        public string MedicineInstruction { get; set; }
        public string QuantityInstruction { get; set; }
    }

    public class IPBO
    {
        public int ID { get; set; }
        public int RoomTypeID { get; set; }
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public decimal Rate { get; set; }
        public string Descriptions { get; set; }
        public string ReferedDate { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
    }

    public class LabtestsBO
    {
        public DateTime TestDate { get; set; }
        public int LabTestID { get; set; }
        public string Labtest { get; set; }
        public string Status { get; set; }
        public string ObservedValue { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public string State { get; set; }
        public int DocumentID { get; set; }
        public string Path { get; set; }
        public int ID { get; set; }
        public int OPID { get; set; }
        public int IPID { get; set; }
        public int PatientID { get; set; }
    }

    public class PhysiotherapyBO
    {
        public int PhysiotherapyID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class XrayBO
    {
        public int ID { get; set; }
        public DateTime XrayDate { get; set; }
        public int XrayID { get; set; }
        public string XrayName { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string Path { get; set; }

    }

    public class RoundsBO
    {
        public int ParentID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int PatientID { get; set; }
        public string AppointmentType { get; set; }
        public string Remarks { get; set; }
        public DateTime RoundsDate { get; set; }
        public DateTime Date { get; set; }
        public string RoundsTime { get; set; }
        public string ClinicalNote { get; set; }
        public string Doctor { get; set; }
        public int UserID { get; set; }
    }

    public class DischargeSummaryBO
    {
        public string CourseInTheHospital { get; set; }
        public string ConditionAtDischarge { get; set; }
        public string DietAdvice { get; set; }
    }

    public class HistoryBO
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
        public DateTime SuggestedReviewDate { get; set; }
        public DateTime DischargedDate { get; set; }
        public string BP { get; set; }
        public string Pulse { get; set; }
        public string Temperature { get; set; }
        public string HR { get; set; }
        public string RR { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Others { get; set; }
        public string PresentingComplaints { get; set; }
        public string Associatedcomplaints { get; set; }
        public string ContemporaryDiagnosis { get; set; }
        public string AyurvedicDiagnosis { get; set; }
        
    }
    public class BaseLineBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
        public string Diagnosis { get; set; }
        public int GeneralOptionID { get; set; }
        public bool IsParent { get; set; }
    }
    public class QuestionnaireBO
    {
       
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int PatientID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int DoctorID { get; set; }


        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
        public string Diagnosis { get; set; }
        public int GeneralOptionID { get; set; }
        public bool IsParent { get; set; }
        public bool IsChecked { get; set; }
        public bool IsExists { get; set; }

    }

}
