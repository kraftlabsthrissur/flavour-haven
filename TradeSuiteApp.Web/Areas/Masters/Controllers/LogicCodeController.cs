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
    public class LogicCodeController : Controller
    {

        private ILogicCodeContract LogicCodeBL;

        public LogicCodeController()
        {
            LogicCodeBL = new LogicCodeBL();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(LogicCodeModel model)
        {
            try
            {
                LogicCodeBO LogicCodeBO = new LogicCodeBO()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Remarks = model.Remarks,
                    Code=model.Code

                };
                LogicCodeBL.Save(LogicCodeBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLogicCodeList(DatatableModel Datatable)
        {
            try
            {
                DatatableResultBO resultBO = LogicCodeBL.GetLogicCodeList(Datatable.Columns[1].Search.Value, Datatable.Columns[2].Search.Value, Datatable.Columns[Datatable.Order[0].Column].Data, Datatable.Order[0].Dir, Datatable.Start, Datatable.Length);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                                return Json(new { Status = "failure", data = res }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(int ID)
        {
            var obj = LogicCodeBL.GetLogicCodeDetails((int)ID);
            LogicCodeModel model = new LogicCodeModel();
            model.ID = obj.ID;
            model.Code = obj.Code;
            model.Name = obj.Name;
            model.Remarks = obj.Remarks;
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var obj = LogicCodeBL.GetLogicCodeDetails((int)ID);
            LogicCodeModel model = new LogicCodeModel();
            model.ID = obj.ID;
            model.Code = obj.Code;
            model.Name = obj.Name;
            model.Remarks = obj.Remarks;
            return View(model);
        }
    }
}