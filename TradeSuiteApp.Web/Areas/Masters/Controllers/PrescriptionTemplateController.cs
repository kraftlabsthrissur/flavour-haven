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

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class PrescriptionTemplateController : Controller
    {
        private ISubmasterContract subMasterBL;
        private IPrescriptionFormatContract prescriptionFormatBL;

        public PrescriptionTemplateController()
        {
            subMasterBL = new SubmasterBL();
            prescriptionFormatBL = new PrescriptionFormatBL();
        }
        // GET: Masters/PrescriptionFormat
        public ActionResult Index()
        {
            List<PrescriptionFormatModel> PrescriptionList = new List<PrescriptionFormatModel>();
            PrescriptionList = prescriptionFormatBL.GetPrescriptionFormatList().Select(a => new PrescriptionFormatModel
            {
                MedicineCategoryID = a.MedicineCategoryID,
                MedicineCategory = a.MedicineCategory

            }).ToList();
            return View(PrescriptionList);
        }
        public ActionResult Details(int ID)
        {
            List<PrescriptionFormatModel> PrescriptionDetail = new List<PrescriptionFormatModel>();
            PrescriptionFormatModel model = prescriptionFormatBL.GetPrescriptionFormatDetails(ID).Select(a => new PrescriptionFormatModel()
            {
                MedicineCategoryID = a.MedicineCategoryID,
                MedicineCategory = a.MedicineCategory
            }).FirstOrDefault();
            model.Items = prescriptionFormatBL.GetPrescriptionFormatDetailTrans(ID).Select(a => new PrescriptionFormatItemModel
            {
                MedicineCategoryID = a.MedicineCategoryID,
                MedicineCategory = a.MedicineCategory,
                Prescription = a.Prescription
            }).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            PrescriptionFormatModel prescriptionFormatmodel = new PrescriptionFormatModel();
            prescriptionFormatmodel.MedicineCategoryGroupList = new SelectList(subMasterBL.GetMedicineCategoryGroupList(), "ID", "Name");
            prescriptionFormatmodel.Items = new List<PrescriptionFormatItemModel>();
            return View(prescriptionFormatmodel);
        }
        public ActionResult Save(PrescriptionFormatModel model)
        {
            try
            {
                PrescriptionFormatBO PrescriptionBO = new PrescriptionFormatBO()
                {

                    MedicineCategoryID = model.MedicineCategoryID,

                };
                List<PrescriptionFormatItemBO> Items = new List<PrescriptionFormatItemBO>();
                if (model.Items != null)
                {

                    PrescriptionFormatItemBO PrescriptionItem;

                    foreach (var item in model.Items)
                    {
                        PrescriptionItem = new PrescriptionFormatItemBO()
                        {
                            MedicineCategoryID = item.MedicineCategoryID,
                            Prescription = item.Prescription,
                        };
                        Items.Add(PrescriptionItem);
                    }
                }
                prescriptionFormatBL.Save(Items, PrescriptionBO);

                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPrescriptionFormatItemList(int ID)
        {
            try
            {
                List<PrescriptionFormatItemBO> Items = prescriptionFormatBL.GetPrescriptionFormatItemList(ID);

                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetPrescription(int CategoryID)
        {
            List<PrescriptionFormatBO> Categories = prescriptionFormatBL.GetPrescription(CategoryID).ToList();
            return Json(new { Status = "success", data = Categories }, JsonRequestBehavior.AllowGet);
        }
    }
}