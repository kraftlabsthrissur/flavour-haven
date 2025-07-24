using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{

    public class ServicePurchaseInvoiceController : Controller
    {
        private IDropdownContract _dropdown;
        private IServicePurchaseInvoice servicePurchaseInvoiceBL;
        private ISupplierContract supplierBL;
        private IGSTContract GstBL;
        private IAddressContract addressBL;
        private IGeneralContract generalBL;
        #region Constructor

        public ServicePurchaseInvoiceController(IDropdownContract iDropdownContract)
        {
            _dropdown = iDropdownContract;
            servicePurchaseInvoiceBL = new ServicePurchaseInvoiceBL();
            supplierBL = new SupplierBL();
            GstBL = new GSTBL();
            addressBL = new AddressBL();
            generalBL = new GeneralBL();
        }

        #endregion

        // GET: Purchase/ServicePurchaseInvoice
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "cancelled" };
            return View();
        }

        // GET: Purchase/ServicePurchaseInvoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ServicePurchaseInvoiceBO servicePurchaseInvoiceBO = servicePurchaseInvoiceBL.GetPurchaseInvoice((int)id);

            if (servicePurchaseInvoiceBO == null)
            {
                return RedirectToAction("Index");
            }

            servicePurchaseInvoiceBO.InvoiceTransItems = servicePurchaseInvoiceBL.GetServicePurchaseInvoiceTransItems((int)id);
            servicePurchaseInvoiceBO.TaxDetails = servicePurchaseInvoiceBL.GetServicePurchaseInvoiceTaxDetails((int)id);
            servicePurchaseInvoiceBO.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");

            // servicePurchaseInvoiceBO.TransDate=General.FormatDate((DateTime)servicePurchaseInvoiceBO.TransDate)
            servicePurchaseInvoiceBO.InvoiceTransItems = servicePurchaseInvoiceBO.InvoiceTransItems.Select(a =>
           {
               a.TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate);

               return a;
           }).ToList();
              SetSalesPurchaseInvoiceViewbagItems();
            return View(servicePurchaseInvoiceBO);
        }

        // GET: Purchase/ServicePurchaseInvoice/Create
        #region Create
        public ActionResult Create()
        {
            ServicePurchaseInvoiceBO servicePurchaseInvoiceBO = new ServicePurchaseInvoiceBO();
            servicePurchaseInvoiceBO.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");
            servicePurchaseInvoiceBO.PurchaseNo = generalBL.GetSerialNo("PurchaseInvoiceForService", "Code");
            servicePurchaseInvoiceBO.TransDate = DateTime.Now;
            servicePurchaseInvoiceBO.ShippingStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            servicePurchaseInvoiceBO.InvoiceTransItems = null;
         
            SetSalesPurchaseInvoiceViewbagItems();
            return View(servicePurchaseInvoiceBO);
        }

        private void SetSalesPurchaseInvoiceViewbagItems()
        {
            ViewBag.Projects = servicePurchaseInvoiceBL.GetProjects();
            ViewBag.Companies = servicePurchaseInvoiceBL.GetCompanies();
            ViewBag.Departments = servicePurchaseInvoiceBL.GetDepartments();
            ViewBag.Locations = servicePurchaseInvoiceBL.GetLocations();
            ViewBag.Employees = servicePurchaseInvoiceBL.GetEmployees();
        }

        /// <summary>
        /// Get SRNTable partial view.
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        //[OutputCache (Duration =30, VaryByParam = "supplierID")]          //UnComment after testing
        public PartialViewResult GetUnProcessedSRNBySupplier(int supplierID)
        {
            // supplierID = 32;        //Testing purpose
            List<SRNBO> srnBOList;
            if (supplierID > 0)
            {
                int finYear = GeneralBO.FinYear;         //Hardcoded. Need to change
                int locationID = GeneralBO.LocationID;         //Hardcoded. Need to change
                int applicationID = GeneralBO.ApplicationID;      //Hardcoded. Need to change

                srnBOList = servicePurchaseInvoiceBL.GetUnProcessedSRNBySupplier(supplierID);
            }
            else
                srnBOList = new List<SRNBO>();
            return PartialView("~/Areas/Purchase/Views/ServicePurchaseInvoice/_srnList.cshtml", srnBOList);
        }

        /// <summary>
        /// Once the SRN selected, get the Items, OtherCharges, TaxDetails view as string
        /// </summary>
        /// <returns></returns>
        //[OutputCache (Duration =30, VaryByParam = "srnIDs")]           //UnComment after testing
        public PartialViewResult GetUnProcessedSRNItemsView(int[] srnArr)
        {
            List<ServicePurchaseInvoiceItemModel> srnTransItemList = new List<ServicePurchaseInvoiceItemModel>();
            if (srnArr != null && srnArr.Count() > 0)
            {
                foreach (var srnID in srnArr)
                {
                    var list = servicePurchaseInvoiceBL.GetUnProcessedSRNItems(srnID).Select(a => new ServicePurchaseInvoiceItemModel()
                    {
                        SRNID = a.SRNID,
                        SRNTransID = a.SRNTransID,
                        IsInclusiveGST = a.IsInclusiveGST,
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        ServiceLocationID = a.ServiceLocationID,
                        DepartmentID = a.DepartmentID,
                        EmployeeID = a.EmployeeID,
                        ProjectID = a.ProjectID,
                        CompanyID = a.CompanyID,
                        Unit = a.Unit,
                        AcceptedQty = a.AcceptedQty,
                        AcceptedValue = a.AcceptedValue,
                        UnMatchedQty = a.UnMatchedQty,
                        ApprovedValue = a.ApprovedValue,
                        InvoiceQty = a.InvoiceQty,
                        InvoiceRate = a.InvoiceRate,
                        InvoiceValue = a.InvoiceValue,
                        Remarks = a.Remarks,
                        PORate = a.PORate,
                        POServiceID = a.POServiceID,
                        SGSTPercent = (decimal)a.SGSTPercent,
                        IGSTPercent = (decimal)a.IGSTPercent,
                        CGSTPercent = (decimal)a.CGSTPercent,
                        TransportMode = a.TransportMode,
                        TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate, "view"),
                        TravelFrom = a.TravelFrom,
                        TravelTo = a.TravelTo,
                        TravelingRemarks = a.TravelingRemarks,
                        CategoryID = a.CategoryID,




                    }).ToList();
                    if (list != null)
                    {
                        List<decimal> availPercentages = new List<decimal>();

                        SelectList temp = new SelectList(GetAvailGSTPercentages(availPercentages), "Value", "Text");
                        ViewBag.TaxPercentages = temp;
                        srnTransItemList.AddRange(list);
                    }
                }
            }
            ViewBag.Projects = servicePurchaseInvoiceBL.GetProjects();
            ViewBag.Companies = servicePurchaseInvoiceBL.GetCompanies();
            ViewBag.Departments = servicePurchaseInvoiceBL.GetDepartments();
            ViewBag.Locations = servicePurchaseInvoiceBL.GetLocations();
            ViewBag.Employees = servicePurchaseInvoiceBL.GetEmployees();
            return PartialView("~/Areas/Purchase/Views/ServicePurchaseInvoice/_unProcessedSRNTrans.cshtml", srnTransItemList);

        }


        public JsonResult GetTDsAmountForUnProcessedSRNItems(string IDs)
        {
            decimal TDSOnAdvance = servicePurchaseInvoiceBL.GetTDsAmountForUnProcessedSRNItems(IDs);
            return Json(new { Status = "success", data = TDSOnAdvance }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the tax details view string based on the selected Items
        /// </summary>
        /// <param name="grnIDs"></param>
        /// <param name="itemDetailViewModelList"></param>
        /// <returns></returns>
        public JsonResult GetTaxDetailsView(int supplierID, SRNItemViewModel srnItems)
        {
            List<ItemViewModelService> itemDetails = srnItems.ItemList;
            List<int> srnArr = srnItems.SrnIDList;

            int finYear = GeneralBO.FinYear;         //Hardcoded. Need to change
            int locationID = GeneralBO.LocationID;         //Hardcoded. Need to change

            decimal freightCharge = decimal.Zero, otherCharge = decimal.Zero, packingCharge = decimal.Zero;

            var taxDetailViewString = string.Empty;
            var otherDeductionViewString = string.Empty;

            if (supplierID > 0 && srnArr != null && srnArr.Count() > 0 && itemDetails != null && itemDetails.Count() > 0)
            {

                var distinctPOServiceIDList = itemDetails.Select(x => x.POServiceID).Distinct().ToList();
                List<PurchaseOrderTransBOService> purchaseOrderTransBOList = null;

                List<decimal> availPercentages = new List<decimal>();

                bool isLocal = true;

                foreach (var poID in distinctPOServiceIDList)
                {

                    var purchaseOrderTrans = servicePurchaseInvoiceBL.GetPurchaseOrderTransDetails_Service(poID);

                    if (purchaseOrderTrans != null)
                    {
                        if (purchaseOrderTransBOList == null)
                            purchaseOrderTransBOList = new List<PurchaseOrderTransBOService>();
                        purchaseOrderTransBOList.AddRange(purchaseOrderTrans);
                    }
                }

                if (purchaseOrderTransBOList != null)
                    isLocal = purchaseOrderTransBOList.Where(x => x.CGSTPercent > 0 || x.SGSTPercent > 0).Count() > 0;

                var groupedItems = GetGroupedItems(purchaseOrderTransBOList, isLocal, out availPercentages);

                var availGSTPercentages = GetAvailGSTPercentages(availPercentages);

                var viewBagItems = new List<KeyValuePair<string, dynamic>>()
                                        {
                                            new KeyValuePair<string, dynamic>("IsLocal", isLocal),
                                            new KeyValuePair<string, dynamic>("AvailablePercentages", availGSTPercentages)
                                        };

                taxDetailViewString = RenderPartialViewToString(this, "~/Areas/Purchase/Views/ServicePurchaseInvoice/_taxDetails.cshtml", groupedItems, viewBagItems);
                //otherDeductionViewString = RenderPartialViewToString(this, "~/Areas/Purchase/Views/PurchaseInvoice/_otherDeductions.cshtml", purchaseOrderOtherDeductionViewModelList);
            }


            return Json(
                new
                {
                    FreightCharge = freightCharge,
                    OtherCharge = otherCharge,
                    PackingCharge = packingCharge,
                    TaxDetailViewStr = taxDetailViewString
                }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Group the Items by GST based on location
        /// </summary>
        /// <param name="purchaseOrderTransBOList"></param>
        /// <param name="isLocal"></param>
        /// <param name="availPercentages"></param>
        /// <returns></returns>
        [NonAction]
        private List<PurchaseOrderTransBOService> GetGroupedItems(List<PurchaseOrderTransBOService> purchaseOrderTransBOList, bool isLocal, out List<decimal> availPercentages)
        {
            availPercentages = new List<decimal>();
            if (isLocal)
            {
                availPercentages.AddRange(purchaseOrderTransBOList.Where(x => x.CGSTPercent > 0).Select(x => x.CGSTPercent).ToList());
                availPercentages.AddRange(purchaseOrderTransBOList.Where(x => x.SGSTPercent > 0).Select(x => x.SGSTPercent).ToList());

                availPercentages = availPercentages.Distinct().ToList();

                return (from potl in purchaseOrderTransBOList
                        group potl by new { potl.CGSTPercent, potl.SGSTPercent } into grpd
                        select new PurchaseOrderTransBOService()
                        {
                            CGSTPercent = grpd.Key.CGSTPercent,
                            CGSTAmt = grpd.Sum(x => x.CGSTAmt), //(grpd.Key.CGSTPercent ?? 0 * grpd.Count()),
                            SGSTPercent = grpd.Key.SGSTPercent, //grpd.Key.SGSTPercent ?? 0,
                            SGSTAmt = grpd.Sum(x => x.SGSTAmt),
                            Count = grpd.Count(),
                            //PurchaseOrderTransBO = grpd.First()
                        }).ToList();

            }
            else
            {
                availPercentages.AddRange(purchaseOrderTransBOList.Where(x => x.IGSTPercent > 0).Select(x => x.IGSTPercent).Distinct().ToList());
                return purchaseOrderTransBOList.GroupBy(info => info.IGSTPercent)
                 .Select(group => new PurchaseOrderTransBOService()
                 {
                     IGSTPercent = group.Key,
                     //PurchaseOrderTransBO = group.First(),
                     IGSTAmt = group.Sum(x => x.IGSTAmt),  //(group.Key ?? 0 * group.Count()),
                     Count = group.Count()
                 })
                 .OrderBy(x => x.IGSTPercent).ToList();

            }
        }

        /// <summary>
        /// Get available GST Percentages from list
        /// </summary>
        /// <param name="availPercentages"></param>
        /// <returns></returns>
        [NonAction]
        private List<SelectListItem> GetAvailGSTPercentages(List<decimal> availPercentages = null)
        {
            var listItem = new List<SelectListItem>();

            listItem.AddRange(new List<SelectListItem>() {
                new SelectListItem() { Text = "0", Value = "0" } ,
                new SelectListItem() { Text = "3", Value = "3" },
                new SelectListItem() { Text = "5", Value = "5" } ,
                new SelectListItem() { Text = "12", Value = "12" } ,
                new SelectListItem() { Text = "18", Value = "18" },
                new SelectListItem() { Text = "28", Value = "28" }
            });

            if (availPercentages != null)           //Some taxes from availPercentages is not in the above added list. 
                listItem.AddRange(availPercentages.Where(x => !listItem.Select(l => l.Value).Contains(x.ToString())).Select(x => new SelectListItem() { Text = x.ToString(), Value = x.ToString() }));

            return listItem != null ? listItem.OrderBy(x => decimal.Parse(x.Value)).ToList() : new List<SelectListItem>();
        }

        [HttpPost]
        public ActionResult Save(ServicePurchaseInvoiceModel servicePurchaseInvoice)
        {
            var rslt = new List<object>();
            try
            {
                if (servicePurchaseInvoice.ServicePurchaseInvoiceID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    ServicePurchaseInvoiceBO Temp = servicePurchaseInvoiceBL.GetPurchaseInvoice(servicePurchaseInvoice.ServicePurchaseInvoiceID);
                    if (!Temp.IsDraft || Temp.IsCanceled)
                    {
                        rslt.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = rslt }, JsonRequestBehavior.AllowGet);
                    }
                }
                ServicePurchaseInvoiceBO PurchaseInvoiceBO = new ServicePurchaseInvoiceBO()
                {
                    AcceptedAmount = servicePurchaseInvoice.AcceptedAmount,
                    IsDraft = servicePurchaseInvoice.IsDraft,
                    CGSTAmount = servicePurchaseInvoice.CGSTAmount,
                    DifferenceAmount = servicePurchaseInvoice.DifferenceAmount,
                    IGSTAmount = servicePurchaseInvoice.IGSTAmount,
                    InvoiceAmount = servicePurchaseInvoice.InvoiceAmount,
                    InvoiceDate = General.ToDateTime(servicePurchaseInvoice.InvoiceDate),
                    InvoiceNo = servicePurchaseInvoice.InvoiceNo,
                    InvoiceTransItems = servicePurchaseInvoice.InvoiceTransItems,
                    LocalSupplierName = servicePurchaseInvoice.LocalSupplierName,
                    NetAmountPayable = servicePurchaseInvoice.NetAmountPayable,
                    NetTDS = servicePurchaseInvoice.NetTDS,
                    PurchaseDate = General.ToDateTime(servicePurchaseInvoice.PurchaseDate),
                    PurchaseNo = servicePurchaseInvoice.PurchaseNo,
                    SGSTAmount = servicePurchaseInvoice.SGSTAmount,
                    ServicePurchaseInvoiceID = servicePurchaseInvoice.ServicePurchaseInvoiceID,
                    SupplierID = servicePurchaseInvoice.SupplierID,
                    TDS = servicePurchaseInvoice.TDS,
                    TDSOnAdvance = servicePurchaseInvoice.TDSOnAdvance,
                    TaxDetails = servicePurchaseInvoice.TaxDetails,
                    Status = servicePurchaseInvoice.Status,
                    TDSID = servicePurchaseInvoice.TDSID,
                    Discount = servicePurchaseInvoice.Discount,
                    OtherDeductions = servicePurchaseInvoice.OtherDeductions
                };
          
                var result = servicePurchaseInvoiceBL.Save(PurchaseInvoiceBO);
                return Json(new { Status = "success" });
            }
            catch (DuplicateEntryException e)
            {
                rslt.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "ServicePurchaseInvoice", "Save", 0, e);
                return Json(new { Status = "failure", data = rslt }, JsonRequestBehavior.AllowGet);
            }
            catch (QuantityExceededException e)
            {
                rslt.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "ServicePurchaseInvoice", "Save", 0, e);
                return Json(new { Status = "failure", data = rslt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                rslt.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "ServicePurchaseInvoice", "Save", 0, e);
                return Json(new { Status = "failure", data = rslt }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion



        // GET: Purchase/ServicePurchaseInvoice/Edit/5
        /// <summary>
        /// Edit Service Purchase Invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ServicePurchaseInvoiceBO servicePurchaseInvoiceBO = servicePurchaseInvoiceBL.GetPurchaseInvoice((int)id);
            if(!servicePurchaseInvoiceBO.IsDraft || servicePurchaseInvoiceBO.IsCanceled)
            {
                return RedirectToAction("Index");
            }
            List<SRNBO> srnBOList;

            if (servicePurchaseInvoiceBO == null)
            {
                return RedirectToAction("Index");
            }

            if (!servicePurchaseInvoiceBO.IsDraft || servicePurchaseInvoiceBO.IsCanceled)
            {
                return RedirectToAction("Index");
            }
            servicePurchaseInvoiceBO.InvoiceTransItems = servicePurchaseInvoiceBL.GetServicePurchaseInvoiceTransItems((int)id);
            servicePurchaseInvoiceBO.TaxDetails = servicePurchaseInvoiceBL.GetServicePurchaseInvoiceTaxDetails((int)id);
            servicePurchaseInvoiceBO.TDSCodeList = new SelectList(_dropdown.GetTDS(), "Rate", "Description");           
            servicePurchaseInvoiceBO.InvoiceTransItems = servicePurchaseInvoiceBO.InvoiceTransItems.Select(a =>
            {
                a.TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate);

                return a;
            }).ToList();
           
            servicePurchaseInvoiceBO.SrDate = General.FormatDate((DateTime)servicePurchaseInvoiceBO.PurchaseOrderDate);

            //  servicePurchaseInvoiceBO.SrDate = General.FormatDate((DateTime)servicePurchaseInvoiceBO.SrDate, "view");
            SetSalesPurchaseInvoiceViewbagItems();
            servicePurchaseInvoiceBO.GstList = GstBL.GetGstList();

            servicePurchaseInvoiceBO.ShippingStateID = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;

            List<decimal> availPercentages = new List<decimal>();
            SelectList temp = new SelectList(GetAvailGSTPercentages(availPercentages), "Value", "Text");
            ViewBag.TaxPercentages = temp;
            srnBOList = servicePurchaseInvoiceBL.GetUnProcessedSRNBySupplier(servicePurchaseInvoiceBO.SupplierID);

            return View(servicePurchaseInvoiceBO);

        }


        public ActionResult Approve(int ID, String Status)
        {
            try
            {
                servicePurchaseInvoiceBL.Approve(ID, Status);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Cancel(int ID, string Table)
        {
            servicePurchaseInvoiceBL.Cancel(ID, Table);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            var count = servicePurchaseInvoiceBL.GetInvoiceNumberCount(Hint, Table, SupplierID);
            return Json(new { Status = "success", data = count }, JsonRequestBehavior.AllowGet);
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

                DatatableResultBO resultBO = servicePurchaseInvoiceBL.GetPurchaseInvoiceList(Type, TransNoHint, TransDateHint, InvoiceNoHint, InvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(ServicePurchaseInvoiceModel servicePurchaseInvoice)
        {
            return Save(servicePurchaseInvoice);
        }
    }
}
