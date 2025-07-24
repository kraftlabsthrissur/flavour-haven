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
    public class LaboratoryTestResultController : Controller
    {
        private ILaboratoryTestResultContract labTestResultBL;
        private IFileContract fileBL;

        public LaboratoryTestResultController()
        {
            labTestResultBL = new LaboratoryTestResultBL();
            fileBL = new FileBL();
        }
        // GET: AHCMS/LaboratoryTestResult
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int? SalesInvoiceID, int? OPID,int? PatientID)
        {
            LaboratoryTestResultModel model = new LaboratoryTestResultModel();
            model.OPID = (int)OPID;
            model.SalesInvoiceID = (int)SalesInvoiceID;
            model.PatientID = (int)PatientID;
            model.Items = labTestResultBL.GetInvoicedLabTestItems((int)SalesInvoiceID).Select(a => new LaboratoryTestResultItemsModel()
            {
                PatientLabTestsID = a.PatientLabTestsID,
                BillablesID = a.BillablesID,
                ItemID = a.ItemID,
                Item = a.Item,
                Unit = a.Unit,
                BiologicalReference = a.BiologicalReference,
                IsProcessed = a.IsProcessed,
                Status = a.Status,
                NormalHighLevel = a.NormalHighLevel,
                NormalLowLevel = a.NormalLowLevel,
                ObservedValue = a.ObservedValue,
                PatientLabTestTransID=a.PatientLabTestTransID,
                StatusList = new SelectList(new List<SelectListItem>{
                                                 //new SelectListItem { Text = "Started", Value ="Started", },
                                                 new SelectListItem { Text = "Completed", Value ="Completed", },
                                                 }, "Value", "Text"),
                SelectedQuotation = fileBL.GetAttachments(a.DocumentID.ToString()),
                DocumentID=a.DocumentID,
                CollectedTime=a.CollectedTime,
                ReportedTime = a.ReportedTime
            }).ToList();
            return View(model);
        }
        public JsonResult GetInvoicedLabTestList(DatatableModel Datatable)
        {
            try
            {
                string InvoiceNo = Datatable.Columns[1].Search.Value;
                string InvoiceDate = Datatable.Columns[2].Search.Value;
                string Patient = Datatable.Columns[3].Search.Value;
                string Doctor = Datatable.Columns[4].Search.Value;
                string NetAmt = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = labTestResultBL.GetInvoicedLabTestList(Type, InvoiceNo, InvoiceDate, Patient, Doctor, NetAmt, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult InvoicedLabTestItems(int SalesInvoiceID)
        {
            LaboratoryTestResultModel model = new LaboratoryTestResultModel();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Save(LaboratoryTestResultModel model)
        {
            try
            {
                List<LaboratoryTestResultBO> Items = new List<LaboratoryTestResultBO>();
                if (model.Items != null)
                {
                    LaboratoryTestResultBO labTestItem;

                    foreach (var item in model.Items)
                    {
                        labTestItem = new LaboratoryTestResultBO()
                        {
                            Status = item.Status,
                            ObservedValue = item.ObservedValue,
                            PatientLabTestsID = item.PatientLabTestsID,
                            PatientLabTestTransID=item.PatientLabTestTransID,
                            DocumentID =item.DocumentID,
                            CollectedTime=item.CollectedTime,
                            ReportedTime=item.ReportedTime
                        };
                        Items.Add(labTestItem);
                    }
                }
                labTestResultBL.Save(Items);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LabTestResultPrintPDF(int ID, int SalesInvoiceID)
        {
            return null;
        }
    }
}