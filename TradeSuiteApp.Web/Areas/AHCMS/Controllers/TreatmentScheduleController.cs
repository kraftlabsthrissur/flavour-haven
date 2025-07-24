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
    public class TreatmentScheduleController : Controller
    {
        private ITreatmentScheduleContract treatmentScheduleBL;
        private IGeneralContract generalBL;
        public TreatmentScheduleController()
        {
            treatmentScheduleBL = new TreatmentScheduleBL();
            generalBL = new GeneralBL();
        }
        // GET: AHCMS/TreatmentSchedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            TreatmentScheduleModel model = new TreatmentScheduleModel();
            return View(model);
        }

        public ActionResult Save(TreatmentScheduleModel model)
        {
            try
            {
                List<TreatmentScheduleItemBO> Items = new List<TreatmentScheduleItemBO>();
                if (model.Items != null)
                {
                    TreatmentScheduleItemBO treatmentScheduleItem;
                    foreach (var item in model.Items)
                    {
                        treatmentScheduleItem = new TreatmentScheduleItemBO()
                        {
                            ScheduleID = item.ScheduleID,
                            ScheduledDate = General.ToDateTime(item.ScheduledDate),
                            EndTime = item.EndTime,
                            TreatmentRoomID = item.TreatmentRoomID,
                            StartTime = item.StartTime,
                            DurationID = item.DurationID,
                            TherapistID = item.TherapistID,
                        };

                        Items.Add(treatmentScheduleItem);
                    }
                }
                treatmentScheduleBL.Save(Items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTreatmentScheduleList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = Datatable.Columns[1].Search.Value;
                string StartTimeHint = Datatable.Columns[2].Search.Value;
                string EndTimeHint = Datatable.Columns[3].Search.Value;
                string TreatmentHint = Datatable.Columns[4].Search.Value;
                string PatientHint = Datatable.Columns[5].Search.Value;
                string DoctorHint = Datatable.Columns[6].Search.Value;
                string TherapistHint = Datatable.Columns[7].Search.Value;
                string MedicineHint = Datatable.Columns[8].Search.Value;
                string TreatmentRoomHint = Datatable.Columns[9].Search.Value;
                string StatusHint = Datatable.Columns[10].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                DatatableResultBO resultBO = treatmentScheduleBL.GetTreatmentScheduleList(Type, DateHint, StartTimeHint, EndTimeHint, TreatmentHint, PatientHint, DoctorHint, TherapistHint, TreatmentRoomHint, StatusHint, MedicineHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "TreatmentSchedule", "GetTreatmentScheduleList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ScheduledTreatmentList(string Type, string FromDate, int PatientID = 0, int AppointmentProcessID = 0)
        {
            DateTime StartDate;
            if (Type != "DateWise")
            {
                StartDate = (DateTime.Now);
            }
            else
            {
                StartDate = General.ToDateTime(FromDate);
            }

            TreatmentScheduleModel model = new TreatmentScheduleModel();
            model.Items = treatmentScheduleBL.GetScheduledTreatmentList(Type, StartDate, PatientID, AppointmentProcessID).Select(a => new TreatmentScheduleItemModel()
            {

                TreatmentID = a.TreatmentID,
                TreatmentName = a.TreatmentName,
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                DoctorID = a.DoctorID,
                DoctorName = a.DoctorName,
                ScheduledDate = General.FormatDate(a.ScheduledDate),
                NoOfTreatment = (int)a.NoOfTreatment,
                TreatmentRoom = a.TreatmentRoom,
                TreatmentRoomID = a.TreatmentRoomID,
                TherapistID = a.TherapistID,
                TherapistName = a.TherapistName,
                PreferedTherapist = a.PreferedTherapist,
                PreferedTherapistID = a.PreferedTherapistID,
                PreferedTreatmentRoom = a.PreferedTreatmentRoom,
                PreferedTreatmentRoomID = a.PreferedTreatmentRoomID,
                Status = a.Status,
                DurationID = (int)a.DurationID,
                ScheduleID = a.ScheduleID,
                Duration = a.Duration,
                StartTime = a.StartTime,
                TotalTreatmentNo = a.TotalTreatmentNo,
                DurationList = new SelectList(treatmentScheduleBL.GetDurationForTreatmentList(), "DurationID", "Duration"),
                TreatmentRoomList = new SelectList(treatmentScheduleBL.GetTreatmentRoomDetails(), "TreatmentRoomID", "TreatmentRoom"),
                TherapistList = new SelectList(treatmentScheduleBL.GetTherapistDetails(), "TherapistID", "TherapistName"),
            }).ToList();
            return PartialView(model);
        }


    }
}