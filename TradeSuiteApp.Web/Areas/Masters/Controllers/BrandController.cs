using BusinessLayer;
using BusinessObject;
using Microsoft.AspNet.SignalR.Hubs;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class BrandController : Controller
    {
        // GET: Masters/Currency
        private IBrandContract iBrandContract;
        public BrandController()
        {
            iBrandContract = new BrandBL();
        }
        public ActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public ActionResult Save(BrandModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BrandBO brandBO = new BrandBO()
                    {
                        Id=model.Id,
                        BrandId=model.BrandId,
                        Code = model.Code,
                        BrandName = model.BrandName,
                        Path=model.Path,
                       image = model.image
                       

                    };
                    iBrandContract.CreateBrand(brandBO);
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
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
            {
                return Json(new { success = false, message = "No file selected or file is empty." });
            }

            try
            {
                string uploadsDir = Server.MapPath("~/Uploads/Brandimage");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // Generate unique file name
                string image = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsDir, image);

                // Save the file to the server
                file.SaveAs(filePath);

                // Construct the absolute file URL
                string fileUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/Uploads/Brandimage/" + image);

                // Return JSON response with file details
                return Json(new { success = true, image = image, Path = fileUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "File upload failed: " + ex.Message });
            }
        }
        public ActionResult Index()
        {
            List<BrandModel> BrandList = new List<BrandModel>();
            BrandList = iBrandContract.GetBrandList().Select(a => new BrandModel()
            {
                Id = a.Id,
                BrandId=a.BrandId,
                Code = a.Code,
                BrandName = a.BrandName,
                Path = a.Path,
              
            }).ToList();
            return View(BrandList);
        }
        public ActionResult Details(int id)
        {
            //try
            //{
            var obj = iBrandContract.GetBrandDetails(id);
            BrandModel brandModel = new BrandModel();
            brandModel.Id = obj.Id;
            brandModel.BrandId = obj.BrandId;
            brandModel.Code = obj.Code;
            brandModel.BrandName = obj.BrandName;
            brandModel.Path = obj.Path;
            brandModel.image = obj.image;
            return View(brandModel);
            //}
            //catch (Exception e)
            //{
            //    return RedirectToAction("Index");
            //}
        }
        public ActionResult Edit(int id)
        {
            try
            {
                var obj = iBrandContract.GetBrandDetails(id);
                BrandModel BrandModel = new BrandModel();
                BrandModel.Id = obj.Id;
                BrandModel.Code = obj.Code;
                BrandModel.BrandId = obj.BrandId;
                BrandModel.BrandName = obj.BrandName;
                BrandModel.Path = obj.Path;
                BrandModel.image = obj.image;
                    
                
                return View(BrandModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Masters/Country/Edit/5
        [HttpPost]
        public ActionResult Edit(BrandModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BrandBO BrandBO = new BrandBO()
                    {
                        Id = (int)model.Id,
                        Code = model.Code,
                        BrandId=model.BrandId,
                        BrandName=model.BrandName,
                        Path=model.Path,
                        image=model.image
                      
                    };
                    iBrandContract.EditBrand(BrandBO);
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


    }
}