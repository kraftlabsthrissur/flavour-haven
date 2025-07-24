using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.AHCMS.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.AHCMS.Controllers
{
    public class AppointmentScheduleController : Controller
    {

        private IDistrictContract districtBL;
        private IStateContract stateBL;
        private ICountryContract countryBL;
        private IGeneralContract generalBL;
        private IAppointmentScheduleContract appointmentBL;
        private ISubmasterContract submasterBL;
        private IPaymentTypeContract paymentTypeBL;
        private ITreasuryContract treasuryBL;
        private IDepartmentContract departmentBL;
        private ICategoryContract categoryBL;
        private IConsultationScheduleContract scheduleBL;
        public AppointmentScheduleController()
        {
            stateBL = new StateBL();
            countryBL = new CountryBL();
            districtBL = new DistrictBL();
            generalBL = new GeneralBL();
            appointmentBL = new AppointmentScheduleBL();
            submasterBL = new SubmasterBL();
            paymentTypeBL = new PaymentTypeBL();
            treasuryBL = new TreasuryBL();
            departmentBL = new DepartmentBL();
            categoryBL = new CategoryBL();
        }

        // GET: AHCMS/AppointmentSchedule
        public ActionResult Index()
        {
            AppointmentScheduleModel Model = new AppointmentScheduleModel();
            Model.day1 = General.FormatDate(DateTime.Now);
            Model.day2 = General.FormatDate(DateTime.Now.AddDays(1));
            Model.day3 = General.FormatDate(DateTime.Now.AddDays(2));
            Model.day4 = General.FormatDate(DateTime.Now.AddDays(3));
            Model.day5 = General.FormatDate(DateTime.Now.AddDays(4));
            Model.day6 = General.FormatDate(DateTime.Now.AddDays(5));
            Model.day7 = General.FormatDate(DateTime.Now.AddDays(6));
            return View(Model);
        }
        // GET: AHCMS/AppointmentSchedule/IndexV2 - For triveni
        public ActionResult IndexV2()
        {
            AppointmentScheduleModel Model = new AppointmentScheduleModel();
            Model.day1 = General.FormatDate(DateTime.Now);
            Model.day2 = General.FormatDate(DateTime.Now.AddDays(1));
            Model.day3 = General.FormatDate(DateTime.Now.AddDays(2));
            Model.day4 = General.FormatDate(DateTime.Now.AddDays(3));
            Model.day5 = General.FormatDate(DateTime.Now.AddDays(4));
            Model.day6 = General.FormatDate(DateTime.Now.AddDays(5));
            Model.day7 = General.FormatDate(DateTime.Now.AddDays(6));
            return View(Model);
        }
        // GET: AHCMS/AppointmentSchedule/IndexV3 - For EshCoimbatore
        public ActionResult IndexV3()
        {
            AppointmentScheduleModel Model = new AppointmentScheduleModel();
            Model.day1 = General.FormatDate(DateTime.Now);
            Model.day2 = General.FormatDate(DateTime.Now.AddDays(1));
            Model.day3 = General.FormatDate(DateTime.Now.AddDays(2));
            Model.day4 = General.FormatDate(DateTime.Now.AddDays(3));
            Model.day5 = General.FormatDate(DateTime.Now.AddDays(4));
            Model.day6 = General.FormatDate(DateTime.Now.AddDays(5));
            Model.day7 = General.FormatDate(DateTime.Now.AddDays(6));
            return View(Model);
        }
        public ActionResult Create(int DoctorID = 0, int PatientID = 0)
        {
            AppointmentScheduleModel model = new AppointmentScheduleModel();
            model.Code = generalBL.GetSerialNo("Patients", "Code");
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.FromDate = DateTime.Now;
            model.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            model.BloodGroupList = new SelectList(submasterBL.GetBloodGroupList(), "ID", "Name");
            model.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male", },
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                    new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            model.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single", },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 new SelectListItem { Text = "Widower", Value = "Widower"},
                                                 new SelectListItem { Text = "Divorced", Value = "Divorced"},
                                                 }, "Value", "Text");
            model.PatientReferedList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            model.EmployedInIndia = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Yes", Value ="Yes", },
                                                 new SelectListItem { Text = "No", Value = "No"},
                                                 }, "Value", "Text");

            model.OccupationList = new SelectList(submasterBL.GetOccupationList(), "ID", "Name");
            model.ReferenceThroughList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            model.DiscountTypeList = new SelectList(submasterBL.GetGeneralDiscountType(), "ID", "Name");
            model.DepartmentID = 1;
            model.DiscountPercentageList = categoryBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            model.IsAllowConsultationSchedule = Convert.ToInt16(generalBL.GetConfig("AllowConsultationSchedule"));
            model.IsReferenceThroughRequired = Convert.ToInt16(generalBL.GetConfig("IsReferenceThroughRequired"));
            model.DoctorID = DoctorID;
            model.PatientID = PatientID;
            return View(model);
        }

        //CreateV2-For Thriveni
        public ActionResult CreateV2()
        {
            AppointmentScheduleModel model = new AppointmentScheduleModel();
            model.Code = generalBL.GetSerialNo("Patients", "Code");
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.FromDate = DateTime.Now;
            model.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            model.BloodGroupList = new SelectList(submasterBL.GetBloodGroupList(), "ID", "Name");
            model.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male", },
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                    new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            model.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single", },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 new SelectListItem { Text = "Widower", Value = "Widower"},
                                                 new SelectListItem { Text = "Divorced", Value = "Divorced"},
                                                 }, "Value", "Text");
            model.PatientReferedList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            model.EmployedInIndia = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Yes", Value ="Yes", },
                                                 new SelectListItem { Text = "No", Value = "No"},
                                                 }, "Value", "Text");
            model.OccupationList = new SelectList(submasterBL.GetOccupationList(), "ID", "Name");
            model.ReferenceThroughList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            model.DiscountTypeList = new SelectList(submasterBL.GetGeneralDiscountType(), "ID", "Name");
            model.DiscountPercentageList = categoryBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            model.IsAllowConsultationSchedule = Convert.ToInt16(generalBL.GetConfig("AllowConsultationSchedule"));
            model.IsReferenceThroughRequired = Convert.ToInt16(generalBL.GetConfig("IsReferenceThroughRequired"));
            return View(model);
        }

        //CreateV3-For EshCoimbatore
        public ActionResult CreateV3()
        {
            AppointmentScheduleModel model = new AppointmentScheduleModel();
            model.Code = generalBL.GetSerialNo("Patients", "Code");
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.FromDate = DateTime.Now;
            model.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            model.BloodGroupList = new SelectList(submasterBL.GetBloodGroupList(), "ID", "Name");
            model.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male", },
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                    new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            model.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single", },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 new SelectListItem { Text = "Widower", Value = "Widower"},
                                                 new SelectListItem { Text = "Divorced", Value = "Divorced"},
                                                 }, "Value", "Text");
            model.PatientReferedList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            model.EmployedInIndia = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Yes", Value ="Yes", },
                                                 new SelectListItem { Text = "No", Value = "No"},
                                                 }, "Value", "Text");
            model.OccupationList = new SelectList(submasterBL.GetOccupationList(), "ID", "Name");
            model.ReferenceThroughList = new SelectList(submasterBL.GetPatientReferenceBy(), "ID", "Name");
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            model.DiscountTypeList = new SelectList(submasterBL.GetGeneralDiscountType(), "ID", "Name");
            model.DiscountPercentageList = categoryBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            model.IsAllowConsultationSchedule = Convert.ToInt16(generalBL.GetConfig("AllowConsultationSchedule"));
            model.IsReferenceThroughRequired = Convert.ToInt16(generalBL.GetConfig("IsReferenceThroughRequired"));
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(AppointmentScheduleModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                AppointmentScheduleBO appointmentScheduleBO = new AppointmentScheduleBO()
                {
                    DoctorID = model.DoctorID,
                    FromDate = General.ToDateTime(model.FromDateString),
                };
                List<AppointmentScheduleItemBO> Items = new List<AppointmentScheduleItemBO>();
                if (model.Items != null)
                {
                    AppointmentScheduleItemBO Item;

                    foreach (var item in model.Items)
                    {
                        Item = new AppointmentScheduleItemBO()
                        {
                            PatientID = item.PatientID,
                            Time = item.Time,
                            TokenNo = item.TokenNo,
                            AppointmentScheduleItemID = item.AppointmentScheduleItemID,
                            AppointmentProcessID = item.AppointmentProcessID,
                            DepartmentID = item.DepartmentID
                        };
                        Items.Add(Item);
                    }
                }
                appointmentBL.Save(appointmentScheduleBO, Items);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "AppointmentSchedule", "Save", model.ID, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAppointmentItems(int DoctorID, string Date)
        {
            try
            {
                AppointmentScheduleModel Model = new AppointmentScheduleModel();
                DateTime FromDate = General.ToDateTime(Date);
                List<AppointmentScheduleItemBO> scheduleList = appointmentBL.GetAppointmentItems(DoctorID, FromDate);
                AppointmentScheduleItemModel scheduleItemModel;
                Model.Items = new List<AppointmentScheduleItemModel>();
                foreach (var m in scheduleList)
                {
                    scheduleItemModel = new AppointmentScheduleItemModel()
                    {
                        PatientID = m.PatientID,
                        PatientName = m.PatientName,
                        TokenNo = m.TokenNo,
                        Time = m.Time,
                        AppointmentScheduleItemID = m.AppointmentScheduleItemID,
                        AppointmentProcessID = m.AppointmentProcessID,
                        DepartmentID = m.DepartmentID,
                        DepartmentName = m.DepartmentName
                    };
                    Model.Items.Add(scheduleItemModel);
                }
                return Json(new { Status = "success", Data = scheduleList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsDeletable(int DoctorID, int PatientID, string FromDate)
        {
            DateTime Date = General.ToDateTime(FromDate);
            bool IsDeletable = appointmentBL.IsDeletable(DoctorID, PatientID, Date);
            if (IsDeletable)
            {
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetAppointmentScheduleList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = null;
                string DoctorNameHint = Datatable.Columns[1].Search.Value;
                string PatientCodeHint = Datatable.Columns[2].Search.Value;
                string PatientHint = Datatable.Columns[3].Search.Value;
                string TimeHint = Datatable.Columns[5].Search.Value;
                string TokenNoHint = Datatable.Columns[6].Search.Value;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                if (Type == "Past" || Type == "Future")
                {
                    DateHint = Datatable.Columns[7].Search.Value;
                }

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = appointmentBL.GetAppointmentScheduleList(Type, DoctorNameHint, DateHint, PatientCodeHint, PatientHint, TimeHint, TokenNoHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsAppointmentProcessed(int DoctorID, int PatientID)
        {
            bool IsAppointmentProcessed = appointmentBL.IsAppointmentProcessed(DoctorID, PatientID);
            return Json(new { Status = "success", IsAppointmentProcessed = IsAppointmentProcessed }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAppointmentConfirmation(int DoctorID, int PatientID, string Date, int AppointmentScheduleItemID, int BillablesID)
        {
            DateTime date = General.ToDateTime(Date);
            bool Appointment = appointmentBL.GetAppointmentConfirmation(DoctorID, PatientID, date, AppointmentScheduleItemID, BillablesID);
            return Json(new { Status = "success", IsAppointment = Appointment }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAppointment(int AppointmentScheduleItemID)
        {
            var obj = appointmentBL.GetAppointmentFee(AppointmentScheduleItemID);
            AppointmentScheduleModel model = new AppointmentScheduleModel();
            if (obj != null)
            {
                model.BillableID = obj.BillableID;
                model.ItemID = obj.ItemID;
                model.ItemName = obj.ItemName;
                model.Quantity = obj.Quantity;
                model.Rate = obj.Rate;
                model.PatientID = obj.PatientID;
                model.PatientName = obj.Patient;
                model.TokenNo = obj.TokenNo;
                model.Time = obj.Time;
                model.SheduledDate = General.FormatDate(DateTime.Now);
                model.TransNo = generalBL.GetSerialNo("ServiceSalesOrder", "Code");
                //model.NetAmount = obj.Rate;
                model.BankList = new SelectList(treasuryBL.GetBank("Receipt", ""), "ID", "BankName");
                model.PaymentModeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
                model.AppointmentScheduleItemID = AppointmentScheduleItemID;
                model.ConsultationMode = obj.ConsultationMode;
            }
            return PartialView(model);
        }

        public PartialViewResult ConsultationDetails(int AppointmentScheduleItemID)
        {
            AppointmentScheduleModel model = new AppointmentScheduleModel();
            model.ConsulationItem = appointmentBL.GetConsulationItem(AppointmentScheduleItemID).Select(m => new ConsultationModel()
            {
                ItemName = m.ItemName,
                Rate = m.Rate,
                ItemID = m.ItemID
            }).ToList();
            return PartialView(model);
        }

        public ActionResult SaveAppointment(AppointmentScheduleBO model)
        {
            try
            {
                AppointmentScheduleBO Appointment = new AppointmentScheduleBO()
                {
                    TransNo = model.TransNo,
                    PatientID = model.PatientID,
                    BillableID = model.BillableID,
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    PaymentModeID = model.PaymentModeID,
                    BankID = model.BankID,
                    Remarks = model.Remarks,
                    ConsultationMode = model.ConsultationMode,
                    NetAmount = model.NetAmount
                };
                List<ConsultationBO> Items = new List<ConsultationBO>();
                if (model.ConsultationItems != null)
                {
                    ConsultationBO Item;

                    foreach (var item in model.ConsultationItems)
                    {
                        Item = new ConsultationBO()
                        {
                            ItemID = item.ItemID,
                            Rate = item.Rate,
                        };
                        Items.Add(Item);
                    }
                }
                appointmentBL.SaveAppointment(Appointment, Items);
                return Json(new { Status = "Success", Message = "Appointment scheduled successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Appointment Creation failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult CreateAppointmentCancellation(int AppointmentScheduleItemID, int PatientID, string Date)
        {
            DateTime date = General.ToDateTime(Date, "YYYY-MM-DD");
            bool cancel = appointmentBL.CreateAppointmentCancellation(AppointmentScheduleItemID, PatientID, date);
            return Json(new { Status = "success", Iscancel = cancel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAndConfirmAppointment(AppointmentScheduleModel model)
        {
            try
            {
                AppointmentScheduleBO Appointment = new AppointmentScheduleBO()
                {
                    DoctorID = model.DoctorID,
                    FromDate = General.ToDateTime(model.FromDateString),
                    PatientID = model.PatientID,
                    Time = model.Time,
                    TokenNo = model.TokenNo,
                    DepartmentID = model.DepartmentID,
                    BillableID = model.BillableID
                };
                int AppointmentScheduleItemID = appointmentBL.SaveAndConfirmAppointment(Appointment);
                return Json(new { Status = "success", AppointmentScheduleItemID = AppointmentScheduleItemID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Appointment Creation failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteAppointmentScheduleItems(int AppointmentScheduleItemID)
        {
            bool cancel = appointmentBL.DeleteAppointmentScheduleItems(AppointmentScheduleItemID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetPatientDetails(int ID)
        {
            AppointmentScheduleModel model = new AppointmentScheduleModel();
            model.Items = appointmentBL.GetPatientDetails(ID).Select(m => new AppointmentScheduleItemModel()
            {
                PatientName = m.Patient,
                Age = m.Age,
                Code = m.PatientCode,
                Gender = m.Gender
            }).ToList();
            //return PartialView(model);
            return PartialView(model);
        }

        public JsonResult GetPatientForBarCodeGenerator(int ID)
        {
            try
            {
                List<AppointmentScheduleBO> BarCodeItems = appointmentBL.GetPatientForBarCodeGenerator(ID);

                return Json(new { Status = "success", Data = BarCodeItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetPatientForBarCodeGeneratorWithImage(int ID)
        {
            try
            {
                List<AppointmentScheduleBO> BarCodeItems = appointmentBL.GetPatientForBarCodeGeneratorWithImage(ID);

                return Json(new { Status = "success", Data = BarCodeItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public PartialViewResult Edit(DateTime Date, int PatientID = 0, string Patient = "", int DoctorID = 0, string Doctor = "", int DepartmentID = 0, string Department = "", int AppointmentItemID = 0, int BillablesID = 0)
        {
            AppointmentScheduleModel model = new AppointmentScheduleModel();
            model.AppointmentDate = Convert.ToDateTime(Date).ToString("dd-MM-yyyy");

            model.PatientID = PatientID;
            model.PatientName = Patient;
            model.DoctorID = DoctorID;
            model.DoctorName = Doctor;
            model.DepartmentID = DepartmentID;
            model.AppointmentScheduleItemID = AppointmentItemID;
            model.BillableID = BillablesID;
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            return PartialView(model);

        }
        public ActionResult Update(AppointmentScheduleModel model)
        {
            try
            {
                AppointmentScheduleBO Appointment = new AppointmentScheduleBO()
                {
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    DoctorID = model.DoctorID,
                    PatientID = model.PatientID,
                    DepartmentID = model.DepartmentID,
                    AppointmentDate = General.ToDateTime(model.AppointmentDate)
                };
                appointmentBL.UpdateAppointment(Appointment);
                return Json(new { Status = "Success", Message = "Appointment updated successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Appointment updation failed" }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}