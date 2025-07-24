using BusinessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class DACalculationsController : Controller
    {
        ILocationContract locationBL;
        ICategoryContract categoryBL;
        IEmployeeContract employeeBL;
        IDepartmentContract departmentBL;
        public DACalculationsController()
        {
            locationBL = new LocationBL();
            categoryBL = new CategoryBL();
            employeeBL = new EmployeeBL();
            departmentBL = new DepartmentBL();
        }
        // GET: Masters/DA/Index
        public ActionResult Index()
        {
            List<DACalculationsModel> dacalculist = new List<DACalculationsModel>()
            {
                new DACalculationsModel()
                {
                    ID=1,
                    Code="0001",
                    EmployeeCategory="Indirect Workers",
                    EmployeeStatus="Permanent",
                    PayrollCategory="Maintenance Managers",
                    Department="Human Resource & Admin",
                    Location="Secunderabad ",
                    Month="JAN 2019",
                    StartDateStr="07/01/2019",
                    EndDateStr="07/01/2019"
                }
            };
            return View(dacalculist);
        }

        public ActionResult Details()
        {
            DACalculationsModel dacalcu = new DACalculationsModel();

            dacalcu.ID = 1;
            dacalcu.Code = "DAC0001";
            dacalcu.Location = "Secunderabad ";
            dacalcu.EmployeeCategory="Indirect Workers";
            dacalcu.EmployeeStatus = "Permanent";
            dacalcu.PayrollCategory = "Maintenance Managers";
            dacalcu.Department = "Human Resource & Admin";
            dacalcu.Month = "JAN 2019";
            dacalcu.BasicPoints = 1000;
            dacalcu.AdditionalPoints = 50;
            dacalcu.BasicValue = 1050;
            dacalcu.AdditionalValue =50;
            dacalcu.StartDateStr = "07/01/2019";
            dacalcu.EndDateStr = "07/01/2019";
            return View(dacalcu);
        }

        // GET: Masters/DA/Create
        public ActionResult Create()
        {
            DACalculationsModel dacalc = new DACalculationsModel();
            dacalc.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            dacalc.EmployeeCategoryList = new SelectList(categoryBL.GetEmployeeCategoryList(), "ID", "Name");
            dacalc.EmployeeStatusList = new SelectList(employeeBL.GetEmployeeJobTypeList(), "EmploymentJobTypeID", "EmploymentJobType");
            dacalc.PayrollCategoryList = new SelectList(categoryBL.GetPayrollCategoryList(), "ID", "Name");
            dacalc.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            //dacalc.Month = "May 2018";
            return View(dacalc);
        }
        //[HttpPost]
        //public ActionResult Create()
        //{
        //    return View();
        //}
    }
}