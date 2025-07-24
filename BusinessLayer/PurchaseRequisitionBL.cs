using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System.Collections.Generic;
using System;

namespace BusinessLayer
{
    public class PurchaseRequisitionBL : IPurchaseRequisition
    {
        PurchaseRequisitionRepository purchaseRequisitionDAL;

        public PurchaseRequisitionBL()
        {
            purchaseRequisitionDAL = new PurchaseRequisitionRepository();
        }
        public DatatableResultBO GetPurchaseRequisitionListForPurchaseOrder(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string CategoryNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return purchaseRequisitionDAL.GetPurchaseRequisitionListForPurchaseOrder(Type, TransNoHint, TransDateHint, SupplierNameHint, CategoryNameHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetPurchaseRequisitionList(string Type, string TransNoHint, string TransDateHint, string FromDepartmentHint, string ToDepartmentHint, string CategoryNameHint, string ItemNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return purchaseRequisitionDAL.GetPurchaseRequisitionList(Type, TransNoHint, TransDateHint, FromDepartmentHint, ToDepartmentHint, CategoryNameHint, ItemNameHint, SortField, SortOrder, Offset, Limit);
        }

        public int IsPurchaseRequisitionEditable(int PurchaseRequisitionID)
        {
            return purchaseRequisitionDAL.IsPurchaseRequisitionEditable(PurchaseRequisitionID);
        }

        public List<RequisitionBO> PurchaseRequisitionDetails(int ID)
        {
            return purchaseRequisitionDAL.PurchaseRequisitionDetails(ID);
        }

        public List<ItemBO> PurchaseRequisitionTransDetails(int ID)
        {
            return purchaseRequisitionDAL.PurchaseRequisitionTransDetails(ID);
        }

        public bool SavePurchaseRequisition(RequisitionBO _masterPr, List<ItemBO> _prdetails)
        {
            return purchaseRequisitionDAL.SavePurchaseRequisition(_masterPr, _prdetails);
        }

        public bool UpdatePurchaseRequisition(RequisitionBO _masterPr, List<ItemBO> _prdetails)
        {
            return purchaseRequisitionDAL.UpdatePurchaseRequisition(_masterPr, _prdetails);
        }
    }
}
