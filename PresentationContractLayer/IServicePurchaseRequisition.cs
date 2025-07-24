using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace PresentationContractLayer
{
    public interface IServicePurchaseRequisition
    {
        bool SavePurchaseRequisitionForService(RequisitionBO _masterPr, List<RequisitionServiceItemBO> _prdetails);

        RequisitionBO GetServicePurchaseRequisition(int ID);

        List<RequisitionServiceItemBO> PurchaseRequisitionTransDetailsForService(int ID);

        bool UpdatePurchaseRequisitionForService(RequisitionBO _masterPr, List<RequisitionServiceItemBO> _prdetails);

        List<RequisitionBO> GetUnProcessedPurchaseRequisitionForService();
        DatatableResultBO GetPurchaseRequisitionList(string Type, string TransNoHint, string TransDateHint, string PurchaseOrderNumberHint, string CategoryNameHint, string ItemNameHint, string SortField, string SortOrder, int Offset, int Limit);

    }
}
