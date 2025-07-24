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
    public class TreatmentRoomController : Controller
    {
        private ITreatmentRoomContract treatmentRoomBL;
        private IGeneralContract generalBL;

        public TreatmentRoomController()
        {
            treatmentRoomBL = new TreatmentRoomBL();
            generalBL = new GeneralBL();
        }
        // GET: Masters/TreatmentRoom
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(TreatmentRoomModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TreatmentRoomBO treatmentRoomBO = new TreatmentRoomBO()
                    {
                        ID = model.ID,
                        Name = model.Name,
                        Remarks = model.Remarks
                    };
                    treatmentRoomBL.Save(treatmentRoomBO);
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

        public JsonResult GetTreatmentRoomList(DatatableModel Datatable)
        {
            try
            {
                string NameHint = Datatable.Columns[1].Search.Value;
                string RemarkHint = Datatable.Columns[2].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = treatmentRoomBL.GetTreatmentRoomList(NameHint, RemarkHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "TreatmentRoom", "GetTreatmentRoomList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            TreatmentRoomModel model = treatmentRoomBL.GetTreatmentRoomDetails(ID).Select(a => new TreatmentRoomModel()
            {
                ID = a.ID,
                Name = a.Name,
                Remarks = a.Remarks
            }).First();
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            TreatmentRoomModel model = treatmentRoomBL.GetTreatmentRoomDetails(ID).Select(a => new TreatmentRoomModel()
            {
                ID = a.ID,
                Name = a.Name,
                Remarks = a.Remarks
            }).First();
            return View(model);
        }
        public JsonResult GetTreatmentRoomAutoComplete(string Hint)
        {
            DatatableResultBO resultBO = treatmentRoomBL.GetTreatmentRoomAutoComplete(Hint);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
    }
}