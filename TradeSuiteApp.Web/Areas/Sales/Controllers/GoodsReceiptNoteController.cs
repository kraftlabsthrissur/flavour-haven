using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
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
    public class GoodsReceiptNoteController : Controller
    {
        private ISalesGoodsReceiptNote salesGoodsReceiptNoteBL;
        private ISalesOrder salesOrderBL;
        private ICustomerContract customerBL;
        private ICategoryContract categroyBL;
        private IAddressContract addressBL;
        private IWareHouseContract warehouseBL;
        private IGeneralContract generalBL;
        private IPaymentModeContract paymentModeBL;
        private ISalesInvoice salesInvoiceBL;
        private IUnitContract unitBL;
        private ILocationContract locationBL;
        private ICounterSalesContract counterSalesBL;

        public GoodsReceiptNoteController()
        {
            salesGoodsReceiptNoteBL = new SalesGoodsReceiptNoteBL();
            salesOrderBL = new SalesOrderBL();
            customerBL = new CustomerBL();
            categroyBL = new CategoryBL();
            addressBL = new AddressBL();
            warehouseBL = new WarehouseBL();
            generalBL = new GeneralBL();
            paymentModeBL = new PaymentModeBL();
            unitBL = new UnitBL();
            salesInvoiceBL = new SalesInvoiceBL();
            locationBL = new LocationBL();
            counterSalesBL = new CounterSalesBL();
        }

        // GET: Sales/GoodsReceiptNote
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Sales/GoodsReceiptNote/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            GoodsReceiptModel GoodsReceiptNote = new GoodsReceiptModel();
            int GoodReceiptNoteID = (int)id;
            GoodsReceiptNote = salesGoodsReceiptNoteBL.GetGoodReceiptNote(GoodReceiptNoteID).MapToModel();
            GoodsReceiptNote.Items = salesGoodsReceiptNoteBL.GetGoodReceiptNoteItem(GoodReceiptNoteID).MapToModel();
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();


            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                GoodsReceiptNote.normalclass = classdata.normalclass;
                GoodsReceiptNote.DecimalPlaces = classdata.DecimalPlaces;
            }
            return View(GoodsReceiptNote);
        }

        // GET: Sales/GoodsReceiptNote/Create
        public ActionResult Create()
        {
            GoodsReceiptModel GoodsReceiptNote = new GoodsReceiptModel();
            List<CategoryBO> ItemCategoryList = categroyBL.GetItemCategoryForSales();
            int DefaultCategoryID = ItemCategoryList.FirstOrDefault().ID;

            GoodsReceiptNote.TransDate = General.FormatDate(DateTime.Now);
            GoodsReceiptNote.TransNo = generalBL.GetSerialNo("SalesGoodsReceiptNote", "Code");
            GoodsReceiptNote.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            GoodsReceiptNote.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", GoodsReceiptNote.PaymentModeID);
            GoodsReceiptNote.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            GoodsReceiptNote.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(DefaultCategoryID), "ID", "Name");
            GoodsReceiptNote.StoreList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            GoodsReceiptNote.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            GoodsReceiptNote.CheckStock = true;
            var address = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            GoodsReceiptNote.LocationStateID = address != null ? address.StateID : 0;
            GoodsReceiptNote.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", 0, ""), "AddressID", "Place");
            GoodsReceiptNote.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", 0, ""), "AddressID", "Place");
            GoodsReceiptNote.Items = new List<GoodsReceiptItemModel>();
            GoodsReceiptNote.UnitList = new SelectList(
                                           new List<SelectListItem>
                                           {
                                                new SelectListItem { Text = "", Value = "0"}

                                           }, "Value", "Text");

            ViewBag.Statuses = new List<string>() { "Zero-Quantity", "Insufficient-Order-Quantity", "Insufficient-Offer-Quantity" };
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                GoodsReceiptNote.normalclass = classdata.normalclass;
                GoodsReceiptNote.DecimalPlaces = classdata.DecimalPlaces;
            }
            return View(GoodsReceiptNote);
        }

        // POST: Sales/GoodsReceiptNote/Create
        [HttpPost]
        public ActionResult Save(GoodsReceiptModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    SalesGoodsReceiptBO Temp = salesGoodsReceiptNoteBL.GetGoodReceiptNote(model.ID);
                    if (!Temp.IsDraft || Temp.IsCancelled || Temp.IsProcessed)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                SalesGoodsReceiptBO Invoice = model.MapToBO();
                List<SalesGoodsReceiptItemBO> Items = model.Items.MapToBO();
                if (salesGoodsReceiptNoteBL.Save(Invoice, Items) != 0)
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
                generalBL.LogError("Sales", "GoodsReceiptNote", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (DatabaseException e)
            {
                result.Add(new { ErrorMessage = "Database error" });
                generalBL.LogError("Sales", "GoodsReceiptNote", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (AlreadyCancelledException e)
            {
                result.Add(new { ErrorMessage = "Some sales order are cancelled " });
                generalBL.LogError("Sales", "GoodsReceiptNote", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (QuantityExceededException e)
            {
                result.Add(new { ErrorMessage = "Some items quantity already met" });
                generalBL.LogError("Sales", "GoodsReceiptNote", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "GoodsReceiptNote", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/GoodsReceiptNote/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            GoodsReceiptModel GoodsReceiptNote = new GoodsReceiptModel();
            int GoodReceiptNoteID = (int)id;
            GoodsReceiptNote = salesGoodsReceiptNoteBL.GetGoodReceiptNote(GoodReceiptNoteID).MapToModel();
            if (!GoodsReceiptNote.IsDraft || GoodsReceiptNote.IsCanceled || GoodsReceiptNote.IsProcessed)
            {
                return RedirectToAction("Index");
            }
            List<CategoryBO> ItemCategoryList = categroyBL.GetItemCategoryForSales();
            int DefaultCategoryID = ItemCategoryList.FirstOrDefault().ID;

            GoodsReceiptNote.Items = salesGoodsReceiptNoteBL.GetGoodReceiptNoteItem(GoodReceiptNoteID).MapToModel();

            GoodsReceiptNote.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            GoodsReceiptNote.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", GoodsReceiptNote.PaymentModeID);
            GoodsReceiptNote.ItemCategoryList = new SelectList(categroyBL.GetItemCategoryForSales(), "ID", "Name");
            GoodsReceiptNote.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(DefaultCategoryID), "ID", "Name");
            GoodsReceiptNote.StoreList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            GoodsReceiptNote.StoreID = Convert.ToInt16(generalBL.GetConfig("DefaultStore", GeneralBO.CreatedUserID));
            GoodsReceiptNote.CheckStock = true;
            GoodsReceiptNote.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            GoodsReceiptNote.BatchTypeID = customerBL.GetBatchTypeID(GoodsReceiptNote.CustomerID).ID;
            GoodsReceiptNote.BillingAddressList = new SelectList(addressBL.GetBillingAddress("Customer", GoodsReceiptNote.CustomerID, ""), "AddressID", "Place");
            GoodsReceiptNote.ShippingAddressList = new SelectList(addressBL.GetShippingAddress("Customer", GoodsReceiptNote.CustomerID, ""), "AddressID", "Place");
            GoodsReceiptNote.UnitList = new SelectList(
                                       new List<SelectListItem>
                                       {
                                                new SelectListItem { Text = "", Value = "0"}

                                       }, "Value", "Text");

            var classdata = GoodsReceiptNote.Items != null && GoodsReceiptNote.Items.Count > 0 ? counterSalesBL.GetCurrencyDecimalClassByCurrencyID(GoodsReceiptNote.Items.First().CurrencyID) : null;
            if (classdata != null)
            {
                GoodsReceiptNote.normalclass = classdata.normalclass;
                GoodsReceiptNote.DecimalPlaces = classdata.DecimalPlaces;
            }
            //ViewBag.Statuses = new List<string>() { "Zero-Quantity", "Insufficient-Order-Quantity", "Insufficient-Offer-Quantity", "Insufficient-Stock" };
            return View(GoodsReceiptNote);
        }

        [HttpPost]
        public JsonResult GetGoodReceiptNoteList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string DateHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string NetAmountHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string ReceiptType = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = salesGoodsReceiptNoteBL.GetGoodReceiptNoteList(CodeHint, DateHint, CustomerNameHint, NetAmountHint, ReceiptType, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "GoodsReceiptNote", "GetGoodsReceiptNoteList", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public JsonResult GetGoodReceiptNoteItems(int[] GoodsReceiptNoteID, int CustomerID, int SchemeID)
        //{
        //    try
        //    {
        //        int[] ItemID;
        //        int[] UnitID;
        //        List<GoodsReceiptNoteItemModel> Items = salesGoodsReceiptNoteBL.GetGoodReceiptNoteItems(GoodsReceiptNoteID).MapToModel();
        //        ItemID = Items.Select(a => a.ItemID).Distinct().ToArray();
        //        UnitID = Items.Select(a => a.UnitID).Distinct().ToArray();

        //        List<DiscountAndOfferBO> discountAndOffer = salesOrderBL.GetOfferDetails(CustomerID, SchemeID, ItemID, UnitID);
        //        return Json(new { Status = "success", Items = Items, discountAndOffer = discountAndOffer }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        var res = new List<object>();
        //        generalBL.LogError("Sales", "GoodsReceiptNote", "GetGoodsReceiptNoteItems", 0, e);
        //        return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult Cancel(int GoodReceiptNoteID)
        {
            try
            {
                if (salesGoodsReceiptNoteBL.IsCancelable(GoodReceiptNoteID))
                {
                    salesGoodsReceiptNoteBL.Cancel(GoodReceiptNoteID);
                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "GoodsReceiptNote", "Cancel", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Print(int id)
        {
            string URL = "";// Request.Url.GetLeftPart(UriPartial.Authority) + salesGoodsReceiptNoteBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAsDraft(GoodsReceiptModel model)
        {
            return Save(model);
        }
        public JsonResult GoodsReceiptNotePrintPdf(int Id)
        {
            return null;
        }
        public JsonResult GoodsReceiptNoteExportPrintPdf(int Id)
        {
            return null;
        }
    }
}