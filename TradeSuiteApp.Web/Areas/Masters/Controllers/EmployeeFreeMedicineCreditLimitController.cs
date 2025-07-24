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
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class EmployeeFreeMedicineCreditLimitController : Controller
    {
        private ILocationContract locationBL;
        private ICategoryContract EmpCategoryBL;
        private IEmployeeFreeMedicineCreditLimitContract employeemedicinecreditlimitbl;
        public EmployeeFreeMedicineCreditLimitController()
        {
            EmpCategoryBL = new CategoryBL();
            locationBL = new LocationBL();
            employeemedicinecreditlimitbl= new EmployeeFreeMedicineCreditLimitBL();
        }
        // GET: Masters/EmployeeFreeMedicineLocationMapping
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            EmployeeFreeMedicineCreditLimitModel Employee = new EmployeeFreeMedicineCreditLimitModel();
            Employee.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            Employee.EmpCategoryList = new SelectList(EmpCategoryBL.GetEmployeeCategoryList(), "ID", "Name");
            Employee.Date = General.FormatDate(DateTime.Now);
            return View(Employee);
        }

        [HttpPost]
        public JsonResult GetEmployeeByFilterForFreeMedicineCreditLimit(int LocationID = 0, int EmployeeCategoryID = 0,int EmployeeID=0)
        {
            try
            {
                List<EmployeeFreeMedicineCreditLimitBO> Employeelist = employeemedicinecreditlimitbl.GetEmployeeByFilterForFreeMedicineCreditLimit(LocationID, EmployeeCategoryID, EmployeeID);
                return Json(new { Status = "success", Data = Employeelist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Save(EmployeeFreeMedicineCreditLimitModel model)
        {
            try
            {
                List<EmployeeFreeMedicineCreditLimitBO> EmployeeFreeMedicineCreditLimits = new List<EmployeeFreeMedicineCreditLimitBO>();
                EmployeeFreeMedicineCreditLimitBO EmployeeFreeMedicineCreditLimit;
                foreach (var item in model.Items)
                {
                    EmployeeFreeMedicineCreditLimit = new EmployeeFreeMedicineCreditLimitBO()
                    {
                       EmployeeID=item.EmployeeID,
                       Amount=item.Amount,
                       StartDate= General.ToDateTime(item.StartDate),
                       EndDate= General.ToDateTime(item.EndDate)
                    };

                    EmployeeFreeMedicineCreditLimits.Add(EmployeeFreeMedicineCreditLimit);
                }
                employeemedicinecreditlimitbl.Save(EmployeeFreeMedicineCreditLimits);

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
                          Status = "",
                          data = "",
                          message = e.Message
                      }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetEmployeeFreeMedicineCreditLimitList(DatatableModel Datatable)
        {
            try
            {
                string EmployeeCodeHint = Datatable.Columns[1].Search.Value;
                string EmployeeNameHint = Datatable.Columns[2].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = employeemedicinecreditlimitbl.GetEmployeeFreeMedicineCreditLimitList(EmployeeCodeHint, EmployeeNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}