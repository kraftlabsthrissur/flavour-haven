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
    public class RoomReservationController : Controller
    {
        private ISubmasterContract subMasterBL;
        private IGeneralContract generalBL;
        private IDistrictContract districtBL;
        private IStateContract stateBL;
        private ICountryContract countryBL;
        private IRoomReservationContract roomReservationBL;
        private IRoomAllocationContract roomAllocationBL;

        public RoomReservationController()
        {
            subMasterBL = new SubmasterBL();
            generalBL = new GeneralBL();
            districtBL = new DistrictBL();
            stateBL = new StateBL();
            countryBL = new CountryBL();
            roomReservationBL = new RoomReservationBL();
            roomAllocationBL = new RoomAllocationBL();
        }
        // GET: AHCMS/RoomReservation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            RoomAllocationModel model = new RoomAllocationModel();
            model.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
            model.Code = generalBL.GetSerialNo("Patients", "Code");
            model.EmployeeCategoryID = subMasterBL.GetEmployeeCategoryID("Doctor");
            model.Date = General.FormatDate(DateTime.Now);
            model.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            model.BloodGroupList = new SelectList(subMasterBL.GetBloodGroupList(), "ID", "Name");
            model.GenderList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Male", Value ="Male", },
                                                 new SelectListItem { Text = "Female", Value = "Female"},
                                                    new SelectListItem { Text = "Others", Value = "Others"},
                                                 }, "Value", "Text");
            model.MartialStatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Single", Value ="Single", },
                                                 new SelectListItem { Text = "Married", Value = "Married"},
                                                 }, "Value", "Text");
            model.PatientReferedList = new SelectList(subMasterBL.GetPatientReferenceBy(), "ID", "Name");
            model.EmployedInIndia = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Yes", Value ="Yes", },
                                                 new SelectListItem { Text = "No", Value = "No"},
                                                 }, "Value", "Text");
            model.OccupationList = new SelectList(subMasterBL.GetOccupationList(), "ID", "Name");
            return View(model);
        }

        public JsonResult GetAvailableRoom(int ID,string FromDate,string ToDate,int PatientID=0)
        {

            DateTime fromDate = General.ToDateTime(FromDate);
            DateTime toDate= General.ToDateTime(ToDate);
            List<RoomAllocationBO> Room = roomReservationBL.GetAvailableRooms(ID, fromDate,toDate, PatientID).ToList();
            return Json(new { Status = "success", data = Room }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoomDetailsByID(int ID)
        {
            List<RoomAllocationBO> RoomDetails = roomReservationBL.GetRoomDetailsByID(ID).ToList();
            return Json(new { Status = "success", data = RoomDetails }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(RoomAllocationModel model)
        {
            try
            {
                RoomAllocationBO RoomAllocation = new RoomAllocationBO()
                {
                    ID=model.ID,
                    Date=General.ToDateTime(model.Date),
                    PatientID = model.PatientID,
                    RoomTypeID=model.RoomTypeID,
                    RoomID=model.RoomID,
                    FromDate= General.ToDateTime(model.FromDate),
                    ToDate= General.ToDateTime(model.ToDate),
                    Rate=model.Rate       
                };
                if (RoomAllocation.ID == 0)
                {
                    roomReservationBL.Save(RoomAllocation);
                }
                else
                {
                    roomReservationBL.UpdateRoomReservation(RoomAllocation);
                }
                return Json(new { Status = "Success", Message = "Room Reservation Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Room Reservation failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetRoomReservationList(DatatableModel Datatable)
        {
            try
            {
                string FromDate = Datatable.Columns[1].Search.Value;
                string ToDate = Datatable.Columns[2].Search.Value;
                string Patient = Datatable.Columns[3].Search.Value;
                string Room = Datatable.Columns[4].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = roomReservationBL.GetRoomReservationList(FromDate, ToDate, Patient, Room, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "RoomReservation", "Index", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int Id)
        {
            RoomAllocationModel Model = roomReservationBL.GetRoomReservationDetails(Id).Select(m => new RoomAllocationModel()
            {
                ID = m.ID,
                Date = General.FormatDate(m.Date, "view"),
                FromDate = General.FormatDate(m.FromDate, "view"),
                ToDate = General.FormatDate(m.ToDate, "view"),
                PatientName=m.PatientName,
                PatientID=m.PatientID,
                RoomType=m.RoomType,
                RoomName=m.RoomName,
                Rate = m.Rate,
                IPID=m.IPID
                
            }).First();
            return View(Model);
        }

        public ActionResult Edit(int Id)
        {
            RoomAllocationModel roomModel = roomReservationBL.GetRoomReservationDetails(Id).Select(m => new RoomAllocationModel()
            {
                ID = m.ID,
                Date = General.FormatDate(m.Date),
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.ToDate),
                PatientName = m.PatientName,
                PatientID = m.PatientID,
                RoomTypeID = m.RoomTypeID,
                RoomName = m.RoomName,
                Rate = m.Rate,
                RoomID=m.RoomID

            }).First();
            roomModel.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
            //roomModel.RoomList = new SelectList(roomReservationBL.GetAllRooms(), "ID", "RoomName");
            roomModel.RoomList = new SelectList(roomReservationBL.GetRoomByID(Id), "ID", "RoomName");
            return View(roomModel);
        }
        public JsonResult GetRoomType()
        {
            List<RoomAllocationBO> RoomType = roomAllocationBL.GetRoomType().ToList();
            return Json(new { Status = "success", data = RoomType }, JsonRequestBehavior.AllowGet);
        }

    }
}