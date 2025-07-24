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
    public class PaymentDaysController : Controller
    {
        private IPaymentDaysContract paymentDaysBL;
        public PaymentDaysController()
        {
            paymentDaysBL = new PaymentDaysBL();
        }

        // GET: Masters/PaymentDays
        public ActionResult Index()
        {
            List<PaymentDaysModel> PaymentDaysList = new List<PaymentDaysModel>();
            PaymentDaysList = paymentDaysBL.GetPaymentDaysList().Select(a => new PaymentDaysModel
            {
                ID = a.ID,
                Name = a.Name
            }).ToList();
            return View(PaymentDaysList);
        }

        public ActionResult Details(int Id)
        {
            PaymentDaysModel paymentDays = paymentDaysBL.GetPaymentDaysDetails(Id).Select(m => new PaymentDaysModel()
            {
                ID = m.ID,
                Name = m.Name,
                Days = m.Days
            }).First();
            return View(paymentDays);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(PaymentDaysModel paymentDaysModel)
        {
            try
            {
                PaymentDaysBO paymentDaysBO = new PaymentDaysBO()
                {
                    ID = paymentDaysModel.ID,
                    Name = paymentDaysModel.Name,
                    Days = paymentDaysModel.Days,

                };
                paymentDaysBL.Save(paymentDaysBO);
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int Id)
        {
            PaymentDaysModel paymentDays = paymentDaysBL.GetPaymentDaysDetails(Id).Select(m => new PaymentDaysModel()
            {
                ID = m.ID,
                Name = m.Name,
                Days = m.Days
            }).First();
            return View(paymentDays);
        }

    }
}