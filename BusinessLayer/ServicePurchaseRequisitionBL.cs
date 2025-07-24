using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class ServicePurchaseRequisitionBL : IServicePurchaseRequisition
    {
        ServicePurchaseRequisitionRepository ServicePRRepository;

        public ServicePurchaseRequisitionBL()
        {
            ServicePRRepository = new ServicePurchaseRequisitionRepository();
        }
        public DatatableResultBO GetPurchaseRequisitionList(string Type, string TransNoHint, string TransDateHint, string PurchaseOrderNumberHint, string CategoryNameHint, string ItemNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return ServicePRRepository.GetPurchaseRequisitionList(Type, TransNoHint, TransDateHint, PurchaseOrderNumberHint, CategoryNameHint, ItemNameHint, SortField, SortOrder, Offset, Limit);
        }

        public List<RequisitionBO> GetUnProcessedPurchaseRequisitionForService()
        {
            return ServicePRRepository.GetUnProcessedPurchaseRequisitionForService();
        }

        public RequisitionBO GetServicePurchaseRequisition(int ID)
        {
            return ServicePRRepository.PurchaseRequisitionDetailsForService(ID);
        }

        public List<RequisitionServiceItemBO> PurchaseRequisitionTransDetailsForService(int ID)
        {
            return ServicePRRepository.PurchaseRequisitionTransDetailsForService(ID);
        }

        public bool SavePurchaseRequisitionForService(RequisitionBO _masterPr, List<RequisitionServiceItemBO> _prdetails)
        {
            if (_masterPr.ID == 0)
            {
                return ServicePRRepository.SavePurchaseRequisitionForService(_masterPr, _prdetails);
            }
            else
            {
                return ServicePRRepository.UpdatePurchaseRequisitionForService(_masterPr, _prdetails);
            }
        }

        public bool UpdatePurchaseRequisitionForService(RequisitionBO _masterPr, List<RequisitionServiceItemBO> _prdetails)
        {
            return ServicePRRepository.UpdatePurchaseRequisitionForService(_masterPr, _prdetails);
        }

    }
}
