using BusinessObject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class AppointmentScheduleModel : TransactionFormModel
    {
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public int PatientID { get; set; }
        public int OccupationID { get; set; }
        public int StateID { get; set; }
        public string PatientName { get; set; }
        public string Time { get; set; }
        public string ConsultationTime { get; set; }
        public int TokenNo { get; set; }
        public int CountryID { get; set; }
        public int DistrictID { get; set; }
        public string Code { get; set; }
        public string PinCode { get; set; }
        public string Name { get; set; }
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
        public string GuardianName { get; set; }
        public bool IsGSTRegistered { get; set; }
        public SelectList StateList { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList DistrictList { get; set; }
        public SelectList GenderList { get; set; }
        public SelectList OccupationList { get; set; }

        public int EmployeeCategoryID { get; set; }

        public string Occupation { get; set; }
        public string MartialStatus { get; set; }
        public SelectList MartialStatusList { get; set; }
        public string Gender { get; set; }
        public SelectList BloodGroupList { get; set; }
        public int BloodGroupID { get; set; }
        public string BloodGroup { get; set; }
        public int Age { get; set; }
        public string ReferalContactNo { get; set; }
        public int PatientReferedByID { get; set; }
        public string PatientReferedBy { get; set; }
        public SelectList PatientReferedList { get; set; }
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
        public SelectList EmployedInIndia { get; set; }
        public string EmployedIn { get; set; }
        public List<FileBO> SelectedPhoto { get; set; }
        public List<FileBO> SelectedPassport { get; set; }
        public List<FileBO> SelectedVisa { get; set; }
        public List<AppointmentScheduleItemModel> Items { get; set; }
        public int BillableID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public string ItemName { get; set; }
        public decimal Rate { get; set; }

        public string day1 { get; set; }
        public string day2 { get; set; }
        public string day3 { get; set; }
        public string day4 { get; set; }
        public string day5 { get; set; }
        public string day6 { get; set; }
        public string day7 { get; set; }

        //created by priyanka
        public SelectList ConsultationModeList { get; set; }
        public string ConsultationMode { get; set; }
        public SelectList PaymentModeList { get; set; }
        public int PaymentModeID { get; set; }
        public decimal NetAmount { get; set; }
        public List<ConsultationModel> ConsulationItem { get; set; }
        //public List<PaymentModeModel> PaymentModeList { get; set; }
        public string SheduledDate { get; set; }
        public string Remarks { get; set; }
        public int AppointmentScheduleItemID { get; set; }
        public int BankID { get; set; }
        public SelectList BankList { get; set; }
        public string Place { get; set; }
        public int Month { get; set; }
        public int ReferenceThroughID { get; set; }
        public string ReferenceThrough { get; set; }
        public SelectList ReferenceThroughList { get; set; }
        public SelectList DepartmentList { get; set; }
        public int DepartmentID { get; set; }
        public SelectList SlotList { get; set; }
        public string SlotName { get; set; }

        public SelectList DiscountTypeList { get; set; }
        public int DiscountTypeID { get; set; }
        public string DiscountStartDate { get; set; }
        public string DiscountEndDate { get; set; }
        public decimal MaxDisccountAmount { get; set; }
        public List<CategoryBO> DiscountPercentageList { get; set; }

        public int IsAllowConsultationSchedule { get; set; }
        public int IsReferenceThroughRequired { get; set; }
        public string AppointmentDate { get; set; }
        public string Form { get; set; }
    }
    public class ConsultationModel
    {
        public string ItemName { get; set; }
        public decimal Rate { get; set; }
        public int ItemID { get; set; }
    }
    public class AppointmentScheduleItemModel
    {
        public string PatientName { get; set; }
        public int PatientID { get; set; }
        public int TokenNo { get; set; }
        public string Time { get; set; }
        public int AppointmentScheduleItemID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int Age { get; set; }
        public int PhotoID { get; set; }
        public string Code { get; set; }
        public string Gender { get; set; }
        public string PhotoName { get; set; }
        public string PhotoPath { get; set; }
    }

    public class DateLisModel
    {
        public string day1 { get; set; }
        public string day2 { get; set; }
        public string day3 { get; set; }
        public string day4 { get; set; }
        public string day5 { get; set; }
        public string day6 { get; set; }
        public string day7 { get; set; }
    }
}