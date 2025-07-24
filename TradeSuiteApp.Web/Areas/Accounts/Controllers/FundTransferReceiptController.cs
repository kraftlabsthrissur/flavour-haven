using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class FundTransferReceiptController : Controller
    {
        private IGeneralContract generalBL;
        private ILocationContract locationBL;
        private ITreasuryContract treauryBL;
        private IFundTransferReceiptContract fundreceiptBL;

        public FundTransferReceiptController()
        {
            generalBL = new GeneralBL();
            locationBL = new LocationBL();
            treauryBL = new TreasuryBL();
            fundreceiptBL = new FundTransferReceiptBL();
        }
        // GET: Accounts/FundTransferReceipt
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            FundTransferReceiptModel model = new FundTransferReceiptModel();
            model.ToLocationID = GeneralBO.LocationID;
            model.TransNo = generalBL.GetSerialNo("FundTransferReceipt", "Code");
            model.FromLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.ToLocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.FromBankList = new SelectList(treauryBL.GetBank(), "ID", "BankName");
            model.ToBankList = new SelectList(treauryBL.GetBank(), "ID", "BankName");
            return View(model);
        }

        public JsonResult GetFundTransferIssueList(DatatableModel Datatable)
        {
            try
            {
                string IssueTransNoHint = Datatable.Columns[2].Search.Value;
                string IssueLocationHint = Datatable.Columns[3].Search.Value;
                string IssueBankDetailsHint = Datatable.Columns[4].Search.Value;
                string ReceiptLocationHint = Datatable.Columns[5].Search.Value;
                string ReceiptBankDetailsHint = Datatable.Columns[6].Search.Value;
                string ModeOfPaymentHint = Datatable.Columns[7].Search.Value;
                string AmountHint = Datatable.Columns[8].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int IssueLocationID = Convert.ToInt32(Datatable.GetValueFromKey("IssueLocationID", Datatable.Params));
                DatatableResultBO resultBO = fundreceiptBL.GetFundTransferIssueList(IssueLocationID, IssueTransNoHint, IssueLocationHint, IssueBankDetailsHint, ReceiptLocationHint, ReceiptBankDetailsHint, ModeOfPaymentHint, AmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTransferIssuedItems(int IssueID)
        {
            try
            {
                List<FundTransferReceiptBO> Items = fundreceiptBL.GetTransferIssuedItems(IssueID);
                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Save(FundTransferReceiptModel Model)
        {
            try
            {
                FundTransferReceiptBO ReceiptBO = new FundTransferReceiptBO()
                {
                    ID = Model.ID,
                    TransNo = Model.TransNo,
                    TransDate = General.ToDateTime(Model.TransDate),
                };

                List<FundTransferItemBO> Items = new List<FundTransferItemBO>();
                FundTransferItemBO ReceiptItem;

                foreach (var item in Model.Items)
                {
                    ReceiptItem = new FundTransferItemBO()
                    {
                        IssueTransID = item.IssueTransID,
                        FromLocationID = item.FromLocationID,
                        FromBankID = item.FromBankID,
                        ToLocationID = item.ToLocationID,
                        ToBankID = item.ToBankID,
                        ModeOfPayment = item.ModeOfPayment,
                        Amount = item.Amount
                    };
                    Items.Add(ReceiptItem);
                }
                fundreceiptBL.Save(ReceiptBO, Items);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            FundTransferReceiptModel model = fundreceiptBL.GetFundTransferReceiptByID(ID).Select(m => new FundTransferReceiptModel()
            {

                TransDate = General.FormatDate(m.TransDate),
                TransNo = m.TransNo,
            }).First();

            model.Items = fundreceiptBL.GetFundTransferReceiptTransByID(ID).Select(m => new FundTransferItemModel()
            {
                FromLocationName = m.FromLocationName,
                ToLocationName = m.ToLocationName,
                Payment = m.Payment,
                Amount = m.Amount,
                FromBankName = m.FromBankName,
                ToBankName = m.ToBankName,
            }).ToList();
            return View(model);
        }

        public JsonResult GetFundTransferReceipt(DatatableModel Datatable)
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

                DatatableResultBO resultBO = fundreceiptBL.GetFundTransferReceipt(FundTransferNo, FundTransferDate, FromLocation, ToLocation, ModeOfPayment, TotalAmount, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FundTransferReceiptPrintPdf(int Id)
        {
            return null;
        }

    }
}



