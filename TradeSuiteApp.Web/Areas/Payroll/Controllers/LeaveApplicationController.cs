using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Payroll.Models;

namespace TradeSuiteApp.Web.Areas.Payroll.Controllers
{
    public class LeaveApplicationController : Controller
    {
        ILeaveTypeContract leaveTypeBL;
        IEmployeeContract employeeBL;
        IDepartmentContract departmentBL;
        ICategoryContract categoryBL;

        public LeaveApplicationController()
        {
            leaveTypeBL = new LeaveTypeBL();
            employeeBL = new EmployeeBL();
            departmentBL = new DepartmentBL();
            categoryBL = new CategoryBL();
        }

        // GET: Payroll/LeaveApplication/Index
        public ActionResult Index()
        {
            List<LeaveApplicationModel> leaveapplication = new List<LeaveApplicationModel>()
            {
                new LeaveApplicationModel()
                {
                    ID=1,
                    EmployeeName="Rajesh",
                    EmpCategory="Indirect Workers",
                    LeaveType="Casual Leave",
                    NoOfDays="1",
                    StartDateStr="07/01/2019",
                    EndDateStr="08/01/2019"
                }
            };
            return View(leaveapplication);
        }

        // GET: Payroll/LeaveApplication/Details
        public ActionResult Details()
        {
            LeaveApplicationModel leaveApplication = new LeaveApplicationModel();
            leaveApplication.ID = 1;
            leaveApplication.EmpCategory = "Indirect Workers";
            leaveApplication.EmployeeName = "Rajesh";
            leaveApplication.Department = "Material Store";
            leaveApplication.ReportingToName = "Yousuf Ali";
            leaveApplication.LoginName = "Admin";
            leaveApplication.Department = "Human Resource & Admin";
            leaveApplication.LeaveType = "Casual Leave";
            leaveApplication.NoOfDays = "2";
            leaveApplication.Reason = "Fever";
            leaveApplication.StartDateStr = "07/01/2019";
            leaveApplication.EndDateStr = "07/01/2019";
            leaveApplication.LeaveAvailable = "15";
            leaveApplication.BalanceAvailable = "10";
            leaveApplication.IsStartForenoon = true;
            leaveApplication.IsStartAfterenoon = false;
            leaveApplication.IsEndForenoon = true;
            leaveApplication.IsEndAfterenoon = false;
            return View(leaveApplication);
        }

        // GET: Payroll/LeaveApplication/Create
        public ActionResult Create()
        {
            LeaveApplicationModel leaveApplication = new LeaveApplicationModel();
            leaveApplication.EmpCategoryList = new SelectList(categoryBL.GetEmployeeCategoryList(), "ID", "Name");
            leaveApplication.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            leaveApplication.ReportingToName = "Yousuf Ali";
            leaveApplication.LoginID = GeneralBO.CreatedUserID;
            leaveApplication.LoginNameList = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            leaveApplication.LeaveTypeList = new SelectList(leaveTypeBL.GetLeaveTypeList(), "LeaveTypeID", "Name");
            return View(leaveApplication);
        }

       
    }
}