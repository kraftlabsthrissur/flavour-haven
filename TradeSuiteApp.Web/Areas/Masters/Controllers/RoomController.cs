using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class RoomController : Controller
    {
        private ISubmasterContract subMasterBL;
        private IGeneralContract generalBL;
        private IRoomContract roomBL;
        private IConfigurationContract configurationBL;
        public RoomController()
        {
            subMasterBL = new SubmasterBL();
            generalBL = new GeneralBL();
            roomBL = new RoomBL();
            configurationBL = new ConfigurationBL();
        }
        // GET: Masters/Room
        public ActionResult Index()
        {
            List<RoomModel> RoomList = new List<RoomModel>();
            RoomList = roomBL.GetRoomList().Select(a => new RoomModel
            {
                ID = a.ID,
               Code=a.Code,
               RoomName=a.RoomName,
               StartDate= General.FormatDate(a.StartDate),
               EndDate= General.FormatDate(a.EndDate)
            }).ToList();
            return View(RoomList);
        }
        public ActionResult Create()
        {
            RoomModel roomModel = new RoomModel();
            roomModel.StartDate = General.FormatDate(DateTime.Now);
            roomModel.RoomTypeList=new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
            roomModel.StoreList = new SelectList(configurationBL.GetDefaultStoreList(1), "StoreID", "StoreName");
            roomModel.Code = generalBL.GetSerialNo("Room", "Code");
            return View(roomModel);
        }

        public ActionResult Save(RoomModel model)
        {
            try
            {
                RoomBO Room = new RoomBO()
                {
                   ID=model.ID,
                   Code=model.Code,
                   RoomTypeID=model.RoomTypeID,
                   RoomName=model.RoomName,
                   StartDate=General.ToDateTime(model.StartDate),
                   EndDate= General.ToDateTime(model.EndDate),
                   Description=model.Description,
                   Rate=model.Rate,
                   StoreID=model.StoreID
                };
                if (Room.ID == 0)
                {
                    roomBL.Save(Room);
                }
                else
                {
                    roomBL.UpdateRoom(Room);
                }
                return Json(new { Status = "Success", Message = "Room Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Room Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save Room failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int Id)
        {
            RoomModel room = roomBL.GetRoomDetails(Id).Select(m => new RoomModel()
            {
                ID = m.ID,
                Code=m.Code,
                RoomType=m.RoomType,
                RoomName=m.RoomName,
                StartDate= General.FormatDate(m.StartDate,"view"),
                EndDate= General.FormatDate(m.EndDate,"view"),
                Description = m.Description,
                Rate=m.Rate,
                Store=m.Store
            }).First();
            return View(room);
        }

        public ActionResult Edit(int Id)
        {
            RoomModel roomModel = roomBL.GetRoomDetails(Id).Select(m => new RoomModel()
            {
                ID = m.ID,
                Code = m.Code,
                RoomTypeID = m.RoomTypeID,
                RoomName = m.RoomName,
                StartDate = General.FormatDate(m.StartDate),
                EndDate = General.FormatDate(m.EndDate),
                Description = m.Description,
                Rate = m.Rate,
                StoreID=m.StoreID

            }).First();
            roomModel.RoomTypeList = new SelectList(subMasterBL.GetRoomTypeList(), "ID", "Name");
            roomModel.StoreList = new SelectList(configurationBL.GetDefaultStoreList(1), "StoreID", "StoreName");
            return View(roomModel);
        }

    }
}