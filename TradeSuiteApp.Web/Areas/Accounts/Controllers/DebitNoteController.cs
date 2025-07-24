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
    public class DebitNoteController : Controller
    {
        #region Private members

        private IDebitNoteContract debitnoteBL;
        private IGeneralContract generalBL;
        private IGSTCategoryContract GstBL;
        #endregion

        #region Constructor
        public DebitNoteController()
        {
            debitnoteBL = new DebitNoteBL();
            generalBL = new GeneralBL();
            GstBL = new GSTCategoryBL();
        }
        #endregion
        // GET: Accounts/CreditNote
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            DebitNoteModel model = new DebitNoteModel();
            model.TransNo = generalBL.GetSerialNo("DebitNote", "Code");
            model.TransDate = General.FormatDate(DateTime.Now);
            model.GSTList = new SelectList(
                            new List<SelectListItem>
                            {
                                                new SelectListItem { Text = "IncGST", Value = "1"},
                                                new SelectListItem { Text = "GSTExtra", Value = "2"}

                            }, "Value", "Text");
            model.GSTCategoryList = new SelectList(
                                               GstBL.GetGSTList(), "ID", "IGSTPercent");
            return View(model);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    DebitNoteModel model;
                    DebitNoteBO DebitNoteBO = debitnoteBL.GetDebitNote((int)id);
                    model = new DebitNoteModel()
                    {
                        TransNo = DebitNoteBO.TransNo,
                        TransDate = General.FormatDate(DebitNoteBO.TransDate),
                        IsDraft = DebitNoteBO.IsDraft,
                        ID = DebitNoteBO.ID,
                        CreditAccountID = DebitNoteBO.CreditAccountID,
                        DebitAccountID = DebitNoteBO.DebitAccountID,
                        CreditAccount = DebitNoteBO.CreditAccount,
                        DebitAccount = DebitNoteBO.DebitAccount,
                        Remarks = DebitNoteBO.Remarks,
                        Amount = DebitNoteBO.Amount,
                        IsProcessed = DebitNoteBO.IsProcessed,
                        IsInclusive = DebitNoteBO.IsInclusive,
                        GSTCategoryID = DebitNoteBO.GSTCategoryID,
                        TaxableAmount = DebitNoteBO.TaxableAmount,
                        GSTAmount = DebitNoteBO.GSTAmount,
                        TotalAmount = DebitNoteBO.TotalAmount,
                        GSTPercent = DebitNoteBO.GSTPercent
                    };

                    return View(model);
                }

                catch (Exception e)
                {
                    return View();
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    DebitNoteModel model;
                    DebitNoteBO DebitNoteBO = debitnoteBL.GetDebitNote((int)id);
                    model = new DebitNoteModel()
                    {
                        TransNo = DebitNoteBO.TransNo,
                        TransDate = General.FormatDate(DebitNoteBO.TransDate),
                        IsDraft = DebitNoteBO.IsDraft,
                        ID = DebitNoteBO.ID,
                        CreditAccountID = DebitNoteBO.CreditAccountID,
                        DebitAccountID = DebitNoteBO.DebitAccountID,
                        CreditAccount = DebitNoteBO.CreditAccount,
                        DebitAccount = DebitNoteBO.DebitAccount,
                        Remarks = DebitNoteBO.Remarks,
                        Amount = DebitNoteBO.Amount,
                        IsProcessed = DebitNoteBO.IsProcessed,
                        IsInclusive = DebitNoteBO.IsInclusive,
                        GSTCategoryID = DebitNoteBO.GSTCategoryID,
                        TaxableAmount = DebitNoteBO.TaxableAmount,
                        GSTAmount = DebitNoteBO.GSTAmount,
                        TotalAmount = DebitNoteBO.TotalAmount,
                        GSTPercent = DebitNoteBO.GSTPercent
                    };
                    model.GSTList = new SelectList(
                            new List<SelectListItem>
                            {
                                                new SelectListItem { Text = "IncGST", Value = "1"},
                                                new SelectListItem { Text = "GSTExtra", Value = "2"}

                            }, "Value", "Text");
                    model.GSTCategoryList = new SelectList(
                                                       GstBL.GetGSTList(), "ID", "IGSTPercent");

                    return View(model);
                }

                catch (Exception e)
                {
                    return View();
                }
            }
        }

        public ActionResult Save(DebitNoteModel model)
        {
            try
            {
                DebitNoteBO DebitNote = new DebitNoteBO()
                {
                    ID = model.ID,
                    DebitAccountID = model.DebitAccountID,
                    CreditAccountID = model.CreditAccountID,
                    Amount = model.Amount,
                    Remarks = model.Remarks,
                    TransNo = model.TransNo,
                    IsDraft = model.IsDraft,
                    TransDate = General.ToDateTime(model.TransDate),
                    IsInclusive = model.IsInclusive,
                    GSTCategoryID = model.GSTCategoryID,
                    TaxableAmount = model.TaxableAmount,
                    GSTAmount = model.GSTAmount,
                    TotalAmount = model.TotalAmount

                };
                debitnoteBL.Save(DebitNote);
                return Json(new { Status = "Success", Message = "AccountHead Created" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "AccountHead Creation failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAsDraft(DebitNoteModel model)
        {
            return Save(model);
        }

        public JsonResult GetDebitNoteList(DatatableModel Datatable)
        {
            try
            {
                string TransNo = Datatable.Columns[1].Search.Value;
                string TransDate = Datatable.Columns[2].Search.Value;
                string DebitAccount = Datatable.Columns[3].Search.Value;
                string CreditAccount = Datatable.Columns[4].Search.Value;
                string Amount = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = debitnoteBL.GetDebitNoteList(Type, TransNo, TransDate, DebitAccount, CreditAccount, Amount, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        

    }
}