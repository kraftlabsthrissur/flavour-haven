using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Areas.Masters.Models
{

    public class EmployeeModel
    {
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public int UserID { get; set; }

        public int ID { get; set; }
        public string Title { get; set; }
        public SelectList TitleList { get; set; }
        public string Designation { get; set; }
        public int DesignationID { get; set; }
        public string ExDesignation { get; set; }
        public SelectList DesignationList { get; set; }
        public string Gender { get; set; }
        public SelectList GenderList { get; set; }
        public string MartialStatus { get; set; }
        public SelectList MartialStatusList { get; set; }
        public string DOB { get; set; }
        public string ExDateOfSeverance { get; set; }
        public string ExDateOfJoining { get; set; }
        public int StateID { get; set; }
        public SelectList StateList { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfConfirmation { get; set; }
        public int EmpCategoryID { get; set; }
        public SelectList EmpCategoryList { get; set; }
        public int LocationID { get; set; }
        public SelectList LocationList { get; set; }
        public int InterCompanyID { get; set; }
        public SelectList InterCompanyList { get; set; }
        public int DepartmentID { get; set; }
        public SelectList DepartmentList { get; set; }
        public int CountryID { get; set; }
        public SelectList CountryList { get; set; }
        public bool IsERPUser { get; set; }
        public string DateOfSeverance { get; set; }
        public string DateOfReJoining { get; set; }
        public bool MinimumWagesStatus { get; set; }
        public int EmploymentJobTypeID { get; set; }
        public string EmploymentJobType { get; set; }
        public SelectList EmploymentJobTypeList { get; set; }
        public int PayrollCategoryID { get; set; }

        public SelectList PayrollCategoryList { get; set; }
        public string PrintPayroll { get; set; }
        public SelectList PrintPayrollList { get; set; }
        public SelectList BloodGroupList { get; set; }
        public int BloodGroupID { get; set; }
        public string BloodGroup { get; set; }
        public string Qualification1 { get; set; }
        public string Qualification2 { get; set; }
        public string Qualification3 { get; set; }
        public int NoOfDependent { get; set; }
        public string NameOfSpouse { get; set; }
        public string NameOfGuardian { get; set; }
        public bool IsExcludeFromPayroll { get; set; }
        public string PayGrade { get; set; }
        public string CompanyEmail { get; set; }
        public string ReportingCode { get; set; }
        public string ReportingName { get; set; }
        public string ProbationDuration { get; set; }
        public bool PFStatus { get; set; }
        public bool ESIStatus { get; set; }
        public bool NPSStatus { get; set; }
        public bool MedicalInsuranceStatus { get; set; }
        public bool AttandancePunchingStatus { get; set; }
        public bool MultiLocationPunchingStatus { get; set; }
        public bool SpecialLeaveStatus { get; set; }
        public bool ProbationStatus { get; set; }
        public bool ProductionIncentiveStatus { get; set; }
        public bool SalesIncentiveStatus { get; set; }
        public bool FixedIncentiveStatus { get; set; }
        public bool MedicalAidStatus { get; set; }
        public bool ProfessionalTaxStatus { get; set; }
        public bool BonusStatus { get; set; }
        public bool WelfareDeductionStatus { get; set; }
        public string PanNo { get; set; }
        public string AadhaarNo { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string IFSC { get; set; }
        public bool IsEnglish { get; set; }
        public bool IsHindi { get; set; }
        public bool IsMalayalam { get; set; }
        public bool IsTamil { get; set; }
        public bool IsTelugu { get; set; }
        public bool IsKannada { get; set; }
        public List<ExEmployDetailModel> ExEmployDetails { get; set; }
        public List<SalaryDetailsModel> SalaryDetails { get; set; }
        public string MobileNumber { get; set; }
        public string TranscationRole { get; set; }
        public string MISReportRole { get; set; }
        public string D2DReportRole { get; set; }
        public string PFVoluntaryContribution { get; set; }
        public string PFAccountNo { get; set; }
        public string PFUAN { get; set; }
        public string ESINo { get; set; }
        public string DepartmentName { get; set; }
        public string EmplyeeCategory { get; set; }
        public string PayRollCategory { get; set; }
        //public string PayrollCategory { get; set; }
        public string InterCompany { get; set; }
        public string Location { get; set; }
        public string Password { get; set; }
        public int ASPNetUserID { get; set; }

        public int IsBilling { get; set; }
        public int IsShipping { get; set; }
        public List<EmployeeAddressModel> AddressList { get; set; }
        public SelectList DefaultStoreList { get; set; }
        public string StoreName { get; set; }
        public int StoreID { get; set; }
        public SelectList UserLocationList { get; set; }
        public int UserLocationID { get; set; }
        public string UserLocationName { get; set; }
        public bool IsAlreadyERPUser { get; set; }
        public bool ChangePassword { get; set; }
        public List<FreeMedicineLocationMapping> FreeMedicineLocationList { get; set; }

    }
    public class EmployeeSalaryComponentsModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Formula { get; set; }

    }
    public class EmpAddressModel
    {
        public int AddressID { get; set; }
        public string LandLine1 { get; set; }
        public string LandLine2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PIN { get; set; }
        public string MobileNo { get; set; }
        public string ContactPerson { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public bool IsBilling { get; set; }
        public bool IsShipping { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultShipping { get; set; }
    }
    public class ExEmployDetailModel
    {
        public string EmployerName { get; set; }
        public string Designation { get; set; }
        public string ExEmployAddress1 { get; set; }
        public string ExEmployAddress2 { get; set; }
        public string ExEmployAddress3 { get; set; }
        public string ExEmployPlace { get; set; }
        public string ExEmployPin { get; set; }
        public string DateOfJoinning { get; set; }
        public string DateOfSeverance { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public int CountryID { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Country { get; set; }



    }

    public class SalaryDetailsModel
    {
        public string PayType { get; set; }
        public bool IsFinancePayRoll { get; set; }
        public bool IsProductionIncentivePayRoll { get; set; }
        public bool IsSalesIncentivePayRoll { get; set; }
        public Decimal SalaryMonthly { get; set; }
        public Decimal SalaryAnnual { get; set; }
        public bool PaytypeStatus { get; set; }
    }
    public class EmployeeAddressModel
    {
        public int AddressID { get; set; }
        public string LandLine1 { get; set; }
        public string LandLine2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PIN { get; set; }
        public string MobileNo { get; set; }
        public string ContactPerson { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public bool IsBilling { get; set; }
        public bool IsShipping { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultShipping { get; set; }
    }

    public class FreeMedicineLocationMapping
    {
        public int LocationID { get; set; }
        public int CustomerLocationID { get; set; }
        public string LocationName { get; set; }
    }
}