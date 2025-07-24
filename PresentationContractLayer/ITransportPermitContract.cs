using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
   public interface ITransportPermitContract
    {
        List<TransportPermitItemBO> PendingPermitList(TransportPermitBO permitBO);
        int SaveTransportPermit(TransportPermitBO permitBO, List<TransportPermitItemBO> itemBO);
        List<TransportPermitBO> GetTransportPermitList();
        List<TransportPermitItemBO> GetTransportPermitTrans(int ID);
        List<TransportPermitBO> GetTransportPermit(int ID);
        DatatableResultBO GetTransportPermitListForDataTable(string TransNo, string ValidFromdate, string ValidTodate, string SortField, string SortOrder, int Offset, int Limit);
    }
}
