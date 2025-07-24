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
    public class UserRolesBL : IUserRolesContract
    {
        UserRolesDAL userRolesDAL;

        public UserRolesBL()
        {
            userRolesDAL = new UserRolesDAL();
        }

        public DatatableResultBO GetRolesList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return userRolesDAL.GetRolesList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
        }

        public List<UserRolesBO> GetUserRoles()
        {
            return userRolesDAL.GetUserRoles();
        }

        public int Save(List<UserRolesBO> Items)
        {
                return userRolesDAL.Save(Items);
        }

        public bool IsUserhaveRoles(int UserID)
        {
            return userRolesDAL.IsUserhaveRoles(UserID);
        }

        public List<UserRolesBO> GetUserRolesDetails(int ID)
        {
            return userRolesDAL.GetUserRolesDetails(ID);
        }

    }
}
