using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
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
    public class CounterSalesReturnController : Controller
    {
        // GET: Sales/CounterSalesReturn
        private ICounterSalesReturn salesReturnBL;
        private ICounterSalesContract countersalesBL;
        private IGeneralContract generalBL;
        private IUnitContract unitBL;
        private IPaymentModeContract paymentModeBL;
        private ITreasuryContract treasuryBL;

        public CounterSalesReturnController()
        {
            // customerBL = new CustomerBL();
            // categroyBL = new CategoryBL();
            salesReturnBL = new CounterSalesReturnBL();
            countersalesBL = new CounterSalesBL();
            generalBL = new GeneralBL();
            unitBL = new UnitBL();
            paymentModeBL = new PaymentModeBL();
            treasuryBL = new TreasuryBL();
        }

        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };

            return View();
        }

        // GET: Sales/SalesOrder/Details/5
        public ActionResult Details(int id)
        {
            CounterSalesReturnModel salesReturn = salesReturnBL.GetCounterSalesReturn(id).Select(m => new CounterSalesReturnModel()
            {
                ID = m.ID,
                ReturnNo = m.ReturnNo,
                ReturnDate = General.FormatDate(m.ReturnDate, "view"),
                NetAmount = m.NetAmount,
                IsDraft = m.IsDraft,
                RoundOff = m.RoundOff,
                PaymentModeID = m.PaymentModeID,
                PaymentMode = m.PaymentMode,
                BankID = m.BankID,
                BankName = m.BankName,
                PartyName = m.PartyName,
                InvoiceID = m.InvoiceID,
                InvoiceNo = m.InvoiceNo,
                Reason = m.Reason,
                BillDiscount = m.BillDiscount,
                VATAmount = m.VATAmount,
            }).FirstOrDefault();
            salesReturn.Items = salesReturnBL.GetCounterSalesReturnTrans(id).Select(itm => new CounterSalesReturnItemModel()
            {
                ID = (int)itm.ID,
                CounterSalesID = itm.CounterSalesID,
                ItemID = itm.ItemID,
                MRP = itm.MRP,
                Rate = itm.Rate,
                Qty = itm.Quantity,
                Name = itm.Name,
                SGSTAmount = itm.SGSTAmount,
                CGSTAmount = itm.CGSTAmount,
                IGSTAmount = itm.IGSTAmount,
                NetAmount = itm.NetAmount,
                FullOrLoose = itm.FullOrLoose,
                IGSTPercentage = itm.IGSTPercentage,
                CGSTPercentage = itm.CGSTPercentage,
                SGSTPercentage = itm.SGSTPercentage,
                GrossAmount = itm.GrossAmount,
                Code = itm.Code,
                BatchNo = itm.BatchNo,
                BatchTypeID = itm.BatchTypeID,
                WareHouseID = itm.WareHouseID,
                ItemCode = itm.Code,
                Unit = itm.Unit,
                UnitID = itm.UnitID,
                BatchID = itm.BatchID,
                TaxableAmount = itm.TaxableAmount,
                ReturnQty = itm.ReturnQty,
                CounterSalesTransID = itm.CounterSalesTransID,
                CounterSalesTransUnitID = itm.CounterSalesTransUnitID,
                ConvertedQuantity = itm.ConvertedQuantity,
                CessPercentage = itm.CessPercentage,
                CessAmount = itm.CessAmount,
                SecondaryUnit = itm.SecondaryUnit,
                SecondaryQty = itm.Qty / itm.SecondaryUnitSize,
                SecondaryReturnQty = itm.SecondaryReturnQty,
                SecondaryUnitSize = itm.SecondaryUnitSize,
                SecondaryRate = itm.SecondaryRate,
                DiscountPercentage =itm.DiscountPercentage,
                DiscountAmount = itm.DiscountAmount,
                VATPercentage = itm.VATPercentage,
                VATAmount = itm.VATAmount,
            }).ToList();
            return View(salesReturn);
        }

        // GET: Sales/SalesOrder/Create
        public ActionResult Create()
        {
            CounterSalesReturnModel salesReturn = new CounterSalesReturnModel();

            salesReturn.ReturnNo = generalBL.GetSerialNo("CountersalesReturn", "Code");
            salesReturn.ReturnDate = General.FormatDate(DateTime.Now);
            salesReturn.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", salesReturn.PaymentModeID);
            salesReturn.BankList = new SelectList(treasuryBL.GetBankForCounterSales("Cash"), "ID", "BankName");
            salesReturn.Items = new List<CounterSalesReturnItemModel>();

            return View(salesReturn);
        }

        // POST: Sales/SalesReturn/Create
        [HttpPost]
        public ActionResult Save(CounterSalesReturnModel modal)
        {
            var result = new List<Object>();
            try
            {
                if (modal.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    CounterSalesReturnBO Temp = salesReturnBL.GetCounterSalesReturn(Convert.ToInt16(modal.ID)).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                CounterSalesReturnBO counterSales = new CounterSalesReturnBO();
                counterSales.ID = modal.ID;
                counterSales.ReturnNo = modal.ReturnNo;
                counterSales.ReturnDate = General.ToDateTime(modal.ReturnDate);
                counterSales.NetAmount = modal.NetAmount;
                counterSales.IsDraft = modal.IsDraft;
                counterSales.SGSTAmount = modal.SGSTAmount;
                counterSales.CGSTAmount = modal.CGSTAmount;
                counterSales.IGSTAmount = modal.IGSTAmount;
                counterSales.RoundOff = modal.RoundOff;
                counterSales.BankID = modal.BankID;
                counterSales.PaymentModeID = modal.PaymentModeID;
                counterSales.InvoiceID = modal.InvoiceID;
                counterSales.Reason = modal.Reason;
                counterSales.BillDiscount = modal.BillDiscount;
                counterSales.PartyID = modal.PartyID;
                counterSales.VATAmount = counterSales.VATAmount;
                var ItemList = new List<CounterSalesReturnItemBO>();
                CounterSalesReturnItemBO counterSalesItemBO;
                foreach (var Items in modal.Items)
                {
                    counterSalesItemBO = new CounterSalesReturnItemBO();
                    counterSalesItemBO.CounterSalesID = Items.CounterSalesID;
                    counterSalesItemBO.CounterSalesTransID = Items.CounterSalesTransID;
                    counterSalesItemBO.ReturnQty = Items.ReturnQty;
                    counterSalesItemBO.FullOrLoose = Items.FullOrLoose;
                    counterSalesItemBO.ItemID = Items.ItemID;
                    counterSalesItemBO.BatchID = Items.BatchID;
                    counterSalesItemBO.Quantity = Items.Quantity;
                    counterSalesItemBO.Rate = Items.Rate;
                    counterSalesItemBO.MRP = Items.MRP;
                    counterSalesItemBO.GrossAmount = Items.GrossAmount;
                    counterSalesItemBO.SGSTPercentage = Items.SGSTPercentage;
                    counterSalesItemBO.CGSTPercentage = Items.CGSTPercentage;
                    counterSalesItemBO.IGSTPercentage = Items.IGSTPercentage;
                    counterSalesItemBO.SGSTAmount = Items.SGSTAmount;
                    counterSalesItemBO.CGSTAmount = Items.CGSTAmount;
                    counterSalesItemBO.IGSTAmount = Items.IGSTAmount;
                    counterSalesItemBO.NetAmount = Items.NetAmount;
                    counterSalesItemBO.BatchTypeID = Items.BatchTypeID;
                    counterSalesItemBO.WareHouseID = Items.WareHouseID;
                    counterSalesItemBO.TaxableAmount = Items.TaxableAmount;
                    counterSalesItemBO.UnitID = Items.UnitID;
                    counterSalesItemBO.SecondaryUnit = Items.SecondaryUnit;
                    counterSalesItemBO.SecondaryReturnQty = Items.SecondaryReturnQty;
                    counterSalesItemBO.SecondaryUnitSize = Items.SecondaryUnitSize;
                    counterSalesItemBO.SecondaryRate = Items.SecondaryRate;
                    counterSalesItemBO.CessPercentage = Items.CessPercentage;
                    counterSalesItemBO.CessAmount = Items.CessAmount;
                    counterSalesItemBO.VATPercentage = Items.VATPercentage;
                    counterSalesItemBO.VATAmount = Items.VATAmount;
                    counterSalesItemBO.DiscountPercentage = Items.DiscountPercentage;
                    counterSalesItemBO.DiscountAmount = Items.DiscountAmount;
                    ItemList.Add(counterSalesItemBO);
                }
                var outID = salesReturnBL.SaveCounterSalesReturn(counterSales, ItemList);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "CounterSalesReturn", "Save", Convert.ToInt16(modal.ID), e);
                return Json(new { response = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(CounterSalesReturnModel modal)
        {
            return Save(modal);
        }

        public ActionResult Edit(int id)
        {
            CounterSalesReturnModel salesReturn = salesReturnBL.GetCounterSalesReturn(id).Select(m => new CounterSalesReturnModel()
            {
                ID = m.ID,
                ReturnNo = m.ReturnNo,
                ReturnDate = General.FormatDate(m.ReturnDate),
                NetAmount = m.NetAmount,
                IsDraft = m.IsDraft,
                RoundOff = m.RoundOff,
                SGSTAmount = m.SGSTAmount,
                CGSTAmount = m.CGSTAmount,
                IGSTAmount = m.IGSTAmount,
                PaymentModeID = m.PaymentModeID,
                PaymentMode = m.PaymentMode,
                BankID = m.BankID,
                BankName = m.BankName,
                BankList = new SelectList(treasuryBL.GetBankForCounterSales(m.PaymentModeID == 1 ? "Cash" : "Bank"), "ID", "BankName"),
                PartyID = m.PartyID,
                PartyName = m.PartyName,
                InvoiceID = m.InvoiceID,
                InvoiceNo = m.InvoiceNo,
                Reason = m.Reason,
                BillDiscount = m.BillDiscount,
                VATAmount = m.VATAmount
            }).FirstOrDefault();
            if (!salesReturn.IsDraft)
            {
                return RedirectToAction("Index");
            }
            salesReturn.Items = salesReturnBL.GetCounterSalesReturnTrans(id).Select(itm => new CounterSalesReturnItemModel()
            {
                ID = (int)itm.ID,
                CounterSalesID = itm.CounterSalesID,
                ItemID = itm.ItemID,
                MRP = itm.MRP,
                Rate = itm.Rate,
                Qty = itm.Quantity,
                Name = itm.Name,
                SGSTAmount = itm.SGSTAmount,
                CGSTAmount = itm.CGSTAmount,
                IGSTAmount = itm.IGSTAmount,
                NetAmount = itm.NetAmount,
                FullOrLoose = itm.FullOrLoose,
                IGSTPercentage = itm.IGSTPercentage,
                CGSTPercentage = itm.CGSTPercentage,
                SGSTPercentage = itm.SGSTPercentage,
                GrossAmount = itm.GrossAmount,
                Code = itm.Code,
                BatchNo = itm.BatchNo,
                BatchTypeID = itm.BatchTypeID,
                WareHouseID = itm.WareHouseID,
                ItemCode = itm.Code,
                Unit = itm.Unit,
                Stock = itm.Stock,
                BatchID = itm.BatchID,
                TaxableAmount = itm.TaxableAmount,
                ReturnQty = itm.ReturnQty,
                CounterSalesTransID = itm.CounterSalesTransID,
                CessPercentage = itm.CessPercentage,
                CessAmount = itm.CessAmount,
                UnitList = new SelectList(
                                             new List<SelectListItem>
                                             {
                                                new SelectListItem { Text = itm.SalesUnitName, Value =itm.SalesUnitID.ToString()},
                                                new SelectListItem { Text = itm.PrimaryUnit, Value =itm.PrimaryUnitID.ToString()},
                                             }, "Value", "Text"),//;
                                                                 // UnitList = new SelectList(unitBL.GetUnitsByItemID(itm.ItemID, "sales"), "ID", "Name"),
                                                                 // UnitList = new SelectList(unitBL.GetUnitsByItemID(itm.ItemID, "sales"), "ID", "Name"),
                UnitID = itm.UnitID,
                SalesUnitID = itm.SalesUnitID,
                PrimaryUnitID = itm.PrimaryUnitID,
                FullPrice = itm.FullPrice,
                LoosePrice = itm.LoosePrice,
                CounterSalesTransUnitID = itm.CounterSalesTransUnitID,
                ConvertedQuantity = itm.ConvertedQuantity,
                CounterSalesQty = itm.CounterSalesQty,
                DiscountPercentage = itm.DiscountPercentage,
                DiscountAmount = itm.DiscountAmount,
                VATPercentage = itm.VATPercentage,
                VATAmount = itm.VATAmount,
            }).ToList();
            salesReturn.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", salesReturn.PaymentModeID);

            return View(salesReturn);
        }

        public JsonResult GetCounterSalesReturnListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string ReturnNo = Datatable.Columns[1].Search.Value;
                string ReturnDate = Datatable.Columns[2].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = salesReturnBL.GetCounterSalesReturnListForDataTable(Type, ReturnNo, ReturnDate, SortField, SortOrder, Offset, Limit);
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
        public JsonResult CounterSalesReturnPrintPdf(int Id)
        {
            return null;
        }
    }
}