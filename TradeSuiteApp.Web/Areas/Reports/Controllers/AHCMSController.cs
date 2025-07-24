using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Reports.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Reports.Controllers
{
    public class AHCMSController : BaseReportController
    {
        private IPatientDiagnosisContract managePatientBL;
        private ILabTestContract labTestBL;
        private IXrayContract xrayBL;
        private ReportsEntities RptEntity;
        private IDischargeContract dischargeBL;
        private ISubmasterContract subMasterBL;
        private IAppointmentScheduleContract appointmentScheduleBL;
        private ILaboratoryTestResultContract labresultBL;
        private IEmployeeContract employeeBL;
        private IServiceSalesOrderContract serviceSalesOrderBL;
        private IDepartmentContract departmentBL;
        private IPlacesContract placesBL;
        private IDistrictContract districtBL;
        private SelectList AtoZRange = new SelectList(Enumerable.Range('A', 'Z' - 'A' + 1).Select(c => (char)c).ToList());

        private DateTime StartDate, EndDate, ChequeDate, ReceiptDateFrom, ReceiptDateTo, ReportAsAt;
        // GET: Reports/AHCMS
        public AHCMSController()
        {
            managePatientBL = new PatientDiagnosisBL();
            RptEntity = new ReportsEntities();
            labTestBL = new LabTestBL();
            dischargeBL = new DischargeBL();
            subMasterBL = new SubmasterBL();
            appointmentScheduleBL = new AppointmentScheduleBL();
            labresultBL = new LaboratoryTestResultBL();
            employeeBL = new EmployeeBL();
            serviceSalesOrderBL = new ServiceSalesOrderBL();
            departmentBL = new DepartmentBL();
            placesBL = new PlacesBL();
            districtBL = new DistrictBL();
            xrayBL = new XrayBL();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult MedicinePrescriptionPrintPdf(int ID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.MedicinePrescription);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var MedicinePrescription = managePatientBL.GetPatientDetailsItemsByID((int)ID).Select(a => new SpGetPatientDetailsListByID_Result()
            {
                HIN = a.HIN,
                PatientName = a.PatientName,
                Address = a.Address,
                Age = a.Age,
                Gender = a.Gender,
                MobileNo = a.Mobile,
                Date = (DateTime)a.Date,
                DoctorName = a.Doctor,
                NextVisitedDate = (a.NextVisitDate),
                TransNo = a.TransNo
            }).ToList();
            var MedicinePrescriptionTrans = managePatientBL.GetMedicineListItemsByID(ID).Select(a => new SpGetMedicineListByID_Result()
            {
                MedicineName = a.Medicine,
                Unit = a.Unit,
                Quantity = (decimal)(a.Quantity),
                Instructions = a.Instructions,
                TransID = a.TransID,
                Description = a.Description,
                MorningTime = a.MorningTime,
                NoonTime = a.NoonTime,
                EveningTime = a.EveningTime,
                NightTime = a.NightTime,
                MedicineTime = a.MedicineTime,
                StartDate = a.StartDate,
                EndDate = a.EndDate
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("MedicinePrescriptionPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionPrintPdfDataSet", MedicinePrescription));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "MedicinePrescriptionPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/MedicinePrescription/"), FileName);
            string URL = "/Outputs/MedicinePrescription/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult TreatmentDetailsPrintPdf(int ID)
        {
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.TreatmentDetails);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PatientDetails = managePatientBL.GetPatientDetailsItemsByID((int)ID).Select(a => new SpGetPatientDetailsListByID_Result()
            {
                HIN = a.HIN,
                PatientName = a.PatientName,
                Address = a.Address,
                Age = a.Age,
                Gender = a.Gender,
                MobileNo = a.Mobile,
                Date = (DateTime)a.Date,
                DoctorName = a.Doctor,
                NextVisitedDate = a.NextVisitDate,
                TransNo = a.TransNo
            }).ToList();
            var TreatmentDetails = managePatientBL.GetTreatmentListItemsByID((int)ID).Select(a => new SpGetTreatmentListItemByID_Result()
            {
                TreatmentName = a.Name,
                Therapist = a.TherapistName,
                TreatmentRoom = a.TreatmentRoomName,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                TreatmentNo = a.NoOfTreatment
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("TreatmentDetailsPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentDetailsPrintPdfDataSet", PatientDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentDetailsTransPrintPdfDataSet", TreatmentDetails));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "TreatmentDetailsPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/TreatmentDetails/"), FileName);
            string URL = "/Outputs/TreatmentDetails/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult OPRegister()
        {
            OpRegisterModel model = new OpRegisterModel();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            model.StartDate = General.FormatDate(startDate);
            model.EndDate = General.FormatDate(DateTime.Now);
            return View(model);
        }

        [HttpPost]
        public ActionResult OPRegister(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "OP Register Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            //FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            //ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/OPRegister.rdlc";
            var OPRegister = RptEntity.SpRptOPRegister(
                                    General.ToDateTime(model.StartDate),
                                    General.ToDateTime(model.EndDate),
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("OPRegisterDataSet", OPRegister));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult CentralOpRegister()
        {
            CentralOpRegisterModel model = new CentralOpRegisterModel();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            model.FromDateString = General.FormatDate(startDate);
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            model.DistrictList = new SelectList(districtBL.GetDistrict(), "ID", "Name");
            model.FromPlaceRangeList = AtoZRange;
            model.ToPlaceRangeList = AtoZRange;
            //model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            //model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }


        [HttpPost]
        public ActionResult CentralOpRegister(CentralOpRegisterModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            //FilterParam = new ReportParameter("Filters", model.Filters);
            //UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            if (model.ReportType == "summary")
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/CentralOpRegister.rdlc";
                var CentralOpRegister = RptEntity.SpRptCentralOPRegister(
                                        model.FromDate,
                                        model.ToDate,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                        ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Central OP Register Report");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CentralOpRegisterDataSet", CentralOpRegister));
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, ReportNameParam, FromDateParam, ToDateParam });
            }
            else
            {
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/CentralOpRegisterDetail.rdlc";
                var CentralOpRegister = RptEntity.SpRptCentralOPRegisterDetail(
                                        model.FromDate,
                                        model.ToDate,
                                        model.DoctorID,
                                        model.DepartmentID,
                                        model.Place,
                                        model.DistrictID,
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID
                                        ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Central OP Register Detail Report");
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CentralOpRegisterDetailDataSet", CentralOpRegister));
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, ReportNameParam, FromDateParam, ToDateParam });
            }
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        public JsonResult GetPlacesForAutoComplete(string Hint = "")
        {
            List<PlacesBO> placeList = new List<PlacesBO>();
            placeList = placesBL.GetPatientPlace(Hint).Select(a => new PlacesBO()
            {
                ID = a.ID,
                Place = a.Place
            }).ToList();

            return Json(placeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Treatment()
        {
            TreatmentReportModel model = new TreatmentReportModel();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            model.StartDate = General.FormatDate(startDate);
            model.EndDate = General.FormatDate(DateTime.Now);
            model.TreatmentGroupList = new SelectList(subMasterBL.GetTreatmentGroupList(), "ID", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult Treatment(TreatmentReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName","Treatment Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            TreatmentGroupParam = new ReportParameter("TreatmentGroup", model.TreatmentGroup);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var Rep = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/Treatment.rdlc";
            var Treatment = RptEntity.SpRptTreatment(
                                    model.ReportType,
                                    General.ToDateTime(model.StartDate),
                                    General.ToDateTime(model.EndDate),
                                    model.TreatmentID,
                                    model.TreatmentGroupID,
                                    model.PatientID,
                                    model.TreatmentRoomID,
                                    model.TherapistID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentReportDataSet", Treatment));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, TreatmentGroupParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult MedicineSchedule()
        {
            TreatmentReportModel model = new TreatmentReportModel();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            model.StartDate = General.FormatDate(startDate);
            model.EndDate = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult MedicineSchedule(TreatmentReportModel model)
        {
            var Medicine = RptEntity.SpRptMedicineSchedule(
                                    General.ToDateTime(model.StartDate),
                                    General.ToDateTime(model.EndDate),
                                    model.NursingStationID,
                                  model.PatientID,
                                  model.RoomID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    ).ToList();

            if(model.ReportType == "PatientWise")
            {
                if(model.PatientName != null)
                {
                    var ReportName = "Medicine Schedule Report of Patient : " + model.PatientName;
                    ReportNameParam = new ReportParameter("ReportName", ReportName);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineSchedulePatientWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineSchedulePatientWiseDataSet", Medicine));
                }
                else
                {
                    ReportNameParam = new ReportParameter("ReportName", "Medicine Schedule Report");
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineSchedulePatientWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineSchedulePatientWiseDataSet", Medicine));
                }
            }
            if(model.ReportType == "RoomWise")
            {
                if(model.Room != null)
                {
                    var ReportName = "Medicine Schedule Report of Room  : " + model.Room;
                    ReportNameParam = new ReportParameter("ReportName", ReportName);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineScheduleRoomWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineScheduleRoomWiseDataSet", Medicine));
                }
                else
                {
                    ReportNameParam = new ReportParameter("ReportName", "Medicine Schedule Report");
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineScheduleRoomWise.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineScheduleRoomWiseDataSet", Medicine));
                }
            }
            if(model.ReportType == "NursingStationWise")
            {
                if(model.NursingStation != null)
                {
                    var ReportName = "Medicine Schedule Report of Nursing Station : " + model.NursingStation;
                    ReportNameParam = new ReportParameter("ReportName", ReportName);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineSchedule.rdlc";

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineScheduleDataSet", Medicine));
                }
                else
                {
                    ReportNameParam = new ReportParameter("ReportName", "Medicine Schedule Report");
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineSchedule.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineScheduleDataSet", Medicine));
                }
            }


          //  ReportNameParam = new ReportParameter("ReportName", "Medicine Schedule Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var Rep = new object();
            //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineSchedule.rdlc";
          
           // reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineScheduleDataSet", Medicine));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult TransferQuantity()
        {
            TreatmentReportModel model = new TreatmentReportModel();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            model.StartDate = General.FormatDate(startDate);
            model.EndDate = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult TransferQuantity(TreatmentReportModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Transfer Quantity Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var Rep = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/TransferQuantity.rdlc";
            var Medicine = RptEntity.SpRptTransferQuantity(
                                    General.ToDateTime(model.StartDate),
                                    General.ToDateTime(model.EndDate),
                                    model.NursingStationID,
                                    model.WareHouseToID,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TransferQuantityDataSet", Medicine));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult IPRegister()
        {
            OpRegisterModel model = new OpRegisterModel();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            model.StartDate = General.FormatDate(startDate);
            model.EndDate = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult IPRegister(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "IP Register Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/IPRegister.rdlc";
            var IPRegister = RptEntity.SpRptIPRegister(
                                    General.ToDateTime(model.StartDate),
                                    General.ToDateTime(model.EndDate),
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("IPRegisterDataSet", IPRegister));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult CentralIPRegister()
        {
            OpRegisterModel model = new OpRegisterModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult CentralIPRegister(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Central IP Register Report");
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/CentralIPRegister.rdlc";
            var CentralIPRegister = RptEntity.SpRptCentralIPRegister(
                                    //General.ToDateTime(model.StartDate),
                                    //General.ToDateTime(model.EndDate),
                                    model.FromDate,
                                    model.ToDate,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CentralIPRegisterDataset", CentralIPRegister));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam,FromDateParam,ToDateParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }








        public JsonResult LabTestResultPrintPDF(int ID, int SalesInvoiceID,int PatientID)
        {
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.LabTestReport);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var LabTest = labTestBL.GetPatientDetails((int)ID, PatientID).Select(a => new SpGetPatientDetailForLabTest_Result()
          
            {
                Patient = a.Patient,
                PatientCode = a.PatientCode,
                Age = a.Age,
                Gender = a.Sex,
                MobileNo = a.Mobile,
                Doctor = a.Doctor
            }).ToList();
            var LabTestItems = labresultBL.GetInvoicedLabTestItems(SalesInvoiceID).Select(a => new SpGetInvoicedLabTestItems_Result()
            {
                Item = a.Item,
                BiologicalReference = a.BiologicalReference,
                PatientLabTestsID = a.PatientLabTestsID,
                Unit = a.Unit,
                ObservedValue = a.ObservedValue,
                HighReference = a.NormalHighLevel,
                LowReference = a.NormalLowLevel,
                Specimen = a.Specimen,
                SpecimenID = a.SpecimenID,
                CollectedDate = a.CollectedDate,
                ReportedDate = a.ReportedDate,
                ReportTime=a.ReportTime,
                CollectedTime=a.CollectedTime,
                Type = a.Type,
                Method=a.Method,
                Test=a.Test
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("LabTestResultPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LabTestResultDataSet", LabTest));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LabTestResultItemsDataSet", LabTestItems));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "LabTestResultPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/LabTest/"), FileName);
            string URL = "/Outputs/LabTest/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DischargeSummaryPrintPdf(int IPID)
        {
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.DischargeSummary);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var PatientDetails = dischargeBL.GetDischargePatientList((int)IPID).Select(a => new SpGetDischargedPatientDetailsForPrint_Result()
            {
                PatientName = a.Patient,
                Age = a.Age,
                Gender = a.Gender,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                OPNO = a.OPNo,
                IPNO = a.IPNo,
                DischargeDate = a.DischargeDate,
                AdmissionDate = (DateTime)a.AdmissionDate,
                MobileNo = a.Phone,
                Diagnosis = a.Diagnosis,
                //HIN = a.HIN
            }).ToList();
            var InternalMedicineDetails = dischargeBL.GetInternalMedicineList((int)IPID).Select(a => new SpGetInternalMedicines_Result()
            {
                InternalMedicines = a.Medicine,
                InternalMedicineDescription = a.Instructions,
                InNoOfDays = a.NoOfDays
            }).ToList();
            var DischargeMedicineDetails = dischargeBL.GetDischargeMedicineList((int)IPID).Select(a => new SpGetDischargeMedicineList_Result()
            {
                DischargeMedicines = a.Medicine,
                MedicineDescription = a.Instructions,
                NoOfDays = a.NoOfDays
            }).ToList();
            var TreatmentDetails = dischargeBL.GetTreatmentList((int)IPID).Select(a => new SpGetTreatmentDetailsForPrint_Result()
            {
                Treatment = a.Treatment,
                TreatmentNo = a.NoOfDays,
                Complaint = a.Complaint1,
                Complaint2 = a.Complaint2,
                Doctor = a.Doctor
            }).ToList();
            var DischageAdviceList = dischargeBL.GetDischargeAdviceList((int)IPID).Select(a => new SpGetDischargeSummaryDetailsByID_Result()
            {
                CourseInHospital = a.Course,
                ConditionAtDischarge = a.Condition,
                DietAdvice = a.Diet
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("DischargeSummary");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, PINParam, LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DischargePatientDataSet", PatientDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("InternalMedicineDataSet", InternalMedicineDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DischargeMedicineDataSet", DischargeMedicineDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DischargedTreatmentDataSet", TreatmentDetails));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DischargeAdviceListDataSet", DischageAdviceList));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "DischargeSummaryPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/DischargeSummary/"), FileName);
            string URL = "/Outputs/DischargeSummary/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult BedOccupancyRegister()
        {
            OpRegisterModel model = new OpRegisterModel();
            model.MonthList = new SelectList(subMasterBL.GetMonthList(), "ID", "Name");
            List<SelectListItem> ls = new List<SelectListItem>();
            for (int i = 2020; i <= 2099; i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            model.YearList = ls;
            return View(model);
        }
        [HttpPost]
        public ActionResult BedOccupancyRegister(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Daily Bed Occupancy Register for the Month of");
            MonthParam = new ReportParameter("Month", model.Month);
            YearParam = new ReportParameter("Year", model.YearName);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/BedOccupancyRegister.rdlc";
            var CurrentYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.ToString("MM");
            if (model.MonthID == 0)
            {
                model.MonthID = Convert.ToInt32(sMonth);
            }
            if (model.YearID == 0)
            {
                model.YearID = Convert.ToInt32(CurrentYear);
            }
            //var Month = model.Month;
            //DateTime first = new DateTime(2020, 5, 1);
            var BORegister = RptEntity.SpRptBedOccupancyRegister(
                                    model.MonthID,
                                    model.YearID
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("BedOccupancyRegisterDataSet", BORegister));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam,MonthParam,YearParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        public JsonResult AppointmentBookingPrintPdf(int ID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.RegistrationfeeReceipt);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var AppointmentSchedule = appointmentScheduleBL.GetAppointmentScheduleDetailsForPrint((int)ID).Select(a => new SpGetAppointmentScheduleDetailsForPrint_Result()
            {
                Time = a.Time,
                ValidFromDate = a.FromDate,
                ValidToDate = a.ToDate,
                HIN = a.HIN,
                PatientName = a.Patient,
                Addressline1 = a.Addressline1,
                Addressline2 = a.Addressline2,
                Addressline3 = a.Addressline3,
                Place = a.Place,
                District = a.District,
                Rate = a.Rate,
                ValidDate = a.CreatedDate,
                Qty = a.Quantity,
                ItemName = a.ItemName,
                SalesOrderNo = a.TransNo,
                DoctorName = a.DoctorName,
                TokenNo = a.TokenNo
            }).ToList();
            //var AppointmentScheduleItems = appointmentScheduleBL.GetAppointmentScheduleItemDetailsForPrint((int)ID).Select(a => new SpGetAppointmentScheduleDetailsForPrint_Result()
            //{
            //    Time = a.Time,
            //    ValidFromDate = a.FromDate,
            //    ValidToDate = a.ToDate,
            //    HIN = a.HIN,
            //    PatientName = a.Patient,
            //    Rate = a.Rate,
            //    ValidDate = a.CreatedDate,
            //    ItemName = a.ItemName,
            //    SalesOrderNo = a.TransNo,
            //    DoctorName = a.DoctorName,
            //    TokenNo = a.TokenNo
            //}).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("AppointmentBookingPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AppointmentBookingPrintPdfDataSet", AppointmentSchedule));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "AppointmentBookingPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/AppointmentBooking/"), FileName);
            string URL = "/Outputs/AppointmentBooking/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        //AppointmentBookingPrintPdfV2 For Triveni
        public JsonResult AppointmentBookingPrintPdfV2(int ID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.RegistrationfeeReceipt);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var AppointmentSchedule = appointmentScheduleBL.GetAppointmentScheduleDetailsForPrint((int)ID).Select(a => new SpGetAppointmentScheduleDetailsForPrint_Result()
            {
                Time = a.Time,
                ValidFromDate = a.FromDate,
                ValidToDate = a.ToDate,
                HIN = a.HIN,
                PatientName = a.Patient,
                Rate = a.Rate,
                ValidDate = a.CreatedDate,
                ItemName = a.ItemName,
                SalesOrderNo = a.TransNo,
                Mode = a.Mode,
                DoctorName = a.DoctorName,
                TokenNo = a.TokenNo
            }).ToList();
            //var AppointmentScheduleItems = appointmentScheduleBL.GetAppointmentScheduleItemDetailsForPrint((int)ID).Select(a => new SpGetAppointmentScheduleDetailsForPrint_Result()
            //{
            //    Time = a.Time,
            //    ValidFromDate = a.FromDate,
            //    ValidToDate = a.ToDate,
            //    HIN = a.HIN,
            //    PatientName = a.Patient,
            //    Rate = a.Rate,
            //    ValidDate = a.CreatedDate,
            //    ItemName = a.ItemName,
            //    SalesOrderNo = a.TransNo,
            //    DoctorName = a.DoctorName,
            //    TokenNo = a.TokenNo
            //}).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("AppointmentBookingPrintPdfV2");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam,LandLine1Param });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AppointmentBookingPrintPdfDataSet", AppointmentSchedule));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "AppointmentBookingPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/AppointmentBooking/"), FileName);
            string URL = "/Outputs/AppointmentBooking/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AppointmentBookingPrintPdfV3(int ID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.RegistrationfeeReceipt);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var AppointmentSchedule = appointmentScheduleBL.GetAppointmentScheduleDetailsForPrint((int)ID).Select(a => new SpGetAppointmentScheduleDetailsForPrint_Result()
            {
                Time = a.Time,
                ValidFromDate = a.FromDate,
                ValidToDate = a.ToDate,
                HIN = a.HIN,
                PatientName = a.Patient,
                Addressline1 = a.Addressline1,
                Addressline2 = a.Addressline2,
                Addressline3 = a.Addressline3,
                Place = a.Place,
                District = a.District,
                Rate = a.Rate,
                ValidDate = a.CreatedDate,
                Qty = a.Quantity,
                ItemName = a.ItemName,
                SalesOrderNo = a.TransNo,
                DoctorName = a.DoctorName,
                TokenNo = a.TokenNo
            }).ToList();
            //var AppointmentScheduleItems = appointmentScheduleBL.GetAppointmentScheduleItemDetailsForPrint((int)ID).Select(a => new SpGetAppointmentScheduleDetailsForPrint_Result()
            //{
            //    Time = a.Time,
            //    ValidFromDate = a.FromDate,
            //    ValidToDate = a.ToDate,
            //    HIN = a.HIN,
            //    PatientName = a.Patient,
            //    Rate = a.Rate,
            //    ValidDate = a.CreatedDate,
            //    ItemName = a.ItemName,
            //    SalesOrderNo = a.TransNo,
            //    DoctorName = a.DoctorName,
            //    TokenNo = a.TokenNo
            //}).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("AppointmentBookingPrintPdfV3");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("AppointmentBookingPrintPdfDataSet", AppointmentSchedule));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "AppointmentBookingPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/AppointmentBooking/"), FileName);
            string URL = "/Outputs/AppointmentBooking/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LaboratoryTestPrintPdf(int ID, int SalesInvoiceID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.LaboratoryTestBill);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var LabTest = labTestBL.GetLabTestDetailsForBilling((int)ID, (int)SalesInvoiceID).Select(a => new SpGetBillableDetailsOfLabTest_Result()
            {
                TransNo = a.TransNo,
                CreatedDate = a.InvoiceDate,
                PatientName = a.Patient,
                PatientCode = a.PatientCode,
                Name = a.ItemName,
                NetAmt = a.NetAmount,
                Price = a.price
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("LaboratoryTestPrintPdf");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, ImagePathParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LaboratoryTestBillPrintPdfDataSet", LabTest));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "LaboratoryTestPrintPdf.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/LabTestBilling/"), FileName);
            string URL = "/Outputs/LabTestBilling/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult LabTestPrintPDF(int ID, int patientLabTestMasterID, int IPID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.LaboratoryTestBill);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var LabTest = labTestBL.GetLabTestDetailsForPrint( ID, patientLabTestMasterID,IPID).Select(a => new SpGetItemsForLabTestPrint_Result()
            {
                TransNo = a.TransNo,
                PatientID = a.PatientID,
                Patient = a.Patient,
                PatientCode = a.PatientCode,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                Place = a.Place,
                ItemID = (int)a.ItemID,
                CreatedDate = a.CreatedDate,
                ItemName = a.ItemName,
                MRP = a.MRP
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("LabTestPrintPDF");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LabTestPrintPDFDataSet", LabTest));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "LabTestPrintPDF.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/LabTest/"), FileName);
            string URL = "/Outputs/LabTest/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult XrayPrintPDF(int ID, int patientLabTestMasterID, int IPID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.XrayBill);
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            var LabTest = xrayBL.GetXrayDetailsForPrint(ID, patientLabTestMasterID, IPID).Select(a => new SpGetItemsForLabTestPrint_Result()
            {
                TransNo = a.TransNo,
                PatientID = a.PatientID,
                Patient = a.Patient,
                PatientCode = a.PatientCode,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                Place = a.Place,
                ItemID = (int)a.ItemID,
                CreatedDate = a.CreatedDate,
                ItemName = a.ItemName,
                MRP = a.MRP
            }).ToList();
            reportViewer.LocalReport.ReportPath = GetReportPath("LabTestPrintPDF");
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, PINParam, ReportNameParam, MobileNoParam });
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LabTestPrintPDFDataSet", LabTest));
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicinePrescriptionTransPrintPdfDataSet", MedicinePrescriptionTrans));
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
            // Open generated PDF.
            string FileName = "LabTestPrintPDF.pdf";
            string FilePath = Path.Combine(Server.MapPath("~/Outputs/LabTest/"), FileName);
            string URL = "/Outputs/LabTest/" + FileName;

            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult RegisterForNoOfPatientsForTheMonth()
        {
            OpRegisterModel model = new OpRegisterModel();
            model.MonthList = new SelectList(subMasterBL.GetMonthList(), "ID", "Name");
            List<SelectListItem> ls = new List<SelectListItem>();
            for (int i = 2020; i <= 2099; i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            model.YearList = ls;
            return View
                (model);
        }
        [HttpPost]
        public ActionResult RegisterForNoOfPatientsForTheMonth(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Monthly Consultation Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            var CurrentYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.ToString("MM");
            if (model.MonthID == 0)
            {
                model.MonthID = Convert.ToInt32(sMonth);
            }
            if (model.YearID == 0)
            {
                model.YearID = Convert.ToInt32(CurrentYear);
            }
            //var Month = model.Month;
            //DateTime first = new DateTime(2020, 5, 1);
            var OPRegister = RptEntity.SpRptRegisterForNoOfPatientsForTheMonth(
                                    model.MonthID,
                                    model.YearID
                                    ).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/RegisterForNoOfPatientsForTheMonth.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("RegisterForNoOfPatientsForTheMonthDataSet", OPRegister));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }
        [HttpGet]
        public ActionResult OPConsultation()
        {
            OpRegisterModel model = new OpRegisterModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.DoctorList = new SelectList(employeeBL.GetDoctor(), "ID", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult OPConsultation(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "OPConsultation Report");
            FilterParam = new ReportParameter("Filters", "OPConsultation");
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            string XMLParams = XMLHelper.ParseXML(model);
            var OPConsultation = RptEntity.SpRptOPConsultationReport(
                                        XMLParams
                                        ).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/OPConsultation.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("OPConsultationDataSet", OPConsultation));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, FromDateParam, ToDateParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");


            //ReportNameParam = new ReportParameter("ReportName", "Employee Daily Report");
            //FilterParam = new ReportParameter("Filters", model.Filters);
            //UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            //FromDateParam = new ReportParameter("FromDate", "01-Apr-2020");
            //ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            //var Production = new object();
            //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/Sales/EmployeeDailyReport.rdlc";
            //string XMLParams = XMLHelper.ParseXML(model);
            //var Employee = dbEntity.SpRptGetEmployeeDailySalesDetails(
            //                        XMLParams
            //                        ).ToList();
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("EmployeeDailyReportDataSet", Employee));
            //ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            //ViewBag.ReportViewer = reportViewer;
            //return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult DailyConsultationBill()
        {
            OpRegisterModel model = new OpRegisterModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult DailyConsultationBill(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Daily Consultation Bill");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            model.LocationID = GeneralBO.LocationID;
            var Production = new object();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/DailyConsultationBill.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            var ConsultationBill = RptEntity.SpRptDailyConsultationBill(
                                    XMLParams
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DailyConsultationBillDataSet", ConsultationBill));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult DailyTotalCollection()
        {
            OpRegisterModel model = new OpRegisterModel();
            model.FromDateString = General.FormatDate(DateTime.Now);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult DailyTotalCollection(OpRegisterModel model)
        {
            ReportNameParam = new ReportParameter("ReportName", "Daily Total Collection Report");
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            model.LocationID = GeneralBO.LocationID;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/DailyTotalCollection.rdlc";
            string XMLParams = XMLHelper.ParseXML(model);
            var ConsultationBill = RptEntity.SpRptDailyTotalCollectionReport(
                                    XMLParams
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DailyTotalCollectionDataSet", ConsultationBill));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, ReportNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ImagePathParam, UserParam, FilterParam });
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { FromDateParam, ToDateParam, CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult CollectionSummary()
        {
            CollectionSummaryModel model = new CollectionSummaryModel();
            DateTime now = DateTime.Now;
            model.FromDate = GeneralBO.FinStartDate;
            model.ToDate = General.FormatDateTime(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult CollectionSummary(CollectionSummaryModel model)
        {
            if (model.FromDate != null)
                StartDate = General.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                EndDate = General.ToDateTime(model.ToDate);
            ReportNameParam = new ReportParameter("ReportName", "Collection Summary");
            FromDateParam = new ReportParameter("FromDate", StartDate.ToShortDateString());
            ToDateParam = new ReportParameter("ToDate", EndDate.ToShortDateString());
            var CollectionSummary = new object();

            CollectionSummary = RptEntity.SpRptCollectionSummary(
                                   StartDate,
                                   EndDate,
                                   model.AccountName
                                   ).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/CollectionSummary.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("CollectionSummaryDataSet", CollectionSummary));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, FromDateParam, ToDateParam, ReportNameParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //public JsonResult ProvisionBillPrintPdf(int IPID, int PatientID)
        //{
        //    ToDateParam = new ReportParameter("ToDate", DateTime.Now.ToShortDateString());
        //    ReportNameParam = new ReportParameter("ReportName", App_LocalResources.Reports.ProvisionBill);
        //    Warning[] warnings;
        //    string[] streamIds;
        //    string contentType;
        //    string mimeType = string.Empty;
        //    string encoding = string.Empty;
        //    string extension = string.Empty;
        //    var ProvBill = serviceSalesOrderBL.GetBillableDetails((int)IPID, (int)PatientID,0).Select(a => new SpGetBillableDetails_Result
        //    { 
        //        ID=a.BillableID,
        //        Item=a.ItemName,
        //        Unit = a.Unit,
        //        CategoryID = a.ItemCategoryID,
        //        ItemCode = a.Code,
        //        Quantity = (decimal)a.Qty,
        //        NetAmt  = (decimal)a.NetAmount,
        //        //CustomerCode=a.PatientCode,
        //        Customer=a.PatientName

        //    }).ToList(); 
        //    reportViewer.LocalReport.ReportPath = GetReportPath("ProvBillPrintPdf");
        //    ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
        //    reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, PINParam, LandLine1Param, ToDateParam });
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProvBillPrintPdfDataSet", ProvBill));
        //    //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LabTestResultItemsDataSet", LabTestItems));
        //    byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);
        //    // Open generated PDF.
        //    string FileName = "ProvBillPrintPdf.pdf";
        //    string FilePath = Path.Combine(Server.MapPath("~/Outputs/ProvisionBilling/"), FileName);
        //    string URL = "/Outputs/ProvisionBilling/" + FileName;

        //    using (FileStream fs = new FileStream(FilePath, FileMode.Create))
        //    {
        //        fs.Write(bytes, 0, bytes.Length);
        //    }
        //    return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);

        //}

        //[HttpGet]
        //public ActionResult DepartmentOpRegister()
        //{
        //    OpRegisterModel model = new OpRegisterModel();
        //    model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
        //    model.ToDateString = General.FormatDate(DateTime.Now);
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult DepartmentOpRegister(OpRegisterModel model)
        //{
        //    if (model.Department == "" || model.Department == null)
        //    {
        //        ReportNameParam = new ReportParameter("ReportName", "OP REGISTER FOR THE DEPARTMENT OF");
        //    }
        //    else
        //    {
        //        var ReportName = "OP REGISTER FOR THE DEPARTMENT OF " + model.Department;
        //        ReportNameParam = new ReportParameter("ReportName", ReportName);
        //    }
        //    FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
        //    ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
        //    var Production = new object();
        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/DepartmentOPRegister.rdlc";
        //    string XMLParams = XMLHelper.ParseXML(model);
        //    var OpRegister = RptEntity.SpRptDepartmentOpRegister(
        //                            XMLParams
        //                            ).ToList();
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DepartmentOPDataSet", OpRegister));
        //    ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
        //    reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam,FromDateParam,ToDateParam });
        //    ViewBag.ReportViewer = reportViewer;
        //    return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        //}

        //[HttpGet]
        //public ActionResult DepartmentIpRegister()
        //{
        //    OpRegisterModel model = new OpRegisterModel();
        //    model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
        //    model.ToDateString = General.FormatDate(DateTime.Now);
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult DepartmentIpRegister(OpRegisterModel model)
        //{
        //    FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
        //    ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
        //    var Production = new object();
        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/DepartmentIPRegister.rdlc";
        //    var IpRegister = RptEntity.SpRptDepartmentIpRegister(
        //                                model.FromDate,
        //                                model.ToDate,
        //                                model.DepartmentID,
        //                                GeneralBO.FinYear,
        //                                GeneralBO.LocationID,
        //                                GeneralBO.ApplicationID
        //                            ).ToList();
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DepartmentIPDataSet", IpRegister));
        //    ReportNameParam = new ReportParameter("ReportName", "IP Register for the Department of");
        //    reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
        //    ViewBag.ReportViewer = reportViewer;
        //    return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        //}

        [HttpGet]
        public ActionResult BilledAndBillable()
        {
            ReportModel model = new ReportModel();
            model.FromDateString = GeneralBO.FinStartDate;
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult BilledAndBillable(ReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var BilledAndBillable = RptEntity.SpRptBilledANDBillableList(
                            model.ReportType,
                            model.FromDate,
                            model.ToDate,
                            GeneralBO.LocationID
                            ).ToList();
            if (model.ReportType == "Billed")
            {
                ReportNameParam = new ReportParameter("ReportName", "Outstanding Report (Billed) for the period from ");             
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/PatientBill.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PatientBillDataSet", BilledAndBillable));
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
            }
            else
            {
                ReportNameParam = new ReportParameter("ReportName", "Outstanding Report (UnBilled) for the period from ");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/PatientBill.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("PatientBillDataSet", BilledAndBillable));
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
            }
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult DoctorwiseMonthlySummary()
        {
            DoctorwiseMonthlySummaryModel model = new DoctorwiseMonthlySummaryModel();
            model.StartDate = General.FormatDate(General.FirstDayOfMonth);
            model.EndDate = General.FormatDate(DateTime.Now);
            return View(model);

        }

        [HttpPost]
        public ActionResult DoctorwiseMonthlySummary(DoctorwiseMonthlySummaryModel model)
        {
            if (model.DoctorName == "" || model.DoctorName == null)
            {
                ReportNameParam = new ReportParameter("ReportName", "Monthly Summary ");
            }
            else
            {
                var ReportName = "Doctorwise Monthly Summary of " + model.DoctorName;
                ReportNameParam = new ReportParameter("ReportName", ReportName);
            }
            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/DoctorwiseMonthlySummary.rdlc";
            var DoctorwiseMonthlySummary = RptEntity.spGetDoctorWiseMonthlySummary(
                                    General.ToDateTime(model.StartDate),
                                    General.ToDateTime(model.EndDate),
                                    model.DoctorID,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID  
                                    ).ToList();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DoctorwiseMonthlySummaryDataSet", DoctorwiseMonthlySummary));
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
    }


        [HttpGet]
        public ActionResult TreatmentScheduleByTherapist()
        {
            TreatmentScheduleByTherapistModel model = new TreatmentScheduleByTherapistModel();
            model.StartDate = General.FormatDate(General.FirstDayOfMonth);
            model.EndDate = General.FormatDate(DateTime.Now);
            return View(model);

        }

        [HttpPost]
        public ActionResult TreatmentScheduleByTherapist(TreatmentScheduleByTherapistModel model)
        {
            var TreatmentScheduleByTherapist = RptEntity.SpRptTreatmentScheduleByTherapist(
                          General.ToDateTime(model.StartDate),
                          General.ToDateTime(model.EndDate),
                          model.TherapistID,
                          model.PatientID,
                          GeneralBO.LocationID,
                          GeneralBO.ApplicationID
                          ).ToList();

            if (model.ReportType == "Therapist")
            {
                if(model.TherapistName!= null)
                {
                    var ReportName = "Treatment Shedule Report Of Therapist : " + model.TherapistName;
                    ReportNameParam = new ReportParameter("ReportName", ReportName);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/TreatmentScheduleByTherapist.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentScheduleByTherapistDataSet", TreatmentScheduleByTherapist));
                }

                else
                {

                    ReportNameParam = new ReportParameter("ReportName", "Treatment Schedule Report");
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/TreatmentScheduleByTherapist.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentScheduleByTherapistDataSet", TreatmentScheduleByTherapist));
                }

            }

            if(model.ReportType == "Patient")
            {
                if(model.PatientName != null)

                {
                    var ReportName = "Treatment Shedule Report of Patient : " + model.PatientName;
                    ReportNameParam = new ReportParameter("ReportName", ReportName);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/TreatmentScheduleByPatient.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentScheduleByPatientDataSet", TreatmentScheduleByTherapist));
                }
                else
                {
                    ReportNameParam = new ReportParameter("ReportName", "Treatment Schedule Report");
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/TreatmentScheduleByPatient.rdlc";
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentScheduleByPatientDataSet", TreatmentScheduleByTherapist));
                }
            }

            FilterParam = new ReportParameter("Filters", model.Filters);
            UserParam = new ReportParameter("User", GeneralBO.EmployeeName);
            ImagePathParam = new ReportParameter("ImagePath", "file:\\" + Request.MapPath(Request.ApplicationPath).ToString() + GeneralBO.ImagePath);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, ImagePathParam, UserParam, FilterParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");

        }

        [HttpGet]
        public ActionResult LabRegister()
        {
            TreatmentReportModel model = new TreatmentReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult LabRegister(TreatmentReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var LabRegister = RptEntity.SpRptLabRegister(
                            model.FromDate,
                            model.ToDate,
                            model.PatientID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            GeneralBO.CreatedUserID
                            ).ToList();
                ReportNameParam = new ReportParameter("ReportName", "Lab Register");
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/LabRegister.rdlc";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LabRegisterDataSet", LabRegister));
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        //[HttpGet]
        //public ActionResult MedicineIssue()
        //{
        //    TreatmentScheduleByTherapistModel model = new TreatmentScheduleByTherapistModel();
        //    model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
        //    model.ToDateString = General.FormatDate(DateTime.Now);
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult MedicineIssue(TreatmentScheduleByTherapistModel model)
        //{
        //    FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
        //    ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
        //    var ReportName = "Medicine Issue For " + model.ReportType;
        //    ReportNameParam = new ReportParameter("ReportName", ReportName);
        //    var MedicineIssue = RptEntity.SpRptMedicineIssue(
        //                    model.ReportType,
        //                    model.FromDate,
        //                    model.ToDate,
        //                    model.PatientID,
        //                    model.TherapistID,
        //                    model.TreatmentRoomID,
        //                    GeneralBO.CreatedUserID,
        //                    GeneralBO.FinYear,
        //                    GeneralBO.LocationID,
        //                    GeneralBO.ApplicationID                          
        //                    ).ToList();
        //        if (model.Mode == "Summary")
        //        {
        //            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineIssueSummary.rdlc";
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineIssueDataSet", MedicineIssue));
        //            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
        //        }
        //        else
        //        {
        //            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/MedicineIssueDetail.rdlc";
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MedicineIssueDataSet", MedicineIssue));
        //            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
        //        }
        //    ViewBag.ReportViewer = reportViewer;
        //    return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        //}

        [HttpGet]
        public ActionResult RoomRegister()
        {
            RoomModel model = new RoomModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            model.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult RoomRegister(RoomModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var RoomRegister = RptEntity.SpRptRoomAdmissionMonthWise(
                            model.FromDate,
                            model.ToDate,
                            model.RoomTypeID,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID                           
                            ).ToList();
            ReportNameParam = new ReportParameter("ReportName", "Room Admission List for the Period of");
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/RoomRegister.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("RoomRegisterDataSet", RoomRegister));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult TreatmentRegister()
        {
            TreatmentReportModel model = new TreatmentReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult TreatmentRegister(TreatmentReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var TreatmentRegister = RptEntity.SpRptTreatmentListMonthwise(
                            model.FromDate,
                            model.ToDate,
                            model.ReportType,
                            model.TreatmentID,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            ).ToList();
            ReportNameParam = new ReportParameter("ReportName", model.ReportType+ " " + "Treatment List for the Period of");
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/TreatmentRegister.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TreatmentRegisterDataSet", TreatmentRegister));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

        [HttpGet]
        public ActionResult LabBillRegister()
        {
            TreatmentReportModel model = new TreatmentReportModel();
            model.FromDateString = General.FormatDate(General.FirstDayOfMonth);
            model.ToDateString = General.FormatDate(DateTime.Now);
            return View(model);
        }
        [HttpPost]
        public ActionResult LabBillRegister(TreatmentReportModel model)
        {
            FromDateParam = new ReportParameter("FromDate", model.FromDateFormatted);
            ToDateParam = new ReportParameter("ToDate", model.ToDateFormatted);
            var LabBillRegister = RptEntity.SpRptLabBillListMonthwise(
                            model.FromDate,
                            model.ToDate,
                            model.ReportType,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            ).ToList();
            ReportNameParam = new ReportParameter("ReportName", model.ReportType + " " + "List for the Period of");
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Areas/Reports/RDLC/AHCMS/LabBillRegister.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("LabBillRegisterDataSet", LabBillRegister));
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { CompanyNameParam, Address1Param, Address2Param, Address3Param, Address4Param, Address5Param, ReportNameParam, FromDateParam, ToDateParam });
            ViewBag.ReportViewer = reportViewer;
            return View("~/Areas/Reports/Views/ReportViewer.cshtml");
        }

    }
  
}
