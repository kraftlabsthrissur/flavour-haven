using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;   
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class AdvanceReceiptController : Controller
    {
        private ITreasuryContract treasuryBL;
        private IPaymentTypeContract paymentTypeBL;
        private IAdvanceReceiptContract advanceReceiptBL;
        private IGeneralContract generalBL;
        private IDropdownContract _dropdown;

        public AdvanceReceiptController(IDropdownContract IDropdown)
        {
            treasuryBL = new TreasuryBL();
            paymentTypeBL = new PaymentTypeBL();
            advanceReceiptBL = new AdvanceReceiptBL();
            generalBL = new GeneralBL();
            _dropdown = IDropdown;
        }

        public ActionResult Index()
        {
            List<AdvanceReceiptModel> ReceiptList = new List<AdvanceReceiptModel>();
            ReceiptList = advanceReceiptBL.GetReceiptList().Select(a => new AdvanceReceiptModel
            {
                ID = a.ID,
                AdvanceReceiptNo = a.AdvanceReceiptNo,
                AdvanceReceiptDate = General.FormatDate(a.AdvanceReceiptDate),
                CustomerName = a.CustomerName,
                NetAmount = a.NetAmount
            }).ToList();

            return View(ReceiptList);
        }

        public ActionResult Details(int id)
        {
            AdvanceReceiptModel advanceReceiptModel = advanceReceiptBL.GetAdvanceReceiptDetails((int)id).Select(k => new AdvanceReceiptModel()
            {
                ID = k.ID,
                AdvanceReceiptNo = k.AdvanceReceiptNo,
                AdvanceReceiptDate = General.FormatDate(k.AdvanceReceiptDate, "view"),
                CustomerName = k.CustomerName,
                PaymentTypeName = k.PaymentTypeName,
                BankName = k.BankName,
                ReferenceNo = k.ReferenceNo,
                NetAmount = k.NetAmount,

            }
             ).First();

            advanceReceiptModel.Items = advanceReceiptBL.GetAdvanceReceiptTransDetails((int)id).Select(k => new AdvanceReceiptItem()
            {
                ID = k.ID,
                SalesOrderDate = General.FormatDate(k.SalesOrderDate, "view"),
                ItemName = k.ItemName,
                Amount = k.Amount,
                TDSCode = k.TDSCode,
                TDSAmount = k.TDSAmount,
                TransNo = k.TransNo,
                NetAmount = k.Amount - k.TDSAmount,
                Remarks = k.Remarks

            }).ToList();
            return View(advanceReceiptModel);
        }

        public ActionResult Create()
        {
            AdvanceReceiptModel advanceReceiptModel = new Models.AdvanceReceiptModel();
            advanceReceiptModel.BankList = new SelectList(treasuryBL.GetBank("Receipt", ""), "ID", "BankName");
            advanceReceiptModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", advanceReceiptModel.PaymentTypeID);
            advanceReceiptModel.AdvanceReceiptDate = General.FormatDate(DateTime.Now);
            advanceReceiptModel.AdvanceReceiptNo = generalBL.GetSerialNo("AdvanceReceipt", "Code");
            return View(advanceReceiptModel);
        }

        public PartialViewResult GetSalesOrders(int CustomerID)
        {
            ViewBag.TDSCodes = new SelectList(_dropdown.GetTDS(), "Description", "Rate");
            List<SalesOrderBO> SalesOrderBOList = new List<SalesOrderBO>();
            SalesOrderBOList = advanceReceiptBL.GetSalesOrders(CustomerID);
            return PartialView("_unProcessedItems", SalesOrderBOList);
        }

        public JsonResult GetItemNamesForSalesOrder(int SalesID, string TransNo, string search)
        {
            List<SalesOrderBO> ItemList = new List<SalesOrderBO>();
            ItemList = advanceReceiptBL.GetItemNamesForSalesOrder(SalesID, TransNo, search);
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(AdvanceReceiptModel model)
        {
            try
            {
                AdvanceReceiptBO advanceReceiptBO = new AdvanceReceiptBO()
                {
                    AdvanceReceiptNo = model.AdvanceReceiptNo,
                    AdvanceReceiptDate = General.ToDateTime(model.AdvanceReceiptDate),
                    CustomerID = model.CustomerID,
                    PaymentTypeID = model.PaymentTypeID,
                    BankName = model.BankName,
                    ReferenceNo = model.ReferenceNo,
                    Amount = model.Amount,
                    TDSAmount = model.TDSAmount,
                    NetAmount = model.NetAmount

                };

                List<AdvanceReceiptItemBO> ReceiptItems = new List<AdvanceReceiptItemBO>();
                AdvanceReceiptItemBO AdvanceReceiptItems;
                foreach (var item in model.Items)
                {
                    AdvanceReceiptItems = new AdvanceReceiptItemBO()
                    {
                        TransNo = item.TransNo,
                        SalesOrderDate = General.ToDateTime(item.SalesOrderDate),
                        ItemID = item.ItemID,
                        TDSID = item.TDSID,
                        Remarks = item.Remarks,
                        ItemAmount = item.ItemAmount

                    };

                    ReceiptItems.Add(AdvanceReceiptItems);
                }
                advanceReceiptBL.Save(advanceReceiptBO, ReceiptItems);

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
                generalBL.LogError("Accounts", "AdvanceReceipt", "Save", model.ID, e);
                return
                      Json(new
                      {
                          Status = "",
                          data = "",
                          message = e.Message
                      }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Edit(int id)
        {

            AdvanceReceiptModel advanceReceiptModel = advanceReceiptBL.GetAdvanceReceiptDetails((int)id).Select(k => new AdvanceReceiptModel()
            {
                ID = k.ID,
                AdvanceReceiptNo = k.AdvanceReceiptNo,
                AdvanceReceiptDate = General.FormatDate(k.AdvanceReceiptDate, "view"),
                CustomerName = k.CustomerName,
                PaymentTypeID = k.PaymentTypeID,
                PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", k.PaymentTypeID),
                BankList = new SelectList(treasuryBL.GetBank("Receipt", ""), "ID", "BankName"),
                ReferenceNo = k.ReferenceNo,
                NetAmount = k.NetAmount,
                BankName = k.BankName
            }
           ).First();
            advanceReceiptModel.Items = advanceReceiptBL.GetAdvanceReceiptTransDetails((int)id).Select(k => new AdvanceReceiptItem()
            {
                ID = k.ID,
                SalesOrderDate = General.FormatDate(k.SalesOrderDate, "view"),
                ItemName = k.ItemName,
                Amount = k.Amount,
                TDSCode = k.TDSCode,
                TDSAmount = k.TDSAmount,
                TransNo = k.TransNo,
                NetAmount = k.Amount - k.TDSAmount,
                Remarks = k.Remarks,
                Rate = k.TDSValue



            }).ToList();
            ViewBag.TDSCodes = new SelectList(_dropdown.GetTDS(), "Description", "Rate");
            return View(advanceReceiptModel);
        }
    }
}