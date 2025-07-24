using BusinessObject;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class UserBL : IUserContract
    {
        UserDAL userDAL;

        public UserBL()
        {
            userDAL = new UserDAL();
        }

        public List<UserBO> GetUserList()
        {
            return userDAL.GetUserList();
        }

        public bool AddUserLocation(string userID, string locationIDsStr)
        {
            return userDAL.AddUserLocation(userID, locationIDsStr);
        }

        public UserBO GetLoginDetails(int UserID, string Email)
        {
            return userDAL.GetLoginDetails(UserID, Email);
        }

        public int UpdateSessionLog(int UserID, int SessionID)
        {
           return userDAL.UpdateSessionLog(UserID, SessionID);
        }
    }
}
