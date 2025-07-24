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
    public class UserLocationsBL : IUserLocationsContract
    {

        UserLocationsDAL userLocationsDAL;

        public UserLocationsBL()
        {
            userLocationsDAL = new UserLocationsDAL();
        }

        public DatatableResultBO GetUserLocationsList(string Code, string Name, string UserName, string DefaultLocation, string CurrentLocation, string OtherLocation, string SortField, string SortOrder, int Offset, int Limit)
        {
            return userLocationsDAL.GetUserLocationsList(Code, Name, UserName, DefaultLocation, CurrentLocation, OtherLocation, SortField, SortOrder, Offset, Limit);
        }
    }
}

