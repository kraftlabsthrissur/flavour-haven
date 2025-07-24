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
    public class RoomAllocationController : Controller
    {
        private IRoomAllocationContract roomAllocationBL;
        private IGeneralContract generalBL;
        private ISubmasterContract subMasterBL;
        private IRoomReservationContract roomReservationBL;
        public RoomAllocationController()
        {
            roomAllocationBL = new RoomAllocationBL();
            generalBL = new GeneralBL();
            subMasterBL = new SubmasterBL();
            roomReservationBL = new RoomReservationBL();
        }
        // GET: AHCMS/IP
        public ActionResult ReferedToIPList()
        {
            return View();
        }
        public JsonResult GetReferedToIPList(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string Patient = Datatable.Columns[2].Search.Value;
                string Doctor = Datatable.Columns[3].Search.Value;
                string AdmissionDate = Datatable.Columns[4].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = roomAllocationBL.GetReferedToIPList(TransNo, Patient, Doctor, AdmissionDate, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "RoomAllocation", "GetReferedToIPList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create(int AppointmentProcessID, int ReservationID, int RoomStatusID,bool IsRoomChange=false)
        {
            RoomReservationModel roomAllocationModel = new RoomReservationModel();
            if (IsRoomChange == true)
            {
                var obj = roomAllocationBL.GetAllocatedRoomDetails(RoomStatusID);
                roomAllocationModel.RoomStatusID = RoomStatusID;
                roomAllocationModel.RoomTypeID = obj.RoomTypeID;
                roomAllocationModel.RoomID = obj.RoomID;
                roomAllocationModel.RoomName = obj.RoomName;
                roomAllocationModel.RoomType = obj.RoomType;
                roomAllocationModel.AdmissionDate = General.FormatDate(obj.AdmissionDate);
                roomAllocationModel.AdmissionDateTill = General.FormatDate(obj.AdmissionDateTill);
                roomAllocationModel.PatientID =obj.PatientID;
                roomAllocationModel.PatientName = obj.PatientName;
                roomAllocationModel.ByStander = obj.ByStander;
                roomAllocationModel.MobileNumber = obj.MobileNumber;
                roomAllocationModel.IsRoomChange = IsRoomChange;
                roomAllocationModel.RoomChangeDate= General.FormatDate(DateTime.Now);
                roomAllocationModel.RoomList = new SelectList(roomReservationBL.GetAllRooms(), "ID", "RoomName");
                roomAllocationModel.RoomList = new SelectList(roomReservationBL.GetAllocatedRoom(RoomStatusID), "ID", "RoomName");
                roomAllocationModel.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
            }
            else
            {
                if (ReservationID > 0 && IsRoomChange == false)
                {
                    var obj = roomAllocationBL.GetReservationDetailsByID(ReservationID,AppointmentProcessID);
                    roomAllocationModel.ReservationID = obj.ReservationID;
                    roomAllocationModel.RoomTypeID = obj.RoomTypeID;
                    roomAllocationModel.RoomID = obj.RoomID;
                    roomAllocationModel.RoomName = obj.RoomName;
                    roomAllocationModel.Rate = obj.Rate;
                    roomAllocationModel.AdmissionDate = General.FormatDate(DateTime.Now);
                    //roomAllocationModel.AdmissionDate = General.FormatDate(obj.FromDate);
                    roomAllocationModel.AdmissionDateTill = General.FormatDate(obj.ToDate);
                    roomAllocationModel.PatientID = obj.PatientID;
                    roomAllocationModel.PatientName = obj.PatientName;
                    roomAllocationModel.TransNo = obj.TransNo;
                    roomAllocationModel.AppointmentProcessID = obj.AppointmentProcessID;
                    roomAllocationModel.DoctorID = obj.DoctorID;      
                    roomAllocationModel.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
                    //roomAllocationModel.RoomList = new SelectList(roomReservationBL.GetAllRooms(), "ID", "RoomName");
                    roomAllocationModel.RoomList = new SelectList(roomReservationBL.GetRoomByID(ReservationID), "ID", "RoomName");
                }
                else
                {
                    var item = roomAllocationBL.GetPatientDetailsByID(AppointmentProcessID);
                    roomAllocationModel.PatientID = item.PatientID;
                    roomAllocationModel.DoctorID = item.DoctorID;
                    roomAllocationModel.PatientName = item.PatientName;
                    roomAllocationModel.TransNo = item.TransNo;
                    roomAllocationModel.AdmissionDate = General.FormatDate(DateTime.Now);
                    //roomAllocationModel.AdmissionDate = General.FormatDate(item.AdmissionDate);
                    roomAllocationModel.AppointmentProcessID = AppointmentProcessID;
                    roomAllocationModel.RoomStatusID = item.RoomStatusID;
                    roomAllocationModel.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
                    roomAllocationModel.RoomList = new SelectList(roomReservationBL.GetAllRooms(), "ID", "RoomName");
                }
            }
           
            return View(roomAllocationModel);
        }
        public JsonResult GetAvailableRoom(int ID,string FromDate,string ToDate,int PatientID)
        {
            DateTime fromDate = General.ToDateTime(FromDate);
            DateTime toDate = General.ToDateTime(ToDate);
            List<RoomAllocationBO> Room = roomReservationBL.GetAvailableRooms(ID, fromDate, toDate, PatientID).ToList();
            return Json(new { Status = "success", data = Room }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoomDetailsByID(int ID)
        {
            List<RoomAllocationBO> RoomDetails = roomAllocationBL.GetRoomDetailsByID(ID).ToList();
            return Json(new { Status = "success", data = RoomDetails }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save(RoomReservationModel model)
        {
            try
            {
                RoomAllocationBO IP = new RoomAllocationBO()
                {
                    AppointmentProcessID = model.AppointmentProcessID,
                    PatientID = model.PatientID,
                    DoctorID = model.DoctorID,
                    AdmissionDate = General.ToDateTime(model.AdmissionDate),
                    AdmissionDateTill = General.ToDateTime(model.AdmissionDateTill),
                    RoomTypeID=model.RoomTypeID,
                    RoomID=model.RoomID,
                    Rate=model.Rate,
                    ByStander = model.ByStander,
                    MobileNumber=model.MobileNumber,
                    ReservationID=model.ReservationID,
                    IsRoomChange=model.IsRoomChange,
                    RoomStatusID=model.RoomStatusID

                };
                if (model.RoomChangeDate != null)
                    IP.RoomChangeDate = General.ToDateTime(model.RoomChangeDate);
                if (IP.ID == 0)
                {
                    roomAllocationBL.Save(IP);
                }
                return Json(new { Status = "Success", Message = "IP Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save Room failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult InPatientList()
        {
            return View();
        }
        public JsonResult GetInPatientList(DatatableModel Datatable)
        {
            try
            {
                string Patient = Datatable.Columns[1].Search.Value;
                string Room = Datatable.Columns[2].Search.Value;
                string AdmissionDate = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = roomAllocationBL.GetInPatientList(Patient, Room, AdmissionDate, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "RoomAllocation", "GetReferedToIPList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRoomType()
        {
            List<RoomAllocationBO> RoomType = roomAllocationBL.GetRoomType().ToList();
            return Json(new { Status = "success", data = RoomType }, JsonRequestBehavior.AllowGet);
        }

    }
}