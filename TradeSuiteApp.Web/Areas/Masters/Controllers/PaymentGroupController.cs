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
    public class PaymentGroupController : Controller
    {
        private IPaymentGroupContract paymentGroupBL;
        public PaymentGroupController()
        {
            paymentGroupBL = new PaymentGroupBL();
        }
        // GET: Masters/PaymentGroup
        public ActionResult Index()
        {
            List<PaymentGroupModel> PaymentGroupList = new List<PaymentGroupModel>();
            PaymentGroupList = paymentGroupBL.GetPaymentGroupList().Select(a => new PaymentGroupModel
            {
                ID = a.ID,
                Name = a.Name,
                PaymentWeek = a.PaymentWeek
            }).ToList();
            return View(PaymentGroupList);
        }
        public ActionResult Details(int Id)
        {
            PaymentGroupModel paymentGroup = paymentGroupBL.GetPaymentGroupDetails(Id).Select(m => new PaymentGroupModel()
            {
                ID = m.ID,
                Name = m.Name,
                PaymentWeek = m.PaymentWeek
            }).First();
            return View(paymentGroup);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(PaymentGroupModel paymentGroupModel)
        {
            try
            {
                PaymentGroupBO paymentGroupBO = new PaymentGroupBO()
                {
                    ID = paymentGroupModel.ID,
                    Name = paymentGroupModel.Name,
                    PaymentWeek = paymentGroupModel.PaymentWeek,

                };
                paymentGroupBL.Save(paymentGroupBO);
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int Id)
        {
            PaymentGroupModel paymentGroup = paymentGroupBL.GetPaymentGroupDetails(Id).Select(m => new PaymentGroupModel()
            {
                ID = m.ID,
                Name = m.Name,
                PaymentWeek = m.PaymentWeek
            }).First();
            return View(paymentGroup);
        }
    }

}
