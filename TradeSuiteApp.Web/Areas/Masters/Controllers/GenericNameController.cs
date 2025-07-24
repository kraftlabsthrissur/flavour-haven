using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class GenericNameController : Controller
    {
        private IGeneralContract generalBL;
        private IGenericNameContract genericNameBL;
        // GET: Masters/GenericName
        public GenericNameController()
        {
            generalBL = new GeneralBL();
            genericNameBL = new GenericNameBL();
        }
        public ActionResult Index()
        {
            List<GenericNameModel> List = new List<GenericNameModel>();
            List = genericNameBL.GetGenericNameList().Select(a => new GenericNameModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name
            }).ToList();
            return View(List);
        }
        public ActionResult Create()
        {
            GenericNameModel model = new GenericNameModel();
            model.Code = generalBL.GetSerialNo("GenericName", "Code");
            return View(model);
        }
        public ActionResult Save(GenericNameModel model)
        {
            try
            {
                GenericNameBO GenericName = new GenericNameBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    Description = model.Description
                };
                if (GenericName.ID == 0)
                {
                    genericNameBL.Save(GenericName);
                }
                else
                {
                    genericNameBL.Update(GenericName);
                }
                return Json(new { Status = "Success", Message = "GenericName Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "GenericName Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save GenericName failed" }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Details(int Id)
        {
            GenericNameModel GenericName = genericNameBL.GetGenericNameDetails(Id).Select(m => new GenericNameModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description
            }).First();
            return View(GenericName);
        }

        public ActionResult Edit(int Id)
        {
            GenericNameModel GenericName = genericNameBL.GetGenericNameDetails(Id).Select(m => new GenericNameModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description
            }).First();
            return View(GenericName);
        }
    }
}