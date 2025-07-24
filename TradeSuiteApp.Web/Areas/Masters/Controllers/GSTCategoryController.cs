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
    public class GSTCategoryController : Controller
    {      
        private IGSTCategoryContract gstCategoryBL;
        public GSTCategoryController()
        {
            gstCategoryBL = new GSTCategoryBL();
        }

        // GET: Masters/GSTCategory
        public ActionResult Index()
        {
            List<GSTCategoryModel> GSTCategoryList = new List<GSTCategoryModel>();
            GSTCategoryList = gstCategoryBL.GetGSTCategoryList().Select(a => new GSTCategoryModel
            {
                ID = a.ID,
                Name = a.Name
            }).ToList();           

            return View(GSTCategoryList);
        }
        // GET: Masters/GSTCategory/Create
        public ActionResult Create()
        {           
           
            return View();
        }
        // Post: Masters/GSTCategory/Save
        public ActionResult Save(GSTCategoryModel model)
        {
            try
            {
                GSTCategoryBO GSTCategory = new GSTCategoryBO()
                {
                    ID = model.ID,                   
                    Name = model.Name,                    
                    IGSTPercent=model.IGSTPercent,
                    CGSTPercent=model.CGSTPercent,
                    SGSTPercent=model.SGSTPercent
                };
                if (GSTCategory.ID == 0)
                {
                    gstCategoryBL.Save(GSTCategory);
                }
                else
                {
                   gstCategoryBL.Update(GSTCategory);
                }
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }           
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save GSTCategory failed" }, JsonRequestBehavior.AllowGet);
            }

        }
        // GET: Masters/GSTCategory/Details
        public ActionResult Details(int Id)
        {
            GSTCategoryModel GSTCategory = gstCategoryBL.GetGSTCategoryDetails(Id).Select(m => new GSTCategoryModel()
            {
                ID = m.ID,               
                Name = m.Name,
                IGSTPercent = m.IGSTPercent,
                CGSTPercent = m.CGSTPercent,
                SGSTPercent = m.SGSTPercent
            }).First();

            return View(GSTCategory);
        }
        public ActionResult Edit(int Id)
        {
            GSTCategoryModel GSTCategory = gstCategoryBL.GetGSTCategoryDetails(Id).Select(m => new GSTCategoryModel()
            {
                ID = m.ID,
                Name = m.Name,
                IGSTPercent = m.IGSTPercent,
                CGSTPercent = m.CGSTPercent,
                SGSTPercent = m.SGSTPercent
            }).First();

            return View(GSTCategory);
        }

        public JsonResult GetDropdownVal()
        {
            List<GSTCategoryBO> Gst = new List<GSTCategoryBO>();
            //{
            //    new GSTCategoryBO()
            //    {
            //        ID = 0,
            //        IGSTPercent = 0
            //    }
            //};
            Gst.AddRange(gstCategoryBL.GetGSTList());
            return Json(new { Status = "success", data = Gst }, JsonRequestBehavior.AllowGet);
        }
    }
}