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
    public class AccountGroupController : Controller
    {
        private IAccountGroupContract accountGroupBL;
        private IGeneralContract generalBL;
        public AccountGroupController()
        {
            accountGroupBL = new AccountGroupBL();
            generalBL = new GeneralBL();
        }
        // GET: Masters/AccountGroupV1
        public ActionResult IndexV3()
        {
            return View();
        }

        public ActionResult CreateV3()
        {
            AccountGroupModel model = new AccountGroupModel();
            model.Code = generalBL.GetSerialNo("AccountGroup", "Code");
            return View(model);
        }

        public ActionResult DetailsV3(int? Id)
        {
            if (Id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    AccountGroupModel model;
                    AccountGroupBO accountGroupBO = accountGroupBL.GetAccountGroup((int)Id);
                    model = new AccountGroupModel()
                    {
                        ID = (int)Id,
                        Code = accountGroupBO.Code,
                        AccountGroupName = accountGroupBO.AccountGroupName,
                        IsAllowAccountsUnder = (bool)accountGroupBO.IsAllowAccountsUnder,
                        AccountHeadCodePrefix = accountGroupBO.AccountHeadCodePrefix,
                        ParentGroupID = accountGroupBO.ParentGroupID,
                        ParentGroup = accountGroupBO.ParentGroup,
                    };
                    return View(model);
                }

                catch (Exception e)
                {
                    return View();
                }
            }
        }

        public ActionResult EditV3(int? Id)
        {
            if (Id == null)
            {
                return View("PageNotFound");
            }
            else
            {
                try
                {
                    AccountGroupModel model;
                    AccountGroupBO accountGroupBO = accountGroupBL.GetAccountGroup((int)Id);
                    model = new AccountGroupModel()
                    {
                        ID=(int)Id,
                        Code = accountGroupBO.Code,
                        AccountGroupName = accountGroupBO.AccountGroupName,
                        IsAllowAccountsUnder = (bool)accountGroupBO.IsAllowAccountsUnder,
                        AccountHeadCodePrefix = accountGroupBO.AccountHeadCodePrefix,
                        ParentGroupID = accountGroupBO.ParentGroupID,
                        ParentGroup = accountGroupBO.ParentGroup,
                    };
                    return View(model);
                }

                catch (Exception e)
                {
                    return View();
                }
            }
        }

        public JsonResult GetAccountGroupParentAutoComplete(string Hint = "")
        {
            try
            {
                DatatableResultBO resultBO = accountGroupBL.GetAccountGroupParentList(Hint, "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(AccountGroupModel model)
        {
            try
            {
                AccountGroupBO AccountGroup = new AccountGroupBO()
                {
                    ID = model.ID,
                    AccountGroupName=model.AccountGroupName,
                    ParentGroupID=model.ParentGroupID,
                    IsAllowAccountsUnder=model.IsAllowAccountsUnder,
                    AccountHeadCodePrefix=model.AccountHeadCodePrefix

                };
                accountGroupBL.Save(AccountGroup);
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
                return
                     Json(new
                     {
                         Status = "failure",
                         data = "",
                         message = e.Message
                     }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetAccountGroupListV3(DatatableModel Datatable)
        {
            try
            {
                string AccountNameHint = Datatable.Columns[1].Search.Value;
                string ParentAccountNameHint = Datatable.Columns[2].Search.Value;


                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = accountGroupBL.GetAccountGroupListV3(AccountNameHint, ParentAccountNameHint, SortField, SortOrder, Offset, Limit);
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