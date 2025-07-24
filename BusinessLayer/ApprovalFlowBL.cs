using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ApprovalFlowBL : IApprovalFlowContract
    {
        ApprovalFlowDAL approvalflowDAL;

        public ApprovalFlowBL()
        {   
            approvalflowDAL = new ApprovalFlowDAL();
        }

        public List<ApprovalFlowBO> GetApprovalFlows()
        {
            return approvalflowDAL.GetApprovalFlows();
        }

        public ApprovalFlowBO GetApprovalFlow(int ID)
        {
            return approvalflowDAL.GetApprovalFlow(ID);
        }

        public int Save(ApprovalFlowBO approvalFlowBO)
        {
            if (approvalFlowBO.ID == 0)
            {
                return approvalflowDAL.Save(approvalFlowBO);
            }
            else
            {
                return approvalflowDAL.Update(approvalFlowBO);
            }
        }

        public List<ApprovalFlowItemBO> GetApprovalFlow(int ForDepartmentID, int ItemCategoryID, int ItemAccountsCategoryID, int LocationID)
        {
            return approvalflowDAL.GetApprovalFlow(ForDepartmentID, ItemCategoryID, ItemAccountsCategoryID, LocationID);
        }

        public DatatableResultBO GetApprovalFlowList(string AppQueueName, string ForDepartmentName, string AmountAbove, string AmountBelow, string ItemCategoryName, string ItemAccountsCategoryName, string SuppliercategoryName, string SupplierAccountscategoryName, string SortField, string SortOrder, int Offset, int Limit)
        {
            return approvalflowDAL.GetApprovalFlowList(AppQueueName, ForDepartmentName, AmountAbove, AmountBelow, ItemCategoryName, ItemAccountsCategoryName, SuppliercategoryName, SupplierAccountscategoryName, SortField, SortOrder, Offset, Limit);
        }

        public List<ApprovalFlowBO> GetLocationList()
        {
            return approvalflowDAL.GetLocationList();
        }

    }
}

