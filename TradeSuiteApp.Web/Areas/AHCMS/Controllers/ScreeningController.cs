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
    public class ScreeningController : Controller
    {
        private IScreeningContract screeningBL;
        private IGeneralContract generalBL;
        private IPatientDiagnosisContract managePatientBL;
        private IFileContract fileBL;

        public ScreeningController()
        {
            screeningBL = new ScreeningBL();
            generalBL = new GeneralBL();
            managePatientBL = new PatientDiagnosisBL();
            fileBL = new FileBL();
        }

        // GET: AHCMS/Screening
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetOpPatientList(DatatableModel Datatable)
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

                DatatableResultBO resultBO = screeningBL.GetOpPatientList(Type, CodeHint, NameHint, TimeHint, TokenHint, DateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "Screening", "GetOpPatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create(int ID, int AppointmentScheduleItemID, int OPID, bool IsCompleted, bool IsReferedIP)
        {
            PatientDiagnosisModel model = managePatientBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                PatientCode = a.PatientCode,
                Age = a.Age,
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
            model.DateList = screeningBL.GetDateListByID(ID, OPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.AppointmentProcessID = OPID;
            model.IsCompleted = IsCompleted;
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }
           
            model.ReportDate = General.FormatDate(DateTime.Now);
           
           
            model.AppointmentScheduleItemID = AppointmentScheduleItemID;
            model.IsReferedIP = IsReferedIP;
            return View(model);
        }

        public ActionResult Save(PatientDiagnosisModel model)
        {
            try
            {
                PatientDiagnosisBO diagnosis = new PatientDiagnosisBO()
                {
                    PatientID = model.PatientID,
                    AppointmentType = model.AppointmentType,
                    VisitType = model.VisitType,
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    AppointmentProcessID = model.AppointmentProcessID,
                    ParentID = model.ParentID,
                    ReviewID = model.ReviewID,
                    Date = General.ToDateTime(model.Date),
                };
               
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

                screeningBL.Save(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems);
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

        public PartialViewResult Vitals(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = General.ToDateTime(Date);
            var obj = screeningBL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
            PatientDiagnosisModel model = new PatientDiagnosisModel();
            if (obj != null)
            {
                model.RespiratoryRate = obj.RespiratoryRate;
                model.Others = obj.Others;
                model.IsCompleted = IsCompleted;
            }
            return PartialView(model);
        }

        public PartialViewResult VitalChart(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = General.ToDateTime(Date);
            var obj = screeningBL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
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
            DateTime FromDate = General.ToDateTime(Date);
            var obj = screeningBL.GetVitalChart(PatientID, FromDate, AppointmentProcessID);
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

        public PartialViewResult GetReport(int PatientID, string Date, bool IsCompleted, int AppointmentProcessID)
        {
            DateTime FromDate = General.ToDateTime(Date);
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

        //for Ilaj by priyanka

        public ActionResult IndexV2()
        {
            return View();
        }

        public ActionResult CreateV2(int ID, int AppointmentScheduleItemID, int OPID, bool IsCompleted, bool IsReferedIP)
        {
            PatientDiagnosisModel model = managePatientBL.GetPatientDetails(ID).Select(a => new PatientDiagnosisModel()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                PatientCode = a.PatientCode,
                Age = a.Age,
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
            var obj = screeningBL.IsPatientExists(OPID);
            model.IsExists = obj.IsExists;
            if(model.IsExists==true)
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
            

            model.DateList = screeningBL.GetDateListByID(ID, OPID).Select(r => new SelectListItem
            {
                Text = General.FormatDate(r.Date),
                Value = General.FormatDate(r.Date)
            });
            model.AppointmentProcessID = OPID;
            model.IsCompleted = IsCompleted;
            model.GeneralOptions = new List<KeyValuePair<string, SelectList>>();
            foreach (var item in model.ExaminationItems.Where(a => a.Type == "Value"))
            {
                model.GeneralOptions.Add(new KeyValuePair<string, SelectList>(item.Name, new SelectList(managePatientBL.GetOptionList(item.Name), "ID", "Name")));
            }

            model.ReportDate = General.FormatDate(DateTime.Now);
            model.AppointmentScheduleItemID = AppointmentScheduleItemID;
            model.IsReferedIP = IsReferedIP;
            return View(model);
        }

        public ActionResult SaveV2(PatientDiagnosisModel model)
        {
            try
            {
                PatientDiagnosisBO diagnosis = new PatientDiagnosisBO()
                {
                    PatientID = model.PatientID,
                    AppointmentType = model.AppointmentType,
                    VisitType = model.VisitType,
                    AppointmentScheduleItemID = model.AppointmentScheduleItemID,
                    AppointmentProcessID = model.AppointmentProcessID,
                    ParentID = model.ParentID,
                    ReviewID = model.ReviewID,
                    Date = General.ToDateTime(model.Date),
                };

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
                            Description = item.Description
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
                List<QuestionnaireBO> QuestionnaireItems = new List<QuestionnaireBO>();
                if (model.QuestionnaireItems != null)
                {
                    QuestionnaireBO Item;

                    foreach (var item in model.QuestionnaireItems)
                    {
                        Item = new QuestionnaireBO()
                        {
                            Question=item.Question,
                            Answer=item.Answer
                        };
                        QuestionnaireItems.Add(Item);
                    }
                }
                screeningBL.SaveV2(diagnosis, VitalChartItems, Items, BaseLineItems, ReportItems, QuestionnaireItems);
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

        //public JsonResult Questionnaire(int PatientID, int AppointmentProcessID)
        //{
        //    try
        //    {
        //        PatientDiagnosisModel Model = new PatientDiagnosisModel();
        //        List<PatientDiagnosisBO> question = screeningBL.GetQuestionnaireList(PatientID, AppointmentProcessID);
        //        QuestionnaireModel questionModel;
        //        Model.QuestionnaireItems = new List<QuestionnaireModel>();
        //        foreach (var m in question)
        //        {
        //            questionModel = new QuestionnaireModel()
        //            { 
        //              Question = m.Question,
        //              QuestionID = m.QuestionID,
        //              Answer = m.Answer,
        //        };
        //            Model.QuestionnaireItems.Add(questionModel);
        //        }
        //        return Json(new { Status = "success", Data = question }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new
        //        {
        //            Status = "failure",
        //            Message = e.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}