using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class PatientController : Controller
    {
        private IPatientContract patientBL;
        private IGeneralContract generalBL;
        public PatientController()
        {
            patientBL = new PatientBL();
            generalBL = new GeneralBL();
        }
        public JsonResult GetPatientList(DatatableModel Datatable)
        {
            try
            {
                DatatableResultBO resultBO = patientBL.GetPatientList(Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPatientListForPopup(DatatableModel Datatable)
        {
            try
            {
                DatatableResultBO resultBO = patientBL.GetPatientList(Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetPatientAutoComplete(string Hint = "", int CustomerCategoryID = 0)
        {
            DatatableResultBO resultBO = patientBL.GetPatientList("", Hint, "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        // GET: Masters/Patient
        public ActionResult Index()
        {
            return View();
        }

        // GET: Masters/Patient/Details/5
        public ActionResult Details(int ID)
        {
            PatientModel Patient = patientBL.GetPatientDetails(ID).Select(m => new PatientModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Age = m.Age,
                Sex = m.Sex,
                DOB =m.DOB!=null? General.FormatDate((DateTime)m.DOB, "view"):null,
                Address1 = m.Address1,
                Address2 = m.Address2,
                Place = m.Place,
                Email = m.Email,
                Mobile = m.Mobile,
                PinCode = m.PinCode,
                DoctorID = m.DoctorID,
                DoctorName = m.DoctorName
            }).First();
            return View(Patient);
        }



        // GET: Masters/Patient/Create
        public ActionResult Create()
        {
            PatientModel model = new PatientModel();
            model.Code = generalBL.GetSerialNo("Patient", "Code");
            model.PatientSexList = new SelectList(
              new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");

            return View(model);
        }

        // POST: Masters/Patient/Create
        [HttpPost]
        public ActionResult Save(PatientModel model)
        {
            {
                try
                {
                    // TODO: Add insert logic here
                    PatientBO patientBO = new PatientBO()
                    {
                        ID = model.ID,
                        Name = model.Name,
                        Code = model.Code,
                        DoctorID = model.DoctorID,                      
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        Age = model.Age,
                        PinCode = model.PinCode,
                        Place = model.Place,
                        Sex = model.Sex,
                        Mobile = model.Mobile,
                        Email = model.Email,
                        DoctorName=model.DoctorName

                    };
                    if(model.DOB!=null)
                    {
                        patientBO.DOB= General.ToDateTime(model.DOB);
                    }
                    int patientId = patientBL.Save(patientBO);

                    return Json(new { Status = "success", Data = patientId }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        // GET: Masters/Patient/Edit/5
        public ActionResult Edit(int ID)
        {

            PatientModel Patient = patientBL.GetPatientDetails(ID).Select(m => new PatientModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Age = m.Age,
                Sex = m.Sex,
                DOB = m.DOB != null ? General.FormatDate((DateTime)m.DOB, "view") : null,
                Address1 = m.Address1,
                Address2 = m.Address2,
                Place = m.Place,
                Email = m.Email,
                Mobile = m.Mobile,
                PinCode = m.PinCode,
                DoctorID = m.DoctorID,
                DoctorName = m.DoctorName
            }).First();
            Patient.PatientSexList = new SelectList(
             new List<SelectListItem> {
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Male", Value = "M" }}, "Value", "Text");
            return View(Patient);
        }
    }
}
