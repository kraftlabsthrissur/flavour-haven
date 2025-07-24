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
    public class ProcessController : Controller
    {
        private IProcessContract processBL;

        public ProcessController()
        {
            processBL = new ProcessBL();
        }

        // GET: Masters/Process
        public ActionResult Index()
        {
            List<ProcessModel> ProcessList = new List<ProcessModel>();
            ProcessList = processBL.GetProcessList().Select(a => new ProcessModel
            {
                ID = a.ID,
                Code = a.Code,
                Process = a.Process

            }).ToList();

            return View(ProcessList);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(ProcessModel model)
        {
            try
            {
                ProcessBO processBO = new ProcessBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Process = model.Process
                };
                processBL.Save(processBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            var obj = processBL.GetProcessDetails((int)ID);
            ProcessModel model = new ProcessModel();
            model.ID = obj.ID;
            model.Process = obj.Process;
            model.Code = obj.Code;
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var obj = processBL.GetProcessDetails((int)ID);
            ProcessModel model = new ProcessModel();
            model.ID = obj.ID;
            model.Process = obj.Process;
            model.Code = obj.Code;
            return View(model);
        }
    }
}