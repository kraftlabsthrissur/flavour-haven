using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class SerialNumberController : Controller
    {
        ILocationContract locationBL;
        ISerialNo serialNumberBL;
        public SerialNumberController()
        {
            locationBL = new LocationBL();
            serialNumberBL = new SerialNumberBL();

        }
        // GET: Masters/SerialNumber
        public ActionResult Index()
        {
            List<SerialNumberModel> serialNumberList = new List<SerialNumberModel>();
            return View(serialNumberList);
        }

        // GET: Masters/SerialNumber/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Masters/SerialNumber/Create
        public ActionResult Create()
        {
            SerialNumberModel serial = new SerialNumberModel();
            serial.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            serial.MasterList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "NO", Value = "0"},
                                                 new SelectListItem { Text = "Yes", Value = "1"}
                                                 }, "Value", "Text");
            return View(serial);
        }

        // POST: Masters/SerialNumber/Save
        [HttpPost]
        public ActionResult Save(SerialNumberModel model)
        {
            try
            {
                SerialNumberBO serialNo = new SerialNumberBO()
                {
                    ID = model.ID,
                    FormName = model.FormName,
                    Field = model.Field,
                    LocationID = model.LocationID,
                    Prefix = model.Prefix,
                    SpecialPrefix = model.SpecialPrefix,
                    FinYearPrefix = model.FinYearPrefix,
                    Value = model.Value,
                    IsLeadingZero = model.IsLeadingZero,
                    NumberOfDigits = model.NumberOfDigits,
                    Suffix = model.Suffix,
                    IsMaster = model.IsMaster
                };

                var i = serialNumberBL.CreateSerialNumber(serialNo);

                if (i == 0)
                {
                    return Json(new { Status = "Success", Message = "Serial number created  Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "fail", Message = "Serial number already exist" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Masters/SerialNumber/Edit/5
        public ActionResult Edit()
        {
            NewFinYearModel serial = new NewFinYearModel();
            int finyear = GeneralBO.FinYear + 1;
            serial.NewFinYear = GeneralBO.FinYear;
            serial.NewFinPrefix = GeneralBO.FinYear.ToString().Substring(2, 2).ToString() + (finyear.ToString().Substring(2, 2).ToString());
            return View(serial);
        }

        // POST: Masters/SerialNumber/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Masters/SerialNumber/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Masters/SerialNumber/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public JsonResult GetFinYearDetails(DatatableModel Datatable)
        {
            NewFinYearModel serial = new NewFinYearModel();
            string Nextyear = DateTime.Now.AddYears(1).ToShortDateString();
            var month = DateTime.Now.Month;
            serial.NewFinYear = GeneralBO.FinYear;
            serial.NewFinPrefix = GeneralBO.FinYear.ToString().Substring(2, 2).ToString() + (GeneralBO.FinYear.ToString().Substring(2, 2).ToString() + 1);
            string FormHint = Datatable.Columns[1].Search.Value;

            string LocationHint = Datatable.Columns[3].Search.Value;
            string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
            string SortOrder = Datatable.Order[0].Dir;
            int Offset = Datatable.Start;
            int Limit = Datatable.Length;
            DatatableResultBO resultBO = serialNumberBL.GetSerialNumberByFinYear(GeneralBO.FinYear, FormHint, LocationHint, SortField, SortOrder, Offset, Limit);
            var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdateFinYearAndFinPrefix(NewFinYearModel modal)
        {
            try
            {

                var SerialList = new List<SerialNumberBO>();
                SerialNumberBO serialBO;
                foreach (var itm in modal.Trans)
                {
                    serialBO = new SerialNumberBO();
                    serialBO.ID = itm.ID;
                    serialBO.FormName = itm.FormName;
                    serialBO.Field = itm.Field;
                    serialBO.LocationID = itm.LocationID;
                    serialBO.Prefix = itm.Prefix == null ? "" : itm.Prefix;
                    serialBO.SpecialPrefix = itm.SpecialPrefix == null ? "" : itm.SpecialPrefix;
                    serialBO.FinYearPrefix = itm.FinYearPrefix;
                    serialBO.Value = itm.Value;
                    serialBO.IsLeadingZero = itm.IsLeadingZero;
                    serialBO.Suffix = itm.Suffix == null ? " " : itm.Suffix;
                    serialBO.LocationPrefix = itm.LocationPrefix;
                    serialBO.NumberOfDigits = itm.NumberOfDigits;
                    serialBO.NewFinYear = itm.FinYear;
                    serialBO.IsMaster = itm.IsMaster;
                    SerialList.Add(serialBO);
                }
                var outId = serialNumberBL.UpdateFinYearAndFinPrefix(SerialList);
                return Json(new { Status = "Success", Message = "Financial Year Updated  Successfully" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {

                return Json(new { Status = "failure", Message = "Failed to update serialno" }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetSerialNumberList(DatatableModel Datatable)
        {
            try
            {
                string FormHint = Datatable.Columns[1].Search.Value;
                string PrefixHint = Datatable.Columns[2].Search.Value;
                string LocationPrefixHint = Datatable.Columns[3].Search.Value;
                string FinYearPrefixHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = serialNumberBL.GetSerialNumberList(FormHint, PrefixHint, LocationPrefixHint, FinYearPrefixHint, SortField, SortOrder, Offset, Limit);
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
