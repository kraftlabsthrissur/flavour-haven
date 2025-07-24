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
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class FSOController : Controller
    {
        private ICategoryContract categroyBL;
        private IFSOContract fsoBL;
        private IStateContract stateBL;
        private ICustomerContract customerBL;
        public FSOController()
        {
            stateBL = new StateBL();
            categroyBL = new CategoryBL();
            fsoBL = new FSOBL();
            customerBL = new CustomerBL();
        }
        // GET: Masters/FSO
        public ActionResult Index()
        {
            List<FSOModel> FSO = fsoBL.GetFSOLst().Select(a => new FSOModel()
            {
                ID = a.ID,
                SalesManager = a.SalesManager,
                RegionalSalesManager = a.RegionalSalesManager,
                AreaManager = a.AreaManager,
                ZonalManager = a.ZonalManager,
                RouteCode = a.RouteCode,
                FSOName = a.FSOName,

            }).ToList();
            return View(FSO);
        }

        public ActionResult Create()
        {
            FSOModel fso = new FSOModel();
            fso.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(0), "ID", "Name");
            fso.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(0), "ID", "Name");
            fso.SalesIncentiveCategoryList = new SelectList(categroyBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
            fso.SalesManagerList = new SelectList(categroyBL.GetSalesManagerCategory(), "ID", "Name");
            fso.RegionalSalesManagerList = new SelectList(categroyBL.GetRegionalSalesManagerCateogry(), "ID", "Name");
            fso.ZonalManagerList = new SelectList(categroyBL.GetZonalManagerCategory(), "ID", "Name");
            fso.AreaManagerList = new SelectList(categroyBL.GetAreaManagerCategory(), "ID", "Name");
            fso.IsActiveStatus = "checked";
            fso.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            fso.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            return View(fso);
        }

        public ActionResult Save(FSOModel model)
        {
            try
            {
                FSOBO FSOBO = new FSOBO()
                {
                    ID = model.ID,
                    FSOCode = model.FSOCode,
                    IsAreaManager = model.IsAreaManager,
                    IsZonalManager = model.IsZonalManager,
                    IsRegionalSalesManager = model.IsRegionalSalesManager,
                    IsSalesManager = model.IsSalesManager,
                    BusinessCategoryID = model.BusinessCategoryID,
                    SalesIncentiveCategoryID = model.SalesIncentiveCategoryID,
                    SalesCategoryID = model.SalesCategoryID,
                    RouteCode = model.RouteCode,
                    RouteName = model.RouteName,
                    ZoneCode = model.ZoneCode,
                    ZoneName = model.ZoneName,
                    FSOName = model.FSOName,
                    AreaManager = model.AreaManager,
                    ZonalManager = model.ZonalManager,
                    FromDate = General.ToDateTime(model.FromDate),
                    EmployeeID = model.EmployeeID,
                    IsActive = model.IsActive,
                    ZonalManagerID = model.ZonalManagerID,
                    AreaManagerID = model.AreaManagerID,
                    SalesManagerID = model.SalesManagerID,
                    RegionalSalesManagerID = model.RegionalSalesManagerID,
                    ReportingToID = model.ReportingToID
                };
                if (model.ToDate != "" && model.ToDate != null)
                {
                    FSOBO.ToDate = General.ToDateTime(model.ToDate);
                }
                List<FSOIncentiveItemBO> FSOIncentiveItems = new List<FSOIncentiveItemBO>();
                if (model.Items != null)
                {
                    FSOIncentiveItemBO FSOIncentiveItem;
                    foreach (var item in model.Items)
                    {
                        FSOIncentiveItem = new FSOIncentiveItemBO()
                        {
                            CustomerID = item.CustomerID,
                            SalesIncentiveCategoryID = item.SalesIncentiveCategoryID,
                            StateID = item.StateID,
                            DistrictID = item.DistrictID,
                            StartDate = General.ToDateTime(item.StartDate)
                        };
                        FSOIncentiveItems.Add(FSOIncentiveItem);
                    }
                }
                fsoBL.Save(FSOBO, FSOIncentiveItems);
                return
                     Json(new
                     {
                         Status = "success",
                         data = "",
                         message = ""
                     }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return
                     Json(new
                     {
                         Status = "failure",
                         data = "",
                         message = e.Message
                     }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Edit(int ID)
        {
            FSOModel fso;
            FSOBO fsobo = fsoBL.GetFSODetails((int)ID);
            fso = new FSOModel()
            {
                ID = fsobo.ID,
                FSOCode = fsobo.FSOCode,
                FSOName = fsobo.FSOName,
                EmployeeID = fsobo.EmployeeID,
                SalesCategoryID = fsobo.SalesCategoryID,
                SalesCategoryName = fsobo.SalesCategoryName,
                SalesIncentiveCategoryID = fsobo.SalesIncentiveCategoryID,
                SalesIncentiveCategoryName = fsobo.SalesIncentiveCategoryName,
                BusinessCategoryID = fsobo.BusinessCategoryID,
                BusinessCategoryName = fsobo.BusinessCategoryName,
                RouteCode = fsobo.RouteCode,
                RouteName = fsobo.RouteName,
                ZoneCode = fsobo.ZoneCode,
                ZoneName = fsobo.ZoneName,
                ZonalManager = fsobo.ZonalManager,
                ZonalManagerID = fsobo.ZonalManagerID,
                AreaManager = fsobo.AreaManager,
                AreaManagerID = fsobo.AreaManagerID,
                FromDate = fsobo.FromDate == null ? "" : General.FormatDate((DateTime)fsobo.FromDate),
                ToDate = fsobo.ToDate == null ? "" : General.FormatDate((DateTime)fsobo.ToDate),
                AreaManagerStatus = fsobo.IsAreaManager ? "checked" : "",
                ZonalManagerStatus = fsobo.IsZonalManager ? "checked" : "",
                IsZonalManager = fsobo.IsZonalManager,
                IsAreaManager = fsobo.IsAreaManager,
                IsActive = fsobo.IsActive,
                IsActiveStatus = fsobo.IsActive ? "checked" : "",
                SalesManagerStatus = fsobo.IsSalesManager ? "checked" : "",
                RegionalSalesManagerStatus = fsobo.IsRegionalSalesManager ? "checked" : "",
                RegionalSalesManagerID = fsobo.RegionalSalesManagerID,
                SalesManagerID = fsobo.SalesManagerID,
                SalesManager = fsobo.SalesManager,
                RegionalSalesManager = fsobo.RegionalSalesManager,
                IsSalesManager = fsobo.IsSalesManager,
                IsRegionalSalesManager = fsobo.IsRegionalSalesManager,
                ReportingToID = fsobo.ReportingToID,
                ReportingToName = fsobo.ReportingToName

            };
            fso.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(0), "ID", "Name");
            fso.BusinessCategoryList = new SelectList(categroyBL.GetBusinessCategoryList(0), "ID", "Name");
            fso.SalesIncentiveCategoryList = new SelectList(categroyBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
            fso.SalesManagerList = new SelectList(categroyBL.GetSalesManagerCategory(), "ID", "Name");
            fso.RegionalSalesManagerList = new SelectList(categroyBL.GetRegionalSalesManagerCateogry(), "ID", "Name");
            fso.ZonalManagerList = new SelectList(categroyBL.GetZonalManagerCategory(), "ID", "Name");
            fso.AreaManagerList = new SelectList(categroyBL.GetAreaManagerCategory(), "ID", "Name");
            fso.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            fso.CustomerCategoryList = new SelectList(customerBL.GetCustomerCategories(), "ID", "Name");
            return View(fso);


        }

        public ActionResult Details(int ID)
        {

            FSOModel fso;
            FSOBO fsobo = fsoBL.GetFSODetails((int)ID);
            fso = new FSOModel()
            {

                ID = fsobo.ID,
                FSOCode = fsobo.FSOCode,
                FSOName = fsobo.FSOName,
                EmployeeID = fsobo.EmployeeID,
                SalesCategoryID = fsobo.SalesCategoryID,
                SalesCategoryName = fsobo.SalesCategoryName,
                SalesIncentiveCategoryID = fsobo.SalesIncentiveCategoryID,
                SalesIncentiveCategoryName = fsobo.SalesIncentiveCategoryName,
                BusinessCategoryID = fsobo.BusinessCategoryID,
                BusinessCategoryName = fsobo.BusinessCategoryName,
                RouteCode = fsobo.RouteCode,
                RouteName = fsobo.RouteName,
                ZoneCode = fsobo.ZoneCode,
                ZoneName = fsobo.ZoneName,
                ZonalManager = fsobo.ZonalManager,
                ZonalManagerID = fsobo.ZonalManagerID,
                AreaManager = fsobo.AreaManager,
                AreaManagerID = fsobo.AreaManagerID,
                FromDate = fsobo.FromDate == null ? "" : General.FormatDate((DateTime)fsobo.FromDate),
                ToDate = fsobo.ToDate == null ? "" : General.FormatDate((DateTime)fsobo.ToDate),
                AreaManagerStatus = fsobo.IsAreaManager ? "checked" : "",
                ZonalManagerStatus = fsobo.IsZonalManager ? "checked" : "",
                IsZonalManager = fsobo.IsZonalManager,
                IsAreaManager = fsobo.IsAreaManager,
                IsActive = fsobo.IsActive,
                IsActiveStatus = fsobo.IsActive ? "checked" : "",
                SalesManagerStatus = fsobo.IsSalesManager ? "checked" : "",
                RegionalSalesManagerStatus = fsobo.IsRegionalSalesManager ? "checked" : "",
                RegionalSalesManagerID = fsobo.RegionalSalesManagerID,
                SalesManagerID = fsobo.SalesManagerID,
                SalesManager = fsobo.SalesManager,
                RegionalSalesManager = fsobo.RegionalSalesManager,
                IsSalesManager = fsobo.IsSalesManager,
                IsRegionalSalesManager = fsobo.IsRegionalSalesManager,
                ReportingToName = fsobo.ReportingToName,
                ReportingToID = fsobo.ReportingToID
            };
            return View(fso);
        }

        public JsonResult IsFSOExist(int ID)
        {
            try
            {
                bool IsExist = false;
                IsExist = fsoBL.IsFSOExist(ID);
                return Json(new { Status = "success", Data = IsExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFSOList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string SalesManagerHint = Datatable.Columns[4].Search.Value;
                string RegionalManagerHint = Datatable.Columns[5].Search.Value;
                string ZonalManagerHint = Datatable.Columns[6].Search.Value;
                string AreaManagerHint = Datatable.Columns[7].Search.Value;
                string RouteCodeHint = "";
                string RouteNameHint = Datatable.Columns[8].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = fsoBL.GetFSOList(CodeHint, NameHint, SalesManagerHint, RegionalManagerHint, ZonalManagerHint, AreaManagerHint, RouteCodeHint, RouteNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFSOAutoComplete(string Hint = "")
        {
            try
            {
                DatatableResultBO resultBO = fsoBL.GetFSOList("", Hint, "", "", "", "", "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCustomersByFilters(int StateID = 0, int DistrictID = 0, int CustomerCategoryID = 0, int FSOID = 0, int SalesIncentiveCategoryID = 0)
        {
            try
            {
                List<FSOIncentiveItemBO> Customerlist = fsoBL.GetCustomersByFilters(StateID, DistrictID, CustomerCategoryID, FSOID, SalesIncentiveCategoryID);
                return Json(new { Status = "success", Data = Customerlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCustomersByFSO(int ID)
        {
            try
            {
                List<FSOIncentiveItemBO> Customerlist = fsoBL.GetCustomersByFSO(ID);
                return Json(new { Status = "success", Data = Customerlist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InactiveConfirm(int ID)
        {
            try
            {
                bool result = fsoBL.InactiveConfirm(ID);
                return Json(new { Status = "success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Status = "failure",
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult GetFSOManagersList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string DesignationHint = Datatable.Columns[4].Search.Value;
                string RouteNameHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = fsoBL.GetFSOManagersList(CodeHint, NameHint, DesignationHint, RouteNameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFSOManagersAutoComplete(string Hint = "")
        {
            try
            {
                DatatableResultBO resultBO = fsoBL.GetFSOManagersList("", Hint, "", "", "Name", "ASC", 0, 20);
                return Json(resultBO.data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}