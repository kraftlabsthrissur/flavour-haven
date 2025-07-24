//File created by prama 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class MilkSupplierController : Controller
    {
        IMilkSupplierContract milkSupplierBL;
        public MilkSupplierController()
        {
            milkSupplierBL = new MilkSupplierBL();
        }
        // GET: Masters/MilkSupplier
        public ActionResult Index()
        {
            return View();
        }

        // GET: Masters/MilkSupplier/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Masters/MilkSupplier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Masters/MilkSupplier/Create
        [HttpPost]
        public ActionResult Create(MilkSupplierModel model)
        {
            try
            {
                // TODO: Add insert logic here
                MilkSupplierBO milkSupplier = new MilkSupplierBO();
                milkSupplier.SupplierName = model.SupplierName;
                milkSupplier.ContactNo = model.ContactNo;
                milkSupplier.Address = model.Address;
                var outId = milkSupplierBL.SaveMilkSupplier(milkSupplier);
                if (outId > 0)
                {
                    return Json(new { Status = "success", SupplierID = outId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Masters/MilkSupplier/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Masters/MilkSupplier/Edit/5
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
