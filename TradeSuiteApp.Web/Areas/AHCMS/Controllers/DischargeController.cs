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

namespace TradeSuiteApp.Web.Areas.AHCMS.Controllers
{
    public class DischargeController : Controller
    {
        private IGeneralContract generalBL;
        private IDischargeContract dischargeBL;

        public DischargeController()
        {
            generalBL = new GeneralBL();
            dischargeBL = new DischargeBL();
        }
        // GET: AHCMS/Discharge
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int IPID)
        {
            DischargeModel dischargeModel = new DischargeModel();
            var obj = dischargeBL.GetDischargeSummaryDetails(IPID);
            dischargeModel.Course = obj.Course;
            dischargeModel.Condition = obj.Condition;
            dischargeModel.Diet = obj.Diet;
            dischargeModel.IPID = IPID;
            dischargeModel.IsDischarged = obj.IsDischarged;
            return View(dischargeModel);
        }

        public JsonResult GetDischargeAdvicedInpatientList(DatatableModel Datatable)
        {
            try
            {
                string AdmissionDate = Datatable.Columns[1].Search.Value;
                string Patient = Datatable.Columns[2].Search.Value;
                string Room = Datatable.Columns[3].Search.Value;
                string Doctor = Datatable.Columns[4].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = dischargeBL.GetDischargeAdvicedInpatientList(Patient, Room, AdmissionDate, Doctor, Type, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "Discharge", "GetDischargeAdvicedInpatientList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult MedicineList(int IPID)
        {
            DischargeModel model = new DischargeModel();
            model.MedicineList = dischargeBL.GetMedicineList(IPID).Select(m => new DischargeModelItems()
            {
                Medicine=m.Medicine,
                Qty=m.Qty,
                Unit=m.Unit,
                Instructions=m.Instructions
            }).ToList();
            return PartialView(model);
        }

        public JsonResult IsBillPaid(int IPID)
        {
            bool IsBillPaid;
            IsBillPaid = dischargeBL.IsBillPaid(IPID);
            return Json(new { Status = "success", IsBillPaid = IsBillPaid }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Discharge(int IPID)
        { 
            dischargeBL.Discharge(IPID);
            return Json(new { Status = "success"}, JsonRequestBehavior.AllowGet);
        }
    }
}