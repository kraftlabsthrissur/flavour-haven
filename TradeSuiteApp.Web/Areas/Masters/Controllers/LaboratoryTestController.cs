using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class LaboratoryTestController : Controller
    {
        private ItemContract itemBL;
        private ILaboratoryTestContract laboratoryTestBL;
        private IGeneralContract generalBL;
        private IGSTCategoryContract GstBL;
        private ISpecimenContract specimenBL;       
        public LaboratoryTestController()
        {
            itemBL = new ItemBL();
            laboratoryTestBL = new LaboratoryTestBL();
            generalBL = new GeneralBL();
            GstBL = new GSTCategoryBL();
            specimenBL = new SpecimenBL();
        }
        // GET: Masters/LabTest
        public ActionResult Index()
        {
            List<LaboratoryTestModel> LaboratoryTestList = new List<LaboratoryTestModel>();
            LaboratoryTestList = laboratoryTestBL.GetLaboratoryTestList().Select(a => new LaboratoryTestModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return View(LaboratoryTestList);
        }
        public ActionResult Create()
        {
            LaboratoryTestModel model = new LaboratoryTestModel();
            model.Code = generalBL.GetSerialNo("Laboratory Test", "Code");
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
            var item = laboratoryTestBL.GetLaboratoryTestDetails();
            model.CategoryID = item.CategoryID;
            model.ItemUnitID = item.ItemUnitID;
            model.GSTCategoryList = new SelectList(GstBL.GetGSTList(), "ID", "IGSTPercent");
            model.SpecimenList = new SelectList(specimenBL.GetSpecimenList(), "ID", "Name");
            return View(model);
        }
        public ActionResult Save(LaboratoryTestModel model)
        {
            try
            {
                LaboratoryTestBO Lab = new LaboratoryTestBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    BiologicalReference = model.BiologicalReference,
                    Unit = model.Unit,
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
                    CategoryID = model.CategoryID,
                    Method = model.Method,
                    SpecimenID = model.SpecimenID,
                    Rate = model.Rate,
                    GSTCategoryID = model.GSTCategoryID,
                    IsAlsoGroup = model.IsAlsoGroup

                };
                List<LabItemBO> Items = new List<LabItemBO>();
                LabItemBO LabTest;
                if (model.Items != null)
                {
                    foreach (var item in model.Items)
                    {
                        LabTest = new LabItemBO()
                        {
                            LabTest = item.LabTest,
                            LabTestID = item.LabTestID
                        };
                        Items.Add(LabTest);
                    }

                }
                laboratoryTestBL.Save(Lab, Items);
                return Json(new { Status = "Success", Message = "Laboratory Test Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Laboratory Test Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save Laboratory Test failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int Id)
        {
            LaboratoryTestModel lab = laboratoryTestBL.GetLaboratoryTestDetailsByID(Id).Select(m => new LaboratoryTestModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                BiologicalReference = m.BiologicalReference,
                Unit = m.Unit,
                AddedDate = General.FormatDate(m.AddedDate),
                Method = m.Method,
                SpecimenID = m.SpecimenID,
                Rate = m.Rate,
                GSTCategoryID = m.GSTCategoryID,
                GSTCategory = m.GSTCategory,
                Specimen = m.Specimen,
                IsAlsoGroup=m.IsAlsoGroup
            }).First();
            List<LabItemBO> LabItemBO;          
            LabItemModel LabItemModel;
            LabItemBO = laboratoryTestBL.GetLaboratoryTestItemDetailsByID(Id);
            lab.Items= new List<LabItemModel>();
            foreach (var m in LabItemBO)
            {
                LabItemModel = new LabItemModel()
                {
                    LabTestID=m.LabTestID,
                    LabTest=m.LabTest
                };
                lab.Items.Add(LabItemModel);

            }
                return View(lab);
        }
        public ActionResult Edit(int Id)
        {

            LaboratoryTestModel lab = laboratoryTestBL.GetLaboratoryTestDetailsByID(Id).Select(m => new LaboratoryTestModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                BiologicalReference = m.BiologicalReference,
                Unit = m.Unit,
                AddedDate = General.FormatDate(m.AddedDate),
                Method = m.Method,
                SpecimenID = m.SpecimenID,
                Rate = m.Rate,
                GSTCategoryID = m.GSTCategoryID,
                GSTCategory = m.GSTCategory,
                Specimen = m.Specimen,
                IsAlsoGroup=m.IsAlsoGroup
            }).First();
            List<LabItemBO> LabItemBO;
            LabItemModel LabItemModel;
            LabItemBO = laboratoryTestBL.GetLaboratoryTestItemDetailsByID(Id);
            lab.Items = new List<LabItemModel>();
            foreach (var m in LabItemBO)
            {
                LabItemModel = new LabItemModel()
                {
                    LabTestID = m.LabTestID,
                    LabTest = m.LabTest
                };
                lab.Items.Add(LabItemModel);

            }
            lab.GSTCategoryList = new SelectList(GstBL.GetGSTList(), "ID", "IGSTPercent");
            lab.SpecimenList = new SelectList(specimenBL.GetSpecimenList(), "ID", "Name");
            return View(lab);
        }

        public JsonResult GetLabTestList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string TypeHint = Datatable.Columns[3].Search.Value;
                string ServiceHint = Datatable.Columns[4].Search.Value;
                string NameHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = laboratoryTestBL.GetLabTestList(CodeHint, TypeHint, ServiceHint, NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "LaboratoryTestController", "GetLabTestList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLabTestAutoComplete(string Hint, string CodeHint = "",string ServiceHint = "",string TypeHint="")
        {
            DatatableResultBO resultBO = laboratoryTestBL.GetLabTestList(CodeHint, TypeHint, ServiceHint, Hint,"Name", "ASC", 0, 20);

            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
    }
}