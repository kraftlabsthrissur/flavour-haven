using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class ChartOfAccountsController : Controller
    {
        private IChartOfAccountContract chartOfAccountsBL;

        public ChartOfAccountsController()
        {
            chartOfAccountsBL = new ChartOfAccountsBL();
        }

        // GET: Masters/ChartOfAccounts
        public ActionResult Index()
        {
            List<ChartOfAccountsModel> ChartOfAccountList = new List<ChartOfAccountsModel>();
            ChartOfAccountList = chartOfAccountsBL.GetAccountHeadList().Select(a => new ChartOfAccountsModel
            {
                ID = a.ID,
                AccountID = a.AccountID,
                AccountName = a.AccountName,
                ParentID = a.ParentID,
                OpeningAmount = a.OpeningAmount,
                Level = a.Level,
                ParentName = a.ParentName,
                ParentAccountCode = a.ParentAccountCode
            }).ToList();

            return View(ChartOfAccountList);
        }

        public ActionResult Create()
        {
            ChartOfAccountsModel Model = new ChartOfAccountsModel();
            Model.AccountList = chartOfAccountsBL.GetChartOfAccountList();
            return View(Model);
        }

        public ActionResult AddAccountHead()
        {
            return View();
        }

        public ActionResult Save(ChartOfAccountsModel model)
        {
            try
            {
                ChartOfAccountBO chartOfAccountBO = new ChartOfAccountBO()
                {
                    ID = model.ID,
                    AccountID = model.AccountID,
                    AccountName = model.AccountName,
                    OpeningAmount = model.OpeningAmount,
                    IsManual = model.IsManual,
                    ParentID = model.ParentID,
                    Level = model.Level
                };
                chartOfAccountsBL.Save(chartOfAccountBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IsRemovedItem(int ID)
        {
            try
            {
                bool IsRemovedItem = chartOfAccountsBL.IsRemovedItem(ID);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }


        }
    }
}