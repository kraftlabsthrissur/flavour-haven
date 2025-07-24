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
    public class TreatmentProcessController : Controller
    {
        private ITreatmentProcessContract treatmentProcessBL;
        private IGeneralContract generalBL;
        private ITreatmentScheduleContract treatmentScheduleBL;
        private IBatchContract batchBL;

        public TreatmentProcessController()
        {
            treatmentProcessBL = new TreatmentProcessBL();
            generalBL = new GeneralBL();
            treatmentScheduleBL = new TreatmentScheduleBL();
            batchBL = new BatchBL();
        }
        // GET: AHCMS/TreatmentIssue
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            TreatmentScheduleModel model = new TreatmentScheduleModel();
            return View(model);
        }

        public PartialViewResult ScheduleList(string Date)
        {
            TreatmentScheduleModel model = new TreatmentScheduleModel();
            DateTime fromDate = General.ToDateTime(Date);
            model.Items = treatmentProcessBL.GetTreatmentScheduleList(fromDate).Select(a => new TreatmentScheduleItemModel()
            {
                TreatmentID = a.TreatmentID,
                TreatmentName = a.TreatmentName,
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                DoctorID = a.DoctorID,
                DoctorName = a.DoctorName,
                TotalTreatmentNo = a.TotalTreatmentNo,
                NoOfTreatment = a.NoOfTreatment,
                TreatmentRoom = a.TreatmentRoom,
                TreatmentRoomID = a.TreatmentRoomID,
                StartTime = a.StartTime,
                DurationID = a.DurationID,
                Duration = a.Duration,
                EndTime = a.EndTime,
                TherapistID = a.TherapistID,
                TherapistName = a.TherapistName,
                Status = a.Status,
                StatusID = a.StatusID,
                TreatmentScheduleItemID = a.TreatmentScheduleItemID,
                AppointmentProcessID = a.AppointmentProcessID,
                TreatmentProcessID = a.TreatmentProcessID,
                DurationList = new SelectList(treatmentScheduleBL.GetDurationForTreatmentList(), "DurationID", "Duration"),
                TreatmentStatusList = new SelectList(treatmentProcessBL.GetDropDownDetails(), "TreatmentStatusID", "TreatmentStatus"),
            }).ToList();
            return PartialView(model);
        }

        public JsonResult GetDropDownDetails()
        {
            List<TreatmentScheduleBO> treatmentIssueStatus = new List<TreatmentScheduleBO>()
            {
                new TreatmentScheduleBO()
                {
                    TreatmentStatusID = 0,
                    TreatmentStatus = "Select"
                }
            };
            treatmentIssueStatus.AddRange(treatmentProcessBL.GetDropDownDetails());
            return Json(new { Status = "success", data = treatmentIssueStatus }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Save(TreatmentScheduleModel model)
        {
            try
            {
                List<TreatmentScheduleItemBO> Items = new List<TreatmentScheduleItemBO>();
                List<TreatmentMedicineItemBO> Medicines = new List<TreatmentMedicineItemBO>();
                if (model.Items != null)
                {
                    TreatmentScheduleItemBO treatmentScheduleItem;
                    foreach (var item in model.Items)
                    {
                        treatmentScheduleItem = new TreatmentScheduleItemBO()
                        {
                            TreatmentProcessID = item.TreatmentProcessID,
                            PatientID = item.PatientID,
                            TreatmentID = item.TreatmentID,
                            NoOfTreatment = item.NoOfTreatment,
                            TreatmentRoomID = item.TreatmentRoomID,
                            StartTime = item.StartTime,
                            DurationID = item.DurationID,
                            TherapistID = item.TherapistID,
                            StatusID=item.StatusID,
                            Remarks=item.Remarks,
                            TreatmentScheduleItemID=item.TreatmentScheduleItemID,
                            EndTime=item.EndTime,
                            Date = General.ToDateTime(item.Date),
                            AppointmentProcessID = item.AppointmentProcessID,
                            Status = item.Status
                        };
                        Items.Add(treatmentScheduleItem);
                    }
                }
                if (model.Medicines != null)
                {
                    TreatmentMedicineItemBO treatmentMedicine;
                    foreach (var item in model.Medicines)
                    {
                        treatmentMedicine = new TreatmentMedicineItemBO()
                        {
                            TreatmentProcessID = item.TreatmentProcessID,
                            TreatmentScheduleID = item.TreatmentScheduleID,
                            ItemID=item.ItemID,
                            UnitID=item.UnitID,
                            Qty=item.Qty
                        };
                        Medicines.Add(treatmentMedicine);
                    }
                }
                treatmentProcessBL.Save(Items, Medicines);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTreatmentProcessList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = Datatable.Columns[1].Search.Value;
                string StartTimeHint = Datatable.Columns[2].Search.Value;
                string EndTimeHint = Datatable.Columns[3].Search.Value;
                string TreatmentHint = Datatable.Columns[4].Search.Value;
                string PatientHint = Datatable.Columns[5].Search.Value;
                string DoctorHint = Datatable.Columns[6].Search.Value;
                string MedicineHint = Datatable.Columns[7].Search.Value;
                string TreatmentRoomHint = Datatable.Columns[8].Search.Value;
                string StatusHint = Datatable.Columns[9].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                DatatableResultBO resultBO = treatmentProcessBL.GetTreatmentProcessList(Type,DateHint, StartTimeHint, EndTimeHint, TreatmentHint, PatientHint, DoctorHint, MedicineHint, TreatmentRoomHint, StatusHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "TreatmentProcess", "GetTreatmentProcessList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult TreatmentMedicineDataList(int TreatmentScheduleID, int TreatmentProcessID = 0)
        {
            TreatmentScheduleModel model = new TreatmentScheduleModel();
            model.Medicines = treatmentProcessBL.GetTreatmentMedicines(TreatmentScheduleID, TreatmentProcessID).Select(
                    a => new TreatmentMedicineItemModel()
                    {
                        ItemID = a.ItemID,
                        Item = a.Item,
                        Stock = a.Stock,
                        Qty = a.Qty,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        TreatmentScheduleID = TreatmentScheduleID,
                        TreatmentProcessID = TreatmentProcessID,
                        ProductionGroupID = a.ProductionGroupID,
                        BatchList = new SelectList(batchBL.GetBatchList(a.ItemID, a.StoreID), "BatchID", "Batch")
                    }
                    ).ToList();
                return PartialView(model);
        }

        public JsonResult GetItemStockByBatchID(int ItemID,int BatchID,int TreatmentScheduleID)
        {
            decimal Stock=treatmentProcessBL.GetItemStockByBatchID(ItemID, BatchID, TreatmentScheduleID);
            return Json(new { Status = "success", data = Stock }, JsonRequestBehavior.AllowGet);

        }
    }
}