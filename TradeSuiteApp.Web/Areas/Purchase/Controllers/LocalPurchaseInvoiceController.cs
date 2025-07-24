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
    public class LocalPurchaseInvoiceController : Controller
    {
        private IGeneralContract generalBL;
        private ICategoryContract categoryBL;
        private IBatchTypeContract BatchBL;
        private ILocalPurchaseInvoiceContract LocalPurchaseInvoiceBL;

        public LocalPurchaseInvoiceController(IDropdownContract IDropdown)
        {
            generalBL = new GeneralBL();
            categoryBL = new CategoryBL();
            BatchBL = new BatchTypeBL();
            LocalPurchaseInvoiceBL = new LocalPurchaseInvoiceBL();
        }
        // GET: Purchase/LocalPurchase
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            LocalPurchaseInvoiceModel LocalPurchaseModel = new LocalPurchaseInvoiceModel();
            LocalPurchaseModel.PurchaseOrderNo = generalBL.GetSerialNo("LocalPurchaseInvoice", "Code");
            LocalPurchaseModel.PurchaseOrderDate = General.FormatDate(DateTime.Now);
            LocalPurchaseModel.DDLItemCategory = Convert.ToInt32(generalBL.GetConfig("DefaultCategoryForLocalPurchase"));
            LocalPurchaseModel.PurchaseCategoryList = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            LocalPurchaseModel.BatchTypeList = new SelectList(
                                            BatchBL.GetBatchTypeList(), "ID", "Name");
            LocalPurchaseModel.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}
                                    }, "Value", "Text");
            LocalPurchaseModel.SupplierID = LocalPurchaseInvoiceBL.GetLocalPurchaseID().SupplierID;
            LocalPurchaseModel.IsGSTRegistered = LocalPurchaseInvoiceBL.GetLocalPurchaseID().IsGSTRegistered;
            return View(LocalPurchaseModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            bool IsExist = false;
            IsExist = LocalPurchaseInvoiceBL.GetIsLocalPurchase((int)id);
            if (!IsExist)
            {
                return View("PageNotFound");
            }


            List<PurchaseOrderTransBO> PurchaseOrderTransBO;
            LocalPurchaseInvoiceModel LocalPurchaseInvoiceModel;
            LocalPurchaseInvoiceItemsModel localPurchaseInvoiceItems;
            LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO = LocalPurchaseInvoiceBL.GetLocalPurchaseOrder((int)id);
            if(!LocalPurchaseInvoiceBO.IsDraft)
            {
                return RedirectToAction("Index");;
            }
            LocalPurchaseInvoiceModel = new LocalPurchaseInvoiceModel()
            {
                ID = LocalPurchaseInvoiceBO.ID,
                PurchaseOrderNo = LocalPurchaseInvoiceBO.PurchaseOrderNo,
                PurchaseOrderDate = General.FormatDate(LocalPurchaseInvoiceBO.PurchaseOrderDate),
                SupplierReference = LocalPurchaseInvoiceBO.SupplierReference,
                NetAmount = LocalPurchaseInvoiceBO.NetAmount,
                GSTAmount = LocalPurchaseInvoiceBO.GSTAmount
            };

            PurchaseOrderTransBO = LocalPurchaseInvoiceBL.GetLocalPurchaseOrderItems((int)id);
            LocalPurchaseInvoiceModel.Items = new List<LocalPurchaseInvoiceItemsModel>();
            foreach (var m in PurchaseOrderTransBO)
            {
                localPurchaseInvoiceItems = new LocalPurchaseInvoiceItemsModel()
                {
                    ItemID = m.ItemID,
                    UnitID = m.UnitID,
                    Qty = (decimal)m.QtyOrdered,
                    Rate = (decimal)m.Rate,
                    Remarks = m.Remarks,
                    CGSTAmount = (decimal)m.CGSTAmt,
                    SGSTAmount = (decimal)m.SGSTAmt,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    TotalAmount = (decimal)m.NetAmount,
                    ItemName = m.Name,
                    Unit = m.Unit,
                    Value = (decimal)m.Amount

                };
                LocalPurchaseInvoiceModel.Items.Add(localPurchaseInvoiceItems);
            }
            LocalPurchaseInvoiceModel.PurchaseOrderNo = generalBL.GetSerialNo("LocalPurchaseInvoice", "Code");
            LocalPurchaseInvoiceModel.PurchaseOrderDate = General.FormatDate(DateTime.Now);
            LocalPurchaseInvoiceModel.DDLItemCategory = Convert.ToInt32(generalBL.GetConfig("DefaultCategoryForLocalPurchase"));
            LocalPurchaseInvoiceModel.PurchaseCategoryList = new SelectList(categoryBL.GetPurchaseCategoryList(0), "ID", "Name");
            LocalPurchaseInvoiceModel.BatchTypeList = new SelectList(
                                            BatchBL.GetBatchTypeList(), "ID", "Name");
            LocalPurchaseInvoiceModel.UnitList = new SelectList(
                                    new List<SelectListItem>
                                    {
                                                new SelectListItem { Text = "", Value = "0"}
                                    }, "Value", "Text");
            LocalPurchaseInvoiceModel.SupplierID = LocalPurchaseInvoiceBL.GetLocalPurchaseID().SupplierID;
            LocalPurchaseInvoiceModel.IsGSTRegistered = LocalPurchaseInvoiceBL.GetLocalPurchaseID().IsGSTRegistered;


            return View(LocalPurchaseInvoiceModel);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            bool IsExist = false;
            IsExist = LocalPurchaseInvoiceBL.GetIsLocalPurchase((int)id);
            if (!IsExist)
            {
                return View("PageNotFound");
            }
            List<PurchaseOrderTransBO> PurchaseOrderTransBO;
            LocalPurchaseInvoiceModel LocalPurchaseInvoiceModel;
            LocalPurchaseInvoiceItemsModel localPurchaseInvoiceItems;
            LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO = LocalPurchaseInvoiceBL.GetLocalPurchaseOrder((int)id);
            LocalPurchaseInvoiceModel = new LocalPurchaseInvoiceModel()
            {
                ID = LocalPurchaseInvoiceBO.ID,
                PurchaseOrderNo = LocalPurchaseInvoiceBO.PurchaseOrderNo,
                PurchaseOrderDate = General.FormatDate(LocalPurchaseInvoiceBO.PurchaseOrderDate),
                SupplierReference = LocalPurchaseInvoiceBO.SupplierReference,
                NetAmount = LocalPurchaseInvoiceBO.NetAmount,
                IsDraft = LocalPurchaseInvoiceBO.IsDraft
            };

            PurchaseOrderTransBO = LocalPurchaseInvoiceBL.GetLocalPurchaseOrderItems((int)id);
            LocalPurchaseInvoiceModel.Items = new List<LocalPurchaseInvoiceItemsModel>();
            foreach (var m in PurchaseOrderTransBO)
            {
                localPurchaseInvoiceItems = new LocalPurchaseInvoiceItemsModel()
                {
                    ItemID = m.ItemID,
                    UnitID = m.UnitID,
                    Qty = (decimal)m.QtyOrdered,
                    Rate = (decimal)m.Rate,
                    Remarks = m.Remarks,
                    CGSTAmount = (decimal)m.CGSTAmt,
                    SGSTAmount = (decimal)m.SGSTAmt,
                    SGSTPercent = (decimal)m.SGSTPercent,
                    CGSTPercent = (decimal)m.CGSTPercent,
                    TotalAmount = (decimal)m.NetAmount,
                    ItemName = m.Name,
                    Unit = m.Unit,
                    Value = (decimal)m.Amount,
                    GSTAmount = (decimal)m.IGSTAmt,
                    GSTPercentage = (decimal)m.GSTPercentage,
                };
                LocalPurchaseInvoiceModel.Items.Add(localPurchaseInvoiceItems);
            }

            return View(LocalPurchaseInvoiceModel);
        }

        [HttpPost]
        public ActionResult Save(LocalPurchaseInvoiceModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    LocalPurchaseInvoiceBO Temp = LocalPurchaseInvoiceBL.GetLocalPurchaseOrder(model.ID);
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                LocalPurchaseInvoiceBO LocalPurchaseInvoiceBO = new LocalPurchaseInvoiceBO()
                {
                    ID = model.ID,
                    IsDraft = model.IsDraft,
                    PurchaseOrderNo = model.PurchaseOrderNo,
                    PurchaseOrderDate = General.ToDateTime(model.PurchaseOrderDate),
                    SupplierReference = model.SupplierReference,
                    GSTAmount = model.GSTAmount,
                    NetAmount = model.NetAmount,
                    SupplierID = model.SupplierID,
                    GrossAmnt = model.GrossAmnt
                };
                List<PurchaseOrderTransBO> LocalPurchaseInvoiceItems = new List<PurchaseOrderTransBO>();
                PurchaseOrderTransBO LocalPurchaseInvoiceItem;
                foreach (var item in model.Items)
                {
                    LocalPurchaseInvoiceItem = new PurchaseOrderTransBO()
                    {
                        ItemID = item.ItemID,
                        UnitID = item.UnitID,
                        Rate = item.Rate,
                        CGSTAmt = item.CGSTAmount,
                        SGSTAmt = item.SGSTAmount,
                        GSTPercentage = item.GSTPercentage,
                        SGSTPercent = item.SGSTPercent,
                        CGSTPercent = item.SGSTPercent,
                        Amount = item.Value,
                        IGSTPercent = 0,
                        IGSTAmt = 0,
                        BatchTypeID = 0,
                        NetAmount = item.TotalAmount,
                        Remarks = item.Remarks,
                        Quantity = item.Qty,

                    };
                    LocalPurchaseInvoiceItems.Add(LocalPurchaseInvoiceItem);
                }
                LocalPurchaseInvoiceBL.Save(LocalPurchaseInvoiceBO, LocalPurchaseInvoiceItems);
                return
                 Json(new
                 {
                     Status = "success",
                     data = "",
                     message = ""
                 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "LocalPurchaseInvoice", "Save", model.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAsDraft(LocalPurchaseInvoiceModel model)
        {
            return Save(model);
        }

        [HttpPost]
        public JsonResult GetLocalPurchases(DatatableModel Datatable)
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
                DatatableResultBO resultBO = LocalPurchaseInvoiceBL.GetLocalPurchases(Type, TransNoHint, TransDateHint, SupplierHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + LocalPurchaseInvoiceBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LocalPurchaseInvoicePrintPdf(int Id)
        {
            return null;
        }
    }
}