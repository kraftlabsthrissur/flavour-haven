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
    public class PowerConsumptionController : Controller
    {

        private IPowerConsumptionContract powerConsumptionBL;


        public PowerConsumptionController()
        {
            powerConsumptionBL = new PowerConsumptionBL();
        }

        // GET: Masters/PowerConsumption
        public ActionResult Index()
        {
            List<PowerConsumptionModel> PowerConsumptionList = new List<PowerConsumptionModel>();
            PowerConsumptionList = powerConsumptionBL.GetLocationList().Select(a => new PowerConsumptionModel
            {
                Location = a.Location,
                LocationName = a.LocationName,
            }).ToList();
            return View(PowerConsumptionList);
        }

        public ActionResult Create()
        {
            PowerConsumptionModel PowerConsumption = new PowerConsumptionModel();
            PowerConsumption.LocationList = new SelectList(powerConsumptionBL.GetLocationList(), "Location", "LocationName");
            return View(PowerConsumption);
        }

        public ActionResult Save(PowerConsumptionModel model)
        {
            try
            {
                PowerConsumptionBO powerConsumptionBO = new PowerConsumptionBO()
                {
                    ID=model.ID,
                    Location = model.Location
                };
                List<PowerConsumptionItemBO> Items = new List<PowerConsumptionItemBO>();
                    PowerConsumptionItemBO powerConsumptionItem;
                    foreach (var item in model.Items)
                    {
                        powerConsumptionItem = new PowerConsumptionItemBO()
                        {
                            Amount = item.Amount,
                            Time = item.Time
                        };
                        Items.Add(powerConsumptionItem);
                    }
                powerConsumptionBL.Save(Items, powerConsumptionBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            PowerConsumptionModel model = powerConsumptionBL.GetPowerConsumptionDetails(ID).Select(m => new PowerConsumptionModel()
            {
                Location = m.Location,
                LocationName = m.LocationName,
            }).First();

            model.Items = powerConsumptionBL.GetPowerConsumptionTransDetails(ID).Select(m => new PowerConsumptionItemModel()
            {
                Time=m.Time,
                Amount=m.Amount
            }).ToList();
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            PowerConsumptionModel model = powerConsumptionBL.GetPowerConsumptionDetails(ID).Select(m => new PowerConsumptionModel()
            {
                ID=m.ID,
                Location = m.Location,
                LocationName = m.LocationName,
                LocationList = new SelectList(powerConsumptionBL.GetLocationList(), "Location", "LocationName")
        }).First();

            model.Items = powerConsumptionBL.GetPowerConsumptionTransDetails(ID).Select(m => new PowerConsumptionItemModel()
            {
                Time = m.Time,
                Amount = m.Amount
            }).ToList();
            return View(model);
        }
    }
}