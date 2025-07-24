using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CustomerBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int StateID { get; set; }
        public int  CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        
        public int DistrictID { get; set; }
        public int SchemeID { get; set; }
        public bool IsGSTRegistered { get; set; }
        public int PriceListID { get; set; }
        public int OccupationID { get; set; }
        public string CustomerCategory { get; set; }
        public string PriceListName { get; set; }
        public int DiscountCategoryID { get; set; }
        public string DiscountCategory { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal CashDiscountPercentage { get; set; }
        public int CustomerTaxCategoryID { get; set; }
        public string CustomerTaxCategoryName { get; set; }
        public int CustomerAccountsCategoryID { get; set; }
        public string CustomerAccountsCategoryName { get; set; }
        public string PaymentTypeName { get; set; }
        public int CreditDaysID { get; set; }
        public string CreditDaysName { get; set; }
        public int CreditDays { get; set; }
        public string AadhaarNo { get; set; }
        public string PanNo { get; set; }
        public string EmailID { get; set; }
        public string FaxNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ContactPersonName { get; set; }
        public int CategoryID { get; set; }
        public int DiscountID { get; set; }
        public int CashDiscountID { get; set; }
        public int PaymentTypeID { get; set; }
        public decimal MinCreditLimit { get; set; }
        public decimal MaxCreditLimit { get; set; }
        public string GstNo { get; set; }
        public string Name2 { get; set; }
        public string Currency { get; set; }
        public string OldCode { get; set; }
        public string OldName { get; set; }
        public string CategoryName { get; set; }
        public bool IsInterCompany { get; set; }
        public string CustomerTaxCategory { get; set; }
        public bool IsMappedtoExpsEntries { get; set; }
        public bool IsBlockedForSalesOrders { get; set; }
        public bool IsBlockedForSalesInvoices { get; set; }
        public bool IsAlsoASupplier { get; set; }
        public int SupplierID { get; set; }
        public int CustomerRouteID { get; set; }
        public int CashDiscountCategoryID { get; set; }
        public int FSOID { get; set; }
        public string FSOName { get; set; }
        public string Color { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
        public decimal CustomerMonthlyTarget { get; set; }
        public string TradeLegalName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int CountryID { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Category { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Email { get; set; }
        public string SupplierName { get; set; }
        public bool IsMappedToServiceSales { get; set; }
        public bool IsBlockedForChequeReceipt { get; set; }
        public string PinCode { get; set; }
        public bool IsDraft { get; set; }
        public int AccountHeadID { get; set; }

        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string GuardianName { get; set; }
        public string Occupation { get; set; }
        public string MartialStatus { get; set; }
        public string Gender { get; set; }
        public int BloodGroupID { get; set; }
        public string BloodGroup { get; set; }
        public int Age { get; set; }
        public string ReferalContactNo { get; set; }
        public int PatientReferedByID { get; set; }
        public string PatientReferedBy { get; set; }
        public string PurposeOfVisit { get; set; }
        public string PassportNo { get; set; }
        public DateTime DateOfIssuePassport { get; set; }
        public DateTime DateOfIssueVisa { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public string PlaceOfIssue { get; set; }
        public string VisaNo { get; set; }
        public DateTime DateOfExpiryVisa { get; set; }
        public string DestinationTo { get; set; }
        public string ArrivedFrom { get; set; }
        public DateTime DateOfArrival { get; set; }
        public string ProceedingTo { get; set; }
        public int DurationOfStay { get; set; }
        public int PhotoID { get; set; }
        public int PassportID { get; set; }
        public int VisaID { get; set; }
        public string EmployedIn { get; set; }
        public string Country { get; set; }

        public string LandLine { get; set; }
        public string Place { get; set; }
        public int Month { get; set; }

        public int DiscountTypeID { get; set; }
        public decimal MaxDisccountAmount { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }

        public string ReferalName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CountryCode { get; set; }
        public string OtherQuotationIDS { get; set; }
        public string EmergencyContactNo { get; set; }
        public List<FileBO> OtherQuotation { get; set; }

    }
    public class DuplicateCheckBO
    {
        public string Message { get; set; }
        public bool IsDuplicate { get; set; }
    }
    public class PatientsListBO
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string patientsList { get; set; }
        }

    public class TreatmentPatientsListBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int AppointmentProcessID { get; set; }
        public string patientsList { get; set; }
    }

}

