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
    public class IPCaseSheetController : Controller
    {
        private IIPCaseSheetContract ipcasesheetBL;
        private IGeneralContract generalBL;
        private ITreatmentRoomContract treatmentRoomBL;
        private IFileContract fileBL;
        private ISubmasterContract submasterBL;
        private IPatientDiagnosisContract managePatientBL;
        private IDepartmentContract departmentBL;

        public IPCaseSheetController()
        {
            ipcasesheetBL = new IPCaseSheetBL();
            generalBL = new GeneralBL();
            treatmentRoomBL = new TreatmentRoomBL();
            fileBL = new FileBL();
            submasterBL = new SubmasterBL();
            managePatientBL = new PatientDiagnosisBL();
            departmentBL = new DepartmentBL();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int ID, int IPID, int OPID, bool IsDischarged, bool IsDischargeAdviced,int DischargeSummaryID)
        {
            PatientDiagnosisModel model = ipcasesheetBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                PatientCode = a.PatientCode,
                Age = a.Age,
                DOB = a.DOB == General.ToDateTime("01-01-1900") ? "" : General.FormatDate((DateTime)a.DOB),
                Place=a.Place,
                Mobile=a.Mobile,
                Month=a.Month
            }).First();
            model.ExaminationItems = ipcasesheetBL.GetExaminationList(ID, IPID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                Value = m.GeneralOptionID

            }).ToList();
            model.BaseLineItems = ipcasesheetBL.GetIPBaseLineInformationDetails(ID, IPID).Select(m => new BaseLineModel()
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
                IsChecked = m.IsChecked
            }).ToList();

            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(ipcasesheetBL.GetOptionList(item.Name), "ID", "Name")));
            }
            model.TherapistList = new SelectList(ipcasesheetBL.GetTherapistDetails(), "ID", "Name");
            model.TreatmentRoomList = new SelectList(treatmentRoomBL.GetTreatmentRoomDetailsList(), "ID", "Name");
            model.ReportDate = General.FormatDate(DateTime.Now);
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.NextVisitDate = General.FormatDate(DateTime.Now);
            model.StartDate = General.FormatDate(DateTime.Now);
            model.Doctor = GeneralBO.EmployeeName;          
            model.TreatmentStartDate = General.FormatDate(DateTime.Now);
            model.DateList = ipcasesheetBL.GetDateListByID(IPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.TreatmentNoList = new SelectList(ipcasesheetBL.GetTreatmentNumberList(), "ID", "Text");
            model.InstructionsList = new SelectList(ipcasesheetBL.GetInstructionsList(), "ID", "Name");
            model.MorningList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Morning"), "ID", "Name");
            model.NoonList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Noon"), "ID", "Name");
            model.EveningList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Evening"), "ID", "Name");
            model.NightList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Night"), "ID", "Name");
            model.UnitList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                                new SelectListItem { Text = "", Value = "0"}

                                        }, "Value", "Text");
            model.IPID = IPID;
            model.AppointmentProcessID = OPID;
            model.VitalChartDate = General.FormatDate(DateTime.Now);
            model.RoundsDate = General.FormatDate(DateTime.Now);
            model.PhysioFromDate = General.FormatDate(DateTime.Now);
            model.TestDate = General.FormatDate(DateTime.Now);
            model.IsDischargeAdvice = IsDischargeAdviced;
            model.IsDischarged = IsDischarged;
            model.DischargeSummaryID = DischargeSummaryID;
            model.ModeOfAdministrationList = new SelectList(submasterBL.GetModeOfAdministration(), "ID", "Name");
            return View(model);
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
                    AppointmentProcessID = model.AppointmentProcessID,
                    IPID = model.IPID,
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    IsDischargeAdvice = model.IsDischargeAdvice,
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
                            Date = General.ToDateTime(item.Date),
                            BP = item.BP,
                            Pulse = item.Pulse,
                            Temperature = item.Temperature,
                            HR = item.HR,
                            RR = item.RR,
                            Weight = item.Weight,
                            Height = item.Height,
                            Others = item.Others,
                            Time = item.Time
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



                List<DischargeSummaryBO> DischargeSummary = new List<DischargeSummaryBO>();
                if (model.DischargeSummary != null)
                {
                    DischargeSummaryBO Item;

                    foreach (var item in model.DischargeSummary)
                    {
                        Item = new DischargeSummaryBO()
                        {
                            CourseInTheHospital = item.CourseInTheHospital,
                            ConditionAtDischarge = item.ConditionAtDischarge,
                            DietAdvice = item.DietAdvice
                        };
                        DischargeSummary.Add(Item);
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
                            ID = item.ID,
                            DocumentID = item.DocumentID,
                            Name = item.Name,
                            Date = General.ToDateTime(item.Date),
                            Description = item.Description
                        };
                        ReportItems.Add(Item);
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
                            TreatmentMedicineUnitID = item.TreatmentMedicineUnitID
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
                            PatientMedicinesID=item.PatientMedicinesID
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
                            NoofDays = item.NoofDays,
                            IsEmptyStomach = item.IsEmptyStomach,
                            IsAfterFood = item.IsAfterFood,
                            IsBeforeFood = item.IsBeforeFood,
                            Description = item.Description,
                            GroupID = item.GroupID,
                            Frequency = item.Frequency,
                            PatientMedicineID = item.PatientMedicineID,
                            ModeOfAdministrationID = item.ModeOfAdministrationID,
                            IsMiddleOfFood = item.IsMiddleOfFood,
                            IsWithFood = item.IsWithFood,
                            MedicineInstruction = item.MedicineInstruction,
                            QuantityInstruction = item.QuantityInstruction,
                        };
                        MedicinesItemsList.Add(Item);
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

                List<RoundsBO> RoundsList = new List<RoundsBO>();
                if (model.RoundsItems != null)
                {
                    RoundsBO Item;

                    foreach (var item in model.RoundsItems)
                    {
                        Item = new RoundsBO()
                        {
                            Remarks = item.Remarks,
                            RoundsDate = General.ToDateTime(item.RoundsDate),
                        };
                        RoundsList.Add(Item);
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
                            TestDate = General.ToDateTime(item.TestDate),
                            LabTestID = item.LabTestID,
                        };
                        LabTestItems.Add(Item);
                    }
                }

                List<PhysiotherapyBO> PhysiotherapyItems = new List<PhysiotherapyBO>();
                if (model.PhysiotherapyItems != null)
                {
                    PhysiotherapyBO Item;

                    foreach (var item in model.PhysiotherapyItems)
                    {
                        Item = new PhysiotherapyBO()
                        {
                            PhysiotherapyID = item.PhysiotherapyID,
                            StartDate = General.ToDateTime(item.StartDate),
                            EndDate = General.ToDateTime(item.EndDate),
                        };
                        PhysiotherapyItems.Add(Item);
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
                            XrayDate = General.ToDateTime(item.XrayDate),
                            XrayID = item.XrayID,
                            ID = item.ID
                        };
                        XrayItem.Add(Item);
                    }
                }

                List<MedicineBO> InternalMedicinesList = new List<MedicineBO>();
                if (model.InternalMedicine != null)
                {
                    MedicineBO Item;

                    foreach (var item in model.InternalMedicine)
                    {
                        Item = new MedicineBO()
                        {
                            MedicineID = item.MedicinesID,
                            UnitID = item.UnitID,
                            Quantity = item.Quantity,
                            GroupID = item.GroupID
                        };
                        InternalMedicinesList.Add(Item);
                    }
                }

                List<MedicineItemBO> InternalMedicinesItems = new List<MedicineItemBO>();
                if (model.InternalMedicineItems != null)
                {
                    MedicineItemBO Item;

                    foreach (var item in model.InternalMedicineItems)
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
                            NoofDays = item.NoofDays,
                            IsEmptyStomach = item.IsEmptyStomach,
                            IsAfterFood = item.IsAfterFood,
                            IsBeforeFood = item.IsBeforeFood,
                            Description = item.Description,
                            GroupID = item.GroupID,
                            Frequency = item.Frequency,
                            PatientMedicineID = item.PatientMedicineID,
                            MedicineInstruction = item.MedicineInstruction,
                            QuantityInstruction = item.QuantityInstruction,
                        };
                        InternalMedicinesItems.Add(Item);
                    }
                }


                ipcasesheetBL.Save(diagnosis, Items, BaseLineItems, VitalChartItems, ReportItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, LabTestItems, PhysiotherapyItems, XrayItem, RoundsList, InternalMedicinesList, InternalMedicinesItems, DischargeSummary, DoctorList);
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
        public PartialViewResult VitalChart(int PatientID, int IPID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.VitalChartItems = ipcasesheetBL.GetVitalChart(PatientID, IPID).Select(a => new VitalChartModel()
            {
                Date = General.FormatDate(a.Date),
                BP = a.BP,
                Pulse = a.Pulse,
                Temperature = a.Temperature,
                HR = a.HR,
                RR = a.RR,
                Height = a.Height,
                Weight = a.Weight,
                Others = a.Others,
                Time = a.Time
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult Rounds(int PatientID, int IPID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.RoundsItems = ipcasesheetBL.GetRoundsList(PatientID, IPID).Select(a => new RoundsModel()
            {
                RoundsDate = General.FormatDate(a.RoundsDate),
                Remarks = a.Remarks
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult RoundsV5(int PatientID, int IPID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.RoundsItems = ipcasesheetBL.GetRoundsListV5(PatientID, IPID).Select(a => new RoundsModel()
            {
                RoundsDate = General.FormatDate(a.RoundsDate),
                RoundsTime=a.RoundsTime,
                Remarks = a.Remarks,
                ClinicalNote=a.ClinicalNote,
                Doctor=a.Doctor
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult Treatment(int PatientID, int IPID, bool IsDischarged)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.TreatmentItems = ipcasesheetBL.GetTreatmentList(PatientID, IPID).Select(m => new TreatmentModel()
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
                PatientTreatmentID = m.PatientTreatmentID,
                IsDischarged = IsDischarged,
                Instructions = m.Instructions,
                DoctorName = m.DoctorName,
                IsMorning = m.IsMorning,
                MorningTime = m.MorningTime,
                IsNoon = m.IsNoon,
                NoonTime = m.NoonTime,
                Isevening = m.Isevening,
                EveningTime = m.EveningTime,
                IsNight = m.IsNight,
                NightTime = m.NightTime,
                MorningID = Convert.ToInt16(m.MorningTimeID),
                NoonID = Convert.ToInt16(m.NoonTimeID),
                EveningID = Convert.ToInt16(m.EveningTimeID),
                NightID = Convert.ToInt16(m.NightTimeID)
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult GetReport(int PatientID, int IPID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ReportItems = ipcasesheetBL.GetReportListByID(PatientID, IPID).Select(a => new ReportModel()
            {
                ID = a.ID,
                Name = a.Name,
                DocumentID = a.DocumentID,
                Description = a.Description,
                Date = General.FormatDate(a.Date),
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString())
            }).ToList();

            return PartialView(model);
        }
        public PartialViewResult GetReportV5(int PatientID, int IPID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ReportItems = ipcasesheetBL.GetReportListByIDV5(PatientID, IPID).Select(a => new ReportModel()
            {
                ID = a.ID,
                Name = a.Name,
                DocumentID = a.DocumentID,
                Description = a.Description,
                Date = General.FormatDate(a.Date),
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString()),
                IsBeforeAdmission=a.IsBeforeAdmission
            }).ToList();

            return PartialView(model);
        }
        public JsonResult GetTreatmentMedicineList(int PatientID, int IPID)
        {
            try
            {
                List<TreatmentItemBO> TreatmentItems = ipcasesheetBL.GetTreatmentMedicineListByID(PatientID, IPID);

                return Json(new { Status = "success", Data = TreatmentItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetMedicinesList(int PatientID, int IPID)
        {
            try
            {
                List<MedicineBO> MedicineList = ipcasesheetBL.GetMedicinesDetails(0,PatientID, IPID);

                return Json(new { Status = "success", Data = MedicineList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetExternalMedicinesList(int DischargeSumaryID,int PatientID, int IPID)
        {
            try
            {
                List<MedicineBO> MedicineList = ipcasesheetBL.GetMedicinesDetails(DischargeSumaryID,PatientID, IPID);

                return Json(new { Status = "success", Data = MedicineList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetMedicinesItemsList(int PatientID, int IPID)
        {
            try
            {
                PatientDiagnosisModel model = new PatientDiagnosisModel();
                List<MedicineItemBO> MedicineList = ipcasesheetBL.GetMedicinesItemsList(0,PatientID, IPID);
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
                        PatientMedicineID = a.GroupID,
                        ModeOfAdministrationID = a.ModeOfAdministrationID,
                        IsWithFood = a.IsWithFood,
                        IsMiddleOfFood = a.IsMiddleOfFood,
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
        public JsonResult GetExternalMedicinesItemsList(int DischargeSummaryID,int PatientID, int IPID)
        {
            try
            {
                PatientDiagnosisModel model = new PatientDiagnosisModel();
                List<MedicineItemBO> MedicineList = ipcasesheetBL.GetMedicinesItemsList(DischargeSummaryID,PatientID, IPID);
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
                        PatientMedicineID = a.GroupID
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
        public PartialViewResult Medicines(int PatientID, int IPID,bool IsDischarged)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.Medicines = ipcasesheetBL.GetMedicineListByID(0,PatientID, IPID).Select(a => new MedicineModel()
            {
                Medicine = a.Medicine,
                Prescription = a.Prescription,
                PrescriptionID = a.PrescriptionID,
                StartDate = General.FormatDate(a.StartDate),
                EndDate = General.FormatDate(a.EndDate),
                TransID = a.TransID,
                IsDischarged = IsDischarged,
                PatientMedicinesID = a.TransID,
                DoctorName = a.DoctorName,
                Qty=a.Qty,
                Status=a.Status,
                MedicineInstruction = a.MedicineInstruction,
                QuantityInstruction = a.QuantityInstruction,
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult ExternalMedicines(int DischargeSummaryID,int PatientID, int IPID, bool IsDischarged)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.InternalMedicine = ipcasesheetBL.GetMedicineListByID(DischargeSummaryID, PatientID, IPID).Select(a => new MedicineModel()
            {
                Medicine = a.Medicine,
                Prescription = a.Prescription,
                PrescriptionID = a.PrescriptionID,
                StartDate = General.FormatDate(a.StartDate),
                EndDate = General.FormatDate(a.EndDate),
                TransID = a.TransID,
                DischargeSummaryID = a.DischargeSummaryID,
                IsDischarged = IsDischarged,
                PatientMedicinesID = a.TransID,
                MedicineInstruction = a.MedicineInstruction,
                QuantityInstruction = a.QuantityInstruction,
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult Examination(int PatientID, string Date)
        {
            DateTime FromDate = General.ToDateTime(Date);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.ExaminationItems = ipcasesheetBL.GetExaminationByDate(PatientID, FromDate).Select(a => new ExaminationModel()
            {
                Value = a.Value,
                Description = a.Description,
                Text = a.Text,
                GroupName = a.GroupName,
                Name = a.Name
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult BaseLineInformation(int PatientID, string Date, int IPID, int ReviewID = 0)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.BaseLineItems = ipcasesheetBL.GetIPBaseLineInformationDetailsByID(PatientID, IPID).Select(m => new BaseLineModel()
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
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = ipcasesheetBL.GetIPPatientList(Type,CodeHint, NameHint, DateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "PatientDiagnosis", "GetIPPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLabTestAutoComplete(string Hint)
        {
            List<DropDownListBO> LabItems = ipcasesheetBL.GetLabTestAutoComplete(Hint);
            return Json(LabItems, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPhysiotherapyAutoComplete(string Hint)
        {
            List<DropDownListBO> PhysiotherapyItems = ipcasesheetBL.GetPhysiotherapyAutoComplete(Hint);
            return Json(PhysiotherapyItems, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetXrayAutoComplete(string Hint)
        {
            List<DropDownListBO> XrayItems = ipcasesheetBL.GetXrayAutoComplete(Hint);
            return Json(XrayItems, JsonRequestBehavior.AllowGet);
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
        public PartialViewResult LabTest(int PatientID, int IPID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.LabAndXrayItems = ipcasesheetBL.GetLabItems(PatientID, IPID).Select(a => new LabTestItemModel()
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
            }).ToList();
            return PartialView(model);
        }
        public PartialViewResult XrayTest(int PatientID, int IPID)
        {
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            model.XrayItemList = ipcasesheetBL.GetXrayItems(PatientID, IPID).Select(a => new XraysItemModel()
            {
                ID = a.ID,
                Date = General.FormatDate(a.Date),
                ItemID = a.ItemID,               
                ItemName = a.ItemName,
                Category = a.Category, 
                DocumentID = (int)a.DocumentID,
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString()),
            }).ToList();
            return PartialView(model);
        }
        public JsonResult DischargeSummary(int PatientID, int IPID)
        {
            try
            {
                var obj = ipcasesheetBL.GetDischargeSummary(PatientID, IPID);
                DischargeSummary model = new DischargeSummary();
                if (obj != null)
                {
                    model.CourseInTheHospital = obj.CourseInTheHospital;
                    model.DietAdvice = obj.DietAdvice;
                    model.ConditionAtDischarge = obj.ConditionAtDischarge;

                }
                return Json(new { Status = "success", Data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetDoctorList(int PatientID, string Date, int IPID)
        {
            try
            {
                DateTime FromDate = new DateTime();
                if (Date != null)
                {
                    FromDate = General.ToDateTime(Date);
                }
                List<DoctorListBO> DoctorList = ipcasesheetBL.GetDoctorList(PatientID, FromDate, IPID);

                return Json(new { Status = "success", Data = DoctorList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        //For Ilaj by priyanka
        public ActionResult CreateV2(int ID, int IPID, int OPID, bool IsDischarged, bool IsDischargeAdviced, int DischargeSummaryID)
        {
            PatientDiagnosisModel model = ipcasesheetBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
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
            model.ExaminationItems = ipcasesheetBL.GetIPExaminationList(ID, IPID).Select(m => new ExaminationModel()
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
            model.CaseSheetItems = ipcasesheetBL.GetCaseSheetList(ID, OPID).Select(m => new ExaminationModel()
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

            model.BaseLineItems = ipcasesheetBL.GetIPBaseLineInformationDetails(ID, IPID).Select(m => new BaseLineModel()
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
                IsChecked = m.IsChecked
            }).ToList();
            model.RogaPareekshaItems = ipcasesheetBL.GetRogaPareekshaList(ID, OPID).Select(m => new ExaminationModel()
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

            model.RogaNirnayamItems = ipcasesheetBL.GetRogaNirnayamList(ID, OPID).Select(m => new ExaminationModel()
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

            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(ipcasesheetBL.GetOptionList(item.Name), "ID", "Name")));
            }
            model.TherapistList = new SelectList(ipcasesheetBL.GetTherapistDetails(), "ID", "Name");
            model.TreatmentRoomList = new SelectList(treatmentRoomBL.GetTreatmentRoomDetailsList(), "ID", "Name");
            model.ReportDate = General.FormatDate(DateTime.Now);
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.NextVisitDate = General.FormatDate(DateTime.Now);
            model.StartDate = General.FormatDate(DateTime.Now);
            model.Doctor = GeneralBO.EmployeeName;
            model.TreatmentStartDate = General.FormatDate(DateTime.Now);
            model.DateList = ipcasesheetBL.GetDateListByID(IPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.TreatmentNoList = new SelectList(ipcasesheetBL.GetTreatmentNumberList(), "ID", "Text");
            model.InstructionsList = new SelectList(ipcasesheetBL.GetInstructionsList(), "ID", "Name");
            model.MorningList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Morning"), "ID", "Name");
            model.NoonList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Noon"), "ID", "Name");
            model.EveningList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Evening"), "ID", "Name");
            model.NightList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Night"), "ID", "Name");
            model.UnitList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                                new SelectListItem { Text = "", Value = "0"}

                                        }, "Value", "Text");
            model.IPID = IPID;
            model.AppointmentProcessID = OPID;
            model.VitalChartDate = General.FormatDate(DateTime.Now);
            model.RoundsDate = General.FormatDate(DateTime.Now);
            model.PhysioFromDate = General.FormatDate(DateTime.Now);
            model.TestDate = General.FormatDate(DateTime.Now);
            model.IsDischargeAdvice = IsDischargeAdviced;
            model.IsDischarged = IsDischarged;
            model.DischargeSummaryID = DischargeSummaryID;
            model.ModeOfAdministrationList = new SelectList(submasterBL.GetModeOfAdministration(), "ID", "Name");
            return View(model);
        }
        public ActionResult CreateV5(int ID, int IPID, int OPID, bool IsDischarged, bool IsDischargeAdviced, int DischargeSummaryID)
        {
            PatientDiagnosisModel model = ipcasesheetBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
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
            model.ExaminationItems = ipcasesheetBL.GetIPExaminationList(ID, IPID).Select(m => new ExaminationModel()
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
            model.CaseSheetItems = ipcasesheetBL.GetCaseSheetList(ID, OPID).Select(m => new ExaminationModel()
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

            model.BaseLineItems = ipcasesheetBL.GetIPBaseLineInformationDetails(ID, IPID).Select(m => new BaseLineModel()
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
                IsChecked = m.IsChecked
            }).ToList();
            model.RogaPareekshaItems = ipcasesheetBL.GetRogaPareekshaList(ID, OPID).Select(m => new ExaminationModel()
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

            model.RogaNirnayamItems = ipcasesheetBL.GetRogaNirnayamList(ID, OPID).Select(m => new ExaminationModel()
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

            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(ipcasesheetBL.GetOptionList(item.Name), "ID", "Name")));
            }
            model.TherapistList = new SelectList(ipcasesheetBL.GetTherapistDetails(), "ID", "Name");
            model.TreatmentRoomList = new SelectList(treatmentRoomBL.GetTreatmentRoomDetailsList(), "ID", "Name");
            model.ReportDate = General.FormatDate(DateTime.Now);
            model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            model.NextVisitDate = General.FormatDate(DateTime.Now);
            model.StartDate = General.FormatDate(DateTime.Now);
            model.Doctor = GeneralBO.EmployeeName;
            model.UserID = GeneralBO.CreatedUserID;
            model.TreatmentStartDate = General.FormatDate(DateTime.Now);
            model.DateList = ipcasesheetBL.GetDateListByID(IPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.TreatmentNoList = new SelectList(ipcasesheetBL.GetTreatmentNumberList(), "ID", "Text");
            model.InstructionsList = new SelectList(ipcasesheetBL.GetInstructionsList(), "ID", "Name");
            model.MorningList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Morning"), "ID", "Name");
            model.NoonList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Noon"), "ID", "Name");
            model.EveningList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Evening"), "ID", "Name");
            model.NightList = new SelectList(ipcasesheetBL.GetMedicineTimeList("Night"), "ID", "Name");
            model.UnitList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                                new SelectListItem { Text = "", Value = "0"}

                                        }, "Value", "Text");
            model.IPID = IPID;
            model.AppointmentProcessID = OPID;
            model.VitalChartDate = General.FormatDate(DateTime.Now);
            model.RoundsDate = General.FormatDate(DateTime.Now);
            model.PhysioFromDate = General.FormatDate(DateTime.Now);
            model.TestDate = General.FormatDate(DateTime.Now);
            model.XrayDate = General.FormatDate(DateTime.Now);
            model.IsDischargeAdvice = IsDischargeAdviced;
            model.IsDischarged = IsDischarged;
            model.DischargeSummaryID = DischargeSummaryID;
            model.ModeOfAdministrationList = new SelectList(submasterBL.GetModeOfAdministration(), "ID", "Name");
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
            model.ExaminationItems = managePatientBL.GetExamination(PatientID, FromDate, AppointmentProcessID, ReviewID).Select(m => new ExaminationModel()
            {
                ID = m.ID,
                Name = m.Name,
                Type = m.Type,
                GroupName = m.GroupName,
                Description = m.Description,
                GeneralOptionID = m.GeneralOptionID,
                Text = m.Text,
                IsParent = m.IsParent
            }).ToList();
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
            return PartialView(model);
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
                    ReviewID = model.ReviewID,
                    IPID = model.IPID,
                    IsDischargeAdvice = model.IsDischargeAdvice,
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
                            Date = General.ToDateTime(item.Date),
                            BP = item.BP,
                            Pulse = item.Pulse,
                            Temperature = item.Temperature,
                            HR = item.HR,
                            RR = item.RR,
                            Weight = item.Weight,
                            Height = item.Height,
                            Others = item.Others,
                            Time = item.Time
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

                List<DischargeSummaryBO> DischargeSummary = new List<DischargeSummaryBO>();
                if (model.DischargeSummary != null)
                {
                    DischargeSummaryBO Item;

                    foreach (var item in model.DischargeSummary)
                    {
                        Item = new DischargeSummaryBO()
                        {
                            CourseInTheHospital = item.CourseInTheHospital,
                            ConditionAtDischarge = item.ConditionAtDischarge,
                            DietAdvice = item.DietAdvice
                        };
                        DischargeSummary.Add(Item);
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
                            ID = item.ID,
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
                            NoofDays = item.NoofDays,
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
                            TreatmentMedicineUnitID = item.TreatmentMedicineUnitID
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
                            QuantityInstruction = item.QuantityInstruction,
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
                List<RoundsBO> RoundsList = new List<RoundsBO>();
                if (model.RoundsItems != null)
                {
                    RoundsBO Item;

                    foreach (var item in model.RoundsItems)
                    {
                        Item = new RoundsBO()
                        {
                            Remarks = item.Remarks,
                            RoundsDate = General.ToDateTime(item.RoundsDate),
                        };
                        RoundsList.Add(Item);
                    }
                }

                List<MedicineBO> InternalMedicinesList = new List<MedicineBO>();
                if (model.InternalMedicine != null)
                {
                    MedicineBO Item;

                    foreach (var item in model.InternalMedicine)
                    {
                        Item = new MedicineBO()
                        {
                            MedicineID = item.MedicinesID,
                            UnitID = item.UnitID,
                            Quantity = item.Quantity,
                            GroupID = item.GroupID
                        };
                        InternalMedicinesList.Add(Item);
                    }
                }

                List<MedicineItemBO> InternalMedicinesItems = new List<MedicineItemBO>();
                if (model.InternalMedicineItems != null)
                {
                    MedicineItemBO Item;

                    foreach (var item in model.InternalMedicineItems)
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
                            NoofDays = item.NoofDays,
                            IsEmptyStomach = item.IsEmptyStomach,
                            IsAfterFood = item.IsAfterFood,
                            IsBeforeFood = item.IsBeforeFood,
                            Description = item.Description,
                            GroupID = item.GroupID,
                            Frequency = item.Frequency,
                            PatientMedicineID = item.PatientMedicineID,
                            DischargeSummaryID=item.DischargeSummaryID
                        };
                        InternalMedicinesItems.Add(Item);
                    }
                }


                ipcasesheetBL.SaveV2(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem,RoundsList, DischargeSummary, InternalMedicinesList, InternalMedicinesItems);
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
                    ReviewID = model.ReviewID,
                    IPID = model.IPID,
                    IsDischargeAdvice = model.IsDischargeAdvice,
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
                            Date = General.ToDateTime(item.Date),
                            BP = item.BP,
                            Pulse = item.Pulse,
                            Temperature = item.Temperature,
                            HR = item.HR,
                            RR = item.RR,
                            Weight = item.Weight,
                            Height = item.Height,
                            Others = item.Others,
                            Time = item.Time
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
                            Area = item.Area,
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

                List<DischargeSummaryBO> DischargeSummary = new List<DischargeSummaryBO>();
                if (model.DischargeSummary != null)
                {
                    DischargeSummaryBO Item;

                    foreach (var item in model.DischargeSummary)
                    {
                        Item = new DischargeSummaryBO()
                        {
                            CourseInTheHospital = item.CourseInTheHospital,
                            ConditionAtDischarge = item.ConditionAtDischarge,
                            DietAdvice = item.DietAdvice
                        };
                        DischargeSummary.Add(Item);
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
                            ID=item.ID,
                            DocumentID = item.DocumentID,
                            Name = item.Name,
                            Date = General.ToDateTime(item.Date),
                            Description = item.Description,
                            IsBeforeAdmission = item.IsBeforeAdmission
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
                            TreatmentMedicineUnitID = item.TreatmentMedicineUnitID
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
                            QuantityInstruction = item.QuantityInstruction,
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
                List<RoundsBO> RoundsList = new List<RoundsBO>();
                if (model.RoundsItems != null)
                {
                    RoundsBO Item;

                    foreach (var item in model.RoundsItems)
                    {
                        Item = new RoundsBO()
                        {
                            Remarks = item.Remarks,
                            ClinicalNote = item.ClinicalNote,
                            RoundsTime = item.RoundsTime,
                            RoundsDate = General.ToDateTime(item.RoundsDate),
                        };
                        RoundsList.Add(Item);
                    }
                }

                List<MedicineBO> InternalMedicinesList = new List<MedicineBO>();
                if (model.InternalMedicine != null)
                {
                    MedicineBO Item;

                    foreach (var item in model.InternalMedicine)
                    {
                        Item = new MedicineBO()
                        {
                            MedicineID = item.MedicinesID,
                            UnitID = item.UnitID,
                            Quantity = item.Quantity,
                            GroupID = item.GroupID
                        };
                        InternalMedicinesList.Add(Item);
                    }
                }

                List<MedicineItemBO> InternalMedicinesItems = new List<MedicineItemBO>();
                if (model.InternalMedicineItems != null)
                {
                    MedicineItemBO Item;

                    foreach (var item in model.InternalMedicineItems)
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
                            NoofDays = item.NoofDays,
                            IsEmptyStomach = item.IsEmptyStomach,
                            IsAfterFood = item.IsAfterFood,
                            IsBeforeFood = item.IsBeforeFood,
                            Description = item.Description,
                            GroupID = item.GroupID,
                            Frequency = item.Frequency,
                            PatientMedicineID = item.PatientMedicineID,
                            DischargeSummaryID = item.DischargeSummaryID
                        };
                        InternalMedicinesItems.Add(Item);
                    }
                }


                ipcasesheetBL.SaveV5(diagnosis, VitalChartItems, Items, NewItems, BaseLineItems, ReportItems, LabTestItems, Treatments, TreatmentMedicines, MedicinesList, MedicinesItemsList, XrayItem, DoctorList, RogaPareekshaItems, CaseSheetItems, RogaNirnayamItem, RoundsList, DischargeSummary, InternalMedicinesList, InternalMedicinesItems);
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
        public JsonResult GetDashaVidhaPareekhsa(int PatientID, int IPID)
        {
            try
            {
                List<ExaminationBO> DashaVidhaPareekhsa = ipcasesheetBL.GetDashaVidhaPareekhsalist(PatientID, IPID);

                return Json(new { Status = "success", Data = DashaVidhaPareekhsa }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}