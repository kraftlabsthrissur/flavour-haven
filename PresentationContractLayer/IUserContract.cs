using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IUserContract
    {
        List<UserBO> GetUserList();

        bool AddUserLocation(string userID, string locationIDsStr);

        UserBO GetLoginDetails(int UserID, string Email);

        int UpdateSessionLog(int UserID, int SessionID);
    }
}
