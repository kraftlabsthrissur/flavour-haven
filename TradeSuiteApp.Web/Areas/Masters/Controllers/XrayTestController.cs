using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.AHCMS.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class XrayTestController : Controller
    {
        private IGeneralContract generalBL;
        private ItemContract itemBL;
        private IXrayTestContract xrayTestBL;
        public XrayTestController()
        {
            generalBL = new GeneralBL();
            itemBL = new ItemBL();
            xrayTestBL = new XrayTestBL();
        }
        // GET: Masters/Xray
        public ActionResult Index()
        {
            List<XrayTestModel> XrayList = new List<XrayTestModel>();
            XrayList = xrayTestBL.GetXrayTestList().Select(a => new XrayTestModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return View(XrayList);
        }
        public ActionResult Create()
        {
            XrayTestModel model = new XrayTestModel();
            model.Code = generalBL.GetSerialNo("Xray", "Code");
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
            var item = xrayTestBL.GetXrayCategory();
            model.CategoryID = item.CategoryID;
            model.ItemUnitID = item.ItemUnitID;
            return View(model);
        }
        public ActionResult Save(XrayTestModel model)
        {
            try
            {
                XrayTestBO Xray = new XrayTestBO()
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
                if (Xray.ID == 0)
                {
                    xrayTestBL.Save(Xray);
                }
                else
                {
                    xrayTestBL.Update(Xray);
                }
                return Json(new { Status = "Success", Message = "Xray Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Xray Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save Xray failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int Id)
        {
            XrayTestModel xray = xrayTestBL.GetXrayDetailsByID(Id).Select(m => new XrayTestModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                AddedDate = General.FormatDate(m.AddedDate),
            }).First();
            return View(xray);
        }
        public ActionResult Edit(int Id)
        {
            XrayTestModel xray = xrayTestBL.GetXrayDetailsByID(Id).Select(m => new XrayTestModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                AddedDate = General.FormatDate(m.AddedDate),
            }).First();
            return View(xray);
        }
    }
}