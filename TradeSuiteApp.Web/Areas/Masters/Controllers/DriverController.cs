using BusinessLayer;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using PresentationContractLayer;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class DriverController : Controller
    {
        private IDriverContract driverBL;
        private IGeneralContract generalBL;
        public DriverController()
        {
            driverBL = new DriverBL();
            generalBL = new GeneralBL();

        }
        // GET: Masters/Driver
        public ActionResult Index()
        {
            List<DriverModel> driverList = new List<DriverModel>();
            driverList = driverBL.GetDriverList().Select(a => new DriverModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Address = a.Address,
                LicenseNo = a.LicenseNo,
                PhoneNo = a.PhoneNo
            }).ToList();
            return View(driverList);
        }

        // GET: Masters/Driver/Details/5
        public ActionResult Details(int id)
        {
            DriverModel driver = driverBL.GetDriverDetails(id).Select(a => new DriverModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                LicenseNo = a.LicenseNo,
                Address = a.Address,
                PhoneNo = a.PhoneNo
            }).First();

            return View(driver);
        }

        // GET: Masters/Driver/Create
        public ActionResult Create()
        {
            DriverModel Driver = new DriverModel();
            return View(Driver);
        }

        // POST: Masters/Driver/Save
        [HttpPost]
        public ActionResult Save(DriverModel modal)
        {
            try
            {
                DriverBO driver = new DriverBO()
                {
                    ID = modal.ID,
                    Code = modal.Code,
                    Name = modal.Name,
                    Address = modal.Address,
                    LicenseNo = modal.LicenseNo,
                    PhoneNo = modal.PhoneNo,
                    IsActive = modal.IsActive
                };
                if (driver.ID == 0)
                {
                    driverBL.SaveDriver(driver);
                }
                else
                {
                    driverBL.UpdateDriver(driver);
                }
                return Json(new { Status = "Success", Message = "Driver Created Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        // GET: Masters/Driver/Edit/5
        public ActionResult Edit(int id)
        {
            DriverModel driver = driverBL.GetDriverDetails(id).Select(a => new DriverModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                LicenseNo = a.LicenseNo,
                Address = a.Address,
                PhoneNo = a.PhoneNo
            }).First();
            return View(driver);
        }

        // POST: Masters/Driver/Edit/5
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
