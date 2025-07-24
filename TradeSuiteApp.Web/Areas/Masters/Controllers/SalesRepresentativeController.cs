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
    public class SalesRepresentativeController : Controller
    {
        private ISalesRepresentativeContract salesRepresentativeBL;
        private IDesignationContract designationBL;
        private ICategoryContract categroyBL;

        public SalesRepresentativeController()
        {
            salesRepresentativeBL = new SalesRepresentativeBL();
            designationBL = new DesignationBL();
            categroyBL = new CategoryBL();
        }
        // GET: Masters/SalesRepresentative
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            SalesRepresentativeModel Model = new SalesRepresentativeModel();
            List<SalesRepresentativeBO> salesRepresentativeBO;
            SalesRepresentativeListModel salesRepresentativeList;
            Model.DesignationList = new SelectList(designationBL.GetDesignationList(), "ID", "Name");
            salesRepresentativeBO = salesRepresentativeBL.GetSalesRepresentatives();
            Model.SalesRepresentatives = new List<SalesRepresentativeListModel>();
            foreach (var m in salesRepresentativeBO)
            {
                salesRepresentativeList = new SalesRepresentativeListModel()
                {
                  FSOName=m.FSOName,
                  EmployeeID=m.EmployeeID,
                  Designation=m.Designation,
                  DesignationID=m.DesignationID,
                  Area=m.Area,
                  AreaID=m.AreaID,
                  ID=m.ID,
                  IsSubLevel=m.IsSubLevel,
                  ParentID=m.ParentID,
                  SalesIncentiveCategoryID=m.SalesIncentiveCategoryID,
                  IsChild=m.IsChild
                };
                Model.SalesRepresentatives.Add(salesRepresentativeList);
            }

            //Model.SalesRepresentatives = salesRepresentativeBL.GetSalesRepresentatives();
            Model.SalesCategoryList = new SelectList(categroyBL.GetSalesCategory(222), "ID", "Name");
            Model.AreaList = new SelectList(
                                         new List<SelectListItem>
                                         {
                                                new SelectListItem { Text = "Select", Value = "0"}
                                         }, "Value", "Text");
            return View(Model);
        }

        public JsonResult GetAreasByParentArea(int ParentAreaID)
        {
            try
            {
                List<SalesAreaBO> AreaList = salesRepresentativeBL.GetAreasByParentArea(ParentAreaID);
                return Json(new { Status = "success", Data = AreaList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetAreas(int AreaID)
        {
            try
            {
                List<SalesAreaBO> AreaList = salesRepresentativeBL.GetAreas(AreaID);
                return Json(new { Status = "success", Data = AreaList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Save(SalesRepresentativeModel model)
        {
            try
            {
                SalesRepresentativeBO salesRepresentativeBO = new SalesRepresentativeBO()
                {
                    ID = model.ID,
                    FSOName=model.FSOName,
                    AreaID=model.AreaID,
                    DesignationID=model.DesignationID,
                    IsSubLevel=model.IsSubLevel,
                    ParentID = model.ParentID,
                    EmployeeID=model.EmployeeID,
                    SalesIncentiveCategoryID = model.SalesIncentiveCategoryID
                };
                salesRepresentativeBL.Save(salesRepresentativeBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RemoveFSO(int ID)
        {
            try
            {
                bool IsRemovedItem = salesRepresentativeBL.RemoveFSO(ID);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult GetSalesRepresentativeList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string ParentNameHint = Datatable.Columns[3].Search.Value;
                string AreaHint = Datatable.Columns[4].Search.Value;
                string SalesIncentiveCategoryHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = salesRepresentativeBL.GetSalesRepresentativeList(CodeHint, NameHint, ParentNameHint, AreaHint, SalesIncentiveCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}