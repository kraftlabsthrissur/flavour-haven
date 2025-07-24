using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.RolePrivileges;
using TradeSuiteApp.Web.Utils;
using System.Web.UI.WebControls;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{

    public class PurchaseOrderController : Controller
    {
        private IDropdownContract _dropdown;
        private IPurchaseOrder purchaseOrderBL;
        private IFileContract fileBL;
        private ISupplierContract supplierBL;
        private IAddressContract addressBL;
        private IPaymentModeContract paymentModeBL;
        private IPaymentDaysContract paymentDaysBL;
        private IGeneralContract generalBL;
        private IBatchTypeContract BatchBL;
        private ICategoryContract categoryBL;
        private IConfigurationContract configBL;
        private ILocationContract locationBL;
        private ICategoryContract categroyBL;
        private ICounterSalesContract counterSalesBL;
        private string failureMessage = App_LocalResources.Common.createfail;
        public PurchaseOrderController(IDropdownContract IDropdown)
        {
            _dropdown = IDropdown;
            fileBL = new FileBL();
            supplierBL = new SupplierBL();
            addressBL = new AddressBL();
            paymentModeBL = new PaymentModeBL();
            paymentDaysBL = new PaymentDaysBL();
            purchaseOrderBL = new PurchaseOrderBL();
            BatchBL = new BatchTypeBL();
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            configBL = new ConfigurationBL();
            locationBL = new LocationBL();
            categroyBL = new CategoryBL();
            counterSalesBL = new CounterSalesBL();
        }

        // GET: Purchase/PurchaseOrders

        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled", "Suspended" };
            return View();
        }

        public ActionResult IndexV4()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled", "Suspended" };
            return View();
        }

        // GET: Purchase/PurchaseOrders/Details/5
        public ActionResult Details(int id)
        {
            PurchaseOrderBO _po = new PurchaseOrderBO();
            _po = purchaseOrderBL.GetPurchaseOrder(Convert.ToInt32(id));
            _po.IsCancellable = purchaseOrderBL.IsPOCancellable(Convert.ToInt32(id));
            _po.SelectedQuotation = fileBL.GetAttachments(_po.SelectedQuotationID.ToString());
            _po.OtherQuotations = fileBL.GetAttachments(_po.OtherQuotationIDS);

            _po.items = purchaseOrderBL.GetPurchaseOrderItems(Convert.ToInt32(id));
            var firstItem = _po.items.FirstOrDefault();
            _po.PurchaseRequisitionIDS = String.Join(",", _po.items.Select(a => a.PurchaseRequisitionNo).Distinct());
            _po.HomeAddress = new AddressBO(); //addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            _po.HomeAddress.Name = GeneralBO.CompanyName;
            _po.SupplierAddress = addressBL.GetBillingAddress("Supplier", _po.SupplierID, "").FirstOrDefault();
            _po.SupplierAddress.Name = _po.SupplierAddress.Supplier;
            _po.DDLLocation = new SelectList(locationBL.getInterCompanyLocation(_po.InterCompanyLocationID), "ID", "Name");

            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.Contains("Approval"))
            {
                ViewBag.CloseURL = "/Approval";
            }
            if (firstItem != null)
            {
                _po.CurrencyID = firstItem.CurrencyID;
                _po.IsGST = firstItem.IsGST;
                _po.IsVat = firstItem.IsVat;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(_po.CurrencyID);
            if (classdata != null)
            {
                _po.DecimalPlaces = classdata.DecimalPlaces;
                _po.normalclass = classdata.normalclass;
            }
            return View(_po);
        }

        // GET: Purchase/PurchaseOrders/Create
        [HttpGet]
        public ActionResult Create()
        {
            PurchaseOrderBO purchaseOrder = new PurchaseOrderBO();
            purchaseOrder.DDLItemCategory = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            purchaseOrder.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            purchaseOrder.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            purchaseOrder.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name");
            purchaseOrder.BatchTypeList = new SelectList(
                                               BatchBL.GetBatchTypeList(), "ID", "Name");
            purchaseOrder.UnProcessedPrList = purchaseOrderBL.GetUnProcessedPurchaseRequisitionForPO();
            purchaseOrder.DDLLocation = new SelectList(locationBL.GetLocationListByLocationHead(), "ID", "Name");

            purchaseOrder.ShippingAddressList = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            purchaseOrder.BillingAddressList = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            var obj = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.ShippingAddressID = obj.Count > 0 ? obj[0].AddressID : 0;
            purchaseOrder.ShippingStateID = obj.Count > 0 ? obj[0].StateID : 0;
            obj = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.BillingAddressID = obj.Count > 0 ? obj[0].AddressID : 0;
            purchaseOrder.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}

                                    }, "Value", "Text");
            purchaseOrder.GSTList = new SelectList(
                                  new List<SelectListItem>
                                  {
                                                new SelectListItem { Text = "IncVAT", Value = "1"},
                                                new SelectListItem { Text = "VATExtra", Value = "2"}

                                  }, "Value", "Text");
            purchaseOrder.GSTID = Convert.ToInt16(generalBL.GetConfig("GSTValue"));

            purchaseOrder.PurchaseOrderNo = generalBL.GetSerialNo("PurchaseOrder", "Code");
            purchaseOrder.PurchaseOrderDate = DateTime.Now;
            purchaseOrder.CashPaymentLimit = configBL.GetCashPayementLimit();
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                purchaseOrder.CurrencyID = currency.CurrencyID;
                purchaseOrder.CurrencyName = currency.CurrencyName;
                purchaseOrder.CurrencyCode = currency.CurrencyCode;
                purchaseOrder.IsVat = currency.IsVat;
                purchaseOrder.IsGST = currency.IsGST;
                purchaseOrder.TaxType = currency.TaxType;
                purchaseOrder.TaxTypeID = currency.TaxTypeID;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                purchaseOrder.DecimalPlaces = classdata.DecimalPlaces;
                purchaseOrder.normalclass = classdata.normalclass;
            }
            purchaseOrder.OrderTypeList = new SelectList(purchaseOrderBL.GetOrderTypeList(), "Name", "Name");
            purchaseOrder.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            return View(purchaseOrder);
        }

        public JsonResult getProductList(string Areas, int SupplierId, string term = "", string ItemCategoryID = "", string PurchaseCategoryID = "", int BusinessCategoryID = 0)
        {
            List<PurchaseOrderItemBO> _outItems = new List<PurchaseOrderItemBO>();
            var ItemCategoryIDInt = (ItemCategoryID != "") ? Convert.ToInt32(ItemCategoryID) : 0;
            var PurchaseCategoryIDInt = (PurchaseCategoryID != "") ? Convert.ToInt32(PurchaseCategoryID) : 0;

            _outItems = _dropdown.GetPurchaseOrderItems(term, SupplierId, ItemCategoryIDInt, PurchaseCategoryIDInt, BusinessCategoryID);
            return Json(_outItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPoTransByPrId(int[] PurchaseRequisitionIDList, int SupplierID)
        {
            List<PurchaseOrderItemBO> _outItems = new List<PurchaseOrderItemBO>();

            foreach (var PurchaseRequisitionID in PurchaseRequisitionIDList)
            {
                var obj = purchaseOrderBL.GetUnProcessedPurchaseRequisitionTransForPO(Convert.ToInt32(PurchaseRequisitionID), SupplierID);
                _outItems.AddRange(obj);
            }

            return Json(_outItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsItemSuppliedBySupplier(string ItemLists, int SupplierID)
        {
            List<IsItemSuppliedBySupplier> _outItems = new List<IsItemSuppliedBySupplier>();

            _outItems = purchaseOrderBL.IsItemSuppliedBySupplier(ItemLists, SupplierID);

            return Json(_outItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(PurchaseOrderBO PO, List<PurchaseOrderTransBO> Details)
        {
            var result = new List<object>();
            try
            {
                if (PO.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PurchaseOrderBO Temp = purchaseOrderBL.GetPurchaseOrder(PO.ID);
                    if (!Temp.IsDraft || Temp.IsSuspended || Temp.Cancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var _out = purchaseOrderBL.SavePurchaseOrder(PO, Details);
                return Json(_out, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Add(new { ErrorMessage = ex.Message });
                generalBL.LogError("Purchase", "PurchaseOrder", "Save", PO.ID, ex);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Purchase/PurchaseOrders/Edit/5
        public ActionResult Edit(int id)
        {
            PurchaseOrderBO _po = new PurchaseOrderBO();
            _po = purchaseOrderBL.GetPurchaseOrder(Convert.ToInt32(id));

            if (!_po.IsDraft || _po.IsSuspended || _po.Cancelled)
            {
                return RedirectToAction("Index");
            }
            _po.IsCancellable = purchaseOrderBL.IsPOCancellable(Convert.ToInt32(id));
            _po.SelectedQuotation = fileBL.GetAttachments(_po.SelectedQuotationID.ToString());
            _po.OtherQuotations = fileBL.GetAttachments(_po.OtherQuotationIDS);
            _po.BatchTypeList = new SelectList(BatchBL.GetBatchTypeList(), "ID", "Name");
            _po.DDLItemCategory = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            _po.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            _po.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            _po.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name");
            _po.DDLLocation = new SelectList(locationBL.getInterCompanyLocation(_po.InterCompanyLocationID), "ID", "Name");

            _po.ShippingAddressList = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            _po.BillingAddressList = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);

            _po.items = purchaseOrderBL.GetPurchaseOrderItems(Convert.ToInt32(id));
            var firstItem = _po.items.FirstOrDefault();
            _po.UnitList = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "", Value = "0" } }, "Value", "Text");
            _po.GSTList = new SelectList(new List<SelectListItem> {
                                                new SelectListItem { Text = "IncGST", Value = "1"},
                                                new SelectListItem { Text = "GSTExtra", Value = "2"}}, "Value", "Text");
            _po.GSTID = _po.InclusiveGST == true ? 1 : 2;
            _po.UnProcessedPrList = purchaseOrderBL.GetUnProcessedPurchaseRequisitionForPO();
            _po.PurchaseRequisitionIDS = String.Join(",", _po.items.Select(a => a.PurchaseRequisitionNo).Distinct());
            _po.ItemCategoryID = _po.items.FirstOrDefault().ItemCategoryID;
            SetPOViewbagItems();
            _po.IsClone = false;
            _po.CashPaymentLimit = configBL.GetCashPayementLimit();
            if (firstItem != null)
            {
                _po.CurrencyID = firstItem.CurrencyID;
                _po.CurrencyName = firstItem.CurrencyName;
                _po.CurrencyCode = firstItem.CurrencyCode;
                _po.IsGST = firstItem.IsGST;
                _po.IsVat = firstItem.IsVat;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(_po.CurrencyID);
            if (classdata != null)
            {
                _po.DecimalPlaces = classdata.DecimalPlaces;
                _po.normalclass = classdata.normalclass;
            }
            _po.OrderTypeList = new SelectList(purchaseOrderBL.GetOrderTypeList(), "Name", "Name");
            _po.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            return View(_po);

        }

        [HttpPost]
        public ActionResult Cancel(int PurchaseOrderID)
        {
            try
            {
                if (purchaseOrderBL.IsPOCancellable(PurchaseOrderID))
                {
                    purchaseOrderBL.CancelPurchaseOrder(PurchaseOrderID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDropdownVal()
        {
            List<BatchTypeBO> BatchType = new List<BatchTypeBO>()
            {
               new BatchTypeBO() {
                   ID = 0,
                   Name = "Select"
               }
            };
            BatchType.AddRange(BatchBL.GetBatchTypeList());
            return Json(new { Status = "success", data = BatchType }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRateForInterCompany(int ItemID, string BatchType)
        {
            var Rate = purchaseOrderBL.GetRateForInterCompany(ItemID, BatchType);
            return Json(new { Status = "success", data = Rate }, JsonRequestBehavior.AllowGet);
        }

        private void SetPOViewbagItems()
        {
            ViewBag.BatchType = BatchBL.GetBatchTypeList();
        }

        public ActionResult Clone(int id)
        {

            PurchaseOrderBO _po = new PurchaseOrderBO();
            _po = purchaseOrderBL.GetPurchaseOrder(Convert.ToInt32(id));

            _po.IsCancellable = purchaseOrderBL.IsPOCancellable(Convert.ToInt32(id));
            _po.PurchaseOrderNo = generalBL.GetSerialNo("PurchaseOrder", "Code");
            _po.PurchaseOrderDate = DateTime.Now;
            _po.BatchTypeList = new SelectList(BatchBL.GetBatchTypeList(), "ID", "Name");
            _po.DDLItemCategory = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            _po.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            _po.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            _po.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name");
            _po.ShippingAddressList = addressBL.GetShippingAddress("Location", null, "");
            _po.BillingAddressList = addressBL.GetBillingAddress("Location", null, "");
            _po.DDLLocation = new SelectList(locationBL.getInterCompanyLocation(_po.InterCompanyLocationID), "ID", "Name");


            _po.UnitList = new SelectList(
                    new List<SelectListItem>
                    {
                         new SelectListItem { Text = "", Value = "0"}
                    }, "Value", "Text");
            var Items = purchaseOrderBL.GetPurchaseOrderItems(Convert.ToInt32(id));
            decimal GSTPercentage = 0;
            _po.items = Items.Select(a =>
            {
                a.PRTransID = 0;
                GSTPercentage = (decimal)a.IGSTAmt == 0 ? (decimal)(a.CGSTPercent + a.SGSTPercent) : (decimal)a.IGSTPercent;
                if (!_po.IsDraft && _po.InclusiveGST && _po.IsGSTRegistred)
                {

                    a.Rate = a.Rate + a.Rate * (GSTPercentage) / 100;
                    a.Amount = a.Rate * a.Quantity;
                }

                return a;
            }).ToList();
            _po.UnProcessedPrList = purchaseOrderBL.GetUnProcessedPurchaseRequisitionForPO();
            _po.PurchaseRequisitionIDS = String.Join(",", _po.items.Select(a => a.PurchaseRequisitionNo).Distinct());
            _po.ItemCategoryID = _po.items.FirstOrDefault().ItemCategoryID;
            _po.ID = 0;
            SetPOViewbagItems();
            _po.IsClone = true;
            _po.CashPaymentLimit = configBL.GetCashPayementLimit();
            return View(_po);

        }

        public ActionResult Suspend(int ID, string Table)
        {
            var output = purchaseOrderBL.SuspendPurchaseOrder(ID, Table);
            return Json(new { Status = "success", Data = output }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuspendItem(int ID)
        {
            try
            {
                var output = purchaseOrderBL.SuspendPurchaseOrderItem(ID);
                return Json(new { Status = "success", Data = output }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Data = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        //for role privileges 
        public ActionResult approve()
        {
            return null;
        }
        public ActionResult SaveAsDraft(PurchaseOrderBO PO, List<PurchaseOrderTransBO> Details)
        {
            return Save(PO, Details);
        }

        public JsonResult GetPurchaseOrderList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SupplierNameHint = Datatable.Columns[3].Search.Value;
                string NetAmtHint = Datatable.Columns[4].Search.Value;
                string CategoryNameHint = Datatable.Columns[5].Search.Value;
                string ItemNameHint = Datatable.Columns[6].Search.Value;


                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = purchaseOrderBL.GetPurchaseOrderList(Type, TransNoHint, TransDateHint, SupplierNameHint, ItemNameHint, CategoryNameHint, NetAmtHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult Summary(int ID)
        {
            return PartialView();
        }

        public JsonResult PurchaseOrderPrintPdf(int Id)
        {
            return null;
        }

        [HttpGet]
        public ActionResult CreateV4()
        {
            PurchaseOrderBO purchaseOrder = new PurchaseOrderBO();
            purchaseOrder.DDLItemCategory = new SelectList(categoryBL.GetItemCategoryList(), "ID", "Name");
            purchaseOrder.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            purchaseOrder.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            purchaseOrder.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name");
            purchaseOrder.BatchTypeList = new SelectList(
                                               BatchBL.GetBatchTypeList(), "ID", "Name");
            purchaseOrder.UnProcessedPrList = purchaseOrderBL.GetUnProcessedPurchaseRequisitionForPO();
            purchaseOrder.DDLLocation = new SelectList(locationBL.GetLocationListByLocationHead(), "ID", "Name");

            purchaseOrder.ShippingAddressList = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            purchaseOrder.BillingAddressList = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            var obj = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.ShippingAddressID = obj[0].AddressID;
            purchaseOrder.ShippingStateID = obj[0].StateID;
            obj = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.BillingAddressID = obj[0].AddressID;
            purchaseOrder.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}

                                    }, "Value", "Text");
            purchaseOrder.GSTList = new SelectList(
                                  new List<SelectListItem>
                                  {
                                                new SelectListItem { Text = "IncGST", Value = "1"},
                                                new SelectListItem { Text = "GSTExtra", Value = "2"}

                                  }, "Value", "Text");
            purchaseOrder.GSTID = Convert.ToInt16(generalBL.GetConfig("GSTValue"));

            purchaseOrder.PurchaseOrderNo = generalBL.GetSerialNo("PurchaseOrder", "Code");
            purchaseOrder.PurchaseOrderDate = DateTime.Now;
            purchaseOrder.CashPaymentLimit = configBL.GetCashPayementLimit();
            purchaseOrder.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            purchaseOrder.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
            return View(purchaseOrder);
        }

        public JsonResult PurchaseOrderItemCodePrintPdf(int Id)
        {
            return null;
        }
        public JsonResult PurchaseOrderPartNoPrintPdf(int Id)
        {
            return null;
        }
        public JsonResult PurchaseOrderExportIemCodePrintPdf(int Id)
        {
            return null;
        }
        public JsonResult PurchaseOrderExportPartNoPrintPdf(int Id)
        {
            return null;
        }
    }
}
