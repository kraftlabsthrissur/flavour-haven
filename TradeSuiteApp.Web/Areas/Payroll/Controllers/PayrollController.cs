using BusinessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Payroll.Models;

namespace TradeSuiteApp.Web.Areas.Payroll.Controllers
{
    public class PayrollController : Controller
    {
        private ILocationContract locationBL;
        private ILocationHeadContract locationHeadBL;
        #region constructor
        public PayrollController()
        {
            locationBL = new LocationBL();
            locationHeadBL = new LocationHeadBL();
        }
        #endregion
        #region public method

        // GET: Payroll/Payroll
        public ActionResult Index()
        {



            return View();
        }

        // GET: Payroll/Payroll/Create
        public ActionResult Create()
        {
            PayrollModel payroll = new PayrollModel();

           
            payroll.IncometaxComputeList = new SelectList(
                                 new List<SelectListItem>
                                 {
                                       new SelectListItem { Text = "Compute for this month", Value = "1"},
                                       new SelectListItem { Text = "Use previous month's TDS", Value = "2"},
                                       new SelectListItem { Text = "Consider zero TDS", Value = "3"}
                                 }, "Value", "Text");
            payroll.RunPayrollList = new SelectList(
                                new List<SelectListItem>
                                {
                                       new SelectListItem { Text = "All Employees", Value = "1"},
                                       new SelectListItem { Text = "Selected Employyees", Value = "2"},
                                                                             
                                }, "Value", "Text");
            payroll.LocationHeadList = locationHeadBL.GetLocationHeadList();
            payroll.LocationList = locationBL.GetLocationList();

            return View(payroll);
        }
        #endregion
    }
}