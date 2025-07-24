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
    public class ProductionIssueController : Controller
    {

        private IDropdownContract _dropdownService;
        private IProductionIssue _productionIssueService;
        private IProductionSchedule _productionSchedule;
        private IWareHouseContract warehouseBL;
        private IGeneralContract generalBL;
        private ILocationContract locationBL;
        private IBatchContract batchBL;
        private IBatchTypeContract batchTypeBL;
        public ProductionIssueController(IDropdownContract dropDownService, IProductionIssue productionIssueService, IProductionSchedule productionSchedule)
        {
            warehouseBL = new WarehouseBL();
            _dropdownService = dropDownService;
            _productionIssueService = productionIssueService;
            _productionSchedule = productionSchedule;
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            batchBL = new BatchBL();
            batchTypeBL = new BatchTypeBL();

        }

        // GET: Manufacturing/ProductionIssue
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Manufacturing/ProductMix/Details/5
        public ActionResult Details(int id)

        {
            ViewBag.IsViewOnly = true;
            ProductionIssueViewModel productionIssue = new ProductionIssueViewModel();
            productionIssue = _productionIssueService.GetProductionIssue(id).MapToModel();

            productionIssue.SequenceItemList = new SelectList(new List<SelectListItem>(), "", "");
            productionIssue.SequenceItemID = 1;

            return View(productionIssue);
        }

        // GET: Manufacturing/ProductMix/Create
        #region Create
        /// <summary>
        /// Create Production Issue
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            ProductionIssueViewModel productionIssue = new ProductionIssueViewModel();
            productionIssue.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultProductionStore"));

            productionIssue.StartDate = DateTime.Now;
      
            ViewBag.WareHouses = new SelectList(GetWareHouseSelectListItem(), "Value", "Text");
            ViewBag.DefaultWareHouseID = generalBL.GetConfig("DefaultProductionStore");
            ViewBag.ProductionLocations = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            productionIssue.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");

            productionIssue.TransNo = generalBL.GetSerialNo("ProductionIssue", "Code");
            productionIssue.ProductionLocationID = GeneralBO.LocationID;
            productionIssue.BatchNo = generalBL.GetSerialNo("BatchMaster", "Production");

            productionIssue.Materials = new List<ProductionIssueMaterialItemModel>();
            productionIssue.Processes = new List<ProductionIssueProcessItemModel>();
            productionIssue.Output = new List<OutputModel>();
            productionIssue.BatchList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            productionIssue.SequenceItemList = new SelectList(new List<SelectListItem>(), "", "");


            return View(productionIssue);
        }

        // GET: Manufacturing/ProductMix/Edit/5
        public ActionResult Edit(int id)
        {

            if (!_productionIssueService.IsProductionIssueEditable(id))
            {
                return RedirectToAction("Index");
            }

            var productionIssue = _productionIssueService.GetProductionIssue(id).MapToModel();
            productionIssue.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultProductionStore"));

            ViewBag.WareHouses = new SelectList(GetWareHouseSelectListItem(), "Value", "Text");
            ViewBag.ProductionLocations = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            productionIssue.BatchList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            productionIssue.SequenceItemList = new SelectList(new List<SelectListItem>(), "", "");
            productionIssue.BatchTypeList = new SelectList(batchTypeBL.GetBatchTypeList(), "ID", "Name");

            productionIssue.SequenceItemID = 1;
            ViewBag.DefaultWareHouseID = generalBL.GetConfig("DefaultProductionStore");
            return View(productionIssue);
        }

        /// <summary>
        /// Create or EditSave ProductionIssue
        /// </summary>
        /// <param name="productionViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(ProductionIssueViewModel productionViewModel)
        {
            try
            {
                productionViewModel.TransDate = DateTime.Now;
                var productionBO = productionViewModel.MapToBO();

                var productionIssueID = _productionIssueService.Save(productionBO);
                return Json(new { Status = "Success", Result = productionIssueID, Message = "Production issue saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (NotEditableException e)
            {
                return Json(new { Status = "Failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (OutofStockException e)
            {
                return Json(new { Status = "Failure", Message = "Item out of stock" }, JsonRequestBehavior.AllowGet);
            }
            catch (InvalidReturnException e)
            {
                return Json(new { Status = "Failure", Message = "Invalid return quantity" }, JsonRequestBehavior.AllowGet);
            }

        }


        //For Role privilege
        public ActionResult Complete()
        {
            return null;
        }
        //For Role privilege
        public ActionResult Abort()
        {
            return null;
        }

        public PartialViewResult GetProductionSchedules(int ItemID)
        {
            List<ProductionScheduleViewModel> productionSchedules = _productionSchedule.GetProductionSchedulesByItem(ItemID).Select(a => new ProductionScheduleViewModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                ProductGroupName = a.ProductionGroupName,
                ActualBatchSize = a.ActualBatchSize,
                ProductionGroupName = a.ProductionGroupName,
                TransDateStr = General.FormatDate(a.TransDate),
                ProductionStartDateStr = General.FormatDate(a.ProductionStartDate),
                BatchNo = a.BatchNo,
                BatchID = a.BatchID
            }).ToList();

            return PartialView(productionSchedules);
        }
        public JsonResult GetProductionSequences(int ItemID)
        {
            List<ProductionSequenceBO> productionSequences = _productionIssueService.GetProductionSequences(ItemID);
            return Json(new { Status = "success", data = productionSequences }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Additional issue Items for AutoComplete
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMaterialReturnAutoCompleteItemList(int productionID, int productionSequence, string itemHint)
        {
            var productionIssueItemModelList = _productionIssueService.GetProductionIssueMaterials(productionID);
            return Json(productionIssueItemModelList.Where(a => a.ProductionSequence == productionSequence && a.IssueQty > 0 && a.RawMaterialName.ToLower().Contains(itemHint.ToLower())), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Get Material and Process views after selecting Item
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProductionMaterialProcessView(int productionGroupID, int productionSequence, int itemID,int ProductID=0)
        {

            var wareHouseList = GetWareHouseSelectListItem();
            var materialModelList = _productionIssueService.GetProductionIssueMaterials(productionGroupID, productionSequence, itemID, ProductID).MapToModelList();

            var defaultWareHouseID = generalBL.GetConfig("DefaultProductionReceiptStore");

            var materialProductionIssueViewStr = GetMaterialViewStr(itemID, materialModelList, wareHouseList, Convert.ToInt32(defaultWareHouseID));

            var processesModelList = _productionIssueService.GetProductionIssueProcesses(productionGroupID, productionSequence, itemID).MapToModelList(1);
            processesModelList = processesModelList != null ? processesModelList.Select(x => { x.StartTimeStr = ""; x.EndTimeStr = ""; return x; }).ToList() : null;
            var processProductionIssueViewStr = GetProcessViewStr(processesModelList, isViewOnly: false);

            //otherDeductionViewString = RenderPartialViewToString(this, "~/Areas/Purchase/Views/PurchaseInvoice/_otherDeductions.cshtml", purchaseOrderOtherDeductionViewModelList);
            return Json(new { MaterialView = materialProductionIssueViewStr, ProcessView = processProductionIssueViewStr, Stores = wareHouseList, defaultReceiptStoreID = defaultWareHouseID }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Material view string
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="productionMaterialItemModelList"></param>
        /// <param name="batchList"></param>
        /// <param name="wareHouseList"></param>
        /// <returns></returns>
        private string GetMaterialViewStr(int itemID, List<ProductionIssueMaterialItemModel> productionMaterialItemModelList, List<SelectListItem> wareHouseList, int defaultWareHouseID = 0, bool isViewOnly = false)
        {
            defaultWareHouseID = Convert.ToInt16(generalBL.GetConfig("DefaultProductionStore"));
            KeyValuePair<string, dynamic> DefaultWareHouseKVP = new KeyValuePair<string, dynamic>("DefaultWareHouseID", defaultWareHouseID);
            List<KeyValuePair<string, dynamic>> viewBagItems = new List<KeyValuePair<string, dynamic>>() { DefaultWareHouseKVP };

            return RenderPartialViewToString(this, "_materialProductionIssue", productionMaterialItemModelList, viewBagItems);
        }

        /// <summary>
        /// Get Process View string
        /// </summary>
        /// <param name="productionProcessItemModelList"></param>
        /// <returns></returns>
        private string GetProcessViewStr(List<ProductionIssueProcessItemModel> productionProcessItemModelList, bool isViewOnly)
        {


            KeyValuePair<string, dynamic> isViewOnlyViewBag = new KeyValuePair<string, dynamic>("IsViewOnly", isViewOnly);
            return RenderPartialViewToString(this, "_processProductionIssue", productionProcessItemModelList, new List<KeyValuePair<string, dynamic>>() { isViewOnlyViewBag });
        }


        /// <summary>
        /// Get warehouses
        /// </summary>
        private List<WareHouseBO> GetWarehouseBOList()
        {
            //As per discussion. RM store is default
            return warehouseBL.GetWareHouses();//.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();
        }

        private List<SelectListItem> GetWareHouseSelectListItem()
        {
            return GetWarehouseBOList().Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();

        }

        /// <summary>
        /// Get Material Qty Maintaince
        /// </summary>
        /// <param name="productionIssueMaterialsID"></param>
        /// <param name="itemName"></param>
        /// <param name="uom"></param>
        /// <param name="batchNo"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public JsonResult GetMaterialQtyMaintainanceView(int productionIssueMaterialID)
        {
            var materialTrans = _productionIssueService.GetMaterialTrans(productionIssueMaterialID).MapToModelList();
            return Json(new { Data = materialTrans, Status = "Success" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get the Output view
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemName"></param>
        /// <param name="standardOutput"></param>
        /// <returns></returns>
        public JsonResult GetOutputView(int itemID, string itemName, int standardOutput)
        {

            KeyValuePair<string, dynamic> wareHouseKVP = new KeyValuePair<string, dynamic>("WareHouses", GetWareHouseSelectListItem());
            List<KeyValuePair<string, dynamic>> viewBagItems = new List<KeyValuePair<string, dynamic>>() { wareHouseKVP };

            OutputModel outputModel = new OutputModel()
            {
                ItemID = itemID,
                ItemName = itemName,
                StandardOutput = standardOutput
            };

            var outputViewStr = RenderPartialViewToString(this, "_output", outputModel, viewBagItems);


            var materialProductionIssueViewStr = RenderPartialViewToString(this, "_materialProductionIssue", null);
            var processProductionIssueViewStr = RenderPartialViewToString(this, "_processProductionIssue", null);

            return Json(new { OutputView = outputViewStr, MaterialView = materialProductionIssueViewStr, ProcessView = processProductionIssueViewStr }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Render view to string
        [NonAction]
        private string RenderPartialViewToString(Controller controller, string viewName, object model, List<KeyValuePair<string, dynamic>> viewBagItems = null)
        {
            controller.ViewData.Model = model;
            if (viewBagItems != null && viewBagItems.Count > 0)
            {
                foreach (var item in viewBagItems)
                {
                    if (!controller.ViewData.Keys.Contains(item.Key))
                        controller.ViewData.Add(item.Key, item.Value);
                }
            }
            try
            {
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        #endregion

        public JsonResult GetProductionIssueList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string ProductionGroupNameHint = Datatable.Columns[3].Search.Value;
                string BatchNoHint = Datatable.Columns[4].Search.Value;
                string BatchsizeHint = Datatable.Columns[6].Search.Value;
                string UnitHint = Datatable.Columns[5].Search.Value;
                string ExpectedDateHint=Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = _productionIssueService.GetProductionIssueList(Type, TransNoHint, TransDateHint, ExpectedDateHint, ProductionGroupNameHint, BatchNoHint, BatchsizeHint, UnitHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetReturnItemBatch(int productionID, int itemID = 0)
        {
            List<BatchBO> Batches = batchBL.GetBatchForProductionIssueMaterialReturn(productionID, itemID).ToList();
            return Json(new { Status = "success", data = Batches }, JsonRequestBehavior.AllowGet);
        }

    }
}