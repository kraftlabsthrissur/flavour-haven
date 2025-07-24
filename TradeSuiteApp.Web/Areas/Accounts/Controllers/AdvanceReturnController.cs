using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObject;
using BusinessLayer;
using PresentationContractLayer;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Utils;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class AdvanceReturnController : Controller
    {
        private IAdvanceReturnContract advanceReturnBL;
        private IGeneralContract generalBL;
        private IPaymentTypeContract paymentTypeBL;
        private ITreasuryContract treasuryBL;

        public AdvanceReturnController()
        {
            generalBL = new GeneralBL();
            advanceReturnBL = new AdvanceReturnBL();
            paymentTypeBL = new PaymentTypeBL();
            treasuryBL = new TreasuryBL();

        } // GET: Accounts/AdvanceReturn
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed", "cancelled" };
            return View();
        }

        // GET: Accounts/AdvanceReturn/Details/5
        public ActionResult Details(int id)
        {
            AdvanceReturnModel advanceReturnList = advanceReturnBL.GetAdvanceReturnDetails(id).Select(a => new AdvanceReturnModel
            {
                ID = a.ID,
                Date = General.FormatDate(a.Date, "view"),
                Category = a.Category,
                EmployeeName = a.EmployeeName,
                NetAmount = a.NetAmount,
                ReturnNo = a.ReturnNo,
                IsDraft = a.IsDraft,
                PaymentTypeID = a.PaymentTypeID,
                BankID = a.BankID,
                PaymentTypeName = a.PaymentTypeName,
                BankName = a.BankName,
                ReferenceNumber = a.ReferenceNumber,
                Remarks = a.Remarks,
                SupplierID = a.SupplierID,
                EmployeeID = a.EmployeeID,
                SupplierName = a.SupplierName
            }).First();

            advanceReturnList.Items = advanceReturnBL.GetAdvanceReturnTransDetails(id).Select(k => new AdvanceReturnTransModel()
            {
                ID = k.ID,
                PODate = General.FormatDate(k.PODate, "view"),
                ItemName = k.ItemName,
                Amount = k.Amount,
                ReturnAmount = k.ReturnAmount,
                TransNo = k.TransNo,
                AdvanceID = k.AdvanceID,
                ItemID = k.ItemID
            }).ToList();
            return View(advanceReturnList);
        }

        // GET: Accounts/AdvanceReturn/Create
        public ActionResult Create()
        {
            AdvanceReturnModel advanceReturn = new AdvanceReturnModel();
            advanceReturn.UnProcessedAPList = new List<AdvancePaymentModel>();
            advanceReturn.Categories = new List<SelectListItem>()
                                        {
                                            new SelectListItem() { Text = "Supplier", Value = "Supplier" },
                                            new SelectListItem() { Text = "Employee", Value = "Employee" }
                                        };
            advanceReturn.Date = General.FormatDate(DateTime.Now);
            advanceReturn.ReturnNo = generalBL.GetSerialNo("AdvanceReturn", "Code");
            advanceReturn.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", advanceReturn.PaymentTypeID);
            advanceReturn.BankList = new SelectList(treasuryBL.GetBank("Payment", ""), "ID", "BankName");
            return View(advanceReturn);
        }

        // POST: Accounts/AdvanceReturn/Create
        [HttpPost]
        public ActionResult Save(AdvanceReturnModel model)
        {
            var result = new List<object>();

            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    AdvanceReturnBO Temp = advanceReturnBL.GetAdvanceReturnDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }

                // TODO: Add insert logic here
                AdvanceReturnBO advanceReturn = new AdvanceReturnBO();

                advanceReturn.ID = model.ID;
                advanceReturn.Date = General.ToDateTime(model.Date);
                advanceReturn.Category = model.Category;
                advanceReturn.SupplierID = model.SupplierID;
                advanceReturn.EmployeeID = model.EmployeeID;
                advanceReturn.NetAmount = model.NetAmount;
                advanceReturn.IsDraft = model.IsDraft;
                advanceReturn.PaymentTypeID = model.PaymentTypeID;
                advanceReturn.BankID = model.BankID;
                advanceReturn.ReferenceNumber = model.ReferenceNumber;
                advanceReturn.Remarks = model.Remarks;

                List<AdvanceReturnTransBO> ItemList = new List<AdvanceReturnTransBO>();
                AdvanceReturnTransBO advanceReturnTransBO;
                foreach (var itm in model.Items)
                {
                    advanceReturnTransBO = new AdvanceReturnTransBO();
                    advanceReturnTransBO.AdvanceID = itm.AdvanceID;
                    advanceReturnTransBO.ReturnAmount = itm.ReturnAmount;
                    advanceReturnTransBO.ItemID = itm.ItemID;
                    advanceReturnTransBO.AdvancePaidAmount = itm.AdvancePaidAmount;

                    ItemList.Add(advanceReturnTransBO);
                }

                if (advanceReturnBL.Save(advanceReturn, ItemList))
                {
                    return Json("success", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.Add(new { ErrorMessage = "Unknown Error" });
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "AdvanceReturn", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Accounts/AdvanceReturn/Edit/5
        public ActionResult Edit(int id)
        {
            AdvanceReturnModel advanceReturnList = advanceReturnBL.GetAdvanceReturnDetails(id).Select(a => new AdvanceReturnModel
            {
                ID = a.ID,
                Date = General.FormatDate(a.Date),
                Category = a.Category,
                EmployeeName = a.EmployeeName,
                NetAmount = a.NetAmount,
                ReturnNo = a.ReturnNo,
                IsDraft = a.IsDraft,
                PaymentTypeID = a.PaymentTypeID,
                BankID = a.BankID,
                PaymentTypeName = a.PaymentTypeName,
                BankName = a.BankName,
                ReferenceNumber = a.ReferenceNumber,
                Remarks = a.Remarks,
                SupplierID = a.SupplierID,
                EmployeeID = a.EmployeeID,
                SupplierName = a.SupplierName
            }).First();
            if(!advanceReturnList.IsDraft)
            {
                return RedirectToAction("Index");
            }

            advanceReturnList.Items = advanceReturnBL.GetAdvanceReturnTransDetails(id).Select(k => new AdvanceReturnTransModel()
            {
                ID = k.ID,
                PODate = General.FormatDate(k.PODate, "view"),
                ItemName = k.ItemName,
                Amount = k.Amount,
                ReturnAmount = k.ReturnAmount,
                TransNo = k.TransNo,
                AdvanceID = k.AdvanceID,
                ItemID = k.ItemID
            }).ToList();

            advanceReturnList.PaymentTypeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", advanceReturnList.PaymentTypeID);
            advanceReturnList.BankList = new SelectList(treasuryBL.GetBank("Payment", advanceReturnList.PaymentTypeName == "CASH" ? "Cash" : "Bank"), "ID", "BankName");
            advanceReturnList.Categories = new List<SelectListItem>()
                                        {
                                            new SelectListItem() { Text = "Supplier", Value = "Supplier" },
                                            new SelectListItem() { Text = "Employee", Value = "Employee" }
                                        };
            advanceReturnList.UnProcessedAPList = new List<AdvancePaymentModel>();
            return View(advanceReturnList);
        }

        public ActionResult SaveAsDraft(AdvanceReturnModel model)
        {
            return Save(model);
        }

        public JsonResult GetAdvanceReturnList(DatatableModel Datatable)
        {
            try
            {
                string ARNoHint = Datatable.Columns[1].Search.Value;
                string ARDateHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string CategoryHint = Datatable.Columns[4].Search.Value;
                string NetAmountHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = advanceReturnBL.GetAdvanceReturnList(Type, ARNoHint, ARDateHint, NameHint, CategoryHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Accounts", "AdvanceReturn", "GetAdvanceReturnList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
