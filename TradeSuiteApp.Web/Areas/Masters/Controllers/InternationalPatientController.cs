using BusinessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessObject;
using TradeSuiteApp.Web.Utils;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Areas.AHCMS.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class InternationalPatientController : Controller
    {
        private IStateContract stateBL;
        private ICountryContract countryBL;
        private IOutPatientContract outPatientBL;
        private IGeneralContract generalBL;
        private IDistrictContract districtBL;
        private ISubmasterContract submasterBL;
        private IInternationalPatientContract patientBL;
        private IFileContract fileBL;
        private ICategoryContract categoryBL;
        private IPatientDiagnosisContract managePatientBL;

        public InternationalPatientController()
        {
            stateBL = new StateBL();
            countryBL = new CountryBL();
            outPatientBL = new OutPatientBL();
            generalBL = new GeneralBL();
            districtBL = new DistrictBL();
            submasterBL = new SubmasterBL();
            patientBL = new InternationalPatientBL();
            fileBL = new FileBL();
            categoryBL = new CategoryBL();
            managePatientBL = new PatientDiagnosisBL();
        }
        // GET: Masters/InternationalPatient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            OutPatientModel Model = new OutPatientModel();
            Model.Code = generalBL.GetSerialNo("Patients", "Code");
            Model.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            Model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            Model.BloodGroupList = new SelectList(submasterBL.GetBloodGroupList(), "ID", "Name");
            Model.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male", },
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                    new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            Model.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single", },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 new SelectListItem { Text = "Widower", Value = "Widower"},
                                                 new SelectListItem { Text = "Divorced", Value = "Divorced"},
                                                 }, "Value", "Text");
            Model.PatientReferedList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            Model.EmployedInIndia = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Yes", Value ="Yes", },
                                                 new SelectListItem { Text = "No", Value = "No"},
                                                 }, "Value", "Text");
            Model.OccupationList = new SelectList(submasterBL.GetOccupationList(), "ID", "Name");
            Model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            Model.DiscountTypeList = new SelectList(submasterBL.GetGeneralDiscountType(), "ID", "Name");
            Model.DiscountPercentageList = categoryBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            return View(Model);
        }

        public ActionResult Save(OutPatientModel model)
        {
            {
                try
                {
                    CustomerBO CustomerBO = new CustomerBO()
                    {
                        ID = model.ID,
                        Name = model.Name,
                        Code = model.Code,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        StateID = model.StateID,
                        DistrictID = model.DistrictID,
                        CountryID = model.CountryID,
                        MobileNumber = model.MobileNumber,
                        EmailID = model.Email,
                        Category = model.Category,
                        PinCode = model.PinCode,
                        GuardianName = model.GuardianName,
                        Gender = model.Gender,
                        MartialStatus = model.MartialStatus,
                        BloodGroup = model.BloodGroup,
                        OccupationID = model.OccupationID,
                        PatientReferedByID = model.PatientReferedByID,
                        ReferalContactNo = model.ReferalContactNo,
                        PurposeOfVisit = model.PurposeOfVisit,
                        PassportNo = model.PassportNo,
                        PlaceOfIssue = model.PlaceOfIssue,
                        VisaNo = model.VisaNo,
                        ArrivedFrom = model.ArrivedFrom,
                        ProceedingTo = model.ProceedingTo,
                        DurationOfStay = model.DurationOfStay,
                        PhotoID = model.PhotoID,
                        PassportID = model.PassportID,
                        VisaID = model.VisaID,
                        EmployedIn = model.EmployedIn,
                        DoctorID = model.DoctorID,
                        Age = model.Age,
                        LandLine = model.LandLine,
                        Place = model.Place,
                        Month=model.Month,
                        DiscountTypeID=model.DiscountTypeID,
                        ReferalName = model.ReferalName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        CountryCode = model.CountryCode,
                        OtherQuotationIDS = model.OtherQuotationIDS,
                        EmergencyContactNo = model.EmergencyContactNo

                    };
                    if (model.DOB != null)
                    {
                        CustomerBO.DOB = General.ToDateTime(model.DOB);
                    }
                    if (model.DateOfArrival != null)
                    {
                        CustomerBO.DateOfArrival = General.ToDateTime(model.DateOfArrival);
                    }
                    if (model.DiscountTypeID> 0)
                    {
                        CustomerBO.DiscountEndDate = General.ToDateTime(model.DiscountEndDate);
                        CustomerBO.DiscountStartDate = General.ToDateTime(model.DiscountStartDate);
                        CustomerBO.MaxDisccountAmount = model.MaxDisccountAmount;
                    }
                   
                    if (model.DateOfIssuePassport != null)
                    {
                        CustomerBO.DateOfIssuePassport = General.ToDateTime(model.DateOfIssuePassport);
                    }
                    if (model.DateOfExpiry != null)
                    {
                        CustomerBO.DateOfExpiry = General.ToDateTime(model.DateOfExpiry);
                    }
                    if (model.DateOfIssueVisa != null)
                    {
                        CustomerBO.DateOfIssueVisa = General.ToDateTime(model.DateOfIssueVisa);
                    }
                    if (model.DateOfExpiryVisa != null)
                    {
                        CustomerBO.DateOfExpiryVisa = General.ToDateTime(model.DateOfExpiryVisa);
                    }
                    List<DiscountBO> DiscountDetails = new List<DiscountBO>();
                    if (model.DiscountTypeID>0)
                    {
                        DiscountBO DiscountDetail;
                        foreach (var item in model.DiscountDetails)
                        {
                            DiscountDetail = new DiscountBO()
                            {
                                BusinessCategoryID = item.BusinessCategoryID,
                                DiscountCategoryID = item.DiscountCategoryID,
                                DiscountPercentage = item.DiscountPercentage
                            };
                            DiscountDetails.Add(DiscountDetail);
                        }
                    }
                    int ID=patientBL.Save(CustomerBO, DiscountDetails);
                    return Json(new { Status = "success",data = ID }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult GetInternationalPatientList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string PlaceHint = Datatable.Columns[4].Search.Value;
                string DistrictHint = Datatable.Columns[5].Search.Value;
                string DoctorHint = Datatable.Columns[6].Search.Value;
                string PhoneHint= Datatable.Columns[7].Search.Value;
                string LastVisitDateHint = Datatable.Columns[8].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = patientBL.GetInternationalPatientList("",CodeHint, NameHint, PlaceHint, DistrictHint, DoctorHint, PhoneHint, LastVisitDateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "InternationalPatient", "GetInternationalPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            OutPatientModel Patient = patientBL.GetInternationalPatientDetails(ID).Select(a => new OutPatientModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                State = a.State,
                District = a.District,
                MobileNumber = a.MobileNumber,
                Email = a.Email,
                DistrictID = (int)a.DistrictID,
                StateID = (int)a.StateID,
                DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                PinCode = a.PinCode,
                DoctorID = (int)a.DoctorID,
                DoctorName = a.DoctorName,
                GuardianName = a.GuardianName,
                Gender = a.Gender,
                MartialStatus = a.MartialStatus,
                BloodGroup = a.BloodGroup,
                Age = (int)a.Age,
                CountryID = (int)a.CountryID,
                Country = a.Country,
                Occupation = a.Occupation,
                PatientReferedBy = a.PatientReferedBy,
                PatientReferedByID = a.PatientReferedByID,
                ReferalContactNo = a.ReferalContactNo,
                PurposeOfVisit = a.PurposeOfVisit,
                PassportNo = a.PassportNo,
                PlaceOfIssue = a.PlaceOfIssue,
                DateOfIssuePassport = General.FormatDate(a.DateOfIssuePassport),
                DateOfExpiry = General.FormatDate(a.DateOfExpiry),
                EmployedIn = a.EmployedIn,
                VisaNo = a.VisaNo,
                DateOfIssueVisa = General.FormatDate(a.DateOfIssueVisa),
                DateOfExpiryVisa = General.FormatDate(a.DateOfExpiryVisa),
                ArrivedFrom = a.ArrivedFrom,
                ProceedingTo = a.ProceedingTo,
                DurationOfStay = a.DurationOfStay,
                DateOfArrival = General.FormatDate(a.DateOfArrival),
                PhotoID = a.PhotoID,
                PassportID = a.PassportID,
                VisaID = a.VisaID,
                LandLine = a.LandLine,
                Place=a.Place,
                Month=a.Month,
                ReferalName = a.ReferalName,
                MiddleName = a.MiddleName,
                LastName = a.LastName,
                CountryCode = a.CountryCode,
                OtherQuotationIDS = a.OtherQuotationIDS,
                EmergencyContactNo = a.EmergencyContactNo
            }).First();
            Patient.SelectedPhoto = fileBL.GetAttachments(Patient.PhotoID.ToString());
            Patient.SelectedPassport = fileBL.GetAttachments(Patient.PassportID.ToString());
            Patient.SelectedVisa = fileBL.GetAttachments(Patient.VisaID.ToString());
            Patient.OtherQuotation = fileBL.GetAttachments(Patient.OtherQuotationIDS);
            return View(Patient);
        }

        public ActionResult Edit(int ID)
        {
            OutPatientModel Patient = patientBL.GetInternationalPatientDetails(ID).Select(a => new OutPatientModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                State = a.State,
                District = a.District,
                MobileNumber = a.MobileNumber,
                Email = a.Email,
                DistrictID = (int)a.DistrictID,
                StateID = (int)a.StateID,
                //DOB = General.FormatDate(a.DOB),
                DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                PinCode = a.PinCode,
                DoctorID = (int)a.DoctorID,
                DoctorName = a.DoctorName,
                GuardianName = a.GuardianName,
                Gender = a.Gender,
                MartialStatus = a.MartialStatus,
                BloodGroup = a.BloodGroup,
                Age = (int)a.Age,
                CountryID = (int)a.CountryID,
                Country = a.Country,
                OccupationID = a.OccupationID,
                PatientReferedBy = a.PatientReferedBy,
                PatientReferedByID = a.PatientReferedByID,
                ReferalContactNo = a.ReferalContactNo,
                PurposeOfVisit = a.PurposeOfVisit,
                PassportNo = a.PassportNo,
                PlaceOfIssue = a.PlaceOfIssue,
                DateOfIssuePassport = General.FormatDate(a.DateOfIssuePassport),
                DateOfExpiry = General.FormatDate(a.DateOfExpiry),
                EmployedIn = a.EmployedIn,
                VisaNo = a.VisaNo,
                DateOfIssueVisa = General.FormatDate(a.DateOfIssueVisa),
                DateOfExpiryVisa = General.FormatDate(a.DateOfExpiryVisa),
                ArrivedFrom = a.ArrivedFrom,
                ProceedingTo = a.ProceedingTo,
                DurationOfStay = a.DurationOfStay,
                DateOfArrival = General.FormatDate(a.DateOfArrival),
                PhotoID = a.PhotoID,
                PassportID = a.PassportID,
                VisaID = a.VisaID,
                LandLine = a.LandLine,
                Place=a.Place,
                Month=a.Month,
                ReferalName = a.ReferalName,
                MiddleName = a.MiddleName,
                LastName = a.LastName,
                CountryCode = a.CountryCode,
                OtherQuotationIDS = a.OtherQuotationIDS,
                EmergencyContactNo = a.EmergencyContactNo
            }).First();
            Patient.SelectedPhoto = fileBL.GetAttachments(Patient.PhotoID.ToString());
            Patient.OtherQuotation = fileBL.GetAttachments(Patient.OtherQuotationIDS);
            Patient.SelectedPassport = fileBL.GetAttachments(Patient.PassportID.ToString());
            Patient.SelectedVisa = fileBL.GetAttachments(Patient.VisaID.ToString());
            Patient.Code = generalBL.GetSerialNo("Patients", "Code");
            Patient.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            Patient.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            Patient.BloodGroupList = new SelectList(submasterBL.GetBloodGroupList(), "ID", "Name");
            Patient.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male", },
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                    new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            Patient.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single", },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 }, "Value", "Text");
            Patient.PatientReferedList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            Patient.EmployedInIndia = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Yes", Value ="Yes", },
                                                 new SelectListItem { Text = "No", Value = "No"},
                                                 }, "Value", "Text");
            Patient.OccupationList = new SelectList(submasterBL.GetOccupationList(), "ID", "Name");
            Patient.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            Patient.DiscountTypeList = new SelectList(submasterBL.GetGeneralDiscountType(), "ID", "Name");
            Patient.DiscountPercentageList = categoryBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            return View(Patient);
        }

        public JsonResult GetPatientAutoComplete(string Hint)
        {
            DatatableResultBO resultBO = patientBL.GetInternationalPatientList("","", Hint,"","","","","","Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInternationalPatientListView(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string PhoneHint = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = patientBL.GetInternationalPatientList(Type,CodeHint, NameHint, "", "", "", PhoneHint, "", SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "InternationalPatient", "GetInternationalPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInPatientList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string PatientHint = Datatable.Columns[3].Search.Value;
                string InPatientNoHint = Datatable.Columns[4].Search.Value;
                string RoomNameHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = patientBL.GetInPatientList(PatientHint, CodeHint, InPatientNoHint, RoomNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "InternationalPatient", "GetInPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInPatientListAutoComplete(string Hint)
        {
            DatatableResultBO resultBO = patientBL.GetInPatientList(Hint,"", "", "", "Patient", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAppointmentScheduledPatientList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string OPnoHint = Datatable.Columns[4].Search.Value;
                string PhoneHint = Datatable.Columns[6].Search.Value;


                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = patientBL.GetAppointmentScheduledPatientList(CodeHint, NameHint, OPnoHint, PhoneHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "InternationalPatient", "GetAppointmentScheduledPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPatientDiscount(int PatientID = 0)
        {
            try
            {
                CustomerBO Items = patientBL.GetPatientDiscount(PatientID);
                return Json(new { Status = "success", Items = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Master", "InternationalPatient", "GetPatientDiscount", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetInternationalPatientDetails(int ID = 0)
        {
            try
            {
                OutPatientModel Patient = patientBL.GetInternationalPatientDetails(ID).Select(a => new OutPatientModel()
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                    State = a.State,
                    District = a.District,
                    MobileNumber = a.MobileNumber,
                    Email = a.Email,
                    DistrictID = (int)a.DistrictID,
                    StateID = (int)a.StateID,
                    DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                    PinCode = a.PinCode,
                    DoctorID = (int)a.DoctorID,
                    DoctorName = a.DoctorName,
                    GuardianName = a.GuardianName,
                    Gender = a.Gender,
                    MartialStatus = a.MartialStatus,
                    BloodGroup = a.BloodGroup,
                    Age = (int)a.Age,
                    CountryID = (int)a.CountryID,
                    Country = a.Country,
                    Occupation = a.Occupation,
                    PatientReferedBy = a.PatientReferedBy,
                    PatientReferedByID = a.PatientReferedByID,
                    ReferalContactNo = a.ReferalContactNo,
                    PurposeOfVisit = a.PurposeOfVisit,
                    PassportNo = a.PassportNo,
                    PlaceOfIssue = a.PlaceOfIssue,
                    DateOfIssuePassport = General.FormatDate(a.DateOfIssuePassport),
                    DateOfExpiry = General.FormatDate(a.DateOfExpiry),
                    EmployedIn = a.EmployedIn,
                    VisaNo = a.VisaNo,
                    DateOfIssueVisa = General.FormatDate(a.DateOfIssueVisa),
                    DateOfExpiryVisa = General.FormatDate(a.DateOfExpiryVisa),
                    ArrivedFrom = a.ArrivedFrom,
                    ProceedingTo = a.ProceedingTo,
                    DurationOfStay = a.DurationOfStay,
                    DateOfArrival = General.FormatDate(a.DateOfArrival),
                    PhotoID = a.PhotoID,
                    PassportID = a.PassportID,
                    VisaID = a.VisaID,
                    LandLine = a.LandLine,
                    Place = a.Place,
                    Month = a.Month,
                    ReferalName = a.ReferalName,
                    MiddleName = a.MiddleName,
                    LastName = a.LastName,
                    CountryCode = a.CountryCode,
                    OtherQuotationIDS = a.OtherQuotationIDS,
                    EmergencyContactNo = a.EmergencyContactNo
                }).First();
                return Json(new { Status = "success", Items = Patient }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Master", "InternationalPatient", "GetInternationalPatientDetails", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult PatientHistory(int ID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model = patientBL.GetInternationalPatientDetails(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.ID,
                PatientCode = a.Code,
                PatientName = a.Name,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                State = a.State,
                District = a.District,
                MobileNumber = a.MobileNumber,
                Email = a.Email,
                DistrictID = (int)a.DistrictID,
                StateID = (int)a.StateID,
                DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                PinCode = a.PinCode,
                DoctorName = a.DoctorName,
                GuardianName = a.GuardianName,
                Gender = a.Gender,
                MartialStatus = a.MartialStatus,
                BloodGroup = a.BloodGroup,
                Age = (int)a.Age,
                CountryID = (int)a.CountryID,
                Country = a.Country,
                Occupation = a.Occupation,
                PatientReferedBy = a.PatientReferedBy,
                PatientReferedByID = a.PatientReferedByID,
                ReferalContactNo = a.ReferalContactNo,
                PurposeOfVisit = a.PurposeOfVisit,
                PassportNo = a.PassportNo,
                PlaceOfIssue = a.PlaceOfIssue,
                DateOfIssuePassport = General.FormatDate(a.DateOfIssuePassport),
                DateOfExpiry = General.FormatDate(a.DateOfExpiry),
                EmployedIn = a.EmployedIn,
                VisaNo = a.VisaNo,
                DateOfIssueVisa = General.FormatDate(a.DateOfIssueVisa),
                DateOfExpiryVisa = General.FormatDate(a.DateOfExpiryVisa),
                ArrivedFrom = a.ArrivedFrom,
                ProceedingTo = a.ProceedingTo,
                DurationOfStay = a.DurationOfStay,
                DateOfArrival = General.FormatDate(a.DateOfArrival),
                PhotoID = a.PhotoID,
                PassportID = a.PassportID,
                VisaID = a.VisaID,
                LandLine = a.LandLine,
                Place = a.Place,
                Month = a.Month,
                ReferalName = a.ReferalName,
                MiddleName = a.MiddleName,
                LastName = a.LastName,
                CountryCode = a.CountryCode,
                OtherQuotationIDS = a.OtherQuotationIDS,
                EmergencyContactNo = a.EmergencyContactNo
            }).First();
            model.History = managePatientBL.GetHistoryListByID(0, ID).Select(a => new HistoryModel()
            {
                ParentID = a.ParentID,
                AppointmentProcessID = a.AppointmentProcessID,
                IPID = (int)a.IPID,
                PatientID = a.PatientID,
                AppointmentType = a.AppointmentType,
                Disease = a.Disease,
                ReportedDate = a.ReportedDate,
                CaseSheet = a.CaseSheet,
                Remarks = a.Remarks,
                Doctor = a.Doctor,
                SuggestedReviewDate = a.SuggestedReviewDate == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.SuggestedReviewDate),
                Patient = a.Patient,
                TransNo = a.TransNo,
                DischargedDate = a.DischargedDate == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DischargedDate),
                PresentingComplaints = a.PresentingComplaints,
                Associatedcomplaints = a.Associatedcomplaints,
                AyurvedicDiagnosis = a.AyurvedicDiagnosis,
                ContemporaryDiagnosis = a.ContemporaryDiagnosis
            }).ToList();
            return View(model);
        }
    }
}