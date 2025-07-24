using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Approvals.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Approvals.Controllers
{
    public class ApprovalController : Controller
    {
        IApprovalContract approvalBL;
        IGeneralContract generalBL;
        public ApprovalController()
        {
            approvalBL = new ApprovalBL();
            generalBL = new GeneralBL();
        }
        // GET: Approval
        public ActionResult Index()
        {
            ViewBag.Statuses = new List<string>() { "next" };
            return View();
        }
        
        public JsonResult GetApprovalList(DatatableModel Datatable)
        {
            try
            {
                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = approvalBL.GetApprovalList(Type,
                        Datatable.Columns[1].Search.Value,
                        Datatable.Columns[2].Search.Value,
                        Datatable.Columns[3].Search.Value,
                        Datatable.Columns[4].Search.Value,
                        Datatable.Columns[5].Search.Value,
                        Datatable.Columns[6].Search.Value,
                        Datatable.Columns[7].Search.Value,
                        Datatable.Columns[8].Search.Value,
                        Datatable.Columns[9].Search.Value,
                        Datatable.Columns[10].Search.Value,
                        Datatable.Columns[Datatable.Order[0].Column].Data,
                        Datatable.Order[0].Dir,
                        Datatable.Start,
                        Datatable.Length
                        );
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                generalBL.LogError("Approvals", "Approval", "GetApprovalList", 0, e);
                return Json(new { Status = "failure", data = res , Message = e.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        private ApprovalModel GetApprovalDetails(string Area, string Controller, string Action, int ID)
        {
            ApprovalModel Approval = approvalBL.GetApprovalData(Area, Controller, Action, ID)
                .Select(a => new ApprovalModel()
                {
                    ApprovalID = a.ID,
                    Name = a.Name,
                    TransID = a.TransID,
                    TransNo = a.TransNo,
                    ApprovalFlowID = a.ApprovalFlowID,
                    IsApproved = a.IsApproved,
                    Status = a.Status,
                    LastActionUserID = a.LastActionUserID,
                    NextActionUserID = a.NextActionUserID,
                    UserID = a.UserID,
                    CreatedDate = a.CreatedDate,
                    ApprovalTypeID = a.ApprovalTypeID,
                    UserName = a.UserName,
                    Requirement = a.Requirement,
                    LoggedInUserID = GeneralBO.CreatedUserID

                })
                .FirstOrDefault();

            List<ApprovalProcessModel> ApprovalProcess = approvalBL.GetApprovalProcess(Area, Controller, Action, ID)
                .Select(a => new ApprovalProcessModel()
                {
                    ApprovalTransID = a.ID,
                    ApprovalID = a.ApprovalID,
                    IsActive = a.IsActive,
                    Date = General.FormatDate(a.Date, "view"),
                    Time = a.Date.ToShortTimeString(),
                    UserID = a.UserID,
                    Status = a.Status,
                    Comment = a.Comment,
                    IsApproved = a.IsApproved,
                    SortOrder = a.SortOrder,
                    UserName = a.UserName,
                    NextActionUserID = a.NextActionUserID,
                    Requirement = a.Requirement,
                    IsActiveForUser = a.IsActiveForUser
                })
                .ToList();
            if (Approval != null)
            {
                Approval.History = ApprovalProcess.Where(Process => !Process.IsActive).ToList();
                Approval.Process = ApprovalProcess.Where(Process => Process.IsActive).ToList();
                Approval.UsersList = new SelectList(ApprovalProcess
                    .Select(x => new { ID = x.UserID, Name = x.UserName, Status = x.Status, IsActiveForUser = x.IsActiveForUser }).Distinct()
                    .Where(x => x.ID != GeneralBO.CreatedUserID && x.Status != "" && x.IsActiveForUser == true).ToList(), "ID", "Name");
            }
            else
            {
                Approval = new ApprovalModel();
                Approval.History = null;
                Approval.Process = null;
            }

            return Approval;
        }

        public JsonResult InitiateApprovalRequest( string Area, string Controller, string Action, int ID) {
            try
            {
                approvalBL.InitiateApprovalRequest(Area, Controller, Action, ID, 0);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Approvals", "Approval", "InitiateApprovalRequest", 0, e);
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public ActionResult Clarify(int ApprovalID, int UserID, string ApprovalComment, string Status, int ClarificationUserID, string Area, string Controller, string Action, int ID)
        {
            approvalBL.DoApprovalAction(ApprovalID, UserID, ApprovalComment, Status, ClarificationUserID);
            ApprovalModel Approval = GetApprovalDetails(Area, Controller, Action, ID);
            return View("GetApprovalProcessDetails", Approval);
        }

        [HttpPost]
        public ActionResult DoApprovalAction(int ApprovalID, int UserID, string ApprovalComment, string Status, int ClarificationUserID, string Area, string Controller, string Action, int ID)
        {
            approvalBL.DoApprovalAction(ApprovalID, UserID, ApprovalComment, Status, ClarificationUserID);
            ApprovalModel Approval = GetApprovalDetails(Area, Controller, Action, ID);
            return View("GetApprovalProcessDetails", Approval);
        }

        [HttpPost]
        public PartialViewResult GetApprovalProcessDetails(string Area, string Controller, string Action, int ID)
        {
            ApprovalModel Approval = GetApprovalDetails(Area, Controller, Action, ID);
            return PartialView(Approval);
        }
    }
}


