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
    public class PaymentModeController : Controller
    {
        private IPaymentModeContract paymentModeBL;
        public PaymentModeController()
        {
            paymentModeBL = new PaymentModeBL();
        }
        // GET: Masters/PaymentMode
        public ActionResult Index()
        {
            List<PaymentModeModel> PaymentModeList = new List<PaymentModeModel>();
            PaymentModeList = paymentModeBL.GetPaymentModeList().Select(a => new PaymentModeModel
            {
                ID = a.ID,           
                Name = a.Name
            }).ToList();

            return View(PaymentModeList);        
        }
        // GET: Masters/PaymentMode/Create
        public ActionResult Create()
        {
           
            return View();
        }
        // GET: Masters/PaymentMode/Details
        public ActionResult Details(int Id)
        {
            PaymentModeModel PaymentMode = paymentModeBL.GetPaymentModeDetails(Id).Select(m => new PaymentModeModel()
            {
                ID = m.ID,         
                Name = m.Name,
               
            }).First();
            return View(PaymentMode);
        }

        // GET: Masters/PaymentMode/Edit
        public ActionResult Edit(int Id)
        {
            PaymentModeModel PaymentMode = paymentModeBL.GetPaymentModeDetails(Id).Select(m => new PaymentModeModel()
            {
                ID = m.ID,               
                Name = m.Name, 
            }).First();          
            return View(PaymentMode);
        }
        public ActionResult Save(DepartmentModel model)
        {
            try
            {
                PaymentModeBO PaymentMode = new PaymentModeBO()
                {
                    ID = model.ID,              
                    Name = model.Name,                   

                };
                if (PaymentMode.ID == 0)
                {
                    paymentModeBL.Save(PaymentMode);
                }
                else
                {
                    paymentModeBL.Update(PaymentMode);
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