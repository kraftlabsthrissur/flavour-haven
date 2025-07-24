using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class ConfigurationController : Controller
    {
        private IConfigurationContract configurationBL;

        public ConfigurationController()
        {
            configurationBL = new ConfigurationBL();
        }

        // GET: Masters/Configuration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(ConfigurationModel model)
        {
            try
            {
                ConfigurationBO configurationBO = new ConfigurationBO()
                {
                    LocationID=model.LocationID,
                    StoreName=model.StoreName,
                    StoreID =model.StoreID,
                    UserID =model.UserID,
                };
                configurationBL.Save(configurationBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDefaultStoreList(int LocationID,int UserID)
        {
            try
            {
                List<ConfigurationBO> DefaultStoreList = configurationBL.GetDefaultStoreList(LocationID);
                int SelectedStoreID = configurationBL.GetSelectedStore(LocationID, UserID);
                return Json(new { Status = "success", data = DefaultStoreList, SelectedStoreID = SelectedStoreID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}