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
    public class TreatmentController : Controller
    {
        private ISubmasterContract subMasterBL;
        private ITreatmentContract treatmentBL;
        private IGeneralContract generalBL;
        // GET: Masters/Treatment
        public TreatmentController()
        {
            subMasterBL = new SubmasterBL();
            treatmentBL = new TreatmentBL();
            generalBL = new GeneralBL();
        }
        public ActionResult Index()
        {
            List<TreatmentModel> TreatmentList = new List<TreatmentModel>();
            TreatmentList = treatmentBL.GetAllTreatment().Select(a => new TreatmentModel
            {
                ID = a.ID,
                TreatmentCode = a.TreatmentCode,
                TreatmentName = a.TreatmentName
            }).ToList();
            return View(TreatmentList);
        }
        public ActionResult Create()
        {
            TreatmentModel treatmentModel = new TreatmentModel();
            treatmentModel.TreatmentGroupList = new SelectList(subMasterBL.GetTreatmentGroupList(), "ID", "Name");
            treatmentModel.TreatmentCode = generalBL.GetSerialNo("Treatment", "Code");
            return View(treatmentModel);
        }
        // Post: Masters/Treatment/Save
        public ActionResult Save(TreatmentModel model)
        {
            try
            {
                TreatmentListBO Treatment = new TreatmentListBO()
                {
                    ID = model.ID,
                    TreatmentCode = model.TreatmentCode,
                    TreatmentName = model.TreatmentName,
                    TreatmentGroupID = model.TreatmentGroupID,
                    AddedDate = General.ToDateTime(model.AddedDate),
                    Description = model.Description
                };
                if (Treatment.ID == 0)
                {
                    treatmentBL.Save(Treatment);
                }
                else
                {
                    treatmentBL.UpdateTreatment(Treatment);
                }
                return Json(new { Status = "Success", Message = "Treatment Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Treatment Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save Treatment failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Details(int Id)
        {
            TreatmentModel treatment = treatmentBL.GetTreatmentDetails(Id).Select(m => new TreatmentModel()
            {
                ID = m.ID,
                TreatmentCode = m.TreatmentCode,
                TreatmentName = m.TreatmentName,
                AddedDate = General.FormatDate(m.AddedDate, "view"),
                Description = m.Description
            }).First();
            return View(treatment);
        }
        // GET: Masters/Treatment/Edit
        public ActionResult Edit(int Id)
        {
            TreatmentModel Treatment = treatmentBL.GetTreatmentDetails(Id).Select(m => new TreatmentModel()
            {
                ID = m.ID,
                TreatmentCode = m.TreatmentCode,
                TreatmentName = m.TreatmentName,
                AddedDate = General.FormatDate(m.AddedDate, ""),
                Description = m.Description

            }).First();
            Treatment.TreatmentGroupList = new SelectList(subMasterBL.GetTreatmentGroupList(), "ID", "Name");
            return View(Treatment);
        }

        public JsonResult GetTreatmentAutoComplete(string Hint)
        {
            DatatableResultBO resultBO = treatmentBL.GetTreatmentAutoComplete(Hint);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
    }
}