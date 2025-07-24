using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IAdvanceRequestContract
    {
        List<AdvanceRequestBO> GetAdvanceRequestList(int ID);

        List<AdvanceRequestTransBO> GetAdvanceRequesTrans(int ID);

        string SaveAdvanceRequest(AdvanceRequestBO advanceRequestBO,List< AdvanceRequestTransBO> advanceRequestTransBO);

        DatatableResultBO GetAdvanceRequestListForDataTable(string Type,string AdvanceRequestNo, string AdvanceRequestDate, string EmployeeName, string Amount,string SortField, string SortOrder, int Offset, int Limit);
        int Suspend(int ID);
    }

}
