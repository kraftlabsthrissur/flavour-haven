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
    public class PaymentReturnVoucherController : Controller
    {
        #region Private members
        private ISupplierContract supplierBL;
        private ITreasuryContract treasuryBL;
        private IGeneralContract generalBL;
        private IPaymentTypeContract paymentTypeBL;
        private IPaymentReturnVoucherContract paymentReturnVoucherBL;
        private IPayment _iPayment;
        #endregion

        #region Constructor
        public PaymentReturnVoucherController()
        {
            supplierBL = new SupplierBL();
            paymentTypeBL = new PaymentTypeBL();
            treasuryBL = new TreasuryBL();
            generalBL = new GeneralBL();
            paymentReturnVoucherBL = new PaymentReturnVoucherBL();

        }
        #endregion
        // GET: Accounts/PaymentReturnVoucher
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            PaymentReturnVoucherModel paymentReturnVoucher = new PaymentReturnVoucherModel();
            paymentReturnVoucher.VoucherNo = generalBL.GetSerialNo("PaymentReturnVoucher", "Code");
            paymentReturnVoucher.VoucherDate = General.FormatDate(DateTime.Now);
            paymentReturnVoucher.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            paymentReturnVoucher.BankList = new SelectList(treasuryBL.GetBankList(), "ID", "BankName");
            return View(paymentReturnVoucher);
        }
        public ActionResult Details(int  id)
        {
            PaymentReturnVoucherModel paymentReturn = paymentReturnVoucherBL.GetpaymentReturnVoucherDetails(id).Select(m => new PaymentReturnVoucherModel()
            {
                ID = m.ID,
                VoucherNo = m.VoucherNo,
                VoucherDate = General.FormatDate(m.VoucherDate, "view"),
                SupplierName = m.SupplierName,
                PaymentTypeName = m.PaymentTypeName,
                BankName = m.BankName,
                Amount = m.Amount,
                BankReferenceNumber = m.BankReferenceNumber,
                IsDraft = m.IsDraft
            }).First();
            paymentReturn.Items = paymentReturnVoucherBL.GetPaymentReturnTransDetails(id).Select(m => new PaymentReturnVoucherItemModel()
            {
                Amount = m.Amount,
                Remarks = m.Remarks
            }).ToList();
            return View(paymentReturn);
        }

        public ActionResult Edit(int id)
        {

            PaymentReturnVoucherModel paymentReturn = paymentReturnVoucherBL.GetpaymentReturnVoucherDetails(id).Select(m => new PaymentReturnVoucherModel()
            {
                ID = m.ID,
                VoucherNo = m.VoucherNo,
                VoucherDate = General.FormatDate(m.VoucherDate),
                SupplierName = m.SupplierName,
                SupplierID = m.SupplierID,
                PaymentTypeName = m.PaymentTypeName,
                PaymentTypeID = m.PaymentTypeID,
                BankName = m.BankName,
                BankID = m.BankID,
                Amount = m.Amount,
                BankReferenceNumber = m.BankReferenceNumber,
                IsDraft = m.IsDraft
            }).First();
            if (!paymentReturn.IsDraft)
            {
                return RedirectToAction("Index");
            }
            paymentReturn.Items = paymentReturnVoucherBL.GetPaymentReturnTransDetails(id).Select(m => new PaymentReturnVoucherItemModel()
            {
                Amount = m.Amount,
                Remarks = m.Remarks
            }).ToList();
            paymentReturn.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
        
            paymentReturn.BankList = new SelectList(treasuryBL.GetBankList(), "ID", "BankName");
            return View(paymentReturn);
        }
        [HttpPost]
        public ActionResult Save(PaymentReturnVoucherModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    PaymentReturnVoucherBO Temp = paymentReturnVoucherBL.GetpaymentReturnVoucherDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                PaymentReturnVoucherBO paymentreturn = new PaymentReturnVoucherBO()
                {
                    ID = model.ID,
                    VoucherNo = model.VoucherNo,
                    VoucherDate = General.ToDateTime(model.VoucherDate),
                    SupplierID = model.SupplierID,
                    PaymentTypeID = model.PaymentTypeID,
                    BankID = model.BankID,
                    BankReferenceNumber = model.BankReferenceNumber,
                    IsDraft = model.IsDraft,
                    DebitNoteID = model.DebitNoteID,
                    TransNo = model.TransNo,
                    DebitAccountCode = model.DebitAccountCode
                };
                List<PaymentReturnVoucherItemBO> ItemList = new List<PaymentReturnVoucherItemBO>();
                PaymentReturnVoucherItemBO paymentreturnItem;
                foreach (var item in model.Items)
                {
                    paymentreturnItem = new PaymentReturnVoucherItemBO()
                    {
                        Amount = item.Amount,
                        Remarks = item.Remarks
                    };
                    ItemList.Add(paymentreturnItem);
                }
                    paymentReturnVoucherBL.Save(paymentreturn, ItemList);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "PaymentReturnVoucher", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(PaymentReturnVoucherModel model)
        {
            return Save(model);
        }

        public JsonResult GetPaymentReturnVoucherList(DatatableModel Datatable)
        {
            try
            {
                string VoucherNoHint = Datatable.Columns[1].Search.Value;
                string VoucherDateHint = Datatable.Columns[2].Search.Value;
                string SupplierNameHint = Datatable.Columns[3].Search.Value;
                string PaymentHint = Datatable.Columns[4].Search.Value;
                string ReturnAmountHint = Datatable.Columns[5].Search.Value;


                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = paymentReturnVoucherBL.GetPaymentReturnVoucherList(Type, VoucherNoHint, VoucherDateHint, SupplierNameHint, PaymentHint, ReturnAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PaymentReturnVoucherPrintPdf(int Id)
        {
            return null;
        }

        public PartialViewResult GetUnSettledPurchaseInvoicesV3(int AccountHeadID)
        {
            var unsettledPurchaseInvoiceList = _iPayment.GetPayableDetailsForPaymentVoucherV3(AccountHeadID);
            return PartialView("~/Areas/Accounts/Views/PaymentReturnVoucher/_unSettledPurchaseInvoice.cshtml", unsettledPurchaseInvoiceList);
        }

        [HttpGet]
        public JsonResult GetDebitNoteListForPaymentReturn(int id)
        {
            try
            {
                PaymentReturnVoucherModel paymentReturnVoucherModel = new PaymentReturnVoucherModel();

                paymentReturnVoucherModel.DebitNoteList = paymentReturnVoucherBL.GetDebitNoteListForPaymentReturn(id).Select(m => new DebitNoteModel()
                {
                    ID = m.ID,
                    TransNo = m.TransNo,
                    TransDate = General.FormatDate(m.TransDate),
                    DebitAccount = m.DebitAccount,
                    DebitAccountCode = m.DebitAccountCode,
                    Amount = m.Amount
                }
                ).ToList();
                return Json(paymentReturnVoucherModel.DebitNoteList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

    }
}