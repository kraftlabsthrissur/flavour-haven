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
    public class ProjectController : Controller
    {
        private IProjectContract projectBL;

        public ProjectController()
        {
            projectBL = new ProjectBL();

        }

        public ActionResult Index()
        {
            List<ProjectModel> ProjectList = new List<ProjectModel>();
            ProjectList = projectBL.GetProjectList().Select(a => new ProjectModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return View(ProjectList);

        }

        public ActionResult Create()
        {
            ProjectModel projectModel = new ProjectModel();
            projectModel.StartDate = General.FormatDate(DateTime.Now);
            projectModel.EndDate = General.FormatDate(DateTime.Now);
            return View(projectModel);
        }

        public ActionResult Save(ProjectModel model)
        {
            try
            {
                ProjectBO ProjectBO = new ProjectBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = General.ToDateTime(model.StartDate),
                    EndDate = General.ToDateTime(model.EndDate),
                };
                projectBL.Save(ProjectBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int? ID)
        {
            var obj = projectBL.GetProjectDetails((int)ID);
            ProjectModel model = new ProjectModel();
            model.ID = obj.ID;
            model.Code = obj.Code;
            model.Name = obj.Name;
            model.Description = obj.Description;
            model.StartDate = General.FormatDate(obj.StartDate);
            model.EndDate = General.FormatDate(obj.EndDate);
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var obj = projectBL.GetProjectDetails((int)ID);
            ProjectModel model = new ProjectModel();
            model.ID = obj.ID;
            model.Code = obj.Code;
            model.Name = obj.Name;
            model.Description = obj.Description;
            model.StartDate = General.FormatDate(obj.StartDate);
            model.EndDate = General.FormatDate(obj.EndDate);
            return View(model);
        }
    }
}