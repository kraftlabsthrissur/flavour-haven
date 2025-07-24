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
    public class ChequeStatusBL : IChequeStatusContract
    {
        ChequeStatusDAL statusDAL;
        public  ChequeStatusBL()
        {
            statusDAL = new ChequeStatusDAL();
        }
        public List<ChequeStatusTransBO> getChequeStatus(string InstrumentStatus,DateTime fromReciptDate, DateTime ToReciptDate)
        {
            return statusDAL.getChequeStatus(InstrumentStatus,fromReciptDate, ToReciptDate);
        }
        public string Save(ChequeStatusBO Master, List<ChequeStatusTransBO> Details)
        {
            return statusDAL.Save(Master, Details);
        }
        public List<ChequeStatusBO> getChequeStatusList()
        {
            return statusDAL.getChequeStatusList();
        }
        public List<ChequeStatusBO> getChequeStatusDetails(int ChequeStatusID)
        {
            return statusDAL.getChequeStatusDetails(ChequeStatusID);
        }
        public List<ChequeStatusTransBO> getChequeStatusTransDetails(int ChequeStatusID)
        {
            return statusDAL.getChequeStatusTransDetails(ChequeStatusID);
        }
        public bool Update(ChequeStatusBO Master, List<ChequeStatusTransBO> Details)
        {
            return statusDAL.Update(Master, Details);
        }

        public DatatableResultBO GetChequeStatusList(string Type, string StatusNoHint, string TransDateHint, string InstrumentStatusHint, string FromDateHint, string ToDateHint, string CustomerNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return statusDAL.GetChequeStatusList(Type, StatusNoHint, TransDateHint, InstrumentStatusHint, FromDateHint, ToDateHint, CustomerNameHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
