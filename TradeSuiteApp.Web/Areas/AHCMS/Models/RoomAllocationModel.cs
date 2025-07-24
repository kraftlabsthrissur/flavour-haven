using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.AHCMS.Models
{
    public class RoomAllocationModel
    {
        public int DoctorID { get; set; }
        public int RoomID { get; set; }
        public int PatientID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int RoomTypeID { get; set; }
        public int IPID { get; set; }
        public string ReferedDate { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public string Descriptions { get; set; }
        public string AdmissionDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Date { get; set; }
        public string PatientName { get; set; }
        public string TransNo { get; set; }

        public decimal Rate { get; set; }
        public SelectList RoomTypeList { get; set; }
        public SelectList RoomList { get; set; }

        public int ID { get; set; }
        public string DoctorName { get; set; }
        public int OccupationID { get; set; }
        public int StateID { get; set; }
        public string Time { get; set; }
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
    }

    public class RoomReservationModel
    {
        public int DoctorID { get; set; }
        public int RoomID { get; set; }
        public int PatientID { get; set; }
        public int AppointmentProcessID { get; set; }
        public int RoomTypeID { get; set; }
        public int ReservationID { get; set; }
        public int RoomStatusID { get; set; }
        public int IPID { get; set; }

        public string ReferedDate { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public string Descriptions { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmissionDateTill { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Date { get; set; }
        public string PatientName { get; set; }
        public string TransNo { get; set; }
        public string ByStander { get; set; }
        public string MobileNumber { get; set; }
        public string RoomChangeDate { get; set; }

        public decimal Rate { get; set; }
        public bool IsRoomChange { get; set; }
        public SelectList RoomTypeList { get; set; }
        public SelectList RoomList { get; set; }
        public List<IpRoomModel> RoomItems { get; set; }

    }
    public class IpRoomModel
    {
        public int RoomID { get; set; }
        public int RoomTypeID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int RoomStatusID { get; set; }
        public string RoomName { get; set; }
        public decimal Rate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RoomType { get; set; }
    }
}