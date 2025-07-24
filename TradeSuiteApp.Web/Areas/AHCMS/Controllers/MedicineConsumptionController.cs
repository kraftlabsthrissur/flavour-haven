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
    public class MedicineConsumptionController : Controller
    {
        private IRoomReservationContract roomReservationBL;
        private IGeneralContract generalBL;
        private IMedicineConsumptionContract medicineConsumptionBL;
        public MedicineConsumptionController()
        {
            roomReservationBL = new RoomReservationBL();
            generalBL = new GeneralBL();
            medicineConsumptionBL = new MedicineConsumptionBL();
        }
        // GET: AHCMS/MedicineConsumption
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            MedicineConsumptionModel MedicineConsumptionModel = new MedicineConsumptionModel();
            MedicineConsumptionModel.RoomList = new SelectList(roomReservationBL.GetAllRooms(), "RoomID", "RoomName");
            MedicineConsumptionModel.TimeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Morning", Value ="Morning", },
                                                 new SelectListItem { Text = "Noon", Value = "Noon"},
                                                 new SelectListItem { Text = "Evening", Value = "Evening"},
                                                 new SelectListItem { Text = "Night", Value = "Night"},
                                                 }, "Value", "Text");
            MedicineConsumptionModel.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            return View(MedicineConsumptionModel);
        }

        public ActionResult Save(MedicineConsumptionModel model)
        {
            try
            {
                List<MedicineConsumptionBO> Items = new List<MedicineConsumptionBO>();
                List<MedicinesConsumptionListBO> Medicines = new List<MedicinesConsumptionListBO>();
                if (model.Items != null)
                {
                    MedicineConsumptionBO MedicineConsumptionItem;
                    foreach (var item in model.Items)
                    {
                        MedicineConsumptionItem = new MedicineConsumptionBO()
                        {
                            Date = General.ToDateTime(item.Date),
                            AppointmentProcessID = item.AppointmentProcessID,
                            Status = item.Status,
                            Time = item.Time,
                            ActiualTime=item.ActiualTime,
                            MedicineConsumptionID=item.MedicineConsumptionID
                        };
                        Items.Add(MedicineConsumptionItem);
                    }
                }
                if (model.Medicines != null)
                {
                    MedicinesConsumptionListBO consumptionMedicine;
                    foreach (var item in model.Medicines)
                    {
                        consumptionMedicine = new MedicinesConsumptionListBO()
                        {
                            PatientMedicinesID = item.PatientMedicinesID,
                            MedicineConsumptionID = item.MedicineConsumptionID,
                            ItemID = item.ItemID,
                            UnitID = item.UnitID,
                            Qty = item.Qty,
                            StoreID=item.StoreID
                        };
                        Medicines.Add(consumptionMedicine);
                    }
                }
                medicineConsumptionBL.Save(Items, Medicines);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult MedicineConsumptionList(string Datestring, int StoreID ,int RoomID = 0, string Time = null)
        {
          DateTime Date;
         Date = General.ToDateTime(Datestring);

            MedicineConsumptionModel model = new MedicineConsumptionModel();
            model.Items = medicineConsumptionBL.GetMedicineConsumptionList(Date, StoreID, RoomID, Time).Select(a => new MedicineConsumptionItemModel()
            {
                DoctorID = a.DoctorID,
                DoctorName = a.DoctorName,
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                Room = a.Room,
                RoomID = (int)a.RoomID,
                PatientMedicinesID = a.PatientMedicinesID,
                AppointmentProcessID = (int)a.AppointmentProcessID,
                IPID = (int)a.IPID,
                Medicine = a.Medicine,
                ModeOfAdminstration = a.ModeOfAdminstration,
                BeforeOrAfterFood = a.BeforeOrAfterFood,
                Date = General.FormatDate(Date),
                Time = a.Time,
                Description = a.Description,
                MedicineConsumptionID=a.MedicineConsumptionID,
                ActiualTime=a.ActiualTime,
                Status = a.Status,
                StatusList= new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Scheduled", Value ="Scheduled", },
                                                 new SelectListItem { Text = "Completed", Value = "Completed"}
                                                 }, "Value", "Text"),
        }).ToList();
            return PartialView(model);
        }

        public PartialViewResult MedicinesDataList(int PatientMedicinesID, int StoreID,int MedicineConsumptionID)
        {
            MedicineConsumptionModel model = new MedicineConsumptionModel();
            model.Medicines = medicineConsumptionBL.MedicinesForConsumption(PatientMedicinesID,StoreID).Select(
                    a => new MedicinesConsumptionListModel()
                    {
                        ItemID = a.ItemID,
                        Item = a.Item,
                        Stock = a.Stock,
                        Qty = a.Qty,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        PatientMedicinesID = PatientMedicinesID,
                        ProductionGroupID = a.ProductionGroupID,
                        MedicineConsumptionID= MedicineConsumptionID
                        //BatchList = new SelectList(batchBL.GetBatchList(a.ItemID, a.StoreID), "BatchID", "Batch")
                    }
                    ).ToList();
            return PartialView(model);
        }

        public JsonResult GetMedicineConsumptionListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string DateHint = Datatable.Columns[1].Search.Value;
                string TimeHint = Datatable.Columns[2].Search.Value;
                string PatientHint = Datatable.Columns[3].Search.Value;
                string DoctorHint = Datatable.Columns[4].Search.Value;
                string MedicineHint = Datatable.Columns[5].Search.Value;
                string RoomHint = Datatable.Columns[6].Search.Value;
                string StatusHint = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);
                DatatableResultBO resultBO = medicineConsumptionBL.GetMedicineConsumptionListForDataTable(Type, DateHint, TimeHint, PatientHint, DoctorHint, MedicineHint, RoomHint, StatusHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "TreatmentProcess", "GetTreatmentProcessList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}