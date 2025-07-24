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
    public class CreditNoteController : Controller
    {

        #region Private members

        private ICreditNoteContract creditnoteBL;
        private IGeneralContract generalBL;
        private IGSTCategoryContract GstBL;
        #endregion

        #region Constructor
        public CreditNoteController( )
        {
            creditnoteBL = new CreditNoteBL();
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
            CreditNoteModel model = new CreditNoteModel();
            model.TransNo = generalBL.GetSerialNo("CreditNote", "Code");
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
                    CreditNoteModel model;
                    CreditNoteBO CreditNoteBO = creditnoteBL.GetCreditNote((int)id);
                    model = new CreditNoteModel()
                    {
                        TransNo = CreditNoteBO.TransNo,
                        TransDate = General.FormatDate(CreditNoteBO.TransDate),
                        IsDraft = CreditNoteBO.IsDraft ,
                        ID = CreditNoteBO.ID,
                        CreditAccountID= CreditNoteBO.CreditAccountID,
                        DebitAccountID=CreditNoteBO.DebitAccountID,
                        CreditAccount=CreditNoteBO.CreditAccount,
                        DebitAccount=CreditNoteBO.DebitAccount,
                        Remarks=CreditNoteBO.Remarks,
                        Amount=CreditNoteBO.Amount,
                        IsProcessed=CreditNoteBO.IsProcessed,
                        IsInclusive = CreditNoteBO.IsInclusive,
                        GSTCategoryID = CreditNoteBO.GSTCategoryID,
                        TaxableAmount = CreditNoteBO.TaxableAmount,
                        GSTAmount = CreditNoteBO.GSTAmount,
                        TotalAmount = CreditNoteBO.TotalAmount,
                        GSTPercent = CreditNoteBO.GSTPercent

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
                    CreditNoteModel model;
                    CreditNoteBO CreditNoteBO = creditnoteBL.GetCreditNote((int)id);
                    model = new CreditNoteModel()
                    {
                        TransNo = CreditNoteBO.TransNo,
                        TransDate = General.FormatDate(CreditNoteBO.TransDate),
                        IsDraft = CreditNoteBO.IsDraft,
                        ID = CreditNoteBO.ID,
                        CreditAccountID = CreditNoteBO.CreditAccountID,
                        DebitAccountID = CreditNoteBO.DebitAccountID,
                        CreditAccount = CreditNoteBO.CreditAccount,
                        DebitAccount = CreditNoteBO.DebitAccount,
                        Remarks = CreditNoteBO.Remarks,
                        Amount = CreditNoteBO.Amount,
                        IsProcessed = CreditNoteBO.IsProcessed,
                        IsInclusive = CreditNoteBO.IsInclusive,
                        GSTCategoryID = CreditNoteBO.GSTCategoryID,
                        TaxableAmount = CreditNoteBO.TaxableAmount,
                        GSTAmount = CreditNoteBO.GSTAmount,
                        TotalAmount = CreditNoteBO.TotalAmount,
                        GSTPercent = CreditNoteBO.GSTPercent
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

        public ActionResult Save(CreditNoteModel model)
        {
            try
            {
                CreditNoteBO CreditNote = new CreditNoteBO()
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
                creditnoteBL.Save(CreditNote);
                return Json(new { Status = "Success", Message = "AccountHead Created" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "AccountHead Creation failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAsDraft(CreditNoteModel model)
        {
            return Save(model);
        }

        public JsonResult GetCreditNoteList(DatatableModel Datatable)
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

                DatatableResultBO resultBO = creditnoteBL.GetCreditNoteList(Type, TransNo, TransDate, DebitAccount, CreditAccount, Amount, SortField, SortOrder, Offset, Limit);
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