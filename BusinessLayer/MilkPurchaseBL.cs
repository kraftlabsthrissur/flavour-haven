using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class MilkPurchaseBL : IMilkPurchase
    {
        MilkPurchaseRepository MilkPurchaseDAL;

        public MilkPurchaseBL()
        {
            MilkPurchaseDAL = new MilkPurchaseRepository();
        }

        public List<MilkPurchaseBO> GetAllMilkPurchase(int ID)
        {
            return MilkPurchaseDAL.GetAllMilkPurchase(ID);
        }

        public List<MilkPurchseTransBO> GetAllMilkPurchaseTrans(int ID)
        {
            return MilkPurchaseDAL.GetAllMilkPurchaseTrans(ID);
        }

        public List<MilkFatRangeBO> GetMilkFatRange(string Hint)
        {
            return MilkPurchaseDAL.GetMilkFatRange(Hint);
        }

        public MilkFatRangeBO GetMilkFatRangePrice(int ID)
        {
            return MilkPurchaseDAL.GetMilkFatRangePrice(ID);
        }

        public List<MilkPurchaseRequisitionBO> GetMilkPurchaseRqusition(DateTime Date)
        {
            return MilkPurchaseDAL.GetMilkPurchaseRqusition(Date);
        }

        public List<MilkSupplierBO> GetMilkSupplierList(string Hint)
        {
            return MilkPurchaseDAL.GetMilkSupplierList(Hint);
        }

        public bool SaveMilkPurchase(MilkPurchaseBO _milkMaster, List<MilkPurchseTransBO> _milkTrans)
        {
            if (_milkMaster.ID == 0)
            {
                return MilkPurchaseDAL.SaveMilkPurchase(_milkMaster, _milkTrans);
            }
            else
            {
                return MilkPurchaseDAL.UpdateMilkPurchase(_milkMaster, _milkTrans);
            }

        }

        public DatatableResultBO GetMilkPurchaseList(string Type, string TransNoHint, string TransDateHint, string SupplierNameHint, string QuantityHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return MilkPurchaseDAL.GetMilkPurchaseList(Type, TransNoHint, TransDateHint, SupplierNameHint, QuantityHint, AmountHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
