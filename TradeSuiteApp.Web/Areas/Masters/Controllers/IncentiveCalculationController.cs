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
    public class IncentiveCalculationController : Controller
    {
        private ISubmasterContract submasterBL;
        private IIncentiveCalculationContract incentiveBL;
        public IncentiveCalculationController()
        {
            submasterBL = new SubmasterBL();
            incentiveBL = new IncentiveCalculationBL();
        }
        // GET: Masters/IncentiveCalculation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            IncentiveCalculationModel incentiveModel = new IncentiveCalculationModel();
            incentiveModel.DurationList = new SelectList(submasterBL.GetDurationList(), "ID", "Name");
            incentiveModel.TimePeriodList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Select", Value ="0", }
                                                 }, "Value", "Text");
            incentiveModel.PartyList=new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "FSO", Value ="FSO", },
                                                 new SelectListItem { Text = "Managers", Value ="Managers", },
                                                 new SelectListItem { Text = "Branches", Value ="Branches", },
                                                 }, "Value", "Text");
            return View(incentiveModel);
        }
        public JsonResult GetTimePeriodList(int DurationID)
        {
            List<SubmasterBO> District = submasterBL.GetTimePeriodList(DurationID).ToList();
            return Json(new { Status = "success", data = District }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCalculatedIncentives(int DurationID, int TimePeriodID, string PartyType)
        {
            try
            {
                List<IncentiveCalculationBO> incentivelist = incentiveBL.GetCalculatedIncentives( DurationID, TimePeriodID, PartyType);
                return Json(new { Status = "success", Data = incentivelist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Save()
        {
            return View();
        }
    }
}