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
    public class SupplierAccountsCategoryController : Controller
    {
        private ISupplierAccountsCategoryContract supplierAccountsCategoryBL;

        public SupplierAccountsCategoryController()
        {
            supplierAccountsCategoryBL = new SupplierAccountsCategoryBL();
        }

        public ActionResult Index()
        {
            List<SupplierAccountCategoryModel> Supplieraccountcategoryitem = supplierAccountsCategoryBL.GetAllSupplierAccountsCategory().Select(a => new SupplierAccountCategoryModel()
            {
                ID = a.ID,
                Name = a.Name
            }).ToList();

            return View(Supplieraccountcategoryitem);

        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(SupplierAccountCategoryModel Model)
        {
            try
            {
                SupplierAccountsCategoryBO SupplieraccountscategoryBO = new SupplierAccountsCategoryBO()
                {
                    ID = Model.ID,
                    Name = Model.Name

                };
                supplierAccountsCategoryBL.Save(SupplieraccountscategoryBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int? ID)
        {

            var obj = supplierAccountsCategoryBL.GetSupplierAccountsCategoryDetails((int)ID);
            SupplierAccountCategoryModel model = new SupplierAccountCategoryModel();
            model.ID = obj.ID;
            model.Name = obj.Name;
            return View(model);

        }

        public ActionResult Edit(int ID)
        {
            var obj = supplierAccountsCategoryBL.GetSupplierAccountsCategoryDetails(ID);
            SupplierAccountCategoryModel model = new SupplierAccountCategoryModel();
            model.ID = obj.ID;
            model.Name = obj.Name;
            return View(model);
        }

    }
}