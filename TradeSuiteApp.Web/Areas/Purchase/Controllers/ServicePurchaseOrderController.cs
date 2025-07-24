using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class ServicePurchaseOrderController : Controller
    {
        private IDropdownContract dropdown;
        private IServicePurchaseOrderContract purchaseOrderBL;
        private IServicePurchaseRequisition servicePurchaseRequisitionBL;
        private IFileContract fileBL;
        private ILocationContract locationBL;
        private IAddressContract addressBL;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private IPlacesContract placesBL;
        private IPaymentModeContract paymentModeBL;
        private IPaymentDaysContract paymentDaysBL;
        private IGeneralContract generalBL;
        private ICategoryContract categoryBL;
        private IConfigurationContract configBL;


        public ServicePurchaseOrderController(IDropdownContract IDropdown)
        {
            fileBL = new FileBL();
            locationBL = new LocationBL();
            addressBL = new AddressBL();
            purchaseOrderBL = new ServicePurchaseOrderBL();
            departmentBL = new DepartmentBL();
            employeeBL = new EmployeeBL();
            dropdown = IDropdown;
            servicePurchaseRequisitionBL = new ServicePurchaseRequisitionBL();
            placesBL = new PlacesBL();
            paymentModeBL = new PaymentModeBL();
            paymentDaysBL = new PaymentDaysBL();
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            configBL = new ConfigurationBL();

        }
        // GET: Purchase/ServicePurchaseOrder
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled", "suspended" };
            return View();
        }

        // GET: Purchase/ServicePurchaseOrder/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            PurchaseOrderBO _po = new PurchaseOrderBO();
            _po = purchaseOrderBL.GetPurchaseOrder(Convert.ToInt32(id));
            _po.IsCancellable = purchaseOrderBL.IsPOSCancellable(Convert.ToInt32(id));
            _po.SelectedQuotation = fileBL.GetAttachments(_po.SelectedQuotationID.ToString());
            _po.OtherQuotations = fileBL.GetAttachments(_po.OtherQuotationIDS);
            _po.InvoiceDateStr = _po.InvoiceDate == null ? "" : General.FormatDate((DateTime)_po.InvoiceDate);
            _po.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", _po.PaymentModeID);
            _po.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name", _po.PaymentWithinID);
            _po.items = purchaseOrderBL.GetPurchaseOrderTransDetails(Convert.ToInt32(id));
            _po.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);

            _po.items = _po.items.Select(a =>
            {
                //a.TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate);
                return a;
            }).ToList();
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.Contains("Approval"))
            {
                ViewBag.CloseURL = "/Approval";
            }
            return View(_po);
        }

        // GET: Purchase/ServicePurchaseOrder/Create
        public ActionResult Create()
        {

            PurchaseOrderBO purchaseOrder = new PurchaseOrderBO();
            purchaseOrder.PurchaseOrderDate = DateTime.Now;
            purchaseOrder.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);
            if (purchaseOrder.IsBranchLocation)
            {
                purchaseOrder.LocationID = GeneralBO.LocationID;
                purchaseOrder.DepartmentID = Convert.ToInt32(generalBL.GetConfig("DefaultDepartmentForBranch"));
            }
            purchaseOrder.PurchaseOrderNo = generalBL.GetSerialNo("PurchaseOrderForService", "Code");
            purchaseOrder.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            purchaseOrder.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name");
            purchaseOrder.DDLItemCategory = new SelectList(dropdown.GetItemCategoryForService(), "ID", "Name");
            purchaseOrder.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(-1), "ID", "Name");

            purchaseOrder.DDLLocation = new SelectList(locationBL.GetLocationListByLocationHead(), "ID", "Name");
            purchaseOrder.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            purchaseOrder.DDLEmployee = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            purchaseOrder.DDLInterCompany = new SelectList(dropdown.GetInterCompanyList(), "ID", "Name");
            purchaseOrder.DDLProject = new SelectList(dropdown.GetProjectList(), "ID", "Name");
            purchaseOrder.UnProcessedPrList = servicePurchaseRequisitionBL.GetUnProcessedPurchaseRequisitionForService();
            purchaseOrder.ShippingAddressList = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            purchaseOrder.BillingAddressList = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            var obj = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.ShippingAddressID = obj.Count > 0 ? obj[0].AddressID : 0;
            purchaseOrder.ShippingStateID = obj.Count > 0 ? obj[0].StateID : 0;
            obj = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.BillingAddressID = obj.Count > 0 ? obj[0].AddressID : 0;

            purchaseOrder.InvoiceDateStr = General.FormatDate(DateTime.Now);
            purchaseOrder.TransportModeList = new SelectList(generalBL.GetModeOfTransport(), "ID", "Name");

            purchaseOrder.TravelFromList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
            purchaseOrder.TravelToList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
            purchaseOrder.CashPaymentLimit = configBL.GetCashPayementLimit();
            //

            return View(purchaseOrder);
        }

        // POST: Purchase/ServicePurchaseOrder/Create
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
                    if (!Temp.IsDraft || Temp.Cancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                List<PurchaseOrderTransBO> TransList = new List<PurchaseOrderTransBO>();

                foreach (var itm in Details)
                {
                    var tranObj = new PurchaseOrderTransBO();

                    tranObj.TravelFromID = itm.TravelFromID;
                    tranObj.TravelToID = itm.TravelToID;
                    tranObj.TransportModeID = itm.TransportModeID;
                    tranObj.ItemID = itm.ItemID;
                    if (itm.TravelDateString != null && itm.TravelDateString != "")
                    {
                        tranObj.TravelDate = General.ToDateTime(itm.TravelDateString);
                    }
                    else
                    {
                        tranObj.TravelDate = null;

                        //}
                        //tranObj.TravelingRemarks = itm.TravelingRemarks;
                        tranObj.Quantity = itm.Quantity;
                        if (!PO.IsDraft && PO.InclusiveGST && PO.IsGSTRegistred)
                        {
                            tranObj.Rate = itm.Rate * 100 / (100 + itm.CGSTPercent + itm.SGSTPercent + itm.IGSTPercent);
                            tranObj.Amount = tranObj.Rate * itm.Quantity;
                        }
                        else
                        {
                            tranObj.Rate = itm.Rate;
                            tranObj.Amount = itm.Amount;
                        }

                        tranObj.SGSTPercent = itm.SGSTPercent;
                        tranObj.CGSTPercent = itm.CGSTPercent;
                        tranObj.IGSTPercent = itm.IGSTPercent;
                        tranObj.SGSTAmt = itm.SGSTAmt;
                        tranObj.CGSTAmt = itm.CGSTAmt;
                        tranObj.IGSTAmt = itm.IGSTAmt;
                        tranObj.NetAmount = itm.NetAmount;
                        tranObj.ServiceLocationID = itm.ServiceLocationID;
                        tranObj.DepartmentID = itm.DepartmentID;
                        tranObj.EmployeeID = itm.EmployeeID;
                        tranObj.CompanyID = itm.CompanyID;
                        tranObj.ProjectID = itm.ProjectID;
                        tranObj.Remarks = itm.Remarks;
                        tranObj.PRTransID = itm.PRTransID;
                        tranObj.PurchaseReqID = itm.PurchaseReqID;
                        TransList.Add(tranObj);
                    }
                }
                    PO.InvoiceDateStr = General.FormatDate(DateTime.Now);
                    if (PO.InvoiceDateStr != null)
                    {
                        PO.InvoiceDate = General.ToDateTime(PO.InvoiceDateStr);
                    }

                    var response = purchaseOrderBL.SavePurchaseOrder(PO, TransList);
                    //if (PO.DirectInvoice)
                    //{
                    //    purchaseOrderBL.CreateAutomaticSRNAndInvoice(response.Data.ID, PO.InvoiceNo, (DateTime)PO.InvoiceDate, (decimal)PO.Discount, (decimal)PO.OtherDeductions);
                    //}
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            catch (Exception ex)
            {
                result.Add(new { ErrorMessage = ex.Message });
                generalBL.LogError("Purchase", "ServicePurchaseOrder", "Save", 0, ex);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        public ActionResult Update(PurchaseOrderBO PO, List<PurchaseOrderTransBO> Details)
        {
            var result = new List<object>();
            try
            {
                if (PO.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PurchaseOrderBO Temp = purchaseOrderBL.GetPurchaseOrder(PO.ID);
                    if (!Temp.IsDraft || Temp.Cancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var _poTrans = Details.Select(x =>
                {
                    x.Purchased = false;
                    if (PO.InclusiveGST && !PO.IsDraft && PO.IsGSTRegistred)
                    {
                        x.Rate = x.Rate * 100 / (100 + x.CGSTPercent + x.SGSTPercent + x.IGSTPercent);
                        x.Amount = x.Rate * x.Quantity;
                    }
                    //if (x.TravelDateString != null && x.TravelDateString != "")
                    //{
                    //    x.TravelDate = General.ToDateTime(x.TravelDateString);
                    //}
                    else
                    {
                        x.TravelDate = null;

                    }
                    return x;
                }).ToList();
                PO.InvoiceDateStr = General.FormatDate(DateTime.Now);
                if (PO.InvoiceDateStr != null)
                {
                    PO.InvoiceDate = General.ToDateTime(PO.InvoiceDateStr);
                }
                var response = purchaseOrderBL.UpdatePurchaseOrder(PO, _poTrans);
                if (PO.DirectInvoice)
                {
                    purchaseOrderBL.CreateAutomaticSRNAndInvoice(response.Data.ID, PO.InvoiceNo, (DateTime)PO.InvoiceDate, (decimal)PO.Discount, (decimal)PO.OtherDeductions);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                result.Add(new { ErrorMessage = ex.Message });
                generalBL.LogError("Purchase", "ServicePurchaseOrder", "Update", 0, ex);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPurchaseRequisitionItems(int[] PurchaseRequisitionID, int SupplierID)
        {
            List<PurchaseOrderTransBO> PurchaseRequisitionItems = new List<PurchaseOrderTransBO>();

            foreach (var ID in PurchaseRequisitionID)
            {
                var obj = purchaseOrderBL.GetUnProcessedPurchaseRequisitionTransForPO(ID, SupplierID);
                obj = obj.Select(a =>
                {
                    //a.TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate);
                    return a;
                }).ToList();
                PurchaseRequisitionItems.AddRange(obj);
            }

            return Json(PurchaseRequisitionItems, JsonRequestBehavior.AllowGet);
        }

        // GET: Purchase/ServicePurchaseOrder/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return View("PageNotFound");
            }

            PurchaseOrderBO purchaseOrder = new PurchaseOrderBO();
            purchaseOrder = purchaseOrderBL.GetPurchaseOrder(Convert.ToInt32(id));
            if (!purchaseOrder.IsDraft || purchaseOrder.Cancelled || purchaseOrder.IsSuspended)
            {
                return RedirectToAction("Index");
            }

            purchaseOrder.SelectedQuotation = fileBL.GetAttachments(purchaseOrder.SelectedQuotationID.ToString());
            purchaseOrder.OtherQuotations = fileBL.GetAttachments(purchaseOrder.OtherQuotationIDS);
            purchaseOrder.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);
            if (purchaseOrder.IsBranchLocation)
            {
                purchaseOrder.LocationID = GeneralBO.LocationID;
                purchaseOrder.DepartmentID = Convert.ToInt32(generalBL.GetConfig("DefaultDepartmentForBranch"));
            }
            purchaseOrder.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", purchaseOrder.PaymentModeID);
            purchaseOrder.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name", purchaseOrder.PaymentWithinID);
            purchaseOrder.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);
            purchaseOrder.DDLItemCategory = new SelectList(dropdown.GetItemCategoryForService(), "ID", "Name");
            purchaseOrder.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(-1), "ID", "Name");
            purchaseOrder.DDLLocation = new SelectList(locationBL.GetLocationListByLocationHead(), "ID", "Name");
            purchaseOrder.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            purchaseOrder.DDLEmployee = new SelectList(new List<SelectListItem>
            { }, "Value", "Text");
            purchaseOrder.DDLInterCompany = new SelectList(dropdown.GetInterCompanyList(), "ID", "Name");
            purchaseOrder.DDLProject = new SelectList(dropdown.GetProjectList(), "ID", "Name");
            purchaseOrder.InvoiceDateStr = purchaseOrder.InvoiceDate == null ? "" : General.FormatDate((DateTime)purchaseOrder.InvoiceDate);
            purchaseOrder.ShippingAddressList = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            purchaseOrder.BillingAddressList = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            var obj = addressBL.GetShippingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.ShippingAddressID = obj[0].AddressID;
            purchaseOrder.ShippingStateID = obj[0].StateID;
            obj = addressBL.GetBillingAddressLocation(GeneralBO.LocationID);
            obj = obj.Where(d => GeneralBO.LocationID.Equals(d.LocationID)).ToList();
            purchaseOrder.BillingAddressID = obj[0].AddressID;
            purchaseOrder.TransportModeList = new SelectList(generalBL.GetModeOfTransport(), "ID", "Name");
            purchaseOrder.TravelFromList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
            purchaseOrder.TravelToList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");

            purchaseOrder.items = purchaseOrderBL.GetPurchaseOrderTransDetails(Convert.ToInt32(id));
            purchaseOrder.items = purchaseOrder.items.Select(a =>
            {
                //a.TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate);
                return a;
            }).ToList();
            purchaseOrder.UnProcessedPrList = servicePurchaseRequisitionBL.GetUnProcessedPurchaseRequisitionForService();
            purchaseOrder.IsClone = false;
            purchaseOrder.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);
            purchaseOrder.CashPaymentLimit = configBL.GetCashPayementLimit();
            return View(purchaseOrder);


        }

        [HttpPost]
        public ActionResult Cancel(int ServicePurchaseOrderID)
        {
            try
            {
                if (purchaseOrderBL.IsPOSCancellable(ServicePurchaseOrderID))
                {
                    purchaseOrderBL.CancelServicePurchaseOrder(ServicePurchaseOrderID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                generalBL.LogError("Purchase", "ServicePurchaseOrder", "Cancel", ServicePurchaseOrderID, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Suspend(int ID, string Table)
        {
            var output = purchaseOrderBL.SuspendPurchaseOrder(ID, Table);
            return Json(new { Status = "success", Data = output }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Clone(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            PurchaseOrderBO purchaseOrder = new PurchaseOrderBO();
            purchaseOrder = purchaseOrderBL.GetPurchaseOrder(Convert.ToInt32(id));

            purchaseOrder.ID = 0;
            purchaseOrder.PurchaseOrderNo = generalBL.GetSerialNo("PurchaseOrderForService", "Code");
            purchaseOrder.SelectedQuotation = fileBL.GetAttachments(purchaseOrder.SelectedQuotationID.ToString());
            purchaseOrder.OtherQuotations = fileBL.GetAttachments(purchaseOrder.OtherQuotationIDS);
            purchaseOrder.DDLPaymentMode = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", purchaseOrder.PaymentModeID);
            purchaseOrder.PaymentDaysList = new SelectList(paymentDaysBL.GetPaymentDaysList(), "ID", "Name", purchaseOrder.PaymentWithinID);
            purchaseOrder.DDLItemCategory = new SelectList(dropdown.GetItemCategoryForService(), "ID", "Name");
            purchaseOrder.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(-1), "ID", "Name");
            purchaseOrder.DDLLocation = new SelectList(locationBL.GetLocationList(), "ID", "Place");
            purchaseOrder.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            purchaseOrder.DDLEmployee = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            purchaseOrder.DDLInterCompany = new SelectList(dropdown.GetInterCompanyList(), "ID", "Name");
            purchaseOrder.DDLProject = new SelectList(dropdown.GetProjectList(), "ID", "Name");
            purchaseOrder.ShippingAddressList = addressBL.GetShippingAddress("Location", null, "");
            purchaseOrder.BillingAddressList = addressBL.GetBillingAddress("Location", null, "");
            purchaseOrder.TransportModeList = new SelectList(generalBL.GetModeOfTransport(), "ID", "Name");
            purchaseOrder.TravelFromList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
            purchaseOrder.TravelToList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
            purchaseOrder.items = purchaseOrderBL.GetPurchaseOrderTransDetails(Convert.ToInt32(id));
            decimal GSTPercentage = 0;
            purchaseOrder.items = purchaseOrder.items.Select(a =>
            {
                a.PRTransID = 0;
                a.PurchaseReqID = 0;
                //a.TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate);
                GSTPercentage = (decimal)a.IGSTAmt == 0 ? (decimal)(a.CGSTPercent + a.SGSTPercent) : (decimal)a.IGSTPercent;
                if (!purchaseOrder.IsDraft && purchaseOrder.InclusiveGST && purchaseOrder.IsGSTRegistred)
                {

                    a.Rate = a.Rate + a.Rate * (GSTPercentage) / 100;
                    a.Amount = a.Rate * a.Quantity;
                }
                return a;
            }).ToList();
            purchaseOrder.UnProcessedPrList = servicePurchaseRequisitionBL.GetUnProcessedPurchaseRequisitionForService();
            purchaseOrder.IsClone = true;
            purchaseOrder.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);
            purchaseOrder.PurchaseOrderDate = DateTime.Now;
            purchaseOrder.InvoiceDateStr = General.FormatDate(DateTime.Now);
            purchaseOrder.InvoiceNo = "";
            purchaseOrder.Discount = 0; ;
            purchaseOrder.OtherDeductions = 0;
            return View(purchaseOrder);

        }

        public JsonResult GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            var count = purchaseOrderBL.GetInvoiceNumberCount(Hint, Table, SupplierID);
            return Json(new { Status = "success", data = count }, JsonRequestBehavior.AllowGet);
        }

        //for role privileges 
        public ActionResult approve()
        {
            return null;
        }
        //public ActionResult SaveAsDraft(PurchaseOrderBO PO, List<PurchaseOrderTransBO> Details)
        //{
        //    return Save(PO, Details);
        //}

        [HttpPost]
        public JsonResult GetPurchaseOrderList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SupplierNameHint = Datatable.Columns[3].Search.Value;
                string CategoryNameHint = Datatable.Columns[5].Search.Value;
                string ItemNameHint = Datatable.Columns[6].Search.Value;
                string NetAmtHint = Datatable.Columns[4].Search.Value;

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
                generalBL.LogError("Purchase", "ServicePurchaseOrder", "GetPurchaseOrderList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult Summary(int ID)
        {
            return PartialView();
        }

        public JsonResult ServicePurchaseOrderPrintPdf(int Id)
        {
            return null;
        }

    }
}
