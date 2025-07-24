using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.AHCMS.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.AHCMS.Controllers
{
    public class RoomChangeController : Controller
    {
        private IRoomAllocationContract roomAllocationBL;
        private IRoomReservationContract roomReservationBL;
        private ISubmasterContract subMasterBL;
        private IRoomChangeContract roomChangeBL;

        public RoomChangeController()
        {
            roomAllocationBL = new RoomAllocationBL();
            roomReservationBL = new RoomReservationBL();
            subMasterBL = new SubmasterBL();
            roomChangeBL = new RoomChangeBL();
        }
        // GET: AHCMS/RoomChange
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int RoomStatusID)
        {
            RoomReservationModel roomAllocationModel = new RoomReservationModel();
            var obj = roomAllocationBL.GetAllocatedRoomDetails(RoomStatusID);
            roomAllocationModel.RoomStatusID = RoomStatusID;
            roomAllocationModel.IPID = obj.IPID;
            roomAllocationModel.AdmissionDate = General.FormatDate(obj.AdmissionDate);
            roomAllocationModel.AdmissionDateTill = General.FormatDate(obj.AdmissionDateTill);
            roomAllocationModel.PatientID = obj.PatientID;
            roomAllocationModel.PatientName = obj.PatientName;
            roomAllocationModel.ByStander = obj.ByStander;
            roomAllocationModel.MobileNumber = obj.MobileNumber;
            roomAllocationModel.RoomChangeDate = General.FormatDate(DateTime.Now);
            roomAllocationModel.DoctorID = obj.DoctorID;
            //roomAllocationModel.RoomList = new SelectList(roomReservationBL.GetAllRooms(), "ID", "RoomName");
            //roomAllocationModel.RoomList = new SelectList(roomReservationBL.GetAllocatedRoom(RoomStatusID), "ID", "RoomName");
            roomAllocationModel.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
            return View(roomAllocationModel);
        }
        public PartialViewResult RoomAllocationDetails(int IPID)
        {
            RoomReservationModel model = new RoomReservationModel();
            model.RoomItems = roomAllocationBL.GetAllocatedRoomDetailsByID(IPID).Select(m => new IpRoomModel()
            {
                FromDate = General.FormatDate(m.FromDate),
                ToDate = General.FormatDate(m.ToDate),
                RoomTypeID = m.RoomTypeID,
                RoomID =m.RoomID,
                RoomName = m.RoomName,
                RoomType = m.RoomType,
                Rate=m.Rate,
            }).ToList();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Save(RoomReservationModel model)
        {
            try
            {
                List<IpRoomBO> RoomItems  = new List<IpRoomBO>();
                if (model.RoomItems != null)
                {
                    IpRoomBO Item;

                    foreach (var item in model.RoomItems)
                    {
                        Item = new IpRoomBO()
                        {
                            FromDate=General.ToDateTime(item.FromDate),
                            ToDate=General.ToDateTime(item.ToDate),
                            RoomID=item.RoomID,
                            RoomTypeID=item.RoomTypeID,
                            PatientID=item.PatientID,
                            DoctorID=item.DoctorID,
                            RoomStatusID=item.RoomStatusID

                        };
                        RoomItems.Add(Item);
                    }
                }
                roomChangeBL.Save(RoomItems);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}