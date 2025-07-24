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
    public class AdvanceReturnBL : IAdvanceReturnContract
    {
        AdvanceReturnDAL advanceReturnDAL;
        public AdvanceReturnBL()
        {
            advanceReturnDAL = new AdvanceReturnDAL();
        }
        public bool Save(AdvanceReturnBO advanceReturn, List<AdvanceReturnTransBO> advancereturntrans)
        {
            if (advanceReturn.ID == 0)
            {
                return advanceReturnDAL.Save(advanceReturn, advancereturntrans);
            }
            else
            {
                return advanceReturnDAL.Update(advanceReturn, advancereturntrans);
            }
        }
        public List<AdvanceReturnBO> GetAdvanceReturnList()
        {
            return advanceReturnDAL.GetAdvanceReturnList();
        }

        public List<AdvanceReturnBO> GetAdvanceReturnDetails(int AdvaneReturnID)
        {
            return advanceReturnDAL.GetAdvanceReturnDetails(AdvaneReturnID);
        }

        public List<AdvanceReturnTransBO> GetAdvanceReturnTransDetails(int AdvaneReturnID)
        {
            return advanceReturnDAL.GetAdvanceReturnTransDetails(AdvaneReturnID);
        }

        public DatatableResultBO GetAdvanceReturnList(string Type, string ARNoHint, string ARDateHint, string NameHint, string CategoryHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return advanceReturnDAL.GetAdvanceReturnList(Type, ARNoHint, ARDateHint, NameHint, CategoryHint, NetAmountHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
