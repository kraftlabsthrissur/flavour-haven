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
    public class AttendanceController : Controller
    {
        #region Private members
        private IDepartmentContract departmentBL;
        private ICategoryContract CategoryBL;
        private ILocationContract locationBL;
        private IEmployeeContract employeeBL; 
        private IAttendanceContract atendanceBL;
        #endregion
        #region condtructors
        public AttendanceController()
        {
            departmentBL = new DepartmentBL();
            CategoryBL = new CategoryBL();
            locationBL = new LocationBL();
            employeeBL = new EmployeeBL();
            atendanceBL = new AttendanceBL();
        }
        #endregion
        #region public methods
        // GET: Payroll/Attendance
        public ActionResult Index()
        {
            AttendanceModel model = new AttendanceModel();
            List<AttendanceModel> Attendance = employeeBL.GetEmployeeList().Select(a => new AttendanceModel
            {

                ID = a.ID,
                EmployeeCode = a.Code,
                EmployeeName = a.Name,
                TotalLeave = 20,
                BalanceLeave = 10

            }).ToList();
            return View(Attendance);
        }

        // GET: Payroll/Attendance/Edit
        public ActionResult Edit()
        {
            AttendanceModel model = new AttendanceModel();

            model.EmployeeCategoryList = new SelectList(CategoryBL.GetEmployeeCategoryList(), "ID", "Name");
            model.Departmentlist = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            model.PayrollCategoryList = new SelectList(CategoryBL.GetPayrollCategoryList(), "ID", "Name");
            model.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            model.MonthList = new SelectList(atendanceBL.GetMonthList(), "ID", "Name");
            return View(model);
        }

        // GET: Payroll/Attendance/Details
        public ActionResult Details()
        {
            AttendanceModel Attendance = new AttendanceModel();
            Attendance.EmployeeList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            return View(Attendance);
        }
        #endregion
    }
}