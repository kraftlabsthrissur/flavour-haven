using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class DepartmentController : Controller
    {
        private IDepartmentContract departmentBL;
        public DepartmentController()
        {
            departmentBL = new DepartmentBL();
        }
        // GET: Masters/Department
        public ActionResult Index()
        {
            List<DepartmentModel> DepartmentList = new List<DepartmentModel>();
            DepartmentList = departmentBL.GetAllDepartment().Select(a => new DepartmentModel
            {
                ID = a.ID,
                Code=a.Code,
                Name=a.Name
            }).ToList();

            return View(DepartmentList);
        }

        // GET: Masters/Department/Create
        public ActionResult Create()
        {
            DepartmentModel Department = new DepartmentModel();
            Department.DepartmentGroupList = new SelectList(departmentBL.GetDepartmentGroupList(), "ID", "Name");
            Department.StartDate =General.FormatDate(DateTime.Now);
            Department.EndDate = General.FormatDate(DateTime.Now);

            return View(Department);
        }

        // Post: Masters/Department/Save
        public ActionResult Save(DepartmentModel model)
        {
            try
            {
                DepartmentBO Department = new DepartmentBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    StartDate = General.ToDateTime(model.StartDate),
                    EndDate = General.ToDateTime(model.EndDate),
                    DepartmentGroupID = model.DepartmentGroupID,
                    IsActive = model.IsActive

                };
                if (Department.ID == 0)
                {
                    departmentBL.Save(Department);
                }
                else
                {
                    departmentBL.UpdateDepartment(Department);
                }
                return Json(new { Status = "Success", Message = "Department Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Department Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save department failed" },  JsonRequestBehavior.AllowGet);
            }
           
        }
        // GET: Masters/Department/Details
        public ActionResult Details(int Id)
        {
            DepartmentModel Department = departmentBL.GetDepartmentDetails(Id).Select(m => new DepartmentModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                DepartmentGroup=m.DepartmentGroup,
                StartDate = General.FormatDate(m.StartDate,"view"),
                EndDate = General.FormatDate(m.EndDate,"view"),
                IsActive=m.IsActive
            }).First();
            
            if(Department.IsActive == true)
            {
                Department.State = "Active";
            }
            else
            {
                Department.State = "Deactive";
            }
            return View(Department);
        }

        // GET: Masters/Department/Edit
        public ActionResult Edit(int Id)
        {
            DepartmentModel Department = departmentBL.GetDepartmentDetails(Id).Select(m => new DepartmentModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                DepartmentGroupID = m.DepartmentGroupID,
                StartDate = General.FormatDate(m.StartDate),
                EndDate = General.FormatDate(m.EndDate),
                IsActive=m.IsActive
                
            }).First();          
            Department.DepartmentGroupList = new SelectList(departmentBL.GetDepartmentGroupList(), "ID", "Name");
            return View(Department);
        }

        public JsonResult GetPatientDepartment()
        {
            List<DepartmentBO> Department = new List<DepartmentBO>();
            //{
            //    new GSTCategoryBO()
            //    {
            //        ID = 0,
            //        IGSTPercent = 0
            //    }
            //};
            Department.AddRange(departmentBL.GetPatientDepartment());
            return Json(new { Status = "success", data = Department }, JsonRequestBehavior.AllowGet);
        }
    }
}