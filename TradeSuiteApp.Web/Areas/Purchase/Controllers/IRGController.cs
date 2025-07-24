using BusinessLayer;
using BusinessObject;
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
    public class IRGController : Controller
    {
        private ISupplierContract supplierBL;
        private IGoodsReceiptNoteContract grnBL;
        private IPremisesContract premisesBL;
        private IIRGContract iRGBL;
        private IGeneralContract generalBL;

        public IRGController()
        {
            supplierBL = new SupplierBL();
            grnBL = new GoodsReceiptNoteBL();
            premisesBL = new PremisesBL();
            iRGBL = new IRGBL();
            generalBL = new GeneralBL();
        }
        // GET: Purchase/PurchaseReturn
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Purchase/PurchaseReturn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            PurchaseReturnModel purchaseReturn = iRGBL.GetIRGDetail((int)id).Select(m => new PurchaseReturnModel()
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
                NetAmount = m.NetAmount,
                IsDraft = m.IsDraft

            }).First();
            List<PurchaseReturnTransItemBO> purchaseReturnItem = iRGBL.GetIRGTransList((int)id);
            purchaseReturn.ReturnItems = iRGBL.GetIRGTransList((int)id).Select(a => new PurchaseReturnTransModel()
            {

                ItemName = a.ItemName,
                Unit = a.Unit,
                AcceptedQty = (decimal)a.AcceptedQty,
                Quantity = a.Quantity,
                Rate = a.Rate,
                SGSTAmount = a.SGSTAmount,
                SGSTPercent = a.SGSTPercent,
                IGSTAmount = a.IGSTAmount,
                IGSTPercent = a.IGSTPercent,
                CGSTAmount = a.CGSTAmount,
                CGSTPercent = a.CGSTPercent,
                Amount = a.Amount,
                Remarks = a.Remarks,
                ReturnNo = a.ReturnNo,
                ItemID = a.ItemID,
                UnitID = a.UnitID,
                InvoiceID = a.InvoiceID,
                PurchaseReturnTransID = a.PurchaseReturnTransID,
                PurchaseReturnID = a.PurchaseReturnID,
                PurchaseReturnOrderID = a.PurchaseReturnOrderID,
                PurchaseReturnOrderTransID = a.PurchaseReturnOrderTransID,
                InvoiceQty = a.InvoiceQty
            }).ToList();
            return View(purchaseReturn);
        }

        // GET: Purchase/PurchaseReturn/Create
        public ActionResult Create()
        {
            PurchaseReturnBO purchaseReturnBO = new PurchaseReturnBO();
            PurchaseReturnModel PurchaseReturnModel = new PurchaseReturnModel();
            PurchaseReturnModel.ReturnNo = generalBL.GetSerialNo("IRG", "Code");
            PurchaseReturnModel.ReturnDate = General.FormatDate(DateTime.Now);
            PurchaseReturnModel.ReturnItems = new List<PurchaseReturnTransModel>();

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
                    PurchaseReturnBO Temp = iRGBL.GetIRGDetail(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                // TODO: Add insert logic here
                PurchaseReturnBO purchaseReturn = new PurchaseReturnBO();
                purchaseReturn.ReturnNo = model.ReturnNo;
                purchaseReturn.ReturnDate = General.ToDateTime(model.ReturnDate);
                purchaseReturn.SupplierID = model.SupplierID;
                purchaseReturn.SGSTPercent = model.SGSTPercent;
                purchaseReturn.IGSTPercent = model.IGSTPercent;
                purchaseReturn.CGSTPercent = model.CGSTPercent;
                purchaseReturn.SGSTAmount = model.SGSTAmount;
                purchaseReturn.CGSTAmount = model.CGSTAmount;
                purchaseReturn.IGSTAmount = model.IGSTAmount;
                purchaseReturn.Freight = model.Freight;
                purchaseReturn.OtherCharges = model.OtherCharges;
                purchaseReturn.PackingCharges = model.PackingCharges;
                purchaseReturn.NetAmount = model.NetAmount;
                purchaseReturn.IsDraft = model.IsDraft;
                purchaseReturn.ReturnQty = model.ReturnQty;
                purchaseReturn.Id = model.ID;
                var ItemList = new List<PurchaseReturnTransItemBO>();
                PurchaseReturnTransItemBO grnTransBO;
                foreach (var itm in model.ReturnItems)
                {
                    grnTransBO = new PurchaseReturnTransItemBO();
                    grnTransBO.ItemID = itm.ItemID;
                    grnTransBO.SGSTAmount = itm.SGSTAmount;
                    grnTransBO.CGSTAmount = itm.CGSTAmount;
                    grnTransBO.IGSTAmount = itm.IGSTAmount;
                    grnTransBO.SGSTPercent = itm.SGSTPercent;
                    grnTransBO.IGSTPercent = itm.IGSTPercent;
                    grnTransBO.CGSTPercent = itm.CGSTPercent;
                    grnTransBO.InvoiceID = itm.InvoiceID;
                    grnTransBO.Quantity = itm.Quantity;
                    grnTransBO.Rate = itm.Rate;
                    grnTransBO.Amount = itm.Amount;
                    grnTransBO.Remarks = itm.Remarks;
                    grnTransBO.WarehouseID = itm.WarehouseID;
                    grnTransBO.BatchTypeID = itm.BatchTypeID;
                    grnTransBO.UnitID = itm.UnitID;
                    grnTransBO.InvoiceID = itm.InvoiceID;
                    grnTransBO.PurchaseReturnTransID = itm.PurchaseReturnTransID;
                    grnTransBO.PurchaseReturnID = itm.PurchaseReturnID;
                    grnTransBO.PurchaseReturnOrderTransID = itm.PurchaseReturnOrderTransID;
                    grnTransBO.PurchaseReturnOrderID = itm.PurchaseReturnOrderID;
                    ItemList.Add(grnTransBO);
                }
                var outId = iRGBL.SaveIRG(purchaseReturn, ItemList);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "IRG", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(PurchaseReturnModel model)
        {
            return Create(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            PurchaseReturnModel purchaseReturn = iRGBL.GetIRGDetail((int)id).Select(m => new PurchaseReturnModel()
            {

                ID = m.Id,
                ReturnNo = m.ReturnNo,
                ReturnDate = General.FormatDate(m.ReturnDate),
                SupplierName = m.SupplierName,
                ItemName = m.ItemName,
                GRNno = m.GRNno,
                PremisesName = m.PremisesName,
                ReturnQty = m.ReturnQty,
                Freight = m.Freight,
                PackingCharges = m.PackingCharges,
                OtherCharges = m.OtherCharges,
                NetAmount = m.NetAmount,
                SupplierID = m.SupplierID,
                IsDraft=m.IsDraft,
                IsProcessed=m.IsProcessed,

            }).First();
            if(!purchaseReturn.IsDraft || purchaseReturn.IsProcessed)
            {
                return RedirectToAction("Index");;
            }
            List<PurchaseReturnTransItemBO> purchaseReturnItem = iRGBL.GetIRGTransList((int)id);
            purchaseReturn.ReturnItems = iRGBL.GetIRGTransList((int)id).Select(a => new PurchaseReturnTransModel()
            {

                ItemName = a.ItemName,
                Unit = a.Unit,
                AcceptedQty = (decimal)a.AcceptedQty,
                Quantity = a.Quantity,
                Rate = a.Rate,
                SGSTAmount = a.SGSTAmount,
                SGSTPercent = a.SGSTPercent,
                IGSTAmount = a.IGSTAmount,
                IGSTPercent = a.IGSTPercent,
                CGSTAmount = a.CGSTAmount,
                CGSTPercent = a.CGSTPercent,
                Amount = a.Amount,
                Remarks = a.Remarks,
                ReturnNo = a.ReturnNo,
                ItemID = a.ItemID,
                UnitID = a.UnitID,
                InvoiceID = a.InvoiceID,
                PurchaseReturnTransID = a.PurchaseReturnTransID,
                PurchaseReturnID = a.PurchaseReturnID,
                WarehouseID = a.WarehouseID,
                PurchaseReturnOrderTransID = a.PurchaseReturnOrderTransID,
                PurchaseReturnOrderID = a.PurchaseReturnOrderID,
                InvoiceQty = a.InvoiceQty
            }).ToList();
            return View(purchaseReturn);
        }

        public ActionResult Save(int id)
        {
            return null;
        }

        public JsonResult GetIRGList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SupplierHint = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = iRGBL.GetIRGList(Type, TransNoHint, TransDateHint, SupplierHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}