using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
namespace TradeSuiteApp.Web.Areas.Masters
{
    public class AgeingBucketController : Controller
    {
        private IAgeingBucketContract ageingBucketBL;
        private IGeneralContract generalBL;

        public AgeingBucketController()
        {
            ageingBucketBL = new AgeingBucketBL();
            generalBL = new GeneralBL();
        }

    // GET: Masters/AgeingBucket
    public ActionResult Index()
        {
            try
            {
                List<AgeingBucketModel> ageingBucket = ageingBucketBL.GetAgeingBucketList().Select(a => new AgeingBucketModel()
                {
                    ID = a.ID,
                    Name = a.Name,
                    Code = a.Code

                }).ToList();
                return View(ageingBucket);
            }

            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Masters/AgeingBucket/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            try
            {
                AgeingBucketModel ageingBucket = ageingBucketBL.GetAgeingBucketDetail(id).Select(a => new AgeingBucketModel()
                {
                    ID = a.ID,
                    Name = a.Name
                }).First();
                ageingBucket.Trans = ageingBucketBL.GetAgeingBucketTrans(id).Select(a => new AgeingBucketTransModel()
                {
                    ID = a.ID,
                    Name = a.Name,
                    Start = a.Start,
                    End = a.End

                }).ToList();
                return View(ageingBucket);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Masters/AgeingBucket/Create
        public ActionResult Create()
        {
            AgeingBucketModel Model = new AgeingBucketModel();
            Model.Code = generalBL.GetSerialNo("AgeingBucket", "Code");
            Model.Trans = new List<AgeingBucketTransModel>();
            return View(Model);
        }

        // POST: Masters/AgeingBucket/Create
        [HttpPost]
        public ActionResult Create(AgeingBucketModel model)
        {
            try
            {
                AgeingBucketBO ageingBucket = new AgeingBucketBO()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Code = model.Code
                };
                var ItemList = new List<AgeingBucketTransBO>();
                AgeingBucketTransBO AgeingBucketTransBO;
                foreach (var itm in model.Trans)
                {
                    AgeingBucketTransBO = new AgeingBucketTransBO();
                    AgeingBucketTransBO.ID = itm.ID;
                    AgeingBucketTransBO.Start = itm.Start;
                    AgeingBucketTransBO.End = itm.End;
                    AgeingBucketTransBO.Name = itm.Name;
                    ItemList.Add(AgeingBucketTransBO);
                }
                bool outId = ageingBucketBL.Save(ageingBucket, ItemList);

                return Json(new { Status = "Success", Message = "Ageing bucket Created Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Status = "failure", Message = "Failed to create ageing bucket " }, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: Masters/AgeingBucket/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            try
            {
                AgeingBucketModel ageingBucket = ageingBucketBL.GetAgeingBucketDetail(id).Select(a => new AgeingBucketModel()
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name
                }).First();
                ageingBucket.Trans = ageingBucketBL.GetAgeingBucketTrans(id).Select(a => new AgeingBucketTransModel()
                {
                    ID = a.ID,
                    Name = a.Name,
                    Start = a.Start,
                    End = a.End,


                }).ToList();
                return View(ageingBucket);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // POST: Masters/AgeingBucket/Edit/5
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

    }
}
