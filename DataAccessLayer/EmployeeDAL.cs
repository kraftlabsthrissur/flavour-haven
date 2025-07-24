using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DBContext;
using BusinessObject;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class EmployeeDAL
    {

        public DatatableResultBO GetEmployeeList(int EmployeeCategoryID, int DefaultLocationID, string Type, string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetEmployeeList(EmployeeCategoryID, DefaultLocationID, Type, CodeHint, NameHint, DepartmentHint, LocationHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.EmployeeName,
                                Location = item.Location,
                                Department = item.Department,
                                UserID = item.UserID,
                                BalAmount = (decimal)item.BalanceAmount,
                                DesignationID = item.DesignationID,
                                Designation = item.Designation
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public List<EmployeeBO> GetEmployeeList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetEmployee().Select(Employee => new EmployeeBO
                {
                    ID = Employee.ID,
                    Name = Employee.Name,
                    Code = Employee.Code
                }).ToList();

            }
        }
        public List<EmployeeBO> GetDoctorsList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetDoctorsList().Select(Employee => new EmployeeBO
                {
                    ID = Employee.ID,
                    Name = Employee.Name,
                    Code = Employee.Code
                }).ToList();

            }
        }

        public List<EmployeeBO> GetEmployeeForAutoComplete(string Hint, int offset = 0, int limit = 10)
        {
            List<EmployeeBO> Employee = new List<EmployeeBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Employee = dbEntity.SpGetEmployeeAutoComplete(GeneralBO.LocationID, GeneralBO.ApplicationID, Hint, offset, limit).Select(a => new EmployeeBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Place = a.Location,
                        UserID = (int)a.UserID,
                        DesignationID = (int)a.DesignationID,
                        Designation = a.Designation
                    }).ToList();
                    return Employee;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeSalaryComponentBO> GetSalaryComponentList()
        {
            List<EmployeeSalaryComponentBO> SalaryComponentList = new List<EmployeeSalaryComponentBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    SalaryComponentList = dbEntity.SpGetSalaryComponents().Select(a => new EmployeeSalaryComponentBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Formula = a.Formula
                    }).ToList();
                    return SalaryComponentList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeBO> GetEmployeeJobTypeList()
        {
            List<EmployeeBO> JobTypeList = new List<EmployeeBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    JobTypeList = dbEntity.SpGetEmployeeJobTypeList().Select(a => new EmployeeBO
                    {
                        EmploymentJobTypeID = a.ID,
                        EmploymentJobType = a.Name
                    }).ToList();
                    return JobTypeList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Save(EmployeeBO employeeBo, List<ExEmployDetailBO> ExemployDetails, List<SalaryDetailsBO> SalaryDetails, List<AddressBO> AddressList, List<EmployeeBO> FreeMedicineLocationList)
        {
            try
            {
                ObjectParameter EmployeeID = new ObjectParameter("EmployeeID", typeof(int));
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    if (employeeBo.IsExcludeFromPayroll)
                    {
                        var j = dbEntity.SpUpdateSerialNo("Virtual Employee", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    }
                    else
                    {
                        var j = dbEntity.SpUpdateSerialNo("Employee", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    }
                    var i = dbEntity.SpCreateEmployee(
                            SerialNo.Value.ToString(),
                            employeeBo.Title,
                            employeeBo.Name,
                            employeeBo.DepartmentID,
                            employeeBo.MobileNumber,
                            employeeBo.Place,
                            employeeBo.UserID,
                            employeeBo.EmpCategoryID,
                            employeeBo.DesignationID,
                            employeeBo.DateOfJoining,
                            employeeBo.Gender,
                            employeeBo.MartialStatus,
                            employeeBo.DOB,
                            employeeBo.Qualification1,
                            employeeBo.Qualification2,
                            employeeBo.Qualification3,
                            employeeBo.BloodGroup,
                            employeeBo.NoOfDependent,
                            employeeBo.NameOfSpouse,
                            employeeBo.NameOfGuardian,
                            employeeBo.IsExcludeFromPayroll,
                            employeeBo.DateOfConfirmation,
                            employeeBo.PayrollCategoryID,
                            employeeBo.PayGrade,
                            employeeBo.CompanyEmail,
                            employeeBo.ReportingCode,
                            employeeBo.ReportingName,
                            employeeBo.TranscationRole,
                            employeeBo.D2DReportRole,
                            employeeBo.MISReportRole,
                            employeeBo.InterCompanyID,
                            employeeBo.DateOfSeverance,
                            employeeBo.DateOfReJoining,
                            employeeBo.ProbationDuration,
                            employeeBo.EmploymentJobTypeID,
                            employeeBo.PrintPayroll,
                            employeeBo.PFStatus,
                            employeeBo.ESIStatus,
                            employeeBo.NPSStatus,
                            employeeBo.MedicalInsuranceStatus,
                            employeeBo.AttandancePunchingStatus,
                            employeeBo.MultiLocationPunchingStatus,
                            employeeBo.SpecialLeaveStatus,
                            employeeBo.ProbationStatus,
                            employeeBo.ProductionIncentiveStatus,
                            employeeBo.SalesIncentiveStatus,
                            employeeBo.FixedIncentiveStatus,
                            employeeBo.MinimumWagesStatus,
                            employeeBo.IsERPUser,
                            employeeBo.MedicalAidStatus,
                            employeeBo.BonusStatus,
                            employeeBo.ProfessionalTaxStatus,
                            employeeBo.WelfareDeductionStatus,
                            employeeBo.PanNo,
                            employeeBo.AadhaarNo,
                            employeeBo.AccountNumber,
                            employeeBo.BankName,
                            employeeBo.BankBranchName,
                            employeeBo.IFSC,
                            employeeBo.IsEnglish,
                            employeeBo.IsHindi,
                            employeeBo.IsMalayalam,
                            employeeBo.IsTamil,
                            employeeBo.IsTelugu,
                            employeeBo.IsKannada,
                            employeeBo.PFVoluntaryContribution,
                            employeeBo.PFAccountNo,
                            employeeBo.PFUAN,
                            employeeBo.ESINo,
                            employeeBo.LocationID,
                            GeneralBO.ApplicationID,
                            EmployeeID
                            );
                    dbEntity.SaveChanges();
                    int employeeID = Convert.ToInt32(EmployeeID.Value);
                    if (ExemployDetails != null)
                    {
                        foreach (var item in ExemployDetails)
                        {
                            dbEntity.SpCreateExEmployment(employeeID,
                              item.EmployerName,
                              item.Designation,
                              item.ExEmployAddress1,
                              item.ExEmployAddress2,
                              item.ExEmployAddress3,
                              item.ExEmployPlace,
                              item.ExEmployPin,
                              item.DateOfJoinning,
                              item.DateOfSeverance,
                              item.ContactPerson,
                              item.ContactNumber,
                              item.StateID,
                              item.CountryID,
                              item.DistrictID
                                );
                        }
                        dbEntity.SaveChanges();
                    }
                    if (AddressList != null)
                    {
                        String PartyType = "Employee";
                        foreach (var item in AddressList)
                        {
                            dbEntity.SpCreateAddress(PartyType,
                                employeeID,
                                item.AddressLine1,
                                item.AddressLine2,
                                item.AddressLine3,
                                0,
                                item.ContactPerson,
                                item.Place,
                                item.DistrictID,
                                item.StateID,
                                item.PIN,
                                item.LandLine1,
                                item.LandLine2,
                                item.MobileNo,
                                item.Fax,
                                item.Email,
                                item.IsBilling,
                                item.IsShipping,
                                item.IsDefault,
                                item.IsDefaultShipping,
                                GeneralBO.CreatedUserID,
                                DateTime.Now,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID);
                            dbEntity.SaveChanges();
                        }
                    }

                    if (SalaryDetails != null)
                    {
                        foreach (var item in SalaryDetails)
                        {
                            dbEntity.SpCreateSalaryDetails(employeeID,
                              item.PayType,
                              item.SalaryMonthly,
                              item.SalaryAnnual,
                              item.IsFinancePayRoll,
                              item.IsProductionIncentivePayRoll,
                              item.IsSalesIncentivePayRoll
                                );
                        }
                    }
                    if (FreeMedicineLocationList != null)
                    {
                        foreach (var item in FreeMedicineLocationList)
                        {
                            dbEntity.SpCreateFreeMedicineLocationMapping(employeeID, item.LocationID, GeneralBO.ApplicationID);
                        }
                    }

                };

                return SerialNo.Value.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string Update(EmployeeBO employeeBo, List<ExEmployDetailBO> ExemployDetails, List<SalaryDetailsBO> SalaryDetails, List<AddressBO> AddressList, List<EmployeeBO> FreeMedicineLocationList)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter EmployeeID = new ObjectParameter("EmployeeID", typeof(int));
                    var i = dbEntity.SpUpdateEmployee(
                            employeeBo.Code,
                            employeeBo.Title,
                            employeeBo.Name,
                            employeeBo.DepartmentID,
                            employeeBo.MobileNumber,
                            employeeBo.Place,
                            employeeBo.UserID,
                            employeeBo.EmpCategoryID,
                            employeeBo.DesignationID,
                            employeeBo.DateOfJoining,
                            GeneralBO.ApplicationID,
                            employeeBo.LocationID,
                            employeeBo.Gender,
                            employeeBo.MartialStatus,
                            employeeBo.DOB,
                            employeeBo.Qualification1,
                            employeeBo.Qualification2,
                            employeeBo.Qualification3,
                            employeeBo.BloodGroup,
                            employeeBo.NoOfDependent,
                            employeeBo.NameOfSpouse,
                            employeeBo.NameOfGuardian,
                            employeeBo.IsExcludeFromPayroll,
                            employeeBo.DateOfConfirmation,
                            employeeBo.PayrollCategoryID,
                            employeeBo.PayGrade,
                            employeeBo.CompanyEmail,
                            employeeBo.ReportingCode,
                            employeeBo.ReportingName,
                            employeeBo.TranscationRole,
                            employeeBo.D2DReportRole,
                            employeeBo.MISReportRole,
                            employeeBo.InterCompanyID,
                            employeeBo.DateOfSeverance,
                            employeeBo.DateOfReJoining,
                            employeeBo.ProbationDuration,
                            employeeBo.EmploymentJobTypeID,
                            employeeBo.PrintPayroll,
                            employeeBo.PFStatus,
                            employeeBo.ESIStatus,
                            employeeBo.NPSStatus,
                            employeeBo.MedicalInsuranceStatus,
                            employeeBo.AttandancePunchingStatus,
                            employeeBo.MultiLocationPunchingStatus,
                            employeeBo.SpecialLeaveStatus,
                            employeeBo.ProbationStatus,
                            employeeBo.ProductionIncentiveStatus,
                            employeeBo.SalesIncentiveStatus,
                            employeeBo.FixedIncentiveStatus,
                            employeeBo.MinimumWagesStatus,
                            employeeBo.IsERPUser,
                            employeeBo.MedicalAidStatus,
                            employeeBo.BonusStatus,
                            employeeBo.ProfessionalTaxStatus,
                            employeeBo.WelfareDeductionStatus,
                            employeeBo.PanNo,
                            employeeBo.AadhaarNo,
                            employeeBo.AccountNumber,
                            employeeBo.BankName,
                            employeeBo.BankBranchName,
                            employeeBo.IFSC,
                            employeeBo.IsEnglish,
                            employeeBo.IsHindi,
                            employeeBo.IsMalayalam,
                            employeeBo.IsTamil,
                            employeeBo.IsTelugu,
                            employeeBo.IsKannada,
                            employeeBo.PFVoluntaryContribution,
                            employeeBo.PFAccountNo,
                            employeeBo.PFUAN,
                            employeeBo.ESINo,
                            employeeBo.ID,
                            GeneralBO.CreatedUserID
                            );
                    dbEntity.SaveChanges();
                    if (ExemployDetails != null)
                    {
                        foreach (var item in ExemployDetails)
                        {
                            dbEntity.SpCreateExEmployment(
                              employeeBo.ID,
                              item.EmployerName,
                              item.Designation,
                              item.ExEmployAddress1,
                              item.ExEmployAddress2,
                              item.ExEmployAddress3,
                              item.ExEmployPlace,
                              item.ExEmployPin,
                              item.DateOfJoinning,
                              item.DateOfSeverance,
                              item.ContactPerson,
                              item.ContactNumber,
                              item.StateID,
                              item.CountryID,
                              item.DistrictID
                                );
                        }
                        dbEntity.SaveChanges();
                        if (AddressList != null)
                        {
                            String PartyType = "Employee";
                            foreach (var item in AddressList)
                            {
                                dbEntity.SpCreateAddress(PartyType,
                                    employeeBo.ID,
                                    item.AddressLine1,
                                    item.AddressLine2,
                                    item.AddressLine3,
                                    0,
                                    item.ContactPerson,
                                    item.Place,
                                    item.DistrictID,
                                    item.StateID,
                                    item.PIN,
                                    item.LandLine1,
                                    item.LandLine2,
                                    item.MobileNo,
                                    item.Fax,
                                    item.Email,
                                    item.IsBilling,
                                    item.IsShipping,
                                    item.IsDefault,
                                    item.IsDefaultShipping,
                                    GeneralBO.CreatedUserID,
                                    DateTime.Now,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID);
                            }
                        }
                    }
                    if (SalaryDetails != null)
                    {
                        foreach (var item in SalaryDetails)
                        {
                            dbEntity.SpCreateSalaryDetails(employeeBo.ID,
                              item.PayType,
                              item.SalaryMonthly,
                              item.SalaryAnnual,
                              item.IsFinancePayRoll,
                              item.IsProductionIncentivePayRoll,
                              item.IsSalesIncentivePayRoll
                                );
                        }
                    }
                    if (FreeMedicineLocationList != null)
                    {
                        foreach (var item in FreeMedicineLocationList)
                        {
                            dbEntity.SpCreateFreeMedicineLocationMapping(employeeBo.ID, item.LocationID, GeneralBO.ApplicationID);
                        }
                    }
                };
                return employeeBo.Code;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public EmployeeBO GetEmployee(int EmployeeID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    return dbEntity.SpGetEmployeeByID(EmployeeID, GeneralBO.ApplicationID).Select(k => new EmployeeBO
                    {
                        Code = k.Code,
                        Title = k.Title,
                        Name = k.Name,
                        DepartmentName = k.DepartmentName,
                        EmplyeeCategory = k.EmplyeeCategory,
                        Designation = k.Designation,
                        DateOfJoining = (DateTime)k.JoiningDate,
                        Gender = k.Gender,
                        MartialStatus = k.MartialStatus,
                        DOB = (DateTime)k.DOB,
                        Qualification1 = k.Qualification1,
                        Qualification2 = k.Qualification2,
                        Qualification3 = k.Qualification3,
                        BloodGroup = k.BloodGroup,
                        NoOfDependent = (int)k.NoOfDependent,
                        NameOfSpouse = k.NameOfSpouse,
                        NameOfGuardian = k.NameOfGuardian,
                        IsExcludeFromPayroll = (bool)k.ExcludeFromPayroll,
                        DateOfConfirmation = (DateTime)k.ConfirmationDate,
                        PayRollCategory = k.PayRollCategory,
                        PayGrade = k.PayGrade,
                        CompanyEmail = k.CompanyEmail,
                        ReportingCode = k.ReportingToCode,
                        ReportingName = k.ReportingToName,
                        LocationID = (int)k.DefaultLocationID,
                        Location = k.Location,
                        InterCompany = k.InterCompany,
                        DateOfSeverance = (DateTime)k.DateOfSeverance,
                        DateOfReJoining = (DateTime)k.DateOfRejoin,
                        ProbationDuration = k.ProbationDuration,
                        EmploymentJobType = k.EmploymentJobType,
                        PrintPayroll = k.PrintPayroll,
                        PFStatus = (bool)k.PFStatus,
                        ESIStatus = (bool)k.ESIStatus,
                        NPSStatus = (bool)k.NPSStatus,
                        MedicalInsuranceStatus = (bool)k.MedicalInsuranceStatus,
                        AttandancePunchingStatus = (bool)k.AttandancePunchingStatus,
                        MultiLocationPunchingStatus = (bool)k.MultiLocationPunchingStatus,
                        SpecialLeaveStatus = (bool)k.SpecialLeaveStatus,
                        ProbationStatus = (bool)k.ProbationStatus,
                        ProductionIncentiveStatus = (bool)k.ProductionIncentiveStatus,
                        SalesIncentiveStatus = (bool)k.SalesIncentiveStatus,
                        FixedIncentiveStatus = (bool)k.FixedIncentiveStatus,
                        MinimumWagesStatus = (bool)k.MinimumWagesStatus,
                        IsERPUser = (bool)k.IsERPUser,
                        MedicalAidStatus = (bool)k.MedicalAidStatus,
                        BonusStatus = (bool)k.BonusStatus,
                        ProfessionalTaxStatus = (bool)k.ProfessionalTaxStatus,
                        WelfareDeductionStatus = (bool)k.WelfareDeductionStatus,
                        PanNo = k.PanNo,
                        AadhaarNo = k.AadhaarNo,
                        AccountNumber = k.AccountNumber,
                        BankName = k.BankName,
                        BankBranchName = k.BankBranchName,
                        IFSC = k.IFSC,
                        IsEnglish = (bool)k.IsEnglish,
                        IsHindi = (bool)k.IsHindi,
                        IsKannada = (bool)k.IsKannada,
                        IsMalayalam = (bool)k.IsMalayalam,
                        IsTamil = (bool)k.IsTamil,
                        IsTelugu = (bool)k.IsTelugu,
                        PFVoluntaryContribution = k.PFVoluntaryContribution,
                        PFAccountNo = k.PFAccountNo,
                        PFUAN = k.PFUAN,
                        ESINo = k.ESINo,
                        DepartmentID = (int)k.DepartmentID,
                        DesignationID = (int)k.DesignationID,
                        PayrollCategoryID = (int)k.PayrollCategoryID,
                        InterCompanyID = (int)k.InterCompanyID,
                        EmploymentJobTypeID = (int)k.EmploymentJobTypeID,
                        BloodGroupID = (int)k.BloodGroupID,
                        EmpCategoryID = (int)k.EmployeeCategoryID,
                        UserID = (int)k.UserID,
                        ASPNetUserID=(int)k.ASPNetUserID,
                        UserName=k.UserName
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ExEmployDetailBO> GetExEmployDetails(int EmployeeID)
        {
            List<ExEmployDetailBO> item = new List<ExEmployDetailBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetExEmployDetails(EmployeeID).Select(k => new ExEmployDetailBO
                    {
                        EmployerName = k.EmployerName,
                        Designation = k.Designation,
                        ExEmployAddress1 = k.AddressLine1,
                        ExEmployAddress2 = k.AddressLine2,
                        ExEmployAddress3 = k.AddressLine3,
                        ExEmployPin = k.Pin,
                        ExEmployPlace = k.Place,
                        DateOfJoinning = (DateTime)k.DateOfJoinning,
                        DateOfSeverance = (DateTime)k.DateOfSeverance,
                        ContactPerson = k.ContactPerson,
                        ContactNumber = k.ContactNumber,
                        State = k.State,
                        Country = k.Country,
                        District = k.District,
                        DistrictID = (int)k.DistrictID,
                        StateID = (int)k.StateID,
                        CountryID = (int)k.CountryID
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalaryDetailsBO> GetSalaryDetails(int EmployeeID)
        {
            List<SalaryDetailsBO> item = new List<SalaryDetailsBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetSalaryDetails(EmployeeID).Select(k => new SalaryDetailsBO
                    {
                        PayType = k.Name,
                        SalaryAnnual = (decimal)k.SalaryAnnual,
                        SalaryMonthly = (decimal)k.SalaryMonthly,
                        IsFinancePayRoll = (bool)k.FinancePayRoll,
                        IsProductionIncentivePayRoll = (bool)k.ProductionIncentivePayroll,
                        IsSalesIncentivePayRoll = (bool)k.IncentivePayroll,
                        PaytypeStatus = (bool)k.Paytypestatus
                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeBO> GetAspNetUsersList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetAspNetUsers().Select(Employee => new EmployeeBO
                {
                    ID = Employee.ID,
                    Name = Employee.UserName

                }).ToList();
            }
        }

        public List<EmployeeBO> GetUserLocationList(int ID)
        {
            List<EmployeeBO> UserLocationList = new List<EmployeeBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    UserLocationList = dbEntity.SpGetUserLocationList(ID).Select(a => new EmployeeBO
                    {
                        UserLocationID = a.LocationID,
                        UserLocationName = a.Name
                    }).ToList();
                    return UserLocationList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeBO> GetEmployeeByDepartment(int? DepartmentID)
        {
            List<EmployeeBO> EmployeeList = new List<EmployeeBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    EmployeeList = dbEntity.SpGetEmployeeByDepartment(DepartmentID, GeneralBO.LocationID).Select(a => new EmployeeBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }).ToList();
                    return EmployeeList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateUserID(string SerialNo, int UserID)
        {
            using (MasterEntities entity = new MasterEntities())
            {
                return entity.SpUpdateEmployeeUserID(SerialNo, UserID);
            }
        }
        public DatatableResultBO GetEmployeeListForUserRole(string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetEmployeeListForUserRoles(CodeHint, NameHint, DepartmentHint, LocationHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.EmployeeName,
                                Location = item.Location,
                                Department = item.Department,
                                UserID = item.UserID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetFreeMedicineEmployeeList(string CodeHint, string NameHint, string DepartmentHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetEmployeeListForFreeMedicine(CodeHint, NameHint, DepartmentHint, LocationHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.EmployeeName,
                                Location = item.Location,
                                Department = item.Department,
                                UserID = item.UserID,
                                BalAmount = item.BalanceAmount,
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public List<EmployeeBO> GetDoctor()
        {
            List<EmployeeBO> DoctorList = new List<EmployeeBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    DoctorList = dbEntity.SpGetEmployeeDoctorList().Select(a => new EmployeeBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }).ToList();
                    return DoctorList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeBO GetDepartmentID()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDepartmentID().Select(a => new EmployeeBO()
                    {
                       DepartmentID=(int)a.DepartmentID,
                       DesignationID=(int)a.DesignationID
                    }
                    ).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DatatableResultBO GetTherapistAutoComplete(string Hint)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetAllTherapistList(Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.Name

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DatatableResult;
        }

    }
}
