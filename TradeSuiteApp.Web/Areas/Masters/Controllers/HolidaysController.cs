using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class HolidaysController : Controller
    {
        // GET: Masters/Holiday/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: Masters/Holiday/Detail
        public ActionResult Details()
        {
            return View();
        }

        // GET: Masters/Holiday/Create
        public ActionResult Create()
        {
            HolidaysModel holidaysModel = new HolidaysModel();
            holidaysModel.LeaveYear = "01/04/2019 - 31/03/20120";
            holidaysModel.DayList = new SelectList(
                                 new List<SelectListItem>
                                 {
                                       new SelectListItem { Text = "Sunday",    Value = "Sunday"},
                                       new SelectListItem { Text = "Monday",    Value = "Monday"},
                                       new SelectListItem { Text = "Tuesday",   Value = "Tuesday"},
                                       new SelectListItem { Text = "Wednesday", Value = "Wednesday"},
                                       new SelectListItem { Text = "Thursday",  Value = "Thursday"},
                                       new SelectListItem { Text = "Friday",    Value = "Friday"},
                                       new SelectListItem { Text = "Saturday",  Value = "Saturday"},

                                 }, "Value", "Text");
            List<SelectListItem> weeks = new List<SelectListItem>();
            weeks.Add(new SelectListItem { Text = "1st Week", Value = "1" });
            weeks.Add(new SelectListItem { Text = "2nd Week", Value = "2" });
            weeks.Add(new SelectListItem { Text = "3rd Week", Value = "3" });
            weeks.Add(new SelectListItem { Text = "4th Week", Value = "4" });
            weeks.Add(new SelectListItem { Text = "5th Week", Value = "5" });
            holidaysModel.WeekList = weeks;
            List<SelectListItem> names = new List<SelectListItem>();
            names.Add(new SelectListItem { Text = "All Location", Value = "" });
            names.Add(new SelectListItem { Text = "VOS TKY", Value = "1" });
            names.Add(new SelectListItem { Text = "VOS POL", Value = "2" });
            names.Add(new SelectListItem { Text = "VOS CHU", Value = "3" });
            names.Add(new SelectListItem { Text = "VOS NHE", Value = "4" });
            names.Add(new SelectListItem { Text = "VOS FDN", Value = "5" });
            names.Add(new SelectListItem { Text = "Kottappuram", Value = "6" });
            holidaysModel.LocationList = names;
            holidaysModel.WeeklyHoliday = "Sunday";
            holidaysModel.Week = "Ist Week";
            holidaysModel.Dates = "04/01/2019";
            holidaysModel.GeneralHoliday = "Friday";
            holidaysModel.HolidayName = "Hartal";
            holidaysModel.HolidayType = "General";
            holidaysModel.Islocation = true;
            return View(holidaysModel);
        }

        // GET: Masters/Holiday/Edit
        public ActionResult Edit()
        {
            return View();
        }
    }
}