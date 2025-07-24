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
    public class DiagnosisController : Controller
    {
        private IDiagnosisContract diagnosisBL;
        private IGeneralContract generalBL;

        public DiagnosisController()
        {
            diagnosisBL = new DiagnosisBL();
            generalBL = new GeneralBL();
        }

        // GET: Masters/Diagnosis
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(DiagnnosisModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DiagnosisBO diagnosisBO = new DiagnosisBO()
                    {
                        ID = model.ID,
                        Name = model.Name,
                        Description = model.Description
                    };
                    diagnosisBL.Save(diagnosisBO);
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    var res = new List<object>();
                    return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDiagnosisList(DatatableModel Datatable)
        {
            try
            {
                string NameHint = Datatable.Columns[1].Search.Value;
                string DescriptionHint = Datatable.Columns[2].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = diagnosisBL.GetDiagnosisList(NameHint, DescriptionHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Diagnosis", "GetDiagnosisList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            DiagnnosisModel model = diagnosisBL.GetDiagnosisDetails(ID).Select(a => new DiagnnosisModel()
            {
               ID = a.ID,
               Name = a.Name,
               Description = a.Description
            }).First();
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            DiagnnosisModel model = diagnosisBL.GetDiagnosisDetails(ID).Select(a => new DiagnnosisModel()
            {
                ID = a.ID,
                Name = a.Name,
                Description = a.Description,
            }).First();
            return View(model);
        }
    }
}