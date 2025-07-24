using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class MaterialPurificationController : Controller
    {
        private IMaterialPurificationContract materialPurificationBL;
        private ICategoryContract categoryBL;

        public MaterialPurificationController()
        {
            materialPurificationBL = new MaterialPurificationBL();
            categoryBL = new CategoryBL();
        }
        // GET: Masters/MaterialPurification
        public ActionResult Index()
        {
            List<MaterialPurificationModel> materialPurificationList = new List<MaterialPurificationModel>();

            materialPurificationList = materialPurificationBL.GetMaterialPurificationList().Select(a => new MaterialPurificationModel()
            {
                ID = a.ID,
                ItemCategoryID = a.ItemCategoryID,
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitID = a.UnitID,
                Unit = a.Unit,
                ProcessID = a.ProcessID,
                ProcessName = a.ProcessName,
                PurificationItemID = a.PurificationItemID,
                PurificationItemName = a.PurificationItemName,
                PurificationUnitID = a.PurificationUnitID,
                PurificationUnit = a.PurificationUnit,
            }).ToList();
            return View(materialPurificationList);
        }

        public ActionResult Create()
        {
            MaterialPurificationModel materialPurification = new MaterialPurificationModel();
            materialPurification.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            materialPurification.MaterialPurificationProcessList = new SelectList(materialPurificationBL.GetMaterialPurificationProcessList(), "ProcessID", "ProcessName");
            return View(materialPurification);

        }

        [HttpPost]
        public ActionResult Save(MaterialPurificationModel materialPurificationModel)
        {
            try
            {
                MaterialPurificationBO materialPurificationBO = new MaterialPurificationBO()
                {
                    ID = materialPurificationModel.ID,
                    ItemID = materialPurificationModel.ItemID,
                    ItemName = materialPurificationModel.ItemName,
                    UnitID = materialPurificationModel.UnitID,
                    ProcessID = materialPurificationModel.ProcessID,
                    ProcessName = materialPurificationModel.ProcessName,
                    PurificationItemID = materialPurificationModel.PurificationItemID,
                    PurificationItemName = materialPurificationModel.PurificationItemName,
                    PurificationUnitID = materialPurificationModel.PurificationUnitID,
                };
                materialPurificationBL.Save(materialPurificationBO);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int id)
        {
            MaterialPurificationModel materialPurification = materialPurificationBL.GetMaterialPurificationDetail(id).Select(a => new MaterialPurificationModel()
            {
                ID = a.ID,
                ItemCategoryID = a.ItemCategoryID,
                CategoryName = a.CategoryName,
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitID = a.UnitID,
                Unit = a.Unit,
                ProcessID = a.ProcessID,
                ProcessName = a.ProcessName,
                PurificationItemID = a.PurificationItemID,
                PurificationItemName = a.PurificationItemName,
                PurificationUnitID = a.PurificationUnitID,
                PurificationUnit = a.PurificationUnit,
            }).First();
            return View(materialPurification);
        }

        public ActionResult Edit(int id)
        {
            MaterialPurificationModel materialPurification = materialPurificationBL.GetMaterialPurificationDetail(id).Select(a => new MaterialPurificationModel()
            {
                ID = a.ID,
                ItemCategoryID = a.ItemCategoryID,
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                UnitID = a.UnitID,
                Unit = a.Unit,
                ProcessID = a.ProcessID,
                ProcessName = a.ProcessName,
                PurificationItemID = a.PurificationItemID,
                PurificationItemName = a.PurificationItemName,
                PurificationUnitID = a.PurificationUnitID,
                PurificationUnit = a.PurificationUnit,
            }).First();
            materialPurification.CategoryList = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            materialPurification.MaterialPurificationProcessList = new SelectList(materialPurificationBL.GetMaterialPurificationProcessList(), "ProcessID", "ProcessName");
            return View(materialPurification);
        }

    }
}