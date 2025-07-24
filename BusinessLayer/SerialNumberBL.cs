using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessLayer;
using DataAccessLayer;
using BusinessObject;

namespace BusinessLayer
{
    public class SerialNumberBL :ISerialNo
    {
        SerialNumberDAL serialNumberDAL;
        public SerialNumberBL()
        {
            serialNumberDAL = new SerialNumberDAL();
        }

        public int CreateSerialNumber(SerialNumberBO serialNumber)
        {
            return serialNumberDAL.CreateSerialNumber(serialNumber);
        }
        public DatatableResultBO GetSerialNumberList(string FormHint, string PrefixHint, string LocationPrefixHint, string FinYearPrefixHint, string SortField, string SortOrder, int Offset, int Limit)
        {
           return serialNumberDAL.GetSerialNumberList( FormHint, PrefixHint, LocationPrefixHint, FinYearPrefixHint, SortField, SortOrder, Offset, Limit);
        }
    
        public bool UpdateFinYearAndFinPrefix(List<SerialNumberBO> serialBO)
        {
            return serialNumberDAL.UpdateFinYearAndFinPrefix( serialBO);
        }
      public  DatatableResultBO GetSerialNumberByFinYear(int NewFinYear, string FormHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return serialNumberDAL.GetSerialNumberByFinYear(NewFinYear, FormHint, LocationHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
