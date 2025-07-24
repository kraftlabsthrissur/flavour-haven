using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class PurchaseInvoiceController : Controller
    {
        private IDropdownContract _dropdown;
        private IPurchaseOrder purchaseOrderBL;
        private IPurchaseInvoice purchaseInvoiceBL;
        private IGoodsReceiptNoteContract goodsReceiptNoteBL;
        private IGSTContract GstBL;
        private IAddressContract addressBL;
        private IGeneralContract generalBL;
        private IGSTCategoryContract gSTCategoryBL;
        private ICategoryContract categroyBL;
        private string createmessage = App_LocalResources.Common.createsucess;
        private string failureMessage = App_LocalResources.Common.createfail;
        private IFileContract fileBL;
        private ILocationContract locationBL;
        private ICounterSalesContract counterSalesBL;
        public PurchaseInvoiceController(IDropdownContract IDropdown)
        {
            _dropdown = IDropdown;
            purchaseOrderBL = new PurchaseOrderBL();
            purchaseInvoiceBL = new PurchaseInvoiceBL();
            goodsReceiptNoteBL = new GoodsReceiptNoteBL();
            GstBL = new GSTBL();
            addressBL = new AddressBL();
            generalBL = new GeneralBL();
            fileBL = new FileBL();
            gSTCategoryBL = new GSTCategoryBL();
            categroyBL = new CategoryBL();
            locationBL = new LocationBL();
            counterSalesBL = new CounterSalesBL();
        }

        // GET: Purchase/PurchaseInvoice
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "cancelled" };
            return View();
        }

        public ActionResult IndexV4()
        {
            ViewBag.Statuses = new List<string>() { "draft", "cancelled" };
            return View();
        }

        // GET: Purchase/PurchaseInvoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            PurchaseInvoiceBO purchaseInvoiceBO = purchaseInvoiceBL.GetPurchaseInvoice((int)id);
            PurchaseInvoiceModel PurchaseInvoiceModel = new PurchaseInvoiceModel()
            {
                Id = purchaseInvoiceBO.Id,
                TransNo = purchaseInvoiceBO.PurchaseNo,
                TransDate = General.FormatDateNull(purchaseInvoiceBO.PurchaseDate, "view"),
                InvoiceNo = purchaseInvoiceBO.InvoiceNo,
                InvoiceDate = General.FormatDateNull(purchaseInvoiceBO.InvoiceDate, "view"),
                SupplierID = (int)purchaseInvoiceBO.SupplierID,
                SupplierName = purchaseInvoiceBO.SupplierName,
                LocalSupplierName = purchaseInvoiceBO.LocalSupplierName,
                NetAmount = (decimal)purchaseInvoiceBO.NetAmount,
                InvoiceTotal = purchaseInvoiceBO.InvoiceTotal,
                TotalDifference = purchaseInvoiceBO.TotalDifference,
                TotalFreight = purchaseInvoiceBO.FreightAmount,
                TDSOnFreight = purchaseInvoiceBO.TDSOnFreight,
                LessTDS = purchaseInvoiceBO.LessTDS,
                Discount = purchaseInvoiceBO.Discount,
                SupplierOtherCharges = purchaseInvoiceBO.SupplierOtherCharges,
                AmountPayable = purchaseInvoiceBO.AmountPayable,
                OtherDeductions = purchaseInvoiceBO.OtherDeductions,
                IsDraft = purchaseInvoiceBO.IsDraft,
                Status = purchaseInvoiceBO.Status,
                Date = General.FormatDate(purchaseInvoiceBO.PurchaseOrderDate, "view"),
                SupplierCode = purchaseInvoiceBO.SupplierCode,
                TDSCode = purchaseInvoiceBO.TDSCode,
                TDSID = purchaseInvoiceBO.TDSID,
                SupplierLocation = purchaseInvoiceBO.SupplierLocation,
                GSTNo = purchaseInvoiceBO.GSTNo,
                IsCancelled = purchaseInvoiceBO.IsCancelled,
                Remarks = purchaseInvoiceBO.Remarks,

            };

            PurchaseInvoiceModel.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");

            PurchaseInvoiceModel.Items = purchaseInvoiceBL.GetPurchaseInvoiceTrans((int)id);
            PurchaseInvoiceModel.OtherChargeDetails = purchaseInvoiceBL.GetPurchaseInvoiceOtherChargeDetails((int)id);
            PurchaseInvoiceModel.TaxDetails = purchaseInvoiceBL.GetPurchaseInvoiceTaxDetails((int)id);
            PurchaseInvoiceModel.SelectedQuotation = fileBL.GetAttachments(purchaseInvoiceBO.SelectedQuotationID.ToString());

            return View(PurchaseInvoiceModel);
        }
        public ActionResult GenerateDetails(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            PurchaseInvoiceBO purchaseInvoiceBO = purchaseInvoiceBL.GetPurchaseInvoiceDetails((int)id);
            PurchaseInvoiceModel PurchaseInvoiceModel = new PurchaseInvoiceModel()
            {
                Id = purchaseInvoiceBO.Id,
                TransNo = purchaseInvoiceBO.PurchaseNo,
                TransDate = General.FormatDateNull(purchaseInvoiceBO.PurchaseDate, "view"),
                InvoiceNo = purchaseInvoiceBO.InvoiceNo,
                InvoiceDate = General.FormatDateNull(purchaseInvoiceBO.InvoiceDate, "view"),
                SupplierID = (int)purchaseInvoiceBO.SupplierID,
                SupplierName = purchaseInvoiceBO.SupplierName,
                LocalSupplierName = purchaseInvoiceBO.LocalSupplierName,
                NetAmount = (decimal)purchaseInvoiceBO.NetAmount,
                InvoiceTotal = purchaseInvoiceBO.InvoiceTotal,
                TotalDifference = purchaseInvoiceBO.TotalDifference,
                TotalFreight = purchaseInvoiceBO.FreightAmount,
                TDSOnFreight = purchaseInvoiceBO.TDSOnFreight,
                LessTDS = purchaseInvoiceBO.LessTDS,
                Discount = purchaseInvoiceBO.Discount,
                SupplierOtherCharges = purchaseInvoiceBO.SupplierOtherCharges,
                AmountPayable = purchaseInvoiceBO.AmountPayable,
                OtherDeductions = purchaseInvoiceBO.OtherDeductions,
                IsDraft = purchaseInvoiceBO.IsDraft,
                Status = purchaseInvoiceBO.Status,
                Date = General.FormatDate(purchaseInvoiceBO.PurchaseOrderDate, "view"),
                SupplierCode = purchaseInvoiceBO.SupplierCode,
                TDSCode = purchaseInvoiceBO.TDSCode,
                TDSID = purchaseInvoiceBO.TDSID,
                SupplierLocation = purchaseInvoiceBO.SupplierLocation,
                GSTNo = purchaseInvoiceBO.GSTNo,
                IsCancelled = purchaseInvoiceBO.IsCancelled,
                Remarks = purchaseInvoiceBO.Remarks,
                IGSTAmt = purchaseInvoiceBO.IGST,
                SGSTAmt = purchaseInvoiceBO.SGST,
                CGSTAmt = purchaseInvoiceBO.CGST,
                GrossAmount = purchaseInvoiceBO.GrossAmount,
                GRNNo = purchaseInvoiceBO.GrnNo,
                InvoiceType = purchaseInvoiceBO.InvoiceType,
            };


            PurchaseInvoiceModel.Items = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails((int)id);
            PurchaseInvoiceModel.SelectedQuotation = fileBL.GetAttachments(purchaseInvoiceBO.SelectedQuotationID.ToString());

            return View(PurchaseInvoiceModel);
        }

        #region Create
        // GET: Purchase/PurchaseInvoice/Create
        public ActionResult Create()
        {
            PurchaseInvoiceModel purchaseInvoice = new PurchaseInvoiceModel();
            purchaseInvoice.TransDate = General.FormatDate(DateTime.Now);
            purchaseInvoice.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");

            purchaseInvoice.TransNo = generalBL.GetSerialNo("PurchaseInvoice", "Code");

            purchaseInvoice.Items = new List<GRNTransItemBO>();
            purchaseInvoice.OtherChargeDetails = new List<PurchaseInvoiceOtherChargeDetailBO>();
            purchaseInvoice.TaxDetails = new List<PurchaseInvoiceTaxDetailBO>();
            purchaseInvoice.GstList = GstBL.GetGstList();

            var obj = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "");
            purchaseInvoice.ShippingStateID = obj[0].StateID;

            ViewBag.TaxPercentages = new SelectList(GstBL.GetGstList(), "IGSTPercentage", "GSTPercentage");
            purchaseInvoice.InvoiceTypeList = new SelectList(purchaseInvoiceBL.GetInvoiceTypeList(), "Name", "Name");
            return View(purchaseInvoice);
        }

        /// <summary>
        /// Once the GRN selected, get the Items, OtherCharges, TaxDetails view as string
        /// </summary>
        /// <returns></returns>
        //[OutputCache (Duration =30, VaryByParam = "grnIDs")]           //UnComment after testing
        [HttpPost]
        public PartialViewResult GetUnProcessedGRNItems(int[] grnIDS)
        {
            List<GRNTransItemBO> grnTransItemBOList = null;
            if (grnIDS != null && grnIDS.Count() > 0)
            {
                foreach (var grnID in grnIDS)
                {
                    var list = goodsReceiptNoteBL.GetUnProcessedGRNItems(grnID);
                    if (grnTransItemBOList == null)
                    {
                        grnTransItemBOList = new List<GRNTransItemBO>();
                    }

                    if (list != null)
                    {

                        ViewBag.TaxPercentages = GstBL.GetGstList();
                        grnTransItemBOList.AddRange(list);
                    }
                }
            }
            else
            {
                grnTransItemBOList = new List<GRNTransItemBO>();
            }
            return PartialView("~/Areas/Purchase/Views/PurchaseInvoice/_unProcessedItems.cshtml", grnTransItemBOList);
        }

        public PartialViewResult GetUnProcessedMilkItems(int[] grnIDS)
        {
            List<GRNTransItemBO> grnTransItemBOList = new List<GRNTransItemBO>();

            foreach (var grnID in grnIDS)
            {
                var list = goodsReceiptNoteBL.GetUnProcessedMilkItems(grnID);
                ViewBag.TaxPercentages = GstBL.GetGstList();
                grnTransItemBOList.AddRange(list);
            }

            return PartialView("~/Areas/Purchase/Views/PurchaseInvoice/_unProcessedItems.cshtml", grnTransItemBOList);
        }

        /// <summary>
        /// Get available GST Percentages from list
        /// </summary>
        /// <param name="availPercentages"></param>
        /// <returns></returns>
        [NonAction]
        private List<GSTBO> GetAvailGSTPercentages(List<decimal> availPercentages = null)
        {
            IGSTContract gstBL = new GSTBL();

            return gstBL.GetGstList();

        }

        /// <summary>
        /// Group the Items by GST based on location
        /// </summary>
        /// <param name="purchaseOrderTransBOList"></param>
        /// <param name="isLocal"></param>
        /// <param name="availPercentages"></param>
        /// <returns></returns>

        [NonAction]
        private List<PurchaseInvoiceTaxViewModel> GetGroupedItems(List<PurchaseOrderTransBO> purchaseOrderTransBOList, bool isLocal, out List<decimal> availPercentages)
        {
            availPercentages = new List<decimal>();
            if (isLocal)
            {
                availPercentages.AddRange(purchaseOrderTransBOList.Where(x => x.CGSTPercent > 0).Select(x => x.CGSTPercent ?? 0).ToList());
                availPercentages.AddRange(purchaseOrderTransBOList.Where(x => x.SGSTPercent > 0).Select(x => x.SGSTPercent ?? 0).ToList());

                availPercentages = availPercentages.Distinct().ToList();

                return (from potl in purchaseOrderTransBOList
                        group potl by new { potl.CGSTPercent, potl.SGSTPercent } into grpd
                        select new PurchaseInvoiceTaxViewModel()
                        {
                            CGSTPercent = grpd.Key.CGSTPercent ?? 0,
                            CGSTAmt = grpd.Sum(x => x.CGSTAmt ?? 0), //(grpd.Key.CGSTPercent ?? 0 * grpd.Count()),
                            SGSTPercent = grpd.Key.SGSTPercent ?? 0, //grpd.Key.SGSTPercent ?? 0,
                            SGSTAmt = grpd.Sum(x => x.SGSTAmt ?? 0),
                            Count = grpd.Count(),
                            PurchaseOrderTransBO = grpd.First()
                        }).ToList();

            }
            else
            {
                availPercentages.AddRange(purchaseOrderTransBOList.Where(x => x.IGSTPercent > 0).Select(x => x.IGSTPercent ?? 0).Distinct().ToList());
                return purchaseOrderTransBOList.GroupBy(info => info.IGSTPercent)
                 .Select(group => new PurchaseInvoiceTaxViewModel()
                 {
                     IGSTPercent = group.Key ?? 0,
                     PurchaseOrderTransBO = group.First(),
                     IGSTAmt = group.Sum(x => x.IGSTAmt ?? 0),  //(group.Key ?? 0 * group.Count()),
                     Count = group.Count()
                 })
                 .OrderBy(x => x.PurchaseOrderTransBO.IGSTPercent).ToList();

            }
        }

        /// <summary>
        /// Save new PurchaseInvoice
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(PurchaseInvoiceSaveViewModel purchaseInvoice)
        {
            var result = new List<object>();
            try
            {
                if (purchaseInvoice.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PurchaseInvoiceBO Temp = purchaseInvoiceBL.GetPurchaseInvoice(purchaseInvoice.ID);
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var isInvoiceNoValid = purchaseOrderBL.CheckInvoiceNumberValid(purchaseInvoice.SupplierID, purchaseInvoice.InvoiceNo);
                if (isInvoiceNoValid || purchaseInvoice.ID != 0)
                {
                    var purchaseInvoiceBO = purchaseInvoice.MapToBo();
                    var purchaseInvoiceID = purchaseInvoiceBL.SavePurchaseInvoice(purchaseInvoiceBO);
                    if (purchaseInvoiceID > 0)
                    {
                        return Json(new { Status = "success", PurchaseInvoiceID = purchaseInvoiceID, Message = "Purchase invoice " + createmessage }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result.Add(new { ErrorMessage = failureMessage + " purchase invoice " });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    result.Add(new { ErrorMessage = " Invalid Invoice no " });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DuplicateEntryException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseInvoice", "Save", purchaseInvoice.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (QuantityExceededException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseInvoice", "Save", purchaseInvoice.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GenereateSave(PurchaseInvoiceSaveViewModel purchaseInvoice)
        {
            var result = new List<object>();
            try
            {
                if (purchaseInvoice.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PurchaseInvoiceBO Temp = purchaseInvoiceBL.GetPurchaseInvoice(purchaseInvoice.ID);
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var isInvoiceNoValid = purchaseOrderBL.CheckInvoiceNumberValid(purchaseInvoice.SupplierID, purchaseInvoice.InvoiceNo);
                if (isInvoiceNoValid || purchaseInvoice.ID != 0)
                {
                    var purchaseInvoiceBO = purchaseInvoice.MapToBo();
                    var purchaseInvoiceID = purchaseInvoiceBL.GeneratePurchaseInvoice(purchaseInvoiceBO);
                    if (purchaseInvoiceID > 0)
                    {
                        return Json(new { Status = "success", PurchaseInvoiceID = purchaseInvoiceID, Message = "Purchase invoice " + createmessage }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result.Add(new { ErrorMessage = failureMessage + " purchase invoice " });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    result.Add(new { ErrorMessage = " Invalid Invoice no " });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DuplicateEntryException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseInvoice", "GenereateSave", purchaseInvoice.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (QuantityExceededException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseInvoice", "GenereateSave", purchaseInvoice.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (LessProfitException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseInvoice", "GenereateSave", purchaseInvoice.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region Testing
        [NonAction]
        private List<GRNTransItemBO> GetUnProcessedTransList_Test()
        {
            return new List<GRNTransItemBO>() {
                        new GRNTransItemBO() { AcceptedQty=10, ApprovedQty=8, ApprovedValue=3, CGSTAmt=Convert.ToDecimal(1.23), CGSTPercent=1, FreightAmt=Convert.ToDecimal(1.2), IGSTAmt=3, IGSTPercent=Convert.ToDecimal(1.3), ItemID=187, ItemName="Item1", OtherCharges=4, PackingShippingCharge=5, PORate=2, PurchaseOrderID=5, SGSTAmt=3, SGSTPercent=Convert.ToDecimal(1.4), Unit="KG", UnitID=1, UnMatchedQty=3   },
                        new GRNTransItemBO() { AcceptedQty=10, ApprovedQty=7, ApprovedValue=4, CGSTAmt=Convert.ToDecimal(1.23), CGSTPercent=1, FreightAmt=Convert.ToDecimal(1.2), IGSTAmt=3, IGSTPercent=Convert.ToDecimal(1.3), ItemID=187, ItemName="Item2", OtherCharges=4, PackingShippingCharge=5, PORate=2, PurchaseOrderID=9, SGSTAmt=3, SGSTPercent=Convert.ToDecimal(1.4), Unit="KG", UnitID=1, UnMatchedQty=3   }
                    };
        }

        #endregion

        #region Check Invoice Valid
        /// <summary>
        /// Check entered invoice no is not duplicated
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public JsonResult CheckInvoiceNoValid(int supplierID, string invoiceNo)
        {
            var result = purchaseOrderBL.CheckInvoiceNumberValid(supplierID, invoiceNo);
            return Json(new { IsValid = result, Message = result ? "Valid" : "Invoice number already exists" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        // GET: Purchase/PurchaseInvoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return View("PageNotFound");
            }
            try
            {
                PurchaseInvoiceBO purchaseInvoiceBO = new PurchaseInvoiceBO();

                purchaseInvoiceBO = purchaseInvoiceBL.GetPurchaseInvoice((int)id);
                if (!purchaseInvoiceBO.IsDraft || purchaseInvoiceBO.IsCancelled)
                {
                    return RedirectToAction("Index"); ;
                }

                PurchaseInvoiceModel PurchaseInvoiceModel = new PurchaseInvoiceModel()
                {
                    Id = purchaseInvoiceBO.Id,
                    TransNo = purchaseInvoiceBO.PurchaseNo,
                    TransDate = General.FormatDateNull(purchaseInvoiceBO.PurchaseDate),
                    InvoiceNo = purchaseInvoiceBO.InvoiceNo,
                    InvoiceDate = General.FormatDateNull(purchaseInvoiceBO.InvoiceDate),
                    SupplierID = (int)purchaseInvoiceBO.SupplierID,
                    SupplierName = purchaseInvoiceBO.SupplierName,
                    LocalSupplierName = purchaseInvoiceBO.LocalSupplierName,
                    NetAmount = (decimal)purchaseInvoiceBO.NetAmount,
                    InvoiceTotal = purchaseInvoiceBO.InvoiceTotal,
                    TotalDifference = purchaseInvoiceBO.TotalDifference,
                    TotalFreight = purchaseInvoiceBO.FreightAmount,
                    TDSOnFreight = purchaseInvoiceBO.TDSOnFreight,
                    LessTDS = purchaseInvoiceBO.LessTDS,
                    Discount = purchaseInvoiceBO.Discount,
                    SupplierOtherCharges = purchaseInvoiceBO.SupplierOtherCharges,
                    AmountPayable = purchaseInvoiceBO.AmountPayable,
                    OtherDeductions = purchaseInvoiceBO.OtherDeductions,
                    IsDraft = purchaseInvoiceBO.IsDraft,
                    StateID = purchaseInvoiceBO.StateID,
                    IsGSTRegistered = purchaseInvoiceBO.IsGSTRegistered,
                    Date = General.FormatDate(purchaseInvoiceBO.PurchaseOrderDate),
                    SupplierCode = purchaseInvoiceBO.SupplierCode,
                    SupplierCategory = purchaseInvoiceBO.SupplierCategory,
                    TDSCode = purchaseInvoiceBO.TDSCode,
                    TDSID = purchaseInvoiceBO.TDSID,
                    Rate = purchaseInvoiceBO.Rate,
                    SupplierLocation = purchaseInvoiceBO.SupplierLocation,
                    GSTNo = purchaseInvoiceBO.GSTNo,
                    Remarks = purchaseInvoiceBO.Remarks,

                };
                PurchaseInvoiceModel.TDSCode = purchaseInvoiceBO.TDSCode;
                PurchaseInvoiceModel.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description", PurchaseInvoiceModel.Rate);

                PurchaseInvoiceModel.Items = purchaseInvoiceBL.GetPurchaseInvoiceTrans((int)id);
                PurchaseInvoiceModel.OtherChargeDetails = purchaseInvoiceBL.GetPurchaseInvoiceOtherChargeDetails((int)id);
                PurchaseInvoiceModel.TaxDetails = purchaseInvoiceBL.GetPurchaseInvoiceTaxDetails((int)id);
                PurchaseInvoiceModel.GstList = GstBL.GetGstList();
                PurchaseInvoiceModel.ShippingStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
                PurchaseInvoiceModel.SelectedQuotation = fileBL.GetAttachments(purchaseInvoiceBO.SelectedQuotationID.ToString());
                ViewBag.TaxPercentages = new SelectList(GstBL.GetGstList(), "IGSTPercentage", "GSTPercentage");
                PurchaseInvoiceModel.InvoiceTypeList = new SelectList(purchaseInvoiceBL.GetInvoiceTypeList(), "Name", "Name");
                return View(PurchaseInvoiceModel);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                ViewBag.StackTrace = e.StackTrace;
                return View("Error");
            }
        }

        public ActionResult Approve(int ID, String Status)
        {
            try
            {
                purchaseInvoiceBL.Approve(ID, Status);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        #region Helper

        #region Render view to string
        [NonAction]
        private string RenderPartialViewToString(Controller controller, string viewName, object model, List<KeyValuePair<string, dynamic>> viewBagItems = null)
        {
            controller.ViewData.Model = model;
            if (viewBagItems != null && viewBagItems.Count > 0)
            {
                foreach (var item in viewBagItems)
                {
                    controller.ViewData.Add(item.Key, item.Value);
                    //controller.ViewBag[item.Key] = "Test";
                    //controller.ViewBag[item.Key] = item.Value;
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

        #endregion
        public JsonResult GetPurchaseInvoiceWithItemID(int id)
        {
            List<PurchaseInvoiceBO> PurchaseInvoice = purchaseInvoiceBL.GetPurchaseInvoiceWithItemID(id).ToList();
            return Json(new { Status = "success", data = PurchaseInvoice }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStockInvoiceTransForPurchaseReturn(int ItemID, int InvoiceID)
        {
            return Json(new { Status = "success", data = purchaseInvoiceBL.GetStockInvoiceTransForPurchaseReturn(ItemID, InvoiceID) }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetPurchaseInvoiceList(int SupplierID)
        {
            List<PurchaseInvoiceModel> SalesInvoice = new List<PurchaseInvoiceModel>();

            SalesInvoice = purchaseInvoiceBL.GetInvoiceList(SupplierID).Select(m => new PurchaseInvoiceModel()
            {
                InvoiceNo = m.InvoiceNo,
                InvoiceDate = General.FormatDateNull(m.InvoiceDate, "view"),
                NetAmount = m.NetAmount,
                Id = m.Id,
                PurchaseNo = m.PurchaseNo
            }).ToList();

            return Json(SalesInvoice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseInvoice(int[] InvoiceIDS)

        {
            try
            {
                PurchaseInvoiceBO purchaseInvoice = new PurchaseInvoiceBO();
                purchaseInvoice.InvoiceTransItems = new List<PurchaseInvoiceTransItemBO>();

                if (InvoiceIDS.Length > 0)
                {

                    foreach (var SalesInvoiceID in InvoiceIDS)
                    {
                        var list = purchaseInvoiceBL.GetInvoiceTransList(SalesInvoiceID);

                        if (list != null)
                        {

                            purchaseInvoice.InvoiceTransItems.AddRange(list);
                        }
                        purchaseInvoice.SuppDocAmount = purchaseInvoice.SuppDocAmount + purchaseInvoice.InvoiceTransItems.First().SuppDocAmount;
                        purchaseInvoice.SuppShipAmount = purchaseInvoice.SuppShipAmount + purchaseInvoice.InvoiceTransItems.First().SuppShipAmount;
                        purchaseInvoice.PackingForwarding = purchaseInvoice.PackingForwarding + purchaseInvoice.InvoiceTransItems.First().PackingForwarding;
                        purchaseInvoice.SupplierOtherCharges = purchaseInvoice.SupplierOtherCharges + purchaseInvoice.InvoiceTransItems.First().SuppOtherCharge;
                        purchaseInvoice.FreightAmount = purchaseInvoice.FreightAmount + purchaseInvoice.InvoiceTransItems.First().FreightAmt ?? 0;

                        purchaseInvoice.LocalCustomsDuty = purchaseInvoice.LocalCustomsDuty + purchaseInvoice.InvoiceTransItems.First().LocalCustomsDuty;
                        purchaseInvoice.LocalFreight = purchaseInvoice.LocalFreight + purchaseInvoice.InvoiceTransItems.First().LocalFreight;
                        purchaseInvoice.LocalMiscCharge = purchaseInvoice.LocalMiscCharge + purchaseInvoice.InvoiceTransItems.First().LocalMiscCharge;
                        purchaseInvoice.LocalOtherCharges = purchaseInvoice.LocalOtherCharges + purchaseInvoice.InvoiceTransItems.First().LocalOtherCharges;
                    }
                }

                return Json(purchaseInvoice, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Cancel(int ID, string Table)
        {
            purchaseInvoiceBL.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {

            var count = purchaseInvoiceBL.GetInvoiceNumberCount(Hint, Table, SupplierID);

            return Json(new { Status = "success", data = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPurchaseInvoiceList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string InvoiceNoHint = Datatable.Columns[3].Search.Value;
                string InvoiceDateHint = Datatable.Columns[4].Search.Value;
                string SupplierNameHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = purchaseInvoiceBL.GetPurchaseInvoiceList(Type, TransNoHint, TransDateHint, InvoiceNoHint, InvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveAsDraft(PurchaseInvoiceSaveViewModel purchaseInvoice)
        {
            return Save(purchaseInvoice);
        }

        public ActionResult GenerateSaveAsDraft(PurchaseInvoiceSaveViewModel purchaseInvoice)
        {
            return GenereateSave(purchaseInvoice);
        }

        public ActionResult ApproveSave(PurchaseInvoiceSaveViewModel purchaseInvoice)
        {
            var result = new List<object>();/// purchase invoice
            try
            {
                if (purchaseInvoice.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PurchaseInvoiceBO Temp = purchaseInvoiceBL.GetPurchaseInvoice(purchaseInvoice.ID);
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var isInvoiceNoValid = purchaseOrderBL.CheckInvoiceNumberValid(purchaseInvoice.SupplierID, purchaseInvoice.InvoiceNo);
                if (isInvoiceNoValid || purchaseInvoice.ID != 0)
                {
                    var purchaseInvoiceBO = purchaseInvoice.MapToBo();
                    var purchaseInvoiceID = purchaseInvoiceBL.ApprovePurchaseInvoice(purchaseInvoiceBO);
                    if (purchaseInvoiceID > 0)
                    {
                        return Json(new { Status = "success", PurchaseInvoiceID = purchaseInvoiceID, Message = "Purchase invoice " + createmessage }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result.Add(new { ErrorMessage = failureMessage + " purchase invoice " });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    result.Add(new { ErrorMessage = " Invalid Invoice no " });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DuplicateEntryException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseInvoice", "SaveApproveData", purchaseInvoice.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (QuantityExceededException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseInvoice", "SaveApproveData", purchaseInvoice.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult Generate()
        {
            PurchaseInvoiceModel purchaseInvoice = new PurchaseInvoiceModel();
            purchaseInvoice.TransDate = General.FormatDate(DateTime.Now);
            purchaseInvoice.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");

            purchaseInvoice.TransNo = generalBL.GetSerialNo("PurchaseInvoice", "Code");

            purchaseInvoice.Items = new List<GRNTransItemBO>();
            purchaseInvoice.OtherChargeDetails = new List<PurchaseInvoiceOtherChargeDetailBO>();
            purchaseInvoice.TaxDetails = new List<PurchaseInvoiceTaxDetailBO>();
            //purchaseInvoice.GstList = GstBL.GetGstList();
            var obj = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "");
            purchaseInvoice.ShippingStateID = obj[0].StateID;
            //purchaseInvoice.GSTPercentageList = new SelectList(gSTCategoryBL.GetGSTList(), "ID", "IGSTPercent");
            ViewBag.TaxPercentages = new SelectList(GstBL.GetGstList(), "IGSTPercentage", "GSTPercentage");
            return View(purchaseInvoice);
        }

        //For  GetGRNItemsForPurchaseInvoice FOR Allopathy(In Ayurware GetUnProcessedGRNItems)
        [HttpPost]
        public PartialViewResult GRNItemsForPurchaseInvoice(int[] grnIDS)
        {
            List<GRNTransItemBO> grnTransItemBOList = null;
            if (grnIDS != null && grnIDS.Count() > 0)
            {
                foreach (var grnID in grnIDS)
                {
                    var list = goodsReceiptNoteBL.GetGRNItemsForPurchaseInvoice(grnID);
                    if (grnTransItemBOList == null)
                    {
                        grnTransItemBOList = new List<GRNTransItemBO>();
                    }

                    if (list != null)
                    {
                        ViewBag.TaxPercentages = GstBL.GetGstList();
                        grnTransItemBOList.AddRange(list);
                    }
                }
            }
            else
            {
                grnTransItemBOList = new List<GRNTransItemBO>();
            }
            return PartialView("~/Areas/Purchase/Views/PurchaseInvoice/GRNItemsForPurchaseInvoice.cshtml", grnTransItemBOList);
        }

        [HttpPost]
        public PartialViewResult GRNItemsForPurchaseInvoiceV3(int[] grnIDS, string normalclass = "")
        {
            List<GRNTransItemBO> grnTransItemBOList = null;
            decimal SuppDocAmount = 0;
            decimal SuppOtherCharge = 0;
            decimal SuppShipAmount = 0;
            decimal SuppFreight = 0;
            decimal LocalCustomsDuty = 0;
            decimal LocalFreight = 0;
            decimal LocalMiscCharge = 0;
            decimal LocalOtherCharges = 0;
            decimal PackingForwarding = 0;
            if (grnIDS != null && grnIDS.Count() > 0)
            {

                foreach (var grnID in grnIDS)
                {
                    var list = goodsReceiptNoteBL.GetGRNItemsForPurchaseInvoice(grnID);
                    if (grnTransItemBOList == null)
                    {
                        grnTransItemBOList = new List<GRNTransItemBO>();
                    }

                    if (list != null)
                    {
                        ViewBag.TaxPercentages = GstBL.GetGstList();
                        grnTransItemBOList.AddRange(list);
                    }
                    if (list.Count > 0)
                    {
                        SuppDocAmount += list.First().SuppDocAmount;
                        SuppOtherCharge += list.First().SuppOtherCharge;
                        SuppShipAmount += list.First().SuppShipAmount;
                        SuppFreight += list.First().SuppFreight;
                        PackingForwarding += list.First().PackingForwarding;
                        LocalCustomsDuty += list.First().LocalCustomsDuty;
                        LocalFreight += list.First().LocalFreight;
                        LocalMiscCharge += list.First().LocalMiscCharge;
                        LocalOtherCharges += list.First().LocalOtherCharges;

                    }
                }
            }
            else
            {
                grnTransItemBOList = new List<GRNTransItemBO>();
            }
            ViewBag.normalclass = normalclass;
            ViewBag.SuppDocAmount = SuppDocAmount;
            ViewBag.SuppShipAmount = SuppShipAmount;
            ViewBag.SuppOtherCharge = SuppOtherCharge;

            ViewBag.SuppFreight = SuppFreight;
            ViewBag.LocalCustomsDuty = LocalCustomsDuty;
            ViewBag.LocalFreight = LocalFreight;
            ViewBag.LocalMiscCharge = LocalMiscCharge;
            ViewBag.LocalOtherCharges = LocalOtherCharges;
            ViewBag.PackingForwarding = PackingForwarding;
            return PartialView("~/Areas/Purchase/Views/PurchaseInvoice/GRNItemsForPurchaseInvoiceV3.cshtml", grnTransItemBOList);
        }

        public JsonResult GRNIMasterForPurchaseInvoice(int grnIDS)
        {
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetail(grnIDS).Select(a => new GRNModel()
            {
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                NetAmount = a.NetAmount,
                InvoiceDate = General.FormatDate((DateTime)a.DeliveryChallanDate),
                InvoiceNo = a.DeliveryChallanNo
            }).FirstOrDefault();
            return Json(new { Status = "failure", data = grnModel }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GRNMasterForPurchaseInvoiceV3(int[] grnIDS)
        {
            GRNModel grnModel = goodsReceiptNoteBL.GetGRNDetailV3(grnIDS).Select(a => new GRNModel()
            {
                IGSTAmt = a.IGSTAmt,
                SGSTAmt = a.SGSTAmt,
                CGSTAmt = a.CGSTAmt,
                RoundOff = a.RoundOff,
                DiscountAmt = a.DiscountAmt,
                GrossAmt = a.GrossAmt,
                NetAmount = a.NetAmount
                //InvoiceDate = General.FormatDate((DateTime)a.
                //InvoiceNo = a.DeliveryChallanNo
            }).FirstOrDefault();
            return Json(new { Status = "failure", data = grnModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateEdit(int? id)
        {
            if (id == null || id <= 0)
            {
                return View("PageNotFound");
            }
            try
            {
                PurchaseInvoiceBO purchaseInvoiceBO = new PurchaseInvoiceBO();

                purchaseInvoiceBO = purchaseInvoiceBL.GetPurchaseInvoiceDetails((int)id);
                if (!purchaseInvoiceBO.IsDraft || purchaseInvoiceBO.IsCancelled)
                {
                    return RedirectToAction("Index"); ;
                }

                PurchaseInvoiceModel PurchaseInvoiceModel = new PurchaseInvoiceModel()
                {
                    Id = purchaseInvoiceBO.Id,
                    TransNo = purchaseInvoiceBO.PurchaseNo,
                    TransDate = General.FormatDateNull(purchaseInvoiceBO.PurchaseDate),
                    InvoiceNo = purchaseInvoiceBO.InvoiceNo,
                    InvoiceDate = General.FormatDateNull(purchaseInvoiceBO.InvoiceDate),
                    SupplierID = (int)purchaseInvoiceBO.SupplierID,
                    SupplierName = purchaseInvoiceBO.SupplierName,
                    LocalSupplierName = purchaseInvoiceBO.LocalSupplierName,
                    NetAmount = (decimal)purchaseInvoiceBO.NetAmount,
                    InvoiceTotal = purchaseInvoiceBO.InvoiceTotal,
                    TotalDifference = purchaseInvoiceBO.TotalDifference,
                    TotalFreight = purchaseInvoiceBO.FreightAmount,
                    TDSOnFreight = purchaseInvoiceBO.TDSOnFreight,
                    LessTDS = purchaseInvoiceBO.LessTDS,
                    Discount = purchaseInvoiceBO.Discount,
                    SupplierOtherCharges = purchaseInvoiceBO.SupplierOtherCharges,
                    AmountPayable = purchaseInvoiceBO.AmountPayable,
                    OtherDeductions = purchaseInvoiceBO.OtherDeductions,
                    IsDraft = purchaseInvoiceBO.IsDraft,
                    StateID = purchaseInvoiceBO.StateID,
                    IsGSTRegistered = purchaseInvoiceBO.IsGSTRegistered,
                    Date = General.FormatDate(purchaseInvoiceBO.PurchaseOrderDate),
                    SupplierCode = purchaseInvoiceBO.SupplierCode,
                    SupplierCategory = purchaseInvoiceBO.SupplierCategory,
                    TDSCode = purchaseInvoiceBO.TDSCode,
                    TDSID = purchaseInvoiceBO.TDSID,
                    Rate = purchaseInvoiceBO.Rate,
                    SupplierLocation = purchaseInvoiceBO.SupplierLocation,
                    GSTNo = purchaseInvoiceBO.GSTNo,
                    Remarks = purchaseInvoiceBO.Remarks,
                    IGSTAmt = purchaseInvoiceBO.IGST,
                    SGSTAmt = purchaseInvoiceBO.SGST,
                    CGSTAmt = purchaseInvoiceBO.CGST,
                    GrossAmount = purchaseInvoiceBO.GrossAmount,
                    GRNNo = purchaseInvoiceBO.GrnNo,
                    InvoiceType = purchaseInvoiceBO.InvoiceType,
                };

                PurchaseInvoiceModel.Items = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails((int)id);
                PurchaseInvoiceModel.ShippingStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
                PurchaseInvoiceModel.SelectedQuotation = fileBL.GetAttachments(purchaseInvoiceBO.SelectedQuotationID.ToString());
                ViewBag.TaxPercentages = new SelectList(GstBL.GetGstList(), "IGSTPercentage", "GSTPercentage");
                PurchaseInvoiceModel.InvoiceTypeList = new SelectList(purchaseInvoiceBL.GetInvoiceTypeList(), "Name", "Name");
                return View(PurchaseInvoiceModel);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                ViewBag.StackTrace = e.StackTrace;
                return View("Error");
            }
        }

        public ActionResult GenerateV4()
        {
            PurchaseInvoiceModel purchaseInvoice = new PurchaseInvoiceModel();
            purchaseInvoice.TransDate = General.FormatDate(DateTime.Now);
            purchaseInvoice.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");

            purchaseInvoice.TransNo = generalBL.GetSerialNo("PurchaseInvoice", "Code");

            purchaseInvoice.Items = new List<GRNTransItemBO>();
            purchaseInvoice.OtherChargeDetails = new List<PurchaseInvoiceOtherChargeDetailBO>();
            purchaseInvoice.TaxDetails = new List<PurchaseInvoiceTaxDetailBO>();
            //purchaseInvoice.GstList = GstBL.GetGstList();
            var obj = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "");
            purchaseInvoice.ShippingStateID = obj[0].StateID;
            purchaseInvoice.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            purchaseInvoice.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
            //purchaseInvoice.GSTPercentageList = new SelectList(gSTCategoryBL.GetGSTList(), "ID", "IGSTPercent");
            ViewBag.TaxPercentages = new SelectList(GstBL.GetGstList(), "IGSTPercentage", "GSTPercentage");
            purchaseInvoice.InvoiceTypeList = new SelectList(purchaseInvoiceBL.GetInvoiceTypeList(), "Name", "Name");
            return View(purchaseInvoice);
        }

        public ActionResult GenerateV3()
        {
            PurchaseInvoiceModel purchaseInvoice = new PurchaseInvoiceModel();
            purchaseInvoice.TransDate = General.FormatDate(DateTime.Now);
            purchaseInvoice.InvoiceDate = purchaseInvoice.TransDate;
            purchaseInvoice.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");

            purchaseInvoice.TransNo = generalBL.GetSerialNo("PurchaseInvoice", "Code");

            purchaseInvoice.Items = new List<GRNTransItemBO>();
            purchaseInvoice.OtherChargeDetails = new List<PurchaseInvoiceOtherChargeDetailBO>();
            purchaseInvoice.TaxDetails = new List<PurchaseInvoiceTaxDetailBO>();
            //purchaseInvoice.GstList = GstBL.GetGstList();
            var obj = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").ToList();
            purchaseInvoice.ShippingStateID = obj.Count > 0 ? obj[0].StateID : 0;
            //purchaseInvoice.GSTPercentageList = new SelectList(gSTCategoryBL.GetGSTList(), "ID", "IGSTPercent");
            ViewBag.TaxPercentages = new SelectList(GstBL.GetGstList(), "IGSTPercentage", "GSTPercentage");
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                purchaseInvoice.CurrencyID = currency.CurrencyID;
                purchaseInvoice.CurrencyName = currency.CurrencyName;
                purchaseInvoice.CurrencyCode = currency.CurrencyCode;
                purchaseInvoice.IsVat = currency.IsVat;
                purchaseInvoice.IsGST = currency.IsGST;
                purchaseInvoice.TaxTypeID = currency.TaxTypeID;
            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                purchaseInvoice.DecimalPlaces = classdata.DecimalPlaces;
                purchaseInvoice.normalclass = classdata.normalclass;
            }
            purchaseInvoice.InvoiceTypeList = new SelectList(purchaseInvoiceBL.GetInvoiceTypeList(), "Name", "Name");
            return View(purchaseInvoice);
        }

        public ActionResult IndexV3()
        {
            ViewBag.Statuses = new List<string>() { "draft", "cancelled" };
            return View();
        }

        public ActionResult GenerateDetailsV3(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            PurchaseInvoiceBO purchaseInvoiceBO = purchaseInvoiceBL.GetPurchaseInvoiceDetails((int)id);
            PurchaseInvoiceModel PurchaseInvoiceModel = new PurchaseInvoiceModel()
            {
                Id = purchaseInvoiceBO.Id,
                TransNo = purchaseInvoiceBO.PurchaseNo,
                TransDate = General.FormatDateNull(purchaseInvoiceBO.PurchaseDate, "view"),
                InvoiceNo = purchaseInvoiceBO.InvoiceNo,
                InvoiceDate = General.FormatDateNull(purchaseInvoiceBO.InvoiceDate, "view"),
                SupplierID = (int)purchaseInvoiceBO.SupplierID,
                SupplierName = purchaseInvoiceBO.SupplierName,
                LocalSupplierName = purchaseInvoiceBO.LocalSupplierName,
                NetAmount = (decimal)purchaseInvoiceBO.NetAmount,
                InvoiceTotal = purchaseInvoiceBO.InvoiceTotal,
                TotalDifference = purchaseInvoiceBO.TotalDifference,

                TDSOnFreight = purchaseInvoiceBO.TDSOnFreight,
                LessTDS = purchaseInvoiceBO.LessTDS,
                SuppDocAmount = purchaseInvoiceBO.SuppDocAmount,
                SuppShipAmount = purchaseInvoiceBO.SuppShipAmount,
                SupplierOtherCharges = purchaseInvoiceBO.SupplierOtherCharges,
                TotalFreight = purchaseInvoiceBO.FreightAmount,
                PackingForwarding = purchaseInvoiceBO.PackingForwarding,
                LocalOtherCharges = purchaseInvoiceBO.LocalOtherCharges,
                LocalMiscCharge = purchaseInvoiceBO.LocalMiscCharge,
                LocalFreight = purchaseInvoiceBO.LocalFreight,
                LocalCustomsDuty = purchaseInvoiceBO.LocalCustomsDuty,
                LocalLandingCost = purchaseInvoiceBO.LocalLandingCost,
                CurrencyExchangeRate = purchaseInvoiceBO.CurrencyExchangeRate,
                CurrencyID = purchaseInvoiceBO.CurrencyID,
                CurrencyCode = purchaseInvoiceBO.CurrencyCode,
                CurrencyName = purchaseInvoiceBO.CurrencyName,
                IsGST = purchaseInvoiceBO.IsGST,
                IsVat = purchaseInvoiceBO.IsVAT,
                Discount = purchaseInvoiceBO.Discount,
                VATAmount = purchaseInvoiceBO.VATAmount,
                VatPercentage = purchaseInvoiceBO.VATPercentage,
                AmountPayable = purchaseInvoiceBO.AmountPayable,
                OtherDeductions = purchaseInvoiceBO.OtherDeductions,
                IsDraft = purchaseInvoiceBO.IsDraft,
                Status = purchaseInvoiceBO.Status,
                Date = General.FormatDate(purchaseInvoiceBO.PurchaseOrderDate, "view"),
                SupplierCode = purchaseInvoiceBO.SupplierCode,
                TDSCode = purchaseInvoiceBO.TDSCode,
                TDSID = purchaseInvoiceBO.TDSID,
                SupplierLocation = purchaseInvoiceBO.SupplierLocation,
                GSTNo = purchaseInvoiceBO.GSTNo,
                IsCancelled = purchaseInvoiceBO.IsCancelled,
                Remarks = purchaseInvoiceBO.Remarks,
                IGSTAmt = purchaseInvoiceBO.IGST,
                SGSTAmt = purchaseInvoiceBO.SGST,
                CGSTAmt = purchaseInvoiceBO.CGST,
                GrossAmount = purchaseInvoiceBO.GrossAmount,
                GRNNo = purchaseInvoiceBO.GrnNo,
               PItype= purchaseInvoiceBO.PItype,
                shipmentmode = purchaseInvoiceBO.shipmentmode,
                Freight = purchaseInvoiceBO.Freight,
                WayBillNo = purchaseInvoiceBO.WayBillNo,
                InvoiceType = purchaseInvoiceBO.InvoiceType,
                suppliercurrencyCode = purchaseInvoiceBO.suppliercurrencyCode,
                OtherChargeDetails = purchaseInvoiceBO.OtherChargeDetails,
                OtherChargesVATAmount = purchaseInvoiceBO.OtherChargesVATAmount,
            };


            PurchaseInvoiceModel.Items = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails((int)id);
            PurchaseInvoiceModel.SelectedQuotation = fileBL.GetAttachments(purchaseInvoiceBO.SelectedQuotationID.ToString());
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(PurchaseInvoiceModel.CurrencyID);
            if (classdata != null)
            {
                PurchaseInvoiceModel.DecimalPlaces = classdata.DecimalPlaces;
                PurchaseInvoiceModel.normalclass = classdata.normalclass;
            }
            return View(PurchaseInvoiceModel);
        }

        public ActionResult GenerateDetailsV4(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            PurchaseInvoiceBO purchaseInvoiceBO = purchaseInvoiceBL.GetPurchaseInvoiceDetails((int)id);
            PurchaseInvoiceModel PurchaseInvoiceModel = new PurchaseInvoiceModel()
            {
                Id = purchaseInvoiceBO.Id,
                TransNo = purchaseInvoiceBO.PurchaseNo,
                TransDate = General.FormatDateNull(purchaseInvoiceBO.PurchaseDate, "view"),
                InvoiceNo = purchaseInvoiceBO.InvoiceNo,
                InvoiceDate = General.FormatDateNull(purchaseInvoiceBO.InvoiceDate, "view"),
                SupplierID = (int)purchaseInvoiceBO.SupplierID,
                SupplierName = purchaseInvoiceBO.SupplierName,
                LocalSupplierName = purchaseInvoiceBO.LocalSupplierName,
                NetAmount = (decimal)purchaseInvoiceBO.NetAmount,
                InvoiceTotal = purchaseInvoiceBO.InvoiceTotal,
                TotalDifference = purchaseInvoiceBO.TotalDifference,
                TotalFreight = purchaseInvoiceBO.FreightAmount,
                TDSOnFreight = purchaseInvoiceBO.TDSOnFreight,
                LessTDS = purchaseInvoiceBO.LessTDS,
                Discount = purchaseInvoiceBO.Discount,
                SupplierOtherCharges = purchaseInvoiceBO.SupplierOtherCharges,
                AmountPayable = purchaseInvoiceBO.AmountPayable,
                OtherDeductions = purchaseInvoiceBO.OtherDeductions,
                IsDraft = purchaseInvoiceBO.IsDraft,
                Status = purchaseInvoiceBO.Status,
                Date = General.FormatDate(purchaseInvoiceBO.PurchaseOrderDate, "view"),
                SupplierCode = purchaseInvoiceBO.SupplierCode,
                TDSCode = purchaseInvoiceBO.TDSCode,
                TDSID = purchaseInvoiceBO.TDSID,
                SupplierLocation = purchaseInvoiceBO.SupplierLocation,
                GSTNo = purchaseInvoiceBO.GSTNo,
                IsCancelled = purchaseInvoiceBO.IsCancelled,
                Remarks = purchaseInvoiceBO.Remarks,
                IGSTAmt = purchaseInvoiceBO.IGST,
                SGSTAmt = purchaseInvoiceBO.SGST,
                CGSTAmt = purchaseInvoiceBO.CGST,
                GrossAmount = purchaseInvoiceBO.GrossAmount,
                GRNNo = purchaseInvoiceBO.GrnNo,
                InvoiceType = purchaseInvoiceBO.InvoiceType,
            };


            PurchaseInvoiceModel.Items = purchaseInvoiceBL.GetPurchaseInvoiceTransDetails((int)id);
            PurchaseInvoiceModel.SelectedQuotation = fileBL.GetAttachments(purchaseInvoiceBO.SelectedQuotationID.ToString());

            return View(PurchaseInvoiceModel);
        }

        public JsonResult PurchaseInvoicePrintPdf(int Id)
        {
            return null;
        }
        public JsonResult PurchaseInvoiceIemCodePrintPdf(int Id)
        {
            return null;
        }
        public JsonResult PurchaseInvoicePartNoPrintPdf(int Id)
        {
            return null;
        }
        public JsonResult PurchaseInvoiceExportIemCodePrintPdf(int Id)
        {
            return null;
        }
        public JsonResult PurchaseInvoiceExportPartNoPrintPdf(int Id)
        {
            return null;
        }
    }
}
