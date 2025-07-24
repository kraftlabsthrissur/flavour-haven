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

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class ChequeStatusController : Controller
    {
        #region Private members

        private IChequeStatusContract chequestatusBL;
        private IGeneralContract generalBL;
        private IAddressContract addressBL;
        private IConfigurationContract configBL;
        private IGSTCategoryContract gstCategoryBL;
        #endregion

        #region Constructor
        public ChequeStatusController()
        {
            chequestatusBL = new ChequeStatusBL();
            generalBL = new GeneralBL();
            addressBL = new AddressBL();
            configBL = new ConfigurationBL();
            gstCategoryBL = new GSTCategoryBL();
        }
        #endregion

        #region Public methods
        // GET: Accounts/ChequeStatus
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft" };
            return View();
        }

        // GET: Accounts/ChequeStatus/Create
        public ActionResult Create()
        {
            ChequeStatusModel chequeStatus = new ChequeStatusModel();
            chequeStatus.GstCategoryID = configBL.GetGstCategoryForChequestatus();
            chequeStatus = gstCategoryBL.GetGSTCategoryDetails(chequeStatus.GstCategoryID).Select(a => new ChequeStatusModel
            {
                IGST = a.IGSTPercent,
                CGST = a.CGSTPercent,
                SGST = a.SGSTPercent,
            }).FirstOrDefault();

            chequeStatus.TransNo = generalBL.GetSerialNo("ChequeStatus", "Code");
            chequeStatus.Items = new List<ChequeStatusTransModel>();
            chequeStatus.UserStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            chequeStatus.InstrumentStatusList = new SelectList(
                         new List<SelectListItem>
                         {
                            new SelectListItem { Text = "Received", Value = "Received"},
                            new SelectListItem { Text = "Deposited", Value = "Deposited"},
                            new SelectListItem { Text = "Collected", Value = "Collected"},
                            new SelectListItem { Text = "Bounced", Value = "Bounced"},
                         }, "Value", "Text");

            ViewBag.StatusReceived = new List<SelectListItem>
                         {
                            new SelectListItem { Text = "Received", Value = "Received"},
                            new SelectListItem { Text = "Deposited", Value = "Deposited"},
                         };

            ViewBag.StatusDeposited = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Deposited", Value = "Deposited"},
                            new SelectListItem { Text = "Collected", Value = "Collected"},
                        };

            ViewBag.StatusCollected = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Collected", Value = "Collected"},
                            new SelectListItem { Text = "Bounced", Value = "Bounced"},
                        };

            ViewBag.StatusPendingError = new List<SelectListItem>
                        {
                             new SelectListItem { Text = "Deposited", Value = "Deposited"},
                        };

            ViewBag.StatusBounced = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Bounced", Value = "Bounced"},
                        };

            ViewBag.StatusCancelled = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Cancelled", Value = "Cancelled"},
                        };

            return View(chequeStatus);
        }
        public JsonResult getChequeStatus(string InstrumentStatus, string FromDate, string ToDate)
        {
            DateTime fromReciptDate = General.ToDateTime(FromDate);
            DateTime toReciptDate = General.ToDateTime(ToDate);

            List<ChequeStatusTransModel> chequestatus = chequestatusBL.getChequeStatus(InstrumentStatus, fromReciptDate, toReciptDate).Select(a => new ChequeStatusTransModel()
            {
                InstrumentNumber = a.InstrumentNumber,
                InstrumentDate = General.FormatDate(a.InstrumentDate),
                ChequeStatus = a.ChequeStatus,
                StatusChangeDate = General.FormatDate(a.StatusChangeDate),
                BankCharges = Convert.ToDecimal(a.BankCharges),
                CustomerID = a.CustomerID,
                CustomerName = a.CustomerName,
                InstrumentAmount = Convert.ToDecimal(a.InstrumentAmount),
                TotalAmount = Convert.ToDecimal(a.TotalAmount),
                ChargesToCustomer = Convert.ToDecimal(a.ChargesToCustomer),
                Remarks = a.Remarks,
                VoucherNo = a.VoucherNo,
                VoucherID = a.VoucherID,
                CGST = a.CGST,
                SGST = a.SGST,
                IGST = a.IGST,
                StateID = a.StateID,
                ChequeReceivedDate = General.FormatDate(a.ChequeReceivedDate)
            }).ToList();
            return Json(chequestatus, JsonRequestBehavior.AllowGet);
        }
        // POST: Accounts/ChequeStatus/Save
        [HttpPost]
        public ActionResult Save(ChequeStatusModel model)
        {
            var result = new List<object>();
            try
            {
                if (model.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    ChequeStatusBO Temp = chequestatusBL.getChequeStatusDetails(model.ID).FirstOrDefault();
                    if (!Temp.IsDraft)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                ChequeStatusBO ChequeStatus = new ChequeStatusBO()
                {
                    ID = model.ID,
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    InstrumentStatus = model.InstrumentStatus,
                    ReceiptDateFrom = General.ToDateTime(model.ReceiptDateFrom),
                    ReceiptDateTo = General.ToDateTime(model.ReceiptDateTo),
                    IsDraft = model.IsDraft
                };
                List<ChequeStatusTransBO> ItemList = new List<ChequeStatusTransBO>();
                ChequeStatusTransBO chequestatusItem;
                foreach (var item in model.Items)
                {
                    chequestatusItem = new ChequeStatusTransBO()
                    {
                        ChequeStatusID = item.ChequeStatusID,
                        InstrumentNumber = item.InstrumentNumber,
                        InstrumentDate = General.ToDateTime(item.InstrumentDate),
                        ChequeStatus = item.ChequeStatus,
                        CustomerID = item.CustomerID,
                        StatusChangeDate = General.ToDateTime(item.StatusChangeDate),
                        InstrumentAmount = item.InstrumentAmount,
                        BankCharges = item.BankCharges,
                        TotalAmount = item.TotalAmount,
                        ChargesToCustomer = item.ChargesToCustomer,
                        IsActive = item.IsActive,
                        VoucherNo = item.VoucherNo,
                        VoucherID = item.VoucherID,
                        Remarks = item.Remarks,
                        CGST = item.CGST,
                        SGST = item.SGST,
                        IGST = item.IGST
                    };
                    ItemList.Add(chequestatusItem);
                }
                if (ChequeStatus.ID == 0)
                {
                    var outId = chequestatusBL.Save(ChequeStatus, ItemList);
                }
                else
                {
                    var outId = chequestatusBL.Update(ChequeStatus, ItemList);
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "ChequeStatus", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(ChequeStatusModel model)
        {
            return Save(model);
        }

        public ActionResult Details(int Id)
        {
            ChequeStatusModel chequestatus = chequestatusBL.getChequeStatusDetails(Id).Select(m => new ChequeStatusModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date),
                InstrumentStatus = m.InstrumentStatus,
                ReceiptDateFrom = General.FormatDate(m.ReceiptDateFrom),
                ReceiptDateTo = General.FormatDate(m.ReceiptDateTo),
                IsDraft = m.IsDraft

            }).First();
            chequestatus.Items = chequestatusBL.getChequeStatusTransDetails(Id).Select(m => new ChequeStatusTransModel()
            {
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate, "view"),
                ChequeStatus = m.ChequeStatus,
                StatusChangeDate = General.FormatDate(m.StatusChangeDate, "view"),
                CustomerID = m.CustomerID,
                CustomerName = m.CustomerName,
                InstrumentAmount = m.InstrumentAmount,
                BankCharges = m.BankCharges,
                TotalAmount = m.TotalAmount,
                ChargesToCustomer = m.ChargesToCustomer,
                Remarks = m.Remarks,
                VoucherNo = m.VoucherNo,
                CGST = m.CGST,
                SGST = m.SGST,
                IGST = m.IGST,
                StateID = m.StateID
            }).ToList();
            return View(chequestatus);

        }

        public ActionResult Edit(int Id)
        {
            ChequeStatusModel chequestatus = chequestatusBL.getChequeStatusDetails(Id).Select(m => new ChequeStatusModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date),
                InstrumentStatus = m.InstrumentStatus,
                ReceiptDateFrom = General.FormatDate(m.ReceiptDateFrom),
                ReceiptDateTo = General.FormatDate(m.ReceiptDateTo),
                IsDraft = m.IsDraft
            }).First();
            if(!chequestatus.IsDraft)
            {
                return RedirectToAction("Index");
            }
            chequestatus.GstCategoryID = configBL.GetGstCategoryForChequestatus();
            chequestatus.IGST = gstCategoryBL.GetGSTCategoryDetails(chequestatus.GstCategoryID).FirstOrDefault().IGSTPercent;
            chequestatus.CGST = gstCategoryBL.GetGSTCategoryDetails(chequestatus.GstCategoryID).FirstOrDefault().CGSTPercent;
            chequestatus.SGST = gstCategoryBL.GetGSTCategoryDetails(chequestatus.GstCategoryID).FirstOrDefault().SGSTPercent;
            chequestatus.UserStateID = addressBL.GetBillingAddress("Location", GeneralBO.LocationID, "").FirstOrDefault().StateID;
            ViewBag.StatusReceived = new List<SelectListItem>
                         {
                            new SelectListItem { Text = "Received", Value = "Received"},
                            new SelectListItem { Text = "Deposited", Value = "Deposited"},
                         };

            ViewBag.StatusDeposited = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Deposited", Value = "Deposited"},
                            new SelectListItem { Text = "Collected", Value = "Collected"},
                        };

            ViewBag.StatusCollected = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Collected", Value = "Collected"},
                            new SelectListItem { Text = "Bounced", Value = "Bounced"},
                        };

            ViewBag.StatusPendingError = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Pending Error", Value = "PendingError"},
                            new SelectListItem { Text = "Deposited", Value = "Deposited"},
                            new SelectListItem { Text = "Cancelled", Value = "Cancelled"},
                         };

            ViewBag.StatusBounced = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Bounced", Value = "Bounced"},
                         };
            ViewBag.StatusCancelled = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Cancelled", Value = "Cancelled"},
                         };

            chequestatus.Items = chequestatusBL.getChequeStatusTransDetails(Id).Select(m => new ChequeStatusTransModel()
            {
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate),
                ChequeStatus = m.ChequeStatus,
                StatusChangeDate = General.FormatDate(m.StatusChangeDate),
                CustomerID = m.CustomerID,
                CustomerName = m.CustomerName,
                InstrumentAmount = m.InstrumentAmount,
                BankCharges = m.BankCharges,
                TotalAmount = m.TotalAmount,
                ChargesToCustomer = m.ChargesToCustomer,
                Remarks = m.Remarks,
                VoucherNo = m.VoucherNo,
                VoucherID = m.VoucherID,
                CGST = m.CGST,
                SGST = m.SGST,
                IGST = m.IGST,
                StateID = m.StateID,
                ChequeReceivedDate = General.FormatDate(m.ChequeReceivedDate),
                EditStatus = chequestatus.InstrumentStatus
            }).ToList();


            chequestatus.InstrumentStatusList = new SelectList(
                       new List<SelectListItem>
                       {
                            new SelectListItem { Text = "Received", Value = "Received"},
                            new SelectListItem { Text = "Deposited", Value = "Deposited"},
                            new SelectListItem { Text = "Collected", Value = "Collected"},
                            new SelectListItem { Text = "Bounced", Value = "Bounced"},
                       }, "Value", "Text");

            return View(chequestatus);
        }

        public JsonResult GetChequeStatusList(DatatableModel Datatable)
        {
            try
            {
                string StatusNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string InstrumentStatusHint = Datatable.Columns[3].Search.Value;
                string FromDateHint = Datatable.Columns[4].Search.Value;
                string ToDateHint = Datatable.Columns[5].Search.Value;
                string CustomerNameHint = Datatable.Columns[6].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = chequestatusBL.GetChequeStatusList(Type, StatusNoHint, TransDateHint, InstrumentStatusHint, FromDateHint, ToDateHint, CustomerNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}