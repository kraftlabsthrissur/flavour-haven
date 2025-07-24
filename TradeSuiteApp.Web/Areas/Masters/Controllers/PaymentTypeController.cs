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
    public class PaymentTypeController : Controller
    {
        private IPaymentTypeContract paymentTypeBL;
        public PaymentTypeController()
        {
            paymentTypeBL = new PaymentTypeBL();
        }
        // GET: Masters/PaymentType
        public ActionResult Index()
        {

            List<PaymentTypeModel> PaymentTypeList = new List<PaymentTypeModel>();
            PaymentTypeList = paymentTypeBL.GetPaymentTypeList().Select(a => new PaymentTypeModel
            {
                ID = a.ID,            
                Name = a.Name
            }).ToList();

            return View(PaymentTypeList);
       
        }
        // GET: Masters/PaymentType/Details
        public ActionResult Details(int Id)
        {
            PaymentTypeModel PaymentType = paymentTypeBL.GetPaymentTypeDetails(Id).Select(m => new PaymentTypeModel()
            {
                ID = m.ID,
                Name = m.Name,

            }).First();
            return View(PaymentType);
        }

        // GET: Masters/PaymentType/Edit
        public ActionResult Edit(int Id)
        {
            PaymentTypeModel PaymentType = paymentTypeBL.GetPaymentTypeDetails(Id).Select(m => new PaymentTypeModel()
            {
                ID = m.ID,
                Name = m.Name,
            }).First();
            return View(PaymentType);
        }
        // GET: Masters/PaymentType/Create
        public ActionResult Create()
        {

            return View();
        }
        public ActionResult Save(DepartmentModel model)
        {
            try
            {
                PaymentTypeBO PaymentType = new PaymentTypeBO()
                {
                    ID = model.ID,
                    Name = model.Name,

                };
                if (PaymentType.ID == 0)
                {
                    paymentTypeBL.Save(PaymentType);
                }
                else
                {
                    paymentTypeBL.Update(PaymentType);
                }
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}