using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers   
{
    public class AdvancePaymentController : Controller
    {
        #region Private members
        private IDropdownContract _dropdown;
        private IPayment _iPayment;
        private IAdvancePayment iAdvancePayment;
        private IPaymentModeContract paymentModeBL;
        private IPaymentTypeContract paymentTypeBL;
        private IPaymentVoucher paymentVoucherBL;
        private ITreasuryContract treasuryBL;
        private ISupplierContract supplierBL;
        private IGeneralContract generalBL;
        private IConfigurationContract configBL;

        #endregion


        #region Constructor
        public AdvancePaymentController(IDropdownContract IDropdown, IPayment iPayment)
        {
            _dropdown = IDropdown;

            _iPayment = iPayment;
            iAdvancePayment = new AdvancePaymentBL();
            paymentVoucherBL = new PaymentVoucherBL();
            paymentModeBL = new PaymentModeBL();
            paymentTypeBL = new PaymentTypeBL();
            treasuryBL = new TreasuryBL();
            supplierBL = new SupplierBL();
            generalBL = new GeneralBL();
            configBL = new ConfigurationBL();
        }
        #endregion

        #region Public methods
        // GET: Accounts/Payment
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }
        public ActionResult Details(int id)
        {

            AdvancePaymentModel advancePaymentModel = iAdvancePayment.GetAdvancePaymentDetails((int)id).Select(k => new AdvancePaymentModel()
            {
                ID = k.ID,
                AdvancePaymentNo = k.AdvancePaymentNo,
                AdvancePaymentDate = General.FormatDate(k.AdvancePaymentDate, "view"),
                Categories = GetCategories(),
                Category = k.Category,
                SelectedName = k.SelectedName,
                Amount = k.Amt,
                ModeOfPaymentName = k.ModeOfPaymentName,
                BankDetail = k.BankDetail,
                ReferenceNo = k.ReferenceNo,
                NetAmount = k.NetAmount,
                Purpose = k.Purpose,
                IsDraft = k.Draft
            }
             ).First();

            advancePaymentModel.List = iAdvancePayment.GetAdvancePaymentTransDetails((int)id).Select(k => new AdvancePaymentList()
            {
                ID = k.ID,
                PurchaseOrderDate = General.FormatDate(k.PurchaseOrderDate, "view"),
                PurchaseOrderTerms = k.PurchaseOrderTerms,
                ItemName = k.ItemName,
                Amount = k.Amount,
                TDSCode = k.TDSCode,
                TDSAmount = k.TDSAmount,
                TransNo = k.TransNo,
                NetAmount = k.Amount - k.TDSAmount,
                Remarks = k.Remarks,
                Advance = k.Advance

            }).ToList();
            return View(advancePaymentModel);
        }
        public ActionResult Edit(int id)
        {

            AdvancePaymentModel advancePaymentModel = iAdvancePayment.GetAdvancePaymentDetails((int)id).Select(k => new AdvancePaymentModel()
            {
                ID = k.ID,
                AdvancePaymentNo = k.AdvancePaymentNo,
                AdvancePaymentDate = General.FormatDate(k.AdvancePaymentDate),
                Categories = GetCategories(),
                Category = k.Category,
                SelectedName = k.SelectedName,
                Amount = k.Amt,
                ModeOfPaymentName = k.ModeOfPaymentName,
                BankName = k.BankDetail,
                ReferenceNo = k.ReferenceNo,
                NetAmount = k.NetAmount,
                Purpose = k.Purpose,
                IsDraft = k.Draft,
                SupplierID = k.SupplierID,
                EmployeeID = k.EmployeeID,
                PaymentTypeID = k.ModeOfPaymentID,


            }
             ).First();
            if (!advancePaymentModel.IsDraft)
            {
                return RedirectToAction("Index");
            }

            advancePaymentModel.List = iAdvancePayment.GetAdvancePaymentTransDetails((int)id).Select(k => new AdvancePaymentList()
            {
                ID = k.ID,
                PurchaseOrderDate = General.FormatDate(k.PurchaseOrderDate, "view"),
                PurchaseOrderTerms = k.PurchaseOrderTerms,
                ItemName = k.ItemName,
                Amount = k.Amount,
                TDSCode = k.TDSCode,
                TDSAmount = k.TDSAmount,
                TransNo = k.TransNo,
                NetAmount = k.Amount - k.TDSAmount,
                Remarks = k.Remarks
            }).ToList();
            advancePaymentModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", advancePaymentModel.PaymentTypeID);
            advancePaymentModel.CashPaymentLimit = configBL.GetCashPayementLimit();

            advancePaymentModel.BankList = treasuryBL.GetBank("Payment", advancePaymentModel.ModeOfPaymentName == "CASH" ? "Cash" : "Bank").Select(
            a => new TreasuryModel()
            {
                BankName = a.BankName,
                ID = a.ID,
                CreditBalance = a.CreditBalance
            }).ToList();

            return View(advancePaymentModel);
        }
        public JsonResult GetNamesByCategory(string category, string search)
        {
            //Hardcoded. Need to change.
            int offset = 1;        //Hardcoded. Need to change.
            int limit = 20;         //Hardcoded. Need to change.

            var details = _iPayment.GetNameByCategory(offset: offset, limit: limit, category: category, search: search);
            return Json(details, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllSupplierAutoComplete(string term = "")
        {
            List<SupplierModel> supplierList = new List<SupplierModel>();
            supplierList = supplierBL.GetAllSupplierAutoComplete(term).Select(a => new SupplierModel()
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                Location = a.Location,
                SupplierCategoryID = a.SupplierCategoryID,
                StateID = a.StateID,
                IsGSTRegistered = a.IsGSTRegistered,
            }).ToList();

            return Json(supplierList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get the ItemName auto complete inside the grid after selecting Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public JsonResult GetItemNamesByPurchaseOrder(int PurchaseID, string TransNo, string search)
        {

            var items = _iPayment.GetItemByPurchaseOrder(PurchaseID, TransNo, search);
            return Json(items, JsonRequestBehavior.AllowGet);

        }
        public PartialViewResult GetAdvanceRequest(int EmployeeID, int IsOfficial)
        {
            List<AdvancePaymentAdvanceRequestTransModel> items = new List<AdvancePaymentAdvanceRequestTransModel>();

            items = iAdvancePayment.GetAdvanceRequest(EmployeeID, IsOfficial).Select(a => new AdvancePaymentAdvanceRequestTransModel()
            {
                ItemName = a.ItemName,
                Amount = a.Amount,
                AdvanceRequestDateStr = General.FormatDate(a.AdvanceRequestDate),
                ItemID = a.ItemID,
                AdvanceRequestNo = a.AdvanceRequestNo,
                ID = a.ID,
                IsDraft = true,
                Advance = a.AdvanceAmount
            }).ToList();

            return PartialView(items);

        }
        public PartialViewResult GetAdvanceRequestForEdit(int ID)
        {
            List<AdvancePaymentAdvanceRequestTransModel> items = new List<AdvancePaymentAdvanceRequestTransModel>();

            items = iAdvancePayment.GetAdvanceRequestForEdit(ID).Select(a => new AdvancePaymentAdvanceRequestTransModel()
            {
                ItemName = a.ItemName,
                Amount = a.Amount,
                AdvanceRequestDateStr = General.FormatDate(a.AdvanceRequestDate),
                ItemID = a.ItemID,
                AdvanceRequestNo = a.AdvanceRequestNo,
                ID = a.ID,
                IsDraft = true,
                AdvDetAmount = a.AdvDetAmount,
                Advance = a.AdvanceAmount,
                Remarks=a.Remarks
            }).ToList();

            return PartialView(items);

        }
        private List<SelectListItem> GetCategories()
        {
            return new List<SelectListItem>() { new SelectListItem() { Text = "Supplier", Value = "Supplier" }, new SelectListItem() { Text = "Employee", Value = "Employee" } };
        }
        [HttpPost]
        public ActionResult Save(AdvancePaymentBO model, List<AdvancePaymentPurchaseOrderBO> trans)
        {

            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    AdvancePaymentBO Temp = iAdvancePayment.GetAdvancePaymentDetails(model.ID).FirstOrDefault();
                    if (!Temp.Draft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                //Hardcoded, need to change
                //    model.AdvancePaymentDate =General.ToDateTime(model.AdvancePaymentDate);

                //   var advancePaymentBO = advancePaymentModel.MapToBo();
                model.AdvancePaymentDate = General.ToDateTime(model.AdvancePaymentDateStr);
                trans = trans.Select(x => { x.PurchaseOrderDate = General.ToDateTime(x.PurchaseOrderDateStr); return x; }).ToList();
                var advancePaymentID = _iPayment.SaveAdvancePayment(model, trans);
                string messsage = advancePaymentID > 0 ? "Saved succesfully" : "Please try again";
                if (advancePaymentID > 0)
                {
                    return Json(new { Message = "Advance payment saved successfully", Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Add(new { ErrorMessage = "Failed to save advance payment" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "AdvancePayment", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveAsDraft(AdvancePaymentBO model, List<AdvancePaymentPurchaseOrderBO> trans)
        {
            return Save(model, trans);
        }
        // GET: Accounts/Payment/Create
        public ActionResult Create()
        {
            AdvancePaymentModel advancePaymentModel = new Models.AdvancePaymentModel();

            ViewBag.PaymentModes = paymentModeBL.GetPaymentModeList();
            advancePaymentModel.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", advancePaymentModel.PaymentTypeID);

            advancePaymentModel.BankList = treasuryBL.GetBank("Payment", "").Select(
         a => new TreasuryModel()
         {
             BankName = a.BankName,
             ID = a.ID,
             CreditBalance = a.CreditBalance
         }).ToList();
            advancePaymentModel.AdvancePaymentDate = General.FormatDate(DateTime.Now);
            advancePaymentModel.AdvancePaymentNo = generalBL.GetSerialNo("AdvancePayment", "Code");
            var bankList = _dropdown.GetBankDetails();
            advancePaymentModel.BankDetails = bankList.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.AccountNo + ", " + x.BankName)).ToList();
            advancePaymentModel.CashPaymentLimit = configBL.GetCashPayementLimit();
            advancePaymentModel.Categories = GetCategories();
            return View(advancePaymentModel);
        }

        public PartialViewResult GetPurchaseOrders(int SupplierID, string Category)
        {
            ViewBag.TDSCodes = new SelectList(_dropdown.GetTDS(), "Description", "Rate");
            List<AdvancePaymentPurchaseOrderModel> purchaseOrderBOList = new List<AdvancePaymentPurchaseOrderModel>();
            if (Category.ToLower().Equals("supplier"))
                purchaseOrderBOList = iAdvancePayment.GetPurchaseOrders(SupplierID).Select(m => new AdvancePaymentPurchaseOrderModel
                {
                    PurchaseOrderID = m.ID,
                    PurchaseOrderNo = m.TransNo,
                    PurchaseOrderDateStr = General.FormatDate(m.PurchaseOrderDate),
                    PaymentWithin = m.PaymentWithin,
                    AdvanceAmount = m.AdvanceAmount,
                    POAmount = m.POAmount,
                    IsDraft = false,
                    Advance = m.Advance,

                }).ToList();
            return PartialView("~/Areas/Accounts/Views/AdvancePayment/_unProcessedItems.cshtml", purchaseOrderBOList);
        }
        public PartialViewResult GetPurchaseOrdersAdvancePaymentForEdit(int ID)
        {
            ViewBag.TDSCodes = new SelectList(_dropdown.GetTDS(), "Description", "Rate");
            List<AdvancePaymentPurchaseOrderModel> purchaseOrderBOList = new List<AdvancePaymentPurchaseOrderModel>();

            purchaseOrderBOList = iAdvancePayment.GetPurchaseOrdersAdvancePaymentForEdit(ID).Select(m => new AdvancePaymentPurchaseOrderModel
            {
                PurchaseOrderID = m.ID,
                PurchaseOrderNo = m.TransNo,
                PurchaseOrderDateStr = General.FormatDate(m.PurchaseOrderDate),
                PaymentWithin = m.PaymentWithin,
                AdvanceAmount = m.AdvanceAmount,
                POAmount = m.POAmount,
                IsDraft = true,
                TDSAmount = m.TDSAmount,
                TDSID = m.TDSID,
                Amount = m.Amount,
                TDS = string.Concat(m.TDSID, "#", m.TDSRate),
                ItemID = m.ItemID,
                ItemName = m.ItemName,
                Advance = m.Advance,
                Remarks = m.Remarks

            }).ToList();
            return PartialView("~/Areas/Accounts/Views/AdvancePayment/_unProcessedItems.cshtml", purchaseOrderBOList);
        }

        public JsonResult GetAdvancePaymentList(int SupplierID, int EmployeeID)
        {
            List<AdvancePaymentModel> advancePayment = new List<AdvancePaymentModel>();

            advancePayment = iAdvancePayment.GetUnProcessedAdvancePaymentListSupplierWise(SupplierID, EmployeeID).Select(a => new AdvancePaymentModel()
            {
                ID = a.ID,
                AdvancePaymentNo = a.AdvancePaymentNo,
                AdvancePaymentDate = General.FormatDate(a.AdvancePaymentDate),
                SupplierName = a.SupplierName,
                Category = a.Category,
                Purpose = a.Purpose,
                Amount = a.Amt

            }).ToList();

            return Json(advancePayment, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUnprocessedAdvancePaymentTrans(int[] PaymentIDs)
        {

            AdvancePaymentModel advancePayment = new AdvancePaymentModel();
            //stockissueModel.UnProcesedSITransList = new List<StockIssueItemBO>();
            advancePayment.PaymentTrans = new List<AdvancePaymentList>();

            if (PaymentIDs.Length > 0)
            {
                foreach (var PaymentID in PaymentIDs)
                {
                    var list = iAdvancePayment.GetUnProcessedAdvancePaymentTransList(PaymentID).Select(a => new AdvancePaymentList()
                    {
                        ID = a.ID,
                        AdvancePaymentNo = a.AdvancePaymentNo,
                        PurchaseOrderDate = General.FormatDate(a.PurchaseOrderDate),
                        ItemName = a.ItemName,
                        Amount = a.Amount,
                        ItemID = a.ItemID

                    }).ToList();

                    if (list != null)
                    {

                        advancePayment.PaymentTrans.AddRange(list);
                    }
                }
            }

            return Json(advancePayment.PaymentTrans, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Print(int id)
        {
            string URL = Request.Url.GetLeftPart(UriPartial.Authority) + iAdvancePayment.GetPrintTextFile(id);
            return Json(new { Status = "success", URL = URL }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAdvancePaymentListForDataTable(DatatableModel Datatable)
        {
            try
            {
                string AdvancePaymentNo = Datatable.Columns[1].Search.Value;
                string AdvancePaymentDate = Datatable.Columns[2].Search.Value;
                string Category = Datatable.Columns[3].Search.Value;
                string Name = Datatable.Columns[4].Search.Value;
                string Amount = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = iAdvancePayment.GetAdvancePaymentListForDataTable(Type, AdvancePaymentNo, AdvancePaymentDate, Category, Name, Amount, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AdvancePaymentPrintPdf(int Id)
        {
            return null;
        }
        #endregion
    }
}
