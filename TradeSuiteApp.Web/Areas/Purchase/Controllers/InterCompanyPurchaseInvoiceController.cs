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

    public class InterCompanyPurchaseInvoiceController : Controller
    {
        private IPaymentModeContract paymentModeBL;
        private IInterCompanyPurchaseInvoiceContract interCompanyBL;
        private IGeneralContract generalBL;
        public InterCompanyPurchaseInvoiceController()
        {
            paymentModeBL = new PaymentModeBL();
            interCompanyBL = new InterCompanyPurchaseInvoiceBL();
            generalBL = new GeneralBL();
        }
        // GET: Purchase/InterCompanyPurchaseInvoice
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "cancelled" };

            return View();
        }

        // GET: Purchase/InterCompanyPurchaseInvoice/Details/5
        public ActionResult Details(int id)
        {
            InterCompanyPurchaseInvoiceBO invoice = interCompanyBL.GetPurchaseInvoiceDetails(id);
            invoice.Trans = interCompanyBL.GetPurchaseInvoiceTrans(id);
            invoice.AmountDetails = interCompanyBL.GetPurchaseInvoiceTaxDetails(id);

            return View(invoice);
        }

        // GET: Purchase/InterCompanyPurchaseInvoice/Create
        public ActionResult Create()
        {
            InterCompanyPurchaseInvoiceModel purchaseInvoice = new InterCompanyPurchaseInvoiceModel();
            purchaseInvoice.Trans = new List<InterCompanyPurchaseInvoiceItemModel>();
            purchaseInvoice.TaxDetails = new List<SalesAmount>();
            purchaseInvoice.PurchaseNo = generalBL.GetSerialNo("InterCompanyPurchaseInvoice", "Code");
            purchaseInvoice.PurchaseDate = General.FormatDate(DateTime.Now);
            return View(purchaseInvoice);
        }

        // POST: Purchase/InterCompanyPurchaseInvoice/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Purchase/InterCompanyPurchaseInvoice/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Purchase/InterCompanyPurchaseInvoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Save(InterCompanyPurchaseInvoiceModel model)
        {
            try
            {
                // TODO: Add insert logic here
                InterCompanyPurchaseInvoiceBO invoice = new InterCompanyPurchaseInvoiceBO();
                invoice.SupplierID = model.SupplierID;
                invoice.InvoiceNo = model.InvoiceNo;
                invoice.InvoiceDate = General.ToDateTime(model.PurchaseDate);
                invoice.GrossAmount = model.GrossAmount;
                invoice.SGSTAmount = model.SGSTAmount;
                invoice.CGSTAmount = model.CGSTAmount;
                invoice.IGSTAmount = model.IGSTAmount;
                invoice.Discount = model.Discount;
                invoice.TurnoverDiscountAvailable = model.TurnoverDiscount;
                invoice.CashDiscountEnabled = model.CashDiscountEnabled;
                invoice.CashDiscountPercentage = model.CashDiscountPercentage;
                invoice.CashDiscount = model.CashDiscount;
                invoice.TurnoverDiscount = model.TurnoverDiscount;
                invoice.DiscountAmount = model.DiscountAmount;
                invoice.AdditionalDiscount = model.AdditionalDiscount;
                invoice.TaxableAmount = model.TaxableAmount;
                invoice.RoundOff = model.RoundOff;
                invoice.SalesInvoiceNo = model.SalesInvoiceNo;
                invoice.SalesInvoiceDate = General.ToDateTime(model.SalesInvoiceDate);
                invoice.SalesInvoiceID = model.SalesInvoiceID;
                invoice.ShippingAddressID = model.ShippingAddressID;
                invoice.BillingAddressID = model.BillingAddressID;
                invoice.TotalDifference = model.TotalDifference;
                invoice.IsDraft = false;
                invoice.TradeDiscount = model.TradeDiscount;
                invoice.NetAmount = model.NetAmount;
                invoice.PaymentModeID = model.PaymentModeID;
                invoice.CashDiscountEnabled = model.CashDiscountEnabled;
                invoice.NetAmount = model.NetAmount;
                var ItemList = new List<InterCompanyPurchaseInvoiceItemBO>();
                InterCompanyPurchaseInvoiceItemBO TransBO;
                foreach (var iti in model.Trans)
                {
                    TransBO = new InterCompanyPurchaseInvoiceItemBO();
                    TransBO.ItemID = iti.ItemID;
                    TransBO.InvoiceQty = iti.InvoiceQty;
                    TransBO.InvoiceRate = iti.InvoiceRate;
                    TransBO.InvoiceValue = iti.InvoiceValue;
                    TransBO.AcceptedQty = iti.AcceptedQty;
                    TransBO.ApprovedQty = iti.ApprovedQty;
                    TransBO.ApprovedValue = iti.ApprovedValue;
                    TransBO.PORate = iti.PORate;
                    TransBO.Difference = iti.Difference;
                    TransBO.Remarks = iti.Remarks;
                    TransBO.UnMatchedQty = iti.UnMatchedQty;
                    TransBO.CGSTPercent = iti.CGSTPercent;
                    TransBO.IGSTPercent = iti.IGSTPercent;
                    TransBO.SGSTPercent = iti.SGSTPercent;
                    TransBO.InvoiceGSTPercent = iti.InvoiceGSTPercent;
                    TransBO.UnitID = iti.UnitID;
                    TransBO.BasicPrice = iti.BasicPrice;
                    TransBO.GrossAmount = iti.GrossAmount;
                    TransBO.DiscountAmount = iti.DiscountAmount;
                    TransBO.DiscountPercentage = iti.DiscountPercentage;
                    TransBO.AdditionalDiscount = iti.AdditionalDiscount;
                    TransBO.TaxableAmount = iti.TaxableAmount;
                    TransBO.GSTAmount = iti.GSTAmount;
                    TransBO.DiscPercentage = iti.DiscountPercentage;
                    TransBO.GSTPercentage = iti.DiscountPercentage;
                    TransBO.TradeDiscPercentage = iti.TradeDiscPercentage;
                    TransBO.TradeDiscAmount = iti.TradeDiscPercentage;
                    TransBO.TurnoverDiscount = iti.TurnoverDiscount;
                    TransBO.CashDiscount = iti.CashDiscount;
                    TransBO.BatchID = iti.BatchID;
                    TransBO.Batch = iti.Batch;
                    TransBO.POTransID = iti.POTransID;
                    TransBO.PurchaseOrderID = iti.PurchaseOrderID;
                    TransBO.ApprovedValue = iti.ApprovedValue;
                    TransBO.InvoiceValue = iti.InvoiceValue;
                    TransBO.Difference = iti.Difference;
                    TransBO.GSTPercentage = (decimal)(iti.CGSTPercent + iti.SGSTPercent + iti.IGSTPercent);
                    TransBO.SalesInvoiceID = iti.SalesInvoiceID;
                    TransBO.SalesInvoiceTransID = iti.SalesInvoiceTransID;
                    TransBO.TradeDiscAmount = iti.TradeDiscAmount;
                    TransBO.NetAmount = iti.NetAmount;
                    TransBO.GrossAmount = iti.GrossAmount;
                    TransBO.IGSTAmt = iti.IGSTAmt;
                    TransBO.CGSTAmt = iti.CGSTAmt;
                    TransBO.SGSTAmt = iti.SGSTAmt;
                    ItemList.Add(TransBO);
                }
                List<SalesAmountBO> SalesAmountDetails = new List<SalesAmountBO>();
                SalesAmountBO SalesAmount;
                foreach (var item in model.TaxDetails)
                {
                    SalesAmount = new SalesAmountBO()
                    {
                        Particulars = item.Particulars,
                        Amount = item.Amount,
                        Percentage = item.Percentage
                    };
                    SalesAmountDetails.Add(SalesAmount);
                }
                var outId = interCompanyBL.SaveInvoice(invoice, ItemList, SalesAmountDetails);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Purchase", "InterCompanyPurchaseInvoice","Save",model.Id, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InterCompanyPurchaseInvoicePrintPdf(int Id)
        {
            return null;
        }

        public JsonResult GetInterCompanyList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SalesInvoiceNoHint = Datatable.Columns[3].Search.Value;
                string SalesInvoiceDateHint = Datatable.Columns[4].Search.Value;
                string SupplierNameHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;


                DatatableResultBO resultBO = interCompanyBL.GetInterCompanyList(TransNoHint, TransDateHint, SalesInvoiceNoHint, SalesInvoiceDateHint, SupplierNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Purchase", "InterCompanyPurchaseInvoice", "GetInterCompanyList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
