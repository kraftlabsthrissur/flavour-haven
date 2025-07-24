using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace BusinessLayer
{
    public class ApprovalBL : IApprovalContract
    {
        ApprovalDAL approvalDAL;

        public ApprovalBL()
        {
            approvalDAL = new ApprovalDAL();
        }

        public int DoApprovalAction(int ApprovalID, int UserID, string ApprovalComment, string Status, int ClarificationUserID)
        {
            return approvalDAL.DoApprovalAction(ApprovalID, UserID, ApprovalComment, Status, ClarificationUserID);
        }

        public List<ApprovalBO> GetApprovalData(string Area, string Controller, string Action, int ID)
        {
            return approvalDAL.GetApprovalData(Area, Controller, Action, ID);
        }

        public DatatableResultBO GetApprovalList(string Type, string CodeHint, string DateHint, string TypeHint, string SupplierNameHint, string FlowHint, string NetAmountHint, string UserNameHint, string LastActionByHint, string NextActionByHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return approvalDAL.GetApprovalList(Type, CodeHint, DateHint, TypeHint, SupplierNameHint, FlowHint, NetAmountHint, UserNameHint, LastActionByHint, NextActionByHint, StatusHint, SortField, SortOrder, Offset, Limit);
        }

        public List<ApprovalProcessBO> GetApprovalProcess(string Area, string Controller, string Action, int ID)
        {
            return approvalDAL.GetApprovalProcess(Area, Controller, Action, ID);
        }

        public bool HasApprovalProcess(string Area, string Controller, string Action)
        {
            return approvalDAL.HasApprovalProcess(Area, Controller, Action);
        }

        public void InitiateApprovalRequest(string Area, string Controller, string Action, int ID, int ApprovalFlowID)
        {
            approvalDAL.InitiateApprovalRequest(Area, Controller, Action, ID, ApprovalFlowID);
            return;
        }
    }
}
