using BusinessLayer;
using BusinessObject;
using Microsoft.AspNet.Identity;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts
{
    public class FundTransferController : Controller
    {
        #region Private members
        private ITreasuryContract treauryBL;
        private IFundTransferContract fundtranferBL;
        private IPaymentModeContract paymentModeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        #endregion

        #region Constructor
        public FundTransferController(IDropdownContract IDropdown)
        {
            fundtranferBL = new FundTransferBL();
            treauryBL = new TreasuryBL();
            paymentModeBL = new PaymentModeBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
        }
        #endregion

        #region Public methods
        // GET: Accounts/FundTransfer
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft" };
            return View();
        }

        // GET: Accounts/FundTransfer/Create
        public ActionResult Create()
        {
            FundTransferModel fundTransfer = new FundTransferModel();
            fundTransfer.FromLocationID = GeneralBO.LocationID;
            fundTransfer.TransNo = generalBL.GetSerialNo("FundTransfer", "Code");
            fundTransfer.InstrumentDate = General.FormatDate(DateTime.Now);

            fundTransfer.FromLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            fundTransfer.ToLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            fundTransfer.ModeOfPaymentList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");

            fundTransfer.ToLocationID = GeneralBO.LocationID;
            fundTransfer.FromBankList = treauryBL.GetBank().Select(a => new TreasuryModel()
            {
                ID = a.ID,
               BankName=a.BankName,
               CreditBalance=a.CreditBalance
            }).ToList();
            fundTransfer.ModeOfPaymentID = 1;
            fundTransfer.ToBankList = new SelectList(treauryBL.GetBank(), "ID", "BankName");
            return View(fundTransfer);

        }
        // POST: Accounts/FundTransfer/Create
        [HttpPost]
        public ActionResult Save(FundTransferModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    FundTranferBO Temp = fundtranferBL.GetFundTransferDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                FundTranferBO fundtransfer = new FundTranferBO()
                {
                    ID=model.ID,
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    TotalAmount = model.TotalAmount,
                    IsDraft = model.IsDraft
                };
                List<FundTranferTransBO> ItemList = new List<FundTranferTransBO>();
                FundTranferTransBO fundtransferItem;
                foreach (var item in model.Items)
                {
                    fundtransferItem = new FundTranferTransBO()
                    {
                        FundTransferID = item.FundTransferID,
                        FromLocationID = item.FromLocationID,
                        ToLocationID = item.ToLocationID,
                        FromBankID = item.FromBankID,
                        ToBankID = item.ToBankID,
                        Amount = item.Amount,
                        ModeOfPaymentID = item.ModeOfPaymentID,
                        InstrumentNumber = item.InstrumentNumber,
                        InstrumentDate = General.ToDateTime(item.InstrumentDate),
                        Remarks = item.Remarks,
                    };
                    ItemList.Add(fundtransferItem);
                }
                if (model.ID == 0)
                {
                    fundtranferBL.Save(fundtransfer, ItemList);
                }
                else
                {
                    fundtranferBL.Update(fundtransfer, ItemList);
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "FundTransfer", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveAsDraft(FundTransferModel model)
        {
            return Save(model);
        }

        //GET :Accounts/FundTransfer/Details
        public ActionResult Details(int id)
        {

            FundTransferModel fundTransfer = fundtranferBL.GetFundTransferDetails(id).Select(m => new FundTransferModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date, "view"),
                TotalAmount = m.TotalAmount,
                IsDraft=m.IsDraft
                
            }).First();
            fundTransfer.Items = fundtranferBL.GetFundTransferTransDetails(id).Select(m => new FundTransferTransModel()
            {
                FromLocation = m.FromLocation,
                ToLocation = m.ToLocation,
                FromBank = m.FromBank,
                ToBank = m.ToBank,
                Amount = m.Amount,
                ModeOfPayment = m.ModeOfPayment,
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate, "view"),
                Remarks = m.Remarks

            }).ToList();

            return View(fundTransfer);
        }

        public JsonResult GetLocationWiseBank(int locationid)
        {
            List<FundTransferModel> itemList = new List<FundTransferModel>();
            itemList = fundtranferBL.GetLocationWiseBank(locationid).Select(a => new FundTransferModel()
            {
                ID = a.ID,
                BankName = a.BankName
            }).ToList();

            return Json(new { Status = "success", data = itemList }, JsonRequestBehavior.AllowGet);
        }
    
        [HttpGet]
        public ActionResult Edit(int id)
        {
            FundTransferModel fundTransfer = fundtranferBL.GetFundTransferDetails(id).Select(m => new FundTransferModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date, "view"),
                TotalAmount = m.TotalAmount,
                IsDraft=m.IsDraft
            }).First();
            if(!fundTransfer.IsDraft)
            {
                return RedirectToAction("Index");
            }
            fundTransfer.Items = fundtranferBL.GetFundTransferTransDetails(id).Select(m => new FundTransferTransModel()
            {
                FromLocation = m.FromLocation,
                FromLocationID = m.FromLocationID,
                ToLocation = m.ToLocation,
                ToLocationID = m.ToLocationID,
                FromBank = m.FromBank,
                FromBankID = m.FromBankID,
                ToBank = m.ToBank,
                ToBankID = m.ToBankID,
                Amount = m.Amount,
                ModeOfPayment = m.ModeOfPayment,
                ModeOfPaymentID = m.ModeOfPaymentID,
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate),
                Remarks = m.Remarks,
                CreditBalance=m.CreditBalance

            }).ToList();
            fundTransfer.FromLocationID = GeneralBO.LocationID;
            fundTransfer.FromLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            fundTransfer.ToLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            fundTransfer.ModeOfPaymentList = new SelectList(paymentModeBL.GetPaymentModeList(), "ID", "Name");
            fundTransfer.FromBankList = treauryBL.GetBank().Select(a => new TreasuryModel()
            {
                ID = a.ID,
                BankName = a.BankName,
                CreditBalance = a.CreditBalance
            }).ToList();
            fundTransfer.ToBankList = new SelectList(treauryBL.GetBank(), "ID", "BankName");
            return View(fundTransfer);
        }

        public JsonResult GetFundTransferList(DatatableModel Datatable)
        {
            try
            {
                string FundTransferNo = Datatable.Columns[1].Search.Value;
                string FundTransferDate = Datatable.Columns[2].Search.Value;
                string FromLocation = Datatable.Columns[3].Search.Value;
                string ToLocation = Datatable.Columns[4].Search.Value;
                string ModeOfPayment = Datatable.Columns[5].Search.Value;
                string TotalAmount = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = fundtranferBL.GetFundTransferList(Type, FundTransferNo, FundTransferDate, FromLocation, ToLocation, ModeOfPayment, TotalAmount, SortField, SortOrder, Offset, Limit);
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

        public JsonResult FundTransferPrintPdf(int Id)
        {
            return null;
        }
            #endregion

        }
}