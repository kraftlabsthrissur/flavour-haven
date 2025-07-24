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

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class DiscountCategoryController : Controller
    {
        private IDiscountCategoryContract discoucategorytBL;

        public DiscountCategoryController()
        {
            discoucategorytBL = new DiscountCategoryBL();

        }
        // GET: Masters/DiscountCategory
        public ActionResult Index()
        {
            List<DiscountCategoryModel> DiscountCategoryList = new List<DiscountCategoryModel>();
            DiscountCategoryList = discoucategorytBL.GetDiscountCategoryList().Select(a => new DiscountCategoryModel
            {
                ID = a.ID,
                DiscountPercentage = a.DiscountPercentage,
                DiscountType = a.DiscountType,
                Days = a.Days
            }).ToList();

            return View(DiscountCategoryList);
        }
        public ActionResult Create()
        {
            DiscountCategoryModel discountCategoryModel = new DiscountCategoryModel();
            discountCategoryModel.DiscountTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Discount", Value ="Discount", },
                                                 new SelectListItem { Text = "CashDiscount", Value ="CashDiscount", },
                                                 }, "Value", "Text");
            return View(discountCategoryModel);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
              
                DiscountCategoryBO DiscountCategoryBO = discoucategorytBL.GetDiscountCategoryDetails((int)id);
                DiscountCategoryModel discountCategoryModel = new DiscountCategoryModel()
                {
                    DiscountType = DiscountCategoryBO.DiscountType,
                    ID = DiscountCategoryBO.ID,
                    DiscountPercentage = DiscountCategoryBO.DiscountPercentage,
                    Days = DiscountCategoryBO.Days
                };
                discountCategoryModel.DiscountTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Discount", Value ="Discount", },
                                                 new SelectListItem { Text = "CashDiscount", Value ="CashDiscount", },
                                                 }, "Value", "Text");
                return View(discountCategoryModel);
            }
        }
        public ActionResult Details(int? id)
        {

            DiscountCategoryBO DiscountCategoryBO = discoucategorytBL.GetDiscountCategoryDetails((int)id);
            DiscountCategoryModel discountCategoryModel = new DiscountCategoryModel()
            {
                DiscountType = DiscountCategoryBO.DiscountType,
                ID = DiscountCategoryBO.ID,
                DiscountPercentage = DiscountCategoryBO.DiscountPercentage,
                Days = DiscountCategoryBO.Days
            };
            return View(discountCategoryModel);
        }
        public ActionResult Save(DiscountCategoryModel model)
        {
            try
            {
                DiscountCategoryBO discountCategoryBO = new DiscountCategoryBO()
                {
                    ID = model.ID,
                    DiscountType = model.DiscountType,
                    DiscountPercentage = model.DiscountPercentage,
                    Days = model.Days
                };
                discoucategorytBL.Save(discountCategoryBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}