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
    public class DesignationController : Controller
    {

        private IDesignationContract designationBL;
        private IDepartmentContract departmentBL;
        private IGeneralContract generalBL;
        public DesignationController()
        {
            designationBL = new DesignationBL();
            departmentBL = new DepartmentBL();
            generalBL = new GeneralBL();
        }
        // GET: Masters/Designation
        public ActionResult Index()
        {
            List<DesignationModel> DepartmentList = new List<DesignationModel>();
            DepartmentList = designationBL.GetDesignationList().Select(a => new DesignationModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                DepartmentName = a.DepartmentName,
                IsActive = a.IsActive,
                StartDate = General.FormatDate(a.StartDate, "view"),
                EndDate = General.FormatDate(a.EndDate, "view")
            }).ToList();

            return View(DepartmentList);
        }
        // GET: Masters/Designation/Details
        public ActionResult Details(int Id)
        {
            DesignationModel Designation = designationBL.GetDesignationDetails(Id).Select(m => new DesignationModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                IsActive = m.IsActive,
                DepartmentName=m.DepartmentName,
                StartDate = General.FormatDate(m.StartDate, "view"),
                EndDate = General.FormatDate(m.EndDate, "view")
            }).First();

            if (Designation.IsActive == true)
            {
                Designation.State = "Active";
            }
            else
            {
                Designation.State = "Deactive";
            }
            return View(Designation);
        }
        // GET: Masters/Designation/Edit
        public ActionResult Edit(int Id)
        {
            DesignationModel Designation = designationBL.GetDesignationDetails(Id).Select(m => new DesignationModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                IsActive = m.IsActive,
                DepartmentID = m.DepartmentID,
                StartDate = General.FormatDate(m.StartDate),
                EndDate = General.FormatDate(m.EndDate)
            }).First();
            Designation.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            return View(Designation);
        }
        // GET: Masters/Designation/Create
        public ActionResult Create()
        {
            DesignationModel Designation = new DesignationModel();
            Designation.DepartmentList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            Designation.StartDate = General.FormatDate(DateTime.Now);
            Designation.EndDate = General.FormatDate(Convert.ToDateTime(generalBL.GetConfig("DefaultDate")));

            return View(Designation);
        }
        // Post: Masters/Department/Save
        public ActionResult Save(DesignationModel model)
        {
            try
            {
                DesignationBO Designation = new DesignationBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    StartDate = General.ToDateTime(model.StartDate),
                    EndDate = General.ToDateTime(model.EndDate),
                    DepartmentID = model.DepartmentID,
                    IsActive = model.IsActive

                };
                if (Designation.ID == 0)
                {
                    designationBL.Save(Designation);
                }
                else
                {
                    designationBL.Update(Designation);
                }
                return Json(new { Status = "Success", Message = "Designation Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Designation Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save Designation failed" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}