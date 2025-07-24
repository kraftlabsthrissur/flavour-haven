using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class SupplierCategoryController : Controller
    {
        private ISupplierCategoryContract supplierCategoryBL;

        public SupplierCategoryController()
        {
            supplierCategoryBL = new SupplierCategoryBL();
        }

        public ActionResult Index()
        {
            List<SupplierCategoryModel> Suppliercategoryitem = supplierCategoryBL.GetAllSupplierCategory().Select(a => new SupplierCategoryModel()
            {
                ID = a.ID,
                Name = a.Name,
                Remarks = a.Remarks
            }).ToList();

            return View(Suppliercategoryitem);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(SupplierCategoryModel model)
        {
            try
            {
                SupplierCategoryBO SuppliercategoryBO = new SupplierCategoryBO()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Remarks = model.Remarks

                };
                supplierCategoryBL.Save(SuppliercategoryBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int? ID)
        {
            var obj = supplierCategoryBL.GetSupplierCategoryDetails((int)ID);
            SupplierCategoryModel model = new SupplierCategoryModel();
            model.ID = obj.ID;
            model.Name = obj.Name;
            model.Remarks = obj.Remarks;
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var obj = supplierCategoryBL.GetSupplierCategoryDetails(ID);
            SupplierCategoryModel model = new SupplierCategoryModel();
            model.ID = obj.ID;
            model.Name = obj.Name;
            model.Remarks = obj.Remarks;
            return View(model);
        }
    }
}