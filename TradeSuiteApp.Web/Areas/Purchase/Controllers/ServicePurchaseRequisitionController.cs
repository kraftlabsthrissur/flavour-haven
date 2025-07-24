using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Purchase.Models;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using TradeSuiteApp.Web.Utils;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Purchase.Controllers
{
    public class ServicePurchaseRequisitionController : Controller
    {

        private IServicePurchaseRequisition servicePurchaseRequisitionBL;
        private IDropdownContract _dropdown;
        private IDepartmentContract departmentBL;
        private IEmployeeContract employeeBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;
        private IPlacesContract placesBL;
        private ICategoryContract categoryBL;

        public ServicePurchaseRequisitionController(IDropdownContract tempDropdown)
        {
            servicePurchaseRequisitionBL = new ServicePurchaseRequisitionBL();
            this._dropdown = tempDropdown;
            departmentBL = new DepartmentBL();
            employeeBL = new EmployeeBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
            placesBL = new PlacesBL();
            categoryBL = new CategoryBL();
        }

        // GET: Purchase/ServicePurchaseRequisition
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "draft", "processed" };
            return View();
        }

        // GET: Purchase/ServicePurchaseRequisition/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ServicePRViewModel _outServicePR = new ServicePRViewModel();
            var obj = servicePurchaseRequisitionBL.GetServicePurchaseRequisition((int)id);
            if (id != null)
            {
                _outServicePR.ID = obj.ID;
                _outServicePR.PrDate = General.FormatDate((DateTime)obj.Date, "view");
                _outServicePR.PurchaseRequisitionNumber = obj.RequisitionNo;

                _outServicePR.Status = obj.Cancelled ? "cancelled" : obj.FullyOrdered ? "processed" : "";
                _outServicePR.IsDraft = obj.IsDraft;
                _outServicePR.FromDeptID = obj.FromDeptID;
                _outServicePR.PrTrans = new List<ServicePRTrans>();
                _outServicePR.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
                var _transObj = servicePurchaseRequisitionBL.PurchaseRequisitionTransDetailsForService((int)id);
                if (_transObj != null)
                {
                    _outServicePR.PrTrans = _transObj.Select(itm => new ServicePRTrans
                    {
                        ID = itm.ItemID,
                        ReqQuantity = itm.Quantity,

                        ExpDate = itm.ExpectedDate == null ? "" : General.FormatDate((DateTime)itm.ExpectedDate, "view"),
                        DepartmentID = itm.RequiredDepartmentID,
                        LocationID = itm.RequiredLocationID,
                        EmployeeID = itm.RequiredEmployeeID,
                        InterCompanyID = itm.RequiredInterCompanyID,
                        ProjectID = itm.RequiredProjectID,
                        Remark = itm.Remarks,

                        ItemName = itm.Item,
                        Location = itm.Location,
                        Department = itm.Department,
                        Employee = itm.Employee,
                        InterCompany = itm.InterCompany,
                        Project = itm.Project,
                        Unit = itm.Unit,
                        TransportMode = itm.TransportMode,
                        TravelDate = itm.TravelDate == null ? "" : General.FormatDate((DateTime)itm.TravelDate, "view"),
                        TravelFrom = itm.TravelFrom,
                        TravelTo = itm.TravelTo,
                        TravelingRemarks = itm.TravelingRemarks,
                        TravelCategoryID = itm.TravelCategoryID
                    }).ToList();
                }

            }

            return View(_outServicePR);

        }

        // GET: Purchase/ServicePurchaseRequisition/Create
        public ActionResult Create()
        {
            ServicePRViewModel model = new ServicePRViewModel();

            model.PurchaseRequisitionNumber = generalBL.GetSerialNo("PurchaseRequisitionForService", "code");
            model.PrDate = DateTime.Now.ToString("dd-MM-yyyy");
            model.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);
            if (model.IsBranchLocation)
            {
                model.LocationID = GeneralBO.LocationID;
                model.DepartmentID = Convert.ToInt32(generalBL.GetConfig("DefaultDepartmentForBranch"));
            }
            model.DDLItemCategory = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
            model.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(-1), "ID", "Name");
            model.DDLLocation = new SelectList(locationBL.GetLocationList(), "ID", "Place");
            model.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
            model.DDLEmployee = new SelectList(new List<SelectListItem>
                                          {},"Value", "Text");
            model.DDLInterCompany = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
            model.DDLProject = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
            model.TransportModeList = new SelectList(generalBL.GetModeOfTransport(), "ID", "Name");
            //model.TransportModeList = new SelectList(
            //                                 new List<SelectListItem>
            //                                 {
            //                                    new SelectListItem { Text = "Air", Value = "1"},
            //                                    new SelectListItem { Text = "Train", Value = "2"},
            //                                     new SelectListItem { Text = "Bus", Value = "3"},
            //                                 }, "Value", "Text");
            model.TravelFromList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
            model.TravelToList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
            model.PrTrans = new List<ServicePRTrans>();
            return View(model);
        }

        public JsonResult getItemForAutoComplete(string Areas, string term = "", string ItemCategoryID = "", string PurchaseCategoryID = "")
        {
            List<ItemBO> _outItems = new List<ItemBO>();
            ItemContract itemBL = new ItemBL();
            var ItemCategoryIDInt = (ItemCategoryID != "") ? Convert.ToInt32(ItemCategoryID) : 0;
            var PurchaseCategoryIDInt = (PurchaseCategoryID != "") ? Convert.ToInt32(PurchaseCategoryID) : 0;

            _outItems = itemBL.GetServiceItems(term, ItemCategoryIDInt, PurchaseCategoryIDInt);
            return Json(_outItems, JsonRequestBehavior.AllowGet);
        }

        // POST: Purchase/ServicePurchaseRequisition/Create
        [HttpPost]
        public ActionResult Create(ServicePRViewModel _master)
        {
            var result = new List<object>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (_master.ID != 0)
                    {
                        //Edit
                        //Check whether editable or not
                        RequisitionBO Temp = servicePurchaseRequisitionBL.GetServicePurchaseRequisition(_master.ID);
                        if (!Temp.IsDraft || Temp.Cancelled )
                        {
                            result.Add(new { ErrorMessage = "Not Editable" });
                            return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    RequisitionBO obj = new RequisitionBO();
                    obj.ID = _master.ID;
                    obj.RequisitionNo = _master.PurchaseRequisitionNumber;
                    obj.Date = General.ToDateTime(_master.PrDate);
                    obj.Cancelled = false;
                    obj.IsDraft = _master.IsDraft;
                    obj.FromDeptID = _master.FromDeptID;
                    List<RequisitionServiceItemBO> TransList = new List<RequisitionServiceItemBO>();

                    foreach (var itm in _master.PrTrans)
                    {
                        var tranObj = new RequisitionServiceItemBO();
                        if (itm.ExpDate != null && itm.ExpDate != "")
                        {
                            tranObj.ExpectedDate = General.ToDateTime(itm.ExpDate);
                        }
                        tranObj.ItemID = itm.ID;
                        tranObj.Quantity = itm.ReqQuantity;
                        tranObj.Remarks = itm.Remark;
                        tranObj.TravelFromID = itm.TravelFromID;
                        tranObj.TravelToID = itm.TravelToID;
                        tranObj.TransportModeID = itm.TransportModeID;
                        if (itm.TravelDate != null && itm.TravelDate != "")
                        {
                            tranObj.TravelDate = General.ToDateTime(itm.TravelDate);
                        }
                        else
                        {
                            tranObj.TravelDate = null;

                        }
                        tranObj.TravelingRemarks = itm.TravelingRemarks;
                        tranObj.RequiredDepartmentID = itm.DepartmentID == null ? 0 : (int)itm.DepartmentID;
                        tranObj.RequiredEmployeeID = itm.EmployeeID == null ? 0 : (int)itm.EmployeeID;
                        tranObj.RequiredInterCompanyID = itm.InterCompanyID == null ? 0 : (int)itm.InterCompanyID;
                        tranObj.RequiredLocationID = itm.LocationID == null ? 0 : (int)itm.LocationID;
                        tranObj.RequiredProjectID = itm.ProjectID == null ? 0 : (int)itm.ProjectID;
                        TransList.Add(tranObj);
                    }

                    // TODO: Add insert logic here
                    bool response = servicePurchaseRequisitionBL.SavePurchaseRequisitionForService(obj, TransList);
                    if (response)
                    {
                        return Json("success", JsonRequestBehavior.AllowGet);

                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    result.Add(new { ErrorMessage = ex.Message });
                    generalBL.LogError("Purchase", "ServicePurchaseRequisition", "Save", _master.ID, ex);
                    return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var Errors = ModelState.Values.SelectMany(func => func.Errors);
                return Json(new { Status = "failure", data = Errors }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAsDraft(ServicePRViewModel _master)
        {
            return Create(_master);
        }

        // GET: Purchase/ServicePurchaseRequisition/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("PageNotFound");
            }
            ServicePRViewModel _outServicePR = new ServicePRViewModel();
            var obj = servicePurchaseRequisitionBL.GetServicePurchaseRequisition((int)id);
            if(!obj.IsDraft || obj.Cancelled)
            {
                return RedirectToAction("Index");
            }
            if (id != null)
            {
                _outServicePR.ID = obj.ID;
                _outServicePR.PrDate = Convert.ToDateTime(obj.Date).ToString("dd-MM-yyyy");
                _outServicePR.PurchaseRequisitionNumber = obj.RequisitionNo;
                _outServicePR.DDLItemCategory = new SelectList(_dropdown.GetItemCategoryForService(), "ID", "Name");
                _outServicePR.IsDraft = obj.IsDraft;
                _outServicePR.FromDeptID = obj.FromDeptID;
                _outServicePR.PrTrans = new List<ServicePRTrans>();
                _outServicePR.DDLPurchaseCategory = new SelectList(categoryBL.GetPurchaseCategoryList(-1), "ID", "Name");
                _outServicePR.TravelFromList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
                _outServicePR.TravelToList = new SelectList(placesBL.GetPlaces(0), "ID", "Name");
                _outServicePR.TransportModeList = new SelectList(generalBL.GetModeOfTransport(), "ID", "Name");
                _outServicePR.IsBranchLocation = locationBL.IsBranchLocation(GeneralBO.LocationID);
                if (_outServicePR.IsBranchLocation)
                {
                    _outServicePR.LocationID = GeneralBO.LocationID;
                    _outServicePR.DepartmentID = Convert.ToInt32(generalBL.GetConfig("DefaultDepartmentForBranch"));
                }
                _outServicePR.DDLLocation = new SelectList(locationBL.GetLocationList(), "ID", "Place");
                _outServicePR.DDLDepartment = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");
                _outServicePR.DDLEmployee = new SelectList(new List<SelectListItem>
                { }, "Value", "Text");
                _outServicePR.DDLInterCompany = new SelectList(_dropdown.GetInterCompanyList(), "ID", "Name");
                _outServicePR.DDLProject = new SelectList(_dropdown.GetProjectList(), "ID", "Name");
                var _transObj = servicePurchaseRequisitionBL.PurchaseRequisitionTransDetailsForService((int)id);
                if (_transObj != null)
                {
                    _outServicePR.PrTrans = _transObj.Select(itm => new ServicePRTrans
                    {
                        ID = itm.ItemID,
                        ReqQuantity = itm.Quantity,
                        ExpDate = itm.ExpectedDate == null ? "" : General.FormatDate((DateTime)itm.ExpectedDate),
                        DepartmentID = itm.RequiredDepartmentID,
                        LocationID = itm.RequiredLocationID,
                        EmployeeID = itm.RequiredEmployeeID,
                        InterCompanyID = itm.RequiredInterCompanyID,
                        ProjectID = itm.RequiredProjectID,
                        Remark = itm.Remarks,
                        ItemName = itm.Item,
                        Location = itm.Location,
                        Department = itm.Department,
                        Employee = itm.Employee,
                        InterCompany = itm.InterCompany,
                        Project = itm.Project,
                        Unit = itm.Unit,
                        TransportMode = itm.TransportMode,
                        TravelDate = itm.TravelDate == null ? "" : General.FormatDate((DateTime)itm.TravelDate),
                        TravelFrom = itm.TravelFrom,
                        TravelFromID = itm.TravelFromID,
                        TravelToID = itm.TravelToID,
                        TransportModeID = itm.TransportModeID,
                        TravelTo = itm.TravelTo,
                        TravelingRemarks = itm.TravelingRemarks,
                        TravelCategoryID = itm.TravelCategoryID
                    }).ToList();
                }

            }

            return View(_outServicePR);

        }

        [HttpPost]
        public JsonResult GetPurchaseRequisitionList(DatatableModel Datatable)
        {
            try
            {
                string TransNoHint = Datatable.Columns[1].Search.Value;
                string TransDateHint = Datatable.Columns[2].Search.Value;
                string PurchaseOrderNumberHint = Datatable.Columns[3].Search.Value;
                string CategoryNameHint = Datatable.Columns[4].Search.Value;
                string ItemNameHint = Datatable.Columns[5].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = servicePurchaseRequisitionBL.GetPurchaseRequisitionList(Type, TransNoHint, TransDateHint, PurchaseOrderNumberHint, CategoryNameHint, ItemNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(int id)
        {
            return null;
        }
              
    }
}
