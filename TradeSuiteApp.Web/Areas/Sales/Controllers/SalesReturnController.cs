using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class SalesReturnController : Controller
    {
        private ICustomerContract customerBL;
        private ICategoryContract categroyBL;
        private ISalesReturn salesReturnBL;
        private ISalesInvoice salesInvoiceBL;
        private IGeneralContract generalBL;
        private IAddressContract addressBL;
        private IUnitContract unitBL;
        public SalesReturnController()
        {
            customerBL = new CustomerBL();
            categroyBL = new CategoryBL();
            salesReturnBL = new SalesReturnBL();
            salesInvoiceBL = new SalesInvoiceBL();
            generalBL = new GeneralBL();
            addressBL = new AddressBL();
            unitBL = new UnitBL();
        }

        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Sales/SalesOrder/Details/5
        public ActionResult Details(int id)
        {
            SalesReturnModel salesReturn = salesReturnBL.GetSalesReturn(id).Select(m => new SalesReturnModel()
            {
                ID = m.ID,
                SRNo = m.SRNo,
                CurrencyCode = m.CurrencyCode,
                SRDate = General.FormatDate(m.SRDate, "view"),
                CustomerName = m.CustomerName,
                NetAmount = m.NetAmount,
                RoundOff = m.RoundOff,
                InvoiceNo = m.InvoiceNo,
                SalesInvoiceID = m.SalesInvoiceID,
                IsNewInvoice = m.IsNewInvoice,
                Status = m.IsCancelled ? "cancelled" : m.IsDraft ? "draft" : ""

            }
            ).First();
            List<SalesItemBO> salesReturnItemBO = salesReturnBL.GetSalesReturnTrans((int)id);
            salesReturn.Items = salesReturnBL.GetSalesReturnTrans(id).Select(a => new SalesReturnItemModel()
            {
                ID = a.ID,
                ItemID = a.ItemID,
                ItemName = a.Name,
                MRP = a.MRP,
                SecondaryQty = a.SecondaryQty,
                SecondaryUnitSize = a.SecondaryUnitSize,
                SecondaryMRP = a.SecondaryMRP,
                SecondaryOfferQty = a.SecondaryOfferQty,
                SecondaryUnit = a.SecondaryUnit,
                BasicPrice = a.BasicPrice,
                Qty = a.Qty,
                OfferQty = a.OfferQty,
                DiscountPercentage = a.DiscountPercentage,
                DiscountAmount = a.DiscountAmount,
                GrossAmount = a.GrossAmount,
                CGST = a.CGST,
                IGST = a.IGST,
                SGST = a.SGST,
                IGSTPercentage = a.IGSTPercentage,
                CGSTPercentage = a.CGSTPercentage,
                SGSTPercentage = a.SGSTPercentage,
                NetAmount = a.NetAmount,
                UnitName = a.Unit,
                UnitID = a.UnitID,
                SaleQty = a.SecondaryUnitSize > 0 ? a.SaleQty / a.SecondaryUnitSize : a.SaleQty,
                ItemCode = a.Code,
                OfferReturnQty = a.OfferReturnQty,
                Batch = a.BatchName,
                LogicCodeID = a.LogicCodeID,
                LogicCode = a.LogicCode,
                LogicName = a.LogicName,
                VATPercentage = a.VATPercentage,
                VATAmount = a.VATAmount,
            }).ToList();
            return View(salesReturn);
        }

        // GET: Sales/SalesOrder/Create
        public ActionResult Create()
        {
            SalesReturnModel salesReturn = new SalesReturnModel();

            salesReturn.SRNo = generalBL.GetSerialNo("SalesReturn", "Code");
            salesReturn.SRDate = General.FormatDate(DateTime.Now);
            salesReturn.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesReturn.ReturnInvoiceList = new List<SalesInvoiceModel>();
            var address = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            salesReturn.LocationStateID = address != null ? address.StateID : 0;
            salesReturn.Items = new List<SalesReturnItemModel>();
            salesReturn.UnitList = new SelectList(
                                            new List<SelectListItem>
                                            {
                                                new SelectListItem { Text = "", Value = "0"}

                                            }, "Value", "Text");
            salesReturn.IsNewInvoice = false;
            salesReturn.SalesReturnLogicCode = new SelectList(salesReturnBL.GetSalesReturnLogicCodeList(), "LogicCodeID", "LogicCode", "LogicName");
            return View(salesReturn);
        }

        // POST: Sales/SalesReturn/Create
        [HttpPost]
        public ActionResult Save(SalesReturnModel SalesReturn)
        {
            var result = new List<object>();
            try
            {
                if (SalesReturn.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    SalesReturnBO Temp = salesReturnBL.GetSalesReturn(SalesReturn.ID).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                // TODO: Add insert logic here
                SalesReturnBO SalesReturnBO = new SalesReturnBO()
                {
                    ID = SalesReturn.ID,
                    SRNo = SalesReturn.SRNo,
                    SRDate = General.ToDateTime(SalesReturn.SRDate),
                    CustomerCategoryID = SalesReturn.CustomerCategoryID,
                    CustomerID = SalesReturn.CustomerID,
                    SchemeAllocationID = SalesReturn.SchemeAllocationID,
                    GrossAmount = SalesReturn.GrossAmount,
                    DiscountAmount = SalesReturn.DiscountAmount,
                    TaxableAmount = SalesReturn.TaxableAmount,
                    SGSTAmount = SalesReturn.SGSTAmount,
                    CGSTAmount = SalesReturn.CGSTAmount,
                    IGSTAmount = SalesReturn.IGSTAmount,
                    RoundOff = SalesReturn.RoundOff,
                    NetAmount = SalesReturn.NetAmount,
                    IsDraft = SalesReturn.IsDraft,
                    SalesInvoiceID = SalesReturn.SalesInvoiceID,
                    InvoiceNo = SalesReturn.InvoiceNo,
                    IsNewInvoice = SalesReturn.IsNewInvoice
                };
                var ItemList = new List<SalesItemBO>();
                SalesItemBO salesReturnItemBO;
                foreach (var item in SalesReturn.Items)
                {
                    salesReturnItemBO = new SalesItemBO()
                    {
                        SalesReturnItemID = item.SalesReturnItemID,
                        MRP = item.MRP,
                        BasicPrice = item.BasicPrice,
                        Qty = item.Qty,
                        SecondaryQty = item.SecondaryQty,
                        SecondaryUnit = item.SecondaryUnit,
                        SecondaryUnitSize = item.SecondaryUnitSize,
                        SecondaryMRP = item.SecondaryMRP,
                        SecondaryOfferQty = item.SecondaryOfferQty,
                        OfferQty = item.OfferQty,
                        GrossAmount = item.GrossAmount,
                        DiscountPercentage = item.DiscountPercentage,
                        DiscountAmount = item.DiscountAmount,
                        TaxableAmount = item.TaxableAmount,
                        GSTPercentage = item.GSTPercentage,
                        CGST = item.CGST,
                        SGST = item.SGST,
                        IGST = item.IGST,
                        NetAmount = item.NetAmount,
                        ItemID = item.ItemID,
                        CGSTPercentage = item.CGSTPercentage,
                        SGSTPercentage = item.SGSTPercentage,
                        IGSTPercentage = item.IGSTPercentage,
                        SaleQty = item.SaleQty,
                        TransNo = item.TransNo,
                        SalesInvoiceID = item.SalesInvoiceID,
                        OfferReturnQty = item.OfferReturnQty,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        SalesInvoiceTransID = item.SalesInvoiceTransID,
                        BatchName = item.Batch,
                        UnitID = item.UnitID,
                        LogicCodeID = item.LogicCodeID,
                        VATPercentage = item.VATPercentage,
                        VATAmount = item.VATAmount
                    };
                    ItemList.Add(salesReturnItemBO);
                }
                salesReturnBL.SaveSalesReturn(SalesReturnBO, ItemList);

                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "SalesReturn", "Save", SalesReturn.ID, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult SaveAsDraft(SalesReturnModel SalesReturn)
        {
            return Save(SalesReturn);
        }

        public ActionResult Edit(int id)
        {
            SalesReturnModel salesReturn = salesReturnBL.GetSalesReturn(id).Select(m => new SalesReturnModel()
            {
                ID = m.ID,
                SRNo = m.SRNo,
                CurrencyCode = m.CurrencyCode,
                SRDate = General.FormatDate(m.SRDate),
                CustomerName = m.CustomerName,
                NetAmount = m.NetAmount,
                CustomerID = m.CustomerID,
                GrossAmount = m.GrossAmount,
                SGSTAmount = m.SGSTAmount,
                CGSTAmount = m.CGSTAmount,
                IGSTAmount = m.IGSTAmount,
                RoundOff = m.RoundOff,
                Status = m.IsCancelled ? "cancelled" : m.IsDraft ? "draft" : "",
                SalesInvoiceID = m.SalesInvoiceID,
                InvoiceNo = m.InvoiceNo,
                IsCancelled = m.IsCancelled,
                IsDraft = m.IsDraft,
                IsProcessed = m.IsProcessed,
                IsNewInvoice = m.IsNewInvoice
            }
           ).First();
            if (!salesReturn.IsDraft || salesReturn.IsCancelled)
            {
                return RedirectToAction("Index");
            }
            salesReturn.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            salesReturn.ReturnInvoiceList = new List<SalesInvoiceModel>();
            salesReturn.UnitList = new SelectList(
                                           new List<SelectListItem>
                                           {
                                                new SelectListItem { Text = "", Value = "0"}

                                           }, "Value", "Text");
            salesReturn.SalesReturnLogicCode = new SelectList(salesReturnBL.GetSalesReturnLogicCodeList(), "LogicCodeID", "LogicName");
            List<SalesItemBO> salesReturnItemBO = salesReturnBL.GetSalesReturnTrans((int)id);
            salesReturn.Items = salesReturnBL.GetSalesReturnTrans(id).Select(a => new SalesReturnItemModel()
            {
                ID = a.ID,
                ItemID = a.ItemID,
                ItemName = a.Name,
                MRP = a.MRP,
                BasicPrice = a.BasicPrice,
                Qty = a.Qty,
                SecondaryQty = a.SecondaryQty,
                SecondaryUnit = a.SecondaryUnit,
                SecondaryUnitSize = a.SecondaryUnitSize,
                SecondaryMRP = a.SecondaryMRP,
                SecondaryOfferQty = a.SecondaryOfferQty,
                OfferQty = a.OfferQty,
                DiscountPercentage = a.DiscountPercentage,
                DiscountAmount = a.DiscountAmount,
                GrossAmount = a.GrossAmount,
                CGST = a.CGST,
                IGST = a.IGST,
                SGST = a.SGST,
                IGSTPercentage = a.IGSTPercentage,
                CGSTPercentage = a.CGSTPercentage,
                SGSTPercentage = a.SGSTPercentage,
                NetAmount = a.NetAmount,
                UnitName = a.Unit,
                UnitID = a.UnitID,
                ItemCode = a.Code,
                OfferReturnQty = a.OfferReturnQty,
                Batch = a.BatchName,
                BatchID = a.BatchID,
                BatchTypeID = a.BatchTypeID,
                SalesInvoiceTransID = a.SalesInvoiceTransID,
                TransNo = a.TransNo,
                UOMList = new SelectList(
                                             new List<SelectListItem>
                                             {
                                                new SelectListItem { Text = a.SalesUnit, Value =a.SalesUnitID.ToString()},
                                                new SelectListItem { Text = a.PrimaryUnit, Value =a.PrimaryUnitID.ToString()},
                                             }, "Value", "Text"),
                //UnitList = new SelectList(unitBL.GetUnitsByItemID(a.ItemID, "sales"), "ID", "Name"),               
                SalesUnitID = a.SalesUnitID,
                PrimaryUnitID = a.PrimaryUnitID,
                FullPrice = a.FullPrice,
                LoosePrice = a.LoosePrice,
                SalesTransUnitID = a.SalesTransUnitID,
                ConvertedQuantity = a.ConvertedQuantity,
                SalesInvoiceQty = a.SalesInvoiceQty,
                LogicCodeID = a.LogicCodeID,
                LogicCode = a.LogicCode,
                LogicName = a.LogicName,
                ConvertedOfferQuantity = a.ConvertedOfferQuantity,
                SalesOfferQty = a.SalesOfferQty,
                VATPercentage = a.VATPercentage,
                VATAmount = a.VATAmount,
            }).ToList();
            return View(salesReturn);
        }

        public JsonResult GetDropdownVal()
        {
            List<SalesReturnBO> SalesReturnLogicCode = new List<SalesReturnBO>()
            {
                new SalesReturnBO()
                {
                    LogicCodeID = 0,
                    LogicName = "Select"
                }
            };
            SalesReturnLogicCode.AddRange(salesReturnBL.GetSalesReturnLogicCodeList());
            return Json(new { Status = "success", data = SalesReturnLogicCode }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesReturnListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string ReturnNo = Datatable.Columns[1].Search.Value;
                string ReturnDate = Datatable.Columns[2].Search.Value;
                string CustomerName = Datatable.Columns[3].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = salesReturnBL.GetSalesReturnListForDataTable(Type, ReturnNo, ReturnDate, CustomerName, SortField, SortOrder, Offset, Limit);
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

        public JsonResult SalesReturnPrintPdf(int Id)
        {
            return null;
        }
    }
}
