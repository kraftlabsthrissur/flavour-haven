using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class MilkPurchaseController : Controller
    {
        private IMilkPurchase milkPurchaseBL;
        private IMilkSupplierContract milkSupplier;
        private IGeneralContract generalBL;
        private string createmessage = App_LocalResources.Common.createsucess;
        private string failureMessage = App_LocalResources.Common.createfail;
        public MilkPurchaseController()
        {
            milkPurchaseBL = new MilkPurchaseBL();
            milkSupplier = new MilkSupplierBL();
            generalBL = new GeneralBL();
        }

        // GET: Purchase/MilkPurchase
        public ActionResult Index()
        {
            return View();
        }

        // GET: Purchase/MilkPurchase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            MilkPurchaseModel milkPurchase = milkPurchaseBL.GetAllMilkPurchase((int)id).Select(a => new MilkPurchaseModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                TotalAmount = a.TotalAmount,
                TotalQty = a.TotalQty,
                Date = General.FormatDate(a.Date, "view"),
                IsDraft = a.IsDraft,
            }).First();

            milkPurchase.MilkPurchaseTrans = milkPurchaseBL.GetAllMilkPurchaseTrans(milkPurchase.ID).Select(a => new MilkPurchseTransModel()
            {
                MilkPurchaseID = a.MilkPurchaseID,
                MilkSupplierID = a.MilkSupplierID,
                MilkSupplier = a.MilkSupplier,
                SupplierCode = a.SupplierCode,
                SlipNo = a.SlipNo,
                Qty = a.Qty,
                Rate = a.Rate,
                Amount = a.Amount,
                FatRangeID = a.FatRangeID,
                FatContentFrom = a.FatContentFrom,
                FatContentTo = a.FatContentTo,
                Remarks = a.Remarks
            }).ToList();
            try
            {
                milkPurchase.MilkSupplierID = milkPurchase.MilkPurchaseTrans.FirstOrDefault().MilkSupplierID;
                milkPurchase.SupplierName = milkPurchase.MilkPurchaseTrans.FirstOrDefault().MilkSupplier;
            }
            catch (Exception e)
            {

            }
            return View(milkPurchase);

        }

        // GET: Purchase/MilkPurchase/Create
        public ActionResult Create()
        {
            MilkPurchaseModel milkPurchase = new MilkPurchaseModel();

            milkPurchase.TransNo = generalBL.GetSerialNo("MilkPurchase", "Code");
            milkPurchase.DateString = General.FormatDate(DateTime.Now);
            milkPurchase.PurchaseRequisitionIDS = "";
            DateTime Date = DateTime.Now;

            milkPurchase.UnProcessedPrList = milkPurchaseBL.GetMilkPurchaseRqusition(Date).Select(a => new MilkPurchaseRequisitionModel()
            {
                ID = a.ID,
                ExpectedDate = General.FormatDate(a.ExpectedDate, "view"),
                FromDept = a.FromDept,
                Qty = a.Qty,
                PrNumber = a.PrNumber,


            }).ToList();
            milkPurchase.FatList = milkPurchaseBL.GetMilkFatRange("").Select(a => new MilkFatRangeModel()
            {
                FatRange = a.FatRange,
                ID = a.ID,
                Price = a.Price,
                WaterFrom = a.WaterFrom,
                WaterTo = a.WaterTo
            }).ToList();
            return View(milkPurchase);

        }

        // POST: Purchase/MilkPurchase/Create
        [HttpPost]
        public ActionResult Save(MilkPurchaseBO _master, List<MilkPurchseTransBO> _trans)
        {
            var result = new List<object>();
            try
            {
                if (_master.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    MilkPurchaseBO Temp = milkPurchaseBL.GetAllMilkPurchase(_master.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                var master = new MilkPurchaseBO();
                master = _master;
                master.Date = General.ToDateTime(master.DateString);

                if (milkPurchaseBL.SaveMilkPurchase(master, _trans))
                {
                    return Json(new { Status = "success", Message = "Milk purchase " + createmessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "MilkPurchase", "Save", _master.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(MilkPurchaseBO _master, List<MilkPurchseTransBO> _trans)
        {
            return Save(_master, _trans);
        }

        // GET: Purchase/MilkPurchase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            MilkPurchaseModel milkPurchase = milkPurchaseBL.GetAllMilkPurchase((int)id).Select(a => new MilkPurchaseModel
            {
                ID = a.ID,
                TransNo = a.TransNo,
                TotalAmount = a.TotalAmount,
                TotalQty = a.TotalQty,
                DateString = General.FormatDate(DateTime.Now),
                DirectInvoice = a.DirectInvoice,
                IsDraft=a.IsDraft
            }).FirstOrDefault();
            if (!milkPurchase.IsDraft)
            {
                return RedirectToAction("Index");
            }

            milkPurchase.FatList = milkPurchaseBL.GetMilkFatRange("").Select(a => new MilkFatRangeModel()
            {
                FatRange = a.FatRange,
                ID = a.ID,
                Price = a.Price,
                WaterFrom = a.WaterFrom,
                WaterTo = a.WaterTo
            }).ToList();

            milkPurchase.MilkPurchaseTrans = milkPurchaseBL.GetAllMilkPurchaseTrans(milkPurchase.ID).Select(a => new MilkPurchseTransModel()
            {
                MilkPurchaseID = a.MilkPurchaseID,
                MilkSupplierID = a.MilkSupplierID,
                MilkSupplier = a.MilkSupplier,
                SupplierCode = a.SupplierCode,
                SlipNo = a.SlipNo,
                Qty = a.Qty,
                Rate = a.Rate,
                Amount = a.Amount,
                FatRangeID = a.FatRangeID,
                FatContentFrom = a.FatContentFrom,
                FatContentTo = a.FatContentTo,
                Remarks = a.Remarks
            }).ToList();
            DateTime Date = DateTime.Now;

            milkPurchase.UnProcessedPrList = milkPurchaseBL.GetMilkPurchaseRqusition(Date).Select(a => new MilkPurchaseRequisitionModel()
            {
                ID = a.ID,
                ExpectedDate = General.FormatDate(a.ExpectedDate, "view"),
                FromDept = a.FromDept,
                Qty = a.Qty,
                PrNumber = a.PrNumber,

            }).ToList();
            try
            {
                milkPurchase.MilkSupplierID = milkPurchase.MilkPurchaseTrans.FirstOrDefault().MilkSupplierID;
                milkPurchase.SupplierName = milkPurchase.MilkPurchaseTrans.FirstOrDefault().MilkSupplier;
            }
            catch (Exception e)
            {

            }
            return View(milkPurchase);

        }

        public JsonResult GetMilkPurchaseList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SupplierNameHint = Datatable.Columns[3].Search.Value;
                string QuantityHint = Datatable.Columns[4].Search.Value;
                string AmountHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = milkPurchaseBL.GetMilkPurchaseList(Type, TransNoHint, TransDateHint, SupplierNameHint, QuantityHint, AmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MilkPurchasePrintPdf(int Id)
        {
            return null;
        }

    }
}