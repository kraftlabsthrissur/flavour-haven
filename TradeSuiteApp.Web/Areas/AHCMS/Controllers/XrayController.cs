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
    public class XrayController : Controller
    {
        private IXrayContract xrayBL;
        private ISubmasterContract submasterBL;
        private IGeneralContract generalBL;
        private IDirectSalesInvoiceContract directSalesInvoiceBL;
        private ITreasuryContract treasuryBL;
        private IPaymentTypeContract paymentTypeBL;
        public XrayController()
        {
            xrayBL = new XrayBL();
            submasterBL = new SubmasterBL();
            generalBL = new GeneralBL();
            directSalesInvoiceBL = new DirectSalesInvoiceBL();
            treasuryBL = new TreasuryBL();
            paymentTypeBL = new PaymentTypeBL();
        }
        // GET: AHCMS/LaboratoryInvoice
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int ID, int PatientLabTestMasterID, int IPID, int PatientID)
        {
            LabTestModel model = xrayBL.GetPatientDetails(ID, PatientID).Select(a => new LabTestModel()
            {
                ID = ID,
                PatientID = a.PatientID,
                Patient = a.Patient,
                PatientCode = a.PatientCode,
                Age = a.Age,
                Sex = a.Sex,
                Mobile = a.Mobile,
                Doctor = a.Doctor,
                PatientLabTestID = a.PatientLabTestID,
                IPID = a.IPID,
                OPID = a.OPID,
                PatientTypeID = a.PatientTypeID,
                PaymentModeID = a.PaymentModeID,
                BankID = a.BankID
            }).First();
            model.Items = xrayBL.GetXrayItems(ID, PatientLabTestMasterID, IPID).Select(a => new LabTestItemModel()
            {
                ItemID = a.ItemID,
                ItemName = a.ItemName,
                BiologicalReference = a.BiologicalReference,
                ID = a.ID,
                Unit = a.Unit,
                Status = a.Status,
                ObserveValue = a.ObserveValue,
                Price = a.Price,
                LabtestType = a.Type,
                IsBillGenerated = a.IsBillGenerated,
                SalesInvoiceID = a.SalesInvoiceID,
                IssueDate = General.FormatDate(DateTime.Now),
                StatusList = new SelectList(new List<SelectListItem>{
                                                 new SelectListItem { Text = "Completed", Value = "Completed"}
                                                 }, "Value", "Text"),
            }).ToList();
            model.IssueDate = General.FormatDate(DateTime.Now);
            model.AppointmentProcessID = ID;
            //model.LabTestType = new SelectList(new List<SelectListItem>
            //                                    {
            //                                    new SelectListItem { Text = "Internal", Value = "Internal"},
            //                                     new SelectListItem { Text = "External", Value = "External"},
            //                                     }, "Value", "Text");
            model.Date = General.FormatDate(DateTime.Now);
            model.SupplierList = new SelectList(submasterBL.GetSupplierForLabItems(), "ID", "Name");
            model.PatientTypeList = new SelectList(directSalesInvoiceBL.GetPatientTypeList(), "ID", "Name");
            model.SalesTypeList = new SelectList(directSalesInvoiceBL.GetSalesType(), "ID", "SalesType");
            model.BankList = new SelectList(treasuryBL.GetBankForCounterSales("Cash"), "ID", "BankName");
            model.PaymentModeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", model.PaymentModeID);
            return View(model);
        }      
        [HttpPost]
        public ActionResult Save(LabTestModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                LabTestBO labtestBO = new LabTestBO()
                {
                    PatientID = model.PatientID,
                    OPID = model.AppointmentProcessID,
                    IPID = model.IPID,
                    PatientLabTestID = model.PatientLabTestID,
                    NetAmount = model.NetAmount,
                    SalesTypeID = model.SalesTypeID,
                    SalesType = model.SalesType,
                    BankID = model.BankID,
                    PaymentModeID = model.PaymentModeID,
                    Date = General.ToDateTime(model.Date)
                };
                List<LabTestItemBO> Items = new List<LabTestItemBO>();
                if (model.Items != null)
                {
                    LabTestItemBO labTestItem;

                    foreach (var item in model.Items)
                    {
                        labTestItem = new LabTestItemBO()
                        {
                            IssueDate = General.ToDateTime(item.IssueDate),
                            ItemID = item.ItemID,
                            Price = item.Price,
                            Type = item.LabtestType,
                            ID = item.ID,
                            SupplierID = item.SupplierID

                        };
                        Items.Add(labTestItem);
                    }
                }
                int SalesInvoiceID = xrayBL.Save(labtestBO, Items);
                return Json(new { Status = "success", SalesInvoiceID = SalesInvoiceID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("AHCMS", "LaboratoryInvoice", "Save", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetXrayList(DatatableModel Datatable)
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

                DatatableResultBO resultBO = xrayBL.GetXrayList(Date, PatientCode, Patient, LabTest, Doctor, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LaboratoryTestPrintPdf(int ID, int SalesInvoiceID)
        {
            return null;
        }
        public ActionResult CreateInvoice()
        {
            LabTestModel model = new LabTestModel();
            model.Date = General.FormatDate(DateTime.Now);
            model.IssueDate = General.FormatDate(DateTime.Now);
            model.SupplierList = new SelectList(submasterBL.GetSupplierForLabItems(), "ID", "Name");
            model.PatientTypeList = new SelectList(directSalesInvoiceBL.GetPatientTypeList(), "ID", "Name");
            model.SalesTypeList = new SelectList(directSalesInvoiceBL.GetSalesType(), "ID", "SalesType");
            model.BankList = new SelectList(treasuryBL.GetBankForCounterSales("Cash"), "ID", "BankName");
            model.PaymentModeList = new SelectList(paymentTypeBL.GetPaymentTypeList(), "ID", "Name", model.PaymentModeID);

            return View(model);
        }
        public JsonResult GetLabTestDetails(int ItemID, int PatientID)
        {
            LabTestItemBO item = xrayBL.GetLabTestItemsDetails(ItemID, PatientID);
            return Json(new { Status = "success", data = item }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult XrayInvoicePrintPDF(int ID, int SalesInvoiceID)
        {
            return null;
        }
    }
}