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
    public class OutPatientController : Controller
    {
        private IStateContract stateBL;
        private ICountryContract countryBL;
        private IOutPatientContract outPatientBL;
        private IGeneralContract generalBL;
        private IDistrictContract districtBL;

        public OutPatientController()
        {
            stateBL = new StateBL();
            countryBL = new CountryBL();
            outPatientBL = new OutPatientBL();
            generalBL = new GeneralBL();
            districtBL = new DistrictBL();
        }
        // GET: Masters/OutPatient
        public ActionResult Index()
        {
            List<OutPatientModel> PatientList = new List<OutPatientModel>();
            PatientList = outPatientBL.GetOutPatientList().Select(a => new OutPatientModel
            {
                ID = a.ID,
                Name = a.Name,
                Code = a.Code,
                Category = a.CategoryName,
                Registered = a.IsGSTRegistered == true ? "Yes" : "No"
            }).ToList();
            return View(PatientList);
        }

        public ActionResult Create()
        {
            OutPatientModel Model = new OutPatientModel();
            Model.Code = generalBL.GetSerialNo("Patients", "Code");
            Model.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            Model.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            Model.CategoryList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Patients", Value ="Patients", },
                                                 new SelectListItem { Text = "ECommerce", Value ="ECOMMERCE", },
                                                 }, "Value", "Text");
            return View(Model);
        }

        public ActionResult Save(OutPatientModel model)
        {
            {
                try
                {
                    CustomerBO CustomerBO = new CustomerBO()
                    {
                        ID = model.ID,
                        Name = model.Name,
                        Code = model.Code,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        StateID = model.StateID,
                        DistrictID = model.DistrictID,
                        CountryID = model.CountryID,
                        MobileNumber = model.MobileNumber,
                        EmailID = model.Email,
                        GstNo = model.GSTNo,
                        Category = model.Category,
                        PinCode = model.PinCode
                    };
                    if(model.DOB!= null)
                    { 
                    CustomerBO.DOB = General.ToDateTime(model.DOB);
                    }
                    outPatientBL.Save(CustomerBO);

                    return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public JsonResult GetSerialNo(string Category)
        {
            try
            {
                OutPatientModel Model = new OutPatientModel();
                Model.Code = generalBL.GetSerialNo(Category, "Code");
                return Json(new { Status = "success", Data = Model.Code }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int ID)
        {
            OutPatientModel OutPatient = outPatientBL.GetOutPatientDetails(ID).Select(m => new OutPatientModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Category = m.Category,
                AddressLine1 = m.AddressLine1,
                AddressLine2 = m.AddressLine2,
                State = m.State,
                District = m.District,
                MobileNumber = m.MobileNumber,
                Email = m.Email,
                GSTNo = m.GstNo,
                StateID = m.StateID,
                DistrictID = m.DistrictID,
                //DOB = General.FormatDate(m.DOB),
                DOB = m.DOB == null ? "" : General.FormatDate((DateTime)m.DOB),
                PinCode = m.PinCode
            }).First();
            return View(OutPatient);
        }

        public ActionResult Edit (int ID)
        {
            OutPatientModel OutPatient = outPatientBL.GetOutPatientDetails(ID).Select(m => new OutPatientModel()
            {
                ID = m.ID,
                Code = m.Code,
                Name = m.Name,
                Category = m.Category,
                AddressLine1 = m.AddressLine1,
                AddressLine2 = m.AddressLine2,
                State = m.State,
                District = m.District,
                MobileNumber = m.MobileNumber,
                Email = m.Email,
                GSTNo = m.GstNo,
                StateID = m.StateID,
                DistrictID = m.DistrictID,
                //DOB = General.FormatDate(m.DOB),
                DOB = m.DOB == null ? "" : General.FormatDate((DateTime)m.DOB),

                PinCode = m.PinCode
            }).First();
            OutPatient.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            OutPatient.CountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            OutPatient.CategoryList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Patients", Value ="Patients", },
                                                 new SelectListItem { Text = "ECommerce", Value ="ECOMMERCE", },
                                                 }, "Value", "Text");
            OutPatient.DistrictList = new SelectList(districtBL.GetDistrictList(OutPatient.StateID), "ID", "Name");
            return View(OutPatient);
        }

        public JsonResult GetOutPatientListForPopup(DatatableModel Datatable)
        {
            try
            {
                DatatableResultBO resultBO = outPatientBL.GetOutPatientListForPopup(Datatable.Columns[2].Search.Value, Datatable.Columns[3].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
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