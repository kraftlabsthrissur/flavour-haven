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
    public class SLAController : Controller
    {
        ISLAContract slaBL;
        IGeneralContract generalBL;

        public SLAController()
        {
            slaBL = new SLABL();
            generalBL = new GeneralBL();
        }

        // GET: Accounts/SLA
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post()
        {
            return null;
        }

        public ActionResult PostAccounts()
        {
            try
            {
                slaBL.CreateAccountEntryDetails();
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                res.Add(new { ErrorMessage = "Unknown Error" });
                generalBL.LogError("Accounts", "SLA", "PostAccounts", 0, e);
                return Json(new { Status = "failure", data = res }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Process()
        {
            try
            {
                slaBL.GenerateAccountEntryDataUsingSLARules();
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                res.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "SLA", "Process", 0, e);
                return Json(new { Status = "failure", data = res }, JsonRequestBehavior.AllowGet);
            }
        }

        //reference for report
        public JsonResult GetTransTypeAutoComplete(string Term = "")
        {
            List<SLAViewModel> PRNoList = new List<SLAViewModel>();
            PRNoList = slaBL.GetTransTypeAutoComplete(Term).Select(a => new SLAViewModel()
            {
                TransactionType = a.TransationType,
            }).ToList();
            return Json(PRNoList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSLAValuesList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = Datatable.Columns[1].Search.Value;
                string TransationTypeHint = Datatable.Columns[2].Search.Value;
                string KeyValueHint = Datatable.Columns[3].Search.Value;
                string AmountHint = Datatable.Columns[4].Search.Value;
                string EventHint = Datatable.Columns[5].Search.Value;
                string DocumentTableHint = Datatable.Columns[6].Search.Value;
                string DocumentNumberHint = Datatable.Columns[7].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = slaBL.GetSLAValuesList(Type, DateHint, TransationTypeHint, KeyValueHint, AmountHint, EventHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Accounts", "SLA", "GetSLAValuesList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSLAToBePostedList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = Datatable.Columns[1].Search.Value;
                string DebitAccountHint = Datatable.Columns[2].Search.Value;
                string DebitAccountNameHint = Datatable.Columns[3].Search.Value;
                string CreditAccountHint = Datatable.Columns[4].Search.Value;
                string CreditAccountNameHint = Datatable.Columns[5].Search.Value;
                string AmountHint = Datatable.Columns[6].Search.Value;
                string ItemNameHint = Datatable.Columns[7].Search.Value;
                string SupplierNameHint = Datatable.Columns[8].Search.Value;
                string DocumentTableHint = Datatable.Columns[9].Search.Value;
                string DocumentNumberHint = Datatable.Columns[10].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DateTime FromDate = DateTime.ParseExact("01-Jan-2018", "dd-MMM-yyyy", null);
                DateTime ToDate = DateTime.Now;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = slaBL.GetSLAToBePostedList(FromDate, ToDate, Type, DateHint, DebitAccountHint, DebitAccountNameHint, CreditAccountHint, CreditAccountNameHint, AmountHint, ItemNameHint, DocumentTableHint, SupplierNameHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Accounts", "SLA", "GetSLAToBePostedList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSLAPostedList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = Datatable.Columns[1].Search.Value;
                string DebitAccountHint = Datatable.Columns[2].Search.Value;
                string DebitAccountNameHint = Datatable.Columns[3].Search.Value;
                string CreditAccountHint = Datatable.Columns[4].Search.Value;
                string CreditAccountNameHint = Datatable.Columns[5].Search.Value;
                string AmountHint = Datatable.Columns[6].Search.Value;
                string DocumentTableHint = Datatable.Columns[7].Search.Value;
                string DocumentNumberHint = Datatable.Columns[8].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DateTime FromDate = DateTime.ParseExact("01-Jan-2018", "dd-MMM-yyyy", null);
                DateTime ToDate = DateTime.Now;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = slaBL.GetSLAPostedList(FromDate, ToDate, Type, DateHint, DebitAccountHint, DebitAccountNameHint, CreditAccountHint, CreditAccountNameHint, AmountHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Accounts", "SLA", "GetSLAPostedList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSLAErrorList(DatatableModel Datatable)
        {
            try
            {
                string DateHint = Datatable.Columns[1].Search.Value;
                string TransationTypeHint = Datatable.Columns[2].Search.Value;
                string KeyValueHint = Datatable.Columns[3].Search.Value;
                string EventHint = Datatable.Columns[4].Search.Value;
                string ItemNameHint = Datatable.Columns[5].Search.Value;
                string SupplierNameHint = Datatable.Columns[6].Search.Value;
                string DescriptionHint = Datatable.Columns[7].Search.Value;
                string RemarksHint = Datatable.Columns[8].Search.Value;
                string DocumentTableHint = Datatable.Columns[9].Search.Value;
                string DocumentNumberHint = Datatable.Columns[10].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DateTime FromDate = DateTime.ParseExact("01-Jan-2018", "dd-MMM-yyyy", null);
                DateTime ToDate = DateTime.Now;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = slaBL.GetSLAErrorList(FromDate, ToDate, Type, DateHint, TransationTypeHint, KeyValueHint, EventHint, ItemNameHint, SupplierNameHint, DescriptionHint, RemarksHint, DocumentTableHint, DocumentNumberHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Accounts", "SLA", "GetSLAErrorList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}