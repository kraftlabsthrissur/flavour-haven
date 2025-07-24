using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class PeriodClosingController : Controller
    {
        private IPeriodClosingContract periodClosingBL;
        public PeriodClosingController()
        {
            periodClosingBL = new PeriodClosingBL();
        }
        // GET: Masters/PeriodClosing
        public ActionResult Index()
        {
            PeriodClosingModel periodClosing = new PeriodClosingModel();
            periodClosing.StatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Open", Value = "Open"},
                                                 new SelectListItem { Text = "Closed", Value = "Closed"},
                                                 new SelectListItem { Text = "FinallyClosed", Value = "FinallyClosed"},
                                                 }, "Value", "Text");
            periodClosing.Items = periodClosingBL.GetPeriodClosingList().Select(a => new PeriodClosingDays
            {
                ID = a.ID,
                Month = a.Month,
                JournalStatus = a.JournalStatus,
                SDNStatus = a.SDNStatus,
                SCNStatus = a.SCNStatus,
                CDNStatus = a.CDNStatus,
                CCNStatus = a.CCNStatus,
                FinYear = a.FinYear
            }).ToList();
            periodClosing.FinYear = periodClosing.Items.FirstOrDefault().FinYear;
            return View(periodClosing);
        }

        public ActionResult Save(PeriodClosingModel periodClosingModel)
        {
            try
            {
                PeriodClosingBO periodClosingBO = new PeriodClosingBO();
                List<PeriodClosingDaysBO> items = new List<PeriodClosingDaysBO>();
                PeriodClosingDaysBO PeriodClosingDaysBO;
                foreach (var item in periodClosingModel.Items)
                {
                    PeriodClosingDaysBO = new PeriodClosingDaysBO()
                    {
                        ID = item.ID,
                        Month = item.Month,
                        JournalStatus = item.JournalStatus,
                        SDNStatus = item.SDNStatus,
                        SCNStatus = item.SCNStatus,
                        CDNStatus = item.CDNStatus,
                        CCNStatus = item.CCNStatus
                    };
                    items.Add(PeriodClosingDaysBO);
                }
                periodClosingBL.Save(items);
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsMonthClosed(string Type,string Date)
        {
            try
            {
                DateTime NDate = General.ToDateTime(Date);
                string Month= (NDate.ToString("MMMMMMMMMM"));
                int Year = GeneralBO.FinYear;
                string data = periodClosingBL.IsMonthClosed(Type, Month, Year);
                //int BatchTypeID = customerBL.GetBatchTypeID(CustomerID);
                return
                     Json(new
                     {
                         Status = "success",
                         data = data,
                         message = ""
                     }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return
                        Json(new
                        {
                            Status = "",
                            data = "",
                            message = e.Message
                        }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}