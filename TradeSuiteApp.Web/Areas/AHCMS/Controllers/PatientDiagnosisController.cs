using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.AHCMS.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.AHCMS.Controllers
{
    public class PatientDiagnosisController : Controller
    {
        private IPatientDiagnosisContract managePatientBL;
        private IGeneralContract generalBL;
        private ITreatmentRoomContract treatmentRoomBL;
        private IFileContract fileBL;
        private ISubmasterContract submasterBL;
        private IIPCaseSheetContract ipcasesheetBL;
        private IDepartmentContract departmentBL;
        private IScreeningContract screeningBL;

        public PatientDiagnosisController()
        {
            managePatientBL = new PatientDiagnosisBL();
            generalBL = new GeneralBL();
            treatmentRoomBL = new TreatmentRoomBL();
            fileBL = new FileBL();
            submasterBL = new SubmasterBL();
            ipcasesheetBL = new IPCaseSheetBL();
            departmentBL = new DepartmentBL();
            screeningBL = new ScreeningBL();
        }
        // GET: AHCMS/ManagePatient
        public ActionResult Index()
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            return View(model);
        }

        public ActionResult Create(int ID, int AppointmentScheduleItemID, int OPID, bool IsCompleted, bool IsReferedIP,int ReviewID, bool IswalkIn)
        {
            PatientDiagnosisModel model = managePatientBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                PatientCode = a.PatientCode,
                Age = a.Age,
                //DOB = General.FormatDate(a.DOB),
                DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                AdmissionDate = General.FormatDate(a.AdmissionDate),
                RoomName = a.RoomName,
                IPDoctor = a.IPDoctor,
                Doctor = a.Doctor,
                Place = a.Place,
                Mobile = a.Mobile,
                Month = a.Month
            }).First();
            model.ExaminationItems = managePatientBL.GetExaminationList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent


            }).ToList();
            model.BaseLineItems = managePatientBL.GetBaseLineInformationList(ID, OPID).Select(m => new BaseLineModel()
            {

                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked,

            }).ToList();
            model.AppointmentProcessID = OPID;
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.IsCompleted = IsCompleted;
            model.IswalkIn = IswalkIn;
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
            model.TherapistList = new SelectList(managePatientBL.GetTherapistDetails(), "ID", "Name");
            model.TreatmentRoomList = new SelectList(treatmentRoomBL.GetTreatmentRoomDetailsList(), "ID", "Name");
            model.ReportDate = General.FormatDate(DateTime.Now);
            model.StartDateMed = General.FormatDate(DateTime.Now);
            model.TreatmentStartDate = General.FormatDate(DateTime.Now);
            model.DateList = managePatientBL.GetDateListByID(ID, OPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.TreatmentNoList = new SelectList(managePatientBL.GetTreatmentNumberList(), "ID", "Text");
            model.InstructionsList = new SelectList(managePatientBL.GetInstructionsList(), "ID", "Name");
            model.MorningList = new SelectList(managePatientBL.GetMedicineTimeList("Morning"), "ID", "Name");
            model.NoonList = new SelectList(managePatientBL.GetMedicineTimeList("Noon"), "ID", "Name");
            model.EveningList = new SelectList(managePatientBL.GetMedicineTimeList("Evening"), "ID", "Name");
            model.NightList = new SelectList(managePatientBL.GetMedicineTimeList("Night"), "ID", "Name");
            model.UnitList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                                new SelectListItem { Text = "", Value = "0"}

                                        }, "Value", "Text");
            model.AppointmentScheduleItemID = AppointmentScheduleItemID;
            model.ModeOfAdministrationList = new SelectList(submasterBL.GetModeOfAdministration(), "ID", "Name");
            model.TestDate = General.FormatDate(DateTime.Now);
            model.IsInPatient = managePatientBL.IsInPatient(ID);
            model.IsReferedIP = IsReferedIP;
            model.ReviewID = ReviewID;
            return View(model);
        }

        public JsonResult GetManagePatientList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = null;
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string TimeHint = Datatable.Columns[3].Search.Value;
                string TokenHint = Datatable.Columns[4].Search.Value;


                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                if (Type == "Previous")
                {
                    DateHint = Datatable.Columns[5].Search.Value;
                }
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = managePatientBL.GetManagePatientList(Type, CodeHint, NameHint, TimeHint, TokenHint, DateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "PatientDiagnosis", "GetManagePatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Save(PatientDiagnosisModel model)
        {
            try
            {
                PatientDiagnosisBO diagnosis = new PatientDiagnosisBO()
                {
                    PatientID = model.PatientID,
                    Date = General.ToDateTime(model.Date),
                    Remark = model.Remark,
                    AppointmentType = model.AppointmentType,
                    VisitType = model.VisitType,
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    IsCompleted = model.IsCompleted,
                    IsReferedIP = model.IsReferedIP,
                    AppointmentProcessID = model.AppointmentProcessID,
                    IswalkIn = model.IswalkIn,
                    ParentID = model.ParentID,
                    ReviewID = model.ReviewID
                };
                if (model.NextVisitDate != null)
                {
                    diagnosis.NextVisitDate = General.ToDateTime(model.NextVisitDate);
                }

                List<VitalChartBO> VitalChartItems = new List<VitalChartBO>();
                if (model.VitalChartItems != null)
                {
                    VitalChartBO Item;
                    foreach (var item in model.VitalChartItems)
                    {
                        Item = new VitalChartBO()
                        {
                            BP = item.BP,
                            Pulse = item.Pulse,
                            Temperature = item.Temperature,
                            Unit=item.Unit,
                            BMI=item.BMI,
                            RespiratoryRate=item.RespiratoryRate,
                            HR = item.HR,
                            RR = item.RR,
                            Weight = item.Weight,
                            Height = item.Height,
                            Others = item.Others
                        };
                        VitalChartItems.Add(Item);
                    }
                }

                List<ExaminationBO> Items = new List<ExaminationBO>();
                if (model.ExaminationItems != null)
                {
                    ExaminationBO Item;

                    foreach (var item in model.ExaminationItems)
                    {
                        Item = new ExaminationBO()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description
                        };
                        Items.Add(Item);
                    }
                }

                List<BaseLineBO> BaseLineItems = new List<BaseLineBO>();
                if (model.BaseLineItems != null)
                {
                    BaseLineBO Item;

                    foreach (var item in model.BaseLineItems)
                    {
                        Item = new BaseLineBO()
                        {

                            Name = item.Name,
                            Description = item.Description,

                        };
                        BaseLineItems.Add(Item);
                    }
                    if (model.OtherConditionsItems != null)
                    {
                        foreach (var item in model.OtherConditionsItems)
                        {

                            Item = new BaseLineBO()
                            {

                                Name = item.Name,
                                Description = item.Description,

                            };
                            BaseLineItems.Add(Item);


                        }
                    }
                }


                List<ReportBO> ReportItems = new List<ReportBO>();
                if (model.ReportItems != null)
                {
                    ReportBO Item;

                    foreach (var item in model.ReportItems)
                    {
                        Item = new ReportBO()
                        {
                            DocumentID = item.DocumentID,
                            Name = item.Name,
                            Date = General.ToDateTime(item.Date),
                            Description = item.Description
                        };
                        ReportItems.Add(Item);
                    }
                }

                List<LabtestsBO> LabTestItems = new List<LabtestsBO>();
                if (model.LabTestItems != null)
                {
                    LabtestsBO Item;

                    foreach (var item in model.LabTestItems)
                    {
                        Item = new LabtestsBO()
                        {
                            ID = item.ID,
                            TestDate = General.ToDateTime(item.TestDate),
                            LabTestID = item.LabTestID
                        };
                        LabTestItems.Add(Item);
                    }
                }

                List<TreatmentBO> Treatments = new List<TreatmentBO>();
                if (model.TreatmentItems != null)
                {
                    TreatmentBO Item;

                    foreach (var item in model.TreatmentItems)
                    {
                        Item = new TreatmentBO()
                        {
                            TreatmentID = item.TreatmentID,
                            TherapistID = item.TherapistID,
                            TreatmentRoomID = item.TreatmentRoomID,
                            Instructions = item.Instructions,
                            StartDate = General.ToDateTime(item.StartDate),
                            EndDate = General.ToDateTime(item.EndDate),
                            TreatmentNo = item.TreatmentNo,
                            PatientTreatmentID = item.PatientTreatmentID,
                            MorningTimeID = item.MorningTime,
                            NoonTimeID = item.NoonTime,
                            EveningTimeID = item.EveningTime,
                            NightTimeID = item.NightTime,
                            IsMorning = item.IsMorning,
                            IsNoon = item.IsNoon,
                            Isevening = item.Isevening,
                            IsNight = item.IsNight,
                            NoofDays = item.NoofDays
                        };
                        Treatments.Add(Item);
                    }
                }

                List<TreatmentItemBO> TreatmentMedicines = new List<TreatmentItemBO>();
                if (model.TreatmentMedicines != null)
                {
                    TreatmentItemBO Item;

                    foreach (var item in model.TreatmentMedicines)
                    {
                        Item = new TreatmentItemBO()
                        {
                            MedicineID = item.MedicineID,
                            Medicine = item.Medicine,
                            TreatmentID = item.TreatmentID,
                            StandardMedicineQty = item.StandardMedicineQty,
                            MedicineUnitID=item.MedicineUnitID
                        };
                        TreatmentMedicines.Add(Item);
                    }
                }

                List<MedicineBO> MedicinesList = new List<MedicineBO>();
                if (model.Medicines != null)
                {
                    MedicineBO Item;

                    foreach (var item in model.Medicines)
                    {
                        Item = new MedicineBO()
                        {
                            MedicineID = item.MedicinesID,
                            UnitID = item.UnitID,
                            Quantity = item.Quantity,
                            GroupID = item.GroupID,
                            PatientMedicinesID = item.PatientMedicinesID
                        };
                        MedicinesList.Add(Item);
                    }
                }

                List<MedicineItemBO> MedicinesItemsList = new List<MedicineItemBO>();
                if (model.MedicineItems != null)
                {
                    MedicineItemBO Item;

                    foreach (var item in model.MedicineItems)
                    {
                        Item = new MedicineItemBO()
                        {
                            MorningTime = item.MorningTime,
                            NoonTime = item.NoonTime,
                            EveningTime = item.EveningTime,
                            NightTime = item.NightTime,
                            StartDate = General.ToDateTime(item.StartDate),
                            EndDate = General.ToDateTime(item.EndDate),
                            InstructionsID = item.InstructionsID,
                            IsMorning = item.IsMorning,
                            Isevening = item.Isevening,
                            IsNoon = item.IsNoon,
                            IsNight = item.IsNight,
                            IsMultipleTimes = item.IsMultipleTimes,
                            NoofDays = item.NoofDays,
                            IsEmptyStomach = item.IsEmptyStomach,
                            IsAfterFood = item.IsAfterFood,
                            IsBeforeFood = item.IsBeforeFood,
                            Description = item.Description,
                            GroupID = item.GroupID,
                            Frequency = item.Frequency,
                            ModeOfAdministrationID = item.ModeOfAdministrationID,
                            PatientMedicineID = item.PatientMedicineID,
                            IsMiddleOfFood = item.IsMiddleOfFood,
                            IsWithFood = item.IsWithFood,
                            MedicineInstruction = item.MedicineInstruction,
                            QuantityInstruction = item.QuantityInstruction
                        };
                        MedicinesItemsList.Add(Item);
                    }
                }
                List<XrayBO> XrayItem = new List<XrayBO>();
                if (model.XrayItems != null)
                {
                    XrayBO Item;

                    foreach (var item in model.XrayItems)
                    {
                        Item = new XrayBO()
                        {
                            ID = item.ID,
                            XrayDate = General.ToDateTime(item.XrayDate),
                            XrayID = item.XrayID
                        };
                        XrayItem.Add(Item);
                    }
                }

                List<DoctorListBO> DoctorList = new List<DoctorListBO>();
                if (model.DoctorList != null)
                {
                    DoctorListBO Item;

                    foreach (var item in model.DoctorList)
                    {
                        Item = new DoctorListBO()
                        {
                            DoctorName = item.DoctorName,
                            DoctorNameID = item.DoctorNameID,
                        };
                        DoctorList.Add(Item);
                    }
                }

                managePatientBL.Save(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "PatientDiagnosis", "Save", model.PatientID, e);
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult VitalChart(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
           
            var obj = managePatientBL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            if (obj != null)
            {
                model.BP = obj.BP;
                model.Pulse = obj.Pulse;
                model.Temperature = obj.Temperature;
                model.HR = obj.HR;
                model.IsCompleted = IsCompleted;
                model.Unit = obj.Unit;
            }
            return PartialView(model);
        }

        public PartialViewResult Chart(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            var obj = managePatientBL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            if (obj != null)
            {
                model.RR = obj.RR;
                model.Height = obj.Height;
                model.Weight = obj.Weight;
                model.BMI = obj.BMI;
                model.IsCompleted = IsCompleted;
            }
            return PartialView(model);
        }

        public PartialViewResult Vitals(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            var obj = managePatientBL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            if (obj != null)
            {
                model.RespiratoryRate = obj.RespiratoryRate;
                model.Others = obj.Others;
                model.IsCompleted = IsCompleted;
            }
            return PartialView(model);
        }

        public PartialViewResult Treatment(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.TreatmentItems = managePatientBL.GetTreatmentListByID(PatientID, FromDate, AppointmentProcessID).Select(m => new TreatmentModel()
            {
                TreatmentID = m.TreatmentID,
                Name = m.Name,
                TreatmentRoomID = m.TreatmentRoomID,
                TreatmentRoomName = m.TreatmentRoomName,
                TherapistID = m.TherapistID,
                TherapistName = m.TherapistName,
                StartDate = General.FormatDate(m.StartDate),
                EndDate = General.FormatDate(m.EndDate),
                FinishedTreatment = m.FinishedTreatment,
                NoOfTreatment = m.NoOfTreatment,
                Status = m.Status,
                Instructions = m.Instructions,
                PatientTreatmentID = m.PatientTreatmentID,
                IsCompleted = IsCompleted,
                DoctorName = m.DoctorName,
                IsMorning = m.IsMorning,
                MorningTime = m.MorningTime,
                IsNoon = m.IsNoon,
                NoonTime = m.NoonTime,
                Isevening = m.Isevening,
                EveningTime = m.EveningTime,
                IsNight = m.IsNight,
                NightTime = m.NightTime,
                MorningID=Convert.ToInt16(m.MorningTimeID),
                NoonID = Convert.ToInt16(m.NoonTimeID),
                EveningID = Convert.ToInt16(m.EveningTimeID),
                NightID = Convert.ToInt16(m.NightTimeID)

            }).ToList();
            return PartialView(model);
        }

        public PartialViewResult GetReport(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ReportItems = managePatientBL.GetReportListByID(PatientID, FromDate, AppointmentProcessID).Select(a => new ReportModel()
            {
                Name = a.Name,
                DocumentID = a.DocumentID,
                Description = a.Description,
                Date = General.FormatDate(a.Date),
                IsCompleted = IsCompleted,
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString())
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult GetReportv5(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ReportItems = managePatientBL.GetReportListByIDV5(PatientID, FromDate, AppointmentProcessID).Select(a => new ReportModel()
            {
                Name = a.Name,
                DocumentID = a.DocumentID,
                Description = a.Description,
                Date = General.FormatDate(a.Date),
                IsCompleted = IsCompleted,
                IsBeforeAdmission=a.IsBeforeAdmission,
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString())
            }).ToList();
            return PartialView(model);
        }

        public JsonResult GetTreatmentMedicineList(int PatientID, string Date, int AppointmentProcessID)
        {
            try
            {
                DateTime FromDate = new DateTime();
                if (Date != null)
                {
                    FromDate = General.ToDateTime(Date);
                }
                List<TreatmentItemBO> TreatmentItems = managePatientBL.GetTreatmentMedicineListByID(PatientID, FromDate, AppointmentProcessID);

                return Json(new { Status = "success", Data = TreatmentItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult Medicines(int OPID, int PatientID, string Date, bool IsCompleted)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.Medicines = managePatientBL.GetMedicineListByID(OPID, PatientID, FromDate).Select(a => new MedicineModel()
            {
                Medicine = a.Medicine,
                Prescription = a.Prescription,
                PrescriptionID = a.PrescriptionID,
                TransID = a.TransID,
                PatientMedicinesID = a.PatientMedicinesID,
                IsCompleted = IsCompleted,
                Status = a.Status,
                DoctorName = a.DoctorName,
                Qty = a.Qty,
                MedicineInstruction = a.MedicineInstruction,
                QuantityInstruction = a.QuantityInstruction,
            }).ToList();
            return PartialView(model);
        }
        public JsonResult GetMedicinesList(int PatientID, string Date, int AppointmentProcessID)
        {
            try
            {
                DateTime FromDate = new DateTime();
                if (Date != null)
                {
                    FromDate = General.ToDateTime(Date);
                }
                List<MedicineBO> MedicineList = managePatientBL.GetMedicinesDetails(PatientID, FromDate, AppointmentProcessID);

                return Json(new { Status = "success", Data = MedicineList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult CaseSheet(int PatientID, string Date, int AppointmentProcessID)
        {
            try
            {
                DateTime FromDate = new DateTime();
                if (Date != null)
                {
                    FromDate = General.ToDateTime(Date);
                }
                var obj = managePatientBL.GetCaseSheet(PatientID, FromDate, AppointmentProcessID);
                PatientDiagnosisModel model = new PatientDiagnosisModel();
                if (obj != null)
                {
                    model.Remark = obj.Remark;
                    model.NextVisitDate = obj.NextVisitDate == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)obj.NextVisitDate);
                }

                return Json(new { Status = "success", Data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult Examination(int PatientID, string Date, int AppointmentProcessID, int ReviewID=0)
        {
            DateTime FromDate = General.ToDateTime(Date);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ExaminationItems = managePatientBL.GetExamination(PatientID, FromDate, AppointmentProcessID, ReviewID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
            }).ToList();
            //model.ExaminationItems = managePatientBL.GetExaminationByDate(PatientID, FromDate, ReviewID, AppointmentProcessID).Select(m => new ExaminationModel()
            //{
            //    ID = m.ID,
            //    Name = m.Name,
            //    Type = m.Type,
            //    GroupName = m.GroupName,
            //    Description = m.Description,
            //    GeneralOptionID = m.GeneralOptionID,
            //    Text = m.Text,
            //}).ToList();
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
            return PartialView(model);
        }

        public PartialViewResult BaseLineInformation(int PatientID, string Date, int AppointmentProcessID, int ReviewID = 0)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.BaseLineItems = managePatientBL.GetBaseLineInformationDetails(PatientID, AppointmentProcessID).Select(m => new BaseLineModel()
            {

                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked=m.IsChecked

            }).ToList();
            return PartialView(model);
        }

        public JsonResult TreatmentDetailsPrintPdf(int ID)
        {
            return null;
        }

        public JsonResult MedicinePrescriptionPrintPdf(int ID)
        {
            return null;
        }

        public ActionResult GetMedicinePrescriptionDetails(int PatientID, string Date)
        {
            DateTime FromDate = General.ToDateTime(Date);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.MedicinePrescription = managePatientBL.GetMedicinePrescriptionByID(PatientID, FromDate).Select(a => new MedicineModel()
            {
                PrescriptionNo = a.PrescriptionNo,
                Medicine = a.Medicine,
                Unit = a.Unit,
                Quantity = (decimal)a.Quantity,
                Instructions = a.Instructions
            }).ToList();
            return View(model);
        }

        public ActionResult PrintList()
        {
            return View();
        }

        public JsonResult GetTreatmentDetailsListForPrint(DatatableModel Datatable)
        {
            try
            {

                string TransNo = Datatable.Columns[1].Search.Value;
                string Date = Datatable.Columns[2].Search.Value;
                string Patient = Datatable.Columns[3].Search.Value;
                string Doctor = Datatable.Columns[4].Search.Value;
                string Time = Datatable.Columns[5].Search.Value;
                string TokenNo = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = managePatientBL.GetTreatmentDetailsListForPrint(TransNo, Date, Patient, Doctor, Time, TokenNo, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "PatientDiagnosis", "GetTreatmentDetailsListForPrint", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int ID,int PatientID)
        {
            DateTime Date = DateTime.Now;
            PatientDiagnosisModel model = managePatientBL.GetPatientDetailsItemsByID(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date),
                Doctor = a.Doctor,
                Time = a.Time,
                TokenNo = a.TokenNumber,
                AppointmentProcessID = a.AppointmentProcessID,
                Address = a.Address,
                Age = a.Age,
                Gender = a.Gender,
                Mobile = a.Mobile
            }).First();
            model.Medicines = managePatientBL.GetMedicineListByID(ID, PatientID, Date).Select(a => new MedicineModel()
            {
                Medicine = a.Medicine,
                Qty = a.Qty,
                Unit = a.Unit,
                Prescription = a.Prescription,
                Description=a.Description,
                TransID = a.TransID
            }).ToList();
            model.TreatmentItems = managePatientBL.GetTreatmentListItemsByID(ID).Select(a => new TreatmentModel()
            {
                Name = a.Name,
                TherapistName = a.TherapistName,
                TreatmentRoomName = a.TreatmentRoomName,
                StartDate = General.FormatDate(a.StartDate),
                EndDate = General.FormatDate(a.EndDate)
            }).ToList();
            return View(model);
        }

        public ActionResult IPIndex()
        {
            return View();
        }

        public JsonResult GetIPPatientList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string DateHint = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = managePatientBL.GetIPPatientList(CodeHint, NameHint, DateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "PatientDiagnosis", "GetIPPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMedicinesItemsList(int PatientID, string Date, int AppointmentProcessID)
        {
            try
            {
                DateTime FromDate = new DateTime();
                if (Date != null)
                {
                    FromDate = General.ToDateTime(Date);
                }
                PatientDiagnosisModel model = new PatientDiagnosisModel();
                List<MedicineItemBO> MedicineList = managePatientBL.GetMedicinesItemsList(PatientID, FromDate, AppointmentProcessID);
                MedicineItemModel medicineItem;
                model.MedicineItems = new List<MedicineItemModel>();
                foreach (var a in MedicineList)
                {
                    medicineItem = new MedicineItemModel()
                    {
                        GroupID = a.GroupID,
                        NoofDays = a.NoofDays,
                        IsMorning = a.IsMorning,
                        IsNoon = a.IsNoon,
                        Isevening = a.Isevening,
                        IsNight = a.IsNight,
                        MorningTime = a.MorningTime,
                        NoonTime = a.NoonTime,
                        EveningTime = a.EveningTime,
                        NightTime = a.NightTime,
                        IsEmptyStomach = a.IsEmptyStomach,
                        IsBeforeFood = a.IsBeforeFood,
                        IsAfterFood = a.IsAfterFood,
                        Description = a.Description,
                        MorningTimeID = a.MorningTimeID,
                        EveningTimeID = a.EveningTimeID,
                        NoonTimeID = a.NoonTimeID,
                        NightTimeID = a.NightTimeID,
                        StartDate = General.FormatDate(a.StartDate),
                        EndDate = General.FormatDate(a.EndDate),
                        PatientMedicineID = a.PatientMedicineID,
                        ModeOfAdministrationID = a.ModeOfAdministrationID,
                        Frequency = a.Frequency,
                        IsWithFood = a.IsWithFood,
                        IsMiddleOfFood = a.IsMiddleOfFood,
                        IsMultipleTimes=a.IsMultipleTimes,
                        MedicineInstruction = a.MedicineInstruction,
                        QuantityInstruction = a.QuantityInstruction,
                    };
                    model.MedicineItems.Add(medicineItem);
                }
                return Json(new { Status = "success", Data = model.MedicineItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetCheckedTest(int OPID)
        {
            List<LabtestsBO> LabTestID = ipcasesheetBL.GetCheckedTest(OPID);
            return Json(new { LabTestID = LabTestID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrescribedXrayTest(int OPID)
        {
            List<XrayBO> XrayID = ipcasesheetBL.GetPrescribedXrayTest(OPID);
            return Json(new { XrayID = XrayID }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult LabTest(int PatientID, int OPID, string Date)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.LabAndXrayItems = managePatientBL.GetLabItems(PatientID, OPID, FromDate).Select(a => new LabTestItemModel()
            {
                ID = a.ID,
                Date = General.FormatDate(a.Date),
                ItemID = a.ItemID,
                ObserveValue = a.ObserveValue,
                ItemName = a.ItemName,
                Category = a.Category,
                BiologicalReference = a.BiologicalReference,
                Unit = a.Unit,
                DocumentID = (int)a.DocumentID,
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString()),
                Path=a.Path
            }).ToList();
            return PartialView(model);
        }

        public PartialViewResult XrayTest(int PatientID, int OPID, string Date)
        {
            DateTime FromDate = new DateTime();
            if (Date != null)
            {
                FromDate = General.ToDateTime(Date);
            }
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.XrayItemList = managePatientBL.GetXrayItems(PatientID, OPID, FromDate).Select(a => new XraysItemModel()
            {
                ID = a.ID,
                Date = General.FormatDate(a.Date),
                ItemID = a.ItemID,           
                ItemName = a.ItemName,
                Category = a.Category,               
                DocumentID = (int)a.DocumentID,
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString()),
                Path = a.Path
            }).ToList();
            return PartialView(model);
        }
        public JsonResult GetPatientMedicines(int PatientID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            List<MedicineModel> Medicines = managePatientBL.GetMedicines(PatientID).Select(a => new MedicineModel()
            {
                Unit = a.Unit,
                Medicine = a.Medicine,
                StartDate = General.FormatDate(a.StartDate),
                EndDate = General.FormatDate(a.EndDate),
                NoofDays = a.NoofDays,
                Instructions = a.Description,
                Qty = a.Qty,
                PatientMedicinesID = a.PatientMedicinesID
            }).ToList();
            return Json(new { Medicine = Medicines }, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult PreviousMedicines(int PatientMedicinesID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.Medicines = managePatientBL.GetPreviousMedicineListByID(PatientMedicinesID).Select(a => new MedicineModel()
            {
                Medicine = a.Medicine,
                Prescription = a.Prescription,
                PrescriptionID = a.PrescriptionID,
                TransID = a.TransID,
                PatientMedicinesID = a.PatientMedicinesID,
                IsCompleted = a.IsCompleted,
            }).ToList();
            return PartialView(model);
        }

        public JsonResult GetPreviousMedicinesList(int PatientMedicinesID)
        {
            try
            {
                List<MedicineBO> MedicineList = managePatientBL.GetPreviousMedicinesList(PatientMedicinesID);

                return Json(new { Status = "success", Data = MedicineList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetPreviousMedicinesItemsList(int PatientMedicinesID)
        {
            try
            {
                PatientDiagnosisModel model = new PatientDiagnosisModel();
                List<MedicineItemBO> MedicineList = managePatientBL.GetPreviousMedicinesItemsList(PatientMedicinesID);
                MedicineItemModel medicineItem;
                model.MedicineItems = new List<MedicineItemModel>();
                foreach (var a in MedicineList)
                {
                    medicineItem = new MedicineItemModel()
                    {
                        GroupID = a.GroupID,
                        NoofDays = a.NoofDays,
                        IsMorning = a.IsMorning,
                        IsNoon = a.IsNoon,
                        Isevening = a.Isevening,
                        IsNight = a.IsNight,
                        MorningTime = a.MorningTime,
                        NoonTime = a.NoonTime,
                        EveningTime = a.EveningTime,
                        NightTime = a.NightTime,
                        IsEmptyStomach = a.IsEmptyStomach,
                        IsBeforeFood = a.IsBeforeFood,
                        IsAfterFood = a.IsAfterFood,
                        Description = a.Description,
                        MorningTimeID = a.MorningTimeID,
                        EveningTimeID = a.EveningTimeID,
                        NoonTimeID = a.NoonTimeID,
                        NightTimeID = a.NightTimeID,
                        StartDate = General.FormatDate(a.StartDate),
                        EndDate = General.FormatDate(a.EndDate),
                        PatientMedicineID = a.PatientMedicineID,
                        ModeOfAdministrationID = a.ModeOfAdministrationID,
                        Frequency = a.Frequency
                    };
                    model.MedicineItems.Add(medicineItem);
                }
                return Json(new { Status = "success", Data = model.MedicineItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PreviousExamination(int PatientID, int ReviewID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ExaminationItems = managePatientBL.GetPreviousExamination(PatientID, ReviewID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
            }).ToList();
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
            return PartialView(model);
        }
        public PartialViewResult PreviousExaminationV5(int PatientID, int ReviewID)
        {
            //DateTime FromDate = General.ToDateTime(Date);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ExaminationItems = managePatientBL.GetPreviousExamination(PatientID, ReviewID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
            }).ToList();
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
            return PartialView(model);
        }

        public JsonResult GetLabTestList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string TypeHint = Datatable.Columns[3].Search.Value;
                string ServiceHint = Datatable.Columns[4].Search.Value;
                string NameHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = managePatientBL.GetAllLabTestList(CodeHint, TypeHint, ServiceHint, NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "InternationalPatient", "GetInternationalPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLaborXrayListV5(DatatableModel Datatable)
        {
            try
            {
                 
                string CodeHint = Datatable.Columns[2].Search.Value;
                string TypeHint = Datatable.GetValueFromKey("Type", Datatable.Params);
                string ServiceHint = Datatable.Columns[4].Search.Value;
                string NameHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = managePatientBL.GetAllLabTestList(CodeHint, TypeHint, ServiceHint, NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "InternationalPatient", "GetInternationalPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult History(int OPID, int PatientID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.History = managePatientBL.GetHistoryListByID(OPID, PatientID).Select(a => new HistoryModel()
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
                Patient=a.Patient,
                TransNo=a.TransNo,
                DischargedDate=a.DischargedDate == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DischargedDate),
                PresentingComplaints=a.PresentingComplaints,
                Associatedcomplaints = a.Associatedcomplaints,
                AyurvedicDiagnosis = a.AyurvedicDiagnosis,
                ContemporaryDiagnosis = a.ContemporaryDiagnosis
            }).ToList();
            return PartialView(model);
        }

        public PartialViewResult HistoryDetails(int ParentID, int OPID, int IPID, int PatientID, string AppointmentType, bool IsCompleted =false)
        {
            List<HistoryBO> History = managePatientBL.GetHistoryByID(ParentID, OPID, PatientID);
            List<MedicineBO> MediceinsHistory = managePatientBL.GetMedicinesHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
            List<TreatmentBO> TreatmentHistory = managePatientBL.GetTreatmentHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
            List<VitalChartBO> VitalChartHistory = managePatientBL.GetVitalChartHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
            List<RoundsBO> RoundsHistory = managePatientBL.GetRoundsHistory(ParentID, OPID, IPID, PatientID, AppointmentType);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.IsCompleted = IsCompleted;
            model.History = History.Select(a => new HistoryModel()
            {
                ParentID = a.ParentID,
                AppointmentProcessID = a.AppointmentProcessID,
                IPID = a.IPID,
                PatientID = a.PatientID,
                AppointmentType = a.AppointmentType,
                Disease = a.Disease,
                ReportedDate = a.ReportedDate,
                CaseSheet = a.CaseSheet,
                Remarks = a.Remarks,
                Doctor = a.Doctor,
                SuggestedReviewDate = a.SuggestedReviewDate == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.SuggestedReviewDate),
                BP = a.BP,
                Pulse = a.Pulse,
                Temperature = a.Temperature,
                HR = a.HR,
                RR = a.RR,
                Height = a.Height,
                Weight = a.Weight,
                Others = a.Others,
                Patient = a.Patient,
                TransNo = a.TransNo,
                DischargedDate = a.DischargedDate == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DischargedDate),
                PresentingComplaints = a.PresentingComplaints,
                ContemporaryDiagnosis = a.ContemporaryDiagnosis,
                Associatedcomplaints = a.Associatedcomplaints,
                AyurvedicDiagnosis=a.AyurvedicDiagnosis,

                MediceinsHistory = MediceinsHistory.Where(m => m.ParentID == a.ParentID && m.AppointmentProcessID == a.AppointmentProcessID && m.PatientID == a.PatientID).Select(m => new MedicineModel()
                {
                    ParentID = m.ParentID,
                    AppointmentProcessID = m.AppointmentProcessID,
                    PatientID = m.PatientID,
                    Medicine = m.Medicine,
                    StartDate = Convert.ToString(m.StartDate),
                    EndDate = Convert.ToString(m.EndDate),
                    NoofDays = m.NoofDays,
                    IsNoon = m.IsNoon,
                    IsEvening = m.IsEvening,
                    IsNight = m.IsNight,
                    IsMultipleTimes = m.IsMultipleTimes,
                    IsEmptyStomach = m.IsEmptyStomach,
                    IsBeforeFood = m.IsBeforeFood,
                    IsAfterFood = m.IsAfterFood,
                    MorningTime = m.MorningTime,
                    NoonTime = m.NoonTime,
                    EveningTime = m.EveningTime,
                    Description=m.Description
                }).ToList(),
                TreatmentHistory = TreatmentHistory.Where(t => t.ParentID == a.ParentID && t.AppointmentProcessID == a.AppointmentProcessID && t.PatientID == a.PatientID).Select(t => new TreatmentModel()
                {
                    ParentID = t.ParentID,
                    AppointmentProcessID = t.AppointmentProcessID,
                    PatientID = t.PatientID,
                    Medicine = t.Medicine,
                    Name = t.Name,
                    TherapistName = t.TherapistName,
                    TreatmentRoomName = t.TreatmentRoomName,
                    StartDate = Convert.ToString(t.StartDate),
                    EndDate = Convert.ToString(t.EndDate),
                    NoofDays = t.NoofDays,
                }).ToList(),
                VitalChartHistory = VitalChartHistory.Where(v => v.ParentID == a.ParentID && v.AppointmentProcessID == a.AppointmentProcessID && v.PatientID == a.PatientID).Select(v => new VitalChartModel()
                {
                    ParentID = (int)v.ParentID,
                    AppointmentProcessID = (int)v.AppointmentProcessID,
                    PatientID = (int)v.PatientID,
                    AppointmentType = v.AppointmentType,
                    BP = v.BP,
                    Pulse = v.Pulse,
                    Temperature = v.Temperature,
                    HR = v.HR,
                    RR = v.RR,
                    Height = v.Height,
                    Weight = v.Weight,
                    Others = v.Others,
                    Time = v.Time
                }).ToList(),
                RoundsHistory = RoundsHistory.Where(r => r.ParentID == a.ParentID && r.AppointmentProcessID == a.AppointmentProcessID && r.PatientID == a.PatientID).Select(r => new RoundsModel()
                {
                    ParentID = (int)r.ParentID,
                    AppointmentProcessID = (int)r.AppointmentProcessID,
                    PatientID = (int)r.PatientID,
                    AppointmentType = r.AppointmentType,
                    RoundsDate = General.FormatDate(r.RoundsDate),
                    Remarks = r.Remarks
                }).ToList(),

            }).ToList();
            return PartialView(model);
        }

        public JsonResult GetDepartmentItems(int AppointmentScheduleItemID)
        {
            try
            {
                var obj = managePatientBL.GetDepartmentItems(AppointmentScheduleItemID);
                PatientDiagnosisModel model = new PatientDiagnosisModel();
                if (obj != null)
                {
                    model.DepartmentID = obj.DepartmentID;
                    model.DepartmentName = obj.DepartmentName;
                    model.PatientID = obj.PatientID;
                    model.PatientName = obj.PatientName;
                    model.Date = General.FormatDate(obj.Date);

                }
                return Json(new { Status = "success", Data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult EditDepartment(PatientDiagnosisModel model)
        {
            try
            {
                PatientDiagnosisBO EditDepartment = new PatientDiagnosisBO()
                {
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    DepartmentID = model.DepartmentID
                };
                managePatientBL.EditDepartment(EditDepartment);
                return Json(new { Status = "Success", Message = "Department Edit successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Department Edit failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult IsPatientHistory(int AppointmentProcessID, int PatientID)
        {
            bool IsPatientHistory = managePatientBL.IsPatientHistory(AppointmentProcessID, PatientID);
            return Json(new { Status = "success", IsPatientHistory = IsPatientHistory }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StopMedicine(int PatientMedicinesID)
        {
            bool delete = managePatientBL.StopMedicine(PatientMedicinesID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoctorList(int PatientID, string Date, int AppointmentProcessID)
        {
            try
            {
                DateTime FromDate = new DateTime();
                if (Date != null)
                {
                    FromDate = General.ToDateTime(Date);
                }
                List<DoctorListBO> DoctorList = managePatientBL.GetDoctorList(PatientID, FromDate, AppointmentProcessID);

                return Json(new { Status = "success", Data = DoctorList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        
        //For Ilaj by priyanka
        public ActionResult CreateV2(int ID, int AppointmentScheduleItemID, int OPID, bool IsCompleted, bool IsReferedIP,int ReviewID,bool IswalkIn)
        {
            PatientDiagnosisModel model = managePatientBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                PatientCode = a.PatientCode,
                Age = a.Age,
                //DOB = General.FormatDate(a.DOB),
                DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                AdmissionDate = General.FormatDate(a.AdmissionDate),
                RoomName = a.RoomName,
                IPDoctor = a.IPDoctor,
                Doctor = a.Doctor,
                Place = a.Place,
                Mobile = a.Mobile,
                Month = a.Month,
                Gender = a.Gender
            }).First();
            model.ExaminationItems = managePatientBL.GetExaminationList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent


            }).ToList();
            model.CaseSheetItems = managePatientBL.GetCaseSheetList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked
            }).ToList();
            model.BaseLineItems = managePatientBL.GetBaseLineInformationList(ID, OPID).Select(m => new BaseLineModel()
            {

                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked,

            }).ToList();

            model.RogaPareekshaItems = managePatientBL.GetRogaPareekshaList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked,

            }).ToList();

            model.RogaNirnayamItems = managePatientBL.GetRogaNirnayamList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked,

            }).ToList();
            var obj = screeningBL.IsPatientExists(OPID);
            model.IsExists = obj.IsExists;
            if (model.IsExists == true)
            {
                model.QuestionnaireItems = screeningBL.GetQuestionnaireAndAnswers(ID, OPID).Select(m => new QuestionnaireModel()
                {
                    ID = m.ID,
                    Name = m.Name,
                    Type = m.Type,
                    GroupName = m.GroupName,
                    Description = m.Description,
                    GeneralOptionID = m.GeneralOptionID,
                    Text = m.Text,
                    Value = m.GeneralOptionID,
                    Diagnosis = m.Diagnosis,
                    DiagnosisID = m.Value,
                    IsParent = m.IsParent,
                    IsChecked = m.IsChecked,

                }).ToList();

            }
            else
            {
                model.QuestionnaireItems = screeningBL.GetQuestionnaireList(ID, OPID).Select(m => new QuestionnaireModel()
                {
                    ID = m.ID,
                    Name = m.Name,
                    Type = m.Type,
                    GroupName = m.GroupName,
                    Description = m.Description,
                    GeneralOptionID = m.GeneralOptionID,
                    Text = m.Text,
                    Value = m.GeneralOptionID,
                    Diagnosis = m.Diagnosis,
                    DiagnosisID = m.Value,
                    IsParent = m.IsParent,
                    IsChecked = m.IsChecked,

                }).ToList();
            }



            model.AppointmentProcessID = OPID;
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.IsCompleted = IsCompleted;
            model.IswalkIn = IswalkIn;
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
            model.TherapistList = new SelectList(managePatientBL.GetTherapistDetails(), "ID", "Name");
            model.TreatmentRoomList = new SelectList(treatmentRoomBL.GetTreatmentRoomDetailsList(), "ID", "Name");
            model.ReportDate = General.FormatDate(DateTime.Now);
            model.StartDateMed = General.FormatDate(DateTime.Now);
            model.TreatmentStartDate = General.FormatDate(DateTime.Now);
            model.DateList = managePatientBL.GetDateListByID(ID, OPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.TreatmentNoList = new SelectList(managePatientBL.GetTreatmentNumberList(), "ID", "Text");
            model.InstructionsList = new SelectList(managePatientBL.GetInstructionsList(), "ID", "Name");
            model.MorningList = new SelectList(managePatientBL.GetMedicineTimeList("Morning"), "ID", "Name");
            model.NoonList = new SelectList(managePatientBL.GetMedicineTimeList("Noon"), "ID", "Name");
            model.EveningList = new SelectList(managePatientBL.GetMedicineTimeList("Evening"), "ID", "Name");
            model.NightList = new SelectList(managePatientBL.GetMedicineTimeList("Night"), "ID", "Name");
            model.UnitList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                                new SelectListItem { Text = "", Value = "0"}

                                        }, "Value", "Text");
            model.AppointmentScheduleItemID = AppointmentScheduleItemID;
            model.ModeOfAdministrationList = new SelectList(submasterBL.GetModeOfAdministration(), "ID", "Name");
            model.TestDate = General.FormatDate(DateTime.Now);
            model.IsInPatient = managePatientBL.IsInPatient(ID);
            model.IsReferedIP = IsReferedIP;
            model.ReviewID = ReviewID;
            return View(model);
        }
        public ActionResult CreateV5(int ID, int AppointmentScheduleItemID, int OPID, bool IsCompleted, bool IsReferedIP, int ReviewID, bool IswalkIn)
        {
            PatientDiagnosisModel model = managePatientBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                PatientCode = a.PatientCode,
                Age = a.Age,
                //DOB = General.FormatDate(a.DOB),
                DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                AdmissionDate = General.FormatDate(a.AdmissionDate),
                RoomName = a.RoomName,
                IPDoctor = a.IPDoctor,
                Doctor = a.Doctor,
                Place = a.Place,
                Mobile = a.Mobile,
                Month = a.Month,
                Gender = a.Gender
            }).First();
            model.ExaminationItems = managePatientBL.GetExaminationList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent


            }).ToList();
            model.CaseSheetItems = managePatientBL.GetCaseSheetList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked
            }).ToList();
            model.BaseLineItems = managePatientBL.GetBaseLineInformationList(ID, OPID).Select(m => new BaseLineModel()
            {

                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked,

            }).ToList();

            model.RogaPareekshaItems = managePatientBL.GetRogaPareekshaList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked,

            }).ToList();

            model.RogaNirnayamItems = managePatientBL.GetRogaNirnayamList(ID, OPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked,

            }).ToList();
            var obj = screeningBL.IsPatientExists(OPID);
            model.IsExists = obj.IsExists;
            if (model.IsExists == true)
            {
                model.QuestionnaireItems = screeningBL.GetQuestionnaireAndAnswers(ID, OPID).Select(m => new QuestionnaireModel()
                {
                    ID = m.ID,
                    Name = m.Name,
                    Type = m.Type,
                    GroupName = m.GroupName,
                    Description = m.Description,
                    GeneralOptionID = m.GeneralOptionID,
                    Text = m.Text,
                    Value = m.GeneralOptionID,
                    Diagnosis = m.Diagnosis,
                    DiagnosisID = m.Value,
                    IsParent = m.IsParent,
                    IsChecked = m.IsChecked,

                }).ToList();

            }
            else
            {
                model.QuestionnaireItems = screeningBL.GetQuestionnaireList(ID, OPID).Select(m => new QuestionnaireModel()
                {
                    ID = m.ID,
                    Name = m.Name,
                    Type = m.Type,
                    GroupName = m.GroupName,
                    Description = m.Description,
                    GeneralOptionID = m.GeneralOptionID,
                    Text = m.Text,
                    Value = m.GeneralOptionID,
                    Diagnosis = m.Diagnosis,
                    DiagnosisID = m.Value,
                    IsParent = m.IsParent,
                    IsChecked = m.IsChecked,

                }).ToList();
            }



            model.AppointmentProcessID = OPID;
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.IsCompleted = IsCompleted;
            model.IswalkIn = IswalkIn;
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
            model.TherapistList = new SelectList(managePatientBL.GetTherapistDetails(), "ID", "Name");
            model.TreatmentRoomList = new SelectList(treatmentRoomBL.GetTreatmentRoomDetailsList(), "ID", "Name");
            model.ReportDate = General.FormatDate(DateTime.Now);
            model.StartDateMed = General.FormatDate(DateTime.Now);
            model.TreatmentStartDate = General.FormatDate(DateTime.Now);
            model.DateList = managePatientBL.GetDateListByID(ID, OPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.TreatmentNoList = new SelectList(managePatientBL.GetTreatmentNumberList(), "ID", "Text");
            model.InstructionsList = new SelectList(managePatientBL.GetInstructionsList(), "ID", "Name");
            model.MorningList = new SelectList(managePatientBL.GetMedicineTimeList("Morning"), "ID", "Name");
            model.NoonList = new SelectList(managePatientBL.GetMedicineTimeList("Noon"), "ID", "Name");
            model.EveningList = new SelectList(managePatientBL.GetMedicineTimeList("Evening"), "ID", "Name");
            model.NightList = new SelectList(managePatientBL.GetMedicineTimeList("Night"), "ID", "Name");
            model.UnitList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                                new SelectListItem { Text = "", Value = "0"}

                                        }, "Value", "Text");
            model.AppointmentScheduleItemID = AppointmentScheduleItemID;
            model.ModeOfAdministrationList = new SelectList(submasterBL.GetModeOfAdministration(), "ID", "Name");
            model.TestDate = General.FormatDate(DateTime.Now);
            model.XrayDate = General.FormatDate(DateTime.Now);
            model.IsInPatient = managePatientBL.IsInPatient(ID);
            model.IsReferedIP = IsReferedIP;
            model.ReviewID = ReviewID;
            return View(model);
        }
        public ActionResult IndexV2()
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            return View(model);
        }
        public ActionResult IndexV5()
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.DepartmentList = new SelectList(departmentBL.GetPatientDepartment(), "ID", "Name");
            return View(model);
        }
        public PartialViewResult CaseSheetV2(int PatientID, string Date, int AppointmentProcessID, int ReviewID = 0)
        {
            DateTime FromDate = General.ToDateTime(Date);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.CaseSheetItems = managePatientBL.GetCaseSheetList(PatientID, AppointmentProcessID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked
            }).ToList();
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.CaseSheetItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
          
            return PartialView(model);
        }
        public ActionResult CaseSheetItem(int PatientID,int AppointmentProcessID)
        {            
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.CaseSheetItems = managePatientBL.GetCaseSheetList(PatientID, AppointmentProcessID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID,
                Diagnosis = m.Diagnosis,
                DiagnosisID = m.Value,
                IsParent = m.IsParent,
                IsChecked = m.IsChecked
            }).ToList();
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.CaseSheetItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }

            return PartialView("~/Areas/AHCMS/Views/PatientDiagnosis/CaseSheetV2.cshtml", model.CaseSheetItems );
        }

        [HttpPost]
        public ActionResult SaveV2(PatientDiagnosisModel model)
        {
            try
            {
                PatientDiagnosisBO diagnosis = new PatientDiagnosisBO()
                {
                    PatientID = model.PatientID,
                    Date = General.ToDateTime(model.Date),
                    Remark = model.Remark,
                    AppointmentType = model.AppointmentType,
                    VisitType = model.VisitType,
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    IsCompleted = model.IsCompleted,
                    IsReferedIP = model.IsReferedIP,
                    AppointmentProcessID = model.AppointmentProcessID,
                    IswalkIn = model.IswalkIn,
                    ParentID = model.ParentID,
                    ReviewID = model.ReviewID
                };
                if (model.NextVisitDate != null)
                {
                    diagnosis.NextVisitDate = General.ToDateTime(model.NextVisitDate);
                }

                List<VitalChartBO> VitalChartItems = new List<VitalChartBO>();
                if (model.VitalChartItems != null)
                {
                    VitalChartBO Item;
                    foreach (var item in model.VitalChartItems)
                    {
                        Item = new VitalChartBO()
                        {
                            BP = item.BP,
                            Pulse = item.Pulse,
                            Temperature = item.Temperature,
                            Unit = item.Unit,
                            BMI = item.BMI,
                            RespiratoryRate = item.RespiratoryRate,
                            HR = item.HR,
                            RR = item.RR,
                            Weight = item.Weight,
                            Height = item.Height,
                            Others = item.Others
                        };
                        VitalChartItems.Add(Item);
                    }
                }

                List<ExaminationBO> Items = new List<ExaminationBO>();
                if (model.ExaminationItems != null)
                {
                    ExaminationBO Item;

                    foreach (var item in model.ExaminationItems)
                    {
                        Item = new ExaminationBO()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description
                        };
                        Items.Add(Item);
                    }
                }

                List<BaseLineBO> BaseLineItems = new List<BaseLineBO>();
                if (model.BaseLineItems != null)
                {
                    BaseLineBO Item;

                    foreach (var item in model.BaseLineItems)
                    {
                        Item = new BaseLineBO()
                        {
                            Name = item.Name,
                            Description = item.Description,

                        };
                        BaseLineItems.Add(Item);
                    }
                    if (model.OtherConditionsItems != null)
                    {
                        foreach (var item in model.OtherConditionsItems)
                        {

                            Item = new BaseLineBO()
                            {

                                Name = item.Name,
                                Description = item.Description,

                            };
                            BaseLineItems.Add(Item);


                        }
                    }
                }


                List<ReportBO> ReportItems = new List<ReportBO>();
                if (model.ReportItems != null)
                {
                    ReportBO Item;

                    foreach (var item in model.ReportItems)
                    {
                        Item = new ReportBO()
                        {
                            DocumentID = item.DocumentID,
                            Name = item.Name,
                            Date = General.ToDateTime(item.Date),
                            Description = item.Description
                        };
                        ReportItems.Add(Item);
                    }
                }

                List<LabtestsBO> LabTestItems = new List<LabtestsBO>();
                if (model.LabTestItems != null)
                {
                    LabtestsBO Item;

                    foreach (var item in model.LabTestItems)
                    {
                        Item = new LabtestsBO()
                        {
                            ID = item.ID,
                            TestDate = General.ToDateTime(item.TestDate),
                            LabTestID = item.LabTestID
                        };
                        LabTestItems.Add(Item);
                    }
                }

                List<TreatmentBO> Treatments = new List<TreatmentBO>();
                if (model.TreatmentItems != null)
                {
                    TreatmentBO Item;

                    foreach (var item in model.TreatmentItems)
                    {
                        Item = new TreatmentBO()
                        {
                            TreatmentID = item.TreatmentID,
                            TherapistID = item.TherapistID,
                            TreatmentRoomID = item.TreatmentRoomID,
                            Instructions = item.Instructions,
                            StartDate = General.ToDateTime(item.StartDate),
                            EndDate = General.ToDateTime(item.EndDate),
                            TreatmentNo = item.TreatmentNo,
                            PatientTreatmentID = item.PatientTreatmentID,
                            MorningTimeID = item.MorningTime,
                            NoonTimeID = item.NoonTime,
                            EveningTimeID = item.EveningTime,
                            NightTimeID = item.NightTime,
                            IsMorning = item.IsMorning,
                            IsNoon = item.IsNoon,
                            Isevening = item.Isevening,
                            IsNight = item.IsNight
                        };
                        Treatments.Add(Item);
                    }
                }

                List<TreatmentItemBO> TreatmentMedicines = new List<TreatmentItemBO>();
                if (model.TreatmentMedicines != null)
                {
                    TreatmentItemBO Item;

                    foreach (var item in model.TreatmentMedicines)
                    {
                        Item = new TreatmentItemBO()
                        {
                            MedicineID = item.MedicineID,
                            Medicine = item.Medicine,
                            TreatmentID = item.TreatmentID,
                            StandardMedicineQty = item.StandardMedicineQty,
                            MedicineUnitID=item.MedicineUnitID
                        };
                        TreatmentMedicines.Add(Item);
                    }
                }

                List<MedicineBO> MedicinesList = new List<MedicineBO>();
                if (model.Medicines != null)
                {
                    MedicineBO Item;

                    foreach (var item in model.Medicines)
                    {
                        Item = new MedicineBO()
                        {
                            MedicineID = item.MedicinesID,
                            UnitID = item.UnitID,
                            Quantity = item.Quantity,
                            GroupID = item.GroupID,
                            PatientMedicinesID = item.PatientMedicinesID
                        };
                        MedicinesList.Add(Item);
                    }
                }

                List<MedicineItemBO> MedicinesItemsList = new List<MedicineItemBO>();
                if (model.MedicineItems != null)
                {
                    MedicineItemBO Item;

                    foreach (var item in model.MedicineItems)
                    {
                        Item = new MedicineItemBO()
                        {
                            MorningTime = item.MorningTime,
                            NoonTime = item.NoonTime,
                            EveningTime = item.EveningTime,
                            NightTime = item.NightTime,
                            StartDate = General.ToDateTime(item.StartDate),
                            EndDate = General.ToDateTime(item.EndDate),
                            InstructionsID = item.InstructionsID,
                            IsMorning = item.IsMorning,
                            Isevening = item.Isevening,
                            IsNoon = item.IsNoon,
                            IsNight = item.IsNight,
                            IsMultipleTimes = item.IsMultipleTimes,
                            NoofDays = item.NoofDays,
                            IsEmptyStomach = item.IsEmptyStomach,
                            IsAfterFood = item.IsAfterFood,
                            IsBeforeFood = item.IsBeforeFood,
                            Description = item.Description,
                            GroupID = item.GroupID,
                            Frequency = item.Frequency,
                            ModeOfAdministrationID = item.ModeOfAdministrationID,
                            PatientMedicineID = item.PatientMedicineID,
                            IsMiddleOfFood = item.IsMiddleOfFood,
                            IsWithFood = item.IsWithFood,
                            MedicineInstruction = item.MedicineInstruction,
                            QuantityInstruction = item.QuantityInstruction
                        };
                        MedicinesItemsList.Add(Item);
                    }
                }
                List<XrayBO> XrayItem = new List<XrayBO>();
                if (model.XrayItems != null)
                {
                    XrayBO Item;

                    foreach (var item in model.XrayItems)
                    {
                        Item = new XrayBO()
                        {
                            ID = item.ID,
                            XrayDate = General.ToDateTime(item.XrayDate),
                            XrayID = item.XrayID
                        };
                        XrayItem.Add(Item);
                    }
                }

                List<DoctorListBO> DoctorList = new List<DoctorListBO>();
                if (model.DoctorList != null)
                {
                    DoctorListBO Item;

                    foreach (var item in model.DoctorList)
                    {
                        Item = new DoctorListBO()
                        {
                            DoctorName = item.DoctorName,
                            DoctorNameID = item.DoctorNameID,
                        };
                        DoctorList.Add(Item);
                    }
                }

                List<BaseLineBO> RogaPareekshaItems = new List<BaseLineBO>();
                if (model.RogaPareekshaItems != null)
                {
                    BaseLineBO Item;

                    foreach (var item in model.RogaPareekshaItems)
                    {
                        Item = new BaseLineBO()
                        {

                            Name = item.Name,
                            Description = item.Description,

                        };
                        RogaPareekshaItems.Add(Item);
                    }
                }

                List<ExaminationBO> RogaNirnayamItem = new List<ExaminationBO>();
                if (model.RogaNirnayamItems != null)
                {
                    ExaminationBO RogaNirnayam;

                    foreach (var item in model.RogaNirnayamItems)
                    {
                        RogaNirnayam = new ExaminationBO()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description
                        };
                        RogaNirnayamItem.Add(RogaNirnayam);
                    }
                }

                List<BaseLineBO> CaseSheetItems = new List<BaseLineBO>();
                if (model.CaseSheetItems != null)
                {
                    BaseLineBO Item;

                    foreach (var item in model.CaseSheetItems)
                    {
                        Item = new BaseLineBO()
                        {

                            Name = item.Name,
                            Description = item.Description,

                        };
                        CaseSheetItems.Add(Item);
                    }
                    if (model.AssociatedConditionsItems != null)
                    {
                        foreach (var item in model.AssociatedConditionsItems)
                        {

                            Item = new BaseLineBO()
                            {

                                Name = item.Name,
                                Description = item.Description,

                            };
                            CaseSheetItems.Add(Item);
                        }
                    }
                }

                List<QuestionnaireBO> QuestionnaireItems = new List<QuestionnaireBO>();
                if (model.QuestionnaireItems != null)
                {
                    QuestionnaireBO Item;

                    foreach (var item in model.QuestionnaireItems)
                    {
                        Item = new QuestionnaireBO()
                        {
                            QuestionID = item.QuestionID,
                            Question = item.Question,
                            Answer = item.Answer
                        };
                        QuestionnaireItems.Add(Item);
                    }
                }
                managePatientBL.SaveV2(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem, QuestionnaireItems);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "PatientDiagnosis", "Save", model.PatientID, e);
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SaveV5(PatientDiagnosisModel model)
        {
            try
            {
                PatientDiagnosisBO diagnosis = new PatientDiagnosisBO()
                {
                    PatientID = model.PatientID,
                    Date = General.ToDateTime(model.Date),
                    Remark = model.Remark,
                    AppointmentType = model.AppointmentType,
                    VisitType = model.VisitType,
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    IsCompleted = model.IsCompleted,
                    IsReferedIP = model.IsReferedIP,
                    AppointmentProcessID = model.AppointmentProcessID,
                    IswalkIn = model.IswalkIn,
                    ParentID = model.ParentID,
                    ReviewID = model.ReviewID
                };
                if (model.NextVisitDate != null)
                {
                    diagnosis.NextVisitDate = General.ToDateTime(model.NextVisitDate);
                }

                List<VitalChartBO> VitalChartItems = new List<VitalChartBO>();
                if (model.VitalChartItems != null)
                {
                    VitalChartBO Item;
                    foreach (var item in model.VitalChartItems)
                    {
                        Item = new VitalChartBO()
                        {
                            BP = item.BP,
                            Pulse = item.Pulse,
                            Temperature = item.Temperature,
                            Unit = item.Unit,
                            BMI = item.BMI,
                            RespiratoryRate = item.RespiratoryRate,
                            HR = item.HR,
                            RR = item.RR,
                            Weight = item.Weight,
                            Height = item.Height,
                            Others = item.Others
                        };
                        VitalChartItems.Add(Item);
                    }
                }

                List<ExaminationBO> Items = new List<ExaminationBO>();
                if (model.ExaminationItems != null)
                {
                    ExaminationBO Item;

                    foreach (var item in model.ExaminationItems)
                    {
                        Item = new ExaminationBO()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description
                        };
                        Items.Add(Item);
                    }
                }

                 List<ExaminationBO> NewItems = new List<ExaminationBO>();
                if (model.ExaminationNewItems != null)
                {
                    ExaminationBO Item;

                    foreach (var item in model.ExaminationNewItems)
                    {
                        Item = new ExaminationBO()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description
                        };
                        NewItems.Add(Item);
                    }
                }
                List<BaseLineBO> BaseLineItems = new List<BaseLineBO>();
                if (model.BaseLineItems != null)
                {
                    BaseLineBO Item;

                    foreach (var item in model.BaseLineItems)
                    {
                        Item = new BaseLineBO()
                        {
                            Name = item.Name,
                            Description = item.Description,

                        };
                        BaseLineItems.Add(Item);
                    }
                    if (model.OtherConditionsItems != null)
                    {
                        foreach (var item in model.OtherConditionsItems)
                        {

                            Item = new BaseLineBO()
                            {

                                Name = item.Name,
                                Description = item.Description,

                            };
                            BaseLineItems.Add(Item);


                        }
                    }
                }


                List<ReportBO> ReportItems = new List<ReportBO>();
                if (model.ReportItems != null)
                {
                    ReportBO Item;

                    foreach (var item in model.ReportItems)
                    {
                        Item = new ReportBO()
                        {
                            DocumentID = item.DocumentID,
                            Name = item.Name,
                            Date = General.ToDateTime(item.Date),
                            Description = item.Description,
                            IsBeforeAdmission=item.IsBeforeAdmission
                        };
                        ReportItems.Add(Item);
                    }
                }

                List<LabtestsBO> LabTestItems = new List<LabtestsBO>();
                if (model.LabTestItems != null)
                {
                    LabtestsBO Item;

                    foreach (var item in model.LabTestItems)
                    {
                        Item = new LabtestsBO()
                        {
                            ID = item.ID,
                            TestDate = General.ToDateTime(item.TestDate),
                            LabTestID = item.LabTestID
                        };
                        LabTestItems.Add(Item);
                    }
                }

                List<TreatmentBO> Treatments = new List<TreatmentBO>();
                if (model.TreatmentItems != null)
                {
                    TreatmentBO Item;

                    foreach (var item in model.TreatmentItems)
                    {
                        Item = new TreatmentBO()
                        {
                            TreatmentID = item.TreatmentID,
                            TherapistID = item.TherapistID,
                            TreatmentRoomID = item.TreatmentRoomID,
                            Instructions = item.Instructions,
                            StartDate = General.ToDateTime(item.StartDate),
                            EndDate = General.ToDateTime(item.EndDate),
                            TreatmentNo = item.TreatmentNo,
                            PatientTreatmentID = item.PatientTreatmentID,
                            MorningTimeID = item.MorningTime,
                            NoonTimeID = item.NoonTime,
                            EveningTimeID = item.EveningTime,
                            NightTimeID = item.NightTime,
                            IsMorning = item.IsMorning,
                            IsNoon = item.IsNoon,
                            Isevening = item.Isevening,
                            IsNight = item.IsNight
                        };
                        Treatments.Add(Item);
                    }
                }

                List<TreatmentItemBO> TreatmentMedicines = new List<TreatmentItemBO>();
                if (model.TreatmentMedicines != null)
                {
                    TreatmentItemBO Item;

                    foreach (var item in model.TreatmentMedicines)
                    {
                        Item = new TreatmentItemBO()
                        {
                            MedicineID = item.MedicineID,
                            Medicine = item.Medicine,
                            TreatmentID = item.TreatmentID,
                            StandardMedicineQty = item.StandardMedicineQty,
                            MedicineUnitID = item.MedicineUnitID
                        };
                        TreatmentMedicines.Add(Item);
                    }
                }

                List<MedicineBO> MedicinesList = new List<MedicineBO>();
                if (model.Medicines != null)
                {
                    MedicineBO Item;

                    foreach (var item in model.Medicines)
                    {
                        Item = new MedicineBO()
                        {
                            MedicineID = item.MedicinesID,
                            UnitID = item.UnitID,
                            Quantity = item.Quantity,
                            GroupID = item.GroupID,
                            PatientMedicinesID = item.PatientMedicinesID
                        };
                        MedicinesList.Add(Item);
                    }
                }

                List<MedicineItemBO> MedicinesItemsList = new List<MedicineItemBO>();
                if (model.MedicineItems != null)
                {
                    MedicineItemBO Item;

                    foreach (var item in model.MedicineItems)
                    {
                        Item = new MedicineItemBO()
                        {
                            MorningTime = item.MorningTime,
                            NoonTime = item.NoonTime,
                            EveningTime = item.EveningTime,
                            NightTime = item.NightTime,
                            StartDate = General.ToDateTime(item.StartDate),
                            EndDate = General.ToDateTime(item.EndDate),
                            InstructionsID = item.InstructionsID,
                            IsMorning = item.IsMorning,
                            Isevening = item.Isevening,
                            IsNoon = item.IsNoon,
                            IsNight = item.IsNight,
                            IsMultipleTimes = item.IsMultipleTimes,
                            NoofDays = item.NoofDays,
                            IsEmptyStomach = item.IsEmptyStomach,
                            IsAfterFood = item.IsAfterFood,
                            IsBeforeFood = item.IsBeforeFood,
                            Description = item.Description,
                            GroupID = item.GroupID,
                            Frequency = item.Frequency,
                            ModeOfAdministrationID = item.ModeOfAdministrationID,
                            PatientMedicineID = item.PatientMedicineID,
                            IsMiddleOfFood = item.IsMiddleOfFood,
                            IsWithFood = item.IsWithFood,
                            MedicineInstruction = item.MedicineInstruction,
                            QuantityInstruction = item.QuantityInstruction
                        };
                        MedicinesItemsList.Add(Item);
                    }
                }
                List<XrayBO> XrayItem = new List<XrayBO>();
                if (model.XrayItems != null)
                {
                    XrayBO Item;

                    foreach (var item in model.XrayItems)
                    {
                        Item = new XrayBO()
                        {
                            ID = item.ID,
                            XrayDate = General.ToDateTime(item.XrayDate),
                            XrayID = item.XrayID
                        };
                        XrayItem.Add(Item);
                    }
                }

                List<DoctorListBO> DoctorList = new List<DoctorListBO>();
                if (model.DoctorList != null)
                {
                    DoctorListBO Item;

                    foreach (var item in model.DoctorList)
                    {
                        Item = new DoctorListBO()
                        {
                            DoctorName = item.DoctorName,
                            DoctorNameID = item.DoctorNameID,
                        };
                        DoctorList.Add(Item);
                    }
                }

                List<BaseLineBO> RogaPareekshaItems = new List<BaseLineBO>();
                if (model.RogaPareekshaItems != null)
                {
                    BaseLineBO Item;

                    foreach (var item in model.RogaPareekshaItems)
                    {
                        Item = new BaseLineBO()
                        {

                            Name = item.Name,
                            Description = item.Description,

                        };
                        RogaPareekshaItems.Add(Item);
                    }
                }

                List<ExaminationBO> RogaNirnayamItem = new List<ExaminationBO>();
                if (model.RogaNirnayamItems != null)
                {
                    ExaminationBO RogaNirnayam;

                    foreach (var item in model.RogaNirnayamItems)
                    {
                        RogaNirnayam = new ExaminationBO()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description
                        };
                        RogaNirnayamItem.Add(RogaNirnayam);
                    }
                }

                List<BaseLineBO> CaseSheetItems = new List<BaseLineBO>();
                if (model.CaseSheetItems != null)
                {
                    BaseLineBO Item;

                    foreach (var item in model.CaseSheetItems)
                    {
                        Item = new BaseLineBO()
                        {

                            Name = item.Name,
                            Description = item.Description,

                        };
                        CaseSheetItems.Add(Item);
                    }
                    if (model.AssociatedConditionsItems != null)
                    {
                        foreach (var item in model.AssociatedConditionsItems)
                        {

                            Item = new BaseLineBO()
                            {

                                Name = item.Name,
                                Description = item.Description,

                            };
                            CaseSheetItems.Add(Item);
                        }
                    }
                }

                List<QuestionnaireBO> QuestionnaireItems = new List<QuestionnaireBO>();
                if (model.QuestionnaireItems != null)
                {
                    QuestionnaireBO Item;

                    foreach (var item in model.QuestionnaireItems)
                    {
                        Item = new QuestionnaireBO()
                        {
                            QuestionID = item.QuestionID,
                            Question = item.Question,
                            Answer = item.Answer
                        };
                        QuestionnaireItems.Add(Item);
                    }
                }
                managePatientBL.SaveV5(diagnosis, VitalChartItems, Items,NewItems, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem, QuestionnaireItems);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "PatientDiagnosis", "Savev5", model.PatientID, e);
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllMedicinesbyProductionGroup(int ProductionGroupID)
        {
            try
            {
                List<MedicineBO> MedicineStockList = managePatientBL.GetAllMedicinesbyProductionGroup(ProductionGroupID);

                return Json(new { Status = "success", Data = MedicineStockList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCategoryWiseLabItems(int[] LabTestCategoryID)
        {
            try
            {
                List<LabTestItemBO> TestList = managePatientBL.GetCategoryWiseLabItems(LabTestCategoryID);

                return Json(new { Status = "success", Data = TestList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult CancelTreatment(int PatientTreatmentID, int TreatmentID,string EndDate)
        {
            DateTime Date = General.ToDateTime(EndDate);
            try
            {
                bool Treatment = managePatientBL.CancelPaitentTreatment(PatientTreatmentID, TreatmentID, Date);

                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDashaVidhaPareekhsa(int PatientID,int AppointmentProcessID)
        {
            try
            {
                List<ExaminationBO> DashaVidhaPareekhsa = managePatientBL.GetDashaVidhaPareekhsalist(PatientID, AppointmentProcessID);

                return Json(new { Status = "success", Data = DashaVidhaPareekhsa }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
