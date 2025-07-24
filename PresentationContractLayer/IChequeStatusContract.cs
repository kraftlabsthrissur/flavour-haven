using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IChequeStatusContract
    {
        List<ChequeStatusTransBO> getChequeStatus(string InstrumentStatus,DateTime FromTransactionNumber, DateTime ToTransactionNumber);
        string Save(ChequeStatusBO Master, List<ChequeStatusTransBO> Details);
        List<ChequeStatusBO> getChequeStatusList();
        List<ChequeStatusBO> getChequeStatusDetails(int ChequeStatusID);
        List<ChequeStatusTransBO> getChequeStatusTransDetails(int ChequeStatusID);
        bool Update (ChequeStatusBO Master, List<ChequeStatusTransBO> Details);

        DatatableResultBO GetChequeStatusList(string Type,string StatusNoHint,string TransDateHint,string InstrumentStatusHint,string FromDateHint,string ToDateHint,string CustomerNameHint,string SortField,string SortOrder,int Offset,int Limit);
    }
}
