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
    public class InterCompanyController : Controller
    {
        private IInterCompanyContract interCompanyBL;

        public InterCompanyController()
        {
            this.interCompanyBL = new InterCompanyBL();
        }


        // GET: Masters/InterCompany
        public ActionResult Index()
        {
            List<InterCompanyModel> InterCompanyList = new List<InterCompanyModel>();
            InterCompanyList = interCompanyBL.GetInterCompanyList().Select(a => new InterCompanyModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name
            }).ToList();

            return View(InterCompanyList);         
        }
        public ActionResult Create()
        {
            InterCompanyModel InterCompany = new InterCompanyModel();

            InterCompany.StartDate = General.FormatDate(DateTime.Now);
            InterCompany.EndDate = General.FormatDate(DateTime.Now);

            return View(InterCompany);
        }
        public ActionResult Save(InterCompanyModel model)
        {
            try
            {
                InterCompanyBO InterCompany = new InterCompanyBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    Description=model.Description,
                    StartDate=General.ToDateTime(model.StartDate),
                    EndDate= General.ToDateTime(model.EndDate)

                };
                if (InterCompany.ID == 0)
                {
                    interCompanyBL.Save(InterCompany);
                }
                else
                {
                    interCompanyBL.UpdateDepartment(InterCompany);
                }
                return Json(new { Status = "Success", Message = "InterCompany Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "InterCompany Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Failed to save" }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Details(int Id)
        {
            InterCompanyModel InterCompany = interCompanyBL.GetInterCompanyDetails(Id).Select(m => new InterCompanyModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description= m.Description,
                StartDate = General.FormatDate(m.StartDate, "view"),
                EndDate = General.FormatDate(m.EndDate, "view"),          
            }).First();
            return View(InterCompany);        
        }
        public ActionResult Edit(int Id)
        {
            InterCompanyModel InterCompany = interCompanyBL.GetInterCompanyDetails(Id).Select(m => new InterCompanyModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                StartDate = General.FormatDate(m.StartDate),
                EndDate = General.FormatDate(m.EndDate),
            }).First();

            return View(InterCompany);
        }
    }
}