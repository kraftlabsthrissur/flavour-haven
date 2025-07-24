using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebAPI.Areas.Masters.Controllers;
using WebAPI.Areas.Masters.Models;
using WebAPI.Areas.Sales.Models;
using WebAPI.Utils;

namespace WebAPI.Areas.Sales.Controllers
{
    [Authorize]
    public class CounterSalesAPIController : Controller
    {
        private IEmployeeContract employeeBL;
        private ICounterSalesContract counterSalesBL;
        private IBatchContract batchBL;
        private IGeneralContract generalBL;

        public CounterSalesAPIController()
        {
            employeeBL = new EmployeeBL();
            counterSalesBL = new CounterSalesBL();
            batchBL = new BatchBL();
            generalBL = new GeneralBL();
        }
        // GET: Sales/CounterSalesAPI
        [HttpGet]

        public JsonResult GetDoctorsList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            employeeList = employeeBL.GetDoctorsList().Select(a => new EmployeeModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Place = a.Place

            }).ToList();
            return Json(new { Status = "Success", Data = employeeList }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetBatchDetailsByBatchNo(string BatchNo)
        {
            //generalBL.LogError("CounterSales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", 0, BatchNo, "", "");
            try
            {
                var obj = batchBL.GetBatchDetailsByBatchNo(BatchNo);
                //generalBL.LogError("CounterSales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", obj.ID, BatchNo, "", "");
                BatchModel batchModel = new BatchModel();
                batchModel.ID = obj.ID;
                batchModel.BatchNo = obj.BatchNo;
                batchModel.CustomBatchNo = obj.CustomBatchNo;
                batchModel.ItemName = obj.Name;
                batchModel.ExpDate = General.FormatDate(obj.ExpiryDate, "view");
                batchModel.Unit = obj.Unit;
                batchModel.UnitID = obj.UnitID;
                batchModel.Stock = obj.Stock;
                batchModel.ItemID = obj.ItemID;
                batchModel.GSTPercentage = obj.GSTPercentage;
                batchModel.CessPercentage = obj.CessPercentage;
                batchModel.RetailLooseRate = obj.RetailLooseRate;
                batchModel.BusinessCategory = obj.BusinessCategory;
                batchModel.BusinessCategoryID = obj.BusinessCategoryID;
                return Json(new { Status = "Success", Data = batchModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //generalBL.LogError("CounterSales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", 0, BatchNo, "", "");
                //generalBL.LogError("Sales", "CounterSalesAPI", "GetBatchDetailsByBatchNo", 0, e);
                return Json(new { Status = "failure", Data = e }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public int PostDummy(string Name)
        {
            generalBL.PostDummy(Name);
            return 1;
        }
        [HttpPost]
        public JsonResult PostCounterSaless(CounterSalesModel modal)
        {
            try
            {
                //string json = new JavaScriptSerializer().Serialize(modal);
                //string jsonitems = new JavaScriptSerializer().Serialize(modal.Items);
                //generalBL.LogError("Sales", "SalesInvoice", "GenerateEInvoice", 0, json, jsonitems, "");
                var count = Convert.ToInt32(counterSalesBL.GetIsCounterSalesAlreadyExists(modal.PartyName,modal.NetAmount).Counts);
                if (count == 0)
                {
                    CounterSalesBO counterSales = new CounterSalesBO();
                    counterSales.ID = modal.ID;
                    counterSales.TransNo = modal.TransNo;
                    counterSales.TransDate = General.ToDateTime(modal.TransDate);
                    counterSales.DoctorID = modal.DoctorID;
                    counterSales.PartyName = modal.PartyName;
                    counterSales.PatientID = modal.PatientID;//0
                    counterSales.WarehouseID = modal.WarehouseID;//1
                    counterSales.NetAmount = Math.Round(modal.NetAmount, 4);
                    counterSales.IsDraft = modal.IsDraft;//0
                    counterSales.SGSTAmount = Math.Round(modal.SGSTAmount, 4);
                    counterSales.CGSTAmount = Math.Round(modal.CGSTAmount, 4);
                    counterSales.IGSTAmount = Math.Round(modal.IGSTAmount, 4);
                    counterSales.PackingPrice = modal.PackingPrice;//0
                    counterSales.RoundOff = modal.RoundOff;
                    counterSales.TotalAmountReceived = Math.Round(modal.TotalAmountReceived, 4);
                    counterSales.PaymentModeID = modal.PaymentModeID;//cash ntet id 
                    counterSales.BalanceToBePaid = modal.BalanceToBePaid;//0
                    counterSales.CessAmount = modal.CessAmount;
                    counterSales.EmployeeID = modal.EmployeeID;//0
                    counterSales.TypeID = modal.TypeID;//0
                    counterSales.GrossAmount = Math.Round(modal.GrossAmount, 4);
                    counterSales.TaxableAmt = Math.Round(modal.TaxableAmt, 4);
                    counterSales.BankID = modal.BankID;//1
                    counterSales.DoctorName = modal.DoctorName == null ? "" : modal.DoctorName;
                    counterSales.DiscountCategoryID = modal.DiscountCategoryID;//0
                    var ItemList = new List<CounterSalesItemsBO>();
                    CounterSalesItemsBO counterSalesItemBO;
                    foreach (var Items in modal.Items)
                    {
                        counterSalesItemBO = new CounterSalesItemsBO();
                        counterSalesItemBO.CounterSalesID = Items.CounterSalesID;
                        counterSalesItemBO.FullOrLoose = Items.FullOrLoose;
                        counterSalesItemBO.ItemID = Items.ItemID;
                        counterSalesItemBO.BatchID = Items.BatchID;
                        counterSalesItemBO.Quantity = Items.Quantity;
                        counterSalesItemBO.Rate = Items.Rate;
                        counterSalesItemBO.MRP = Items.MRP;
                        counterSalesItemBO.BasicPrice = Math.Round(Items.BasicPrice, 4);
                        counterSalesItemBO.GrossAmount = Math.Round(Items.GrossAmount, 4);
                        counterSalesItemBO.SGSTPercentage = Items.SGSTPercentage;
                        counterSalesItemBO.CGSTPercentage = Items.CGSTPercentage;
                        counterSalesItemBO.IGSTPercentage = Items.IGSTPercentage;
                        counterSalesItemBO.SGSTAmount = Math.Round(Items.SGSTAmount, 4);
                        counterSalesItemBO.CGSTAmount = Math.Round(Items.CGSTAmount, 4);
                        counterSalesItemBO.IGSTAmount = Math.Round(Items.IGSTAmount, 4);
                        counterSalesItemBO.NetAmount = Math.Round(Items.NetAmount, 4);
                        counterSalesItemBO.BatchTypeID = Items.BatchTypeID;
                        counterSalesItemBO.WareHouseID = Items.WareHouseID;
                        counterSalesItemBO.TaxableAmount = Math.Round(Items.TaxableAmount, 4);
                        counterSalesItemBO.UnitID = Items.UnitID;
                        counterSalesItemBO.CessAmount = Items.CessAmount;
                        counterSalesItemBO.CessPercentage = Items.CessPercentage;
                        ItemList.Add(counterSalesItemBO);
                    }

                    var amountDtails = new List<CounterSalesAmountDetailsBO>();
                    CounterSalesAmountDetailsBO counterSalesAmountDetailsBO;
                    foreach (var items in modal.AmountDetails)
                    {
                        counterSalesAmountDetailsBO = new CounterSalesAmountDetailsBO();
                        counterSalesAmountDetailsBO.Amount = Math.Round(items.Amount, 4);
                        counterSalesAmountDetailsBO.Particulars = items.Particulars;
                        counterSalesAmountDetailsBO.Percentage = items.Percentage;
                        amountDtails.Add(counterSalesAmountDetailsBO);
                    }
                    var ID = counterSalesBL.SaveCounterSalesInvoice(counterSales, ItemList, amountDtails);
                    return Json(new { Status = "Success", Data = ID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "Success", Message = count }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                //generalBL.LogError("CounterSalesAPI", "CounterSales", "Save", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}