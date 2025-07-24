using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Areas.Sales.Models;
using TradeSuiteApp.Web.Utils;
using System.Web;

namespace TradeSuiteApp.Web.Areas.API.Controllers
{
    [Route("api/[Controller]")]
    public class SalesAPIController : ApiController
    {
        private IEmployeeContract employeeBL;
        private ICounterSalesContract counterSalesBL;
        private IBatchContract batchBL;

        public SalesAPIController()
        {
            employeeBL = new EmployeeBL();
            counterSalesBL = new CounterSalesBL();
            batchBL = new BatchBL();

            //HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            //HttpContext.Current.Session["FinYear"] = 2019;
            //HttpContext.Current.Session["ApplicationID"] = 1;
            //HttpContext.Current.Session["LocationID"] = 1;
            //HttpContext.Current.Session["CreatedUserID"] = 1;
        }

        [HttpGet]
        [Route("GetEmployee")]
        public IEnumerable<EmployeeModel> GetEmployee()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            employeeList = employeeBL.GetEmployeeList().Select(a => new EmployeeModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Place = a.Place

            }).ToList();
            return (employeeList);
        }


        [HttpGet]
        [Route("GetBatchDetailsByBatchNo")]
        public BatchModel GetBatchDetailsByBatchNo(string BatchNo)
        {
            var obj = batchBL.GetBatchDetailsByBatchNo(BatchNo);
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
            return (batchModel);
        }

        [HttpPost]
        [Route("PostCounterSales")]
        public HttpResponseMessage PostCounterSales([FromBody]CounterSalesModel modal)
        {
            try
            {
                CounterSalesBO counterSales = new CounterSalesBO();
                counterSales.ID = modal.ID;
                counterSales.TransNo = modal.TransNo;
                counterSales.TransDate = General.ToDateTime(modal.TransDate);
                counterSales.DoctorID = modal.DoctorID;
                counterSales.PartyName = modal.PartyName;
                counterSales.PatientID = 0;
                counterSales.WarehouseID = 1;
                counterSales.NetAmount = modal.NetAmount;
                counterSales.IsDraft = false;
                counterSales.SGSTAmount = modal.SGSTAmount;
                counterSales.CGSTAmount = modal.CGSTAmount;
                counterSales.IGSTAmount = modal.IGSTAmount;
                counterSales.PackingPrice = //0;
                counterSales.RoundOff = modal.RoundOff;
                counterSales.TotalAmountReceived = modal.TotalAmountReceived;
                counterSales.PaymentModeID = 1;//cash ntet id 
                counterSales.BalanceToBePaid = 0;
                counterSales.CessAmount = modal.CessAmount;
                counterSales.EmployeeID = 0;
                counterSales.TypeID = 3;
                counterSales.GrossAmount = modal.GrossAmount;
                counterSales.TaxableAmt = modal.TaxableAmt;
                counterSales.BankID = 1;
                counterSales.DoctorName = modal.DoctorName == null ? "" : modal.DoctorName;
                counterSales.DiscountCategoryID = 1;
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
                    counterSalesItemBO.BasicPrice = Items.BasicPrice;
                    counterSalesItemBO.GrossAmount = Items.GrossAmount;
                    counterSalesItemBO.SGSTPercentage = Items.SGSTPercentage;
                    counterSalesItemBO.CGSTPercentage = Items.CGSTPercentage;
                    counterSalesItemBO.IGSTPercentage = Items.IGSTPercentage;
                    counterSalesItemBO.SGSTAmount = Items.SGSTAmount;
                    counterSalesItemBO.CGSTAmount = Items.CGSTAmount;
                    counterSalesItemBO.IGSTAmount = Items.IGSTAmount;
                    counterSalesItemBO.NetAmount = Items.NetAmount;
                    counterSalesItemBO.BatchTypeID = 1;
                    counterSalesItemBO.WareHouseID = 1;
                    counterSalesItemBO.TaxableAmount = Items.TaxableAmount;
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
                    counterSalesAmountDetailsBO.Amount = items.Amount;
                    counterSalesAmountDetailsBO.Particulars = items.Particulars;
                    counterSalesAmountDetailsBO.Percentage = items.Percentage;
                    amountDtails.Add(counterSalesAmountDetailsBO);
                }
                var ID = counterSalesBL.SaveCounterSalesInvoice(counterSales, ItemList, amountDtails);
                var message = Request.CreateResponse(HttpStatusCode.Created);
                message.Headers.Location = new Uri(Request.RequestUri + ID.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }

        [HttpPost]
        [Route("PostCounterSalestest")]
        public IHttpActionResult PostCounterSalestest(CounterSalesModel modal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            GeneralBO.FinYear = 2019;
            GeneralBO.ApplicationID = 1;
            GeneralBO.LocationID = 1;
            GeneralBO.CreatedUserID = 616;
            CounterSalesBO counterSales = new CounterSalesBO();
            counterSales.ID = modal.ID;
            counterSales.TransNo = modal.TransNo;
            counterSales.TransDate = General.ToDateTime(modal.TransDate);
            counterSales.DoctorID = modal.DoctorID;
            counterSales.PartyName = modal.PartyName;
            counterSales.PatientID = modal.PatientID;//0
            counterSales.WarehouseID = modal.WarehouseID;//1
            counterSales.NetAmount = modal.NetAmount;
            counterSales.IsDraft = modal.IsDraft;//0
            counterSales.SGSTAmount = modal.SGSTAmount;
            counterSales.CGSTAmount = modal.CGSTAmount;
            counterSales.IGSTAmount = modal.IGSTAmount;
            counterSales.PackingPrice = modal.PackingPrice;//0
            counterSales.RoundOff = modal.RoundOff;
            counterSales.TotalAmountReceived = modal.TotalAmountReceived;
            counterSales.PaymentModeID = modal.PaymentModeID;//cash ntet id 
            counterSales.BalanceToBePaid = modal.BalanceToBePaid;//0
            counterSales.CessAmount = modal.CessAmount;
            counterSales.EmployeeID = modal.EmployeeID;//0
            counterSales.TypeID = modal.TypeID;//0
            counterSales.GrossAmount = modal.GrossAmount;
            counterSales.TaxableAmt = modal.TaxableAmt;
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
                counterSalesItemBO.BasicPrice = Items.BasicPrice;
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
                counterSalesItemBO.CessAmount = Items.CessAmount;
                counterSalesItemBO.CessPercentage = Items.CessPercentage;
                ItemList.Add(counterSalesItemBO);
            }

            var amountDtails = new List<CounterSalesAmountDetailsBO>();
            CounterSalesAmountDetailsBO counterSalesAmountDetailsBO;
            foreach (var items in modal.AmountDetails)
            {
                counterSalesAmountDetailsBO = new CounterSalesAmountDetailsBO();
                counterSalesAmountDetailsBO.Amount = items.Amount;
                counterSalesAmountDetailsBO.Particulars = items.Particulars;
                counterSalesAmountDetailsBO.Percentage = items.Percentage;
                amountDtails.Add(counterSalesAmountDetailsBO);
            }
            var ID = counterSalesBL.SaveCounterSalesInvoice(counterSales, ItemList, amountDtails);
            return Ok();
        }
    }
}
