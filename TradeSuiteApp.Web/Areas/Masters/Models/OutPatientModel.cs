using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class OutPatientModel
    {
        public int ID { get; set; }
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
        public SelectList StateList { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList DistrictList { get; set; }
        public SelectList OccupationList { get; set; }

        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string GuardianName { get; set; }
        public string Occupation { get; set; }
        public string MartialStatus { get; set; }
        public SelectList MartialStatusList { get; set; }
        public string Gender { get; set; }
        public SelectList GenderList { get; set; }
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
        public int EmployeeCategoryID { get; set; }

        public string LandLine { get; set; }
        public string Place { get; set; }
        public int Month { get; set; }

        public int DiscountTypeID { get; set; }
        public List<DiscountDetailModel> DiscountDetails { get; set; }
        public decimal MaxDisccountAmount { get; set; }
        public string DiscountStartDate { get; set; }
        public string DiscountEndDate { get; set; }
        public List<CategoryBO> DiscountPercentageList { get; set; }
        public SelectList DiscountTypeList { get; set; }
        public string ReferalName { get; set; }
        public string OtherQuotationIDS { get; set; }
        public string EmergencyContactNo { get; set; }

        public List<FileBO> OtherQuotation { get; set; }
    }
}