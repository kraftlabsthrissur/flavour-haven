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
    public class ManufacturerController : Controller
    {
        private IStateContract stateBL;
        private IGeneralContract generalBL;
        private IManufacturerContract manufacturerBL;
        // GET: Masters/Manufacturer
        public ManufacturerController()
        {
            stateBL = new StateBL();
            generalBL = new GeneralBL();
            manufacturerBL = new ManufacturerBL();
        }
        public ActionResult Index()
        {
            List<ManufacturerModel> List = new List<ManufacturerModel>();
            List = manufacturerBL.GetManufacturerList().Select(a => new ManufacturerModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Phone=a.Phone
            }).ToList();
            return View(List);
        }
        public ActionResult Create()
        {
            ManufacturerModel Model = new ManufacturerModel();
            Model.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            Model.Code = generalBL.GetSerialNo("Manufacturer", "Code");
            return View(Model);
        }
        public ActionResult Save(ManufacturerModel model)
        {
            try
            {
                ManufacturerBO manufacturer = new ManufacturerBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name=model.Name,
                    AddressLine1=model.AddressLine1,
                    AddressLine2=model.AddressLine2,
                    StateID=model.StateID,
                    Place=model.Place,
                    Phone=model.Phone,
                    Description = model.Description,
                };
                if (manufacturer.ID == 0)
                {
                    manufacturerBL.Save(manufacturer);
                }
                else
                {
                    manufacturerBL.Update(manufacturer);
                }
                return Json(new { Status = "Success", Message = "Manufacturer Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Manufacturer Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Manufacturer failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int Id)
        {
            ManufacturerModel manufacturer = manufacturerBL.GetManufacturerDetails(Id).Select(m => new ManufacturerModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name=m.Name,
                AddressLine1=m.AddressLine1,
                AddressLine2=m.AddressLine2,
                State=m.State,
                Phone=m.Phone,
                Description = m.Description,
                Place=m.Place
            }).First();
            return View(manufacturer);
        }
        public ActionResult Edit(int Id)
        {
            ManufacturerModel manufacturer = manufacturerBL.GetManufacturerDetails(Id).Select(m => new ManufacturerModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                AddressLine1 = m.AddressLine1,
                AddressLine2 = m.AddressLine2,
                StateID = m.StateID,
                Phone = m.Phone,
                Description = m.Description,
                Place = m.Place

            }).First();
            manufacturer.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");   
            return View(manufacturer);
        }
    }
}