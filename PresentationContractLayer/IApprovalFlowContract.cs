using BusinessObject;
using System.Collections.Generic;

namespace PresentationContractLayer
{
    public interface IApprovalFlowContract
    {
        List<ApprovalFlowBO> GetApprovalFlows();

        ApprovalFlowBO GetApprovalFlow(int ID);

        int Save(ApprovalFlowBO approvalFlowBO);

        List<ApprovalFlowItemBO> GetApprovalFlow(int ForDepartmentID, int ItemCategoryID, int ItemAccountsCategoryID, int UserLocationID);

        DatatableResultBO GetApprovalFlowList(string AppQueueName,string ForDepartmentName,string AmountAbove,string AmountBelow,string ItemCategoryName,string ItemAccountsCategoryName,string SuppliercategoryName,string SupplierAccountscategoryName,string SortField,string SortOrder,int Offset,int Limit);

        List<ApprovalFlowBO> GetLocationList();
    }


}
