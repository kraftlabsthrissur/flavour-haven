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
    public class CreditDaysController : Controller
    {
        private ICreditDaysContract creditDaysBL;
        public CreditDaysController()
        {
            creditDaysBL = new CreditDaysBL();
        }

        // GET: Masters/CreditDays
        public ActionResult Index()
        {
            List<CreditDaysModel> CreditDaysList = new List<CreditDaysModel>();
            CreditDaysList = creditDaysBL.GetCreditDaysList().Select(a => new CreditDaysModel
            {
                ID = a.ID,
                Name = a.Name
            }).ToList();
            return View(CreditDaysList);
        }

        public ActionResult Details(int Id)
        {
            CreditDaysModel creditDays = creditDaysBL.GetCreditDaysDetails(Id).Select(m => new CreditDaysModel()
            {
                ID = m.ID,
                Name = m.Name,
                Days = m.Days
            }).First();
            return View(creditDays);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(CreditDaysModel creditDaysModel)
        {
            try
            {
                CreditDaysBO creditDaysBO = new CreditDaysBO()
                {
                    ID = creditDaysModel.ID,
                    Name = creditDaysModel.Name,
                    Days = creditDaysModel.Days
                };
                creditDaysBL.Save(creditDaysBO);
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int Id)
        {
            CreditDaysModel creditDays = creditDaysBL.GetCreditDaysDetails(Id).Select(m => new CreditDaysModel()
            {
                ID = m.ID,
                Name = m.Name,
                Days = m.Days
            }).First();
            return View(creditDays);
        }
    }
}