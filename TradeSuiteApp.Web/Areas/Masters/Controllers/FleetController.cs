using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class FleetController : Controller
    {
        private IFleetContract fleetBL;
        public FleetController()
        {
            fleetBL = new FleetBL();
        }
        // GET: Masters/Fleet
        public ActionResult Index()
        {
            List<FleetModel> FleetList = new List<FleetModel>();
            FleetList = fleetBL.GetFleetList().Select(a => new FleetModel
            {
                ID = a.ID,
                VehicleNo = a.VehicleNo,
                VehicleName = a.VehicleName,
                DriverName = a.DriverName
            }).ToList();
            return View(FleetList);
        }

        // GET: Masters/Fleet/Details/5
        public ActionResult Details(int id)
        {
            FleetModel fleet = fleetBL.GetFleetDetails(id).Select(a => new FleetModel()
            {
                ID= a.ID,
                VehicleNo = a.VehicleNo,
                VehicleName =a.VehicleName,
                DriverName = a.DriverName,
                OtherDetails = a.OtherDetails,
                PurchaseDate = General.FormatDate(a.PurchaseDate,"view"),
                PermitExpairyDate =General.FormatDate(a.PermitExpairyDate,"view"),
                PolicyNo = a.PolicyNo,
                BagCapacity =a.BagCapacity,
                BoxCapacity =a.BoxCapacity,
                CanCapacity = a.CanCapacity,
                InsuranceCompany = a.InsuranceCompany,
                InsuranceExpairyDate = General.FormatDate(a.InsuranceExpairyDate,"view"),
                LicenseNo = a.LicenseNo,
                OwnerName = a.OwnerName,
                TaxExpairyDate = General.FormatDate(a.TaxExpairyDate,"view"),
                TestExpairyDate = General.FormatDate(a.TestExpairyDate,"view"),
                TravellingAgency = a.TravellingAgency,
            }).First();

            return View(fleet);
        }

        // GET: Masters/Fleet/Create
        public ActionResult Create()
        {
            FleetModel Fleet = new FleetModel();
            Fleet.PermitExpairyDate = General.FormatDate(DateTime.Now);
            Fleet.PurchaseDate = General.FormatDate(DateTime.Now);
            Fleet.TaxExpairyDate = General.FormatDate(DateTime.Now);
            Fleet.TestExpairyDate = General.FormatDate(DateTime.Now);
            Fleet.InsuranceExpairyDate = General.FormatDate(DateTime.Now);
            return View(Fleet);
        }

        // POST: Masters/Fleet/Save
        [HttpPost]
        public ActionResult Save(FleetModel model)
        {
          
            try
            {
                FleetBO fleet = new FleetBO()
                {
                    ID = model.ID,
                    VehicleNo = model.VehicleNo,
                    VehicleName = model.VehicleName,
                    DriverName =model.DriverName,
                    LicenseNo = model.LicenseNo,
                    PolicyNo  = model.PolicyNo,
                    OwnerName = model.OwnerName,
                    TravellingAgency = model.TravellingAgency,
                    InsuranceCompany = model.InsuranceCompany,
                    OtherDetails = model.OtherDetails,
                    PurchaseDate = General.ToDateTime(model.PurchaseDate),
                    PermitExpairyDate = General.ToDateTime(model.PermitExpairyDate),
                    TaxExpairyDate = General.ToDateTime(model.TaxExpairyDate),
                    TestExpairyDate= General.ToDateTime(model.TestExpairyDate),
                    InsuranceExpairyDate = General.ToDateTime(model.InsuranceExpairyDate),
                    BagCapacity =model.BagCapacity,
                    BoxCapacity =model.BoxCapacity,
                    CanCapacity= model.CanCapacity
                };
                if (fleet.ID == 0)
                {
                    fleetBL.Save(fleet);
                }
                else
                {
                    fleetBL.UpdateFleet(fleet);
                }
                return Json(new { Status = "Success", Message = "Fleet Created Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Status = "failure", Message = "Fleet creation Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Masters/Fleet/Edit/5
        public ActionResult Edit(int id)
        {
            FleetModel fleet = fleetBL.GetFleetDetails(id).Select(a => new FleetModel()
            {
                ID = a.ID,
                VehicleNo = a.VehicleNo,
                VehicleName = a.VehicleName,
                DriverName = a.DriverName,
                OtherDetails = a.OtherDetails,
                PurchaseDate = General.FormatDate(a.PurchaseDate),
                PermitExpairyDate = General.FormatDate(a.PermitExpairyDate),
                PolicyNo = a.PolicyNo,
                BagCapacity = a.BagCapacity,
                BoxCapacity = a.BoxCapacity,
                CanCapacity = a.CanCapacity,
                InsuranceCompany = a.InsuranceCompany,
                InsuranceExpairyDate = General.FormatDate(a.InsuranceExpairyDate),
                LicenseNo = a.LicenseNo,
                OwnerName = a.OwnerName,
                TaxExpairyDate = General.FormatDate(a.TaxExpairyDate),
                TestExpairyDate = General.FormatDate(a.TestExpairyDate),
                TravellingAgency = a.TravellingAgency,
            }).First();
            return View(fleet);
        }

        // POST: Masters/Fleet/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
       
    }
}
