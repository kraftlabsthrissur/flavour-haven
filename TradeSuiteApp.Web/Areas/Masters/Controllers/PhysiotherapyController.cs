using BusinessLayer;
using BusinessObject;
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
    public class PhysiotherapyController : Controller
    {
        private ItemContract itemBL;
        private IGeneralContract generalBL;
        private IPhysiotherapyContract physiotherapyBL;
        public PhysiotherapyController()
        {
            itemBL = new ItemBL();
            generalBL = new GeneralBL();
            physiotherapyBL = new PhysiotherapyBL();
        }
        // GET: Masters/Physiotherapy
        public ActionResult Index()
        {
            List<PhysiotherapyTestModel> physiotherapyList = new List<PhysiotherapyTestModel>();
            physiotherapyList = physiotherapyBL.GetPhysiotherapyList().Select(a => new PhysiotherapyTestModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return View(physiotherapyList);
        }
        public ActionResult Create()
        {
            PhysiotherapyTestModel model = new PhysiotherapyTestModel();
            model.Code = generalBL.GetSerialNo("Physiotherapy", "Code");
            var obj = itemBL.GetAllCategoryID();
            model.PurchaseCategoryID = obj.PurchaseCategoryID;
            model.QCCategoryID = obj.QCCategoryID;
            model.GSTSubCategoryID = obj.GSTSubCategoryID;
            model.SalesCategoryID = obj.SalesCategoryID;
            model.SalesIncentiveCategoryID = obj.SalesIncentiveCategoryID;
            model.StorageCategoryID = obj.StorageCategoryID;
            model.ItemTypeID = obj.ItemTypeID;
            model.AccountsCategoryID = obj.AccountsCategoryID;
            model.BusinessCategoryID = obj.BusinessCategoryID;
            model.AddedDate = General.FormatDate(DateTime.Now);
            var item = physiotherapyBL.GetPhysiotherapyCategory();
            model.CategoryID = item.CategoryID;
            model.ItemUnitID = item.ItemUnitID;
            return View(model);
        }

        public ActionResult Save(PhysiotherapyTestModel model)
        {
            try
            {
                PhysiotherapyTestBO Physiotherapy = new PhysiotherapyTestBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    AddedDate = General.ToDateTime(model.AddedDate),
                    Description = model.Description,
                    PurchaseCategoryID = model.PurchaseCategoryID,
                    QCCategoryID = model.QCCategoryID,
                    GSTSubCategoryID = model.GSTSubCategoryID,
                    SalesCategoryID = model.SalesCategoryID,
                    SalesIncentiveCategoryID = model.SalesIncentiveCategoryID,
                    StorageCategoryID = model.StorageCategoryID,
                    ItemTypeID = model.ItemTypeID,
                    AccountsCategoryID = model.AccountsCategoryID,
                    BusinessCategoryID = model.BusinessCategoryID,
                    ItemUnitID = model.ItemUnitID,
                    CategoryID = model.CategoryID
                };
                if (Physiotherapy.ID == 0)
                {
                    physiotherapyBL.Save(Physiotherapy);
                }
                else
                {
                    physiotherapyBL.Update(Physiotherapy);
                }
                return Json(new { Status = "Success", Message = "Physiotherapy Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Physiotherapy Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Physiotherapy Xray failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int Id)
        {
            PhysiotherapyTestModel physiotherapy = physiotherapyBL.GetPhysiotherapyDetailsByID(Id).Select(m => new PhysiotherapyTestModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                AddedDate = General.FormatDate(m.AddedDate),
            }).First();
            return View(physiotherapy);
        }
        public ActionResult Edit(int Id)
        {
            PhysiotherapyTestModel physiotherapy = physiotherapyBL.GetPhysiotherapyDetailsByID(Id).Select(m => new PhysiotherapyTestModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                AddedDate = General.FormatDate(m.AddedDate),
            }).First();
            return View(physiotherapy);
        }
    }
}