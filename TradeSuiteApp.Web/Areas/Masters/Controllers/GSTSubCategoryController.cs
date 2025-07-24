using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObject;
using BusinessLayer;
using TradeSuiteApp.Web.Areas.Masters.Models;
using PresentationContractLayer;


namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class GSTSubCategoryController : Controller
    {

        IGSTSubCategoryContract GSTSubCategoryBL;
        public GSTSubCategoryController()
        { 
            GSTSubCategoryBL = new GSTSubCategoryBL();
        }

        // GET: Masters/GSTSubCategory
        public ActionResult Index()
        {
            try
            {
                List<GSTSubCategoryModel> subCategoryList = new List<GSTSubCategoryModel>();
                subCategoryList = GSTSubCategoryBL.GetGSTSubCategoryList().Select(m => new GSTSubCategoryModel
                {
                    ID = m.ID,
                    Name =m.Name,
                    Description = m.Description,
                    Percentage = m.Percentage
                }).ToList();
                return View(subCategoryList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Masters/GSTSubCategory/Details/5
        public ActionResult Details(int id)
        {
           try
            {
                GSTSubCategoryModel subCategory = GSTSubCategoryBL.GetGSTSubCategoryDetails(id).Select(m => new GSTSubCategoryModel
                {
                    ID = m.ID,
                    Name =m.Name,
                    Description = m.Description,
                    Percentage = m.Percentage

                }).First();
                return View(subCategory);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Masters/GSTSubCategory/Create
        public ActionResult Create()
        {
            GSTSubCategoryModel subCategory = new GSTSubCategoryModel();
            return View(subCategory);
        }

        // POST: Masters/GSTSubCategory/Save
        [HttpPost]
        public ActionResult Save(GSTSubCategoryModel model)
        {
            try
            {
                GSTSubCategoryBO subCategory = new GSTSubCategoryBO()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Description = model.Description,
                    Percentage = model.Percentage

                };
                if(model.ID == 0)
                {
                    GSTSubCategoryBL.CreateGSTSubCategory(subCategory);
                }
                else
                {
                    GSTSubCategoryBL.UpdateGSTSubCategory(subCategory);
                }
                return Json(new { Status = "Success", Message = "Serial Number Created Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        // GET: Masters/GSTSubCategory/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                GSTSubCategoryModel subCategory = GSTSubCategoryBL.GetGSTSubCategoryDetails(id).Select(m => new GSTSubCategoryModel
                {
                    ID = m.ID,
                    Name = m.Name,
                    Description = m.Description,
                    Percentage = m.Percentage

                }).First();
                return View(subCategory);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: Masters/GSTSubCategory/Edit/5
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

        // GET: Masters/GSTSubCategory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Masters/GSTSubCategory/Delete/5
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
    }
}
