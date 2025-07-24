using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Manufacturing.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Controllers
{
    public class ProductionScheduleController : Controller
    {
        private IDropdownContract _dropdown;
        private IBatchContract batchBL;
        private IWareHouseContract wareHouseBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private IProductionSchedule _productionSchedule;
        private IProductionIssue _productionIssueService;
        private IDropdownContract _dropdownService;
        private IProcessContract processBL;


        public ProductionScheduleController(IDropdownContract _dropdown, IProductionSchedule iproductionSchedule, IProductionIssue productionIssueService, IDropdownContract dropDownService)
        {
            this._dropdown = _dropdown;
            batchBL = new BatchBL();
            wareHouseBL = new WarehouseBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            processBL = new ProcessBL();
            _productionSchedule = iproductionSchedule;

            this._productionIssueService = productionIssueService;
            this._dropdownService = dropDownService;
        }
        // GET: Manufacturing/ProductionSchedule/Index
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled", "Suspended" };
            return View();
        }
        // GET: Manufacturing/ProductionSchedule/Create
        [HttpGet]
        public ActionResult Create()
        {
            ProductionScheduleViewModel productionScheduleViewModel = new ProductionScheduleViewModel();
            productionScheduleViewModel.ProductionLocationID = GeneralBO.LocationID;
            productionScheduleViewModel.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultProductionStore"));
            productionScheduleViewModel.ProductionLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            productionScheduleViewModel.StoreList = new SelectList(wareHouseBL.GetWareHouses(), "ID", "Name");
            productionScheduleViewModel.UnitList = new SelectList(
                                           new List<SelectListItem>
                                           {
                                                new SelectListItem { Text = "", Value = "0"}

                                           }, "Value", "Text");
            productionScheduleViewModel.TransDate = System.DateTime.Now;
            productionScheduleViewModel.ProductionStartDateStr = General.FormatDate(System.DateTime.Now);
            productionScheduleViewModel.TransNo = generalBL.GetSerialNo("ProductionSchedule", "Code");
            productionScheduleViewModel.BatchNo = generalBL.GetSerialNo("BatchMaster", "Production");
            productionScheduleViewModel.Items = new List<ProductionScheduleItemModel>();
            productionScheduleViewModel.ProcessList = new SelectList(processBL.GetProcessList(), "ID", "Process");

            return View(productionScheduleViewModel);
        }

        [HttpPost]
        public ActionResult Save(ProductionScheduleViewModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    ProductionScheduleBO Temp = _productionSchedule.GetProductionSchedule(model.ID);
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (model.ID <= 0)
                    model.TransDate = System.DateTime.Now;

                model.ProductionStatus = "New";
                var bo = model.MapToBO();

                var productionScheduleID = _productionSchedule.Save(bo);
                return Json(new { Result = productionScheduleID, Message = productionScheduleID > 0 ? "Production schedule saved successfully" : "Please try again" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Manufacturing", "ProductionSchedule", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

            
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            if (id != 0)
            {
                var productionScheduleViewModel = _productionSchedule.GetProductionSchedule(id).MapToModel();
                if (!productionScheduleViewModel.IsDraft || productionScheduleViewModel.IsCancelled)
                {
                    return RedirectToAction("Index");
                }
                productionScheduleViewModel.ProductionLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
                productionScheduleViewModel.StoreList = new SelectList(wareHouseBL.GetWareHouses(), "ID", "Name");
                productionScheduleViewModel.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}

                                    }, "Value", "Text");
                productionScheduleViewModel.ProcessList = new SelectList(processBL.GetProcessList(), "ID", "Process");
                return View(productionScheduleViewModel);
            }

            return RedirectToAction("Index");
        }

        public JsonResult GetProductionGroups(string ItemHind)
        {
            var productionGroups = _productionSchedule.GetProductionGroups(ItemHind);
            return Json(productionGroups, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Production Issue Item View
        /// </summary>
        /// <param name="productionGroupID"></param>
        /// <returns></returns>
        public PartialViewResult GetProductionIssueItemView(int productionGroupID)
        {
            var prodcutionGroupViewModelList = _productionSchedule.GetProductionIssueItemsByProductionGroup(productionGroupID).MapToModelList();

            return PartialView("_items", prodcutionGroupViewModelList);
        }

        public JsonResult GetAdditionalIssueAutoCompleteItemList(string itemHint)
        {
            int itemCategoryID = 0, purchaseCategoryID = 0;   //Static data as per the document


            return Json(_dropdown.GetItemList(itemHint, itemCategoryID, purchaseCategoryID), JsonRequestBehavior.AllowGet);
        }


        // GET: Manufacturing/ProductionSchedule/Details
        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            var productionScheduleViewModel = _productionSchedule.GetProductionSchedule(id).MapToModel();

            return View(productionScheduleViewModel);
        }
        public ActionResult Cancel(int ID, string Table)
        {
            _productionSchedule.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionScheduleList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string ProductionGroupHint = Datatable.Columns[3].Search.Value;
                string StartDateHint = Datatable.Columns[4].Search.Value;
                string BatchsizeHint = Datatable.Columns[5].Search.Value;
                string BatchHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = _productionSchedule.GetProductionScheduleList(Type, TransNoHint, TransDateHint, ProductionGroupHint, StartDateHint, BatchsizeHint, BatchHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductionBatchNo(bool IsKalkan)
        {
            var BatchNo = "";
            if (IsKalkan)
            {
                BatchNo = generalBL.GetSerialNo("BatchMaster", "Kalkan");
            }
            else
            {
                BatchNo = generalBL.GetSerialNo("BatchMaster", "Production");
            }
            return Json(new { Status = "success", data = BatchNo }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAsDraft(ProductionScheduleViewModel model)
        {
            return Save(model);
        }

        public JsonResult ProductionSchedulePrintPdf(int Id)
        {
            return null;
        }
    }
}