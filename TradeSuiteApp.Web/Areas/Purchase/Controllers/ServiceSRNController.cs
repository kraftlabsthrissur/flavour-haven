using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using TradeSuiteApp.Web.Utils;
using BusinessObject;
using BusinessLayer;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class ServiceSRNController : Controller
    {
        private IServiceRecieptNote srnBL;
        private IDropdownContract _dropdown;
        private ISupplierContract supplierBL;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;

        public ServiceSRNController(IDropdownContract IDRP)
        {
            _dropdown = IDRP;
            srnBL = new ServiceRecieptNoteBL();
            departmentBL = new DepartmentBL();
            employeeBL = new EmployeeBL();
            supplierBL = new SupplierBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
        }
        // GET: Purchase/ServiceSRN
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "processed", "cancelled" };
            return View();
        }

        // GET: Purchase/ServiceSRN/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            try
            {
                SRNViewModel model = new SRNViewModel();
                var obj = srnBL.GetAllSRN(id).FirstOrDefault();
                if (id != null)
                {
                    model.ID = obj.ID;
                    model.SRNNumber = obj.Code;
                    model.Date = General.FormatDate(obj.Date, "view");
                    model.ReceiptDate = General.FormatDate(obj.ReceiptDate);
                    model.SupplierID = obj.SupplierID;
                    model.SupplierName = obj.SupplierName;
                    model.DeliveryChallanNo = obj.DeliveryChallanNo;
                    model.DeliveryChallanDate = obj.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)obj.DeliveryChallanDate, "view");
                    model.IsDraft = obj.IsDraft;
                }
                model.Trans = new List<SRNtransViewModel>();
                var _transObj = srnBL.GetAllSRNTrans(id);
                if (_transObj != null)
                {
                    model.Trans = _transObj.Select(SRNTrans => new SRNtransViewModel
                    {
                        SRNID = SRNTrans.SRNID,
                        ItemID = SRNTrans.ItemID,
                        POServiceID = SRNTrans.POServiceID,
                        ItemName = SRNTrans.ItemName,
                        PurchaseOrderNo = SRNTrans.PurchaseOrderNo,
                        Unit = SRNTrans.Unit,
                        PurchaseOrderQty = SRNTrans.PurchaseOrderQty,
                        AcceptedQty = SRNTrans.AcceptedQty,
                        ReceivedQty = SRNTrans.ReceivedQty,
                        ServiceLocationID = (int)SRNTrans.ServiceLocationID,
                        DepartmentID = (int)SRNTrans.DepartmentID,
                        EmployeeID = (int)SRNTrans.EmployeeID,
                        CompanyID = (int)SRNTrans.CompanyID,
                        ProjectID = (int)SRNTrans.ProjectID,
                        Remarks = SRNTrans.Remarks,
                        ServiceLocation = SRNTrans.ServiceLocation,
                        Department = SRNTrans.Department,
                        Company = SRNTrans.Company,
                        Employee = SRNTrans.Employee,
                        Project = SRNTrans.Project,
                        TransportMode = SRNTrans.TransportMode,
                        TravelDateString = SRNTrans.TravelDate == null ? "" : General.FormatDate((DateTime)SRNTrans.TravelDate, "view"),
                        TravelFrom = SRNTrans.TravelFrom,
                        TravelTo = SRNTrans.TravelTo,
                        TravelingRemarks = SRNTrans.TravelingRemarks,
                        CategoryID = SRNTrans.CategoryID,
                        AcceptedValue=SRNTrans.PORate*SRNTrans.AcceptedQty,
                        PORate=SRNTrans.PORate
                    }).ToList();
                }

                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                ViewBag.StackTrace = e.StackTrace;
                return View("Error");
            }
        }

        // GET: Purchase/ServiceSRN/Create
        public ActionResult Create()
        {
            SRNViewModel model = new SRNViewModel();

            model.SRNNumber = generalBL.GetSerialNo("ServiceReceiptNote", "Code");

            model.Trans = new List<SRNtransViewModel>();
            model.Date = General.FormatDate(DateTime.Now);
            return View(model);
        }

        public JsonResult GetDropdownVal()

        {
            SRNViewModel model = new SRNViewModel();
            model.DDLLocation = new SelectList(locationBL.GetLocationList(), "ID", "Place");
            model.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            model.DDLEmployee = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            model.DDLInterCompany = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            model.DDLProject = new SelectList(_dropdown.GetProjectList(), "ID", "Name");

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void InitializeDropDowns()
        {
            ViewBag.Locations = new SelectList(locationBL.GetLocationList(), "ID", "Place");
            ViewBag.Departments = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            ViewBag.Employees = new SelectList(employeeBL.GetEmployeeList(), "ID", "Name");
            ViewBag.Companies = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            ViewBag.Projects = new SelectList(_dropdown.GetProjectList(), "ID", "Name");

        }

        public PartialViewResult GetPoTransByPrId(int[] PoIdList)
        {
            SRNViewModel srnModel = new SRNViewModel();
            srnModel.Trans = new List<SRNtransViewModel>();
            List<SRNTransBO> _outItems = new List<SRNTransBO>();
            List<SRNTransBO> obj = new List<SRNTransBO>();
            //  var obj;
            if (PoIdList != null)
            {
                foreach (var itm in PoIdList)
                {
                    obj = srnBL.GetUnProcessedPurchaseOrderServiceTransForSRN(Convert.ToInt32(itm));
                    obj = obj.Select(a =>
                    {
                        a.TravelDateString = a.TravelDate == null ? "" : General.FormatDate((DateTime)a.TravelDate);
                        return a;
                    }).ToList();
                    _outItems.AddRange(obj);
                }
            }
            InitializeDropDowns();

            srnModel.Trans = _outItems.Select(SRNTrans => new SRNtransViewModel
            {
                SRNID = SRNTrans.SRNID,
                ItemID = SRNTrans.ItemID,
                POServiceID = SRNTrans.POServiceID,
                ItemName = SRNTrans.ItemName,
                PurchaseOrderNo = SRNTrans.PurchaseOrderNo,
                Unit = SRNTrans.Unit,
                PurchaseOrderQty = SRNTrans.PurchaseOrderQty,
                AcceptedQty = SRNTrans.AcceptedQty,
                ReceivedQty = SRNTrans.ReceivedQty,
                ServiceLocationID = (int)SRNTrans.ServiceLocationID,
                DepartmentID = (int)SRNTrans.DepartmentID,
                EmployeeID = (int)SRNTrans.EmployeeID,
                CompanyID = (int)SRNTrans.CompanyID,
                ProjectID = (int)SRNTrans.ProjectID,
                Remarks = SRNTrans.Remarks,
                ServiceLocation = SRNTrans.ServiceLocation,
                Department = SRNTrans.Department,
                Company = SRNTrans.Company,
                Employee = SRNTrans.Employee,
                Project = SRNTrans.Project,
                TransportMode = SRNTrans.TransportMode,
                //code below by prama
                TravelDateString = SRNTrans.TravelDate == null ? "" : General.FormatDate((DateTime)SRNTrans.TravelDate, "view"),
                TravelFrom = SRNTrans.TravelFrom,
                TravelTo = SRNTrans.TravelTo,
                TravelingRemarks = SRNTrans.TravelingRemarks,
                CategoryID = SRNTrans.CategoryID,
                POServiceTransID = SRNTrans.POServiceTransID,
                PORate=SRNTrans.PORate
            }).ToList();



            return PartialView("~/Areas/Purchase/Views/ServiceSRN/_SRNItems.cshtml", srnModel.Trans);
        }

        [HttpPost]
        public JsonResult GetUnprocessedPoService(int SupplierId)
        {
            try
            {
                List<PurchaseOrderViewModel> _outItems = new List<PurchaseOrderViewModel>();

                _outItems = srnBL.GetUnProcessedPurchaseOrderServiceForSRN(Convert.ToInt32(SupplierId))
               .Select(PO => new PurchaseOrderViewModel
               {
                   PurchaseOrderNo = PO.PurchaseOrderNo,
                   ID = PO.ID,
                   PurchaseOrderDate = General.FormatDate(PO.PurchaseOrderDate),
                   SupplierName = PO.SupplierName,
                   RequestedBy = PO.RequestedBy,
                   NetAmt = PO.NetAmt
               }).ToList();
                return Json(new { Status = "success", data = _outItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        // POST: Purchase/ServiceSRN/Create
        [HttpPost]
        public ActionResult save(SRNViewModel _master, List<SRNtransViewModel> _trans)
        {
            var result = new List<object>();
            try
            {
                if (_master.ID != 0)
                {
                    //Edit
                    //Check whether editable or not
                    ServiceRecieptNoteBO Temp = srnBL.GetAllSRN(_master.ID).FirstOrDefault();
                    if (!Temp.IsDraft || Temp.IsCancelled)
                    {
                        result.Add(new { ErrorMessage = "Not Editable" });
                        return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                    }
                }
                ServiceRecieptNoteBO model = new ServiceRecieptNoteBO();
                if (_master != null)
                {
                    model.Code = _master.SRNNumber;
                    model.Date = General.ToDateTime(_master.Date);

                    model.ReceiptDate = General.ToDateTime(_master.Date);
                    model.SupplierID = _master.SupplierID;
                    model.DeliveryChallanNo = _master.DeliveryChallanNo;
                    model.IsDraft = _master.IsDraft;
                    model.ID = _master.ID;

                }
                if (_master.DeliveryChallanDate != null)
                {
                    model.DeliveryChallanDate = General.ToDateTime(_master.DeliveryChallanDate);
                }
                List<SRNTransBO> Trans = new List<SRNTransBO>();
                if (_trans != null)
                {
                    Trans = _trans.Select(item => new SRNTransBO
                    {
                        POServiceID = item.POServiceID,
                        POServiceTransID = item.POServiceTransID,
                        ItemID = item.ItemID,
                        PurchaseOrderQty = item.PurchaseOrderQty,
                        AcceptedQty = item.AcceptedQty,
                        ReceivedQty = item.ReceivedQty,

                        ServiceLocationID = item.ServiceLocationID,
                        DepartmentID = item.DepartmentID,
                        EmployeeID = item.EmployeeID,
                        CompanyID = item.CompanyID,
                        ProjectID = item.ProjectID,
                        Remarks = item.Remarks,
                        TolaranceQty = item.TolaranceQty

                    }).ToList();
                }
                // TODO: Add insert logic here


                bool response = srnBL.SaveSRN(model, Trans);
                if (response)
                {
                    return Json(new { Status = "success", data = "" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (DuplicateEntryException e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Purchase", "ServiceSRN", "Save", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Add(new { ErrorMessage = ex.Message });
                generalBL.LogError("Purchase", "ServiceSRN", "Save", 0, ex);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }

            return View();
        }

        // GET: Purchase/ServiceSRN/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }

            SRNViewModel model = new SRNViewModel();
            var obj = srnBL.GetAllSRN(id).FirstOrDefault();
            if(!obj.IsDraft || obj.IsCancelled)
            {
                return RedirectToAction("Index");
            }
            if (id != null)
            {
                model.ID = obj.ID;
                model.SRNNumber = obj.Code;
                model.Date = General.FormatDate(DateTime.Now);
                model.ReceiptDate = General.FormatDate(obj.ReceiptDate);
                model.SupplierID = obj.SupplierID;
                model.SupplierName = obj.SupplierName;
                model.DeliveryChallanNo = obj.DeliveryChallanNo;
                model.DeliveryChallanDate = obj.DeliveryChallanDate == null ? "" : General.FormatDate((DateTime)obj.DeliveryChallanDate);
                model.IsDraft = obj.IsDraft;
                model.ServicePODate = General.FormatDate(obj.ServicePODate);
            }
            model.Trans = new List<SRNtransViewModel>();
            var _transObj = srnBL.GetAllSRNTrans(id);
            if (_transObj != null)
            {
                model.Trans = _transObj.Select(SRNTrans => new SRNtransViewModel
                {
                    ItemID = SRNTrans.ItemID,
                    POServiceTransID = SRNTrans.POServiceTransID,
                    POServiceID = SRNTrans.POServiceID,
                    QtyTolerancePercent = SRNTrans.QtyTolerancePercent,

                    ItemName = SRNTrans.ItemName,
                    PurchaseOrderNo = SRNTrans.PurchaseOrderNo,
                    Unit = SRNTrans.Unit,
                    PurchaseOrderQty = SRNTrans.PurchaseOrderQty,
                    AcceptedQty = SRNTrans.AcceptedQty,
                    ReceivedQty = SRNTrans.ReceivedQty,
                    ServiceLocationID = (int)SRNTrans.ServiceLocationID,
                    DepartmentID = (int)SRNTrans.DepartmentID,
                    EmployeeID = (int)SRNTrans.EmployeeID,
                    CompanyID = (int)SRNTrans.CompanyID,
                    ProjectID = (int)SRNTrans.ProjectID,
                    Remarks = SRNTrans.Remarks,
                    ServiceLocation = SRNTrans.ServiceLocation,
                    Department = SRNTrans.Department,
                    Company = SRNTrans.Company,
                    Employee = SRNTrans.Employee,
                    Project = SRNTrans.Project,
                    TransportMode = SRNTrans.TransportMode,
                    //code below by prama
                    TravelDateString = SRNTrans.TravelDate == null ? "" : General.FormatDate((DateTime)SRNTrans.TravelDate, "view"),
                    TravelFrom = SRNTrans.TravelFrom,
                    TravelTo = SRNTrans.TravelTo,
                    TravelingRemarks = SRNTrans.TravelingRemarks,
                    CategoryID = SRNTrans.CategoryID,
                    AcceptedValue=SRNTrans.PORate*SRNTrans.AcceptedQty,
                    PORate=SRNTrans.PORate

                }).ToList();
            }
            InitializeDropDowns();

            model.UnProcessedPOService = srnBL.GetUnProcessedPurchaseOrderServiceForSRN(model.SupplierID).Select(PO => new PurchaseOrderViewModel
            {
                PurchaseOrderNo = PO.PurchaseOrderNo,
                ID = PO.ID,
                PurchaseOrderDate = General.FormatDate(PO.PurchaseOrderDate),
                SupplierName = PO.SupplierName,
                RequestedBy = PO.RequestedBy,
                NetAmt = PO.NetAmt
            }).ToList();
            return View(model);
        }

        public ActionResult Cancel(int ID)
        {
            srnBL.Cancel(ID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceNumberCount(string Hint, string Table, int SupplierID)
        {
            var count = srnBL.GetInvoiceNumberCount(Hint, Table, SupplierID);
            return Json(new { Status = "success", data = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSRNList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string SupplierNameHint = Datatable.Columns[3].Search.Value;
                string DeliveryChallanNoHint = Datatable.Columns[4].Search.Value;
                string DeliveryChallanDateHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = srnBL.GetSRNList(Type, TransNoHint, TransDateHint, SupplierNameHint, DeliveryChallanNoHint, DeliveryChallanDateHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveAsDraft(SRNViewModel _master, List<SRNtransViewModel> _trans)
        {
            return save(_master, _trans);
        }
    }
}
