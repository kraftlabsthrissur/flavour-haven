using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using Microsoft.AspNet.Identity.Owin;
using TradeSuiteApp.Web.Utils;
using Microsoft.AspNet.Identity;
namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class DoctorController : Controller
    {
        private IDoctorContract doctorBL;
        private IEmployeeContract employeeBL;
        private IDesignationContract designationBL;
        private IStateContract stateBL;
        private ICategoryContract EmpCategoryBL;
        private ISubmasterContract submasterBL;
        private ILocationContract locationBL;
        private IInterCompanyContract intercompanyBL;
        private IDepartmentContract departmentBL;
        private ICountryContract countryBL;
        private IAddressContract addressBL;
        private IGeneralContract generalBL;
        private IConfigurationContract configurationBL;
        private ApplicationUserManager _userManager;
        public DoctorController(ApplicationUserManager userManager)
        {
            doctorBL = new DoctorBL();
            employeeBL = new EmployeeBL();
            designationBL = new DesignationBL();
            stateBL = new StateBL();
            submasterBL = new SubmasterBL();
            EmpCategoryBL = new CategoryBL();
            locationBL = new LocationBL();
            intercompanyBL = new InterCompanyBL();
            departmentBL = new DepartmentBL();
            countryBL = new CountryBL();
            generalBL = new GeneralBL();
            addressBL = new AddressBL();
            configurationBL = new ConfigurationBL();
            UserManager = userManager;
        }
        public JsonResult GetDoctorList(DatatableModel Datatable)
        {
            try
            {

                DatatableResultBO resultBO = doctorBL.GetDoctorList(Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDoctorAutoComplete(string Hint = "", int CustomerCategoryID = 0)
        {
            DatatableResultBO resultBO = doctorBL.GetDoctorList("", Hint, "Name" ,"ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            employeeList = employeeBL.GetEmployeeList().Select(a => new EmployeeModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Place = a.Place

            }).ToList();
            return View(employeeList);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            List<ExEmployDetailBO> ExEmployDetailsBO;
            List<SalaryDetailsBO> SalaryDetailsBO;
            EmployeeModel employeeModel;
            ExEmployDetailModel ExEmployDetail;
            SalaryDetailsModel SalaryDetail;
            EmployeeBO emploeeBO = doctorBL.GetEmployee((int)id);
            employeeModel = new EmployeeModel()
            {

                Code = emploeeBO.Code,
                Title = emploeeBO.Title,
                Name = emploeeBO.Name,
                DepartmentName = emploeeBO.DepartmentName,
                EmplyeeCategory = emploeeBO.EmplyeeCategory,
                Designation = emploeeBO.Designation,
                //DateOfJoining = General.FormatDate(emploeeBO.DateOfJoining),
                DateOfJoining = emploeeBO.DateOfJoining == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfJoining),
                Gender = emploeeBO.Gender,
                MartialStatus = emploeeBO.MartialStatus,
                //DOB = General.FormatDate(emploeeBO.DOB),
                DOB = emploeeBO.DOB == null ? "" : General.FormatDate((DateTime)emploeeBO.DOB),
                Qualification1 = emploeeBO.Qualification1,
                Qualification2 = emploeeBO.Qualification2,
                Qualification3 = emploeeBO.Qualification3,
                BloodGroup = emploeeBO.BloodGroup,
                NoOfDependent = (int)emploeeBO.NoOfDependent,
                NameOfSpouse = emploeeBO.NameOfSpouse,
                NameOfGuardian = emploeeBO.NameOfGuardian,
                IsExcludeFromPayroll = emploeeBO.IsExcludeFromPayroll,
                //DateOfConfirmation = General.FormatDate(emploeeBO.DateOfConfirmation),
                DateOfConfirmation = emploeeBO.DateOfConfirmation == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfConfirmation),
                PayRollCategory = emploeeBO.PayRollCategory,
                PayGrade = emploeeBO.PayGrade,
                CompanyEmail = emploeeBO.CompanyEmail,
                ReportingCode = emploeeBO.ReportingCode,
                ReportingName = emploeeBO.ReportingName,
                InterCompany = emploeeBO.InterCompany,
                DateOfSeverance = emploeeBO.DateOfSeverance == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfSeverance),
                DateOfReJoining = emploeeBO.DateOfReJoining == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfReJoining),
                ProbationDuration = emploeeBO.ProbationDuration,
                EmploymentJobType = emploeeBO.EmploymentJobType,
                PrintPayroll = emploeeBO.PrintPayroll,
                PFStatus = emploeeBO.PFStatus,
                ESIStatus = emploeeBO.ESIStatus,
                NPSStatus = emploeeBO.NPSStatus,
                MedicalInsuranceStatus = emploeeBO.MedicalInsuranceStatus,
                AttandancePunchingStatus = emploeeBO.AttandancePunchingStatus,
                MultiLocationPunchingStatus = emploeeBO.MultiLocationPunchingStatus,
                SpecialLeaveStatus = emploeeBO.SpecialLeaveStatus,
                ProbationStatus = emploeeBO.ProbationStatus,
                ProductionIncentiveStatus = emploeeBO.ProductionIncentiveStatus,
                SalesIncentiveStatus = emploeeBO.SalesIncentiveStatus,
                FixedIncentiveStatus = emploeeBO.FixedIncentiveStatus,
                MinimumWagesStatus = emploeeBO.MinimumWagesStatus,
                IsERPUser = emploeeBO.IsERPUser,
                MedicalAidStatus = emploeeBO.MedicalAidStatus,
                BonusStatus = emploeeBO.BonusStatus,
                ProfessionalTaxStatus = emploeeBO.ProfessionalTaxStatus,
                WelfareDeductionStatus = emploeeBO.WelfareDeductionStatus,
                PanNo = emploeeBO.PanNo,
                AadhaarNo = emploeeBO.AadhaarNo,
                AccountNumber = emploeeBO.AccountNumber,
                BankName = emploeeBO.BankName,
                BankBranchName = emploeeBO.BankBranchName,
                IFSC = emploeeBO.IFSC,
                IsEnglish = emploeeBO.IsEnglish,
                IsHindi = emploeeBO.IsHindi,
                IsKannada = emploeeBO.IsKannada,
                IsMalayalam = emploeeBO.IsMalayalam,
                IsTamil = emploeeBO.IsTamil,
                IsTelugu = emploeeBO.IsTelugu,
                PFVoluntaryContribution = emploeeBO.PFVoluntaryContribution,
                PFAccountNo = emploeeBO.PFAccountNo,
                PFUAN = emploeeBO.PFUAN,
                ESINo = emploeeBO.ESINo,
                Location = emploeeBO.Location,
                LocationID = emploeeBO.LocationID,
                DoctorFee=emploeeBO.DoctorFee,
                ClinicFee=emploeeBO.ClinicFee,
                UserName=emploeeBO.UserName
            };
            employeeModel.ID = (int)id;
            employeeModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            ExEmployDetailsBO = employeeBL.GetExEmployDetails((int)id);
            employeeModel.ExEmployDetails = new List<ExEmployDetailModel>();
            foreach (var m in ExEmployDetailsBO)
            {
                ExEmployDetail = new ExEmployDetailModel()
                {
                    EmployerName = m.EmployerName,
                    Designation = m.Designation,
                    ExEmployAddress1 = m.ExEmployAddress1,
                    ExEmployAddress2 = m.ExEmployAddress2,
                    ExEmployAddress3 = m.ExEmployAddress3,
                    ExEmployPin = m.ExEmployPin,
                    ExEmployPlace = m.ExEmployPlace,
                    DateOfJoinning = General.FormatDate(m.DateOfJoinning),
                    DateOfSeverance = General.FormatDate(m.DateOfSeverance),
                    ContactPerson = m.ContactPerson,
                    ContactNumber = m.ContactNumber,
                    State = m.State,
                    Country = m.Country,
                    District = m.District
                };
                employeeModel.ExEmployDetails.Add(ExEmployDetail);
            }

            SalaryDetailsBO = employeeBL.GetSalaryDetails((int)id);
            employeeModel.SalaryDetails = new List<SalaryDetailsModel>();
            foreach (var m in SalaryDetailsBO)
            {
                SalaryDetail = new SalaryDetailsModel()
                {
                    PayType = m.PayType,
                    SalaryAnnual = m.SalaryAnnual,
                    SalaryMonthly = m.SalaryMonthly,
                    IsFinancePayRoll = m.IsFinancePayRoll,
                    IsProductionIncentivePayRoll = m.IsProductionIncentivePayRoll,
                    IsSalesIncentivePayRoll = m.IsSalesIncentivePayRoll,
                    PaytypeStatus = m.PaytypeStatus
                };
                employeeModel.SalaryDetails.Add(SalaryDetail);
            }
            employeeModel.TitleList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Mr", Value ="MR", },
                                                   new SelectListItem { Text = "Ms", Value ="MS", },
                                                 new SelectListItem { Text = "Mrs", Value = "MRS"},
                                                 }, "Value", "Text");
            return View(employeeModel);
        }

        public ActionResult Create()
        {
            EmployeeModel Employee = new EmployeeModel();
            SalaryDetailsModel SalaryDetail;
            List<SalaryDetailsBO> SalaryDetailsBO;
            Employee.Code = generalBL.GetSerialNo("Employee", "Code");

            Employee.TitleList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "MR", Value ="MR", },
                                                   new SelectListItem { Text = "MS", Value ="MS", },
                                                 new SelectListItem { Text = "MRS", Value = "MRS"},
                                                 }, "Value", "Text");
            Employee.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male", },
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                    new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            Employee.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single", },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 }, "Value", "Text");
            Employee.PrintPayrollList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "With E-mail", Value ="With E-mail", },
                                                 new SelectListItem { Text = "WithOut E-mail", Value = "WithOut E-mail"},
                                                 }, "Value", "Text");
            Employee.DesignationList = new SelectList(designationBL.GetDesignationList(), "ID", "Name");
            Employee.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            Employee.EmpCategoryList = new SelectList(EmpCategoryBL.GetEmployeeCategoryList(), "ID", "Name");
            Employee.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            Employee.BloodGroupList = new SelectList(submasterBL.GetBloodGroupList(), "ID", "Name");
            Employee.InterCompanyList = new SelectList(intercompanyBL.GetInterCompanyList(), "ID", "Name");
            Employee.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            Employee.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            Employee.EmploymentJobTypeList = new SelectList(employeeBL.GetEmployeeJobTypeList(), "EmploymentJobTypeID", "EmploymentJobType");
            Employee.PayrollCategoryList = new SelectList(EmpCategoryBL.GetPayrollCategoryList(), "ID", "Name");
            SalaryDetailsBO = employeeBL.GetSalaryDetails(0);
            Employee.SalaryDetails = new List<SalaryDetailsModel>();
            foreach (var m in SalaryDetailsBO)
            {
                SalaryDetail = new SalaryDetailsModel()
                {
                    PayType = m.PayType,
                    SalaryAnnual = m.SalaryAnnual,
                    SalaryMonthly = m.SalaryMonthly,
                    IsFinancePayRoll = m.IsFinancePayRoll,
                    IsProductionIncentivePayRoll = m.IsProductionIncentivePayRoll,
                    IsSalesIncentivePayRoll = m.IsSalesIncentivePayRoll,
                    PaytypeStatus = m.PaytypeStatus
                };
                Employee.SalaryDetails.Add(SalaryDetail);
            }

            Employee.IsERPUser = false;
            return View(Employee);
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return View("PageNotFound");
            }

            List<ExEmployDetailBO> ExEmployDetailsBO;
            List<SalaryDetailsBO> SalaryDetailsBO;
            EmployeeModel employeeModel;
            ExEmployDetailModel ExEmployDetail;
            SalaryDetailsModel SalaryDetail;
            EmployeeBO emploeeBO = doctorBL.GetEmployee((int)id);
            employeeModel = new EmployeeModel()
            {
                Code = emploeeBO.Code,
                Title = emploeeBO.Title,
                Name = emploeeBO.Name,
                DepartmentName = emploeeBO.DepartmentName,
                EmplyeeCategory = emploeeBO.EmplyeeCategory,
                Designation = emploeeBO.Designation,
                DesignationID = emploeeBO.DesignationID,
                //DateOfJoining = General.FormatDate(emploeeBO.DateOfJoining),
                DateOfJoining = emploeeBO.DateOfJoining == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfJoining),
                Gender = emploeeBO.Gender,
                MartialStatus = emploeeBO.MartialStatus,
                //DOB = General.FormatDate(emploeeBO.DOB),
                DOB = emploeeBO.DOB == null ? "" : General.FormatDate((DateTime)emploeeBO.DOB),
                Qualification1 = emploeeBO.Qualification1,
                Qualification2 = emploeeBO.Qualification2,
                Qualification3 = emploeeBO.Qualification3,
                BloodGroup = emploeeBO.BloodGroup,
                NoOfDependent = (int)emploeeBO.NoOfDependent,
                NameOfSpouse = emploeeBO.NameOfSpouse,
                NameOfGuardian = emploeeBO.NameOfGuardian,
                IsExcludeFromPayroll = emploeeBO.IsExcludeFromPayroll,
                //DateOfConfirmation = General.FormatDate(emploeeBO.DateOfConfirmation),
                DateOfConfirmation = emploeeBO.DateOfConfirmation == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfConfirmation),
                PayRollCategory = emploeeBO.PayRollCategory,
                PayGrade = emploeeBO.PayGrade,
                CompanyEmail = emploeeBO.CompanyEmail,
                ReportingCode = emploeeBO.ReportingCode,
                ReportingName = emploeeBO.ReportingName,
                InterCompany = emploeeBO.InterCompany,
                DateOfSeverance = emploeeBO.DateOfSeverance == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfSeverance),
                DateOfReJoining = emploeeBO.DateOfReJoining == null ? "" : General.FormatDate((DateTime)emploeeBO.DateOfReJoining),
                ProbationDuration = emploeeBO.ProbationDuration,
                EmploymentJobType = emploeeBO.EmploymentJobType,
                PrintPayroll = emploeeBO.PrintPayroll,
                PFStatus = emploeeBO.PFStatus,
                ESIStatus = emploeeBO.ESIStatus,
                NPSStatus = emploeeBO.NPSStatus,
                MedicalInsuranceStatus = emploeeBO.MedicalInsuranceStatus,
                AttandancePunchingStatus = emploeeBO.AttandancePunchingStatus,
                MultiLocationPunchingStatus = emploeeBO.MultiLocationPunchingStatus,
                SpecialLeaveStatus = emploeeBO.SpecialLeaveStatus,
                ProbationStatus = emploeeBO.ProbationStatus,
                ProductionIncentiveStatus = emploeeBO.ProductionIncentiveStatus,
                SalesIncentiveStatus = emploeeBO.SalesIncentiveStatus,
                FixedIncentiveStatus = emploeeBO.FixedIncentiveStatus,
                MinimumWagesStatus = emploeeBO.MinimumWagesStatus,
                IsERPUser = emploeeBO.IsERPUser,
                MedicalAidStatus = emploeeBO.MedicalAidStatus,
                BonusStatus = emploeeBO.BonusStatus,
                ProfessionalTaxStatus = emploeeBO.ProfessionalTaxStatus,
                WelfareDeductionStatus = emploeeBO.WelfareDeductionStatus,
                PanNo = emploeeBO.PanNo,
                AadhaarNo = emploeeBO.AadhaarNo,
                AccountNumber = emploeeBO.AccountNumber,
                BankName = emploeeBO.BankName,
                BankBranchName = emploeeBO.BankBranchName,
                IFSC = emploeeBO.IFSC,
                IsEnglish = emploeeBO.IsEnglish,
                IsHindi = emploeeBO.IsHindi,
                IsKannada = emploeeBO.IsKannada,
                IsMalayalam = emploeeBO.IsMalayalam,
                IsTamil = emploeeBO.IsTamil,
                IsTelugu = emploeeBO.IsTelugu,
                PFVoluntaryContribution = emploeeBO.PFVoluntaryContribution,
                PFAccountNo = emploeeBO.PFAccountNo,
                PFUAN = emploeeBO.PFUAN,
                ESINo = emploeeBO.ESINo,
                DepartmentID = emploeeBO.DepartmentID,
                PayrollCategoryID = emploeeBO.PayrollCategoryID,
                InterCompanyID = emploeeBO.InterCompanyID,
                EmploymentJobTypeID = emploeeBO.EmploymentJobTypeID,
                BloodGroupID = emploeeBO.BloodGroupID,
                LocationID = emploeeBO.LocationID,
                EmpCategoryID = emploeeBO.EmpCategoryID,
                UserID = emploeeBO.UserID,
                ASPNetUserID = emploeeBO.ASPNetUserID,
                DoctorFee=emploeeBO.DoctorFee,
                ClinicFee=emploeeBO.ClinicFee,
                UserName=emploeeBO.UserName
            };
            employeeModel.ID = (int)id;
            ExEmployDetailsBO = employeeBL.GetExEmployDetails((int)id);
            employeeModel.ExEmployDetails = new List<ExEmployDetailModel>();
            foreach (var m in ExEmployDetailsBO)
            {
                ExEmployDetail = new ExEmployDetailModel()
                {
                    EmployerName = m.EmployerName,
                    Designation = m.Designation,
                    ExEmployAddress1 = m.ExEmployAddress1,
                    ExEmployAddress2 = m.ExEmployAddress2,
                    ExEmployAddress3 = m.ExEmployAddress3,
                    ExEmployPin = m.ExEmployPin,
                    ExEmployPlace = m.ExEmployPlace,
                    DateOfJoinning = General.FormatDate(m.DateOfJoinning),
                    DateOfSeverance = General.FormatDate(m.DateOfSeverance),
                    ContactPerson = m.ContactPerson,
                    ContactNumber = m.ContactNumber,
                    StateID = m.StateID,
                    CountryID = m.CountryID,
                    DistrictID = m.DistrictID,

                };
                employeeModel.ExEmployDetails.Add(ExEmployDetail);
            }

            SalaryDetailsBO = employeeBL.GetSalaryDetails((int)id);
            employeeModel.SalaryDetails = new List<SalaryDetailsModel>();
            foreach (var m in SalaryDetailsBO)
            {
                SalaryDetail = new SalaryDetailsModel()
                {
                    PayType = m.PayType,
                    SalaryAnnual = m.SalaryAnnual,
                    SalaryMonthly = m.SalaryMonthly,
                    IsFinancePayRoll = m.IsFinancePayRoll,
                    IsProductionIncentivePayRoll = m.IsProductionIncentivePayRoll,
                    IsSalesIncentivePayRoll = m.IsSalesIncentivePayRoll,
                    PaytypeStatus = m.PaytypeStatus
                };
                employeeModel.SalaryDetails.Add(SalaryDetail);
            }

            employeeModel.TitleList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "MR", Value ="MR"},
                                                 new SelectListItem { Text = "MS", Value = "MS"},
                                                 new SelectListItem { Text = "MRS", Value = "MRS"},
                                                 }, "Value", "Text");

            employeeModel.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male"},
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                 new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            employeeModel.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single" },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 }, "Value", "Text");
            employeeModel.PrintPayrollList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "With E-mail", Value ="With E-mail" },
                                                 new SelectListItem { Text = "WithOut E-mail", Value = "WithOut E-mail"},
                                                 }, "Value", "Text");
            employeeModel.DesignationList = new SelectList(designationBL.GetDesignationList(), "ID", "Name");
            employeeModel.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            employeeModel.EmpCategoryList = new SelectList(EmpCategoryBL.GetEmployeeCategoryList(), "ID", "Name");
            employeeModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            employeeModel.BloodGroupList = new SelectList(submasterBL.GetBloodGroupList(), "ID", "Name");
            employeeModel.InterCompanyList = new SelectList(intercompanyBL.GetInterCompanyList(), "ID", "Name");
            employeeModel.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            employeeModel.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            employeeModel.EmploymentJobTypeList = new SelectList(employeeBL.GetEmployeeJobTypeList(), "EmploymentJobTypeID", "EmploymentJobType");
            employeeModel.PayrollCategoryList = new SelectList(EmpCategoryBL.GetPayrollCategoryList(), "ID", "Name");
            employeeModel.DefaultStoreList = new SelectList(configurationBL.GetDefaultStoreList(0), "StoreID", "StoreName");
            employeeModel.UserLocationList = new SelectList(employeeBL.GetUserLocationList(employeeModel.UserID), "UserLocationID", "UserLocationName");
            employeeModel.IsAlreadyERPUser = employeeModel.UserID > 0 ? true : false;
            return View(employeeModel);
        }


        [HttpPost]
        public ActionResult Save(EmployeeModel model)
        {
            EmployeeBO employeeBo = new EmployeeBO()
            {
                ID = model.ID,
                Title = model.Title,
                Code = model.Code,
                Name = model.Name,
                DesignationID = model.DesignationID,
                Gender = model.Gender,
                MartialStatus = model.MartialStatus,
                Qualification1 = model.Qualification1,
                Qualification2 = model.Qualification2,
                Qualification3 = model.Qualification3,
                BloodGroup = model.BloodGroup,
                NoOfDependent = model.NoOfDependent,
                NameOfGuardian = model.NameOfGuardian,
                NameOfSpouse = model.NameOfSpouse,
                IsExcludeFromPayroll = model.IsExcludeFromPayroll,
                PayGrade = model.PayGrade,
                EmpCategoryID = model.EmpCategoryID,
                PayrollCategoryID = model.PayrollCategoryID,
                CompanyEmail = model.CompanyEmail,
                ReportingCode = model.ReportingCode,
                ReportingName = model.ReportingName,
                DepartmentID = model.DepartmentID,
                LocationID = model.LocationID,
                InterCompanyID = model.InterCompanyID,
                ProbationDuration = model.ProbationDuration,
                EmploymentJobTypeID = model.EmploymentJobTypeID,
                PrintPayroll = model.PrintPayroll,
                PFStatus = model.PFStatus,
                ESIStatus = model.ESIStatus,
                NPSStatus = model.NPSStatus,
                MedicalInsuranceStatus = model.MedicalInsuranceStatus,
                AttandancePunchingStatus = model.AttandancePunchingStatus,
                MultiLocationPunchingStatus = model.MultiLocationPunchingStatus,
                SpecialLeaveStatus = model.SpecialLeaveStatus,
                ProbationStatus = model.ProbationStatus,
                MinimumWagesStatus = model.MinimumWagesStatus,
                ProductionIncentiveStatus = model.ProductionIncentiveStatus,
                SalesIncentiveStatus = model.SalesIncentiveStatus,
                FixedIncentiveStatus = model.FixedIncentiveStatus,
                IsERPUser = model.IsERPUser,
                MedicalAidStatus = model.MedicalAidStatus,
                BonusStatus = model.BonusStatus,
                ProfessionalTaxStatus = model.ProfessionalTaxStatus,
                WelfareDeductionStatus = model.WelfareDeductionStatus,
                PanNo = model.PanNo,
                AadhaarNo = model.AadhaarNo,
                AccountNumber = model.AccountNumber,
                BankName = model.BankName,
                BankBranchName = model.BankBranchName,
                IFSC = model.IFSC,
                IsEnglish = model.IsEnglish,
                IsHindi = model.IsHindi,
                IsKannada = model.IsKannada,
                IsMalayalam = model.IsMalayalam,
                IsTamil = model.IsTamil,
                IsTelugu = model.IsTelugu,
                PFVoluntaryContribution = model.PFVoluntaryContribution,
                PFAccountNo = model.PFAccountNo,
                PFUAN = model.PFUAN,
                ESINo = model.ESINo,
                Password = model.Password,
                ASPNetUserID = model.ASPNetUserID,
                DoctorFee=model.DoctorFee,
                ClinicFee=model.ClinicFee
            };
            if (model.DateOfSeverance != "" && model.DateOfSeverance != null)
            {
                employeeBo.DateOfSeverance = General.ToDateTime(model.DateOfSeverance);
            }
            if (model.DateOfReJoining != "" && model.DateOfReJoining != null)
            {
                employeeBo.DateOfReJoining = General.ToDateTime(model.DateOfReJoining);
            }
            if (model.DateOfJoining != "" && model.DateOfJoining != null)
            {
                employeeBo.DateOfJoining = General.ToDateTime(model.DateOfJoining);
            }
            if (model.DateOfConfirmation != "" && model.DateOfConfirmation != null)
            {
                employeeBo.DateOfConfirmation = General.ToDateTime(model.DateOfConfirmation);
            }
            if (model.DOB != "" && model.DOB != null)
            {
                employeeBo.DOB = General.ToDateTime(model.DOB);
            }
            if (model.DepartmentID == 0 || model.DesignationID == 0)
            {
                var obj = employeeBL.GetDepartmentID();
                if (model.DesignationID == 0)
                {
                    employeeBo.DepartmentID = obj.DepartmentID;
                }
                if (model.DesignationID == 0)
                {
                    employeeBo.DesignationID = obj.DesignationID;
                }
            }
            List<ExEmployDetailBO> ExemployDetails = new List<ExEmployDetailBO>();
            if (model.ExEmployDetails != null)
            {
                ExEmployDetailBO ExemployDetail;
                foreach (var item in model.ExEmployDetails)
                {
                    ExemployDetail = new ExEmployDetailBO()
                    {
                        EmployerName = item.EmployerName,
                        Designation = item.Designation,
                        ExEmployAddress1 = item.ExEmployAddress1,
                        ExEmployAddress2 = item.ExEmployAddress2,
                        ExEmployAddress3 = item.ExEmployAddress3,
                        ExEmployPlace = item.ExEmployPlace,
                        ExEmployPin = item.ExEmployPin,
                        DateOfJoinning = General.ToDateTime(item.DateOfJoinning),
                        DateOfSeverance = General.ToDateTime(item.DateOfSeverance),
                        ContactNumber = item.ContactNumber,
                        ContactPerson = item.ContactPerson,
                        StateID = item.StateID,
                        CountryID = item.CountryID,
                        DistrictID = item.DistrictID
                    };

                    ExemployDetails.Add(ExemployDetail);
                }
            }
            List<SalaryDetailsBO> SalaryDetails = new List<SalaryDetailsBO>();
            if (model.SalaryDetails != null)
            {
                SalaryDetailsBO SalaryDetail;
                foreach (var item in model.SalaryDetails)
                {
                    SalaryDetail = new SalaryDetailsBO()
                    {
                        PayType = item.PayType,
                        SalaryMonthly = item.SalaryMonthly,
                        SalaryAnnual = item.SalaryAnnual,
                        IsFinancePayRoll = item.IsFinancePayRoll,
                        IsProductionIncentivePayRoll = item.IsProductionIncentivePayRoll,
                        IsSalesIncentivePayRoll = item.IsSalesIncentivePayRoll
                    };
                    SalaryDetails.Add(SalaryDetail);
                }
            }

            List<AddressBO> AddressList = new List<AddressBO>();
            AddressBO AddressItem;
            if (model.AddressList != null)
            {
                foreach (var item in model.AddressList)
                {
                    AddressItem = new AddressBO()
                    {
                        AddressID = item.AddressID,
                        AddressLine1 = item.AddressLine1,
                        AddressLine2 = item.AddressLine2,
                        AddressLine3 = item.AddressLine3,
                        Place = item.Place,
                        ContactPerson = item.ContactPerson,
                        LandLine1 = item.LandLine1,
                        LandLine2 = item.LandLine2,
                        MobileNo = item.MobileNo,
                        StateID = item.StateID,
                        PIN = item.PIN,
                        Fax = item.Fax,
                        DistrictID = item.DistrictID,
                        Email = item.Email,
                        IsBilling = item.IsBilling,
                        IsShipping = item.IsShipping,
                        IsDefault = item.IsDefault,
                        IsDefaultShipping = item.IsDefaultShipping,
                    };
                    AddressList.Add(AddressItem);
                }
            }
            List<EmployeeBO> FreeMedicineLocationList = new List<EmployeeBO>();
            EmployeeBO FreeMedicineLocationMapping;
            if (model.FreeMedicineLocationList != null)
            {
                foreach (var item in model.FreeMedicineLocationList)
                {
                    FreeMedicineLocationMapping = new EmployeeBO()
                    {
                        LocationID = item.LocationID
                    };
                    FreeMedicineLocationList.Add(FreeMedicineLocationMapping);
                }
            }
            if (model.UserName == null)
            {
                model.UserName = model.Name;
            }
            if (model.Password == null)
            {
                model.Password = "user123";
            }
            try
            {
                if (employeeBo.ID == 0)
                {
                    var user = new ApplicationUser { UserName = model.UserName, Email = AddressList.Select(e => e.Email).FirstOrDefault() };
                    var result = _userManager.Create(user, model.Password);
                    employeeBo.UserID = user.Id;
                    if (!result.Succeeded)
                    {
                        return Json(new
                        {
                            Status = "failure",
                            data = "",
                            Message = result.Errors.FirstOrDefault().ToString()
                        }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        model.Code = doctorBL.Save(employeeBo, ExemployDetails, SalaryDetails, AddressList, FreeMedicineLocationList);

                    }
                    model.ASPNetUserID = user.Id;
                }
                else
                {
                    model.Code = doctorBL.Save(employeeBo, ExemployDetails, SalaryDetails, AddressList, FreeMedicineLocationList);

                    if ((model.ChangePassword == true && model.IsERPUser == true))
                    {
                        var user = UserManager.FindById(model.ASPNetUserID);
                        user.UserName = model.UserName;
                        UserManager.Update(user);
                        var removePassword = UserManager.RemovePassword(model.ASPNetUserID);
                        if (removePassword.Succeeded)
                        {
                            //Removed Password Success
                            var AddPassword = UserManager.AddPassword(model.ASPNetUserID, model.Password);
                            if (!AddPassword.Succeeded)
                            {
                                return Json(new
                                {
                                    Status = "failure",
                                    data = "",
                                    Message = AddPassword.Errors.FirstOrDefault().ToString()
                                }, JsonRequestBehavior.AllowGet);

                            }

                        }

                    }

                    employeeBL.UpdateUserID(model.Code, model.ASPNetUserID);
                }

                return Json(new
                {
                    Status = "success",
                    data = "",
                    Message = ""
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Status = "failure",
                    data = "",
                    Message = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }
}
