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
    public class PurchaseReturnOrderController : Controller
    {
        private ISupplierContract supplierBL;
        private IGoodsReceiptNoteContract grnBL;
        private IPremisesContract premisesBL;
        private IPurchaseReturnOrderContract purchaseReturnBL;
        private IGeneralContract generalBL;
        private IGSTCategoryContract gSTCategoryBL;
        private IAddressContract addressBL;
        private ICounterSalesContract counterSalesBL;
        // GET: Purchase/PurchaseReturn
        public PurchaseReturnOrderController()
        {
            supplierBL = new SupplierBL();
            grnBL = new GoodsReceiptNoteBL();
            premisesBL = new PremisesBL();
            purchaseReturnBL = new PurchaseReturnOrderBL();
            generalBL = new GeneralBL();
            gSTCategoryBL = new GSTCategoryBL();
            addressBL = new AddressBL();
            counterSalesBL = new CounterSalesBL();
        }

        public ActionResult Index()
        {

            return View();
        }

        // GET: Purchase/PurchaseReturn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            PurchaseReturnModel purchaseReturn = purchaseReturnBL.GetPurchaseReturnDetail((int)id).Select(m => new PurchaseReturnModel()
            {
                ID = m.Id,
                ReturnNo = m.ReturnNo,
                ReturnDate = General.FormatDate(m.ReturnDate, "view"),
                SupplierName = m.SupplierName,
                ItemName = m.ItemName,
                GRNno = m.GRNno,
                PremisesName = m.PremisesName,
                ReturnQty = m.ReturnQty,
                Freight = m.Freight,
                PackingCharges = m.PackingCharges,
                OtherCharges = m.OtherCharges,
                GrossAmount = m.GrossAmount,
                NetAmount = m.NetAmount,
                IsDraft = m.IsDraft,
                Discount = m.Discount,
                SGSTAmount = m.SGSTAmount,
                CGSTAmount = m.CGSTAmount,
                IGSTAmount = m.IGSTAmount
            }).First();
            List<GRNTransItemBO> purchaseReturnItem = purchaseReturnBL.GetPurchaseReturnTransList((int)id);
            purchaseReturn.Items = purchaseReturnBL.GetPurchaseReturnTransList((int)id).Select(a => new GRNTransItemModel()
            {
                ItemCode = a.ItemCode,
                ItemName = a.ItemName,
                PartsNumber = a.PartsNumber,
                Remark = a.Remark,
                Model = a.Model,
                GrnNo = a.GrnNo,
                CurrencyID = a.CurrencyID,
                UnitID = a.UnitID,
                Unit = a.Unit,
                AcceptedQty = (decimal)a.AcceptedQty,
                ReturnQty = a.ReturnQty,
                Rate = a.Rate,
                SGSTAmt = a.SGSTAmt,
                SGSTPercent = a.SGSTPercent,
                IGSTAmt = a.IGSTAmt,
                IGSTPercent = a.IGSTPercent,
                CGSTAmt = a.CGSTAmt,
                CGSTPercent = a.CGSTPercent,
                GrossAmount = a.GrossAmount,
                Amount = a.Amount,
                Remarks = a.Remarks,
                InvoiceNo = a.InvoiceNo,
                InvoiceQty = a.InvoiceQty,
                SecondaryInvoiceQty = a.InvoiceQty / a.SecondaryUnitSize,
                InvoiceTransID = a.InvoiceTransID,
                PurchaseNo = a.PurchaseNo,
                GSTPercent = a.GSTPercentage,
                GSTAmount = a.GSTAmount,
                GSTID = a.GSTID,
                Discount = a.Discount,
                OfferQty = a.OfferQty,
                OfferReturnQty = a.OfferReturnQty,
                SecondaryReturnQty = a.SecondaryReturnQty,
                SecondaryRate = a.SecondaryRate,
                SecondaryUnit = a.SecondaryUnit,
                SecondaryUnitSize = a.SecondaryUnitSize,
                VATPercentage = a.VATPercentage,
                VATAmount = a.VATAmount,
            }).ToList();
            if (purchaseReturnItem.Count > 0)
            {
                var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(purchaseReturnItem.First().CurrencyID);
                if (classdata != null)
                {
                    purchaseReturn.DecimalPlaces = classdata.DecimalPlaces;
                    purchaseReturn.normalclass = classdata.normalclass;
                }
            }
            return View(purchaseReturn);
        }

        // GET: Purchase/PurchaseReturn/Create
        public ActionResult Create()
        {
            PurchaseReturnBO purchaseReturnBO = new PurchaseReturnBO();
            PurchaseReturnModel PurchaseReturnModel = new PurchaseReturnModel();
            PurchaseReturnModel.ReturnNo = generalBL.GetSerialNo("PurchaseReturn", "Code");
            PurchaseReturnModel.ReturnDate = General.FormatDate(DateTime.Now);
            PurchaseReturnModel.Items = new List<GRNTransItemModel>();

            var obj = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "").ToList();
            PurchaseReturnModel.ShippingStateID = obj.Count > 0 ? obj[0].StateID : 0;
            return View(PurchaseReturnModel);
        }
        // POST: Purchase/PurchaseReturn/Create
        [HttpPost]
        public ActionResult Create(PurchaseReturnModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PurchaseReturnBO Temp = purchaseReturnBL.GetPurchaseReturnDetail(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                // TODO: Add insert logic here
                PurchaseReturnBO purchaseReturn = new PurchaseReturnBO();
                purchaseReturn.Id = model.ID;
                purchaseReturn.ReturnNo = model.ReturnNo;
                purchaseReturn.ReturnDate = General.ToDateTime(model.ReturnDate);
                purchaseReturn.SupplierID = model.SupplierID;
                purchaseReturn.SGSTPercent = model.SGSTPercent;
                purchaseReturn.IGSTPercent = model.IGSTPercent;
                purchaseReturn.CGSTPercent = model.CGSTPercent;
                purchaseReturn.SGSTAmount = model.SGSTAmount;
                purchaseReturn.CGSTAmount = model.CGSTAmount;
                purchaseReturn.IGSTAmount = model.IGSTAmount;
                purchaseReturn.Discount = model.Discount;
                purchaseReturn.CurrencyExchangeRate = model.CurrencyExchangeRate;
                purchaseReturn.CurrencyID = model.CurrencyID;
                purchaseReturn.Freight = model.Freight;
                purchaseReturn.OtherCharges = model.OtherCharges;
                purchaseReturn.PackingCharges = model.PackingCharges;
                purchaseReturn.NetAmount = model.NetAmount;
                purchaseReturn.IsDraft = model.IsDraft;
                purchaseReturn.ReturnQty = model.ReturnQty;

                var ItemList = new List<GRNTransItemBO>();
                GRNTransItemBO grnTransBO;
                foreach (var itm in model.Items)
                {
                    grnTransBO = new GRNTransItemBO();
                    grnTransBO.ItemID = itm.ItemID;
                    grnTransBO.SGSTAmt = itm.SGSTAmt;
                    grnTransBO.CGSTAmt = itm.CGSTAmt;
                    grnTransBO.IGSTAmt = itm.IGSTAmt;
                    grnTransBO.SGSTPercent = itm.SGSTPercent;
                    grnTransBO.IGSTPercent = itm.IGSTPercent;
                    grnTransBO.CGSTPercent = itm.CGSTPercent;
                    grnTransBO.InvoiceID = itm.InvoiceID;
                    grnTransBO.Quantity = itm.Quantity;
                    grnTransBO.OfferQty = itm.OfferQty;
                    grnTransBO.Rate = itm.Rate;
                    grnTransBO.Amount = itm.Amount;
                    grnTransBO.Remarks = itm.Remarks;
                    grnTransBO.WarehouseID = itm.WarehouseID;
                    grnTransBO.BatchTypeID = itm.BatchTypeID;
                    grnTransBO.UnitID = itm.UnitID;
                    grnTransBO.SecondaryUnitSize = itm.SecondaryUnitSize;
                    grnTransBO.SecondaryUnit = itm.SecondaryUnit;
                    grnTransBO.SecondaryReturnQty = itm.SecondaryReturnQty;
                    grnTransBO.SecondaryRate = itm.SecondaryRate;
                    grnTransBO.InvoiceTransID = itm.InvoiceTransID;
                    grnTransBO.GSTPercentage = itm.GSTPercent;
                    grnTransBO.GSTAmount = itm.GSTAmount;
                    grnTransBO.Discount = itm.Discount;
                    grnTransBO.DiscountPercentage = itm.DiscountPercentage;
                    grnTransBO.VATPercentage = itm.VATPercentage;
                    grnTransBO.VATAmount = itm.VATAmount;
                    ItemList.Add(grnTransBO);
                }
                if (model.ID > 0)
                {
                    var outId = purchaseReturnBL.UpdatePurchaseOrderReturn(purchaseReturn, ItemList); ;
                }
                else
                {
                    var outId = purchaseReturnBL.SavePurchaseReturn(purchaseReturn, ItemList); ;
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "PurchaseReturnOrder", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(PurchaseReturnModel model)
        {
            return Create(model);
        }

        public JsonResult GetPurchaseReturnOrderList(int SupplierID)
        {
            List<PurchaseReturnModel> returnorder = new List<PurchaseReturnModel>();

            returnorder = purchaseReturnBL.GetPurchaseReturnOrderList(SupplierID).Select(m => new PurchaseReturnModel()
            {
                ReturnNo = m.ReturnNo,
                ReturnDate = General.FormatDate(m.ReturnDate, "view"),
                NetAmount = m.NetAmount,
                ID = m.Id
            }).ToList();

            return Json(returnorder, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPurchaseReturnOrder(int[] OrderIdS)

        {
            try
            {
                PurchaseReturnBO purchaseInvoice = new PurchaseReturnBO();
                purchaseInvoice.PurchaseReturnTrnasItemBOList = new List<PurchaseReturnTransItemBO>();

                if (OrderIdS.Length > 0)
                {
                    foreach (var OrderID in OrderIdS)
                    {
                        var list = purchaseReturnBL.GetPurchaseReturnOrder(OrderID);

                        if (list != null)
                        {

                            purchaseInvoice.PurchaseReturnTrnasItemBOList.AddRange(list);
                        }
                    }
                }

                return Json(purchaseInvoice.PurchaseReturnTrnasItemBOList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // GET: Purchase/PurchaseReturnOrder/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            PurchaseReturnModel purchaseReturn = purchaseReturnBL.GetPurchaseReturnDetail((int)id).Select(m => new PurchaseReturnModel()
            {

                ID = m.Id,
                ReturnNo = m.ReturnNo,
                ReturnDate = General.FormatDate(m.ReturnDate),
                SupplierName = m.SupplierName,
                SupplierID = m.SupplierID,
                ItemName = m.ItemName,
                GRNno = m.GRNno,
                PremisesName = m.PremisesName,
                ReturnQty = m.ReturnQty,
                Freight = m.Freight,
                PackingCharges = m.PackingCharges,
                OtherCharges = m.OtherCharges,
                NetAmount = m.NetAmount,
                Discount = m.Discount,
                SGSTAmount = m.SGSTAmount,
                CGSTAmount = m.CGSTAmount,
                IGSTAmount = m.IGSTAmount,
                StateID = m.StateID,
                IsGSTRegistred = m.IsGSTRegistred

            }).First();
            List<GRNTransItemBO> purchaseReturnItem = purchaseReturnBL.GetPurchaseReturnTransList((int)id);
            purchaseReturn.Items = purchaseReturnBL.GetPurchaseReturnTransList((int)id).Select(a => new GRNTransItemModel()
            {

                ItemName = a.ItemName,
                ItemID = a.ItemID,
                GrnNo = a.GrnNo,
                Unit = a.Unit,
                UnitID = a.UnitID,
                AcceptedQty = (decimal)a.AcceptedQty,
                ReturnQty = a.ReturnQty,
                Rate = a.Rate,
                SGSTAmt = a.SGSTAmt,
                SGSTPercent = a.SGSTPercent,
                IGSTAmt = a.IGSTAmt,
                IGSTPercent = a.IGSTPercent,
                CGSTAmt = a.CGSTAmt,
                CGSTPercent = a.CGSTPercent,
                Discount = a.Discount,
                UOMList = new SelectList(
                                        new List<SelectListItem>
                                        {
                                                new SelectListItem { Text = a.PurchaseUnit, Value =a.PurchaseUnitID.ToString()},
                                                new SelectListItem { Text = a.PrimaryUnit, Value =a.PrimaryUnitID.ToString()},
                                        }, "Value", "Text"),
                Amount = a.Amount,
                Remarks = a.Remarks,
                InvoiceNo = a.InvoiceNo,
                InvoiceID = a.InvoiceID,
                Stock = a.Stock,
                WarehouseID = a.WarehouseID,
                ConvertedQty = a.ConvertedQty,
                ConvertedStock = a.ConvertedStock,
                PrimaryUnitID = a.PrimaryUnitID,
                PurchaseUnitID = a.PurchaseUnitID,
                InvoiceQty = a.InvoiceQty,
                InvoiceTransID = a.InvoiceTransID,
                PurchaseNo = a.PurchaseNo,
                GSTPercent = a.GSTPercentage,
                GSTAmount = a.GSTAmount,
                GSTPercentageList = new SelectList(gSTCategoryBL.GetGSTList(), "ID", "IGSTPercent"),
                GSTID = a.GSTID,
                OfferQty = a.OfferQty,
                OfferReturnQty = a.OfferReturnQty,
                VATPercentage = a.VATPercentage,
                VATAmount = a.VATAmount,
            }).ToList();

            var obj = addressBL.GetShippingAddress("Location", GeneralBO.LocationID, "");
            purchaseReturn.ShippingStateID = obj[0].StateID;
            return View(purchaseReturn);

        }

        public JsonResult GetPurchaseReturnOderListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value;
                string SupplierName = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = purchaseReturnBL.GetPurchaseReturnOderListForDataTable(Type, TransNo, TransDate, SupplierName, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Cancel(int id)
        {
            return null;
        }

        public JsonResult PurchaseReturnPrintPdf(int Id)
        {
            return null;
        }

    }
}
