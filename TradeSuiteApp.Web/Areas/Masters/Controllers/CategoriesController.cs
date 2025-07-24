using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessObject;
using BusinessLayer;
using PresentationContractLayer;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class CategoriesController : Controller
    {
        private ICategoryContract categoryBL;
        public CategoriesController()
        {
            categoryBL = new CategoryBL();
        }
        // GET: Masters/Categories
        public ActionResult Index()
        {
            List<CategoryModel> categoryList = new List<CategoryModel>();
            categoryList = categoryBL.GetCategoriesList().Select(m => new CategoryModel
            {
                ID = m.ID,
                CategoryName = m.Name,
                CategoryType = m.CategoryType

            }).ToList();
            return View(categoryList);
        }

        // GET: Masters/Categories/Details/5
        public ActionResult Details(int id ,string CategoryType)
        {
            CategoryModel categories = categoryBL.GetCategoriesDetails(id, CategoryType).Select(m => new CategoryModel()
            {
                ID = m.ID,
                CategoryName = m.CategoryName,
            }).First();
            return View(categories);
        }

        // GET: Masters/Categories/Create
        public ActionResult Create()
        {
            CategoryModel category = new CategoryModel();
            category.CategoryTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Customer Accounts Category", Value = "Customer Accounts Category"},
                                                 new SelectListItem { Text = "Supplier Accounts Category", Value = "Supplier Accounts Category"},
                                                 new SelectListItem { Text = "Customer Category", Value = "Customer Category"},
                                                 new SelectListItem { Text = "Customer Tax Category", Value = "Customer Tax Category"},
                                                 new SelectListItem { Text = "Employee Category", Value = "Employee Category"},
                                                 new SelectListItem { Text = "Supplier Tax Category", Value = "Supplier Tax Category"},
                                                 new SelectListItem { Text = "Payroll Category", Value = "Payroll Category"},
                                                 new SelectListItem { Text = "Supplier Category", Value = "Supplier Category"},
                                                 new SelectListItem { Text = "Supplier Tax SubCategory", Value = "Supplier Tax SubCategory"}                                               
                                                 }, "Value", "Text");
            return View(category);
        }

        // POST: Masters/Categories/Create
        [HttpPost]
        public ActionResult Create(CategoryModel modal)
        {
            try
            {
                CategoryBO category = new CategoryBO()
                {
                    ID = modal.ID,
                    CategoryType = modal.CategoryType,
                    CategoryName = modal.CategoryName,
                };
                if(modal.ID == 0)
                {
                  categoryBL.CreateCategories(category);
                }
                else
                {
                    categoryBL.UpdateCategories(category);
                }
                return Json(new { Status = "Success", Message = "Category Created Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Masters/Categories/Edit/5
        public ActionResult Edit(int id ,string CategoryType)
        {
            CategoryModel category = categoryBL.GetCategoriesDetails(id, CategoryType).Select(m => new CategoryModel()
            {
                ID = m.ID,
                CategoryName = m.CategoryName,
                CategoryTypeID = m.CategoryTypeID
            }).First();
            category.CategoryTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Customer Accounts Category", Value = "1"},
                                                 new SelectListItem { Text = "Supplier Accounts Category", Value = "2"},
                                                 new SelectListItem { Text = "Customer Category", Value = "3"},
                                                 new SelectListItem { Text = "Customer Tax Category", Value = "4"},
                                                 new SelectListItem { Text = "Employee Category", Value = "5"},
                                                 new SelectListItem { Text = "Supplier Tax Category", Value = "6"},
                                                 new SelectListItem { Text = "Payroll Category", Value = "7"},
                                                 new SelectListItem { Text = "Supplier Category", Value = "8"},
                                                 new SelectListItem { Text = "Supplier Tax SubCategory", Value = "9"},                                              
                                                 }, "Value", "Text");
            return View(category);
        }

        // POST: Masters/Categories/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Masters/Categories/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Masters/Categories/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Save()
        {
            return null;
        }
    }
}
