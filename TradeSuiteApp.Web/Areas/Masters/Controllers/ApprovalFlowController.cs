using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class ApprovalFlowController : Controller
    {
        IApprovalFlowContract approvalflowBL;
        private ILocationContract locationBL;
        private IApprovalQueueContract approvalBL;
        private ICategoryContract categoryBL;
        private ISupplierContract supplierBL;
        private IDepartmentContract departmentBL;

        public ApprovalFlowController()
        {
            approvalflowBL = new ApprovalFlowBL();
            locationBL = new LocationBL();
            approvalBL = new ApprovalQueueBL();
            categoryBL = new CategoryBL();
            supplierBL = new SupplierBL();
            departmentBL = new DepartmentBL();
        }

        // GET: Masters/ApprovalFlow
        public ActionResult Index()
        {  
            return View();
        }

        // GET: Masters/ApprovalFlow/Details/5
        public ActionResult Details(int id)
        {
            ApprovalFlowBO ApprovalFlow = approvalflowBL.GetApprovalFlow(id);
            ApprovalFlowModel ApprovalFlowModel = new ApprovalFlowModel
            {
                ID = ApprovalFlow.ID,
                LocationCode = ApprovalFlow.LocationCode,
                ApprovalType = ApprovalFlow.ApprovalType,
                ForDepartmentID = ApprovalFlow.ForDepartmentID,
                ForDepartmentName = ApprovalFlow.ForDepartmentName,
                AmountAbove = ApprovalFlow.AmountAbove,
                AmountBelow = ApprovalFlow.AmountBelow,
                ItemCategoryID = ApprovalFlow.ItemCategoryID,
                ItemCategoryName = ApprovalFlow.ItemCategoryName,
                ItemAccountsCategoryID = ApprovalFlow.ItemAccountsCategoryID,
                ItemAccountsCategoryName = ApprovalFlow.ItemAccountsCategoryName,
                SupplierCategoryID = ApprovalFlow.SupplierCategoryID,
                SuppliercategoryName = ApprovalFlow.SuppliercategoryName,
                SupplierAccountsCategoryID = ApprovalFlow.SupplierAccountsCategoryID,
                SupplierAccountscategoryName = ApprovalFlow.SupplierAccountscategoryName,
                AppQueueName = ApprovalFlow.AppQueueName,
                LocationName = ApprovalFlow.LocationName
            };

            return View(ApprovalFlowModel);

        }

        // GET: Masters/ApprovalFlow/Create
        public ActionResult Create()
        {
            ApprovalFlowModel ApprovalFlowModel = new ApprovalFlowModel();

            ApprovalFlowModel.LocationList = new SelectList(approvalflowBL.GetLocationList(), "UserLocationID", "LocationName");
            ApprovalFlowModel.ApprovalList = new SelectList(approvalBL.GetApprovalQueues(), "AppQueueID", "QueueName");
            ApprovalFlowModel.ItemCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(1), "ID", "Name");
            ApprovalFlowModel.ItemAccountsCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(11), "ID", "Name");
            ApprovalFlowModel.SuppliercategoryList = new SelectList(categoryBL.GetSuppliersCategoryList(), "ID", "Name");
            ApprovalFlowModel.SupplierAccountscategoryList = new SelectList(categoryBL.GetSuppliersAccountCategoryGroup(), "ID", "Name");
            ApprovalFlowModel.DeptList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name");

            return View(ApprovalFlowModel);
        }

        public ActionResult Save(ApprovalFlowModel model)
        {
            try
            {
                ApprovalFlowBO approvalFlowBO = new ApprovalFlowBO()
                {
                    ID = model.ID,
                    ForDepartmentID = model.ForDepartmentID,
                    UserLocationID = model.UserLocationID,
                    LocationName = model.LocationName,
                    AmountAbove = model.AmountAbove,
                    AmountBelow = model.AmountBelow,
                    ItemCategoryID = model.ItemCategoryID,
                    ItemAccountsCategoryID = model.ItemAccountsCategoryID,
                    ApprovalQueueID = model.ApprovalQueueID,
                    SupplierCategoryID = model.SupplierCategoryID,
                    SupplierAccountsCategoryID = model.SupplierAccountsCategoryID

                };
                approvalflowBL.Save(approvalFlowBO);
                return Json(new { Status = "success", Data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {

            ApprovalFlowBO ApprovalFlow = approvalflowBL.GetApprovalFlow(id);
            ApprovalFlowModel ApprovalFlowModel = new ApprovalFlowModel
            {
                ID = ApprovalFlow.ID,
                LocationCode = ApprovalFlow.LocationCode,
                ApprovalType = ApprovalFlow.ApprovalType,
                ForDepartmentID = ApprovalFlow.ForDepartmentID,
                ForDepartmentName = ApprovalFlow.ForDepartmentName,
                AmountAbove = ApprovalFlow.AmountAbove,
                AmountBelow = ApprovalFlow.AmountBelow,
                ItemCategoryID = ApprovalFlow.ItemCategoryID,
                ItemCategoryName = ApprovalFlow.ItemCategoryName,
                ItemAccountsCategoryID = ApprovalFlow.ItemAccountsCategoryID,
                ItemAccountsCategoryName = ApprovalFlow.ItemAccountsCategoryName,
                SupplierCategoryID = ApprovalFlow.SupplierCategoryID,
                SuppliercategoryName = ApprovalFlow.SuppliercategoryName,
                SupplierAccountsCategoryID = ApprovalFlow.SupplierAccountsCategoryID,
                SupplierAccountscategoryName = ApprovalFlow.SupplierAccountscategoryName,
                AppQueueName = ApprovalFlow.AppQueueName,
                UserLocationID = ApprovalFlow.UserLocationID,
                ApprovalQueueID = ApprovalFlow.ApprovalQueueID,
                DeptList = new SelectList(departmentBL.GetDepartmentList(), "ID", "Name"),
                ApprovalList = new SelectList(approvalBL.GetApprovalQueues(), "AppQueueID", "QueueName"),
                ItemCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(1), "ID", "Name"),
                ItemAccountsCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(11), "ID", "Name"),
                SuppliercategoryList = new SelectList(categoryBL.GetSuppliersCategoryList(), "ID", "Name"),
                SupplierAccountscategoryList = new SelectList(categoryBL.GetSuppliersAccountCategoryGroup(), "ID", "Name"),
                LocationList = new SelectList(approvalflowBL.GetLocationList(), "UserLocationID", "LocationName"),

            };
            return View(ApprovalFlowModel);

        }

        public JsonResult GetApprovalFlow(int ForDepartmentID, int ItemCategoryID, int ItemAccountsCategoryID, int UserLocationID)
        {
            try
            {
                List<ApprovalFlowItemBO> Items = approvalflowBL.GetApprovalFlow(ForDepartmentID, ItemCategoryID, ItemAccountsCategoryID, UserLocationID);

                return Json(new { Status = "success", Data = Items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetApprovalFlowList(DatatableModel Datatable)
        {
            try
            {
                string AppQueueName = Datatable.Columns[1].Search.Value;
                string ForDepartmentName = Datatable.Columns[2].Search.Value;
                string AmountAbove = Datatable.Columns[3].Search.Value;
                string AmountBelow = Datatable.Columns[4].Search.Value;
                string ItemCategoryName = Datatable.Columns[5].Search.Value;
                string ItemAccountsCategoryName = Datatable.Columns[6].Search.Value;
                string SuppliercategoryName = Datatable.Columns[7].Search.Value;
                string SupplierAccountscategoryName = Datatable.Columns[8].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = approvalflowBL.GetApprovalFlowList(AppQueueName, ForDepartmentName, AmountAbove, AmountBelow, ItemCategoryName, ItemAccountsCategoryName, SuppliercategoryName, SupplierAccountscategoryName, SortField, SortOrder, Offset, Limit);
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
