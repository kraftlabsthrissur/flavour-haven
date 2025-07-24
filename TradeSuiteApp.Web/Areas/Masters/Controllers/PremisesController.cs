//File created by prama on 14-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class PremisesController : Controller
    {
        IPremisesContract premisesBL;
        public PremisesController()
        {           
            premisesBL = new PremisesBL();
        }

        public JsonResult GetPremisesWithItemID(int id)
        {
            List<PremisesBO> Premises = premisesBL.GetPremisesWithItemID(id).ToList();
            return Json(new { Status = "success", data = Premises }, JsonRequestBehavior.AllowGet);
        }
        // GET: Masters/Premises
        public ActionResult Index()
        {
            return View();
        }

        // GET: Masters/Premises/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Masters/Premises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Masters/Premises/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Masters/Premises/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
