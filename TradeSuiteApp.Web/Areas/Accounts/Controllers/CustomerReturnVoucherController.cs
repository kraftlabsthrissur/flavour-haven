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
    public class CustomerReturnVoucherController : Controller
    {
        #region Private members
        private ICustomerContract customerBL;
        private ITreasuryContract treasuryBL;
        private IGeneralContract generalBL;
        private IPaymentTypeContract paymentTypeBL;
        private ICustomerReturnVoucherContract customerReturnVoucherBL;
        #endregion

        #region Constructor
        public CustomerReturnVoucherController()
        {
            customerBL = new CustomerBL();
            paymentTypeBL = new PaymentTypeBL();
            treasuryBL = new TreasuryBL();
            generalBL = new GeneralBL();
            customerReturnVoucherBL = new CustomerReturnVoucherBL();
        }
        #endregion

        // GET: Accounts/CustomerReturn
        public ActionResult Index()
        {
            return View();
        }

        // GET: Accounts/CustomerReturn/Create
        public ActionResult Create()
        {
            CustomerReturnVoucherModel customerReturn = new CustomerReturnVoucherModel();
            customerReturn.VoucherNo = generalBL.GetSerialNo("CustomerReturnVoucher", "Code");
            customerReturn.VoucherDate = General.FormatDate(DateTime.Now);
            customerReturn.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            customerReturn.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");
            customerReturn.BankList = new SelectList(treasuryBL.GetBankList(), "ID", "BankName");
            customerReturn.Items = new List<CustomerReturnVoucherItemModel>();
            return View(customerReturn);
        }

        [HttpPost]
        public ActionResult Save(CustomerReturnVoucherModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    CustomerReturnVoucherBO Temp = customerReturnVoucherBL.GetCustomerReturnDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                CustomerReturnVoucherBO customerReturn = new CustomerReturnVoucherBO()
                {
                    ID = model.ID,
                    VoucherNo = model.VoucherNo,
                    VoucherDate = General.ToDateTime(model.VoucherDate),
                    CustomerID = model.CustomerID,
                    PaymentTypeID = model.PaymentTypeID,
                    BankID = model.BankID,
                    BankReferenceNumber = model.BankReferenceNumber,
                    IsDraft = model.IsDraft
                };
                List<CustomerReturnVoucherItemBO> ItemList = new List<CustomerReturnVoucherItemBO>();
                CustomerReturnVoucherItemBO customerReturnItem;
                foreach (var item in model.Items)
                {
                    customerReturnItem = new CustomerReturnVoucherItemBO()
                    {
                        Amount = item.Amount,
                        Remarks = item.Remarks
                    };
                    ItemList.Add(customerReturnItem);
                }
                if (model.ID == 0)
                {
                    customerReturnVoucherBL.Save(customerReturn, ItemList);
                }
                else
                {
                    customerReturnVoucherBL.Update(customerReturn, ItemList);
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "CustomerReturnVoucher", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(CustomerReturnVoucherModel model)
        {
            return Save(model);
        }

        //GET :Accounts/CustomerReturn/Details
        public ActionResult Details(int id)
        {
            CustomerReturnVoucherModel customerReturn = customerReturnVoucherBL.GetCustomerReturnDetails(id).Select(m => new CustomerReturnVoucherModel()
            {
                ID = m.ID,
                VoucherNo = m.VoucherNo,
                VoucherDate = General.FormatDate(m.VoucherDate, "view"),
                CustomerName = m.CustomerName,
                PaymentTypeName = m.PaymentTypeName,
                BankName = m.BankName,
                Amount = m.Amount,
                BankReferenceNumber = m.BankReferenceNumber,
                IsDraft = m.IsDraft
            }).First();
            customerReturn.Items = customerReturnVoucherBL.GetCustomerReturnTransDetails(id).Select(m => new CustomerReturnVoucherItemModel()
            {
                CustomerName = m.CustomerName,
                Amount = m.Amount,
                Remarks = m.Remarks
            }).ToList();
            return View(customerReturn);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CustomerReturnVoucherModel customerReturn = customerReturnVoucherBL.GetCustomerReturnDetails(id).Select(m => new CustomerReturnVoucherModel()
            {
                ID = m.ID,
                VoucherNo = m.VoucherNo,
                VoucherDate = General.FormatDate(m.VoucherDate),
                CustomerName = m.CustomerName,
                CustomerID = m.CustomerID,
                PaymentTypeName = m.PaymentTypeName,
                PaymentTypeID = m.PaymentTypeID,
                BankName = m.BankName,
                BankID = m.BankID,
                Amount = m.Amount,
                BankReferenceNumber = m.BankReferenceNumber,
                IsDraft = m.IsDraft
            }).First();
            if(!customerReturn.IsDraft)
            {
                return RedirectToAction("Index");
            }
            customerReturn.Items = customerReturnVoucherBL.GetCustomerReturnTransDetails(id).Select(m => new CustomerReturnVoucherItemModel()
            {
                ID = m.ID,
                CustomerName = m.CustomerName,
                Amount = m.Amount,
                Remarks = m.Remarks
            }).ToList();
            customerReturn.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            customerReturn.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name");       
            customerReturn.BankList = new SelectList(treasuryBL.GetBankList(), "ID", "BankName");
            return View(customerReturn);
        }

        public JsonResult GetCustomerReturnVoucherList(DatatableModel Datatable)
        {
            try
            {
                string VoucherNoHint = Datatable.Columns[1].Search.Value;
                string VoucherDateHint = Datatable.Columns[2].Search.Value;
                string CustomerNameHint = Datatable.Columns[3].Search.Value;
                string PaymentHint = Datatable.Columns[4].Search.Value;
                string ReturnAmountHint = Datatable.Columns[5].Search.Value;


                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = customerReturnVoucherBL.GetCustomerReturnVoucherList(Type, VoucherNoHint, VoucherDateHint, CustomerNameHint, PaymentHint, ReturnAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CustomerReturnVoucherPrintPdf(int Id)
        {
            return null;
        }
    }
}