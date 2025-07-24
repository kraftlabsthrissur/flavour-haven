using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class PurchaseReturnProcessingController : Controller
    {
        private IGeneralContract generalBL;
        private IAddressContract addressBL;
        private IPurchaseReturnProcessingContract returnProcessingBL;

        public PurchaseReturnProcessingController ()
        {
            generalBL = new GeneralBL();
            addressBL = new AddressBL();
            returnProcessingBL = new PurchaseReturnProcessingBL();
        }

        // GET: Purchase/ReturnProcessing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            PurchaseReturnProcessingModel model = new PurchaseReturnProcessingModel();
            model.TransNo = generalBL.GetSerialNo("PurchaseReturn", "Code");
            model.TransDate = General.FormatDate(DateTime.Now);
            var obj = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "");
            model.ProcessingTypeList=  new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Non Moving Items", Value ="NonMovingItems", },
                                                 new SelectListItem { Text = "Slow Moving Items", Value ="SlowMovingItems", },
                                                 new SelectListItem { Text = "Near To Expiring", Value = "NearToExpiring"},
                                                 new SelectListItem { Text = "Expired", Value = "Expired"},
                                                 }, "Value", "Text");
            model.LoationStateID = obj[0].StateID;
            return View(model);
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public PartialViewResult PurchaseReturnProcessingItem(string ProcessingType, string FromDate="", string ToDate="",string AsOnDate="",int Days=0)
        {
            if (ProcessingType == "NearToExpiring")
            {
                FromDate = General.FormatDate(DateTime.Now);
                ToDate = General.FormatDate(DateTime.Now);
                
            }
            else if(ProcessingType == "Expired" || ProcessingType == "SlowMovingItems") {
                FromDate = General.FormatDate(DateTime.Now);
                ToDate = General.FormatDate(DateTime.Now);
                AsOnDate = General.FormatDate(DateTime.Now);
            }
            else
            {
                AsOnDate = General.FormatDate(DateTime.Now);
            }

            DateTime fromTransactionDate = General.ToDateTime(FromDate);
            DateTime toTransactionDate = General.ToDateTime(ToDate);
            DateTime asOnDate = General.ToDateTime(AsOnDate);
            PurchaseReturnProcessingModel model = new PurchaseReturnProcessingModel();
            model.Items = returnProcessingBL.GetPurchaseReturnProcessingItem(ProcessingType, fromTransactionDate, toTransactionDate, asOnDate, Days).Select(a => new PurchaseReturnProcessingItemModel()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                BatchID = a.BatchID,
                BatchNo = a.BatchNo,
                IsGSTRegistered = a.IsGSTRegistered,
                Supplier = a.Supplier,
                SupplierID = a.SupplierID,
                InvoiceNo = a.InvoiceNo,
                InvoiceID = a.InvoiceID,
                InvoiceDate = (a.InvoiceDate.ToString("dd-MMM-yyyy")),//General.FormatDateTime( a.InvoiceDate,"view"),
                InvoiceQty = a.InvoiceQty,
                OfferQty = a.OfferQty,
                Qty = a.Qty,
                PurchaseRate = a.PurchaseRate,
                Stock = a.Stock,
                SupplierStateID = a.SupplierStateID,
                ExpiryDate = General.FormatDate(a.ExpiryDate,"view"),
                IGSTPercentage = a.IGSTPercentage,
                SGSTPercentage = a.SGSTPercentage,
                CGSTPercentage = a.CGSTPercentage,
                UnitID = a.UnitID,
                InvoiceTransID=a.InvoiceTransID,
                ReturnQty=a.Qty,
                Value=(a.Qty*a.PurchaseRate)+(a.CGSTAmount+a.SGSTAmount+a.IGSTAmount),
                CGSTAmount=a.CGSTAmount,
                SGSTAmount=a.SGSTAmount,
                IGSTAmount=a.IGSTAmount,
                GSTAmount=(a.CGSTAmount + a.SGSTAmount + a.IGSTAmount),
                NoOFDaysInventoryHeld=a.NoOFDaysInventoryHeld,
                Unit=a.Unit
            }).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Save(PurchaseReturnProcessingModel model)
        {
            var result = new List<object>();
            try
            {
                // TODO: Add insert logic here
                PurchaseReturnProcessingBO purchaseReturn = new PurchaseReturnProcessingBO();

                List<PurchaseReturnProcessingItemBO> Items = new List<PurchaseReturnProcessingItemBO>();
                if (model.Items != null)
                {
                    PurchaseReturnProcessingItemBO PurchaseReturnItem;
                    foreach (var item in model.Items)
                    {
                        PurchaseReturnItem = new PurchaseReturnProcessingItemBO()
                        {
                            ItemID = item.ItemID,
                            ItemName = item.ItemName,
                            BatchID = item.BatchID,
                            BatchNo = item.BatchNo,
                            IsGSTRegistered = item.IsGSTRegistered,
                            Supplier = item.Supplier,
                            SupplierID = item.SupplierID,
                            InvoiceID = item.InvoiceID,
                            InvoiceQty = item.InvoiceQty,
                            OfferQty = item.OfferQty,
                            Qty = item.Qty,
                            ReturnQty = item.ReturnQty,
                            Stock = item.Stock,
                            IGSTPercentage = item.IGSTPercentage,
                            SGSTPercentage = item.SGSTPercentage,
                            CGSTPercentage = item.CGSTPercentage,
                            IGSTAmount=item.IGSTAmount,
                            CGSTAmount = item.CGSTAmount,
                            SGSTAmount = item.SGSTAmount,
                            UnitID = item.UnitID,
                            InvoiceTransID=item.InvoiceTransID,
                            PurchaseRate=item.PurchaseRate,
                            Value=item.Value
                        };

                        Items.Add(PurchaseReturnItem);
                    }
                }
                 returnProcessingBL.Save(Items);

                return Json(new { Status = "Success", data = result }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseReturnProcessing", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}