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
    public class TransportPermitBL : ITransportPermitContract
    {

        TransportPermitDAL transportPermiteDAL;

        public TransportPermitBL()
        {
            transportPermiteDAL = new TransportPermitDAL();
        }
        public List<TransportPermitItemBO> PendingPermitList(TransportPermitBO permitBO)
        {
            return transportPermiteDAL.PendingPermitList(permitBO);
        }
        public int SaveTransportPermit(TransportPermitBO permitBO, List<TransportPermitItemBO> itemBO)
        {
            return transportPermiteDAL.SaveTransportPermit(permitBO, itemBO);
        }
        public List<TransportPermitBO> GetTransportPermitList()
        {
            return transportPermiteDAL.GetTransportPermitList();
        }
        public List<TransportPermitItemBO> GetTransportPermitTrans(int ID)
        {
            return transportPermiteDAL.GetTransportPermitTrans(ID);
        }
        public List<TransportPermitBO> GetTransportPermit(int ID)
        {
            return transportPermiteDAL.GetTransportPermit(ID);
        }
        public DatatableResultBO GetTransportPermitListForDataTable(string TransNo, string ValidFromdate, string ValidTodate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return transportPermiteDAL.GetTransportPermitListForDataTable(TransNo, ValidFromdate, ValidTodate, SortField, SortOrder, Offset, Limit);
        }
    }
}
