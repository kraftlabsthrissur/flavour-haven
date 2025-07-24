using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Sales.Controllers
{
    public class CounterSalesController : Controller
    {
        private ICounterSalesContract counterSalesBL;
        private ICustomerContract customerBL;
        private IWareHouseContract warehouseBL;
        private IBatchContract batchBL;
        private IGeneralContract generalBL;
        private IPaymentModeContract paymentModeBL;
        private IAddressContract addressBL;
        private ICategoryContract categroyBL;
        private ITreasuryContract treasuryBL;
        private ISubmasterContract submasterBL;
        private ILocationContract locationBL;
        private IGSTCategoryContract gstCategoryBL;

        //CounterSalesController class constructor
        public CounterSalesController()
        {
            customerBL = new CustomerBL();
            warehouseBL = new WarehouseBL();
            batchBL = new BatchBL();
            counterSalesBL = new CounterSalesBL();
            generalBL = new GeneralBL();
            paymentModeBL = new PaymentModeBL();
            addressBL = new AddressBL();
            categroyBL = new CategoryBL();
            treasuryBL = new TreasuryBL();
            submasterBL = new SubmasterBL();
            locationBL = new LocationBL();
            gstCategoryBL = new GSTCategoryBL();
        }
        // GET: Sales/CounterSales
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        //For DSC Build
        public ActionResult IndexV5()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Sales/CounterSales/Details/5
        public ActionResult Details(int? id)
        {
            CounterSalesModel counterSales;
            int counterSalesID;
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            counterSales = new CounterSalesModel();
            counterSalesID = (int)id;
            counterSales = this.GetCounterSalesList(counterSalesID, "view");
            counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
            counterSales.IsThermalPrint = counterSalesBL.IsThermalPrint();
            counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
            return View(counterSales);
        }
        public ActionResult DetailsV5(int? id)
        {
            CounterSalesModel counterSales;
            int counterSalesID;
            if (id == null)
            {
                return RedirectToAction("IndexV5");
            }

            counterSales = new CounterSalesModel();
            counterSalesID = (int)id;
            counterSales = this.GetCounterSalesList(counterSalesID, "view");
            counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
            counterSales.IsThermalPrint = counterSalesBL.IsThermalPrint();
            counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
            return View(counterSales);
        }
        private CounterSalesModel GetCounterSalesList(int counterSalesID, string type)
        {
            CounterSalesModel counterSales;
            CountrSalesItemsModel counterSalesItems;
            List<CounterSalesItemsBO> counterSalesItem;
            List<CounterSalesAmountDetailsBO> amountDetail;
            CounterSalesAmountDetails amountDetails;

            counterSales = counterSalesBL.GetCounterSalesDetail(counterSalesID).Select(m => new CounterSalesModel()
            {
                ID = m.ID,
                IsGST = m.IsGST,
                IsVat = m.IsVAT,
                TransNo = m.TransNo,
                TransDate = (type == "view") ? General.FormatDate(m.TransDate, "view") : General.FormatDate(m.TransDate),
                DoctorID = m.DoctorID,
                DoctorName = m.DoctorName,
                WarehouseID = m.WarehouseID,
                WarehouseName = m.WarehouseName,
                Remarks = m.Remarks,
                PrintWithItemCode = m.PrintWithItemCode,
                NetAmount = m.NetAmount,
                IsDraft = m.IsDraft,
                IsCancelled = m.IsCancelled,
                PackingPrice = m.PackingPrice,
                PatientID = m.PatientID,
                RoundOff = m.RoundOff,
                CGSTAmount = m.CGSTAmount,
                IGSTAmount = m.IGSTAmount,
                SGSTAmount = m.SGSTAmount,
                TotalVATAmount = m.TotalVATAmount,
                PaymentModeID = m.PaymentModeID,
                TotalAmountReceived = m.TotalAmountReceived,
                BalanceToBePaid = m.BalanceToBePaid,
                CessAmount = m.CessAmount,
                CompanyName = GeneralBO.CompanyName,
                Address1 = GeneralBO.Address1,
                Address2 = GeneralBO.Address2,
                Address3 = GeneralBO.Address3,
                Address4 = GeneralBO.Address4,
                Address5 = GeneralBO.Address5,
                GSTNo = GeneralBO.GSTNo,
                EmployeeID = m.EmployeeID,
                TypeID = m.TypeID,
                Type = m.Type,
                NetAmountInWords = generalBL.NumberToText(Convert.ToInt32(m.NetAmount)),
                PartyName = m.PartyName,
                CustomerID = m.CustomerID,
                ContactID = m.ContactID,
                ContactName = m.ContactName,
                CivilID = m.CivilID,
                MobileNumber = m.MobileNumber,
                PatientName = m.PatientName,
                EmployeeName = m.EmployeeName,
                BalAmount = m.BalanceAmount,
                TaxableAmt = m.TaxableAmt,
                GrossAmount = m.GrossAmount,
                DiscountAmt = m.DiscountAmt,
                BankID = m.BankID,
                CashSalesName = m.CashSalesName,
                DiscountCategoryID = m.DiscountCategoryID,
                DiscountCategory = m.DiscountCategory,
                BusinessCategoryID = m.BusinessCategoryID,
                BillDiscount = m.DiscountAmt,
                ReferenceNo=m.ReferenceNo,
                BillDiscountPercent = m.DiscountPercentage,
                VATPercentage = m.VATPercentage,
                VATPercentageID = m.VATPercentageID,
                BankList = new SelectList(treasuryBL.GetBankForCounterSales(m.PaymentModeID == 1 ? "Cash" : "Bank"), "ID", "BankName"),
                DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Value = item.Value
                }).ToList(),

            }).FirstOrDefault();
            counterSalesItem = counterSalesBL.GetCounterSalesListDetails(counterSalesID);
            counterSales.Items = new List<CountrSalesItemsModel>();
            counterSales.IsCancelable = counterSalesBL.IsCancelable(counterSalesID);
            foreach (var itm in counterSalesItem)
            {
                counterSalesItems = new CountrSalesItemsModel()
                {
                    ID = (int)itm.ID,
                    CounterSalesID = itm.CounterSalesID,
                    ItemID = itm.ItemID,
                    MRP = itm.MRP,
                    Rate = itm.Rate,
                    SecondaryQty = itm.SecondaryQty,
                    SecondaryUnit = itm.SecondaryUnit,
                    SecondaryRate = itm.SecondaryRate,
                    SecondaryUnitSize = itm.SecondaryUnitSize,
                    SecondaryOfferQty = itm.SecondaryOfferQty,
                    Qty = itm.Quantity,
                    Name = itm.Name,
                    PartsNumber = itm.PartsNumber,
                    DeliveryTerm = itm.DeliveryTerm,
                    Model = itm.Model,
                    SGSTAmount = itm.SGSTAmount,
                    CGSTAmount = itm.CGSTAmount,
                    IGSTAmount = itm.IGSTAmount,
                    VATAmount = itm.VATAmount,
                    NetAmount = itm.NetAmount,
                    FullOrLoose = itm.FullOrLoose,
                    IGSTPercentage = itm.IGSTPercentage,
                    CGSTPercentage = itm.CGSTPercentage,
                    SGSTPercentage = itm.SGSTPercentage,
                    VATPercentage = itm.VATPercentage,
                    CurrencyName = itm.CurrencyName,
                    CurrencyID = itm.CurrencyID,
                    IsGST = itm.IsGST,
                    IsVAT = itm.IsVAT,
                    GrossAmount = itm.GrossAmount,
                    Code = itm.Code,
                    BatchNo = itm.BatchNo,
                    BatchTypeID = itm.BatchTypeID,
                    WareHouseID = itm.WareHouseID,
                    ExpiryDateString = General.FormatDate((DateTime)itm.ExpiryDate),
                    ItemCode = itm.Code,
                    Unit = itm.Unit,
                    SecondaryUnitList = SecondaryUnitList(itm.Unit, itm.SecondaryUnits),
                    UnitID = itm.UnitID,
                    Stock = itm.Stock,
                    BatchID = itm.BatchID,
                    TaxableAmount = itm.TaxableAmount,
                    CessAmount = itm.CessAmount,
                    CessPercentage = itm.CessPercentage,
                    DiscountAmount = itm.DiscountAmount,
                    DiscountPercentage = itm.DiscountPercentage,
                    BasicPrice = itm.BasicPrice,
                    HSNCode = itm.HSNCode,
                    MinimumSalesQty = itm.MinimumSalesQty,
                    MaximumSalesQty = itm.MaximumSalesQty
                };
                counterSales.Items.Add(counterSalesItems);
            }

            amountDetail = counterSalesBL.GetCounterSalesListAmount(counterSalesID);
            counterSales.AmountDetails = new List<CounterSalesAmountDetails>();
            foreach (var itm in amountDetail)
            {
                amountDetails = new CounterSalesAmountDetails()
                {
                    Particulars = itm.Particulars,
                    Amount = itm.Amount,
                    Percentage = itm.Percentage
                };
                counterSales.AmountDetails.Add(amountDetails);
            }
            return (counterSales);
        }
        private List<SecondaryUnitBO> SecondaryUnitList(string Unit, string SecondaryUnits)
        {
            List<SecondaryUnitBO> secondaryUnits = new List<SecondaryUnitBO>();
            SecondaryUnitBO primaryUnitBO = new SecondaryUnitBO();
            primaryUnitBO.Name = Unit;
            primaryUnitBO.PackSize = 1;
            secondaryUnits.Add(primaryUnitBO);
            string[] SecondaryUnitsArray = SecondaryUnits.Split(',');
            for (int i = 0; i < SecondaryUnitsArray.Length; i++)
            {
                var SecondaryUnitItem = SecondaryUnitsArray[i].Split('|'); ;
                if (SecondaryUnitItem.Length > 1)
                {
                    SecondaryUnitBO secondaryUnitBO = new SecondaryUnitBO();
                    var text = SecondaryUnitItem[0];
                    var value = SecondaryUnitItem[1];
                    secondaryUnitBO.Name = text;
                    secondaryUnitBO.PackSize = Convert.ToDecimal(value);
                    secondaryUnits.Add(secondaryUnitBO);
                }
            }
            return secondaryUnits;
        }
        // GET: Sales/CounterSales/Create
        public ActionResult Create()
        {
            CounterSalesModel counterSales = new CounterSalesModel();

            counterSales.TransNo = generalBL.GetSerialNo("CounterSales", "Code");
            counterSales.PatientCode = generalBL.GetSerialNo("Patient", "Code");
            counterSales.TransDate = General.FormatDate(DateTime.Now);
            counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
            counterSales.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            counterSales.SalesCategoryList = new SelectList(categroyBL.GetSalesCategoryList(222), "ID", "Name");
            counterSales.CounterSalesTypeList = new SelectList(counterSalesBL.GetCounterSalesType(), "ID", "Type");
            counterSales.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultFGStore"));
            counterSales.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            counterSales.StateID = generalBL.GetStateIDByLocation();
            counterSales.BankList = new SelectList(treasuryBL.GetBankForCounterSales("Cash"), "ID", "BankName");
            counterSales.DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            //if (counterSales.StateID == 32)
            //{
            //    counterSales.BatchTypeID = 1;
            //}
            //else
            //{
            //    counterSales.BatchTypeID = 2;
            //}
            counterSales.BatchTypeID = 1;
            counterSales.PatientSexList = new SelectList(
              new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");
            counterSales.FullOrLooseID = Convert.ToInt16(generalBL.GetConfig("DefaultFullOrLoose"));

            counterSales.UnitList = new SelectList(
                                       new List<SelectListItem>
                                       {
                                                new SelectListItem { Text = "", Value = "0"}
                                       }, "Value", "Text");
            counterSales.Items = new List<CountrSalesItemsModel>();
            counterSales.AmountDetails = new List<CounterSalesAmountDetails>();
            counterSales.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            //counterSales.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
            counterSales.TypeID = 3;
            counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
            counterSales.IsThermalPrint = counterSalesBL.IsThermalPrint();
            counterSales.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
            counterSales.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            return View(counterSales);
        }


        //For DSC Build -for Print
        // GET: Sales/CounterSales/Create
        public ActionResult CreateV5()
        {
            CounterSalesModel counterSales = new CounterSalesModel();

            counterSales.TransNo = generalBL.GetSerialNo("CounterSales", "Code");
            counterSales.PatientCode = generalBL.GetSerialNo("Patient", "Code");
            counterSales.TransDate = General.FormatDate(DateTime.Now);
            counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
            counterSales.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            counterSales.SalesCategoryList = new SelectList(categroyBL.GetSalesCategoryList(222), "ID", "Name");
            counterSales.CounterSalesTypeList = new SelectList(counterSalesBL.GetCounterSalesType(), "ID", "Type");
            counterSales.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultFGStore"));
            counterSales.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            counterSales.StateID = generalBL.GetStateIDByLocation();
            counterSales.BankList = new SelectList(treasuryBL.GetBankForCounterSales("Cash"), "ID", "BankName");
            counterSales.DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            //if (counterSales.StateID == 32)
            //{
            //    counterSales.BatchTypeID = 1;
            //}
            //else
            //{
            //    counterSales.BatchTypeID = 2;
            //}
            counterSales.BatchTypeID = 1;
            counterSales.PatientSexList = new SelectList(
              new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");
            counterSales.FullOrLooseID = Convert.ToInt16(generalBL.GetConfig("DefaultFullOrLoose"));

            counterSales.UnitList = new SelectList(
                                       new List<SelectListItem>
                                       {
                                                new SelectListItem { Text = "", Value = "0"}
                                       }, "Value", "Text");
            counterSales.Items = new List<CountrSalesItemsModel>();
            counterSales.AmountDetails = new List<CounterSalesAmountDetails>();
            counterSales.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            counterSales.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
            counterSales.TypeID = 3;
            counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
            counterSales.IsThermalPrint = counterSalesBL.IsThermalPrint();
            counterSales.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
            counterSales.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            return View(counterSales);
        }


        // POST: Sales/CounterSales/Create
        [HttpPost]
        public ActionResult Save(CounterSalesModel modal)
        {
            var result = new List<Object>();
            try
            {
                if (modal.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    CounterSalesBO Temp = counterSalesBL.GetCounterSalesDetail(Convert.ToInt16(modal.ID)).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                CounterSalesBO counterSales = new CounterSalesBO();
                counterSales.ID = modal.ID;
                counterSales.TransNo = modal.TransNo;
                counterSales.TransDate = General.ToDateTime(modal.TransDate);
                counterSales.DoctorID = modal.DoctorID;
                counterSales.PartyName = modal.PartyName;
                counterSales.ContactName = modal.ContactName;
                counterSales.CustomerID = modal.CustomerID;
                counterSales.ContactID = modal.ContactID;
                counterSales.CivilID = modal.CivilID;
                counterSales.MobileNumber = modal.MobileNumber;
                counterSales.PatientID = modal.PatientID;
                counterSales.WarehouseID = modal.WarehouseID;
                counterSales.NetAmount = modal.NetAmount;
                counterSales.IsDraft = modal.IsDraft;
                counterSales.SGSTAmount = modal.SGSTAmount;
                counterSales.CGSTAmount = modal.CGSTAmount;
                counterSales.IGSTAmount = modal.IGSTAmount;
                counterSales.TotalVATAmount = modal.TotalVATAmount;
                counterSales.IsVAT = modal.IsVat;
                counterSales.IsGST = modal.IsGST;
                counterSales.CurrencyID = modal.CurrencyID;
                counterSales.PackingPrice = modal.PackingPrice;
                counterSales.RoundOff = modal.RoundOff;
                counterSales.TotalAmountReceived = modal.TotalAmountReceived;
                counterSales.PaymentModeID = modal.PaymentModeID;
                counterSales.BalanceToBePaid = modal.BalanceToBePaid;
                counterSales.CessAmount = modal.CessAmount;
                counterSales.EmployeeID = modal.EmployeeID;
                counterSales.TypeID = modal.TypeID;
                counterSales.GrossAmount = modal.GrossAmount;
                counterSales.TaxableAmt = modal.TaxableAmt;
                counterSales.BankID = modal.BankID;
                counterSales.DoctorName = modal.DoctorName == null ? "" : modal.DoctorName;
                counterSales.DiscountCategoryID = modal.DiscountCategoryID;
                counterSales.DiscountAmt = modal.BillDiscount;
                counterSales.DiscountPercentage = modal.BillDiscountPercent;
                counterSales.PrintWithItemCode = modal.PrintWithItemCode;
                counterSales.Remarks = modal.Remarks;
                counterSales.AmountRecieveds = modal.AmountRecieveds;
                counterSales.ReferenceNo = modal.ReferenceNo;
                counterSales.VATPercentage = modal.VATPercentage;
                counterSales.VATPercentageID = modal.VATPercentageID;
                //counterSales.DiscountAmt = modal.DiscountAmt;
                var ItemList = new List<CounterSalesItemsBO>();
                CounterSalesItemsBO counterSalesItemBO;
                for (int i = 0; i < modal.Items.Count; i++)
                {
                    var Items = modal.Items.Skip(i).Take(1).FirstOrDefault();
                    counterSalesItemBO = new CounterSalesItemsBO();
                    counterSalesItemBO.CounterSalesID = Items.CounterSalesID;
                    counterSalesItemBO.FullOrLoose = Items.FullOrLoose;
                    counterSalesItemBO.ItemID = Items.ItemID;
                    counterSalesItemBO.ItemName = Items.ItemName;
                    counterSalesItemBO.PartsNumber = Items.PartsNumber;
                    counterSalesItemBO.DeliveryTerm = Items.DeliveryTerm;
                    counterSalesItemBO.Model = Items.Model;
                    counterSalesItemBO.BatchID = Items.BatchID;
                    counterSalesItemBO.Quantity = Items.Quantity;
                    counterSalesItemBO.Rate = Items.Rate;
                    counterSalesItemBO.MRP = Items.MRP;
                    counterSalesItemBO.BasicPrice = Items.BasicPrice;
                    counterSalesItemBO.GrossAmount = Items.GrossAmount;
                    counterSalesItemBO.SGSTPercentage = Items.SGSTPercentage;
                    counterSalesItemBO.CGSTPercentage = Items.CGSTPercentage;
                    counterSalesItemBO.IGSTPercentage = Items.IGSTPercentage;
                    counterSalesItemBO.SGSTAmount = Items.SGSTAmount;
                    counterSalesItemBO.CGSTAmount = Items.CGSTAmount;
                    counterSalesItemBO.IGSTAmount = Items.IGSTAmount;
                    counterSalesItemBO.IsVAT = modal.IsVat;
                    counterSalesItemBO.IsGST = modal.IsGST;
                    counterSalesItemBO.SecondaryUnit = Items.SecondaryUnit;
                    counterSalesItemBO.SecondaryOfferQty = Items.SecondaryOfferQty;
                    counterSalesItemBO.SecondaryQty = Items.SecondaryQty;
                    counterSalesItemBO.SecondaryUnitSize = Items.SecondaryUnitSize;
                    counterSalesItemBO.SecondaryRate = Items.SecondaryRate;
                    counterSalesItemBO.CurrencyID = modal.CurrencyID;
                    counterSalesItemBO.VATPercentage = Items.VATPercentage;
                    counterSalesItemBO.VATAmount = Items.VATAmount;
                    counterSalesItemBO.NetAmount = Items.NetAmount;
                    counterSalesItemBO.BatchTypeID = Items.BatchTypeID;
                    counterSalesItemBO.WareHouseID = Items.WareHouseID;
                    counterSalesItemBO.TaxableAmount = Items.TaxableAmount;
                    counterSalesItemBO.UnitID = Items.UnitID;
                    counterSalesItemBO.CessAmount = Items.CessAmount;
                    counterSalesItemBO.CessPercentage = Items.CessPercentage;
                    counterSalesItemBO.DiscountAmount = Items.DiscountAmount;
                    counterSalesItemBO.DiscountPercentage = Items.DiscountPercentage;
                    ItemList.Add(counterSalesItemBO);
                }

                var amountDtails = new List<CounterSalesAmountDetailsBO>();
                CounterSalesAmountDetailsBO counterSalesAmountDetailsBO;
                if (modal.AmountDetails != null)
                {
                    foreach (var items in modal.AmountDetails)
                    {
                        counterSalesAmountDetailsBO = new CounterSalesAmountDetailsBO();
                        counterSalesAmountDetailsBO.Amount = items.Amount;
                        counterSalesAmountDetailsBO.Particulars = items.Particulars;
                        counterSalesAmountDetailsBO.Percentage = items.Percentage;
                        amountDtails.Add(counterSalesAmountDetailsBO);
                    }
                }
                var ID = counterSalesBL.SaveCounterSalesInvoice(counterSales, ItemList, amountDtails);
                return Json(new { Status = "success", ID = ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Sales", "CounterSales", "Save", Convert.ToInt16(modal.ID), e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sales/CounterSales/Edit/5
        public ActionResult Edit(int id)
        {
            CounterSalesModel counterSales;
            int counterSalesID;
            if (id > 0)
            {
                counterSales = new CounterSalesModel();
                counterSalesID = (int)id;
                counterSales = this.GetCounterSalesList(counterSalesID, "");
                if (!counterSales.IsDraft || counterSales.IsCancelled)
                {
                    return RedirectToAction("Index");
                }
                counterSales.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultFGStore"));
                counterSales.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
                counterSales.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
                counterSales.SalesCategoryList = new SelectList(categroyBL.GetSalesCategoryList(222), "ID", "Name");
                counterSales.CounterSalesTypeList = new SelectList(counterSalesBL.GetCounterSalesType(), "ID", "Type");
                counterSales.StateID = generalBL.GetStateIDByLocation();
                counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
                counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
                counterSales.BatchTypeID = 1;
                counterSales.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
                counterSales.PatientSexList = new SelectList(
                new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");

                counterSales.UnitList = new SelectList(
                              new List<SelectListItem>
                              {
                                                new SelectListItem { Text = "", Value = "0"}

                              }, "Value", "Text");
                return View(counterSales);
            }
            return RedirectToAction("Index");

        }

        public ActionResult EditV5(int id)
        {
            CounterSalesModel counterSales;
            int counterSalesID;
            if (id > 0)
            {
                counterSales = new CounterSalesModel();
                counterSalesID = (int)id;
                counterSales = this.GetCounterSalesList(counterSalesID, "");
                if (!counterSales.IsDraft || counterSales.IsCancelled)
                {
                    return RedirectToAction("IndexV5");
                }
                counterSales.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultFGStore"));
                counterSales.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
                counterSales.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
                counterSales.SalesCategoryList = new SelectList(categroyBL.GetSalesCategoryList(222), "ID", "Name");
                counterSales.CounterSalesTypeList = new SelectList(counterSalesBL.GetCounterSalesType(), "ID", "Type");
                counterSales.StateID = generalBL.GetStateIDByLocation();
                counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
                counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
                counterSales.BatchTypeID = 1;
                counterSales.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
                counterSales.PatientSexList = new SelectList(
                new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");

                counterSales.UnitList = new SelectList(
                              new List<SelectListItem>
                              {
                                                new SelectListItem { Text = "", Value = "0"}

                              }, "Value", "Text");
                return View(counterSales);
            }
            return RedirectToAction("IndexV5");

        }
        public JsonResult GetCounterSalesTrans(int[] SalesInvoiceIDS, int PriceListID)
        {
            try
            {
                CounterSalesBO SalesInvoice = new CounterSalesBO();
                SalesInvoice.Items = new List<CounterSalesItemsBO>();
                if (SalesInvoiceIDS.Length > 0)
                {
                    foreach (var SalesInvoiceID in SalesInvoiceIDS)
                    {
                        var list = counterSalesBL.GetCounterSalesTransForCounterSalesReturn(SalesInvoiceID, PriceListID);

                        if (list != null)
                        {
                            SalesInvoice.Items.AddRange(list);
                        }
                    }
                }
                return Json(SalesInvoice.Items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "CounterSales", "GetCounterSalesTrans", 0, e);
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Cancel(int CounterSalesID)
        {
            try
            {
                if (counterSalesBL.IsCancelable(CounterSalesID))
                {
                    counterSalesBL.Cancel(CounterSalesID);
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

        [HttpPost]
        public JsonResult GetBatchwiseItemForCounterSales(int ItemID = 0, int WarehouseID = 0, int BatchTypeID = 0, decimal Qty = 0, int UnitID = 0, string Unit = "", string CustomerType = "", int TaxTypeID = 0, string PartsNumber = "")
        {
            try
            {
                List<CountrSalesItemsModel> itemlist = counterSalesBL.GetBatchwiseItemForCounterSales(ItemID, WarehouseID, BatchTypeID, UnitID, Qty, CustomerType, TaxTypeID).Select(
                    m => new CountrSalesItemsModel()
                    {

                        ItemID = m.ItemID,
                        BatchID = m.BatchID,
                        BatchTypeID = m.BatchTypeID,
                        BatchNo = m.BatchNo,
                        CGSTPercentage = m.CGSTPercentage,
                        SGSTPercentage = m.SGSTPercentage,
                        IGSTPercentage = m.IGSTPercentage,
                        VATPercentage = m.VATPercentage,
                        PartsNumber = m.PartsNumber,
                        DeliveryTerm = m.DeliveryTerm,
                        Model = m.Model,
                        Stock = m.Stock,
                        Unit = Unit,
                        SecondaryUnits = m.SecondaryUnits,
                        UnitID = UnitID,
                        FullPrice = m.FullPrice,
                        LoosePrice = m.LoosePrice,
                        BatchType = m.BatchType,
                        Code = m.Code,
                        ExpiryDateString = General.FormatDateNull(m.ExpiryDate),
                        Quantity = m.Quantity,
                        WareHouseID = WarehouseID,
                        Name = m.Name,
                        SalesUnitID = m.SalesUnitID,
                        CessPercentage = m.CessPercentage,
                        IsGSTRegisteredLocation = m.IsGSTRegisteredLocation,
                        IsGST = m.IsGST,
                        IsVAT = m.IsVAT,
                        TaxType = m.TaxType,
                        CurrencyID = (int)m.CurrencyID,
                        CurrencyName = m.CurrencyName,
                    }).Where(x => !string.IsNullOrEmpty(PartsNumber) && !string.IsNullOrEmpty(x.PartsNumber) ? x.PartsNumber.Trim().ToLower() == PartsNumber.Trim().ToLower() : true).ToList();


                return Json(new { Status = "success", Data = itemlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "CounterSales", "GetBatchwiseItemForCounterSales", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetGoodsReceiptsItemForCounterSales(int[] CounterSalesID)
        {
            try
            {
                var BOItems = counterSalesBL.GetGoodsReciptItemForCounterSales(CounterSalesID).ToList();
                List<SalesItemModel> Items = new List<SalesItemModel>();
                SalesItemModel Item;
                foreach (var item in BOItems)
                {
                    Item = new SalesItemModel()
                    {
                        CounterSalesID = item.CounterSalesID,
                        CounterSalesItemTransID = item.ID,
                        TransNo = item.TransNo,
                        ItemID = item.ItemID,
                        ItemName = item.Name,
                        PartsNumber = item.PartsNumber,
                        PrintWithItemName = item.PrintWithItemName,
                        Remarks = item.Remarks,
                        Model = item.Model,
                        Code = item.Code,
                        SecondaryMRP = item.SecondaryRate,
                        SecondaryUnit = item.SecondaryUnit,
                        SecondaryQty = item.SecondaryQty,
                        UnitName = item.Unit,
                        BatchName = item.Batch,
                        BatchID = item.BatchID,
                        BatchTypeID = item.BatchTypeID,
                        Rate = item.Rate,
                        BasicPrice = item.BasicPrice,
                        Qty = item.Quantity,
                        OfferQty = 0,
                        GrossAmount = item.GrossAmount,
                        DiscountPercentage = item.DiscountPercentage,
                        DiscountAmount = item.DiscountAmount,
                        TaxableAmount = item.TaxableAmount,
                        GSTPercentage = item.GSTPercentage.HasValue ? item.GSTPercentage.Value : 0,
                        SGSTPercentage = item.SGSTPercentage,
                        CGSTPercentage = item.CGSTPercentage,
                        IGSTPercentage = item.IGSTPercentage,
                        VATPercentage = item.VATPercentage,
                        VATAmount = item.VATAmount,
                        IsGST = item.IsGST,
                        IsVat = item.IsVAT,
                        CurrencyID = item.CurrencyID,
                        CurrencyName = item.CurrencyName,
                        IGST = item.IGSTAmount,
                        CGST = item.CGSTAmount,
                        SGST = item.SGSTAmount,
                        //GSTAmount = item.IGSTAmount + item.CGSTAmount + item.SGSTAmount, already set in model
                        NetAmount = item.NetAmount,
                        //StoreID = item.StoreID,
                        InvoiceQty = item.Quantity,
                        InvoiceOfferQty = 0,
                        //InvoiceQtyMet = item.InvoiceQtyMet,
                        //InvoiceOfferQtyMet = item.InvoiceOfferQtyMet,
                        Stock = item.Stock,
                        UnitID = item.UnitID,
                        SalesUnitID = item.SalesUnitID,
                        LooseRate = item.MRP,
                        MRP = item.MRP,
                        CessAmount = item.CessAmount,
                        CessPercentage = item.CessPercentage,
                        Category = item.CategoryName,
                        CategoryID = item.ItemCategoryID
                    };
                    Items.Add(Item);
                }

                return Json(new { Status = "success", Items = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "CounterSales", "GetBatchwiseItemForCounterSales", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetCounterSalesForReturn(DatatableModel Datatable)
        {
            try
            {
                string TransHint = Datatable.Columns[2].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int PartyID = Convert.ToInt32(Datatable.GetValueFromKey("PartyID", Datatable.Params));
                DatatableResultBO resultBO = counterSalesBL.GetCounterSalesForReturn(PartyID, TransHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "CounterSales", "GetCounterSalesForReturn", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(CounterSalesModel modal)
        {
            return Save(modal);
        }

        public JsonResult GetPatientSerialNo()
        {
            string SerialNo = generalBL.GetSerialNo("Patient", "Code");
            return Json(new { Status = "success", data = SerialNo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + counterSalesBL.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerCounterSalesList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string PartyNameHint = Datatable.Columns[3].Search.Value;
                string NetAmountHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));
                DatatableResultBO resultBO = counterSalesBL.GetCustomerCounterSalesList(CustomerID,TransNoHint, TransDateHint, PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "CounterSales", "GetListForCounterSales", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetListForCounterSales(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SalesTypeHint = Datatable.Columns[3].Search.Value;
                string PartyNameHint = Datatable.Columns[6].Search.Value;
                string NetAmountHint = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = counterSalesBL.GetListForCounterSales(Type, TransNoHint, TransDateHint, SalesTypeHint, PartyNameHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "CounterSales", "GetListForCounterSales", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAppointmentProcessList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[2].Search.Value;
                string TransDateHint = Datatable.Columns[3].Search.Value;
                string PatientNameHint = Datatable.Columns[4].Search.Value;
                string DoctorNameHint = Datatable.Columns[5].Search.Value;
                string PhoneHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = counterSalesBL.GetAppointmentProcessList(TransNoHint, TransDateHint, PatientNameHint, DoctorNameHint, PhoneHint, SortField, SortOrder, Offset, Limit);
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
        public JsonResult GetBatchwisePrescriptionItems(int AppointmentProcessID = 0, int PatientID = 0)
        {
            try
            {
                List<SalesItemBO> Items = counterSalesBL.GetBatchwisePrescriptionItems(AppointmentProcessID, PatientID);
                return Json(new { Status = "success", Items = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Sales", "CounterSales", "GetBatchwisePrescriptionItems", 0, e);
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CounterSalesInvoicePrintPdf(int Id)
        {
            return null;
        }

        public ActionResult CreateV4()
        {
            CounterSalesModel counterSales = new CounterSalesModel();

            counterSales.TransNo = generalBL.GetSerialNo("CounterSales", "Code");
            counterSales.PatientCode = generalBL.GetSerialNo("Patient", "Code");
            counterSales.TransDate = General.FormatDate(DateTime.Now);
            counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
            var address = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault();
            counterSales.LocationStateID = address != null ? address.StateID:0;
            counterSales.SalesCategoryList = new SelectList(categroyBL.GetSalesCategoryList(222), "ID", "Name");
            counterSales.CounterSalesTypeList = new SelectList(counterSalesBL.GetCounterSalesType(), "ID", "Type");
            counterSales.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultFGStore"));
            counterSales.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
            counterSales.StateID = generalBL.GetStateIDByLocation();
            counterSales.BankList = new SelectList(treasuryBL.GetBankForCounterSales("Cash"), "ID", "BankName");
            counterSales.IsGSTRegisteredLocation = generalBL.IsGSTRegisteredLocation(GeneralBO.LocationID);
            counterSales.DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
            {
                ID = item.ID,
                Name = item.Name,
                Value = item.Value
            }).ToList();
            counterSales.BatchTypeID = 1;
            counterSales.PatientSexList = new SelectList(
              new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");
            counterSales.FullOrLooseID = Convert.ToInt16(generalBL.GetConfig("DefaultFullOrLoose"));

            counterSales.UnitList = new SelectList(
                                       new List<SelectListItem>
                                       {
                                                new SelectListItem { Text = "", Value = "0"}
                                       }, "Value", "Text");
            counterSales.Items = new List<CountrSalesItemsModel>();
            counterSales.AmountDetails = new List<CounterSalesAmountDetails>();
            counterSales.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
            counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            counterSales.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
            counterSales.TypeID = 3;
            counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
            counterSales.IsThermalPrint = counterSalesBL.IsThermalPrint();
            counterSales.CompanyName = GeneralBO.CompanyName;
            counterSales.Address1 = GeneralBO.Address1;
            counterSales.Address2 = GeneralBO.Address2;
            counterSales.GSTNo = GeneralBO.GSTNo;
            counterSales.PhoneNo = GeneralBO.MobileNo;
            counterSales.LocationID = GeneralBO.LocationID;
            counterSales.IsGSTRegisteredLocation = generalBL.IsGSTRegisteredLocation(GeneralBO.LocationID);
            counterSales.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            if (currency != null)
            {
                counterSales.CurrencyID = currency.CurrencyID;
                counterSales.CurrencyName = currency.CurrencyName;
                counterSales.CountryName = currency.CountryName;
                counterSales.CurrencyCode = currency.CurrencyCode;
                counterSales.DecimalPlaces = currency.DecimalPlaces;
                counterSales.CountryID = currency.CountryID;
                counterSales.IsVat = currency.IsVat;
                counterSales.IsGST = currency.IsGST;
                counterSales.TaxType = currency.TaxType;
                counterSales.TaxTypeID = currency.TaxTypeID;

            }
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(counterSales.CurrencyID);
            if (classdata != null)
            {
                counterSales.normalclass = classdata.normalclass;
                counterSales.largeclass = classdata.largeclass;
                counterSales.DecimalPlaces = classdata.DecimalPlaces;
            }
            counterSales.PrintWithItemCode = true;
            counterSales.IsVATExtra = Convert.ToInt16(generalBL.GetConfig("IsVATExtra"));
            counterSales.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            counterSales.VATPercentageID = Convert.ToInt16(generalBL.GetConfig("VATPercentageID"));
            return View(counterSales);
        }

        public ActionResult DetailsV4(int? id)
        {
            CounterSalesModel counterSales;
            int counterSalesID;
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            counterSales = new CounterSalesModel();
            counterSalesID = (int)id;
            counterSales = this.GetCounterSalesList(counterSalesID, "view");
            counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
            counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
            counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
            counterSales.IsThermalPrint = counterSalesBL.IsThermalPrint();
            counterSales.CompanyName = GeneralBO.CompanyName;
            counterSales.Address1 = GeneralBO.Address1;
            counterSales.Address2 = GeneralBO.Address2;
            counterSales.GSTNo = GeneralBO.GSTNo;
            counterSales.PhoneNo = GeneralBO.MobileNo;
            counterSales.IsGSTRegisteredLocation = generalBL.IsGSTRegisteredLocation(GeneralBO.LocationID);
            var classdata = counterSales.Items != null && counterSales.Items.Count > 0 ? counterSalesBL.GetCurrencyDecimalClassByCurrencyID(counterSales.Items.First().CurrencyID) : null;
            if (classdata != null)
            {
                counterSales.normalclass = classdata.normalclass;
                counterSales.largeclass = classdata.largeclass;
                counterSales.DecimalPlaces = classdata.DecimalPlaces;
            }
            counterSales.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
            return View(counterSales);
        }

        public ActionResult IndexV4()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
            var classdata = counterSalesBL.GetCurrencyDecimalClassByCurrencyID(currency.CurrencyID);
            if (classdata != null)
            {
                ViewBag.normalclass = classdata.normalclass;
                ViewBag.largeclass = classdata.largeclass;
            }
            return View();
        }

        // GET: Sales/CounterSales/Edit/5
        public ActionResult EditV4(int id)
        {
            CounterSalesModel counterSales;
            int counterSalesID;
            if (id > 0)
            {
                counterSales = new CounterSalesModel();
                counterSalesID = (int)id;
                counterSales = this.GetCounterSalesList(counterSalesID, "");
                if (!counterSales.IsDraft || counterSales.IsCancelled)
                {
                    return RedirectToAction("Index");
                }
                counterSales.PaymentModeList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name", counterSales.PaymentModeID);
                counterSales.LocationStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
                counterSales.SalesCategoryList = new SelectList(categroyBL.GetSalesCategoryList(222), "ID", "Name");
                counterSales.CounterSalesTypeList = new SelectList(counterSalesBL.GetCounterSalesType(), "ID", "Type");
                counterSales.WarehouseID = Convert.ToInt16(generalBL.GetConfig("DefaultFGStore"));
                counterSales.WarehouseList = new SelectList(warehouseBL.GetWareHouses(), "ID", "Name");
                counterSales.StateID = generalBL.GetStateIDByLocation();
                counterSales.BankList = new SelectList(treasuryBL.GetBankForCounterSales("Cash"), "ID", "BankName");
                counterSales.IsGSTRegisteredLocation = generalBL.IsGSTRegisteredLocation(GeneralBO.LocationID);
                counterSales.DiscountPercentageList = categroyBL.GetDiscountCategory().Select(item => new CategoryBO()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Value = item.Value
                }).ToList();
                counterSales.BatchTypeID = 1;
                counterSales.PatientSexList = new SelectList(
                  new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");
                counterSales.FullOrLooseID = Convert.ToInt16(generalBL.GetConfig("DefaultFullOrLoose"));

                counterSales.UnitList = new SelectList(
                                           new List<SelectListItem>
                                           {
                                                new SelectListItem { Text = "", Value = "0"}
                                           }, "Value", "Text");

                counterSales.EmployeeCategoryID = submasterBL.GetEmployeeCategoryID("Doctor");
                counterSales.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(222), "ID", "Name");
                counterSales.BusinessCategoryID = Convert.ToInt16(generalBL.GetConfig("DefaultBusinessCategory"));
                counterSales.TypeID = 3;
                counterSales.IsDotMatrixPrint = counterSalesBL.IsDotMatrixPrint();
                counterSales.IsThermalPrint = counterSalesBL.IsThermalPrint();
                counterSales.CompanyName = GeneralBO.CompanyName;
                counterSales.Address1 = GeneralBO.Address1;
                counterSales.Address2 = GeneralBO.Address2;
                counterSales.GSTNo = GeneralBO.GSTNo;
               
                counterSales.PhoneNo = GeneralBO.MobileNo;
                counterSales.IsPriceEditable = Convert.ToInt16(generalBL.GetConfig("IsPriceEditable"));
                var currency = locationBL.GetCurrentLocationTaxDetails().FirstOrDefault();
                if (currency != null)
                {
                    counterSales.CurrencyID = currency.CurrencyID;
                    counterSales.CurrencyName = currency.CurrencyName;
                    counterSales.CountryName = currency.CountryName;
                    counterSales.CurrencyCode = currency.CurrencyCode;
                    counterSales.DecimalPlaces = currency.DecimalPlaces;
                    counterSales.CountryID = currency.CountryID;
                    counterSales.IsVat = currency.IsVat;
                    counterSales.IsGST = currency.IsGST;
                    counterSales.TaxType = currency.TaxType;
                    counterSales.TaxTypeID = currency.TaxTypeID;

                }
                var classdata = counterSales.Items != null && counterSales.Items.Count > 0 ? counterSalesBL.GetCurrencyDecimalClassByCurrencyID(counterSales.Items.First().CurrencyID) : null;
                if (classdata != null)
                {
                    counterSales.normalclass = classdata.normalclass;
                    counterSales.largeclass = classdata.largeclass;
                    counterSales.DecimalPlaces = classdata.DecimalPlaces;
                }
                counterSales.VATPercentageList = new SelectList(gstCategoryBL.GetVatPercentage(), "ID", "VATPercent");
                return View(counterSales);
            }
            return RedirectToAction("Index");

        }

        public JsonResult GetCounterSalesDetails(int? ID)
        {
            try
            {
                int counterSalesID;
                CounterSalesModel counterSales;
                counterSales = new CounterSalesModel();
                counterSalesID = (int)ID;
                counterSales = this.GetCounterSalesList(counterSalesID, "view");
                return Json(new { Status = "success", Data = counterSales }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Sales", "CounterSales", "GetCounterSalesDetails", 0, e);
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
