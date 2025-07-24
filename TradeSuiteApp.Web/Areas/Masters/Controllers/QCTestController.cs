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
    public class QCTestController : Controller
    {
        private IQCTestContract QctestBL;

        public QCTestController()
        {
            QctestBL = new QCTestBL();
        }

        public ActionResult Index()
        {
            List<QctestModel> TestList = new List<QctestModel>();
            TestList = QctestBL.GetQCTestList().Select(a => new QctestModel
            {
                ID = a.ID,
                TestName = a.TestName,
                Type=a.Type
               
            }).ToList();

            return View(TestList);
           
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(QctestModel model)
        {
            try
            {
                QCTestBO QctestBO = new QCTestBO()
                {
                    ID = model.ID,
                    TestName = model.TestName,
                    Type=model.Type
                   
                };
                QctestBL.Save(QctestBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int? ID)
        {
            var obj = QctestBL.GetQCTestDetails((int)ID);
            QctestModel model = new QctestModel();
            model.ID = obj.ID;
            model.TestName = obj.TestName;
            model.Type = obj.Type;
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var obj = QctestBL.GetQCTestDetails((int)ID);
            QctestModel model = new QctestModel();
            model.ID = obj.ID;
            model.TestName = obj.TestName;
            model.Type = obj.Type;
            return View(model);
        }
    }
}