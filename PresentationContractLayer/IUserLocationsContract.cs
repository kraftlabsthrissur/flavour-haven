using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IUserLocationsContract
    {
        DatatableResultBO GetUserLocationsList(string Code,string Name,string UserName,string DefaultLocation,string CurrentLocation,string OtherLocation,string SortField,string SortOrder,int Offset,int Limit);
    }
}
