using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class ProformaInvoiceController : Controller
    {
        private IProformaInvoice proformaInvoiceBL;
        private ISalesOrder salesOrderBL;
        private ICustomerContract customerBL;
        private ICategoryContract categroyBL;
        private IAddressContract addressBL;
        private IWareHouseContract warehouseBL;
        private IGeneralContract generalBL;
        private IPaymentModeContract paymentModeBL;
        private ISalesInvoice salesInvoiceBL;
        private IUnitContract unitBL;

        public ProformaInvoiceController()
        {
            proformaInvoiceBL = new ProformaInvoiceBL();
            salesOrderBL = new SalesOrderBL();
            customerBL = new CustomerBL();
            categroyBL = new CategoryBL();
            addressBL = new AddressBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            paymentModeBL = new PaymentModeBL();
            unitBL = new UnitBL();
            salesInvoiceBL = new SalesInvoiceBL();
        }

        // GET: Sales/ProformaInvoice
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Sales/ProformaInvoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            ProformaInvoiceModel proformaInvoice = new ProformaInvoiceModel();
            int ProformaInvoiceID = (int)id;
            proformaInvoice = proformaInvoiceBL.GetProformaInvoice(ProformaInvoiceID).MapToModel();
            proformaInvoice.Items = proformaInvoiceBL.GetProformaInvoiceItems(ProformaInvoiceID).MapToModel();
            proformaInvoice.AmountDetails = proformaInvoiceBL.GetProformaInvoiceAmountDetails(ProformaInvoiceID).MapToModel();
            proformaInvoice.IsCancelable = proformaInvoiceBL.IsCancelable(ProformaInvoiceID);
            proformaInvoice.PackingDetails = proformaInvoiceBL.GetProformaInvoicePackingDetails(ProformaInvoiceID).MapToModel();
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.Contains("Approval"))
            {
                ViewBag.CloseURL = "/Approval";
            }
            return View(proformaInvoice);
        }

        // GET: Sales/ProformaInvoice/Create
        public ActionResult Create()
        {
            ProformaInvoiceModel proformaInvoice = new ProformaInvoiceModel();
            List<CategoryBO> ItemCategoryList = categroyBL.GetItemCategoryForSales();
            int DefaultCategoryID = ItemCategoryList.FirstOrDefault().ID;

            proformaInvoice.InvoiceDate = General.FormatDate(DateTime.Now);
            proformaInvoice.InvoiceNo = generalBL.GetSerialNo("ProformaInvoice", "Code");
            proformaInvoice.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            proformaInvoice.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", proformaInvoice.PaymentModeID);
            proformaInvoice.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            proformaInvoice.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(DefaultCategoryID), "ID", "Name");
            proformaInvoice.StoreList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            proformaInvoice.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            proformaInvoice.CheckStock = true;
            var address = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            proformaInvoice.LocationStateID = address != null ? address.StateID : 0;
            proformaInvoice.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", 0, ""), "AddressID", "Place");
            proformaInvoice.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", 0, ""), "AddressID", "Place");
            proformaInvoice.Items = new List<ProformaInvoiceItemModel>();
            proformaInvoice.AmountDetails = new List<ProformaInvoiceAmount>();
            proformaInvoice.PackingDetails = new List<PackingDetails>();
            proformaInvoice.FreightTax = salesInvoiceBL.GetFreightTaxForEcommerceCustomer();
            proformaInvoice.UnitList = new SelectList(
                                           new List<SelectListItem>
                                           {
                                                new SelectListItem { Text = "", Value = "0"}

                                           }, "Value", "Text");

            ViewBag.Statuses = new List<string>() { "Zero-Quantity", "Insufficient-Order-Quantity", "Insufficient-Offer-Quantity" };
            return View(proformaInvoice);
        }

        // POST: Sales/ProformaInvoice/Create
        [HttpPost]
        public ActionResult Save(ProformaInvoiceModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    ProformaInvoiceBO Temp = proformaInvoiceBL.GetProformaInvoice(model.ID);
                    if (!Temp.IsDraft || Temp.IsCancelled || Temp.IsProcessed)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                ProformaInvoiceBO Invoice = model.MapToBO();
                List<SalesItemBO> Items = model.Items.MapToBO();
                List<SalesAmountBO> AmountDetails = model.AmountDetails != null ? model.AmountDetails.MapToBO() : null;
                List<SalesPackingDetailsBO> PackingDetails = model.PackingDetails.MapToBO();

                if (proformaInvoiceBL.Save(Invoice, Items, AmountDetails, PackingDetails) != 0)
                {
                    return Json(new { Status = "success", Message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (OutofStockException e)
            {
                result.Add(new { ErrorMessage = "Item out of stock" });
                generalBL.LogError("Sales", "ProformaInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (DatabaseException e)
            {
                result.Add(new { ErrorMessage = "Database error" });
                generalBL.LogError("Sales", "ProformaInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (AlreadyCancelledException e)
            {
                result.Add(new { ErrorMessage = "Some sales order are cancelled " });
                generalBL.LogError("Sales", "ProformaInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (QuantityExceededException e)
            {
                result.Add(new { ErrorMessage = "Some items quantity already met" });
                generalBL.LogError("Sales", "ProformaInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "ProformaInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/ProformaInvoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            ProformaInvoiceModel proformaInvoice = new ProformaInvoiceModel();
            int ProformaInvoiceID = (int)id;
            proformaInvoice = proformaInvoiceBL.GetProformaInvoice(ProformaInvoiceID).MapToModel();
            if (!proformaInvoice.IsDraft || proformaInvoice.IsCanceled || proformaInvoice.IsProcessed)
            {
                return RedirectToAction("Index");
            }
            List<CategoryBO> ItemCategoryList = categroyBL.GetItemCategoryForSales();
            int DefaultCategoryID = ItemCategoryList.FirstOrDefault().ID;


            proformaInvoice.FreightTax = salesInvoiceBL.GetFreightTaxForEcommerceCustomer();
            proformaInvoice.Items = proformaInvoiceBL.GetProformaInvoiceItems(ProformaInvoiceID).MapToModel();
            proformaInvoice.AmountDetails = proformaInvoiceBL.GetProformaInvoiceAmountDetails(ProformaInvoiceID).MapToModel();
            proformaInvoice.PackingDetails = proformaInvoiceBL.GetProformaInvoicePackingDetails(ProformaInvoiceID).MapToModel();

            proformaInvoice.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            proformaInvoice.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", proformaInvoice.PaymentModeID);
            //proformaInvoice.SalesTypeList = new SelectList();
            proformaInvoice.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            proformaInvoice.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(DefaultCategoryID), "ID", "Name");
            proformaInvoice.StoreList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            proformaInvoice.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            proformaInvoice.CheckStock = true;
            proformaInvoice.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            proformaInvoice.BatchTypeID = customerBL.GetBatchTypeID(proformaInvoice.CustomerID).ID;
            proformaInvoice.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", proformaInvoice.CustomerID, ""), "AddressID", "Place");
            proformaInvoice.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", proformaInvoice.CustomerID, ""), "AddressID", "Place");
            proformaInvoice.UnitList = new SelectList(
                                       new List<SelectListItem>
                                       {
                                                new SelectListItem { Text = "", Value = "0"}

                                       }, "Value", "Text");

            ViewBag.Statuses = new List<string>() { "Zero-Quantity", "Insufficient-Order-Quantity", "Insufficient-Offer-Quantity", "Insufficient-Stock" };
            return View(proformaInvoice);
        }

        [HttpPost]
        public JsonResult GetProformaInvoiceList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string SalesTypeHint = Datatable.Columns[3].Search.Value;
                string CustomerNameHint = Datatable.Columns[4].Search.Value;
                string LocationHint = Datatable.Columns[5].Search.Value;
                string NetAmountHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = proformaInvoiceBL.GetProformaInvoiceList(CodeHint, DateHint, CustomerNameHint, LocationHint, NetAmountHint, Type, 0, 0, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "ProformaInvoice", "GetProformaInvoiceList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetUnProcessedProformaInvoiceList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string SalesTypeHint = Datatable.Columns[3].Search.Value;
                string CustomerNameHint = Datatable.Columns[4].Search.Value;
                string LocationHint = Datatable.Columns[5].Search.Value;
                string NetAmountHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));

                if (CustomerID == 0)
                {
                    CustomerID = -1; // Preventing from populating Proforma invoices when CustomerID is 0 
                }
                DatatableResultBO resultBO = proformaInvoiceBL.GetProformaInvoiceList(CodeHint, DateHint, CustomerNameHint, LocationHint, NetAmountHint, "UnProcessed", ItemCategoryID, CustomerID, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "ProformaInvoice", "GetUnProcessedProformaInvoiceList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetProformaInvoiceItems(int[] ProformaInvoiceID, int CustomerID, int SchemeID)
        {
            try
            {
                int[] ItemID;
                int[] UnitID;
                List<ProformaInvoiceItemModel> Items = proformaInvoiceBL.GetProformaInvoiceItems(ProformaInvoiceID).MapToModel();
                ItemID = Items.Select(a => a.ItemID).Distinct().ToArray();
                UnitID = Items.Select(a => a.UnitID).Distinct().ToArray();

                List<DiscountAndOfferBO> discountAndOffer = salesOrderBL.GetOfferDetails(CustomerID, SchemeID, ItemID, UnitID);
                return Json(new { Status = "success", Items = Items, discountAndOffer = discountAndOffer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "ProformaInvoice", "GetProformaInvoiceItems", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Cancel(int ProformaInvoiceID)
        {
            try
            {
                if (proformaInvoiceBL.IsCancelable(ProformaInvoiceID))
                {
                    proformaInvoiceBL.Cancel(ProformaInvoiceID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "ProformaInvoice", "Cancel", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemBatchwise(int ItemID, decimal Qty, decimal OfferQty, int StoreID, int CustomerID, int UnitID)
        {
            try
            {
                List<SalesBatchBO> BatchwiseItems = proformaInvoiceBL.GetItemBatchwise(ItemID, Qty, OfferQty, StoreID, CustomerID, UnitID);

                return Json(new
                {
                    Status = "success",
                    Data = new
                    {
                        BatchwiseItems = BatchwiseItems,
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "ProformaInvoice", "GetItemBatchwise", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + proformaInvoiceBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Summary(int ID)
        {
            CustomerCreditSummaryBO Summary = proformaInvoiceBL.GetCustomerCreditSummary(ID);
            return PartialView(Summary);
        }

        public JsonResult ProformaInvoicePrintPdf(int Id)
        {
            return null;
        }

        public ActionResult SaveAsDraft(ProformaInvoiceModel model)
        {
            return Save(model);
        }
    }
}