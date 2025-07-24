using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IApprovalContract
    {
        bool HasApprovalProcess(string Area, string Controller, string Action);

        void InitiateApprovalRequest(string Area, string Controller, string Action, int ID,int ApprovalFlowID);

        List<ApprovalProcessBO> GetApprovalProcess(string Area, string Controller, string Action, int ID);

        List<ApprovalBO> GetApprovalData(string Area, string Controller, string Action, int ID);

        int DoApprovalAction(int ApprovalID, int UserID, string ApprovalComment,string  Status, int ClarificationUserID);

        DatatableResultBO GetApprovalList(string Type, string CodeHint, string DateHint, string TypeHint, string SupplierNameHint, string FlowHint, string NetAmountHint, string UserNameHint, string LastActionByHint, string NextActionByHint, string StatusHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
