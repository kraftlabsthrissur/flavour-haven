using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class InternationalPatientDAL
    {
        public int Save(CustomerBO CustomerBO, List<DiscountBO> DiscountDetails)
        {
            int ID;
            using (MasterEntities dbEntity = new MasterEntities())
            {
               
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        //ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        //dbEntity.SpUpdateSerialNo(
                        //                          "Patients",
                        //                          "Code",
                        //                          GeneralBO.FinYear,
                        //                          GeneralBO.LocationID,
                        //                          GeneralBO.ApplicationID,
                        //                          SerialNo);
                        ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                        ObjectParameter PatientID = new ObjectParameter("PatientID", typeof(int));
                        var i = dbEntity.SpCreateInternationalPatient(
                        "",
                        CustomerBO.Name,
                        CustomerBO.AddressLine1,
                        CustomerBO.AddressLine2,
                        CustomerBO.StateID,
                        CustomerBO.CountryID,
                        CustomerBO.DistrictID,
                        CustomerBO.DOB,
                        CustomerBO.EmailID,
                        CustomerBO.MobileNumber,
                        CustomerBO.PinCode,
                        CustomerBO.GuardianName,
                        CustomerBO.Gender,
                        CustomerBO.MartialStatus,
                        CustomerBO.BloodGroup,
                        CustomerBO.OccupationID,
                        CustomerBO.PatientReferedByID,
                        CustomerBO.ReferalContactNo,
                        CustomerBO.DateOfArrival,
                        CustomerBO.PurposeOfVisit,
                        CustomerBO.PassportNo,
                        CustomerBO.PlaceOfIssue,
                        CustomerBO.DateOfIssuePassport,
                        CustomerBO.DateOfExpiry,
                        CustomerBO.VisaNo,
                        CustomerBO.DateOfIssueVisa,
                        CustomerBO.DateOfExpiryVisa,
                        CustomerBO.ArrivedFrom,
                        CustomerBO.ProceedingTo,
                        CustomerBO.DurationOfStay,
                        CustomerBO.EmployedIn,
                        CustomerBO.DoctorID,
                        CustomerBO.Age,
                        CustomerBO.PhotoID,
                        CustomerBO.PassportID,
                        CustomerBO.VisaID,
                        CustomerBO.LandLine,
                        CustomerBO.Place,
                        CustomerBO.Month,
                        CustomerBO.DiscountTypeID,
                        CustomerBO.ReferalName,
                        CustomerBO.MiddleName,
                        CustomerBO.LastName,
                        CustomerBO.CountryCode,
                        CustomerBO.OtherQuotationIDS,
                        CustomerBO.EmergencyContactNo,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        ReturnValue,
                        PatientID
                     );


               //         dbEntity.SpCreateAccountHeadByType(
               //        "Customer",
               //        Convert.ToInt32(PatientID.Value),
               //        GeneralBO.CreatedUserID,
               //        GeneralBO.ApplicationID,
               //        GeneralBO.LocationID,
               //        GeneralBO.FinYear
               //);
                        if (CustomerBO.DiscountTypeID > 0)
                        {
                            dbEntity.SpCreatePatientDiscountLimit(
                                           Convert.ToInt32(PatientID.Value),
                                           CustomerBO.MaxDisccountAmount,
                                           CustomerBO.DiscountStartDate,
                                           CustomerBO.DiscountEndDate,
                                           GeneralBO.CreatedUserID,
                                           GeneralBO.FinYear,
                                           GeneralBO.LocationID,
                                           GeneralBO.ApplicationID
                                        );

                            foreach (var item in DiscountDetails)
                            {
                                if (item.ID == 0)
                                {
                                    dbEntity.SpCreateDiscount(
                                           item.ItemID,
                                           Convert.ToInt32(PatientID.Value),
                                           item.CustomerCategoryID,
                                           item.CustomerStateID,
                                           item.BusinessCategoryID,
                                           item.SalesIncentiveCategoryID,
                                           item.SalesCategoryID,
                                           item.DiscountCategoryID,
                                           item.DiscountPercentage,
                                           GeneralBO.CreatedUserID,
                                           GeneralBO.LocationID,
                                           GeneralBO.ApplicationID
                                        );
                                }
                                
                            }
                        }                       
                        if (Convert.ToInt16(ReturnValue.Value) == -1)
                        {
                            throw new Exception("Already exists");
                        }
                        transaction.Commit();
                        ID = Convert.ToInt32(PatientID.Value);
                        return ID;
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }

        }

        public DatatableResultBO GetInternationalPatientList(string Type,string CodeHint, string NameHint, string PlaceHint, string DistrictHint, string DoctorHint, string PhoneHint, string LastVisitDateHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetInternationalPatientList(Type,CodeHint, NameHint, PlaceHint, DistrictHint, DoctorHint, PhoneHint, LastVisitDateHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = item.Name,
                                Phone = item.MobileNo,
                                PatientReferedByID = item.PatientReferedBy,
                                PatientReferedBy = item.PatientRefered,
                                ConsultationMode = item.ConsultationMode,
                                Place = item.Place,
                                District = item.District,
                                Doctor = item.Doctor,
                                LastVisitDate = item.LastVisitDate == null ? "" : DateTime.Parse(item.LastVisitDate.ToString()).ToString("dd-MMM-yyyy"),
                                Type = item.Status
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

        public List<CustomerBO> GetInternationalPatientDetails(int ID)
        {
            try
            {
                List<CustomerBO> Patient = new List<CustomerBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Patient = dbEntity.SpGetInternationalPatientByID(ID, GeneralBO.ApplicationID).Select(a => new CustomerBO()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        State = a.State,
                        District = a.District,
                        MobileNumber = a.MobileNo,
                        Email = a.Email,
                        DistrictID = (int)a.DistrictID,
                        StateID = (int)a.StateID,
                        DOB = a.DOB,
                        PinCode = a.PIN,
                        DoctorID = (int)a.DoctorID,
                        DoctorName = a.Doctor,
                        GuardianName = a.NameOfGuardian,
                        Gender = a.Gender,
                        MartialStatus = a.MartialStatus,
                        BloodGroup = a.BloodGroup,
                        Age = (int)a.Age,
                        CountryID = (int)a.CountryID,
                        Country = a.Country,
                        Occupation = a.Occupation,
                        OccupationID = a.OccupationID,
                        PatientReferedBy = a.PatientRefered,
                        PatientReferedByID = (int)a.PatientReferedBy,
                        ReferalContactNo = a.ReferalContactNo,
                        PurposeOfVisit = a.PurposeOfVisit,
                        PassportNo = a.PassportNo,
                        PlaceOfIssue = a.PlaceOfIssue,
                        DateOfIssuePassport = (DateTime)a.DateOfIssuePassport,
                        DateOfExpiry = (DateTime)a.DateOfExpiryPassport,
                        EmployedIn = a.EmployeeInIndia,
                        VisaNo = a.VisaNo,
                        DateOfIssueVisa = (DateTime)a.DateOFIssueVisa,
                        DateOfExpiryVisa = (DateTime)a.DateOfExpiryVisa,
                        ArrivedFrom = a.ArrivedFrom,
                        ProceedingTo = a.ProceedingTo,
                        DurationOfStay = (int)a.DurationOFStay,
                        DateOfArrival = (DateTime)a.DateOfArrival,
                        PhotoID = (int)a.PhotoID,
                        PassportID = (int)a.PassportCopyID,
                        VisaID = (int)a.VisaCopyID,
                        LandLine = a.LandLine1,
                        Place = a.Place,
                        Month = (int)a.Month,
                        ReferalName = a.ReferalName,
                        MiddleName = a.MiddleName,
                        LastName = a.LastName,
                        CountryCode = a.CountryCode,
                        OtherQuotationIDS = a.OtherQuotationIDS,
                        EmergencyContactNo = a.EmergencyContactNo
                    }
                    ).ToList();

                    return Patient;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(CustomerBO CustomerBO, List<DiscountBO> DiscountDetails)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpUpdateInternationalPatient(
                           CustomerBO.ID,
                           CustomerBO.Name,
                           CustomerBO.AddressLine1,
                           CustomerBO.AddressLine2,
                           CustomerBO.StateID,
                           CustomerBO.CountryID,
                           CustomerBO.DistrictID,
                           CustomerBO.DOB,
                           CustomerBO.EmailID,
                           CustomerBO.MobileNumber,
                           CustomerBO.PinCode,
                           CustomerBO.GuardianName,
                           CustomerBO.Gender,
                           CustomerBO.MartialStatus,
                           CustomerBO.BloodGroup,
                           CustomerBO.OccupationID,
                           CustomerBO.PatientReferedByID,
                           CustomerBO.ReferalContactNo,
                           CustomerBO.DateOfArrival,
                           CustomerBO.PurposeOfVisit,
                           CustomerBO.PassportNo,
                           CustomerBO.PlaceOfIssue,
                           CustomerBO.DateOfIssuePassport,
                           CustomerBO.DateOfExpiry,
                           CustomerBO.VisaNo,
                           CustomerBO.DateOfIssueVisa,
                           CustomerBO.DateOfExpiryVisa,
                           CustomerBO.ArrivedFrom,
                           CustomerBO.ProceedingTo,
                           CustomerBO.DurationOfStay,
                           CustomerBO.EmployedIn,
                           CustomerBO.DoctorID,
                           CustomerBO.Age,
                           CustomerBO.PhotoID,
                           CustomerBO.PassportID,
                           CustomerBO.VisaID,
                           CustomerBO.LandLine,
                           CustomerBO.Place,
                           CustomerBO.Month,
                           CustomerBO.DiscountTypeID,
                           CustomerBO.ReferalName,
                           CustomerBO.MiddleName,
                           CustomerBO.LastName,
                           CustomerBO.CountryCode,
                           CustomerBO.OtherQuotationIDS,
                           CustomerBO.EmergencyContactNo,
                           GeneralBO.CreatedUserID,
                           GeneralBO.FinYear,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID
                           );
                    if (CustomerBO.DiscountTypeID > 0)
                    {
                        foreach (var item in DiscountDetails)
                        {
                            if (item.ID == 0)
                            {
                                dbEntity.SpCreateDiscount(
                                       item.ItemID,
                                       CustomerBO.ID,
                                       item.CustomerCategoryID,
                                       item.CustomerStateID,
                                       item.BusinessCategoryID,
                                       item.SalesIncentiveCategoryID,
                                       item.SalesCategoryID,
                                       item.DiscountCategoryID,
                                       item.DiscountPercentage,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.LocationID,
                                       GeneralBO.ApplicationID
                                    );
                            }
                        }
                    }
                    else
                    {
                        dbEntity.SpUpdateDiscountForPatient(
                                       CustomerBO.ID,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.LocationID,
                                       GeneralBO.ApplicationID
                                    );

                    }

                    return 1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public DatatableResultBO GetInPatientList(string PatientName, string PatientCode, string InPatientNo, string RoomName, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetInPatientListForInAndOutRagister(PatientName, PatientCode, InPatientNo, RoomName, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.PatientID,
                                Patient = item.PatientName,
                                Code = item.PatientCode,
                                IPID = item.IPID,
                                IPNO = item.InPatientNo,
                                RoomID = item.RoomID,
                                Room = item.RoomName,
                                AdmissionDate = ((DateTime)item.AdmissionDate).ToString("dd-MMM-yyyy"),
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
        public DatatableResultBO GetAppointmentScheduledPatientList(string CodeHint, string NameHint, string OpnoHint, string PhoneHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpAppoimentScheduledPatientList(CodeHint, NameHint, OpnoHint, PhoneHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).OrderByDescending(X => X.ID).ToList();
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
                                Name = item.Name,
                                Phone = item.MobileNo,
                                AppointmentProcessID = item.AppointmentProcessID,
                                OPNO = item.OPNO,
                                OPDate = item.OPDate == "" ? null : DateTime.Parse(item.OPDate.ToString()).ToString("dd-MMM-yyyy"),
                                Gender = item.Gender,
                                Age = item.Age,
                                Doctor = item.Doctor,
                                IPID = item.IPID
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
        public CustomerBO GetPatientDiscount(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.GetPatientDiscountbyID(ID).Select(a => new CustomerBO()
                    {
                        DiscountID = a.ID,
                        DiscountCategoryID = a.DiscountCategoryID,
                        DiscountPercentage = a.DiscountPercentage
                    }).FirstOrDefault();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
