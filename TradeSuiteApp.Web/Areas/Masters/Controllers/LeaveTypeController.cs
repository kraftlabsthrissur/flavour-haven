using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class LeaveTypeController : Controller
    {
        // GET: Masters/LeaveType
        public ActionResult Index()
        {
            List<LeaveTypeModel> leavetypelist = new List<LeaveTypeModel>()
            {
                new LeaveTypeModel()
                {
                    ID=1,
                    Code="CL",
                    Name="Casual Leave",
                    IsEncashable=false,
                    CanCarryForward=true,
                    CreditFrequecy = "Yearly"
                }
            };
            return View(leavetypelist);
        }





        public ActionResult Details(int id)
        {
            LeaveTypeModel leaveType = new LeaveTypeModel();

            leaveType.ID = 1;
            leaveType.Code = "CL";
            leaveType.Name = "Casual Leave";
            leaveType.CreditFrequecy = "Yearly";
            leaveType.NoOfDays = 2;
            leaveType.CanCarryForward = true;
            leaveType.IsEncashable = false;
            leaveType.MinServicePeriod = 10;
            leaveType.MaxLeaveBalance = 4;
            leaveType.MinInstanceDays = 3;
            leaveType.MaxInstanceDays = 6;
            leaveType.IsLeavePartOfWeeklyOff = true;
            leaveType.IsLeavePartOfHoliday = false;

            return View(leaveType);

        }

        // GET: Masters/LeaveType
        public ActionResult Create()
        {
            LeaveTypeModel leaveTypeModel = new LeaveTypeModel();
            leaveTypeModel.CreditFrequecyList = new SelectList(
                                 new List<SelectListItem>
                                 {
                                       new SelectListItem { Text = "Monthly", Value = "Monthly"},
                                       new SelectListItem { Text = "Yearly", Value = "Yearly"},

                                 }, "Value", "Text");
            return View(leaveTypeModel);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

    }
}