using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPurchaseRequisition
    {
        bool SavePurchaseRequisition(RequisitionBO _masterPr, List<ItemBO> _prdetails);

        List<RequisitionBO> PurchaseRequisitionDetails(int ID);

        List<ItemBO> PurchaseRequisitionTransDetails(int ID);

        bool UpdatePurchaseRequisition(RequisitionBO _masterPr, List<ItemBO> _prdetails);

        int IsPurchaseRequisitionEditable(int PurchaseRequisitionID);
        DatatableResultBO GetPurchaseRequisitionListForPurchaseOrder(string Type, string TransNoHint, string TransDateHint, string PurchaseOrderNumberHint, string CategoryNameHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetPurchaseRequisitionList(string Type, string TransNoHint, string TransDateHint, string FromDepartmentHint, string ToDepartmentHint, string CategoryNameHint, string ItemNameHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
