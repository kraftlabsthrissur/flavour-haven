using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class ConsultationScheduleController : Controller
    {
        private IConsultationScheduleContract consultationScheduleBL;
        private IGeneralContract generalBL;
        private ISubmasterContract submasterBL;
        public ConsultationScheduleController()
        {
            consultationScheduleBL = new ConsultationScheduleBL();
            generalBL = new GeneralBL();
            submasterBL = new SubmasterBL();
        }
        // GET: Masters/ConsultationSchedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ConsultationScheduleModel Model = new ConsultationScheduleModel();
            Model.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            Model.WeekDayList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Sunday", Value ="Sunday", },
                                                 new SelectListItem { Text = "Monday", Value = "Monday"},
                                                 new SelectListItem { Text = "Tuesday", Value = "Tuesday"},
                                                 new SelectListItem { Text = "Wednesday", Value = "Wednesday"},
                                                 new SelectListItem { Text = "Thursday", Value = "Thursday"},
                                                 new SelectListItem { Text = "Friday", Value = "Friday"},
                                                 new SelectListItem { Text = "Saturday", Value = "Saturday"},
                                                 }, "Value", "Text");
            return View(Model);
        }
        [HttpPost]
        public ActionResult Save(ConsultationScheduleModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                ConsultationScheduleBO ConsultationScheduleBO = new ConsultationScheduleBO()
                {
                    ID = model.ID,
                    DoctorID = model.DoctorID,
                    TimeSlot = model.TimeSlot,
                    IsDraft = model.IsDraft,
                    ConsultationFeeValidity = model.ConsultationFeeValidity,
                    ConsultationFee = model.ConsultationFee
                };
                List<ConsultationScheduleItemBO> Items = new List<ConsultationScheduleItemBO>();
                if (model.Items != null)
                {
                    ConsultationScheduleItemBO Item;

                    foreach (var item in model.Items)
                    {
                        Item = new ConsultationScheduleItemBO()
                        {
                            WeekDay = item.WeekDay,
                            EndTime = item.EndTime,
                            StartTime = item.StartTime
                        };
                        Items.Add(Item);
                    }
                }
                consultationScheduleBL.Save(ConsultationScheduleBO, Items);
                return Json(new { Status = "success", Message = "Saved SuccessFully", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "ConsultationSchedule", "Save", model.ID, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int Id)
        {

            ConsultationScheduleModel model = consultationScheduleBL.GetConsultationScheduleDetails(Id).Select(m => new ConsultationScheduleModel()
            {
                ID = m.ID,
                DoctorID = m.DoctorID,
                DoctorName = m.DoctorName,
                TimeSlot = m.TimeSlot,
                ConsultationFeeValidity = m.ConsultationFeeValidity,
                ConsultationFee=m.ConsultationFee
            }).First();
            model.Items = consultationScheduleBL.GetConsultationScheduleItemDetails(Id).Select(m => new ConsultationScheduleItemModel()
            {
                WeekDay = m.WeekDay,
                EndTime = m.EndTime,
                StartTime = m.StartTime

            }).ToList();
            return View(model);
        }
        public ActionResult Edit(int Id)
        {

            ConsultationScheduleModel model = consultationScheduleBL.GetConsultationScheduleDetails(Id).Select(m => new ConsultationScheduleModel()
            {
                ID = m.ID,
                DoctorID = m.DoctorID,
                DoctorName = m.DoctorName,
                TimeSlot = m.TimeSlot,
                ConsultationFeeValidity = m.ConsultationFeeValidity,
                ConsultationFee=m.ConsultationFee

            }).First();
            model.Items = consultationScheduleBL.GetConsultationScheduleItemDetails(Id).Select(m => new ConsultationScheduleItemModel()
            {
                WeekDay = m.WeekDay,
                EndTime = m.EndTime,
                StartTime = m.StartTime

            }).ToList();
            model.WeekDayList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Sunday", Value ="Sunday", },
                                                 new SelectListItem { Text = "Monday", Value = "Monday"},
                                                 new SelectListItem { Text = "Tuesday", Value = "Tuesday"},
                                                 new SelectListItem { Text = "Wednesday", Value = "Wednesday"},
                                                 new SelectListItem { Text = "Thursday", Value = "Thursday"},
                                                 new SelectListItem { Text = "Friday ", Value = "Friday "},
                                                 new SelectListItem { Text = "Saturday", Value = "Saturday"},
                                                 }, "Value", "Text");
            return View(model);
        }
        public JsonResult GetConsultationScheduleList(DatatableModel Datatable)
        {
            try
            {
                string NameHint = Datatable.Columns[1].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = consultationScheduleBL.GetConsultationScheduleList(NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDoctorConsultationSchedule(int DoctorID, string Date, string StartTime, string EndTime)
        {
            DateTime ScheduleDate = General.ToDateTime(Date);
            List<ConsultationScheduleModel> timelist = new List<ConsultationScheduleModel>();
            timelist = consultationScheduleBL.GetDoctorConsultationSchedule(DoctorID, ScheduleDate, StartTime, EndTime).Select(m => new ConsultationScheduleModel()
            {
                Time = m.Time.ToString("hh:mm tt"),
                SlotName = m.SlotName

            }).ToList();
            return Json(timelist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDoctorConsultationTime(int DoctorID, string Date)
        {
            DateTime ScheduleDate = General.ToDateTime(Date);
            List<ConsultationScheduleModel> timelist = new List<ConsultationScheduleModel>();
            timelist = consultationScheduleBL.GetDoctorConsultationTime(DoctorID, ScheduleDate).Select(m => new ConsultationScheduleModel()
            {
                StartTime = m.StartTime,
                EndTime = m.EndTime

            }).ToList();
            return Json(timelist, JsonRequestBehavior.AllowGet);
        }
    }
}