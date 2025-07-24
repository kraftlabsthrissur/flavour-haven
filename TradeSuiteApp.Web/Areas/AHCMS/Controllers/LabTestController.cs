using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.AHCMS.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.AHCMS.Controllers
{
    public class LabTestController : Controller
    {
        private ILabTestContract labTestBL;

        public LabTestController()
        {
            labTestBL = new LabTestBL();
        }

        // GET: AHCMS/LabTest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int ID, int PatientID)
        {
            LabTestModel model = labTestBL.GetPatientDetails(ID, PatientID).Select(a => new LabTestModel()
            {
                PatientID = a.PatientID,
                Patient = a.Patient,
                PatientCode = a.PatientCode,
                Age = a.Age,
                Sex = a.Sex,
                Mobile = a.Mobile,
                Doctor = a.Doctor

            }).First();
        //    model.Items = labTestBL.GetLabTestItems(ID).Select(a => new LabTestItemModel()
        //    {
        //        ItemID = a.ItemID,
        //        ItemName = a.ItemName,
        //        BiologicalReference = a.BiologicalReference,
        //        ID = a.ID,
        //        Unit = a.Unit,
        //        Status = a.Status,
        //        ObserveValue = a.ObserveValue,
        //        StatusList = new SelectList(new List<SelectListItem>{
        //                                         new SelectListItem { Text = "Completed", Value = "Completed"}
        //                                         }, "Value", "Text")
        //}).ToList();
            model.Date = General.FormatDate(DateTime.Now);
            model.AppointmentProcessID = ID;
            return View(model);
        }

        public JsonResult GetLabTestList(DatatableModel Datatable)
        {
            try
            {
                string Date = Datatable.Columns[1].Search.Value;
                string PatientCode = Datatable.Columns[2].Search.Value;
                string Patient = Datatable.Columns[3].Search.Value;
                string LabTest = Datatable.Columns[4].Search.Value;
                string Doctor = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = labTestBL.GetLabTestList("","",Date, PatientCode, Patient, LabTest, Doctor, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(LabTestModel model)
        {
            try
            {
                List<LabTestItemBO> Items = new List<LabTestItemBO>();
                if (model.Items != null)
                {

                    LabTestItemBO labTestItem;

                    foreach (var item in model.Items)
                    {
                        labTestItem = new LabTestItemBO()
                        {
                            ID = item.ID,
                            Date = General.ToDateTime(item.Date),
                            ItemID = item.ItemID,
                            ObserveValue = item.ObserveValue,
                            Status = item.Status
                        };
                        Items.Add(labTestItem);
                    }
                }
                //labTestBL.Save(Items);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SaveLabTest(LabTestModel model)
        {
            try
            {
                List<LabtestsBO> LabTestItems = new List<LabtestsBO>();
                if (model.Items != null)
                {
                    LabtestsBO Item;

                    foreach (var item in model.Items)
                    {
                        Item = new LabtestsBO()
                        {
                            OPID = item.OPID,
                            IPID = item.IPID,
                            TestDate = General.ToDateTime(item.Date),
                            LabTestID = item.ItemID,
                            PatientID=item.PatientID
                        };
                        LabTestItems.Add(Item);
                    }
                }
                labTestBL.SaveLabTestItems(LabTestItems);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPatientLabTestID(int appoinmentprocessID)
        {
            List<LabTestBO> LabTest = labTestBL.GetPatientLabTestID(appoinmentprocessID).ToList();
            return Json(new { Status = "success", data = LabTest }, JsonRequestBehavior.AllowGet);
        }
    }
}