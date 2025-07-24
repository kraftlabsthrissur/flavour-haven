using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class AccountHeadController : Controller
    {
        IAccountContract accountBL;
        private IAccountGroupContract accountGroupBL;
        private IAccountHeadContract accountHeadBL;
        private IGeneralContract generalBL;

        public AccountHeadController()
        {
            accountBL = new AccountBL();
            accountGroupBL = new AccountGroupBL();
            accountHeadBL = new AccountHeadBL();
            generalBL = new GeneralBL();
        }
        // GET: Masters/Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Masters/Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Masters/Account/Create
        public ActionResult Create()
        {
            AccountGroupModel model = new AccountGroupModel();
            return View(model);
        }

        // GET: Masters/Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        public JsonResult GetAccountHeadsForAutoComplete(string Hint="")
        {
            List<AccountHeadModel> accountList = new List<AccountHeadModel>();
            accountList = accountBL.GetAccountHeadsForAutoComplete(Hint).Select(a => new AccountHeadModel()
            {
                ID = a.ID,
                AccountID = a.AccountId,
                AccountName = a.AccountName,
                GroupClassification = a.GroupClassification,
                OpeningAmt = (Decimal)a.OpeningAmt
            }).ToList();
            return Json(accountList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountHeadsForSLAAutoComplete(string Hint)
        {
            List<AccountHeadModel> accountList = new List<AccountHeadModel>();
            accountList = accountBL.GetAccountHeadsForSLAAutoComplete(Hint).Select(a => new AccountHeadModel()
            {
                ID = a.ID,
                AccountID = a.AccountId,
                AccountName = a.AccountName
            }).ToList();
            return Json(accountList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountHeadNameAutoComplete(string Hint)
        {
            List<AccountHeadModel> accountList = new List<AccountHeadModel>();
            accountList = accountBL.GetAccountHeadNameAutoComplete(Hint).Select(a => new AccountHeadModel()
            {
                ID = a.ID,
                AccountID = a.AccountId,
                AccountName = a.AccountName
            }).ToList();
            return Json(accountList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(int id)
        {
            return null;
        }



        //Created by priyanka
        public ActionResult IndexV3()
        {   
            return View();
        }
        public ActionResult CreateV3()
        {
            AccountGroupModel model = new AccountGroupModel();
            model.OpeningAmountTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Dr", Value ="Dr", },
                                                 new SelectListItem { Text = "Cr", Value = "Cr"},
                                                 }, "Value", "Text");
            return View(model);
        }
        public JsonResult GetAccountID(string AccountName)
        {
            var Code = generalBL.GetSerialNo(AccountName, "Code");
            return Json(new { Status = "success", AccountID = Code }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveV3(AccountGroupModel model)
        {
            try
            {
                int ID = model.ID;
                AccountGroupBO AccountHead = new AccountGroupBO()
                {
                    ID = model.ID,
                    AccountID=model.AccountID,
                    AccountName=model.AccountName,
                    AccountGroupID=model.AccountGroupID,
                    AccountGroupName=model.AccountGroupName,
                    OpeningAmount = model.OpeningAmount,
                    OpeningAmountType = model.OpeningAmountType
                };
                if (AccountHead.ID == 0)
                {
                   ID=  accountHeadBL.SaveAccountHead(AccountHead);
                }
                else
                {
                    accountHeadBL.UpdateAccountHead(AccountHead);
                }
                return Json(new { Status = "Success", Message = "AccountHead Created",ID=ID,data= AccountHead }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "AccountHead Creation failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DetailsV3(int Id)
        {
            AccountGroupModel AccountHead = accountHeadBL.GetAccountHeadDetails(Id).Select(m => new AccountGroupModel()
            {
               ID = m.ID,
               AccountID =m.AccountID,
               AccountName=m.AccountName,
               AccountGroupName=m.AccountGroupName,
               AccountGroupID=m.AccountGroupID,
               OpeningAmount=m.OpeningAmount,
               OpeningAmountType=m.OpeningAmountType
            }).First();
            return View(AccountHead);
        }
        public ActionResult EditV3(int Id)
        {

            AccountGroupModel AccountHead = accountHeadBL.GetAccountHeadDetails(Id).Select(m => new AccountGroupModel()
            {
                ID= m.ID,
                AccountID = m.AccountID,
                AccountName = m.AccountName,
                AccountGroupName = m.AccountGroupName,
                AccountGroupID = m.AccountGroupID,
                OpeningAmount = m.OpeningAmount,
                OpeningAmountType = m.OpeningAmountType
            }).First();
            AccountHead.OpeningAmountTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Dr", Value ="Dr", },
                                                 new SelectListItem { Text = "Cr", Value = "Cr"},
                                                 }, "Value", "Text");
            return View(AccountHead);
        }

        public JsonResult GetAccountHeadListV3(DatatableModel Datatable)
        {
            try
            {
                string AccountIDHint = Datatable.Columns[2].Search.Value;
                string AccountNameHint = Datatable.Columns[3].Search.Value;
                string AccountGroupHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = accountHeadBL.GetAccountHeadListV3(AccountIDHint, AccountNameHint, AccountGroupHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hint"></param>
        /// <returns></returns>
        public JsonResult GetAccountHeadListAutoCompleteV3(string Hint)
        {
            try
            {
                List<AccountHeadModel> accountList = new List<AccountHeadModel>();
                DatatableResultBO resultBO = accountHeadBL.GetAccountHeadListV3("", Hint, "", "AccountName", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "AccountHead", "GetAccountHeadListAutoCompleteV3", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDebitAccountList(DatatableModel Datatable)
        {
            try
            {
                string AccountIDHint = Datatable.Columns[2].Search.Value;
                string AccountNameHint = Datatable.Columns[3].Search.Value;
                string AccountGroupHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = accountHeadBL.GetAccountHeadListV3(AccountIDHint, AccountNameHint, AccountGroupHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCreditAccountList(DatatableModel Datatable)
        {
            try
            {
                string AccountIDHint = Datatable.Columns[2].Search.Value;
                string AccountNameHint = Datatable.Columns[3].Search.Value;
                string AccountGroupHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = accountHeadBL.GetAccountHeadListV3(AccountIDHint, AccountNameHint, AccountGroupHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult AddAccountHead()
        {
            AccountGroupModel model = new AccountGroupModel();
            model.OpeningAmountTypeList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Dr", Value ="Dr", },
                                                 new SelectListItem { Text = "Cr", Value = "Cr"},
                                                 }, "Value", "Text");
            return PartialView(model);
        }

    }
}