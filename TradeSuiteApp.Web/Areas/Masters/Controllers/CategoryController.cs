using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationContractLayer;
using BusinessLayer;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessObject;


namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class CategoryController : Controller
    {
        #region private members
        
        #endregion
        #region private Declarations

        private ICategoryContract categoryBL;

        #endregion

        #region Constructors

        public CategoryController()
        {
           
            categoryBL = new CategoryBL();
           
        }
        #endregion
        // GET: Masters/Category
        public ActionResult Index()
        {
            List<CategoryModel> categoryList = new List<CategoryModel>();

            categoryList = categoryBL.GetCategoryList().Select(a => new CategoryModel()
            {
                ID = a.ID,
                Name = a.Name,
                CategoryGroupID = a.CategoryGroupID,
                GroupName=a.GroupName                
            }).ToList();           
            return View(categoryList);           
        }       

        // GET: Masters/Category/Details/5
        public ActionResult Details(int id)
        {            
            var obj = categoryBL.GetCategoryDetails(id);
            CategoryModel categoryModel = new CategoryModel();
            categoryModel.ID = obj.ID;
            categoryModel.Name = obj.Name;
            categoryModel.CategoryGroupID = obj.CategoryGroupID;
            categoryModel.GroupName = obj.GroupName;
            return View(categoryModel);
        }

        //GET : Masters/Category/Create
        public ActionResult Create()
        {        
            CategoryModel categoryModel = new CategoryModel();            
            categoryModel.CategoryGroup = new SelectList(categoryBL.GetCategoryGroup(), "ID", "Name"); /*obj.GroupName;*/
            return View(categoryModel);            
        }

        //POST: Masters/Unit/Create
       [HttpPost]
        public ActionResult Save(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CategoryBO categoryBO = new CategoryBO()
                    {                                               
                        Name = model.Name,
                        CategoryGroupID = model.CategoryGroupID ,
                        GroupName = model.GroupName,
                    };
                    categoryBL.CreateCategory(categoryBO);
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    var res = new List<object>();
                                        return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }            
        }

        // GET: Masters/Unit/Edit/5
        public ActionResult Edit(int id)
        {
            var obj = categoryBL.GetCategoryDetails(id);
            CategoryModel categoryModel = new CategoryModel();
            categoryModel.ID = obj.ID;
            categoryModel.Name = obj.Name;
            categoryModel.CategoryGroupID = obj.CategoryGroupID;
            categoryModel.CategoryGroup = new SelectList(categoryBL.GetCategoryGroup(), "ID", "Name");
            return View(categoryModel);  
        }

        //POST: Masters/Unit/Edit/5
        [HttpPost]
        public ActionResult Edit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CategoryBO categoryBO = new CategoryBO()
                    {
                        ID = (int)model.ID,
                        Name = model.Name,
                        CategoryGroupID=model.CategoryGroupID ,
                        GroupName = model.GroupName,                       
                    };
                    categoryBL.EditCategory(categoryBO);
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    var res = new List<object>();
                                        return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }                                    
        }

        public JsonResult GetSalesCategory(int id)
        {
            List<CategoryBO> Categories = categoryBL.GetSalesCategory(id).ToList();
            return Json(new { Status = "success", data = Categories }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemCategoryForSales(int id)
        {
            List<CategoryBO> Categories = categoryBL.GetItemCategoryForSales().ToList();
            return Json(new { Status = "succes", data = Categories }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemsWithPackSize(int ProductGroupID)
        {
            List<CategoryBO> Categories = categoryBL.GetItemsWithPackSize(ProductGroupID).ToList();
            return Json(new { Status = "succes", data = Categories }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategoryID(string CategoryName)
        {
            var categoryid = categoryBL.GetCategoryID(CategoryName);
            return Json(new { Status = "succes", data = categoryid }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStockValuationItemCategory(string CategoryName)
        {
            List<CategoryBO> Categories = categoryBL.GetStockValuationItemCategory(CategoryName).ToList();
            return Json(new { Status = "success", data = Categories }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemCategories(string Type)
        {
            List<CategoryBO> Categories = categoryBL.GetItemCategories(Type).ToList();
            return Json(new { Status = "success", data = Categories }, JsonRequestBehavior.AllowGet);
        }
    }
}