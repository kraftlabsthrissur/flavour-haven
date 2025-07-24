using BusinessLayer;
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
    public class AdvanceRequestBL:IAdvanceRequestContract
    {
        AdvanceRequestDAL advanceRequestDAL;

        public AdvanceRequestBL()
        {
            advanceRequestDAL = new AdvanceRequestDAL();
        }

        public List<AdvanceRequestBO> GetAdvanceRequestList(int ID)
        {
            return advanceRequestDAL.GetAdvanceRequestList(ID);
        }
        
        public List<AdvanceRequestTransBO> GetAdvanceRequesTrans(int ID)
        {
            return advanceRequestDAL.GetAdvanceRequesTrans(ID);
        }
        public string SaveAdvanceRequest(AdvanceRequestBO advanceRequestBO,List<AdvanceRequestTransBO> advanceRequestTransBO)
        {
            return advanceRequestDAL.SaveAdvanceRequest(advanceRequestBO, advanceRequestTransBO);
        }
        public DatatableResultBO GetAdvanceRequestListForDataTable(string Type,string AdvanceRequestNo, string AdvanceRequestDate, string EmployeeName, string Amount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return advanceRequestDAL.GetAdvanceRequestListForDataTable(Type,AdvanceRequestNo, AdvanceRequestDate, EmployeeName, Amount, SortField, SortOrder, Offset, Limit);
        }
        public int Suspend(int ID)
        {
            return advanceRequestDAL.Suspend(ID);
        }
    }
}
