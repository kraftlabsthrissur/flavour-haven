using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;



namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class MouldSettingsController : Controller
    {
        private IMouldsettingsContract mouldsettingsBL;
        public MouldSettingsController()
        {
            mouldsettingsBL = new MouldSettingsBL();
        }
        // GET: Masters/MouldSettings
        public ActionResult Index()
        {
            try
            {
                List<MouldSettingsModel> Mould = mouldsettingsBL.GetMachinesForMouldSettings().Select(a => new MouldSettingsModel()
                {
                    ID = a.ID,
                    MachineCode = a.MachineCode,
                    MouldName = a.MouldName,
                    MouldID = a.MouldID,
                    MachineName = a.MachineName
                }).ToList();
                return View(Mould);
            }

            catch (Exception e)
            {
                return View(e);
            }
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            
            else
            {
                MouldSettingsModel mouldSettingsModel;
                MouldSettingsBO mouldSettingsBO = mouldsettingsBL.GetMachinesForMouldSettingsByID((int)id);
                mouldSettingsModel = new MouldSettingsModel()
                {
                    ID = mouldSettingsBO.ID,
                    MachineCode = mouldSettingsBO.MachineCode,
                    MouldName = mouldSettingsBO.MouldName,
                    MouldID = mouldSettingsBO.MouldID,
                    MachineName = mouldSettingsBO.MachineName
                };
               
                return View(mouldSettingsModel);
            }
        }

        public JsonResult GetMouldSettingsByMachine(int ID)
        {
            try
            {
                //List<MouldSettingsHistoryBO> mouldSettingsHistoryBO = mouldsettingsBL.GetMouldSettings(ID);
                MouldSettingsModel mouldSettingsModel=new MouldSettingsModel();
                List<MouldSettingsHistoryBO> mouldSettingsHistoryBO = mouldsettingsBL.GetMouldSettings(ID);
                MouldSettingsHistoryModel Mouldsettings;
                mouldSettingsModel.MouldSettingList = new List<MouldSettingsHistoryModel>();
                foreach (var m in mouldSettingsHistoryBO)
                {
                    Mouldsettings = new MouldSettingsHistoryModel()
                    {
                        Date = General.FormatDate(m.Date),
                        Mould = m.Mould,
                        SettingTime = m.SettingTime,
                        AddorRemove = m.AddorRemove,
                        Reason = m.Reason
                    };
                    mouldSettingsModel.MouldSettingList.Add(Mouldsettings);
                }

                return Json(new { Status = "success", Data = mouldSettingsModel.MouldSettingList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult Save(MouldSettingsModel MouldSettings)
        {
            try
            {
                MouldSettingsBO mouldSettingsBO = new MouldSettingsBO()
                {
                    ID = MouldSettings.ID,
                    MouldID = MouldSettings.MouldID,
                    SettingTime = MouldSettings.SettingTime,
                    Reason = MouldSettings.Reason,
                };
                mouldsettingsBL.Save(mouldSettingsBO);
                return
                     Json(new
                     {
                         Status = "success",
                         data = "",
                         message = ""
                     }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return
                      Json(new
                      {
                          Status = "",
                          data = "",
                          message = e.Message
                      }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}